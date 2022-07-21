"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var freegrid = $("#freegrid");
    var formSearch = $("#formSearch");
    var userName = $("#txtUserName");
    var chatId = $("#txtChatId");
    var functionId = $("#ddl-Functions");
    var status = $("#ddl-SendStatus");
    let bots = $("#ddl-Bots");
    var cachedPageSize = 100;

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
                TextboxCss: "edit-field col_7"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To",
                LabelCss: "lbl-field lblFilter col_4",
                TextboxCss: "edit-field col_8"
            },
        });

        $("#dpk-fromdate").datepicker("setDate", resources.defaultFromDate);
        $("#dpk-todate").datepicker("setDate", resources.defaultToDate);
    }

    const renderMultiChoiceDDL = () => {
        bots.multiSelect({
            minWidth: 160,
            checkAllText: "All",
            isCheckAll: true,
            listData: bots.find("option").map(function () {
                return {
                    Id: this.text + "-" + this.value,
                    Value: this.value,
                    Text: this.text,
                    Checked: true,
                    Enable: true
                };
            }),
            showFooter: false,
            showTooltip: false,
            shadow: true,
        })

        bots = $("#ddl-Bots");
    }

    function getGridOptions()
    {
        var columns = [
            { HeaderText: resources.sentBy, Width: 160, Name: "colSentBy", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.messageContent, Width: 860, Name: "colMessageContent", HeaderAlign: "center" },
            { HeaderText: resources.sentTo, Width: 160, Name: "colSentTo", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.sendStatus, Width: 120, Name: "colSendStatus", HeaderAlign: "center", CellAlign: "center", CellCssClass: "status" },
            { HeaderText: resources.DateGMT4, Width: 180, Name: "colDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
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

            if (userName.val().trim().includes(",")) {
                Validator.showError(userName, "Please enter only 1 Username", "n");
            }
            else if (chatId.val().trim().includes(",")) {
                Validator.showError(chatId, "Please enter only 1 Messenger Username", "n");
            } else {
                getChatbotHistories(0, cachedPageSize);
            }
        })
    }

    function getChatbotHistories(pageIndex, pageSize) {
        var dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        var url = formSearch.attr("action")
            + "?username=" + userName.val().trim().replaceAll("#", '')
            + "&chatid=" + chatId.val().trim().replaceAll("#", '')
            + "&functionid=" + functionId.val()
            + "&botids=" + bots.multiSelect("getCheckedItems").join(",")
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
            var message = value.SentBy !== "Chatbot"
                ? value.Message.replaceAll("<", "&lt;").replaceAll(">", "&gt;").match(/.{1,95}/g).join("<br/>")
                : value.Message.replaceAll("\n", "<br/>");
            columns = [
                { Name: "colSentBy", Value: renderName(value.SentBy, value.BotName) },
                { Name: "colMessageContent", Value: message },
                { Name: "colSentTo", Value: renderName(value.SentTo, value.BotName) },
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

            if (value.Status === 3) {
                data["CssClass"] = 'body-duplicated-text';
            }

            bodyRows.push(data);            
        })
        
      return bodyRows;
    }

    function renderName(sender, botName) {
        if (sender === "Chatbot") {
            return botName;
        }
        return sender;
    }
      
    $(function () {
        bindEvents();

        initDateTimeFields();

        renderMultiChoiceDDL();
    });
})(jQuery);