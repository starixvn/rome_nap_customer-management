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

    var $divMonthFrom = $('#divMonthFrom');
    var $divMonthTo = $('#divMonthTo');
    var $sport = $('#sport');
    var $rate = $("#txt-rate");
    var $depth = $('#depth');
    var $username = $("#txt-username");
    var criteriaExport = null;

    var options = {
        minDate: minDate,
        maxDate: maxDate,
        defaultFromDate: currentDate,
        defaultToDate: currentDate,
        allowYearRange: 1,
        yearRanges: null
    };

    var $virtualSettingMonitorAdvanceGrid = $("#virtual-setting-monitor-advance-freegrid");

    function initDateRangeSelect() {
        var changeMonthFromHandler = function () {

        };

        var changeMonthToHandler = function () {
            var selectedDate = $divMonthTo.monPickr('getMonth', 'MM/dd/yyyy');
            var limitedDate = moment(selectedDate).add('month', -12);
            var minDate = limitedDate.format('MM/DD/YYYY');
            var maxDate = moment(selectedDate).endOf('month').format('MM/DD/YYYY');

            $divMonthFrom.monPickr('setMinMaxDate', minDate, maxDate);
        };

        $divMonthFrom.monPickr({
            plugInId: '#divMonthFrom',
            today: options.defaultFromDate,
            maxDate: options.maxDate,
            minDate: options.minDate,
            showDateRange: false,
            width: 132,
            zIndex: 1,
            afterChangeMonth: changeMonthFromHandler
        });
        $divMonthTo.monPickr({
            plugInId: '#divMonthTo',
            today: options.defaultFromDate,
            maxDate: options.maxDate,
            minDate: options.minDate,
            showDateRange: false,
            width: 138,
            zIndex: 1,
            afterChangeMonth: changeMonthToHandler
        });

        $divMonthFrom.on('click', 'input.monpickr-input', function () {
            $divMonthTo.monPickr('hideDropDown');
        });
        $divMonthTo.on('click', 'input.monpickr-input', function () {
            $divMonthFrom.monPickr('hideDropDown');
        });
    }

    var getVirtualSettingMonitorAdvance = function () {
        var criteria = {
            FromDate: $divMonthFrom.monPickr('getMonth', 'MM/dd/yyyy'),
            ToDate: $divMonthTo.monPickr('getMonth', 'MM/dd/yyyy'),
            SportId: $sport.val(),
            Rate: $rate.val(),
            Depth: $depth.val(),
            Username: $username.val()
        };

        criteriaExport = criteria;

        CMS.getData(rootUrl + 'VirtualSettingControl/VirtualSettingMonitorAdvance/Generate', criteria, renderVirtualSettingMonitorAdvance);
    }

    var renderVirtualSettingMonitorAdvance = function (data) {
        function betcountCustomCellRender(currentCell) {
            return currentCell.Value;
        };

        function getMonthHeaders(month, index) {
            return {
                HeaderText: month, Width: 90, Name: 'colMonth' + index, HeaderAlign: 'center', CellAlign: 'right',
                FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
            };
        };

        function getWinlossCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.Turnover > 0 ? dataItem.Winloss : '' };
        }

        function getTurnoverCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.Turnover > 0 ? dataItem.Turnover : '' };
        }

        function getMarginCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.Turnover > 0 ? dataItem.Margin : '' };
        }

        function getBetCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.Turnover > 0 ? dataItem.BetCount : '', CustomCellRender: betcountCustomCellRender };
        }

        function getVarianceWinlossCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.VarianceTurnover > 0 ? dataItem.VarianceWinloss : '' };
        }

        function getVarianceTurnoverCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.VarianceTurnover > 0 ? dataItem.VarianceTurnover : '' };
        }

        function getVarianceMarginCell(dataItem, index) {
            return { Name: 'colMonth' + index, Value: dataItem.VarianceTurnover > 0 ? dataItem.VarianceMargin : '' };
        }

        var months = data.Months.map(getMonthHeaders);

        var columnHeaders = [
                    { HeaderText: '#', Width: 40, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Account Name', Width: 180, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: 'Figure', Width: 80, Name: 'colFigure', HeaderAlign: 'center', CellAlign: 'left' },
                    {
                        HeaderText: 'Avg. Performance', Width: 95, Name: 'colTotal', HeaderAlign: 'center', CellAlign: 'right',
                        FormatNumber: CMS.formats.DecimalFormat, FormatNegative: '$'
                    }
        ].concat(months);

        function bodyMapFunc(page, pageSize) {

            return function (item, index) {
                //Username header
                var usernameHeader = {
                    Name: 'colUsername', Value: item.Username, MergeRowNumber: 4
                };

                var varianceHeader = {
                    Name: 'colUsername', Value: "Variance:", MergeRowNumber: 3, Css: 'freegrid-even', CellAlign: 'right'
                };
                //Account
                var winlossHeader = {
                    Name: 'colFigure', Value: "Winloss"
                };
                var winlossRow = item.Items.map(getWinlossCell);
                var totalWL = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Winloss : '' };

                var turnoverHeader = {
                    Name: 'colFigure', Value: "Turnover"
                };
                var turnoverRow = item.Items.map(getTurnoverCell);
                var totalTO = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Turnover : '' };

                var marginHeader = {
                    Name: 'colFigure', Value: "Margin (%)"
                };
                var marginRow = item.Items.map(getMarginCell);
                var totalMG = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.Margin : '' };

                var betcountHeader = {
                    Name: 'colFigure', Value: "Bet Count"
                };
                var betRow = item.Items.map(getBetCell);
                var totalBC = { Name: 'colTotal', Value: item.Total.Turnover > 0 ? item.Total.BetCount : '', CustomCellRender: betcountCustomCellRender };
                //

                //Variance
                var varianceWinlossRow = item.Items.map(getVarianceWinlossCell);
                var totalVarianceWL = { Name: 'colTotal', Value: item.Total.VarianceTurnover > 0 ? item.Total.VarianceWinloss : '' };

                var varianceTurnoverRow = item.Items.map(getVarianceTurnoverCell);
                var totalVarianceTO = { Name: 'colTotal', Value: item.Total.VarianceTurnover > 0 ? item.Total.VarianceTurnover : '' };

                var varianceMarginRow = item.Items.map(getVarianceMarginCell);
                var totalVarianceMG = { Name: 'colTotal', Value: item.Total.VarianceTurnover > 0 ? item.Total.VarianceMargin : '' };
                //

                var css = 'freegrid-even';

                var column = {
                    Columns: [
                        { Name: 'colRowNum', Value: page * pageSize + index + 1, MergeRowNumber: 7 }
                    ].concat([usernameHeader, turnoverHeader, totalTO], turnoverRow)
                };

                var winlossCol = {
                    Columns: [winlossHeader, totalWL].concat(winlossRow)
                };

                var marginCol = {
                    Columns: [marginHeader, totalMG].concat(marginRow)
                };

                var betCol = {
                    Columns: [betcountHeader, totalBC].concat(betRow)
                };

                var varianceTurnoverCol = {
                    Columns: [varianceHeader, turnoverHeader, totalVarianceTO].concat(varianceTurnoverRow),
                    CssClass: css
                };

                var varianceWinlossCol = {
                    Columns: [winlossHeader, totalVarianceWL].concat(varianceWinlossRow),
                    CssClass: css
                };

                var varianceMarginCol = {
                    Columns: [marginHeader, totalVarianceMG].concat(varianceMarginRow),
                    CssClass: css
                };

                return [column, winlossCol, marginCol, betCol, varianceTurnoverCol, varianceWinlossCol, varianceMarginCol];
            };
        }

        var gridOptions = {
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 35,
            gridExpandHeight: 230,
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
                    exportExcel();
                }
            },
            bodyRows: data.Reports.slice(0, CMS.pageSize).collect(bodyMapFunc(0, CMS.pageSize))
        };

        function exportExcel() {
            var url = rootUrl + 'VirtualSettingControl/VirtualSettingMonitorAdvance/Export';

            CMS.downloadFile(url, criteriaExport);
        };

        function onPageChangedHandler(index, pageSize) {
            pageSize = parseInt(pageSize);
            index = parseInt(index);
            gridpagerOption.currentPage = index;
            gridpagerOption.itemsPerPage = pageSize;
            gridpagerOption.totalItem = data.Reports.length;

            gridbodyRows = data.Reports.slice(index * pageSize, index * pageSize + pageSize).collect(bodyMapFunc(index, pageSize));

            $virtualSettingMonitorAdvanceGrid.fanexGrid(gridOptions);
        };

        $virtualSettingMonitorAdvanceGrid.removeClass('display-none').fanexGrid(gridOptions);
    }

    function initialize() {
        setTimeout(initDateRangeSelect, 100);

        $('#btn-generate').on('click', function (event) {
            event.preventDefault();

            $rate.tipsy('hide').removeAttr('original-title');
            var isValid = Validator.validateRequiredField($rate, 'w') && Validator.validateRegexField($rate, 'w');

            if (isValid) {
                getVirtualSettingMonitorAdvance();
            }
        });
    }

    initialize();
})();