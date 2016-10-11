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
        SortField: 'ColLogTime',
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
            columns: [{ HeaderText: labels.Order, Width: 40, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                        { HeaderText: labels.LogTime, Width: 150, Name: 'ColLogTime', HeaderAlign: 'center', Sort: true, SortDirection: 'desc' },
                        { HeaderText: labels.Admin, Width: 80, Name: 'ColAdmin', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.SmaOrSmartPunter, Width: 150, Name: 'ColUsername', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Level, Width: 50, Name: 'ColLevel', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Action, Width: 100, Name: 'ColActionName', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Function, Width: 250, Name: 'ColFunctionName', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.IP, Width: 90, Name: 'ColIP', HeaderAlign: 'center', Sort: true },
                        { HeaderText: labels.Restricted, Width: 100, Name: 'ColRestricted', HeaderAlign: 'center', CellAlign: 'center', Sort: true }
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

        var bodyRows = [];
        var row;

        $.each(data.ActionMonitoringLogs, function (index, item) {
            var restrictedHtml = item.IsRestricted
                ? '<input type="checkbox" id="107193" data-active="true" data-chk-group="chkStatus" checked disabled>'
                : '<input type="checkbox" id="107193" data-active="true" data-chk-group="chkStatus">';

            row = {
                Columns: [{ Name: 'ColOrder', Value: index },
                            { Name: 'ColLogTime', Value: item.LogTime },
                            { Name: 'ColAdmin', Value: item.Admin },
                            { Name: 'ColUsername', Value: item.Username },
                            { Name: 'ColLevel', Value: item.Level },
                            { Name: 'ColActionName', Value: item.ActionName },
                            { Name: 'ColFunctionName', Value: item.FunctionName },
                            { Name: 'ColIP', Value: item.IP },
                            { Name: 'ColRestricted', Value: restrictedHtml }

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
        //resetDatepikers();
        //domElements.txtLogUsername.val('').focus();
        //Validator.clearError(domElements.txtLogUsername);

        //// Reset cached parameters.
        //parameters.Username = null;
        //parameters.FromDate = null;
        //parameters.ToDate = null;
        //parameters.PageSize = CMS.pageSize;
        //parameters.PageIndex = 0;
    };

    $this.InitPage = function () {
        cacheDom();
        initFields();
    };
}