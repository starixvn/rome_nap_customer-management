"use strict";
function TicketCurrencyCriteria(conditionList, curList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">13.</span>
                <span class="txt-name">Currency</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketCurCriteria" />
            </div>
            <div class="detail-rule edit-with-error-field">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-ticketCurCondition"></select>
                </div>
                <div class="ddl-second cur-box">
                    <div id="ddl-ticketCur"></div>
                </div>
            </div>
        </div>`;
    var chkEnableTicketCurCriteria, ddlTicketCurCondition, ddlTicketCur;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedCur, condition;
        var isEnabled = rule && rule.EnabledTicketCur;
        if (rule) {
            condition = rule.CurrencyIDListNotIn ? 1 : 0;
            selectedCur = rule.CurrencyIDListNotIn || rule.CurrencyIDList;
        }

        ddlTicketCurCondition = VREngineHelper.renderCombobox("ddl-ticketCurCondition", conditionList, condition);
        ddlTicketCur = VREngineHelper.renderMultiSelect("ddl-ticketCur", curList, selectedCur, 130);

        chkEnableTicketCurCriteria = $("#chk-enableTicketCurCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableTicketCurCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableTicketCurCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlTicketCurCondition.combobox(action);
        ddlTicketCur.fanexMultiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableTicketCurCriteria && chkEnableTicketCurCriteria.is(":checked");
    }

    function validateFromData() {
        var cur = ddlTicketCur.fanexMultiSelect("isCheckAll")
            ? "0"
            : ddlTicketCur.fanexMultiSelect("getItems", true).join(",");

        if (!cur.length) {
            CMS.showTipsyError(ddlTicketCur, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlTicketCur);

        var formData = { name: "Currency" };
        formData[ddlTicketCurCondition.val() === "0" ? "currencyIDList" : "currencyIDNotIn"] = cur;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableTicketCurCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
