"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var freegrid = $("#freegrid");
    var formSearch = $("#formSearch");
    var status = $("#ddl-SendStatus");
    var recipient = $("#txtRecipient");
    var notificationId = $("#txtNotificationId");
    var notificationType = $("#ddl-NotificationType");
    var cachedPageSize = 500;

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
            { HeaderText: resources.symbolNo, Width: 40, Name: "col#", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.notificationId, Width: 100, Name: "colNotificationId", HeaderAlign: "center", CellAlign: "center", Sort: true },
            { HeaderText: resources.notificationType, Width: 180, Name: "colNotificationType", HeaderAlign: "center", Sort: true },
            { HeaderText: resources.sendDateTime, Width: 170, Name: "colSendDate", HeaderAlign: "center", CellAlign: "center", CellCssClass: "status", Sort: true },
            { HeaderText: resources.recipient, Width: 150, Name: "colRecipient", HeaderAlign: "center", CellAlign: "center", AllowTruncate: true, Sort: true },
            { HeaderText: resources.title, Width: 700, Name: "colTitleContent", HeaderAlign: "center", AllowTruncate: true },
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
            }
        }
    }

    function bindEvents() {
        formSearch.submit(function (e) {
            e.preventDefault();
            getNotificationList(0, cachedPageSize);
        })
    }

    function getNotificationList(pageIndex, pageSize) {
        var dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        var url = formSearch.attr("action")
            + "?notificationtype=" + notificationType.multiSelect("getCheckedItems").join(",")
            + "&recipient=" + recipient.val().trim()
            + "&notificationid=" + notificationId.val().trim()
            + "&fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate
            + "&status=" + status.find('option:selected').text()
            + "&pageindex=" + (pageIndex + 1)
            + "&pagesize=" + pageSize;

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            var gridOptions = getGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalRows;
            gridOptions.bodyRows = renderNotificationList(jsonData.NotificationLists);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);
            handleViewDetail();
        });
    }

    function renderNotificationList(data) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "col#", Value: index },
                { Name: "colNotificationId", Value: value.NotificationId },
                { Name: "colNotificationType", Value: value.FunctionName },
                { Name: "colSendDate", Value: value.SendDateStr },
                { Name: "colRecipient", Value: value.Recipient },
                { Name: "colTitleContent", Value: value.Message },
                { Name: "colStatus", Value: renderStatus(value) },
            ];

            data = {
                Columns: columns
            }

            bodyRows.push(data);
        })

        return bodyRows;
    }

    function renderStatus(notification) {
        if (notification.Status === "Fail" || notification.Status === "Invalid") {
            return [
                '<div class="status"><p style="color: red">',
                `${notification.Status}`,
                '</p>',
                '<a><p class="view-detail"',
                'recipient=',
                `"${notification.Recipient}"` + ' ',
                'date=',
                `"${notification.SendDateStr}"` + ' ',
                'description=',
                `"${notification.Description}"` + ' ',
                'retry=',
                `"${notification.Retry}"` + ' ',
                '>View Detail</p ></a ></div >'
            ].join("");
        }
        return notification.Status;
    }

    function handleViewDetail() {
        $(".view-detail").click( function () {
            var rec = $(this).attr("recipient");
            var date = $(this).attr("date");
            var reason = $(this).attr("description");
            var retry = $(this).attr("retry");
            CMS.popup("Notification Sending Error Reason", buildFailPopup(rec, date, reason, retry), { resizedable: false });
        })
    }

    function buildFailPopup(rec, date, reason, retry) {
        return [
            '<div class="notification">',
                '<div class="title">',
                    '<p>',
                        'Recipient:',
                    '</p>',
                   '<p>',
                        'Send at:',
                    '</p>',
                    '<p>',
                        'Reason:',
                    '</p>',
                    '<br>',
                    '<br>',
                    '<br>',
                    '<br>',
                    '<p>',
                        'Retry Count:',
                    '</p>',
                '</div>',
                '<div style="font-weight: bold">',
                   '<p>Super101011</p>',
                    '<p>06/06/2022 05:10:00</p>',
                    '<p>Notifications cannot be sent due to a business error:</p>',
                    '<p>Customer Status: Disable or Closed</p>',
                    '<p>5</p>',
                '</div>',
            '</div>',
        ].join("");
    }

    $(function () {
        initDateTimeFields();

        renderMultiChoiceDDL();

        bindEvents();
    });
})(jQuery);