// 'use strict';

$(document).ready(function () {
    statusLog.init();
});

var statusLog = {
    $code: null,
    $divReport: null,

    pageSize: 100,
    pageIndex: 1,
    parameters: {
        Code: "",
        PageSize: 100,
        PageIndex: 1
    },

    init: function () {
        statusLog.cacheElements();
        statusLog.bindEvents();
    },
    cacheElements: function () {
        statusLog.$code = $("#txt-code");
        statusLog.$divReport = $("#div-report");
    },
    bindEvents: function () {
        $("#btn-generate").off("click").on('click', statusLog.generate);
    },
    generate: function () {
        statusLog.parameters = {
            Code: statusLog.$code.val(),
            PageSize: statusLog.pageSize,
            PageIndex: statusLog.pageIndex
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('StatusLog/Logs'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(statusLog.parameters),
            success: function (result) {
                statusLog.render(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    },
    render: function (result) {
        var columns = [
           { HeaderText: viewResources.LogID, Width: 100, Name: 'ColLogId', HeaderAlign: 'center', CellAlign: 'center' },
           { HeaderText: viewResources.LogDate, Width: 100, Name: 'ColLogDate', HeaderAlign: 'center', CellAlign: 'center' },
           { HeaderText: viewResources.EditedBy, Width: 200, Name: 'ColEditedBy', HeaderAlign: 'center', CellAlign: 'center' },
           { HeaderText: viewResources.Action, Width: 300, Name: 'ColAction', HeaderAlign: 'center', CellAlign: 'left' },
           { HeaderText: viewResources.Detail, Width: 200, Name: 'ColDetail', HeaderAlign: 'center', CellAlign: 'left' }
        ];

        var bodyRows = [];
        if (result) {
            for (var i = 0; i < result.Logs.length; i++) {
                var log = result.Logs[i];

                var detail = viewResources.Old + ": " + log.OldCollStatus +
                    "<br />" + viewResources.New + ": " + log.NewCollStatus;

                var bodyColumns = [
                    { Name: 'ColLogId', Value: log.LogID },
                    { Name: 'ColLogDate', Value: log.LogDateShortDate + "<br />" + log.LogDateLongTime },
                    { Name: 'ColEditedBy', Value: log.EditorName + "<br />(" + log.EditorId + ")" },
                    { Name: 'ColAction', Value: log.Action + "<br />(" + log.SPName + ")" },
                    { Name: 'ColDetail', Value: detail }
                ];

                bodyRows.push({
                    Columns: bodyColumns
                });
            }
        }

        var gridOptions = {
            showTitle: true,
            titleInfo: {
                text: viewResources.StatusLog,
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl('StatusLog/Export'), {
                        Code: statusLog.parameters.Code,
                        PageSize: -1,
                        PageIndex: -1
                    });
                }
            },
            columns: columns,
            showDistinctRow: false,
            showNumberColumn: false,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: statusLog.parameters.PageSize,
                currentPage: statusLog.parameters.PageIndex - 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: statusLog.handleChangingPage,
                totalItem: result ? result.TotalRows : 0
            },
            gridExpandHeight: 200,
            bodyRows: bodyRows
        };
        statusLog.$divReport.removeClass("display-none");
        statusLog.$divReport.fanexGrid(gridOptions);
    },
    handleChangingPage: function (pageIndex, itemsPerPage) {
        statusLog.pageSize = itemsPerPage;
        statusLog.pageIndex = pageIndex + 1;
        statusLog.generate();
    }
};