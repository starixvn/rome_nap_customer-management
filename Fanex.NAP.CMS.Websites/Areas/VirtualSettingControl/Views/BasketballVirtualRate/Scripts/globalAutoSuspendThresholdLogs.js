"use strict";
(function ($, gridEvents) {
    var fromtoDatePicker = $("#fromtodate-picker");
    var reportContainer = $("#report-container");
    var contentGrid = $("#content-grid");

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
        var fromtoDate = fromtoDatePicker.nexDatePicker("getDateRange");
        var formData = {
            fromDate: fromtoDate.FromDate,
            toDate: fromtoDate.ToDate
        };

        CMS.getHtml(url + $.param(formData), function (html) {
            contentGrid.html(buildReportTitle(formData) + html);
            gridEvents.bindEvents(4);

            reportContainer.removeClass("display-none");
        });
    }

    function buildReportTitle(formData) {
        return [
            '<div class="grid-title"><div class="grid-title-label">',
            resources.ReportTitle,
            " - ",
            resources.From,
            " ",
            moment(formData.from).format("MMM DD, YYYY"),
            " ",
            resources.To,
            " ",
            moment(formData.to).format("MMM DD, YYYY"),
            "</div></div>"
        ].join("");
    }

    $(function () {
        init();
    });
})(jQuery, virtualRateLogGridEvents);