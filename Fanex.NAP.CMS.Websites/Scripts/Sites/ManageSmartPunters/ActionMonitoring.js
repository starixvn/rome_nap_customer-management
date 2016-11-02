/* File Created: June 4, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages, hasEditRight */
/* jshint strict: false */
// 'use strict';

Namespace.Register("Fanex.CMS.ManageSmartPunters.ActionMonitoring");
Fanex.CMS.ManageSmartPunters.ActionMonitoring = function () {
    var $this = this;

    var parameters = {
        FromDate: null,
        ToDate: null,
        Username: null,
        Admin: null,
        PageIndex: 0,
        PageSize: CMS.pageSize,
        SortField: 'LogDate',
        SortDirection: 'desc'
    };

    var domElements = {};

    // Cache DOM elements
    var cacheDom = function () {
        domElements.divActionMonitoringForm = $("#divActionMonitoringForm");
        domElements.divFromToDatePicker = $("#from-to-date-picker");
        domElements.txtUsername = $("#txtUserName");
        domElements.txtAdmin = $("#txtAdmin");
        domElements.btnSearch = $("#btnActionMonitoringSearch");
        domElements.txtFromDate = null;
        domElements.txtToDate = null;
        domElements.divActionMonitoringLogList = $("#divActionMonitoringLogList");

    };

    var renderActionMonitoringLogs = function (data) {
        var gridOptions = {
            columns: [{ HeaderText: labels.Order, Width: 40, Name: 'Order', HeaderAlign: 'center', CellAlign: 'center' },
                        { HeaderText: labels.LogTime, Width: 120, Name: 'LogDate', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Admin, Width: 80, Name: 'Admin', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.SmaOrSmartPunter, Width: 150, Name: 'Username', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Level, Width: 50, Name: 'RoleName', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Action, Width: 100, Name: 'ActionName', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Function, Width: 250, Name: 'FunctionName', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.IP, Width: 90, Name: 'IP', HeaderAlign: 'center', CellAlign: 'center', Sort: true },
                        { HeaderText: labels.Restricted, Width: 80, Name: 'IsRestricted', HeaderAlign: 'center', CellAlign: 'center', Sort: true }
            ],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: function (pageIndex, itemsPerPage) {
                    parameters.PageSize = itemsPerPage;
                    parameters.PageIndex = pageIndex;

                    getActionMonitoringLogs();
                }
            },
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    parameters.SortField = sortColumnName;
                    parameters.SortDirection = sortDirection;

                    getActionMonitoringLogs();
                }
            },
            gridExpandHeight: 200
        };

        for (var i = 0; i < gridOptions.columns.length; i++) {
            if (gridOptions.columns[i].Name === parameters.SortField) {
                gridOptions.columns[i].SortDirection = parameters.SortDirection;
            }
        }

        var bodyRows = [];
        var row;

        $.each(data.ActionMonitoringLogs, function (index, item) {
            var restrictedHtml = item.IsRestricted
                ? '<input type="checkbox" id="107193" data-active="true" data-chk-group="chkStatus" checked disabled>'
                : '<input type="checkbox" id="107193" data-active="true" data-chk-group="chkStatus" disabled>';

            row = {
                Columns: [{ Name: 'Order', Value: index },
                            { Name: 'LogDate', Value: item.LogDate },
                            { Name: 'Admin', Value: item.Admin },
                            { Name: 'Username', Value: item.Username },
                            { Name: 'RoleName', Value: item.RoleName },
                            { Name: 'ActionName', Value: item.ActionName },
                            { Name: 'FunctionName', Value: item.FunctionName },
                            { Name: 'IP', Value: item.IP },
                            { Name: 'IsRestricted', Value: restrictedHtml }

                ]
            };
            bodyRows.push(row);
        });

        gridOptions.pagerOption.totalItem = data.TotalItems;
        gridOptions.bodyRows = bodyRows;

        domElements.divActionMonitoringLogList.fanexGrid(gridOptions);


    };

    // Get smart punter log list.
    var getActionMonitoringLogs = function () {
        CMS.disableInput();

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('ManageSmartPunters/GetActionMonitoringLogs'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                renderActionMonitoringLogs(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Initialize fields and handle events.
    var initFields = function () {
        domElements.divFromToDatePicker.nexDatePicker({
            nexDatePickerType: 'daterange',
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: datepickerSetting.MinDate,
                maxDate: datepickerSetting.Today
            },
            firstDatePicker: {
                Id: 'dpk-fromdate',
                Label: 'Log Date',
                LabelCss: 'lbl-field col_5',
                TextboxCss: 'edit-field col_5'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: 'To',
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_5'
            }
        });

        domElements.txtFromDate = domElements.divActionMonitoringForm.find("#dpk-fromdate");
        domElements.txtToDate = domElements.divActionMonitoringForm.find("#dpk-todate");

        domElements.txtUsername.focus();

        domElements.btnSearch.click(function () {
            parameters.FromDate = domElements.txtFromDate.val();
            parameters.ToDate = domElements.txtToDate.val();
            parameters.Username = $.trim(domElements.txtUsername.val());
            parameters.Admin = $.trim(domElements.txtAdmin.val());

            // Reset page 1
            parameters.PageIndex = 0;

            getActionMonitoringLogs();
        });
    };

    $this.ResetForm = function () {
        domElements.txtFromDate.datepicker('option', 'minDate', datepickerSetting.MinDate);
        domElements.txtFromDate.datepicker('option', 'maxDate', datepickerSetting.Today);
        domElements.txtFromDate.datepicker('setDate', datepickerSetting.Today);

        domElements.txtToDate.datepicker('option', 'minDate', datepickerSetting.MinDate);
        domElements.txtToDate.datepicker('option', 'maxDate', datepickerSetting.Today);
        domElements.txtToDate.datepicker('setDate', datepickerSetting.Today);

        domElements.txtUsername.val('').focus();
        domElements.txtAdmin.val('');

        parameters = {
            FromDate: null,
            ToDate: null,
            Username: null,
            Admin: null,
            PageIndex: 0,
            PageSize: CMS.pageSize,
            SortField: 'LogDate',
            SortDirection: 'desc'
        };
    };

    $this.InitPage = function () {
        cacheDom();
        initFields();
    };
}