"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        ddlLeagueGroupBox = $("#ddlLeagueGroupBox"),
        ddlLeagueGroup = null,
        cachedGridOptions = null,
        freeGrid = $("#freegrid"),
        hasHostPermisson = null;

    function init() {
        const ddlCustomerClass = $("#ddlCustomerClass");
        const ddlSportType = $("#ddlSportType")
            .change(renderLeagueGroup)
            .trigger("change");
        const ddlIndicator = $("#ddlIndicator");

        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClass: ddlCustomerClass.val(),
                sportType: ddlSportType.val(),
                leagueGroups: ddlLeagueGroup.fanexMultiSelect("getItems", true).join(),
                isLive: ddlIndicator.val()
            };
            cachedSearchUrlWithQueryString = `${this.action}?${$.param(formData)}`;
            search();
        }).trigger("submit");

        const btnAddNewRule = $("#btnAddNewRule");
        hasHostPermisson = btnAddNewRule.length > 0;
        if (hasHostPermisson) {
            ruleManagementAddOrEdit.init(search);

            btnAddNewRule.click(function () {
                ruleManagementAddOrEdit.openModal(ruleManagementAddOrEdit.ADD_NEW);
            });
        }
    }

    function renderLeagueGroup() {
        const sportType = +this.value;
        let leagueGroups = [];
        leagueGroupList.forEach(item =>
            (item.SportType === sportType || item.SportType === 0)
            && leagueGroups.push({ value: item.Id, text: item.Name, checked: true, enable: true })
        );

        ddlLeagueGroupBox.html('<div id="ddlLeagueGroup"></div>');
        ddlLeagueGroup = customerClassHelper.renderMultiSelect(
            "#ddlLeagueGroup",
            {
                minWidth: 240,
                listData: leagueGroups
            }
        );
    }

    function search() {
        CMS.getJson(
            cachedSearchUrlWithQueryString,
            function (data) {
                cachedGridOptions = cachedGridOptions || defaultGridOptions();
                cachedGridOptions.bodyRows = buildBodyRows(JSON.parse(data));

                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                hasHostPermisson && bindEventsAfterRenderGridSuccessfully();
            },
            function (error) {
                cachedGridOptions = defaultGridOptions();
                cachedGridOptions.bodyRows = buildBodyRows([]);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            });
    }

    function defaultGridOptions() {
        const columns = [
            { HeaderText: resources.ruleTitle, Width: 250, Name: "colRuleTitle", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
            { HeaderText: resources.customerClass, Width: 100, Name: "colCustomerClass", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.sportType, Width: 100, Name: "colSportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.leagueGroup, Width: 100, Name: "colLeagueGroup", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.isLive, Width: 60, Name: "colIsLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.diffBalanceRatio, Width: 60, Name: "colDiffBalanceRatio", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betDelay, Width: 60, Name: "colBetDelay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.nextSuperSiteBetIntervalSec, Width: 120, Name: "colNextSuperSiteBetIntervalSec", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.minMaxBetMPM, Width: 160, Name: "colMinMaxBetMPM", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.repeatedBets, Width: 60, Name: "colRepeatedBets", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.seizeBet, Width: 60, Name: "colSeizeBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.disableParlay, Width: 60, Name: "colDisableParlay", HeaderAlign: "center", CellAlign: "center" },
            //{ HeaderText: resources.peakHourSpread, Width: 60, Name: "colPeakHourSpread", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enable, Width: 60, Name: "colEnable", HeaderAlign: "center", CellAlign: "center" }
        ];
        hasHostPermisson && columns.push({ HeaderText: resources.action, Width: 60, Name: "colAction", HeaderAlign: "center", CellAlign: "center" });

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 55,
            gridExpandHeight: 160,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(data) {
        return data.map(function (rule) {
            const columns = [
                { Name: "colRuleTitle", Value: rule.RuleTitle, TdClass: "pink-cell" },
                { Name: "colCustomerClass", Value: rule.CustomerClass, TdClass: "pink-cell" },
                { Name: "colSportType", Value: rule.SportTypeName, TdClass: "green-cell" },
                { Name: "colLeagueGroup", Value: renderLeagueGroupCell(rule.IsAllLeagueGroups, rule.LeagueGroups, rule.SportType), TdClass: "green-cell" },
                { Name: "colIsLive", Value: ruleManagementHelper.renderIsLiveCell(rule.IsLive), TdClass: "green-cell" },
                { Name: "colDiffBalanceRatio", Value: ruleManagementHelper.renderPercentCell(rule.DiffBalanceRatio) },
                { Name: "colBetDelay", Value: ruleManagementHelper.renderPercentCell(rule.BetDelay) },
                { Name: "colNextSuperSiteBetIntervalSec", Value: ruleManagementHelper.renderSeparateCell(rule.NextBetInterval, rule.SuperBetInterval, rule.SiteBetInterval) },
                { Name: "colMinMaxBetMPM", Value: ruleManagementHelper.renderSeparateCell(rule.MinBet, rule.MaxBet, rule.MaxPerMatch) },
                { Name: "colRepeatedBets", Value: rule.RepeatedBets },
                { Name: "colSeizeBet", Value: ruleManagementHelper.renderPercentCell(rule.SeizeBet) },
                { Name: "colDisableParlay", Value: ruleManagementHelper.renderBooleanCell(rule.DisableParlay) },
                //{ Name: "colPeakHourSpread", Value: ruleManagementHelper.formatInteger(rule.PeakHourSpread) }
            ];

            hasHostPermisson
                ? columns.push(
                    { Name: "colEnable", Value: renderEnableCell(rule.RuleId, rule.RuleTitle, rule.Enable) },
                    { Name: "colAction", Value: renderActionCell(rule.RuleId) })
                : columns.push({ Name: "colEnable", Value: ruleManagementHelper.renderBooleanCell(rule.Enable) });

            return { Columns: columns };
        });
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

    function renderEnableCell(ruleId, ruleTitle, enable) {
        return [
            '<input type="checkbox" class="checkbox-enable" data-ruleid="',
            ruleId,
            '" data-ruletitle="',
            ruleTitle,
            '" ',
            enable ? "checked" : "",
            '/>'
        ].join("");
    }

    function renderActionCell(ruleId) {
        return [
            '<span class="ico-edit mid-margin-right" title="Edit" data-ruleid="', ruleId, '"></span>',
            '<span class="ico-copy" title="Clone" data-ruleid="', ruleId, '"></span>'
        ].join("");
    }

    function bindEventsAfterRenderGridSuccessfully() {
        freeGrid.find(".link-league-group")
            .off("click")
            .click(handleViewLeagueGroups);

        freeGrid.find(".checkbox-enable")
            .off("change")
            .change(handleEnableRule);

        freeGrid.find(".ico-edit")
            .off("click")
            .click(function () {
                handleGetRuleById($(this).data("ruleid"), ruleManagementAddOrEdit.EDIT);
            });

        freeGrid.find(".ico-copy")
            .off("click")
            .click(function () {
                handleGetRuleById($(this).data("ruleid"), ruleManagementAddOrEdit.CLONE);
            });
    }

    function handleViewLeagueGroups() {
        let html = [
            '<div class="form" id="viewLeagueGroupsForm">',
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

    function handleEnableRule() {
        const isChecked = this.checked;
        const $this = $(this);
        const confirmMessage = [
            'Are you sure you want to ',
            isChecked ? '<strong class="text-blue">ENABLE</strong>' : '<strong class="text-red">DISABLE</strong>',
            ' rule title "',
            $this.data("ruletitle"),
            '"?'
        ].join("");
        this.checked = !isChecked;

        CMS.showConfirmMessage(confirmMessage, function () {
            CMS.disableInput();
            CMS.callAjax(
                this,
                CMS.resolveRootUrl("Rule/Enable"),
                { ruleId: $this.data("ruleid"), enable: isChecked },
                function (result) {
                    const resultJson = JSON.parse(result);
                    if (resultJson.IsSuccess) {
                        $this.prop("checked", isChecked);
                    } else {
                        CMS.showErrorMessage(resultJson.Message);
                    }
                });
        });
    }

    function handleGetRuleById(ruleId, action) {
        CMS.getJson(
            CMS.resolveRootUrl(`Rule/Get?ruleId=${ruleId}`),
            function (data) {
                ruleManagementAddOrEdit.openModal(action, JSON.parse(data));
            });
    }

    $(function () {
        init();
    });
})(jQuery);