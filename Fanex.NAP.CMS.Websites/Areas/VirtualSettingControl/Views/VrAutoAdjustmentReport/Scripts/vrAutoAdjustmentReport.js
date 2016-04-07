/* global currentDate, initFromDate, CMS, CMSConstants, numeral */
(function () {
    'use strict';
    var rootUrl = $('#CMSRoot').val();
    var $fromtoDatePicker = $('#fromtodate-picker');
    var $autoAdjustmentReportDetail = $('#auto-adjustment-report-detail');
    var $vrAdjustmentReportGrid = $("#auto-adjustment-report-freegrid");

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

    var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');

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
        var criteria = {
            FromDate: '2016/5/8',
            ToDate: '2016/5/9'
        };

        CMS.getData('GET', rootUrl + 'VirtualSettingControl/AutoAdjustmentReport/ViewVrAdjustmentReport', criteria, renderVrAdjustmentReportGrid);
    }

    var renderVrAdjustmentReportGrid = function (data) {
        var columnHeaders = [
            { HeaderText: '#', Width: 60, Name: 'colRowNum', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Member', Width: 100, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Updated Date', Width: 125, Name: 'colUpdatedDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'Old Rate', Width: 100, Name: 'colOldRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: 'New Rate', Width: 100, Name: 'colNewRate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: '', Width: 60, Name: 'colHistoryLink', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        var bodyMapFunc = function (item, index) {
            return {
                Columns: [
                    {Name: 'colRowNum', Value: index + 1},
                    {Name: 'colUsername', Value: item.Username},
                    {Name: 'colUpdatedDate', Value: item.UpdatedDateString},
                    {Name: 'colOldRate', Value: item.OldRate},
                    {Name: 'colNewRate', Value: item.NewRate},
                    {Name: 'colHistoryLink', Value: ''}
                ]
            };
        };

        var gridOptions = {
            fullHeight: true,
            fullWidth: true,
            gridExpandHeight: 250,
            columns: columnHeaders,
        };
        $vrAdjustmentReportGrid.removeClass('display-none');
        CMS.renderGrid($vrAdjustmentReportGrid, gridOptions, data, bodyMapFunc, null, null,reportEventHandler);
    }

    var reportEventHandler=function (target) {
    }

    function initialize() {
        setTimeout(initDateRangeSelect, 100);

        $('#btn-generate').on('click', function (event) {
            event.preventDefault();
            getVrAdjustmentLog();
            //getCustomerHistory(753422, 'Cao Hung Cuong');
        });
    }

    initialize();
})();
