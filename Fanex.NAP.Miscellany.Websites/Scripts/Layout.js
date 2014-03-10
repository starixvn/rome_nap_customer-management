/* File Created: March 3, 2014 */
/* global Miscellany */
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
                    dateFormat: Miscellany.formats.DateFormat,
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
        createDateRange: function ($obj, fromLabel, toLabel, defaultDate, minDate, maxDate) {
            var configObj = {
                nexDatePickerType: 'daterange',
                firstDatePicker: {
                    Id: 'dpkFromDate',
                    Label: fromLabel,
                    InitDate: defaultDate,
                    LabelCss: 'lbl-field col_2',
                    TextboxCss: 'edit-field col_6 width-auto'
                },
                secondDatePicker: {
                    Id: 'dpkToDate',
                    Label: toLabel,
                    InitDate: defaultDate,
                    LabelCss: 'lbl-field col_2',
                    TextboxCss: 'edit-field col_6 width-auto'
                },

                datePickerFormat: {
                    dateFormat: Miscellany.formats.DateFormat,
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
        }
    };
} (jQuery));

$(document).ready(function () {

    var menuEvent = Miscellany.agentIsMobile ? 'click' : 'hover';

    $('nav').on(menuEvent, '.menu > li', function (e) {
        var $this = $(this),
            parent = $this.parents('.menu');

        if ($this.hasClass('selected')) {
            $this.removeClass('selected');
        } else {
            parent.children('li').removeClass('selected');
            $this.addClass('selected');
        }

        e.stopPropagation();
    });

    $('header').on(menuEvent, '.user-info .greeting', function (e) {
        $(this).toggleClass('selected');
        e.stopPropagation();
    });

    if (Miscellany.agentIsMobile) {
        $('body, header, #headline, #body').on('click', function () {
            $('nav .menu > li').removeClass('selected');
            $('header .user-info .greeting').removeClass('selected');
        });
    }

    $('select.combobox-autocomplete').each(function () {
        $(this).combobox();
    });
    $('input[type=text]').each(function () {
        $(this).clearfield();
    });
});