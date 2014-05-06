/* File Created: March 3, 2014 */
/* global CMS */
/* exported Layout */

'use strict';

var Layout = (function () {
    return {
        createDatepicker: function ($obj, id, label, defaultDate, minDate, maxDate) {
            var configObj = {
                nexDatePickerType: 'single',
                firstDatePicker: {
                    Id: id,
                    Label: label,
                    InitDate: defaultDate,
                    LabelCss: 'lbl-field col_2',
                    TextboxCss: 'edit-field col_6 width-auto'
                },
                datePickerFormat: {
                    dateFormat: CMS.formats.DateFormat,
                    changeMonth: true,
                    changeYear: true,
                    showOtherMonths: true,
                    selectOtherMonths: true
                }
            };

            if (minDate) {
                configObj.datePickerFormat.minDate = new Date(minDate);
            }

            if (maxDate) {
                configObj.datePickerFormat.maxDate = new Date(maxDate);
            }

            return $obj.nexDatePicker(configObj);
        },
        createDateRange: function ($obj, fromLabel, fromCss, toLabel, toCss, defaultDate, minDate, maxDate, onChangeHandler) {
            var configObj = null;

            fromCss = fromCss || 'col_6 width-auto';
            toCss = toCss || 'col_6 width-auto';
            onChangeHandler = onChangeHandler || function () { };

            configObj = {
                nexDatePickerType: 'daterange',
                firstDatePicker: {
                    Id: 'dpkFromDate',
                    Label: fromLabel,
                    InitDate: defaultDate,
                    LabelCss: 'lbl-field col_3',
                    TextboxCss: 'edit-field' + ' ' + fromCss
                },
                secondDatePicker: {
                    Id: 'dpkToDate',
                    Label: toLabel,
                    InitDate: defaultDate,
                    LabelCss: 'lbl-field col_3',
                    TextboxCss: 'edit-field' + ' ' + toCss
                },

                datePickerFormat: {
                    dateFormat: CMS.formats.DateFormat,
                    changeMonth: true,
                    changeYear: true,
                    showOtherMonths: true,
                    selectOtherMonths: true
                },

                onSelectDatePicker: onChangeHandler
            };

            if (minDate) {
                configObj.datePickerFormat.minDate = new Date(minDate);
            }

            if (maxDate) {
                configObj.datePickerFormat.maxDate = new Date(maxDate);
            }

            return $obj.nexDatePicker(configObj);
        }
    };
} (jQuery));

$(document).ready(function () {
    $('select.combobox-autocomplete').each(function () {
        $(this).combobox();
    });
    $('input[type=text]').each(function () {
        $(this).clearfield();
    });
});