"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var freegrid = $("#freegrid");
    var formSearch = $("#formSearch");
    var userName = $("#txtUserName");
    var chatId = $("#txtChatId");
    var functionId = $("#ddl-Functions");
    var status = $("#ddl-SendStatus");
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

    function getGridOptions()
    {
        var columns = [
            { HeaderText: "Sent By", Width: 160, Name: "colSentBy", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: "Message Content", Width: 840, Name: "colMessageContent", HeaderAlign: "center" },
            { HeaderText: "Send Status", Width: 120, Name: "colSendStatus", HeaderAlign: "center", CellAlign: "center", CellCssClass: "status" },
            { HeaderText: "Date (GMT - 4)", Width: 240, Name: "colDate", HeaderAlign: "center", CellAlign: "center", Sort: true },
        ]

        return {
            columns: columns,
            girdExpandHeight: 140,
            showDistinctRow: false,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: [50, 100, 200, 300, 500],
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
            + "?username=" + userName.val().trim()
            + "&chatid=" + chatId.val().trim()
            + "&functionid=" + functionId.val()
            + "&fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate
            + "&status=" + status.val()
            + "&pageindex=" + pageIndex + 1
            + "&pagesize=" + pageSize;

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            var gridOptions = getGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = 49;
            gridOptions.bodyRows = renderChatbotHistories(jsonData);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    function renderChatbotHistories(data) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colSentBy", Value: value.SentBy },
                { Name: "colMessageContent", Value: value.Message },
                { Name: "colSendStatus", Value: value.StatusStr },
                { Name: "colDate", Value: value.CreatedDateStr },
            ];

            data = {
                Columns: columns
            }

            if (value.SentBy === "Customer") {
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
        bindEvents();

        initDateTimeFields();
    });
})(jQuery);