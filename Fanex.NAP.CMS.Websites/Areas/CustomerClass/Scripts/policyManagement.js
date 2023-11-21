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
            { HeaderText: resources.criteria, Width: 600, Name: "colCriteria", HeaderAlign: "center", CellAlign: "left" },
        ];

        hasHostPermisson && columns.push({ HeaderText: resources.delete + '<br/><input type="checkbox" id="chk-select-delete-all" class="ver-mid"><span id="btn-delete-selected" class="ico-trash-white ver-mid" title="Delete"></span>', Width: 80, Name: "colDelete", HeaderAlign: "center", CellAlign: "center" });
        
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

            const row = {
                Columns: [{
                    Name: "colRuleTitle",
                    Value: renderPolicyGroupCell(policy),
                    CellAlign: "left",
                    MergeColNumber: 2
                }],
                SubRows: subRows
            };
            hasHostPermisson && row.Columns.push({
                Name: "colDelete",
                Value: renderDeleteCell(policy.PolicyId),
                CellAlign: "center"
            });

            return row;
        });
    }

    function renderPolicyGroupCell(policy) {
        let html = ['<span style="line-height:1.6">', policy.PolicyTitle, '</span>'];
        if (hasHostPermisson) {
            html.push('<span class="ico-edit float-right margin-right" data-policyid="', policy.PolicyId, '"></span>');
        }

        return html.join("");
    }


    function renderDeleteCell(id) {
        return `<input type="checkbox" class="chk-select-delete" data-id="${id}">`;
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
        var policyIds = [];
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") === true) {
                policyIds.push($(this).data("id"));
            }
        });
        if (policyIds.length === 0) {
            return;
        }
        callCheckPolicyBeforeDelete(policyIds.join());
    }

    function callCheckPolicyBeforeDelete(policies) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Policy/CheckPolicies`),
            { Policies: policies },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    var message = `Are you sure you want to delete the selected setting row(s)?`;
                    CMS.showConfirmMessage(message, function () { callDelete(policies); });
                } else {
                    if (resultJson.Policies === "") {
                        CMS.showErrorMessage(resultJson.Message);
                    } else {
                        var warningMessage = `The following policy(es) have been applied to the sites - currencies:</br>${resultJson.Policies}.</br>Are you sure you want to delete them?</br>Note: If you delete them, their assign settings with Site - Currency will also be deleted.`;
                        CMS.showConfirmMessage(warningMessage, function () { callDelete(policies); });
                    }
                }
            });
    }


    function callDelete(policies) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Policy/DeletePolicies`),
            { Policies: policies },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    search();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    $(function () {
        init();
    });
})(jQuery);