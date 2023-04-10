"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        cachedGridOptions = null,
        freeGrid = $("#freegrid");

    function init() {
        const ddlSite = $("#ddlSite");
        const ddlCustomerClass = $("#ddlCustomerClass");
        const ddlSportType = $("#ddlSportType");

        const searchForm = $("#formSearch").submit(function (e) {
            e.preventDefault();

            const site = ddlSite.val();
            const customerClass = ddlCustomerClass.val();
            const sportType = ddlSportType.val();
            if ((site === "All" && customerClass === "0")
                || (site === "All" && sportType === "0")
                || (customerClass === "0" && sportType === "0")) {
                CMS.showErrorBox($("#note-info-message").text());
                return;
            }

            cachedSearchUrlWithQueryString = `${searchForm.attr("action")}?${searchForm.serialize()}`;
            search(0, getPageSize());
        });
    }

    function search(pageIndex, pageSize) {
        // PageIndex of Fanex.Grid begin at 0 whereas server site begin at 1.
        CMS.getJson(
            `${cachedSearchUrlWithQueryString}&pageIndex=${pageIndex + 1}&pageSize=${pageSize}`,
            function (data) {
                assignCachedGridOptions($.parseJSON(data), pageIndex, pageSize);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                bindEventsAfterRenderGridSuccessfully();
            });
    }

    function assignCachedGridOptions(data, pageIndex, pageSize) {
        cachedGridOptions = cachedGridOptions || defaultGridOptions();

        cachedGridOptions.pagerOption.currentPage = pageIndex;
        cachedGridOptions.pagerOption.itemsPerPage = pageSize;
        cachedGridOptions.pagerOption.totalItem = data.TotalItems;

        cachedGridOptions.bodyRows = buildBodyRows(data.Rules);
    }

    function defaultGridOptions() {
        var columns = [
            { HeaderText: resources.ruleTitle, Width: 250, Name: "colRuleTitle", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
            { HeaderText: resources.customerClass, Width: 100, Name: "colCustomerClass", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.site, Width: 100, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.currency, Width: 100, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.platform, Width: 100, Name: "colPlatform", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.sportType, Width: 100, Name: "colSportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.leagueGroup, Width: 100, Name: "colLeagueGroup", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.isLive, Width: 60, Name: "colIsLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.diffBalanceRatio, Width: 60, Name: "colDiffBalanceRatio", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betDelay, Width: 60, Name: "colBetDelay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.nextSuperSiteBetIntervalSec, Width: 120, Name: "colNextSuperSiteBetIntervalSec", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.minMaxBetMPM, Width: 160, Name: "colMinMaxBetMPM", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.repeatedBets, Width: 60, Name: "colRepeatedBets", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.seizeBet, Width: 60, Name: "colSeizeBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enableParlay, Width: 60, Name: "colEnableParlay", HeaderAlign: "center", CellAlign: "center" },
            //{ HeaderText: resources.peakHourSpread, Width: 60, Name: "colPeakHourSpread", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enable, Width: 60, Name: "colEnable", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 55,
            gridExpandHeight: 180,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: search
            }
        };
    }

    function buildBodyRows(data) {
        return data.map(function (rule) {
            return {
                Columns: [
                    { Name: "colRuleTitle", Value: rule.RuleTitle, TdClass: "pink-cell" },
                    { Name: "colCustomerClass", Value: rule.CustomerClass, TdClass: "pink-cell" },
                    { Name: "colSite", Value: rule.Site, TdClass: "pink-cell" },
                    { Name: "colCurrency", Value: renderLinkWithDetailsPopup(rule.IsAllCurrencies, rule.Currencies, "link-currency"), TdClass: "pink-cell" },
                    { Name: "colPlatform", Value: renderLinkWithDetailsPopup(rule.IsAllPlatforms, rule.PlatformNames, "link-platform"), TdClass: "pink-cell" },
                    { Name: "colSportType", Value: rule.SportTypeName, TdClass: "green-cell" },
                    { Name: "colLeagueGroup", Value: renderLeagueGroupCell(rule.IsAllLeagueGroups, rule.LeagueGroups, rule.SportType), TdClass: "green-cell" },
                    { Name: "colIsLive", Value: ruleManagementHelper.renderIsLiveCell(rule.IsLive), TdClass: "green-cell" },
                    { Name: "colDiffBalanceRatio", Value: ruleManagementHelper.renderPercentCell(rule.DiffBalanceRatio) },
                    { Name: "colBetDelay", Value: ruleManagementHelper.renderPercentCell(rule.BetDelay) },
                    { Name: "colNextSuperSiteBetIntervalSec", Value: ruleManagementHelper.renderSeparateCell(rule.NextBetInterval, rule.SuperBetInterval, rule.SiteBetInterval) },
                    { Name: "colMinMaxBetMPM", Value: ruleManagementHelper.renderSeparateCell(rule.MinBet, rule.MaxBet, rule.MaxPerMatch) },
                    { Name: "colRepeatedBets", Value: rule.RepeatedBets },
                    { Name: "colSeizeBet", Value: ruleManagementHelper.renderPercentCell(rule.SeizeBet) },
                    { Name: "colEnableParlay", Value: ruleManagementHelper.renderBooleanCell(rule.EnableParlay) },
                    //{ Name: "colPeakHourSpread", Value: ruleManagementHelper.formatInteger(rule.PeakHourSpread) },
                    { Name: "colEnable", Value: ruleManagementHelper.renderBooleanCell(rule.Enable) }
                ]
            };
        });
    }

    function renderLinkWithDetailsPopup(isAll, data, className) {
        if (isAll) {
            return `<a href="javascript:;" class="${className}" data-details="${data}">${resources.all}</a>`;
        }

        const numOfData = data.split(",").length;
        return numOfData < 2
            ? data
            : `<a href="javascript:;" class="${className}" data-details="${data}">${numOfData}</a>`;
    }

    function renderLeagueGroupCell(isAllLeagueGroups, leagueGroups, sportType) {
        if (isAllLeagueGroups) {
            return `<a href="javascript:;" class="link-league-group" data-sporttype="${sportType}">${resources.all}</a>`;
        }

        const numOfLeagueGroups = leagueGroups.split(",").length;
        return numOfLeagueGroups < 2
            ? leagueGroups
            : `<a href="javascript:;" class="link-league-group" data-leaguegroups="${leagueGroups}">${numOfLeagueGroups}</a>`;
    }

    function bindEventsAfterRenderGridSuccessfully() {
        freeGrid.find(".link-currency")
            .off("click")
            .click(function () {
                handleDetailsPopup(resources.currency, $(this));
            });
        freeGrid.find(".link-platform")
            .off("click")
            .click(function () {
                handleDetailsPopup(resources.platform, $(this));
            });
        freeGrid.find(".link-league-group")
            .off("click")
            .click(handleViewLeagueGroups);
    }

    function handleDetailsPopup(columnName, element) {
        let html = [
            '<div class="form" id="viewDetailsForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', columnName, '</th></tr></thead>',
            '<tbody>'
        ];
        element.data("details").split(",").forEach(function (item, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            columnName,
            html.join(""),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });
    }

    function handleViewLeagueGroups() {
        let html = [
            '<div class="form" id="viewDetailsForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', resources.leagueGroup, '</th></tr></thead>',
            '<tbody>'
        ];
        const $this = $(this);
        const sportType = $this.data("sporttype");
        if (sportType) {
            let index = 0;
            leagueGroupList.forEach(function (item) {
                if (item.SportType === sportType || item.SportType === 0) {
                    html.push('<tr><td class="align-center">', ++index, '</td><td>', item.Name, '</td></tr>');
                }
            });
        } else {
            $this.data("leaguegroups").split(",").forEach(function (item, index) {
                html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
            });
        }

        html.push('</tbody></table></div>');

        CMS.modal(
            resources.leagueGroup,
            html.join(""),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });
    }

    function getPageSize() {
        return cachedGridOptions ? cachedGridOptions.pagerOption.itemsPerPage : 100;
    }

    $(function () {
        init();
    });
})(jQuery);