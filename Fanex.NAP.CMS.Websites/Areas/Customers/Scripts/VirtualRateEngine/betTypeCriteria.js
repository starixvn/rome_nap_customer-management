"use strict";
function BetTypeCriteria(conditionList, betTypeList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[6].</span>
                <span class="txt-name">Bet Type</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableBetTypeCriteria" />
                <span class="note-info-message" id="span-noteBetTypeCriteria"></span>
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

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlBetTypeCondition = virtualRateEngineHelper.renderCombobox("ddl-betTypeCondition", conditionList);
        ddlBetType = virtualRateEngineHelper.renderMultiSelect("ddl-betType", betTypeList);
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

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
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
            return {
                betType: ddlBetType.multiSelect("getCheckedItems").join(","),
                condition: ddlBetTypeCondition.val()
            };
        }
    };
}
