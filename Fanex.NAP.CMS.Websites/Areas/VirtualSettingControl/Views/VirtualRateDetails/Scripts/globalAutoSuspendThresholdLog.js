"use strict";
(function ($, CMS, resources) {
    var fromtoDatePicker = $("#fromtodate-picker");
    var reportContainer = $("#report-container");
    var reportTitle = $("#report-tile");
    var tableBody = $("#tbl-virtual-rate-report").find("tbody");

    function initDateRange() {
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
    }

    function renderLogs(data, formData) {
        var html = [];

        if (data.length) {
            $.each(data.Sports, function (index, sport) {
                html.push(renderSport(sport, data));
            });
        } else {
            html.push('<tr><td colspan="3" align="center">');
            html.push(resources.Message_NoReportData);
            html.push("</td></tr>");
        }

        reportTitle.html(buildReportTitle(formData));
        tableBody.html(html.join(""));
        reportContainer.removeClass("display-none");
    }

    function buildReportTitle(formData) {
        return [
            resources.ReportTitle,
            " - ",
            resources.From,
            " ",
            formData.from,
            " ",
            resources.To,
            " ",
            formData.to
        ].join("");
    }

    $(document).ready(function () {
        initDateRange();

        var formViewLog = $("#formViewLog").submit(function (event) {
            event.preventDefault();

            var dateRange = fromtoDatePicker.nexDatePicker("getDateRange");
            var formData = {
                sportType: 43,
                from: dateRange.FromDate,
                to: dateRange.ToDate
            };

            CMS.getJson(
                formViewLog.attr("action") + "?" + $.param(formData),
                function (data) {
                    //$.parseJSON(data)
                    renderLogs([], formData);
                });
        });
    });
})(jQuery, CMS, resources);