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

        $("#formSearch").submit(function (e) {
            e.preventDefault();
            getPolicies();
        }).trigger("submit");
    }

    function getPolicies() {
        var policyTitle = encodeURIComponent($("#filterPolicy").val());
        var sportType = $("#ddlSportTypeFilter").val();
        if (sportType === "0") {
            sportType = sportList.map(sp => sp.Value).join(',');
        }
        CMS.getJson(
            CMS.resolveRootUrl(`Policy/GetPolicies?policyTitle=${policyTitle}&sportTypes=${sportType}&sortOrder=asc&sortField=RuleTitle`),
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
                    hasHostPermisson && bindEventsAfterRenderGridSuccessfully("asc", "colRuleTitle");
                }
            },
            function (error) {
                cachedGridOptions = defaultGridOptions();
                cachedGridOptions.bodyRows = buildBodyRows([]);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            });
    }

    function defaultGridOptions() {
        var columns = [
            { HeaderText: resources.ruleTitle, Width: 300, Name: "colRuleTitle", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true, Sort: true },
            { HeaderText: resources.criteria, Width: 600, Name: "colCriteria", HeaderAlign: "center", CellAlign: "left", Sort: true }
        ];

        hasHostPermisson && columns.push({ HeaderText: resources.delete + '<br/><input type="checkbox" id="chk-select-delete-all" class="ver-mid"><span id="btn-delete-selected" class="ico-trash-white ver-mid" title="Delete"></span>', Width: 80, Name: "colDelete", HeaderAlign: "center", CellAlign: "center" });
        
        return {
            showTitle: true,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 180,
            columns: columns,
            titleInfo:
            {
                text: resources.note,
                showCollapseExpandButton: true
            },
            showPager: false,
            sortOption: {
                //customGridSort: customerPolicyGridSort
            },
            showTree: true
        };
    }

    function buildBodyRows(data) {
        return data.map(function (policy) {
            const subRows = policy.Rules  ? policy.Rules.map(function (rule) {
                return {
                    Columns: [
                        { Name: "colRuleTitle", Value: rule.RuleTitle },
                        { Name: "colCriteria", Value: policyManagementHelper.renderCriteriaCell(rule), TdClass: "cell-criteria" }
                    ]
                };
            }) : [];

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
                Value: renderDeleteCell(policy.PolicyId, policy.SportType),
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


    function renderDeleteCell(id, sportType) {
        return `<input type="checkbox" class="chk-select-delete" data-id="${id}" data-sporttype="${sportType}">`;
    }

    function bindEventsAfterRenderGridSuccessfully(sortOrder, sortColumn) {
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
        freeGrid.off('click', '#freegrid_header_sort_colRuleTitle', null);
        freeGrid.off('click', '#freegrid_header_sort_colCriteria', null);
        freeGrid.find("#freegrid_header_sort_colRuleTitle")
            .off("click")
            .click(handleSort);
        freeGrid.find("#freegrid_header_sort_colCriteria")
            .off("click")
            .click(handleSort);
        freeGrid.find(`#freegrid_header_sort_${sortColumn}`).find('.divSorter').addClass(sortOrder);
    }

    function handleSort() {
        var $this = $(this).find('.divSorter');
        var policyTitle = encodeURIComponent($("#filterPolicy").val());
        var sportType = $("#ddlSportTypeFilter").val();
        if (sportType === "0") {
            sportType = sportList.map(sp => sp.Value).join(',');
        }
        var nextSortOrder = $this.hasClass("asc") ? "desc" : "asc";
        var sortType = $this.data("columnname");
        var sortField = sortType === "colRuleTitle" ? "RuleTitle" : "CustomerClass";
        CMS.disableInput();
        CMS.getJson(
            CMS.resolveRootUrl(`Policy/GetPolicies?policyTitle=${policyTitle}&sportTypes=${sportType}&sortOrder=${nextSortOrder}&sortField=${sortField}`),
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
                    hasHostPermisson && bindEventsAfterRenderGridSuccessfully(nextSortOrder, sortType);
                }
            },
            function (error) {
                cachedGridOptions = defaultGridOptions();
                cachedGridOptions.bodyRows = buildBodyRows([]);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
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
        var policyIds = [];
        var sportTypes = [];
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") === true) {
                policyIds.push($(this).data("id"));
                if (!sportTypes.includes($(this).data("sporttype"))) {
                    sportTypes.push($(this).data("sporttype"));
                }
            }
        });
        if (policyIds.length === 0) {
            return;
        }
        callCheckPolicyBeforeDelete(policyIds.join(), sportTypes);
    }

    function callCheckPolicyBeforeDelete(policies, sportTypes) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Policy/CheckPolicies`),
            {
                Policies: policies,
                SportTypes: sportTypes
            },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.Policies !== "") {
                    var warningMessage = `The following policy(es) have been applied to the sites - currencies:</br>${resultJson.Policies}.</br>Are you sure you want to delete them?</br>Note: If you delete them, their assign settings with Site - Currency will also be deleted.`;
                    CMS.showConfirmMessage(warningMessage, function () { callDelete(policies, sportTypes); });
                } else {
                    if (resultJson.IsSuccess) {
                        var message = `Are you sure you want to delete the selected setting row(s)?`;
                        CMS.showConfirmMessage(message, function () { callDelete(policies, sportTypes); });
                    } else {
                        CMS.showErrorMessage(resultJson.Message);
                    }
                }
            });
    }


    function callDelete(policies, sportTypes) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Policy/DeletePolicies`),
            {
                Policies: policies,
                SportTypes: sportTypes
            },
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    getPolicies();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    $(function () {
        init();
    });
})(jQuery);