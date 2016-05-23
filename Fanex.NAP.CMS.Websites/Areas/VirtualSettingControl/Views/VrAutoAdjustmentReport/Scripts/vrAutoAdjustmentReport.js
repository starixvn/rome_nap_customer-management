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
    var $autoAdjustmentReportDetail = $('#auto-adjustment-report-detail');
    var $vrAdjustmentReportGrid = $("#auto-adjustment-report-freegrid");

    function initDateRangeSelect() {
        $fromtoDatePicker.nexDatePicker({
            nexDatePickerType: 'daterange',
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: '01/01/2012',
                maxDate: null
            },
            firstDatePicker: {
                Id: 'dpk-fromdate',
                Label: '',
                LabelCss: '',
                TextboxCss: 'edit-field col_6'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: '',
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: '',
                TextboxCss: 'edit-field col_6'
            }
        });

        $('#dpk-fromdate').datepicker('setDate', initFromDate);
    }

    var getCustomerHistory = function (custId, username) {
        var url = rootUrl + 'VirtualSettingControl/AutoAdjustmentReport/ViewHistory?CustId=' + custId;

        CMS.getJson(url, function (result) {
            $('#divModals').empty();
            var title = 'Customer Margin History: ' + username;
            CMS.modal(title, $autoAdjustmentReportDetail, { resizable: false });

            renderCustomerHistory(result);
        });
    };

    var renderCustomerHistory = function (data) {
        var generateColumnHeaders = function () {
            var columnHeaders =
                [
                    { HeaderText: '', Width: 60, Name: 'colRowName', HeaderAlign: 'center', CellAlign: 'center' },
                    {
                        HeaderText: "AVG. Performance",
                        Width: 150,
                        Name: 'colAvg',
                        HeaderAlign: 'center',
                        CellAlign: 'right',
                        FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber
                    }
                ];

            for (var i = 0; i < data.MonthList.length; i++) {
                var header = {
                    HeaderText: data.MonthList[i],
                    Width: 100,
                    Name: data.MonthList[i],
                    HeaderAlign: 'center',
                    CellAlign: 'right',
                    FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber
                };

                columnHeaders.push(header);
            }

            return columnHeaders;
        };

        var generateBodyRows = function () {
            var rowsName = ['Turnover', 'Winloss', 'Margin'];
            var rows = [];
            $.each(rowsName, function (index, rowName) {
                var avgData = rowName === 'Turnover' ? data.MarginHistory[data.MonthList.length].Turnover :
                    (rowName === 'Winloss' ? data.MarginHistory[data.MonthList.length].Winloss : data.MarginHistory[data.MonthList.length].Margin);
                var columns = [
                    { Name: 'colRowName', Value: rowName },
                    { Name: 'colAvg', Value: avgData }
                ];

                for (var i = 0; i < data.MonthList.length; i++) {
                    var colData = rowName === 'Turnover' ? data.MarginHistory[i].Turnover :
                        (rowName === 'Winloss' ? data.MarginHistory[i].Winloss : data.MarginHistory[i].Margin);
                    var column = { Name: data.MonthList[i], Value: colData };

                    columns.push(column);
                }

                var turnOverRow = {
                    Columns: columns
                };

                rows.push(turnOverRow);
            });

            return rows;
        };

        var bodyRows = generateBodyRows();
        var columnHeaders = generateColumnHeaders();

        var gridOptions = {
            fullHeight: true,
            fullWidth: true,
            gridExpandHeight: 250
        };

        gridOptions.columns = columnHeaders;
        gridOptions.bodyRows = bodyRows;

        $('#auto-adjustment-report-detail-freegrid').fanexGrid(gridOptions);
    };

    var getVrAdjustmentLog = function () {
        var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');
        var criteria = {
            FromDate: fromtoDate.FromDate,
            ToDate: fromtoDate.ToDate
        };

        CMS.getData(rootUrl + 'VirtualSettingControl/AutoAdjustmentReport/ViewVrAdjustmentReport', criteria, renderVrAdjustmentReportGrid);
    }

    var reportEventHandler = function (reportGrid) {
        reportGrid.find('.ico-remark').click(function () {
            var $this = $(this);
            var custId = $this.data('custid');
            var username = $this.data('username');

            getCustomerHistory(custId, username);
        });
    };

    var renderVrAdjustmentReportGrid = function (data) {
        function getMonthHeaders(month, index) {
            return { HeaderText: month, Width: 100, Name: 'colMonth' + index, HeaderAlign: 'center', CellAlign: 'right'};
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
            { HeaderText: '#', Width: 50, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Member', Width: 120, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Updated Date', Width: 80, Name: 'colUpdatedDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Old Rate', Width: 60, Name: 'colOldRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'New Rate', Width: 60, Name: 'colNewRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Figure', Width: 100, Name: 'colFigure', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'AVG. Performance', Width: 100, Name: 'colAvg', HeaderAlign: 'center', CellAlign: 'right' }
        ].concat(months);

        function bodyMapFunc(page, pageSize) {
            return function (item, index) {
                var winlossHeader = { Name: 'colFigure', Value: "Winloss" };
                var winlossRow = item.MarginViewModels.map(getWinlossCell);
                var avgWinloss = { Name: 'colAvg', Value: item.AvgPerformance.Winloss.toFixed(2) };

                var turnoverHeader = { Name: 'colFigure', Value: "Turnover" };
                var turnoverRow = item.MarginViewModels.map(getTurnoverCell);
                var avgTurnover = { Name: 'colAvg', Value: item.AvgPerformance.Turnover.toFixed(2) };

                var marginHeader = { Name: 'colFigure', Value: "Margin" };
                var marginRow = item.MarginViewModels.map(getMarginCell);
                var avgMargin = { Name: 'colAvg', Value: (item.AvgPerformance.Margin * 100).toFixed(2) + "%" };

                var betcountHeader = { Name: 'colFigure', Value: "Bet Count" };
                var betRow = item.MarginViewModels.map(getBetCell);
                var betTotal = { Name: 'colAvg', Value: item.AvgPerformance.BetCount.toFixed(0) };

                var css = (index & 1) == 1 ? 'freegrid-even' : '';

                var column = {
                    Columns: [
                        { Name: 'colRowNum', Value: page * pageSize + index + 1, MergeRowNumber: 4 },
                        { Name: 'colUsername', Value: item.Username, MergeRowNumber: 4 },
                        { Name: 'colUpdatedDate', Value: item.UpdatedDate, MergeRowNumber: 4 },
                        { Name: 'colOldRate', Value: item.OldRate, MergeRowNumber: 4 },
                        { Name: 'colNewRate', Value: item.NewRate, MergeRowNumber: 4 }
                    ].concat([turnoverHeader, avgTurnover], turnoverRow),
                    CssClass: css
                };

                var winlossCol = {
                    Columns: [winlossHeader, avgWinloss].concat(winlossRow),
                    CssClass: css
                };

                var marginCol = {
                    Columns: [marginHeader, avgMargin].concat(marginRow),
                    CssClass: css
                };

                var betCol = {
                    Columns: [betcountHeader, betTotal].concat(betRow),
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
            bodyRows: data.Reports.slice(0, CMS.pageSize).collect(bodyMapFunc(0, CMS.pageSize))
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
        setTimeout(initDateRangeSelect, 100);

        $('#btn-generate').on('click', function (event) {
            event.preventDefault();

            getVrAdjustmentLog();
        });
    }

    initialize();
})();