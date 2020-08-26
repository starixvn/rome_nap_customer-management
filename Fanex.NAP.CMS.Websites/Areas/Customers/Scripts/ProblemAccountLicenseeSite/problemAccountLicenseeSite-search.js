var problemAccountLicenseeSiteSearch = (function ($, resources) {
    "use strict";
    var pagerRanges = [10, 20, 50, 100],
        defaultPageSize = 50,
        filterParams = null,
        gridOptions = null,
        searchProblemAccountUrl = null;
    var elements = {
        formSearch: $("#formSearch"),
        fromtoDatePicker: $("#fromtodate-picker"),
        ddlAcknowledge: $("#ddl-acknowledge"),
        ddlVIP: $("#ddl-vip"),
        ddlCurrency: $("#ddl-currency"),
        ddlIsReduced: $("#ddl-isReduced"),
        ddlReason: $("#ddl-reason"),
        ddlPreventiveActionId: $("#ddl-preventiveActionId"),
        txtMember: $("#member"),
        txtLicensee: $("#licensee"),
        dataGrid: $("#freegrid")
    };

    function initSearchFilter() {
        setTimeout(initDateRange, 100);

        elements.ddlAcknowledge.val(searchFilter.Acknowledge);
        elements.ddlVIP.val(searchFilter.VIP);
        elements.ddlCurrency.val(searchFilter.Currency);
        elements.ddlIsReduced.val(searchFilter.IsReduced);
        elements.ddlPreventiveActionId.val(searchFilter.PreventiveActionId);
        elements.txtMember.val(searchFilter.Member);
        elements.txtLicensee.val(searchFilter.Licensee);

        elements.ddlReason.fanexMultiSelect({
            minWidth: 281,
            checkAllText: "All",
            listData: reasons.map(function (item) {
                return { id: item.Value, value: item.Value, text: item.Text, checked: item.Selected, enable: true };
            })
        });
    }

    function initDateRange() {
        elements.fromtoDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: defaultMinDate,
                maxDate: currentDate
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: "From ",
                LabelCss: "lbl-field col_2",
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To ",
                LabelCss: "lbl-field col_1",
                TextboxCss: "edit-field col_5"
            }
        });

        $("#dpk-fromdate").datepicker("setDate", new Date(searchFilter.From));
        $("#dpk-todate").datepicker("setDate", new Date(searchFilter.To));
    }

    function bindEvents() {
        searchProblemAccountUrl = elements.formSearch.attr("action") + "?";
        elements.formSearch.submit(function (event) {
            event.preventDefault();

            var pageSize = filterParams ? filterParams.PageSize : defaultPageSize;
            filterParams = getFilterParams();
            getProblemAccounts(0, pageSize, true);
        });
    }

    function getFilterParams() {
        var dateRange = elements.fromtoDatePicker.nexDatePicker("getDateRange");

        return {
            from: dateRange.FromDate,
            to: dateRange.ToDate,
            acknowledge: elements.ddlAcknowledge.val(),
            vip: elements.ddlVIP.val(),
            currency: elements.ddlCurrency.val(),
            isReduced: elements.ddlIsReduced.val(),
            reason: elements.ddlReason.fanexMultiSelect("getItems", true).join(","),
            preventiveActionId: elements.ddlPreventiveActionId.val(),
            member: $.trim(elements.txtMember.val()),
            licensee: $.trim(elements.txtLicensee.val())
        };
    }

    function getProblemAccounts(pageIndex, pageSize, isSearchButtionClick) {
        // PageIndex of Fanex.Grid begin at 0 whereas server site begin at 1.
        filterParams.PageIndex = pageIndex + 1;
        filterParams.PageSize = pageSize;
        CMS.getJson(
            searchProblemAccountUrl + $.param(filterParams),
            function (data) {
                var jsonData = $.parseJSON(data);

                if (isSearchButtionClick) {
                    gridOptions = initGridOptions();
                }

                gridOptions.pagerOption.currentPage = pageIndex;
                gridOptions.pagerOption.itemsPerPage = pageSize;
                gridOptions.pagerOption.totalItem = jsonData.TotalItems;
                gridOptions.bodyRows = renderProblemAccounts(jsonData.ProblemAccounts);

                elements.dataGrid.removeClass("display-none").fanexGrid(gridOptions);
                $(".light-note-info-message").tipsy({
                    className: "custom-grid-colheader-note",
                    html: true,
                    opacity: 0.9,
                    gravity: "n"
                });

                if (jsonData.TotalItems > 0) {
                    bindEventsAfterRenderProblemAccounts();
                }
            });
    }

    function initGridOptions() {
        var hiddenColumns = showSelectColumn ? [] : [11],
            reasonAndCommentWidth = 378;

        var columns = [
            { HeaderText: resources.member, Width: 250, Name: "colMember", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
            { HeaderText: resources.currency, Width: 80, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.maxPerMatch, Width: 100, Name: "colMaxPerMatch", HeaderAlign: "center", CellAlign: "none" },
            { HeaderText: resources.maxBet, Width: 100, Name: "colMaxBet", HeaderAlign: "center", CellAlign: "none" },
            { HeaderText: resources.preventiveAction, Width: 120, Name: "colPreventiveAction", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.markDate, Width: 80, Name: "colMarkDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.reason, Width: reasonAndCommentWidth, Name: "colReason", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.comment, Width: reasonAndCommentWidth, Name: "colComment", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.firstWeekInformed, Width: 80, Name: "col1stWeekInformed", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.secondWeekInformed, Width: 80, Name: "col2ndWeekInformed", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.abnormalBet + renderAbnormalBetNote(), Width: 100, Name: "colAbnormalBet", HeaderAlign: "center", CellAlign: "right", CssClass: "custom-grid-colheader" },
            { HeaderText: resources.select + '<br/><input type="checkbox" id="chk-select-all">', Width: 60, Name: "colSelect", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            showTitle: true,
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl("Customers/ProblemAccountLicenseeSite/Export"), filterParams);
                }
            },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 250,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: pagerRanges,
                pagerSelectCallBack: getProblemAccounts
            },
            enableHiddenColumns: false,
            hiddenColumns: hiddenColumns,
            showTree: true
        };
    }

    function renderAbnormalBetNote() {
        return '<span class="light-note-info-message" title="<p>Abnormal Bets with void date belonging to the report date.</p>"></span>';
    }

    function renderProblemAccounts(data) {
        var bodyRows = [];

        if (data.length) {
            var row, subRows, columns;
            var mergeColNumber = showSelectColumn ? 12 : 11;

            $.each(data, function (i, group) {
                subRows = [];

                $.each(group.ProblemAccounts, function (j, problemAccount) {
                    columns = [
                        { Name: "colMember", Value: renderAccountCell(problemAccount) },
                        { Name: "colCurrency", Value: problemAccount.Currency }
                    ];

                    if (problemAccount.IsVIP) {
                        columns.push({ Name: "colMaxPerMatch", Value: "VIP", TdClass: "vip", MergeColNumber: 2 });
                    } else {
                        columns.push({ Name: "colMaxPerMatch", Value: renderMaxPerMatchOrMaxBet(problemAccount.MaxPerMatch), CssClass: "align-right" });
                        columns.push({ Name: "colMaxBet", Value: renderMaxPerMatchOrMaxBet(problemAccount.MaxBet), CssClass: "align-right" });
                    }

                    columns.push({ Name: "colPreventiveAction", Value: renderPreventAction(problemAccount) });
                    columns.push({ Name: "colMarkDate", Value: problemAccount.MarkDateString });
                    columns.push({ Name: "colReason", Value: renderReasonCell(problemAccount.Reasons || []) });
                    columns.push({ Name: "colComment", Value: problemAccount.Comment });
                    columns.push({ Name: "col1stWeekInformed", Value: problemAccount.Inform1 || "<div class='ico-noninformed'></div>" });
                    columns.push({ Name: "col2ndWeekInformed", Value: problemAccount.Inform2 || "<div class='ico-noninformed'></div>" });
                    columns.push({ Name: "colAbnormalBet", Value: renderAbnormalBetCell(problemAccount) });
                    columns.push({ Name: "colSelect", Value: renderSelectCell(problemAccount.Username, problemAccount.Currency, problemAccount.IsAcknowledgement, problemAccount.IsVIP) });

                    subRows.push({ Columns: columns });
                });

                row = {
                    Columns: [{
                        Name: "colMember",
                        Value: group.Site + " [" + group.Acknowledged + "]",
                        CellAlign: "left",
                        MergeColNumber: mergeColNumber
                    }],
                    SubRows: subRows
                };

                bodyRows.push(row);
            });
        }

        return bodyRows;
    }

    function renderAccountCell(problemAccount) {
        var cells = [
            "<p>", problemAccount.Username, "</p>"
        ];

        if (problemAccount.Username2) {
            cells.push("<p><i>");
            cells.push(problemAccount.Username2);
            cells.push("</i></p>");
        }

        return cells.join("");
    }

    function renderMaxPerMatchOrMaxBet(value) {
        if (typeof value === "undefined") {
            return "N/A";
        }

        if (value === 0) {
            return "0";
        }

        return numeral(value).format(CMSConstants.NumberFormat.IntNumber)
    }

    function renderReasonCell(reasons) {
        return reasons.map(function (item) {
            return "<p>" + item + "</p>";
        }).join("");
    }

    function renderAbnormalBetCell(problemAccount) {
        return problemAccount.AbnormalBetNumber > 0
            ? ['<a href="javascript:;" class="abnormal-bet-item" data-custid="',
                problemAccount.CustId,
                '" data-custname="',
                problemAccount.Username,
                '">',
                numeral(problemAccount.AbnormalBetNumber).format(CMSConstants.NumberFormat.IntNumber),
                '</a>'].join("")
            : "0";
    }

    function renderSelectCell(username, currency, isAcknowledgement, isVIP) {
        return [
            '<input type="checkbox" class="chk-select-item" data-username="',
            username,
            '" data-currency="',
            currency,
            '" data-isacknowledgement="',
            isAcknowledgement,
            '" data-isvip="',
            isVIP,
            '" />'
        ].join("");
    }

    function renderPreventAction(problemAccount) {
        if (problemAccount.ActionName === "Void Winning Tickets") {
            var limitAmount = problemAccount.LimitAmount === 0 ? "Unlimited" : problemAccount.LimitAmount;

            return `${problemAccount.ActionName} - ${problemAccount.VoidTime} - ${limitAmount}`;
        }

        return problemAccount.ActionName;
    }

    function bindEventsAfterRenderProblemAccounts() {
        var checkall = $("#chk-select-all"),
            checkitems = $(".chk-select-item");

        checkall.off("change").on("change", function () {
            checkitems.prop("checked", this.checked);
        });

        checkitems.off("change").on("change", function () {
            var numberOfCheckedCheckItems = $(".chk-select-item:checked").length;

            if (numberOfCheckedCheckItems === 0) {
                checkall.prop("indeterminate", false).prop("checked", false);
            } else if (numberOfCheckedCheckItems === checkitems.length) {
                checkall.prop("indeterminate", false).prop("checked", true);
            } else {
                checkall.prop("indeterminate", true);
            }
        });

        $(".abnormal-bet-item").off("click").on("click", function () {
            var $this = $(this);
            window.location.href = [
                CMS.resolveRootUrl("Customers/BetList/AbnormalBetTicketLicenseeSite?custid="),
                $this.data("custid"),
                "&custname=",
                $this.data("custname"),
                "&",
                $.param(filterParams)
            ].join("");
        });
    }

    function init() {
        initSearchFilter();
        bindEvents();
    }

    function getCheckedProblemAccount() {
        var usernames = [];
        var currency;
        var isAcknowledgement;
        var vipUsernames = [];

        if (filterParams) {
            $(".chk-select-item:checked").each(function (index) {
                var checkbox = $(this);
                var isVIP = checkbox.data("isvip");
                var username = checkbox.data("username");
                usernames.push(username);

                if (isVIP) {
                    vipUsernames.push(username);
                }

                if (index === 0) {
                    currency = checkbox.data("currency");
                    isAcknowledgement = checkbox.data("isacknowledgement");
                } else {
                    if (currency.length && currency !== checkbox.data("currency")) {
                        currency = "";
                    }

                    if (isAcknowledgement && isAcknowledgement !== checkbox.data("isacknowledgement")) {
                        isAcknowledgement = false;
                    }
                }
            });
        }

        return {
            usernames: usernames,
            currency: currency,
            isAcknowledgement: isAcknowledgement,
            vipUsernames: vipUsernames
        };
    }

    return {
        init: init,
        getCheckedProblemAccount: getCheckedProblemAccount,
        reloadData: function () {
            getProblemAccounts(0, filterParams.PageSize);
        },
        autoReloadData: function (pageIndex, pageSize) {
            filterParams = getFilterParams();
            getProblemAccounts(pageIndex - 1, pageSize, true);
        },
        reloadDataForMarkNew: function () {
            filterParams
                ? getProblemAccounts(0, filterParams.PageSize)
                : elements.formSearch.trigger("submit");
        }
    };
})(jQuery, resources);
