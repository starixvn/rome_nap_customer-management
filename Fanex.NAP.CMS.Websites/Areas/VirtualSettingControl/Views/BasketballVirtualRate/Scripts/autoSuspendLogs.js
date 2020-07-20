"use strict";
(function ($, gridEvents) {
    var txtUsername = $("#txt-username");
    var fromtoDatePicker = $("#fromtodate-picker");
    var reportContainer = $("#report-container");

    function init() {
        fromtoDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: null,
                maxDate: null
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: resources.From,
                InitDate: initFromDate,
                CurrentDate: currentDate,
                LabelCss: "lbl-field col_2",
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: resources.To,
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: "lbl-field col_2",
                TextboxCss: "edit-field col_5"
            }
        });

        $("#dpk-fromdate").datepicker("setDate", initFromDate);

        var formSearch = $("#form-search");
        var searchURL = formSearch.attr("action") + "?";

        formSearch.submit(function (e) {
            e.preventDefault();
            getAutoSuspendLogs(searchURL);
        });
    }

    function getAutoSuspendLogs(url) {
        var username = $.trim(txtUsername.val());

        if (username.length) {
            var fromtoDate = fromtoDatePicker.nexDatePicker("getDateRange");
            var formData = {
                username: username,
                fromDate: fromtoDate.FromDate,
                toDate: fromtoDate.ToDate
            };

            CMS.getHtml(url + $.param(formData), function (html) {
                if (html.length) {
                    reportContainer.html(html);
                    gridEvents.bindEvents(1);

                    hideTxtUsernameError();
                } else {
                    showTxtUsernameError("This username is not valid.");
                }
            });
        } else {
            showTxtUsernameError(resources.RequiredField);
        }
    }

    function showTxtUsernameError(error) {
        reportContainer.addClass("display-none");
        reportContainer.html("");
        CMS.showTipsyError(txtUsername, error, "n");
    }

    function hideTxtUsernameError() {
        reportContainer.removeClass("display-none");
        CMS.clearTipsyError(txtUsername);
    }

    $(function () {
        init();
    });
})(jQuery, virtualRateLogGridEvents);