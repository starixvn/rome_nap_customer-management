"use strict";
function BetTypeCriteria(conditionList, betTypeList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">6.</span>
                <span class="txt-name">Bet Type</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableBetTypeCriteria" />
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-betTypeCondition"></select>
                </div>
                <div class="ddl-second bet-type-box">
                    <div id="ddl-betType"></div>
                </div>
            </div>
        </div>`;
    var chkEnableBetTypeCriteria, ddlBetTypeCondition, ddlBetType;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedBetType, condition;
        var isEnabled = rule && rule.EnabledBetType;
        if (isEnabled) {
            condition = rule.BetTypeListNotIn ? 1 : 0;
            selectedBetType = rule.BetTypeListNotIn || rule.BetTypeList;
        }

        ddlBetTypeCondition = VREngineHelper.renderCombobox("ddl-betTypeCondition", conditionList, condition);
        ddlBetType = VREngineHelper.renderMultiSelect("ddl-betType", betTypeList, selectedBetType);

        chkEnableBetTypeCriteria = $("#chk-enableBetTypeCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableBetTypeCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableBetTypeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlBetTypeCondition.combobox(action);
        ddlBetType.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableBetTypeCriteria && chkEnableBetTypeCriteria.is(":checked");
    }

    function validateFromData() {
        var betType = ddlBetType.multiSelect("isCheckedAllItems")
            ? "0"
            : ddlBetType.multiSelect("getCheckedItems").join(",");

        if (!betType.length) {
            CMS.showTipsyError(ddlBetType, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlBetType);

        var formData = { name: "Bettype" };
        formData[ddlBetTypeCondition.val() === "0" ? "betType" : "betTypeNotIn"] = betType;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableBetTypeCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
