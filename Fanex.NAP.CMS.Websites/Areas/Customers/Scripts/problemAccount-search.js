/* global CMS, CMSConstants, numeral, resources, currentDate, fromDate, toDate, defaultMinDate, reasons */
var problemAccountSearch = (function ($, resources) {
    "use strict";
    var pagerRanges = [10, 20, 50, 100],
        defaultPageSize = 50,
        filterParams = null,
        gridOptions = null,
        searchProblemAccountUrl = null;
    var elements = {
        formSearch: $("#formSearch"),
        fromtoDatePicker: $("#fromtodate-picker"),
        ddlLevel: $("#ddl-level"),
        ddlAccountStatus: $("#ddl-accountStatus"),
        ddlCurrency: $("#ddl-currency"),
        ddlIsReduced: $("#ddl-isReduced"),
        ddlReason: $("#ddl-reason"),
        txtUsername: $("#username"),
        chkSearchDownlines: $("#chk-seachDownlines"),
        ddlOrderBy: $("#ddl-orderBy"),
        dataGrid: $("#freegrid")
    };

    function initSearchFilter() {
        setTimeout(initDateRange, 100);

        elements.ddlReason.fanexMultiSelect({
            minWidth: 306,
            checkAllText: "All",
            //isCheckAll: true,
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
                LabelCss: "lbl-field col_3",
                TextboxCss: "edit-field col_6"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To ",
                InitDate: toDate,
                CurrentDate: currentDate,
                LabelCss: "lbl-field col_1",
                TextboxCss: "edit-field col_6"
            }
        });

        $("#dpk-fromdate").datepicker("setDate", fromDate);
    }

    function bindEvents() {
        searchProblemAccountUrl = elements.formSearch.attr("action") + "?";
        elements.formSearch.submit(function (event) {
            event.preventDefault();

            filterParams = getFilterParams();
            getProblemAccounts(0, defaultPageSize, true);
        });
    }

    function getFilterParams() {
        var dateRange = elements.fromtoDatePicker.nexDatePicker("getDateRange");

        return {
            from: dateRange.FromDate,
            to: dateRange.ToDate,
            level: elements.ddlLevel.val(),
            status: elements.ddlAccountStatus.val(),
            currency: elements.ddlCurrency.val(),
            isReduced: elements.ddlIsReduced.val(),
            reason: elements.ddlReason.fanexMultiSelect("getItems", true).join(","),
            username: $.trim(elements.txtUsername.val()),
            isSearchDownlines: elements.chkSearchDownlines.is(":checked"),
            orderBy: elements.ddlOrderBy.val()
        };
    }

    function getProblemAccounts(pageIndex, pageSize, isSearchButtionClick) {
        CMS.getJson(
            searchProblemAccountUrl + $.param(filterParams),
            function (data) {
                var jsonData = $.parseJSON(data);
                var jsonData = {
                    ProblemAccounts: [
                        {
                            Headline: "headline",
                            Super: "super",
                            Master: "master",
                            Agent: "agent",
                            Member: "member",
                            AccountStatus: "Active",
                            IsReduced: "No",
                            MaxPerEvent: "",
                            MaxPerBet: "",
                            MarkDate: "01/31/2019 23:59:59",
                            Reasons: ["01/31/2019: Arbitrage", "02/01/2019: Hedging", "08/12/2019: Group Betting"],
                            Comment: "",
                            AbnormalBet: "5"
                        },
                        {
                            Headline: "headline",
                            Super: "super",
                            Master: "master",
                            Agent: "agent",
                            AccountStatus: "View Only",
                            IsReduced: "Yes",
                            MaxPerEvent: "",
                            MaxPerBet: "",
                            MarkDate: "01/31/2019 23:59:59",
                            Reasons: ["01/31/2019: Hedging"],
                            Comment: "Comment B",
                            AbnormalBet: "10"
                        },
                        {
                            Headline: "headline",
                            Super: "super",
                            Master: "master",
                            AccountStatus: "Suspended",
                            IsReduced: "No",
                            MaxPerEvent: "",
                            MaxPerBet: "",
                            MarkDate: "01/31/2019 23:59:59",
                            Reasons: ["01/31/2019: Group Betting"],
                            Comment: "Comment C",
                            AbnormalBet: "100"
                        },
                        {
                            Headline: "headline",
                            Super: "super",
                            AccountStatus: "Inactive",
                            IsReduced: "Yes",
                            MaxPerEvent: "",
                            MaxPerBet: "",
                            MarkDate: "01/31/2019 23:59:59",
                            Comment: "Comment D",
                            AbnormalBet: "1000"
                        }
                    ],
                    Page: 1,
                    Size: 50,
                    TotalItems: 4
                };

                if (isSearchButtionClick) {
                    gridOptions = initGridOptions(filterParams.level !== "1");
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

    function initGridOptions(hideAbnormalBetColumn) {
        var hiddenColumns = showSelectColumn ? [] : [9],
            reasonAndCommentWidth = 380;

        if (hideAbnormalBetColumn) {
            hiddenColumns.push(8);
            reasonAndCommentWidth += 50;
        }

        var columns = [
            { HeaderText: resources.account + renderAccountNote(), Width: 320, Name: "colAccount", HeaderAlign: "center", CellAlign: "left", CssClass: "custom-grid-colheader" },
            { HeaderText: resources.accountStatus, Width: 100, Name: "colAccountStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.isReduced, Width: 80, Name: "colIsReduced", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.maxPerEvent, Width: 160, Name: "colMaxPerEvent", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.maxPerBet, Width: 160, Name: "colMaxPerBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.markDate, Width: 80, Name: "colMarkDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.reason, Width: reasonAndCommentWidth, Name: "colReason", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.comment, Width: reasonAndCommentWidth, Name: "colComment", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.abnormalBet + renderAbnormalBetNote(), Width: 100, Name: "colAbnormalBet", HeaderAlign: "center", CellAlign: "right", CssClass: "custom-grid-colheader" },
            { HeaderText: resources.select + '<br/><input type="checkbox" id="chk-select-all">', Width: 60, Name: "colSelect", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 190,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: pagerRanges,
                currentPage: 0,
                itemsPerPage: defaultPageSize,
                pagerSelectCallBack: getProblemAccounts
            },
            enableHiddenColumns: false,
            hiddenColumns: hiddenColumns
        };
    }

    function renderAccountNote() {
        return '<span class="light-note-info-message" title="<p>Black username: Normal account</p><p>Blue username: Problem account</p>"></span>';
    }

    function renderAbnormalBetNote() {
        return '<span class="light-note-info-message" title="<p>Abnormal Bets with void date belonging to the report date.</p>"></span>';
    }

    function renderProblemAccounts(data) {
        var bodyRows = [];
        var cell, problemAccount;
        var dataLength = data ? data.length : 0;
        for (var i = 0; i < dataLength; i++) {
            problemAccount = data[i];
            cell = [
                { Name: "colAccount", Value: renderAccountCell([problemAccount.Headline, problemAccount.Super, problemAccount.Master, problemAccount.Agent, problemAccount.Member]) },
                { Name: "colAccountStatus", Value: problemAccount.AccountStatus },
                { Name: "colIsReduced", Value: problemAccount.IsReduced },
                { Name: "colMaxPerEvent", Value: problemAccount.MaxPerEvent },
                { Name: "colMaxPerBet", Value: problemAccount.MaxPerBet },
                { Name: "colMarkDate", Value: problemAccount.MarkDate },
                { Name: "colReason", Value: renderReasonCell(problemAccount.Reasons || []) },
                { Name: "colComment", Value: problemAccount.Comment },
                { Name: "colAbnormalBet", Value: renderAbnormalBetCell(1, "member 1.11", problemAccount.AbnormalBet) },
                { Name: "colSelect", Value: renderSelectCell("member 1.11") }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function renderAccountCell(accounts) {
        return accounts.map(function (item, index) {
            if (item) {
                return '<p class="level-' + index + '">' + item + '</p>';
            }
        }).join("");
    }

    function renderReasonCell(reasons) {
        return reasons.map(function (item) {
            return "<p>" + item + "</p>";
        }).join("");
    }

    function renderAbnormalBetCell(custId, custName, betCount) {
        return [
            '<a href="javascript:;" class="abnormal-bet-item" data-custid="',
            custId,
            '" data-custname="',
            custName,
            '">',
            numeral(betCount).format(CMSConstants.NumberFormat.IntNumber),
            '</a>'
        ].join("");
    }

    function renderSelectCell(username) {
        return '<input type="checkbox" class="chk-select-item" data-username="' + username + '" />';
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
                CMS.resolveRootUrl("Customers/BetList/AbnormalBetTicket?custid="),
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
        var level = -1, usernames = [];

        if (filterParams) {
            level = filterParams.level;

            $(".chk-select-item:checked").each(function () {
                usernames.push($(this).data("username"));
            });
        }

        return {
            level: level,
            usernames: usernames
        };
    }

    return {
        init: init,
        getCheckedProblemAccount: getCheckedProblemAccount,
        reloadData: function () {
            elements.formSearch.trigger("submit");
        }
    };
})(jQuery, resources);