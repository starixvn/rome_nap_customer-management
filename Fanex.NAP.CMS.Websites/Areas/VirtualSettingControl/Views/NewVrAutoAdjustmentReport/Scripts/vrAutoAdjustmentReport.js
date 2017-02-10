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
        var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');
        var criteria = {
            AdjustDate: $("#updated-date").val()
        };

        CMS.getData(rootUrl + 'VirtualSettingControl/NewAutoAdjustmentReport/ViewVrAdjustmentReport', criteria, renderVrAdjustmentReportGrid);
    }

    var renderVrAdjustmentReportGrid = function (data) {
        function getMonthHeaders(month, index) {
            return { HeaderText: month, Width: 90, Name: 'colMonth' + index, HeaderAlign: 'center', CellAlign: 'right' };
        };

        function getWinlossCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Winloss.toFixed(2) };
        }

        function getTurnoverCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover.toFixed(2) };
        }

        function getMarginCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: (marginItem.Margin * 100).toFixed(2) + "%" };
        }

        function getBetCell(marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.BetCount.toFixed(0) };
        }

        var months = data.Months.map(getMonthHeaders);

        var columnHeaders = [
                    { HeaderText: '#', Width: 40, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Member', Width: 120, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: 'Months', Width: 50, Name: 'colMonths', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Won Months', Width: 50, Name: 'colWonMonths', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Figure', Width: 80, Name: 'colFigure', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Last 3m', Width: 95, Name: 'colLast3m', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Last 6m', Width: 95, Name: 'colLast6m', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Total', Width: 95, Name: 'colTotal', HeaderAlign: 'center', CellAlign: 'right' }
        ].concat(months);

        function bodyMapFunc(page, pageSize) {
            return function (item, index) {
                var winlossHeader = { Name: 'colFigure', Value: "Winloss" };
                var winlossRow = item.MarginList.map(getWinlossCell);
                var last3mWL = { Name: 'colLast3m', Value: item.Last3Months.Winloss.toFixed(2) };
                var last6mWL = { Name: 'colLast6m', Value: item.Last6Months.Winloss.toFixed(2) };
                var totalWL = { Name: 'colTotal', Value: item.Total.Winloss.toFixed(2) };

                var turnoverHeader = { Name: 'colFigure', Value: "Turnover" };
                var turnoverRow = item.MarginList.map(getTurnoverCell);
                var last3mTO = { Name: 'colLast3m', Value: item.Last3Months.Turnover.toFixed(2) };
                var last6mTO = { Name: 'colLast6m', Value: item.Last6Months.Turnover.toFixed(2) };
                var totalTO = { Name: 'colTotal', Value: item.Total.Turnover.toFixed(2) };

                var marginHeader = { Name: 'colFigure', Value: "Margin" };
                var marginRow = item.MarginList.map(getMarginCell);
                var last3mMG = { Name: 'colLast3m', Value: item.Last3Months.Margin.toFixed(2) + "%" };
                var last6mMG = { Name: 'colLast6m', Value: item.Last6Months.Margin.toFixed(2) + "%" };
                var totalMG = { Name: 'colTotal', Value: item.Total.Margin.toFixed(2) + "%" };

                var betcountHeader = { Name: 'colFigure', Value: "Bet Count" };
                var betRow = item.MarginList.map(getBetCell);
                var last3mBC = { Name: 'colLast3m', Value: item.Last3Months.BetCount.toFixed(0) };
                var last6mBC = { Name: 'colLast6m', Value: item.Last6Months.BetCount.toFixed(0) };
                var totalBC = { Name: 'colTotal', Value: item.Total.BetCount.toFixed(0) };

                var css = (index & 1) == 1 ? 'freegrid-even' : '';

                var column = {
                    Columns: [
                        { Name: 'colRowNum', Value: page * pageSize + index + 1, MergeRowNumber: 4 },
                        { Name: 'colUsername', Value: item.Username, MergeRowNumber: 4 },
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
            var url = rootUrl + 'VirtualSettingControl/AutoAdjustmentReport/ExportVRAdjustmentReport';
            var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');
            var criteria = {
                FromDate: fromtoDate.FromDate,
                ToDate: fromtoDate.ToDate
            };

            CMS.downloadFile(url, criteria);
        };

        function onPageChangedHandler(index, pageSize) {
            //var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');

            //var criteria = {
            //    FromDate: fromtoDate.FromDate,
            //    ToDate: fromtoDate.ToDate,
            //    PageIndex: index,
            //    PageSize: pageSize
            //};

            //CMS.getData(rootUrl + 'VirtualSettingControl/AutoAdjustmentReport/ViewVrAdjustmentReport', criteria, function (data) {
            pageSize = parseInt(pageSize);
            index = parseInt(index);
            gridOptions.pagerOption.currentPage = index;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = data.Reports.length;

            gridOptions.bodyRows = data.Reports.slice(index * pageSize, index * pageSize + pageSize).collect(bodyMapFunc(index, pageSize));

            $vrAdjustmentReportGrid.fanexGrid(gridOptions);
            //});
        };

        $vrAdjustmentReportGrid.removeClass('display-none').fanexGrid(gridOptions);

        //CMS.renderGrid($vrAdjustmentReportGrid, gridOptions, data.Reports, bodyMapFunc);
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