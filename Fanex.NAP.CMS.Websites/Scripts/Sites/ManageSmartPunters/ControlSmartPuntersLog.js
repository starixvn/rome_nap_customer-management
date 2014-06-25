/* File Created: June 20, 2014 */
/* global Namespace, Fanex, CMS, Layout, Validator, labels, datepickerSetting */

'use strict';

Namespace.Register("Fanex.CMS.ManageSmartPunters.ControlSmartPuntersLog");
Fanex.CMS.ManageSmartPunters.ControlSmartPuntersLog = function () {
    var $this = this;
    var urls = {
        GetSmartPunterLogs: CMS.resolveRootUrl('ManageSmartPunters/GetSmartPunterLogs')
    };
    var parameters = {
        Username: null,
        FromDate: null,
        ToDate: null,
        PageIndex: null,
        PageSize: null
    };
    var domElements = {
        divLogForm: null,
        divDatepickerPlugin: null,
        dpkFromDate: null,
        dpkToDate: null,
        txtLogUsername: null,
        btnLogSearch: null,
        divSmartPunterLogList: null,
        divDetailTemplate: null
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divLogForm = $('#divLogForm');
        domElements.divDatepickerPlugin = $('#divDatepickerPlugin');
        domElements.txtLogUsername = $('#txtLogUsername');
        domElements.btnLogSearch = $('#btnLogSearch');
        domElements.divSmartPunterLogList = $('#divSmartPunterLogList');
        domElements.divDetailTemplate = $('#divDetailTemplate').html();
    };

    // Initialize fields and handle events.
    var initFields = function () {
        parameters.PageSize = CMS.pageSize;

        Layout.createDateRange(
            domElements.divDatepickerPlugin,
            labels.FromDate,
            null,
            null,
            labels.ToDate,
            null,
            null,
            datepickerSetting.Today,
            datepickerSetting.MinDate,
            datepickerSetting.Today);
        domElements.dpkFromDate = domElements.divLogForm.find('#dpkFromDate');
        domElements.dpkToDate = domElements.divLogForm.find('#dpkToDate');

        domElements.txtLogUsername.keyup(function (event) {
            if (event.keyCode === 13) {
                domElements.btnLogSearch.click();
            }
        });
        domElements.txtLogUsername.focus();

        domElements.btnLogSearch.click(function () {
            var isValid = Validator.validateRequiredField(domElements.txtLogUsername, 'n');

            if (isValid) {
                parameters.Username = $.trim(domElements.txtLogUsername.val());
                parameters.FromDate = domElements.dpkFromDate.val();
                parameters.ToDate = domElements.dpkToDate.val();
                parameters.PageIndex = 0;

                domElements.txtLogUsername.val(parameters.Username);
                getSmartPuntersLogList();
            }
        });
    };

    // Get smart punter log list.
    var getSmartPuntersLogList = function () {
        CMS.closeMsgBox();
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSmartPunterLogs,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    Validator.showError(domElements.txtLogUsername, result.ErrorMsg, 'n');
                } else {
                    renderSmartPunterLogList(result);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Render smart punter logs.
    var renderSmartPunterLogList = function (data) {
        var editColumn = '<span class="edited-by">{0}</span><span class="edited-by">({1})</span>';
        var actionColumn = '<span class="action">{0}</span><span class="action">({1})</span>';

        var bodyData = [];
        var columns = [];
        var gridOptions = {
            showDistinctRow: false,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex,
                //totalItem: data.TotalItems,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 200
        };

        columns = [{ HeaderText: labels.LogID, Width: 80, Name: 'ColLogID', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.LogDate, Width: 80, Name: 'ColLogDate', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.EditedBy, Width: 150, Name: 'ColEditedBy', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.Action, Width: 250, Name: 'ColAction', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.Details, Width: 650, Name: 'ColDetails', HeaderAlign: 'center', CellAlign: 'left'}];

        if (data && data.Logs.length > 0) {
            $.each(data.Logs, function (index, item) {
                var row = {
                    Columns: [{ Name: 'ColLogID', Value: item.LogID },
                              { Name: 'ColLogDate', Value: item.LogDate },
                              { Name: 'ColEditedBy', Value: editColumn.format(item.EditedBy, item.UserID) },
                              { Name: 'ColAction', Value: actionColumn.format(item.Action, item.SPName) },
                              { Name: 'ColDetails', Value: domElements.divDetailTemplate}]
                };

                bodyData.push(row);
            });
        }

        gridOptions.columns = columns;
        gridOptions.bodyRows = bodyData;
        domElements.divSmartPunterLogList.removeClass('display-none').fanexGrid(gridOptions);
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        parameters.PageSize = itemsPerPage;
        parameters.PageIndex = pageIndex;
        getSmartPuntersLogList();
    };

    // Reset date pickers.
    var resetDatepikers = function () {
        domElements.dpkFromDate.datepicker('option', 'maxDate', datepickerSetting.Today);
        domElements.dpkFromDate.datepicker('option', 'minDate', datepickerSetting.MinDate);
        domElements.dpkFromDate.datepicker('setDate', datepickerSetting.Today);

        domElements.dpkToDate.datepicker('option', 'maxDate', datepickerSetting.Today);
        domElements.dpkToDate.datepicker('option', 'minDate', datepickerSetting.MinDate);
        domElements.dpkToDate.datepicker('setDate', datepickerSetting.Today);
    };

    $this.ResetForm = function () {
        resetDatepikers();
        domElements.txtLogUsername.val('');
    };

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};