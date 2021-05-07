"use strict";
function VREngineHelperRuleList(hasHostFullPermission, hasHostVirtualRateEnginePermission) {
    var $freegrid = $("#freegrid");
    var gridOptions = getGridOptions();
    var totalColumns = hasHostVirtualRateEnginePermission ? 7 : 6;

    function getGridOptions() {
        var columns = [
            { HeaderText: "Priority", Width: 80, Name: "colPriority", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: "Criteria", Width: 320, Name: "colCriteria", HeaderAlign: "center" },
            { HeaderText: "Adjustment", Width: 180, Name: "colAdjustment", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 100, Name: "colStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created Date", Width: 120, Name: "colCreatedDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Created By", Width: 120, Name: "colCreatedBy", HeaderAlign: "center", CellAlign: "center" }
        ];

        if (hasHostVirtualRateEnginePermission) {
            columns.push({ HeaderText: "Action", Width: 120, Name: "colAction", HeaderAlign: "center", CellAlign: "center" });
        }

        return {
            showDistinctRow: false,
            gridExpandHeight: 140,
            columns: columns
        };
    }

    function getRuleList() {
        CMS.getJson(CMS.resolveRootUrl("Customers/VirtualRateEngine/GetRules"), function (jsonString) {
            gridOptions.bodyRows = buildBodyRows(JSON.parse(jsonString));

            $freegrid
                .removeClass("display-none")
                .fanexGrid(gridOptions);

            bindEventsAfterRenderSuccessfully();
        });
    }

    function buildBodyRows(data) {
        var bodyRows = [];

        $.each(data, function (index, datum) {
            bodyRows.push(buildGroup(datum));
            bodyRows.push(buildDataCells(datum));
        })

        return bodyRows;
    }

    function buildGroup(datum) {
        return {
            Columns: [
                {
                    Name: "colPriority",
                    Value: ['<span class="grid-collapse-expand grid-expand"></span>', datum.RuleId, " - ", datum.RuleName].join(""),
                    CellAlign: "left",
                    MergeColNumber: totalColumns
                }
            ],
            CssClass: "parent-row",
            CustomAttr: 'data-ruleid="' + datum.RuleId + '"'
        };
    }

    function buildDataCells(datum) {
        var columns = [
            { Name: "colPriority", Value: datum.Priority },
            { Name: "colCriteria", Value: renderCriteriaCell(datum) },
            { Name: "colAdjustment", Value: datum.AdjustmentCell },
            { Name: "colStatus", Value: datum.StatusName },
            { Name: "colCreatedDate", Value: datum.CreatedDate },
            { Name: "colCreatedBy", Value: datum.CreatedBy }
        ];

        if (hasHostVirtualRateEnginePermission) {
            columns.push({ Name: "colAction", Value: renderActionCell(datum) });
        }

        return {
            Columns: columns,
            CssClass: "child-row-" + datum.RuleId
        };
    }

    function renderCriteriaCell(datum) {
        var html = [];
        if (datum.EnabledSiteType) {
            html.push("1. Site Type: " + datum.SiteTypeName);
            html.push(renderMultipleSelectionCriteria("&nbsp;&nbsp;&nbsp;&nbsp;Site: ", datum.SiteNameList, datum.SiteNameListNotIn));
        }

        datum.EnabledCustomerClass
            && html.push(renderMultipleSelectionCriteria("2. Customer Class: ", datum.CustomerClassList, datum.CustomerClassListNotIn));

        datum.EnabledCurrentVR
            && html.push("3. Current Virtual Rate: From " + datum.VRFrom + "% To " + datum.VRTo + "%");

        datum.EnabledCurrentVRStatus
            && html.push("4. Current Virtual Rate Status: " + datum.VRStatusName);

        datum.EnabledSport
            && html.push(renderMultipleSelectionCriteria("5. Sport: ", datum.SportTypeNameList, datum.SportTypeNameListNotIn));

        datum.EnabledBetType
            && html.push(renderMultipleSelectionCriteria("6. Bet Type: ", datum.BetTypeNameList, datum.BetTypeNameListNotIn));

        datum.EnabledEvent
            && html.push("7. Event: " + datum.LiveIndicatorName);

        if (datum.EnabledLeagueGroup) {
            html.push(renderMultipleSelectionCriteria("8. League Group: ", datum.LeagueGroupNameList, datum.LeagueGroupNameListNotIn));
            html.push(renderMultipleSelectionCriteria("&nbsp;&nbsp;&nbsp;&nbsp;League: ", datum.LeagueList, datum.LeagueListNotIn));
        }

        datum.EnabledBetfrom
            && html.push("9. Bet From: " + datum.BetfromName);

        datum.EnabledStake
            && html.push("10. Stake (RM): From " + datum.StakeFrom + " To " + (datum.StakeTo === 0 ? "Unlimited" : datum.StakeTo));

        datum.EnabledTicketDoW
            && html.push(renderMultipleSelectionCriteria("11. Ticket Day of Week: ", datum.DateOfWeekNameList, datum.DateOfWeekNameListNotIn));

        datum.EnabledTicketTime
            && html.push("12. Ticket Time (GMT+8): From " + VREngineHelper.extractHourMinute(datum.BeginTime) + " To " + VREngineHelper.extractHourMinute(datum.EndTime));

        return html.join("<br/>");
    }

    function renderMultipleSelectionCriteria(criteriaName, nameList, nameListNotIn) {
        var html = [criteriaName];

        if (nameListNotIn) {
            html.push("not in (");
            html.push(nameListNotIn);
            html.push(")");
        } else {
            html.push("in (");
            html.push(nameList);
            html.push(")");
        }

        return html.join("");
    }

    function renderActionCell(datum) {
        var actionCell = '';

        if (hasHostVirtualRateEnginePermission) {
            actionCell = '<span class="ico-edit" title="Edit"></span>';
        }

        if (hasHostFullPermission) {
            actionCell += '&nbsp;&nbsp;<span class="ico-delete" title="Delete" data-ruleid="' + datum.RuleId + '"></span>';
        }

        return actionCell;
    }

    function bindEventsAfterRenderSuccessfully() {
        $freegrid.find(".parent-row")
            .off("click")
            .on("click", collapseExpandRowHandler);

        $freegrid.find(".ico-edit")
            .off("click")
            .on("click", function () {
            });

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

    this.getRuleList = getRuleList;
}