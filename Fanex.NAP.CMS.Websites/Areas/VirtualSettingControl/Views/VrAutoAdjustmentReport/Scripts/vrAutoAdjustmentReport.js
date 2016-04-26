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

    var renderVrAdjustmentReportGrid = function (data) {

        var getMonthHeaders = function (month, index) {
            return { HeaderText: month, Width: 100, Name: 'colMonth' + index, HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber };
        };

        var getWinlossCell = function (marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Winloss };
        }

        var getTurnoverCell = function (marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Turnover };
        }

        var getMarginCell = function (marginItem, index) {
            return { Name: 'colMonth' + index, Value: marginItem.Margin };
        }

        var months = data.Months.map(getMonthHeaders);


        var columnHeaders = [
            { HeaderText: '#', Width: 50, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Member', Width: 100, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: 'Updated Date', Width: 100, Name: 'colUpdatedDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Old Rate', Width: 100, Name: 'colOldRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'New Rate', Width: 100, Name: 'colNewRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'AVG. Performance', Width: 100, Name: 'colAvg', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber },
        ].concat(months);

        var bodyMapFunc = function (item, index) {

            var winlossRow = item.MarginViewModels.map(getWinlossCell);

            var avgWinloss = { Name: 'colMonth0', Value: item.AvgPerformance.Winloss };

            var turnoverRow = item.MarginViewModels.map(getTurnoverCell);

            var avgTurnover = { Name: 'colMonth0', Value: item.AvgPerformance.Turnover };

            var marginRow = item.MarginViewModels.map(getMarginCell);

            var avgMargin = { Name: 'colMonth0', Value: item.AvgPerformance.Margin };

            var column = {
                Columns: [
                    { Name: 'colRowNum', Value: index + 1, MergeRowNumber: 3 },
                    { Name: 'colUsername', Value: item.Username, MergeRowNumber: 3 },
                    { Name: 'colUpdatedDate', Value: item.UpdatedDate, MergeRowNumber: 3 },
                    { Name: 'colOldRate', Value: item.OldRate, MergeRowNumber: 3 },
                    { Name: 'colNewRate', Value: item.NewRate, MergeRowNumber: 3 }
                ].concat( [avgWinloss],winlossRow)
            };

            var turnoverCol = {
                Columns: [avgTurnover].concat(turnoverRow)
            };

            var marginCol = {
                Columns: [avgMargin].concat(marginRow)
            };

            return [column, turnoverCol, marginCol];
        };

        var gridOptions = {
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 35,
            gridExpandHeight: 180,
            showPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges,
                afterPagedFunction: reportEventHandler
            },
            columns: columnHeaders,
            bodyRows: data.Reports.collect(bodyMapFunc)
        };

        $vrAdjustmentReportGrid.removeClass('display-none').fanexGrid(gridOptions);

        //CMS.renderGrid($vrAdjustmentReportGrid, gridOptions, data.Reports, bodyMapFunc);
    }

    var reportEventHandler = function (reportGrid) {
        reportGrid.find('.ico-remark').click(function () {
            var $this = $(this);
            var custId = $this.data('custid');
            var username = $this.data('username');

            getCustomerHistory(custId, username);
        });
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
