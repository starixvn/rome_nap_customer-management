"use strict";
(function ($) {
    const fromToDatePicker = $("#fromtodate-picker");
    const freegrid = $("#freegrid");
    const formSearch = $("#formSearch");
    const status = $("#ddl-SendStatus");
    const recipient = $("#txtRecipient");
    const notificationId = $("#txtNotificationId");
    let notificationType = $("#ddl-NotificationType");
    let cachedPageSize = 500;

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
                LabelCss: "lbl-field lblFilter col_3",
                TextboxCss: "edit-field col_8"
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
        notificationType.multiSelect({
            minWidth: 194,
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
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });

        notificationType = $("#ddl-NotificationType");
    }

    function getGridOptions() {
        const columns = [
            { HeaderText: resources.symbolNo, Width: 40, Name: "col#", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.notificationId, Width: 100, Name: "colNotificationId", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: resources.notificationType, Width: 180, Name: "colNotificationType", HeaderAlign: "center", Sort: true },
            { HeaderText: resources.sendDateTime, Width: 140, Name: "colSendDate", HeaderAlign: "center", CellAlign: "center", CellCssClass: "status", Sort: true },
            { HeaderText: resources.recipient, Width: 150, Name: "colRecipient", HeaderAlign: "center", AllowTruncate: true, Sort: true },
            { HeaderText: resources.title, Width: 700, Name: "colTitleContent", HeaderAlign: "center"},
            { HeaderText: resources.status, Width: 100, Name: "colStatus", HeaderAlign: "center", CellAlign: "center" },
        ]

        return {
            columns: columns,
            showDistinctRow: false,
            numberOfFrozenColumn: 0,
            fullHeight: true,
            gridExpandHeight: 200,
            showPager: true,
            customPager: true,
            showNumberColumn: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    cachedPageSize = pageSize;
                    getNotificationList(pageIndex, pageSize);
                }
            },
            virtualScroller: true
        }
    }

    function bindEvents() {
        formSearch.submit(function (e) {
            e.preventDefault();
            getNotificationList(0, cachedPageSize);
        })
    }

    function getNotificationList(pageIndex, pageSize) {
        const dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        const url = formSearch.attr("action")
            + "?notificationtype=" + notificationType.multiSelect("getCheckedItems").join(",")
            + "&recipient=" + recipient.val().trim().replaceAll("#", '')
            + "&notificationid=" + notificationId.val().trim().replaceAll("#", '')
            + "&fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate
            + "&status=" + status.find('option:selected').text()
            + "&pageindex=" + (pageIndex + 1)
            + "&pagesize=" + pageSize;

        CMS.getJson(url, function (data) {
            const jsonData = $.parseJSON(data);

            const gridOptions = getGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalRows;
            gridOptions.bodyRows = renderNotificationList(jsonData.NotificationLists);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);
            handleLoadMore();
            handleViewDetail();
        });
    }

    function renderNotificationList(data) {
        const bodyRows = [];

        $.each(data, function (index, value) {
            const columns = [
                { Name: "col#", Value: index },
                { Name: "colNotificationId", Value: value.NotificationId },
                { Name: "colNotificationType", Value: value.FunctionName },
                { Name: "colSendDate", Value: value.SendDateStr },
                { Name: "colRecipient", Value: value.Recipient },
                { Name: "colTitleContent", Value: renderContent(index, value.Message) },
                { Name: "colStatus", Value: renderStatus(value) },
            ];

            data = {
                Columns: columns
            }

            bodyRows.push(data);
        })

        return bodyRows;
    }

    function renderContent(index, content) {
        const newContent = breakContent(content);
        const loadMore = newContent.split(/\n/g).length > 3 ? "icons-expand" : "";
        const isHover = newContent.split(/\n/g).length < 4 ? "isNotHover" : "";
        const contentFormated = newContent.replaceAll("\n", "<br/>");
        return [
            `<div class="description ${isHover}" loadmore-id = ${index} isExpand = false><p class="content-text">`,
            `<span class="content-width content${index} content-overflow">`,
            contentFormated,
            '</span>',
            `<a class="expand"><span class="rotate${index} ${loadMore}""></span></a>`,
            '</p></div>',
        ].join("");
    }

    function breakContent(content) {
        const splits = content.split(/\n/g);
        const result = splits.map(item => {
            return item.match(/.{1,104}/g)?.join("\n");
        });
        return result.join("\n");
    }

    function handleLoadMore() {
        $(".description").click(function () {
            const id = $(this).attr("loadmore-id");
            const isExpand = $(this).attr("isExpand");
            const rotateName = ".rotate" + id;
            const contentName = ".content" + id;

            if (isExpand === "false") {
                $(rotateName).addClass("rotate");
                $(contentName).removeClass("content-overflow");
                $(this).attr("isExpand", "true");
            }
            else {
                $(contentName).addClass("content-overflow");
                $(rotateName).removeClass("rotate");
                $(this).attr("isExpand", "false");
            }

            $('#freegrid').fanexGrid('resizeGrid');
        })
    }

    function renderStatus(notification) {
        if (notification.Status === "Fail" || notification.Status === "Invalid") {
            return [
                '<div class="status"><p class="status-fail">',
                `${notification.Status}`,
                '</p>',
                '<a><p class="status-detail"',
                'status=',
                `"${notification.Status}"` + ' ',
                'recipient=',
                `"${notification.Recipient}"` + ' ',
                'date=',
                `"${notification.SendDateStr}"` + ' ',
                'description=',
                `"${notification.Description}"` + ' ',
                'retry=',
                `"${notification.Retry}"` + ' ',
                '>View Detail</p></a ></div >'
            ].join("");
        }
        return notification.Status;
    }

    function handleViewDetail() {
        $(".status-detail").click(function () {
            const notifyStatus = $(this).attr("status");
            const rec = $(this).attr("recipient");
            const date = $(this).attr("date");
            const reason = $(this).attr("description");
            const retry = $(this).attr("retry");
            CMS.popup("Notification Sending Error Reason", buildFailPopup(notifyStatus, rec, date, reason, retry), { resizedable: false });
        })
    }

    function buildFailPopup(notifyStatus, rec, date, reason, retry) {
        const error = notifyStatus === "Fail" ? "technical error." : "business error.";
        return [
            '<div class="notification"><div class="title"><p>Recipient:</p>',
            '<p>Send at:</p><p class="error-title">Reason:</p>',
            '</div><div class="error">',
            `<p>${rec}</p>`,
            `<p>${date}</p>`,
            `<p>Notifications cannot be sent due to a ${error}</p>`,
            `<p class="reason">${reason}</p>`,
            '</div></div>',
            `<div class="notification"><div class="title"><p>Retry Count:</p></div>`,
            `<div class="error"><p>${retry}</p></div></div>`
        ].join("");
    }

    $(function () {
        initDateTimeFields();

        renderMultiChoiceDDL();

        bindEvents();
    });
})(jQuery);