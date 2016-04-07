/* global currentDate, initFromDate, CMS, CMSConstants, numeral */
(function () {
    'use strict';
    var gridOptions = null;
    var rootUrl = $('#CMSRoot').val();
    var $fromtoDatePicker = $('#fromtodate-picker');
    var $autoAdjustmentReportDetail = $('#auto-adjustment-report-detail');

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

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
                TextboxCss: 'edit-field col_8'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: '',
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: '',
                TextboxCss: 'edit-field col_8'
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
                { HeaderText: "AVG. Performance", Width: 150, Name: 'colAvg', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber }
            ];

            for (var i = 0; i < data.MonthList.length; i++) {
                var header = { HeaderText: data.MonthList[i], Width: 100, Name: data.MonthList[i], HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber };

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

    function initialize() {
        setTimeout(initDateRangeSelect, 100);

        $('#btn-generate').on('click', function (event) {
            event.preventDefault();

            //getCustomerHistory(753422, 'Cao Hung Cuong');
        });
    }

    initialize();
})();
