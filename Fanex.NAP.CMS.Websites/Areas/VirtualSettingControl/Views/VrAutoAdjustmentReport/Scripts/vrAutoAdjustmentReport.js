/* global currentDate, initFromDate, CMS, CMSConstants, numeral */
(function () {
    'use strict';
    var gridOptions = null,
        freegrid = $('#freegrid');
    var rootUrl = $('#CMSRoot').val();
    var $fromtoDatePicker = $('#fromtodate-picker');

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
                TextboxCss: 'edit-field col_5'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: '',
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_8'
            }
        });

        $('#dpk-fromdate').datepicker('setDate', initFromDate);
    }

    function initialize() {
        setTimeout(initDateRangeSelect, 100);
    }

    initialize();
})();
