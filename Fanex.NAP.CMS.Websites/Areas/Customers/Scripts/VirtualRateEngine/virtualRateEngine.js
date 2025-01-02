"use strict";
(function ($) {
    var $freegrid = $("#freegrid");
    var gridOptions = getGridOptions();
    var totalColumns = canUpdateRule ? 7 : 6;
    var addOrUpdateRule;

    function getGridOptions() {
        var columns = [
            { HeaderText: "Priority", Width: 80, Name: "colPriority", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: "Criteria", Width: 500, Name: "colCriteria", HeaderAlign: "center" },
            { HeaderText: "Adjustment", Width: 120, Name: "colAdjustment", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 80, Name: "colStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 120, Name: "colCreatedDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created By", Width: 120, Name: "colCreatedBy", HeaderAlign: "center", CellAlign: "center" }
        ];

        if (canUpdateRule) {
            columns.push({ HeaderText: "Action", Width: 80, Name: "colAction", HeaderAlign: "center", CellAlign: "center" });
        }

        return {
            showDistinctRow: false,
            gridExpandHeight: adminPermission ? 140 : 110,
            columns: columns
        };
    }

    function getRuleListAndMatrix() {
        CMS.getJson(CMS.resolveRootUrl("Customers/VirtualRateEngine/GetRulesAndMatrix"), function (jsonString) {
            var jsonData = JSON.parse(jsonString);
            renderRuleList(jsonData.Rules);

            addOrUpdateRule = new AddOrUpdateRule($("#form-addOrUpdateRule"), getRuleList, jsonData.Matrix, adminPermission);

            // Empty the #divPopupContent since it's children was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").remove();
        });
    }

    function getRuleList() {
        CMS.getJson(CMS.resolveRootUrl("Customers/VirtualRateEngine/GetRules"), function (jsonString) {
            renderRuleList(JSON.parse(jsonString));
        });
    }

    function renderRuleList(ruleList) {
        var bodyRows = [];

        $.each(ruleList, function (index, rule) {
            bodyRows.push(renderRuleIdAndName(rule));
            bodyRows.push(renderRuleContent(rule));
        })

        gridOptions.bodyRows = bodyRows;
        $freegrid
            .removeClass("display-none")
            .fanexGrid(gridOptions);

        bindEventsAfterRenderSuccessfully();
    }

    function renderRuleIdAndName(rule) {
        return {
            Columns: [
                {
                    Name: "colPriority",
                    Value: ['<span class="grid-collapse-expand grid-expand"></span>', rule.RuleId, " - ", rule.RuleName].join(""),
                    CellAlign: "left",
                    MergeColNumber: totalColumns
                }
            ],
            CssClass: "parent-row",
            CustomAttr: 'data-ruleid="' + rule.RuleId + '"'
        };
    }

    function renderRuleContent(rule) {
        var columns = [
            { Name: "colPriority", Value: rule.Priority },
            { Name: "colCriteria", Value: renderCriteriaCell(rule) },
            { Name: "colAdjustment", Value: rule.AdjustmentCell },
            { Name: "colStatus", Value: rule.StatusName },
            { Name: "colCreatedDate", Value: rule.CreatedDate },
            { Name: "colCreatedBy", Value: rule.CreatedBy }
        ];

        if (canUpdateRule) {
            columns.push({ Name: "colAction", Value: renderActionCell(rule) });
        }

        return {
            Columns: columns,
            CssClass: "child-row-" + rule.RuleId
        };
    }

    function renderCriteriaCell(rule) {
        var html = [];
        var pushMultipleSelectionCriteria = function (criteriaName, nameList, nameListNotIn) {
            if (nameListNotIn) {
                html.push(criteriaName + "not in (" + nameListNotIn + ")");
            } else {
                html.push(criteriaName + "in (" + nameList + ")");
            }
        };

        if (rule.EnabledSiteType) {
            html.push("1. Site Type: " + rule.SiteTypeName);
            pushMultipleSelectionCriteria("&nbsp;&nbsp;&nbsp;&nbsp;Site: ", rule.SiteNameList, rule.SiteNameListNotIn);
        }

        rule.EnabledCustomerClass
            && pushMultipleSelectionCriteria("2. General Customer Class: ", rule.CustomerClassList.split(",").join(", "), rule.CustomerClassListNotIn.split(",").join(", "));

        if (rule.EnabledCustomerClassDetail) {
            html.push("3. Sport (CC): " + (rule.SportTypeCCNameListNotIn ? rule.SportTypeCCNameListNotIn : rule.SportTypeCCNameList));
            pushMultipleSelectionCriteria("&nbsp;&nbsp;&nbsp;&nbsp;Sport Customer Class: ", rule.CustomerClassDetailList.split(",").join(", "), rule.CustomerClassDetailListNotIn.split(",").join(", "));
        }

        rule.EnabledCurrentVR
            && html.push("4. Current Virtual Rate: From " + rule.VRFrom + "% To " + rule.VRTo + "%");

        rule.EnabledSport
            && pushMultipleSelectionCriteria("5. Sport: ", rule.SportTypeNameList, rule.SportTypeNameListNotIn);

        rule.EnabledBetType
            && pushMultipleSelectionCriteria("6. Bet Type: ", rule.BetTypeNameList, rule.BetTypeNameListNotIn);

        rule.EnabledEvent
            && html.push("7. Event: " + rule.LiveIndicatorName);

        if (rule.EnabledLeagueGroup) {
            pushMultipleSelectionCriteria("8. League Group: ", rule.LeagueGroupNameList, rule.LeagueGroupNameListNotIn);
            pushMultipleSelectionCriteria("&nbsp;&nbsp;&nbsp;&nbsp;League: ", rule.LeagueList === "-1" ? "All" : rule.LeagueList, rule.LeagueListNotIn);
        }

        rule.EnabledBetfrom
            && html.push("9. Bet From: " + rule.BetfromName);

        rule.EnabledStake
            && html.push("10. Stake (RM): From " + rule.StakeFrom + " To " + (rule.StakeTo === 0 ? "Unlimited" : rule.StakeTo));

        rule.EnabledTicketDoW
            && pushMultipleSelectionCriteria("11. Ticket Day of Week: ", rule.DoWNameList, rule.DoWNameListNotIn);

        rule.EnabledTicketTime
            && html.push("12. Ticket Time (GMT+8): From " + VREngineHelper.extractHourMinute(rule.BeginTime) + " To " + VREngineHelper.extractHourMinute(rule.EndTime));

        rule.EnabledTicketCur
            && pushMultipleSelectionCriteria("13. Currency: ", rule.CurrencyNameList, rule.CurrencyNameListNotIn);

        rule.EnabledPreMatchPeriod
            && html.push("14. Pre-match Period: " + (rule.PreMatchPeriod === 1 ? "1 hour" : `${rule.PreMatchPeriod} hours`));

        return html.join("<br/>");
    }

    function renderActionCell(rule) {
        var html = [];

        if (canUpdateRule) {
            html.push('<span class="ico-edit" title="Update" data-ruleid="');
            html.push(rule.RuleId);
            html.push('"></span>');
        }

        if (adminPermission) {
            html.push('&nbsp;&nbsp;<span class="ico-delete" title="Delete" data-ruleid="');
            html.push(rule.RuleId);
            html.push('"></span>');
        }

        return html.join("");
    }

    function bindEventsAfterRenderSuccessfully() {
        $freegrid.find(".parent-row")
            .off("click")
            .on("click", collapseExpandRowHandler);

        $freegrid.find(".ico-edit")
            .off("click")
            .on("click", updateRuleHandler);

        $freegrid.find(".ico-delete")
            .off("click")
            .on("click", deleteRuleHandler);
    }

    function collapseExpandRowHandler() {
        var $this = $(this);
        var trChildRow = $freegrid.find(".child-row-" + $this.data("ruleid"));
        var iconCollapseExpand = $this.find(".grid-collapse-expand");
        if (iconCollapseExpand.hasClass("grid-expand")) {
            trChildRow.addClass("display-none");
            iconCollapseExpand
                .removeClass("grid-expand")
                .addClass("grid-collapse");
        } else {
            trChildRow.removeClass("display-none");
            iconCollapseExpand
                .removeClass("grid-collapse")
                .addClass("grid-expand");
        }

        $freegrid.fanexGrid("resizeGrid");
    }

    function deleteRuleHandler() {
        var ruleId = $(this).data("ruleid");
        CMS.showConfirmMessage(
            "Are you sure you want to delete the selected title?",
            function () {
                CMS.disableInput();
                CMS.callAjax(
                    this,
                    CMS.resolveRootUrl("Customers/VirtualRateEngine/DeleteRule"),
                    { ruleId: ruleId },
                    function (jsonString) {
                        var result = JSON.parse(jsonString);
                        if (result.Code) {
                            CMS.showErrorMessage(result.Message);
                        } else {
                            CMS.showSuccessMessage(result.Message, getRuleList);
                        }
                    });
            });
    }

    function updateRuleHandler() {
        var $this = $(this);
        var ruleId = $this.data("ruleid");
        CMS.getJson(
            CMS.resolveRootUrl("Customers/VirtualRateEngine/GetRule?ruleId=" + ruleId),
            function (jsonString) {
                if (jsonString) {
                    addOrUpdateRule.openModal("Update Rule", JSON.parse(jsonString));
                } else {
                    CMS.showErrorMessage("The rule does not exist.");
                }
            });
    }

    $(function () {
        getRuleListAndMatrix();

        $("#btnAddNewRule").click(function () {
            addOrUpdateRule.openModal("Add New Rule");
        });
    });
})(jQuery);