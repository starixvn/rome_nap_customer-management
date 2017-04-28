/* global currentDate, initFromDate, CMS, CMSConstants, numeral */

Array.prototype.collect = function (collector) {
    var result = [];
    for (var i = 0; i < this.length; ++i) {
        result = result.concat(collector(this[i], i));
    }
    return result;
};

(function () {
    'use strict';
    var rootUrl = $('#CMSRoot').val();

    var $fromtoDatePicker = $('#fromtodate-picker');
    var $vrAdjustmentReportGrid = $("#auto-adjustment-report-freegrid");

    var getVrAdjustmentLog = function () {
        var criteria = {
            AdjustDate: $("#updated-date").val()
        };

        CMS.getData(rootUrl + 'VirtualSettingControl/NewAutoAdjustmentReport/ViewVrAdjustmentReport', criteria, renderVrAdjustmentReportGrid);
    }

    var renderVrAdjustmentReportGrid = function (data) {
        function betcountCustomCellRender(currentCell) {
            return currentCell.Value;
        };

        function getMonthHeaders(month, index) {
            return {
                HeaderText: month, Width: 90, Name: 'colMonth' + index, HeaderAlign: 'center', CellAlign: 'right',
                FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
            };
        };

        function getWinlossCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover > 0 ? marginItem.Winloss : '' };
        }

        function getTurnoverCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover > 0 ? marginItem.Turnover : '' };
        }

        function getMarginCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover > 0 ? marginItem.Margin : '' };
        }

        function getBetCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover > 0 ? marginItem.BetCount : '', CustomCellRender: betcountCustomCellRender };
        }

        var months = data.Months.map(getMonthHeaders);

        var columnHeaders = [
                    { HeaderText: '#', Width: 40, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Member', Width: 180, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: 'Old Rate', Width: 50, Name: 'colOldRate', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'New Rate', Width: 55, Name: 'colNewRate', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Months', Width: 50, Name: 'colMonths', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Won Months', Width: 50, Name: 'colWonMonths', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Figure', Width: 80, Name: 'colFigure', HeaderAlign: 'center', CellAlign: 'left' },
                    {
                        HeaderText: 'Last 3m', Width: 95, Name: 'colLast3m', HeaderAlign: 'center', CellAlign: 'right',
                        FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
                    },
                    {
                        HeaderText: 'Last 6m', Width: 95, Name: 'colLast6m', HeaderAlign: 'center', CellAlign: 'right',
                        FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
                    },
                    {
                        HeaderText: 'Total', Width: 95, Name: 'colTotal', HeaderAlign: 'center', CellAlign: 'right',
                        FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
                    }
        ].concat(months);

        function bodyMapFunc(page, pageSize) {

            return function (item, index) {
                var winlossHeader = {
                    Name: 'colFigure', Value: "Winloss"
                };
                var winlossRow = item.MarginList.map(getWinlossCell);
                var last3mWL = { Name: 'colLast3m', Value: item.Last3Months.Turnover > 0 ? item.Last3Months.Winloss : '' };
                var last6mWL = { Name: 'colLast6m', Value: item.Last6Months.Turnover > 0 ? item.Last6Months.Winloss : '' };
                var totalWL = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Winloss : '' };

                var turnoverHeader = {
                    Name: 'colFigure', Value: "Turnover"
                };
                var turnoverRow = item.MarginList.map(getTurnoverCell);
                var last3mTO = { Name: 'colLast3m', Value: item.Last3Months.Turnover > 0 ? item.Last3Months.Turnover : '' };
                var last6mTO = { Name: 'colLast6m', Value: item.Last6Months.Turnover > 0 ? item.Last6Months.Turnover : '' };
                var totalTO = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Turnover : '' };

                var marginHeader = {
                    Name: 'colFigure', Value: "Margin (%)"
                };
                var marginRow = item.MarginList.map(getMarginCell);
                var last3mMG = { Name: 'colLast3m', Value: item.Last3Months.Turnover > 0 ? item.Last3Months.Margin : '' };
                var last6mMG = { Name: 'colLast6m', Value: item.Last6Months.Turnover > 0 ? item.Last6Months.Margin : '' };
                var totalMG = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Margin : '' };

                var betcountHeader = {
                    Name: 'colFigure', Value: "Bet Count"
                };
                var betRow = item.MarginList.map(getBetCell);
                var last3mBC = { Name: 'colLast3m', Value: item.Last3Months.Turnover > 0 ? item.Last3Months.BetCount : '', CustomCellRender: betcountCustomCellRender };
                var last6mBC = { Name: 'colLast6m', Value: item.Last6Months.Turnover > 0 ? item.Last6Months.BetCount : '', CustomCellRender: betcountCustomCellRender };
                var totalBC = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.BetCount : '', CustomCellRender: betcountCustomCellRender };

                var css = (index & 1) == 1 ? 'freegrid-even' : '';

                var usernameFormat = item.IsBA == null || item.IsBA.toString() == "false" ? "" : "<span class='account-ba'>BA</span>";

                var column = {
                    Columns: [
                        { Name: 'colRowNum', Value: page * pageSize + index + 1, MergeRowNumber: 4 },
                        { Name: 'colUsername', Value: item.Username + usernameFormat, MergeRowNumber: 4 },
                        { Name: 'colOldRate', Value: item.OldRate, MergeRowNumber: 4 },
                        { Name: 'colNewRate', Value: item.NewRate, MergeRowNumber: 4 },
                        { Name: 'colMonths', Value: item.Months, MergeRowNumber: 4 },
                        { Name: 'colWonMonths', Value: item.WonMonths, MergeRowNumber: 4 }
                    ].concat([turnoverHeader, last3mTO, last6mTO, totalTO], turnoverRow),
                    CssClass: css
                };

                var winlossCol = {
                    Columns: [winlossHeader, last3mWL, last6mWL, totalWL].concat(winlossRow),
                    CssClass: css
                };

                var marginCol = {
                    Columns: [marginHeader, last3mMG, last6mMG, totalMG].concat(marginRow),
                    CssClass: css
                };

                var betCol = {
                    Columns: [betcountHeader, last3mBC, last6mBC, totalBC].concat(betRow),
                    CssClass: css
                };

                return [column, winlossCol, marginCol, betCol];
            };
        }

        var gridOptions = {
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 35,
            gridExpandHeight: 180,
            showPager: true,
            customPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges,
                totalItem: data.Reports.length,
                pagerSelectCallBack: onPageChangedHandler
            },
            columns: columnHeaders,
            showTitle: true,
            titleInfo: {
                showExcelButton: true,
                exportExcelFunction: function () {
                    exportVRAdjustmentReport();
                }
            },
            bodyRows: data.Reports.slice(0, CMS.pageSize).collect(bodyMapFunc(0, CMS.pageSize))
        };

        function exportVRAdjustmentReport() {
            var url = rootUrl + 'VirtualSettingControl/NewAutoAdjustmentReport/ExportVRAdjustmentReport';
            var criteria = {
                AdjustDate: $("#updated-date").val()
            };

            CMS.downloadFile(url, criteria);
        };

        function onPageChangedHandler(index, pageSize) {
            pageSize = parseInt(pageSize);
            index = parseInt(index);
            gridOptions.pagerOption.currentPage = index;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = data.Reports.length;

            gridOptions.bodyRows = data.Reports.slice(index * pageSize, index * pageSize + pageSize).collect(bodyMapFunc(index, pageSize));

            $vrAdjustmentReportGrid.fanexGrid(gridOptions);
        };

        $vrAdjustmentReportGrid.removeClass('display-none').fanexGrid(gridOptions);
    }

    function initialize() {
        $("#updated-date").val($("#updated-date option:first").val());

        $('#btn-generate').on('click', function (event) {
            event.preventDefault();

            getVrAdjustmentLog();
        });
    }

    initialize();
})();