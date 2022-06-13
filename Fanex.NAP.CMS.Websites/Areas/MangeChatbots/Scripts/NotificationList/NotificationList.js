"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var freegrid = $("#freegrid");
    var formSearch = $("#formSearch");
    var status = $("#ddl-SendStatus");
    var recipient = $("#txtRecipient");
    var notificationId = $("#txtNotificationId");
    var notificationType = $("#ddl-NotificationType");
    var cachedPageSize = 50;

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
                LabelCss: "lbl-field lblFilter col_6",
                TextboxCss: "edit-field col_8"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To",
                LabelCss: "lbl-field lblFilter col_6",
                TextboxCss: "edit-field col_8"
            },
        });

        $("#dpk-fromdate").datepicker("setDate", resources.defaultFromDate);
        $("#dpk-todate").datepicker("setDate", resources.defaultToDate);
    }

    const renderMultiChoiceDDL = () => {
        notificationType.multiSelect({
            minWidth: 238,
            checkAllText: "All",
            isCheckAll: true,
            listData: notificationType.find("option").map(function () {
                return {
                    Id: this.text + "-" + this.value,
                    Value: this.value,
                    Text: this.text,
                    Checked: true,
                    Enable: true
                };
            }),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });

        notificationType = $("#ddl-NotificationType");
    }

    function getGridOptions() {
        var columns = [
            { HeaderText: "#", Width: 40, Name: "colSentBy", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Notification ID", Width: 120, Name: "colMessageContent", HeaderAlign: "center", Sort: true },
            { HeaderText: "Notification Type", Width: 180, Name: "colSentTo", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: "Send Date/Time (GMT - 4)", Width: 180, Name: "colSendStatus", HeaderAlign: "center", CellAlign: "center", CellCssClass: "status", Sort: true  },
            { HeaderText: "Recipient", Width: 120, Name: "colDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: "Title/Content", Width: 700, Name: "colDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: "Status", Width: 100, Name: "colDate", HeaderAlign: "center", CellAlign: "center" },
        ]

        return {
            columns: columns,
            showDistinctRow: false,
            fullHeight: true,
            gridExpandHeight: 200,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    cachedPageSize = pageSize;
                    getChatbotHistories(pageIndex, pageSize);
                }
            }
        }
    }

    function bindEvents() {

        formSearch.submit(function (e) {
            e.preventDefault();
            var gridOptions = getGridOptions();
            freegrid.removeClass("display-none").fanexGrid(gridOptions);
        })
    }

    function getChatbotHistories(pageIndex, pageSize) {
        var dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        var url = formSearch.attr("action")
            + "?notificationtype=" + userName.val().trim()
            + "&recipient=" + recipient.val().trim()
            + "&notificationid=" + chatId.val().trim()
            + "&fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate
            + "&status=" + status.val()
            + "&pageindex=" + (pageIndex + 1)
            + "&pagesize=" + pageSize;

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            var gridOptions = getGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalRows;
            gridOptions.bodyRows = renderChatbotHistories(jsonData.ChatbotHistories);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    function renderChatbotHistories(data) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            var message = value.SentBy !== "Chatbot" ? value.Message.replaceAll("<", "&lt;").replaceAll(">", "&gt;") : value.Message;
            columns = [
                { Name: "colSentBy", Value: value.SentBy },
                { Name: "colMessageContent", Value: message },
                { Name: "colSentTo", Value: value.SentTo },
                { Name: "colSendStatus", Value: value.StatusStr },
                { Name: "colDate", Value: value.CreatedDateStr },
            ];

            data = {
                Columns: columns
            }

            if (value.SentBy !== "Chatbot") {
                columns[1]["TdClass"] = 'cell-content-class';
                data["CssClass"] = 'body-class-bg';
            }

            if (value.Status === 2) {
                data["CssClass"] = 'body-class-text';
            }

            bodyRows.push(data);
        })

        return bodyRows;
    }

    $(function () {
        initDateTimeFields();

        renderMultiChoiceDDL();

        bindEvents();
    });
})(jQuery);