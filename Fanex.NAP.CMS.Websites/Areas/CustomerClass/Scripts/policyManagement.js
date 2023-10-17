"use strict";

(function ($) {
    let cachedGridOptions = null,
        freeGrid = $("#freegrid"),
        hasHostPermisson = null;

    function init() {
        const btnAddNew = $("#btnAddNew");
        hasHostPermisson = btnAddNew.length > 0;
        if (hasHostPermisson) {
            policyManagementAddOrEdit.init(getPolicies);

            btnAddNew.click(function () {
                policyManagementAddOrEdit.openModal();
            });
        }

        getPolicies();
    }

    function getPolicies() {
        CMS.getJson(
            CMS.resolveRootUrl("Policy/GetPolicies"),
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
        var columns = [
            { HeaderText: resources.ruleTitle, Width: 300, Name: "colRuleTitle", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
            { HeaderText: resources.criteria, Width: 600, Name: "colCriteria", HeaderAlign: "center", CellAlign: "left" }
        ];

        return {
            showTitle: true,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 130,
            columns: columns,
            titleInfo:
            {
                showCollapseExpandButton: true
            },
            showPager: false,
            showTree: true
        };
    }

    function buildBodyRows(data) {
        return data.map(function (policy) {
            const subRows = policy.Rules.map(function (rule) {
                return {
                    Columns: [
                        { Name: "colRuleTitle", Value: rule.RuleTitle },
                        { Name: "colCriteria", Value: policyManagementHelper.renderCriteriaCell(rule), TdClass: "cell-criteria" }
                    ]
                };
            });

            return {
                Columns: [{
                    Name: "colRuleTitle",
                    Value: renderPolicyGroupCell(policy),
                    CellAlign: "left",
                    MergeColNumber: 2
                }],
                SubRows: subRows
            };
        });
    }

    function renderPolicyGroupCell(policy) {
        let html = ['<span style="line-height:1.6">', policy.PolicyTitle, '</span>'];
        if (hasHostPermisson) {
            html.push('<span class="ico-edit float-right margin-right" data-policyid="', policy.PolicyId, '"></span>');
        }

        return html.join("");
    }

    function bindEventsAfterRenderGridSuccessfully() {
        $(".ico-edit")
            .off("click")
            .click(function (e) {
                e.stopPropagation();
                CMS.getJson(
                    CMS.resolveRootUrl(`Policy/Get?policyId=${$(this).data("policyid")}`),
                    function (data) {
                        policyManagementAddOrEdit.openModal(JSON.parse(data));
                    });
            });
    }

    $(function () {
        init();
    });
})(jQuery);