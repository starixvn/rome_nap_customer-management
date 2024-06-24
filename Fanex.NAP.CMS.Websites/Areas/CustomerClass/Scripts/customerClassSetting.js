"use strict";

(function ($) {
    let cachedFilter = null,
        cachedGridOptions = null,
        cachedLicenseeSites = [],
        cachedLicenseeSitesName = [],
        cachedCreditSites = [],
        cachedCreditSitesName = [],
        isMultiSite = 0,
        freeGrid = $("#freegrid");

    function init() {
        const txtFilterCustomerClass = $("#txtFilterCustomerClass");
        const txtFilterCustomerClassDetail = $("#txtFilterCustomerClassDetail");
        txtFilterCustomerClass.blur(() => txtFilterCustomerClass.val(customerClassHelper.replaceCharsCommaInput(txtFilterCustomerClass.val())));
        txtFilterCustomerClassDetail.blur(() => txtFilterCustomerClassDetail.val(customerClassHelper.replaceCharsCommaInput(txtFilterCustomerClassDetail.val())));
        txtFilterCustomerClass.keydown((e) => customerClassHelper.handleNumberCommanTextInput(e));
        txtFilterCustomerClassDetail.keydown((e) => customerClassHelper.handleNumberCommanTextInput(e));
        const ddlSportType = $("#ddlSportType");
        getCreditAndLicenseeSites();
        renderCurrencies();

        const ddlSite = customerClassHelper.renderMultiSelect(
            "#ddlSites",
            {
                minWidth: 180,
                showFilter: true,
                showTooltip: false,
                listData: customerClassHelper.buildGroupSiteList(true, true, "All", cachedCreditSites, cachedLicenseeSites)
            });

        const searchForm = $("#formSearch").submit(function (e) {
            e.preventDefault();

            const site = customerClassHelper.getSelectedTextReplaceAllValue("ddlSites", "All", siteList.length);
            const customerClass = txtFilterCustomerClass.val();
            const customerClassDetail = txtFilterCustomerClassDetail.val();
            var sportTypes = ddlSportType.val();
            if (sportTypes === "0") {
                sportTypes = sportList.map(sp => sp.Value).join(',');
            }

            cachedFilter = {
                Site: site,
                CustomerClass: customerClass,
                CustomerClassDetail: customerClassDetail,
                SportTypes: sportTypes,
                currencies: getSelectedReplaceAllValue("ddlCurrencies", "0", currencyList.length),
                PageIndex: 0,
                PageSize: 100
            };
            search(0, getPageSize());
        });
    }

    function getSelectedReplaceAllValue(divId, allvalue, allItemsLength) {
        var selectedItems = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedItems.length === allItemsLength) {
            return allvalue;
        }

        return selectedItems.join();
    }

    function renderCurrencies() {
        let optionList = [];
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: true, enable: true })
        );

        customerClassHelper.renderMultiSelect(
            "#ddlCurrencies",
            {
                minWidth: 120,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }

    function getCreditAndLicenseeSites() {
        let creditOrLicensee, creditOrLicenseeName;
        cachedCreditSites = [];
        cachedLicenseeSites = [];
        siteList.forEach(function (item) {
            creditOrLicensee = item.IsLicensee ? cachedLicenseeSites : cachedCreditSites;
            creditOrLicenseeName = item.IsLicensee ? cachedLicenseeSitesName : cachedCreditSitesName;
            creditOrLicensee.push({
                value: item.Id,
                text: item.Name,
                checked: false,
                enable: true
            });
            creditOrLicenseeName.push(item.Name);
        });
    }

    function search(pageIndex, pageSize) {
        // PageIndex of Fanex.Grid begin at 0 whereas server site begin at 1.
        cachedFilter.PageIndex = pageIndex + 1;
        cachedFilter.PageSize = pageSize;
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CustomerClassSetting/Search`),
            cachedFilter,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message) {
                    CMS.showErrorMessage(result.Message);
                    assignCachedGridOptions({ TotalItems: 0, Rules: [] }, 0, 100);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                } else {
                    assignCachedGridOptions(result, pageIndex, pageSize);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                    bindEventsAfterRenderGridSuccessfully();
                }
            },
            function (error) {
                assignCachedGridOptions({ TotalItems: 0, Rules: []}, 0, 100);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            },
            function () {
                CMS.enableInput();
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
        var groupHeaderRows = [
            {
                Columns: [
                    { HeaderText: "Conditions", Name: "colRuleTitle", MergeColNumber: 5, ThClass: "header-main" },
                    { HeaderText: "Apply To", Name: "colLeagueGroup", MergeColNumber: 2, ThClass: "header-main" },
                    { HeaderText: "Restriction Settings", Name: "colDiffBalanceRatio", MergeColNumber: 8, ThClass: "header-main" }
                ]
            }
        ];
        var columns = [
            { HeaderText: resources.ruleTitle, Width: 250, Name: "colRuleTitle", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, Sort: true, SortDirection: 'asc' },
            { HeaderText: resources.sportType, Width: 100, Name: "colSportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.generalCC, Width: 100, Name: "colGeneralCC", HeaderAlign: "center", CellAlign: "center", Sort: true, SortType: 'string' },
            { HeaderText: resources.sportCC, Width: 100, Name: "colSportCC", HeaderAlign: "center", CellAlign: "center", Sort: true, SortType: 'string' },
            { HeaderText: `${resources.site} - ${resources.currency} - ${resources.platform}`, Width: 150, Name: "colSiteCurPlat", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.leagueGroup, Width: 100, Name: "colLeagueGroup", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.isLive, Width: 60, Name: "colIsLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.diffBalanceRatio, Width: 60, Name: "colDiffBalanceRatio", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betDelay, Width: 60, Name: "colBetDelay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.nextSuperSiteBetIntervalSec, Width: 120, Name: "colNextSuperSiteBetIntervalSec", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.minMaxBetMPM, Width: 160, Name: "colMinMaxBetMPM", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.repeatedBets, Width: 60, Name: "colRepeatedBets", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.seizeBet, Width: 60, Name: "colSeizeBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.disableParlay, Width: 60, Name: "colDisableParlay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enable, Width: 60, Name: "colEnable", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            customizeHeader: true,
            groupHeaderRows: groupHeaderRows,
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
            },
            sortOption: {
                afterGridSorted: function (bodyRows, columnname, currentDirection) {
                    bindEventsAfterRenderGridSuccessfully();
                    return bodyRows;
                }
            }
        };
    }

    function buildBodyRows(data) {
        return data.map(function (rule) {
            return {
                Columns: [
                    { Name: "colRuleTitle", Value: rule.RuleTitle, TdClass: "pink-cell" },
                    { Name: "colSportType", Value: rule.SportTypeName, TdClass: "green-cell" },
                    { Name: "colGeneralCC", Value: rule.CustomerClass ? rule.CustomerClass.toString() : "", TdClass: "pink-cell" },
                    { Name: "colSportCC", Value: rule.CustomerClassDetail ? rule.CustomerClassDetail.toString() : "", TdClass: "pink-cell" },
                    { Name: "colSiteCurPlat", Value: renderLinkWithDetailsPopup(rule), TdClass: "pink-cell" },
                    { Name: "colLeagueGroup", Value: renderLeagueGroupCell(rule.IsAllLeagueGroups, rule.LeagueGroups, rule.SportType), TdClass: "green-cell" },
                    { Name: "colIsLive", Value: ruleManagementHelper.renderIsLiveCell(rule.IsLive), TdClass: "green-cell" },
                    { Name: "colDiffBalanceRatio", Value: ruleManagementHelper.renderPercentCell(rule.DiffBalanceRatio) },
                    { Name: "colBetDelay", Value: ruleManagementHelper.renderPercentCell(rule.BetDelay) },
                    { Name: "colNextSuperSiteBetIntervalSec", Value: ruleManagementHelper.renderSeparateCell(rule.NextBetInterval, rule.SuperBetInterval, rule.SiteBetInterval) },
                    { Name: "colMinMaxBetMPM", Value: ruleManagementHelper.renderSeparateCell(rule.MinBet, rule.MaxBet, rule.MaxPerMatch) },
                    { Name: "colRepeatedBets", Value: rule.RepeatedBets },
                    { Name: "colSeizeBet", Value: ruleManagementHelper.renderPercentCell(rule.SeizeBet) },
                    { Name: "colDisableParlay", Value: ruleManagementHelper.renderBooleanCell(rule.DisableParlay) },
                    { Name: "colEnable", Value: ruleManagementHelper.renderBooleanCell(rule.Enable) }
                ]
            };
        });
    }

    function renderLinkWithDetailsPopup(rule) {
        return `<a href="javascript:;" class="link-rule-detail" data-ruleid="${rule.RuleId}" data-ruletitle="${rule.RuleTitle}" data-sporttype="${rule.SportType}">View Detail</a>`;
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
        freeGrid.find(".link-league-group")
            .off("click")
            .click(handleViewLeagueGroups);

        freeGrid.find(".link-rule-detail")
            .off("click")
            .click(handleSiteCurrencyPlatformViewDetail);
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
        const $this = $(this);
        const sportType = $this.data("sporttype");

        if (sportType && !customerClassHelper.leagueBySport[sportType]) {
            customerClassHelper.getLeagueGroups(sportType, function () { OpenPopUpLeague(sportType) });
            return;
        }

        OpenPopUpLeague(sportType, $this.data("leaguegroups"));
    }

    function OpenPopUpLeague(sportType, itemleagues) {
        let html = [
            '<div class="form" id="viewDetailsForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', resources.leagueGroup, '</th></tr></thead>',
            '<tbody>'
        ];

        if (sportType) {
            let index = 0;
            customerClassHelper.leagueBySport[sportType].forEach(function (item) {
                html.push('<tr><td class="align-center">', ++index, '</td><td>', item.Name, '</td></tr>');
            });
        } else {
            itemleagues.split(",").forEach(function (item, index) {
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

    function handleSiteCurrencyPlatformViewDetail() {
        const $this = $(this);
        const ruleId = $this.data("ruleid");
        const ruleTitle = $this.data("ruletitle");
        const sportTypes = $this.data("sporttype");
        const currencies = cachedFilter.currencies;
        const request = {
            ruleId,
            sportTypes,
            currencies,
            isMultiSite
        };

        CallGetDetailRule(ruleTitle, request);
    }

    function CallGetDetailRule(ruleTitle, request) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CustomerClassSetting/RuleDetail`),
            request,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message) {
                    CMS.showErrorMessage(result.Message);
                } else {
                    OpenPopupRuleDetail(ruleTitle, result);
                    RegisterSwitchView(ruleTitle, request);
                }
            },
            null,
            function () {
                CMS.enableInput();
            });
    }

    function RegisterSwitchView(ruleTitle, request) {
        $("#btn-switch-detail")
            .off("click")
            .click(function () {
                isMultiSite = isMultiSite ? 0 : 1;
                request.isMultiSite = isMultiSite;
                CallGetDetailRule(ruleTitle, request);
            });
    }

    function OpenPopupRuleDetail(ruleTitle, ruleDetail) {
        let html = [
            '<div class="form" id="viewRuleDetailsForm">',
            `<div><b>Rule Title: </b>${ruleTitle}</div>`,
            `<div class="divSwitch"><button id="btn-switch-detail" class="btn-switch"><span class="icon-switch"></span>${isMultiSite ? "Group by Site" : "Normal View"}</button></div>`,
            '<table class="tbl-freegrid">',
            '<thead><tr>',
            '<th class="width-20" >No.</th >',
            '<th class="width-250">Sites</th>',
            isMultiSite ? '<th class="width-60">Number Of Sites</th>' : '',
            isMultiSite ? '<th class="width-250">Currency Name</th>' : '<th class="width-310">Currency Name</th>',
            '<th class="width-60">Number Of Currencies</th>',
            '<th class="width-200">Platform</th>',
            '<th class="width-60">Number Of Platforms</th>',
            '</tr></thead><tbody>'
        ];
        ruleDetail.forEach(function (item, index) {
            html.push(`<tr><td class="align-center width-20">${index + 1}</td>
                        <td class="width-250">${item.Sites.replace("AllCredit", "All Credit").replace("AllDeposit", "All Deposit")}</td>
                        ${isMultiSite ? '<td class="width-60 detail-count">' + item.NumberOfSites + '</td>': ''}
                        <td class="width-${isMultiSite ? '250' : '310'}">${item.CurrencyName}</td>
                        <td class="width-60 detail-count">${item.NumberOfCurrencies}</td>
                        <td class="width-200">${item.PlatformNames}</td>
                        <td class="width-60 detail-count">${item.NumberOfPlatform}</td></tr>`);
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            ruleTitle,
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