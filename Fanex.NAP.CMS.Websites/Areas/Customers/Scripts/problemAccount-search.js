var problemAccountSearch = (function ($, resources) {
    "use strict";
    var pagerRanges = [10, 20, 50, 100],
        defaultPageSize = 50,
        roleMapper = { 0: "&nbsp;&nbsp;S:", 1: "&nbsp;&nbsp;M:", 2: "&nbsp;&nbsp;A:", 3: "mb:" },
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
        dataGrid: $("#freegrid"),
        directlyMarked: $("#ddl-directlyMarked")
    };

    function initSearchFilter() {
        setTimeout(initDateRange, 100);

        elements.ddlLevel.val(searchFilter.Level);
        elements.ddlCurrency.val(searchFilter.Currency);
        elements.ddlAccountStatus.val(searchFilter.Status);
        elements.ddlIsReduced.val(searchFilter.IsReduced);
        elements.txtUsername.val(searchFilter.Username);
        elements.chkSearchDownlines.prop("checked", searchFilter.IsSearchDownlines);
        elements.ddlOrderBy.val(searchFilter.OrderBy);
        elements.directlyMarked.val(searchFilter.IsDirectlyMarked);

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
            level: elements.ddlLevel.val(),
            status: elements.ddlAccountStatus.val(),
            currency: elements.ddlCurrency.val(),
            isReduced: elements.ddlIsReduced.val(),
            reason: elements.ddlReason.fanexMultiSelect("getItems", true).join(","),
            username: $.trim(elements.txtUsername.val()),
            isSearchDownlines: elements.chkSearchDownlines.is(":checked"),
            orderBy: elements.ddlOrderBy.val(),
            isDirectlyMarked: elements.directlyMarked.val()
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
        var hiddenColumns = showSelectColumn ? [] : [10],
            reasonAndCommentWidth = 335;

        if (hideAbnormalBetColumn) {
            hiddenColumns.push(9);
            reasonAndCommentWidth += 53;
        }

        var columns = [
            { HeaderText: resources.account + renderAccountNote(), Width: 320, Name: "colAccount", HeaderAlign: "center", CellAlign: "left", CssClass: "custom-grid-colheader" },
            { HeaderText: resources.accountStatus, Width: 100, Name: "colAccountStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.maxPerEvent, Width: 160, Name: "colMaxPerEvent", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.maxPerBet, Width: 160, Name: "colMaxPerBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.markDate, Width: 80, Name: "colMarkDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.reason, Width: reasonAndCommentWidth, Name: "colReason", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.comment, Width: reasonAndCommentWidth, Name: "colComment", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.FirstWeekInformed, Width: 80, Name: "col1stWeekInformed", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.SecondWeekInformed, Width: 80, Name: "col2ndWeekInformed", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.abnormalBet + renderAbnormalBetNote(), Width: 100, Name: "colAbnormalBet", HeaderAlign: "center", CellAlign: "right", CssClass: "custom-grid-colheader" },
            { HeaderText: resources.select + '<br/><input type="checkbox" id="chk-select-all">', Width: 60, Name: "colSelect", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            showTitle: true,
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl("Customers/ProblemAccount/Export"), filterParams);
                }
            },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 280,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: pagerRanges,
                pagerSelectCallBack: getProblemAccounts
            },
            enableHiddenColumns: false,
            hiddenColumns: hiddenColumns
        };
    }

    function renderAccountNote() {
        return '<span class="light-note-info-message" title="<p>Black username: Normal account</p><p><span class=\'text-blue\'>Blue username</span>: Problem account</p>"></span>';
    }

    function renderAbnormalBetNote() {
        return '<span class="light-note-info-message" title="<p>Abnormal Bets with void date belonging to the report date.</p>"></span>';
    }

    function renderProblemAccounts(data) {
        var bodyRows = [];
        var cell, problemAccount, maxPerEvents, maxPerBets;
        var roleId = parseInt(filterParams.level);
        var dataLength = data ? data.length : 0;
        for (var i = 0; i < dataLength; i++) {
            problemAccount = data[i];
            maxPerEvents = buildMaxPerEvents(problemAccount, roleId);
            maxPerBets = buildMaxPerBets(problemAccount, roleId);
            cell = [
                { Name: "colAccount", Value: renderAccountCell(problemAccount, roleId) },
                { Name: "colAccountStatus", Value: problemAccount.StatusName },
                { Name: "colMaxPerEvent", Value: renderMaxPerEventOrMaxPerBet(maxPerEvents) },
                { Name: "colMaxPerBet", Value: renderMaxPerEventOrMaxPerBet(maxPerBets) },
                { Name: "colMarkDate", Value: problemAccount.MarkDateString },
                { Name: "colReason", Value: renderReasonCell(problemAccount.Reasons || []) },
                { Name: "colComment", Value: problemAccount.Comment },
                { Name: "col1stWeekInformed", Value: problemAccount.FirstWeekInformed || "<div class='ico-noninformed'></div>" },
                { Name: "col2ndWeekInformed", Value: problemAccount.SecondWeekInformed || "<div class='ico-noninformed'></div>" },
                { Name: "colAbnormalBet", Value: renderAbnormalBetCell(problemAccount) },
                { Name: "colSelect", Value: renderSelectCell(problemAccount.Username) }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function buildMaxPerEvents(problemAccount, roleId) {
        var maxPerEvents = [problemAccount.SuperMPM];
        if (roleId <= 3) maxPerEvents.push(problemAccount.MasterMPM);
        if (roleId <= 2) maxPerEvents.push(problemAccount.AgentMPM);
        if (roleId <= 1) maxPerEvents.push(problemAccount.MPM);

        return maxPerEvents;
    }

    function buildMaxPerBets(problemAccount, roleId) {
        var maxPerBets = [problemAccount.SuperMaxBet];
        if (roleId <= 3) maxPerBets.push(problemAccount.MasterMaxBet);
        if (roleId <= 2) maxPerBets.push(problemAccount.AgentMaxBet);
        if (roleId <= 1) maxPerBets.push(problemAccount.MaxBet);

        return maxPerBets;
    }

    function renderAccountCell(problemAccount, roleId) {
        var cells = [
            '<p>', problemAccount.Headline, '</p>',
            '<p class="level-1', problemAccount.IsProblemAccSuper ? ' text-blue' : '', '">', problemAccount.Super, '</p>'
        ];

        if (roleId <= 3) {
            cells.push('<p class="level-2', problemAccount.IsProblemAccMaster ? ' text-blue' : '', '">', problemAccount.Master, '</p>');
        }

        if (roleId <= 2) {
            cells.push('<p class="level-3', problemAccount.IsProblemAccAgent ? ' text-blue' : '', '">', problemAccount.Agent, '</p>');
        }

        if (roleId <= 1) {
            cells.push('<p class="level-4', problemAccount.IsProblemAcc ? ' text-blue' : '', '">', problemAccount.Username, '</p>');
        }

        return cells.join("");
    }

    function renderMaxPerEventOrMaxPerBet(values) {
        var lastIndex = lastIndexOfMinValue(values);

        if (lastIndex === -1) {
            return "";
        }

        var cells = ['<p>&nbsp;</p>'];

        for (var i = 0, max = values.length; i < max; i++) {
            cells.push('<p class="clear-fix');
            cells.push(i === lastIndex ? " text-blue" : "");
            cells.push('">');
            cells.push('<span class="float-left">');
            cells.push(roleMapper[i]);
            cells.push('</span>');
            cells.push('<span class="float-right">');

            if (typeof values[i] === "undefined") {
                cells.push("N/A");
            } else if (values[i] === 0) {
                cells.push("0");
            } else {
                cells.push(numeral(values[i]).format(CMSConstants.NumberFormat.IntNumber));
            }

            cells.push('</span></p>');
        }

        return cells.join("");
    }

    function lastIndexOfMinValue(values) {
        var minValue = values[0];
        var lastIndex = 0;

        for (var i = 1, max = values.length; i < max; i++) {
            if (typeof minValue === "undefined" || (typeof values[i] !== "undefined" && values[i] <= minValue)) {
                minValue = values[i];
                lastIndex = i;
            }
        }

        return typeof minValue === "undefined" ? -1 : lastIndex;
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
