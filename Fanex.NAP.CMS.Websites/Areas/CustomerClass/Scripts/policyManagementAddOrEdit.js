"use strict";

const policyManagementAddOrEdit = (function ($) {
    let addNewOrEditForm = null,
        isAddNew = null,
        inlineMessage = null,
        ddlSportType = null,
        txtPolicyTitle = null,
        txtSelectRule = null,
        ddlAction = null,
        freeGridBody = null,
        freeGridBodyRows = null,
        reloadPolicyGrid = null,
        sportTypePolicyTitleMapping = {
            "0": "",
            "1": "SC - ",
            "2": "BK - ",
            "99": "OS - ",
            "43": "ES - "
        };

    function init(reloadGridFunc) {
        reloadPolicyGrid = reloadGridFunc;
        addNewOrEditForm = $("#addNewOrEditForm");
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    }

    function openModal(policy) {
        isAddNew = typeof policy === "undefined";
        CMS.modal(
            isAddNew ? resources.addPolicySetting : resources.editPolicySetting,
            addNewOrEditForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

        policy = policy || {};
        cacheElements(policy);
        bindEvents(policy);
    }

    function cacheElements(policy) {
        inlineMessage = $("#inlineMessage");
        ddlSportType = $("#ddlSportType").val(policy.SportType).combobox();
        txtPolicyTitle = $("#txtPolicyTitle").val(policy.PolicyTitle).clearfield();
        txtSelectRule = $("#txtSelectRule").clearfield();
        ddlAction = $("#ddlAction");
        freeGridBody = $("#freegridRule").find("tbody");
    }

    function bindEvents(policy) {
        ddlSportType
            .off("change")
            .change(function () {
                clearAllErrors();
                const sportType = this.value;
                txtPolicyTitle.val(sportTypePolicyTitleMapping[sportType]);
                txtSelectRule.val("");
                renderRuleGrid(sportType, new Set());
            });

        txtSelectRule.off("keyup").keyup(filterRules);

        ddlAction.off("change").change(filterRules);

        $("#btnSave")
            .off("click")
            .click(function () {
                clearAllErrors();
                const formData = getFormData();
                formData && handleOnSave(formData, policy.PolicyId);
            });

        $("#btnCancel").off("click").click(CMS.closePopup);

        if (!isAddNew) {
            renderRuleGrid(policy.SportType, new Set(policy.RuleIds));
        }
    }

    function renderRuleGrid(sportType, selectedRules) {
        if (sportType === "0") {
            renderEmptyRuleGrid();
        } else {
            CMS.getJson(
                CMS.resolveRootUrl(`Policy/GetRules?sportType=${sportType}`),
                function (data) {
                    const dataJson = $.parseJSON(data);
                    if (!dataJson.length) {
                        renderEmptyRuleGrid();
                    } else {
                        freeGridBody.html(buildBodyRows(dataJson, selectedRules));
                        freeGridBodyRows = freeGridBody.find("tr[data-ruleid]");

                        bindEventsAfterRenderGridSuccessfully();
                    }
                });
        }
    }

    function renderEmptyRuleGrid() {
        freeGridBody.html(buildRowNoData());
        freeGridBodyRows = freeGridBody.find("tr[data-ruleid]");
    }

    function buildBodyRows(rules, selectedRules) {
        let html = [buildRowSelectAll()];
        rules.forEach(function (rule) {
            let tdClass = rule.Enable ? "" : " disabled-cell";
            html.push('<tr data-ruleid="', rule.RuleId, '" data-ruletitle="', rule.RuleTitle, '">');
            html.push('<td class="align-center', tdClass, '"><input type="checkbox" id="chkAction-', rule.RuleId, '"', selectedRules.has(rule.RuleId) ? "checked" : "", '/></td>');
            html.push('<td class="cell-rule-title', tdClass, '">', rule.RuleTitle, '</td>');
            html.push('<td class="cell-criteria', tdClass, '">', policyManagementHelper.renderCriteriaCell(rule, true), '</td>');
            html.push('</tr>');
        });

        return html.join("");
    }

    function buildRowSelectAll() {
        return `<tr><td colspan="3"><input type="checkbox" class="chk-all" id="chkAll"/><label for="chkAll">(${resources.selectAllFollowFilter})</label></tr>`;
    }

    function buildRowNoData() {
        return `<tr><td colspan="3" class="text-red align-center">${resources.noData}</td></tr>`;
    }

    function bindEventsAfterRenderGridSuccessfully() {
        $("#chkAll")
            .off("change")
            .change(function () {
                const checked = this.checked;
                freeGridBodyRows.each(function () {
                    const $this = $(this);
                    if (!$this.hasClass("display-none")) {
                        const checkbox = $this.find(`#chkAction-${$this.data("ruleid")}`);
                        checkbox.prop("checked", checked);
                    }
                });
            });
    }

    function filterRules() {
        const action = ddlAction.val();
        const ruleTitle = $.trim(txtSelectRule.val()).toLowerCase();

        if (action === "0") {
            freeGridBodyRows.each(function () {
                const $this = $(this);
                if ($this.data("ruletitle").toString().toLowerCase().indexOf(ruleTitle) > -1) {
                    $this.removeClass("display-none");
                } else {
                    $this.addClass("display-none");
                }
            });
        } else if (action === "1") {
            freeGridBodyRows.each(function () {
                const $this = $(this);
                if ($this.data("ruletitle").toString().toLowerCase().indexOf(ruleTitle) > -1) {
                    const checkbox = $this.find(`#chkAction-${$this.data("ruleid")}`);
                    if (checkbox.is(":checked")) {
                        $this.removeClass("display-none");
                    } else {
                        $this.addClass("display-none");
                    }
                } else {
                    $this.addClass("display-none");
                }
            });
        } else {
            freeGridBodyRows.each(function () {
                const $this = $(this);
                if ($this.data("ruletitle").toString().toLowerCase().indexOf(ruleTitle) > -1) {
                    const checkbox = $this.find(`#chkAction-${$this.data("ruleid")}`);
                    if (checkbox.is(":checked")) {
                        $this.addClass("display-none");
                    } else {
                        $this.removeClass("display-none");
                    }
                } else {
                    $this.addClass("display-none");
                }
            });
        }
    }

    function getFormData() {
        const sportType = validateSportType();
        const policyTitle = validatePolicyTitle();

        if (sportType && policyTitle) {
            const ruleIds = validateSelectedRuleIds();

            if (ruleIds) {
                return { sportType, policyTitle, ruleIds: ruleIds.join(",") };
            }
        }
    }

    function validateSportType() {
        const sportType = ddlSportType.val();
        if (sportType === "0") {
            CMS.showTipsyError(ddlSportType.next("#ddlSportType"), resources.requiredField, "w");
            return;
        }

        return sportType;
    }

    function validatePolicyTitle() {
        const policyTitle = $.trim(txtPolicyTitle.val());
        if (!policyTitle.length) {
            CMS.showTipsyError(txtPolicyTitle, resources.requiredField, "w");
            return;
        }

        if (/<|&|"/.test(policyTitle) || !customerClassHelper.isASCII(policyTitle)) {
            CMS.showTipsyError(txtPolicyTitle, `Policy Title cannot contain special characters ('<', '&' and ' " '), symbol and punctuation.`, "w");
            return;
        }

        return policyTitle;
    }

    function validateSelectedRuleIds() {
        let ruleIds = [];
        freeGridBodyRows && freeGridBodyRows.each(function () {
            const $this = $(this);
            const ruleId = $this.data("ruleid");
            $this.find(`#chkAction-${ruleId}`).is(":checked") && ruleIds.push(ruleId);
        });

        return ruleIds;
    }

    function clearAllErrors() {
        CMS.clearTipsyError(ddlSportType.next("#ddlSportType"));
        CMS.clearTipsyError(txtPolicyTitle);
        CMS.hideInlineMsg(inlineMessage);
    }

    function handleOnSave(formData, policyId) {
        let action;
        if (isAddNew) {
            action = "Add";
        } else {
            action = "Edit";
            formData.policyId = policyId;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Policy/${action}`),
            formData,
            function (result) {
                customerClassHelper.showInlineMessageAndClosePopup(result, inlineMessage, reloadPolicyGrid);
            });
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);