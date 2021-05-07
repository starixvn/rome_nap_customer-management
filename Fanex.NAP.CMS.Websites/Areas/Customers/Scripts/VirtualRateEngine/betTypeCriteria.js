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
        </div>`
    var chkEnableBetTypeCriteria, ddlBetTypeCondition, ddlBetType;

    function renderHtml(criteriaContainer, selectedBetTypeList) {
        criteriaContainer.append(htmlTemplate);
        ddlBetTypeCondition = VREngineHelper.renderCombobox("ddl-betTypeCondition", conditionList);
        ddlBetType = VREngineHelper.renderMultiSelect("ddl-betType", betTypeList, selectedBetTypeList);
        chkEnableBetTypeCriteria = $("#chk-enableBetTypeCriteria");
    }

    function bindEvents() {
        chkEnableBetTypeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteBetTypeCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlBetTypeCondition.combobox(action);
        ddlBetType.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableBetTypeCriteria.is(":checked");
    }

    function validateFromData() {
        var betType = ddlBetType.multiSelect("isCheckedAllItems")
            ? "0"
            : ddlBetType.multiSelect("getCheckedItems").join(",");

        if (!betType.length) {
            CMS.showTipsyError(ddlBetType, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlBetType);

        var formData = { name: "Bettype" };
        formData[ddlBetTypeCondition.val() === "0" ? "betType" : "betTypeNotIn"] = betType;

        return formData;
    }

    this.initAddNew = function (criteriaContainer, selectedBetTypeList) {
        renderHtml(criteriaContainer, selectedBetTypeList);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableBetTypeCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
