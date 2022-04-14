"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var freegrid = $("#freegrid");
    var formSearch = $("#formSearch");
    var userName = $("#txtUserName");
    var messengerUserName = $("#txtMessengerUsername");

    function initDateTimeFields() {
        fromToDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            blankable: true,
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                maxDate: "",
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: "From",
                LabelCss: "lbl-field lblFilter col_2",
                TextboxCss: "edit-field"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To",
                LabelCss: "lbl-field lblFilter col_3",
                TextboxCss: "edit-field"
            },
        });

        $("#dpk-fromdate").datepicker("setDate", resources.defaultFromDate);
        $("#dpk-todate").datepicker("setDate", resources.defaultToDate);
    }

    function getGirdOptions()
    {
        var columns = [
            { HeaderText: "Sent By", Width: 160, Name: "colSentBy", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: "Message Content", Width: 840, Name: "colMessageContent", HeaderAlign: "center" },
            { HeaderText: "Send Status", Width: 120, Name: "colSendStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Date (GMT - 4)", Width: 240, Name: "colDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
        ]

        return {
            columns: columns,
            girdExpandHeight: 140
        }
    }

    function bindEvents() {

        formSearch.submit(function (e) {
            e.preventDefault();

            if (userName.val().trim().includes(",")) {
                Validator.showError(userName, "Please enter only 1 Username", "n");
            }
            else if (messengerUserName.val().trim().includes(",")) {
                Validator.showError(messengerUserName, "Please enter only 1 Messenger Username", "n");
            } else {
                getChatbotHistories();
            }
        })
    }

    function getChatbotHistories() {
        var url = formSearch.attr("action")
            + "?username=" + "test"
            + "&messengerusername=" + "test"
            + "&fromdate=" + "1/1/2020"
            + "&todate=" + "1/1/2021"
            + "&function=" + "test"
            + "&sendstatus=" + "fail";

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            var girdOptions = getGirdOptions();
            girdOptions.bodyRows = renderChatbotHistories(jsonData);

            freegrid.removeClass("display-none").fanexGrid(girdOptions);
        });
    }

    function renderChatbotHistories(data) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colSentBy", Value: value.SendBy },
                { Name: "colMessageContent", Value: value.MessageContent },
                { Name: "colSendStatus", Value: value.SendStatus },
                { Name: "colDate", Value: value.Date },
            ];

            data = {
                Columns: columns
            }

            if (value.SendBy === "Customer") {
                columns[1]["TdClass"] = 'cell-content-class';
                data["CssClass"] = 'body-class-bg';
            }

            if (value.SendStatus === "Failed") {
                data["CssClass"] = 'body-class-text';
            }

            bodyRows.push(data);            
        })
        
      return bodyRows;
    }
      
    $(function () {
        bindEvents();

        initDateTimeFields();
    });
})(jQuery);