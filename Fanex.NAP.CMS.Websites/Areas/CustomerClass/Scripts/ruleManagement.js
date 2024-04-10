"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        ddlLeagueGroupBox = $("#ddlLeagueGroupBox"),
        ddlLeagueGroup = null,
        cachedGridOptions = null,
        freeGrid = $("#freegrid"),
        hasHostPermisson = null,
        isFirstLoad = true;
    

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
        });

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
        if (customerClassHelper.leagueBySport[sportType]) {
            BuildDropDownLeague(sportType);
        } else {
            customerClassHelper.getLeagueGroups(sportType, function () { BuildDropDownLeague(sportType); });
        }
    }

    function BuildDropDownLeague(sportType) {
        let leagueGroups = [];
        customerClassHelper.leagueBySport[sportType].forEach(function (item) {
            leagueGroups.push({
                value: item.Id,
                text: item.Name,
                checked: true,
                enable: true
            });
        });

        ddlLeagueGroupBox.html('<div id="ddlLeagueGroup"></div>');
        ddlLeagueGroup = customerClassHelper.renderMultiSelect(
            "#ddlLeagueGroup",
            {
                minWidth: 240,
                listData: leagueGroups
            }
        );

        if (isFirstLoad) {
            $("#formSearch").trigger("submit");
            isFirstLoad = false;
        }
    }

    function search() {
        CMS.getJson(
            cachedSearchUrlWithQueryString,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message) {
                    CMS.showErrorMessage(result.Message);
                    cachedGridOptions = defaultGridOptions();
                    cachedGridOptions.bodyRows = buildBodyRows([]);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                } else {
                    cachedGridOptions = cachedGridOptions || defaultGridOptions();
                    cachedGridOptions.bodyRows = buildBodyRows(result);

                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                    freeGrid.find(".link-league-group")
                        .off("click")
                        .click(handleViewLeagueGroups);
                    hasHostPermisson && bindEventsAfterRenderGridSuccessfully();
                }
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
            { HeaderText: resources.diffBalanceRatio, Width: 80, Name: "colDiffBalanceRatio", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betDelay, Width: 60, Name: "colBetDelay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.nextSuperSiteBetIntervalSec, Width: 120, Name: "colNextSuperSiteBetIntervalSec", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.minMaxBetMPM, Width: 160, Name: "colMinMaxBetMPM", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.repeatedBets, Width: 60, Name: "colRepeatedBets", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.seizeBet, Width: 60, Name: "colSeizeBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.policyTitle, Width: 160, Name: "colPolicies", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.disableParlay, Width: 60, Name: "colDisableParlay", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enable, Width: 60, Name: "colEnable", HeaderAlign: "center", CellAlign: "center" }
        ];
        hasHostPermisson && columns.push({ HeaderText: resources.action, Width: 60, Name: "colAction", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.delete + '<br/><input type="checkbox" id="chk-select-delete-all" class="ver-mid"><span id="btn-delete-selected" class="ico-trash-white ver-mid" title="Delete"></span>', Width: 80, Name: "colDelete", HeaderAlign: "center", CellAlign: "center" });

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
                { Name: "colPolicies", Value: renderPolicyCell(rule.PolicyTitles) },
                { Name: "colDisableParlay", Value: ruleManagementHelper.renderBooleanCell(rule.DisableParlay) }
            ];

            hasHostPermisson
                ? columns.push(
                    { Name: "colEnable", Value: renderEnableCell(rule.RuleId, rule.RuleTitle, rule.SportType, rule.Enable) },
                    { Name: "colAction", Value: renderActionCell(rule.RuleId, rule.SportType) },
                    { Name: "colDelete", Value: renderDeleteCell(rule.RuleId, rule.SportType) })
                : columns.push({ Name: "colEnable", Value: ruleManagementHelper.renderBooleanCell(rule.Enable) });

            return { Columns: columns };
        });
    }

    function renderLeagueGroupCell(isAllLeagueGroups, leagueGroups, sportType) {
        if (isAllLeagueGroups) {
            return `<a href="javascript:;" class="link-league-group" data-sporttype="${sportType}" data-title="${resources.leagueGroup}">${resources.all}</a>`;
        }

        const numOfLeagueGroups = leagueGroups.split(",").length;
        return numOfLeagueGroups < 2
            ? leagueGroups
            : `<a href="javascript:;" class="link-league-group" data-leaguegroups="${leagueGroups}" data-title="${resources.leagueGroup}">${numOfLeagueGroups}</a>`;
    }

    function renderPolicyCell(policyTitles) {
        var policies = policyTitles.split(',');
        const numOfPolicies = policies.length;
        return numOfPolicies < 2
            ? policyTitles
            : `<a href="javascript:;" class="link-league-group" data-leaguegroups="${policies}" data-title="${resources.policyTitle}">${numOfPolicies}</a>`;
    }

    function renderEnableCell(ruleId, ruleTitle, sportType, enable) {
        return `<input type="checkbox" class="checkbox-enable" data-ruleid="${ruleId}" data-ruletitle="${ruleTitle}" data-sporttype="${sportType}" ${enable ? "checked" : ""}>`;
    }

    function renderActionCell(ruleId, sportType) {
        return [
            `<span class="ico-edit mid-margin-right" title="Edit" data-ruleid="${ruleId}" data-sporttype="${sportType}"></span>`,
            '<span class="ico-copy" title="Clone" data-ruleid="', ruleId, '"></span>'
        ].join("");
    }


    function renderDeleteCell(id, sportType) {
        return `<input type="checkbox" class="chk-select-delete" data-id="${id}" data-sporttype="${sportType}">`;
    }

    function bindEventsAfterRenderGridSuccessfully() {
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

        freeGrid.find("#chk-select-delete-all")
            .off("click")
            .click(handleCheckDelAll);
        freeGrid.find("#btn-delete-selected")
            .off("click")
            .click(handleDelSelected);
        freeGrid.find(".chk-select-delete")
            .off("click")
            .click(handleSingleCheckDel);
    }

    function handleViewLeagueGroups() {
        const $this = $(this);
        let html = [
            '<div class="form" id="viewLeagueGroupsForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', $this.data("title"), '</th></tr></thead>',
            '<tbody>'
        ];
        const sportType = $this.data("sporttype");
        if (sportType) {
            let index = 0;
            customerClassHelper.leagueBySport[sportType].forEach(function (item) {
                html.push('<tr><td class="align-center">', ++index, '</td><td>', item.Name, '</td></tr>');
            });
        } else {
            $this.data("leaguegroups").split(",").forEach(function (item, index) {
                html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
            });
        }

        html.push('</tbody></table></div>');

        CMS.modal(
            $this.data("title"),
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
                { ruleId: $this.data("ruleid"), enable: isChecked, sportType: $this.data("sporttype")},
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

    function handleCheckDelAll() {
        let checked = $(this).prop("checked");
        $(".chk-select-delete").each(function () {
            $(this).prop("checked", checked);
        });
    }

    function handleSingleCheckDel() {
        let isCheckAll = true;
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") !== true) {
                isCheckAll = false;

                return false;
            }
        });

        $("#chk-select-delete-all").prop("checked", isCheckAll);
    }

    function handleDelSelected() {
        var ruleIds = [];
        var sportTypes = [];
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") === true) {
                ruleIds.push($(this).data("id"));
                if (!sportTypes.includes($(this).data("sporttype"))) {
                    sportTypes.push($(this).data("sporttype"));
                }
            }
        });
        if (ruleIds.length === 0) {
            return;
        }

        callCheckRuleBeforeDelete(ruleIds, sportTypes);
    }


    function callCheckRuleBeforeDelete(ruleIds, sportTypes) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Rule/CheckRules`),
            {
                RuleIDs: ruleIds.join(),
                SportTypes: sportTypes
            },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.Rules && resultJson.Rules !== "") {
                    var warningMessage = `The following rule(s) have been added to the policies:</br>${resultJson.Rules}.</br>Are you sure you want to delete them?</br>Note: If you delete them, they will also be removed from policies.`
                    CMS.showConfirmMessage(warningMessage, function () { callDelete(ruleIds, sportTypes); });
                } else {
                    if (resultJson.IsSuccess) {
                        var message = `Are you sure you want to delete the selected setting row(s)?`;
                        CMS.showConfirmMessage(message, function () { callDelete(ruleIds, sportTypes); });
                    } else {
                        CMS.showErrorMessage(resultJson.Message);
                        CMS.enableInput();
                    }
                }
            });
    }


    function callDelete(ruleIds, sportTypes) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Rule/DeleteRules`),
            {
                RuleIDs: ruleIds.join(),
                SportTypes: sportTypes
            },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    search();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                    CMS.enableInput();
                }
            });
    }

    $(function () {
        init();
    });
})(jQuery);