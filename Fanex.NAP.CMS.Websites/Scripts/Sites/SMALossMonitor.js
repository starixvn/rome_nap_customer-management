/* File Created: May 23, 2014 */
/* global Namespace, numeral, Fanex, CMS, labels */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.SMALossMonitor();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.SMALossMonitor");
Fanex.CMS.SMALossMonitor = function () {
    var $this = this;
    var urls = {
        GetSMALossReport: CMS.resolveRootUrl('SMALossMonitor/GetSMALossReport')
    };
    var domElements = {
        divSMALossMonitor: null,
        divSMALossMonitorContainer: null,
        divAutoRefresh: null
    };
    var refreshTimer = {
        Interval: [90, 120, 160, 180],
        Default: 90
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divAutoRefresh = $('#divAutoRefresh');
        domElements.divSMALossMonitor = $('#divSMALossMonitor');
        domElements.divSMALossMonitorContainer = $('#divSMALossMonitorContainer');
    };

    // Initialize fields.
    var initializeFields = function () {
        domElements.divAutoRefresh = domElements.divAutoRefresh.autoRefresh(refreshTimer.Interval, refreshTimer.Default, getSMALossReport);
    };

    // Get SMA turnover report.
    var getSMALossReport = function () {
        CMS.disableInput();

        if (domElements.divAutoRefresh && domElements.divAutoRefresh.stop) {
            domElements.divAutoRefresh.stop();
        }

        $.ajax({
            type: 'POST',
            url: urls.GetSMALossReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            ////data: JSON.stringify(parameters),
            success: function (result) {
                if (domElements.divAutoRefresh && domElements.divAutoRefresh.run) {
                    domElements.divAutoRefresh.run();
                }

                renderGrid(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var progressBarFormat = '<div class="percentage-used" data-value="{0}"><div class="percentage-label"><span>{1}%<span></div></div>';
        var percentageCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.No, Width: 70, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.SMAName, Width: 200, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left', ShowCollapseExpand: true },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 70, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.TotalBalance, Width: 150, Name: 'ColTotalBalance', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MaxLoss, Width: 120, Name: 'ColMaxLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.IntegerFormat
                      },
                      { HeaderText: labels.LossPercentage, Width: 150, Name: 'ColLossPercentage', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 70,
            showDistinctRow: false,
            showTree: true,
            showNumberColumn: true
        };
        var bodyData = [];
        var row;
        var subRows;

        if (data && data.length > 0) {
            $.each(data, function (groupIndex, group) {
                subRows = [];

                $.each(group.SMAItems, function (index, item) {
                    percentageCol = progressBarFormat.format(item.LossPercentage, numeral(item.LossPercentage).format(CMS.formats.IntegerFormat));
                    row = {
                        Columns: [{ Name: 'ColNo', Value: index },
                                  { Name: 'ColSMAName', Value: item.Username },
                                  { Name: 'ColLevel', Value: item.Level },
                                  { Name: 'ColCurrency', Value: item.Currency },
                                  { Name: 'ColTotalBalance', Value: item.TotalBalance },
                                  { Name: 'ColMaxLoss', Value: item.MaxLoss },
                                  { Name: 'ColLossPercentage', Value: percentageCol}]
                    };
                    subRows.push(row);
                });

                row = {
                    Columns: [{ Name: 'ColNo', Value: group.Range, CellAlign: 'left', MergeColNumber: 7, TdClass: 'loss-level-' + group.Level}],
                    SubRows: subRows
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSMALossMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divTurnoverMonitor.find('div.percentage-used').each(function () {
            var $obj = $(this);
            var percentage = parseInt($obj.attr('data-value'));

            $obj.progressbar({
                value: percentage
            });

            if (percentage >= 100) {
                $obj.addClass('full-percents');
            }
        });
    };

    $this.InitPage = function () {
        cacheDOM();
        initializeFields();
    };
};