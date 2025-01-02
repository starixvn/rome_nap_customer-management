"use strict";
function PreMatchPeriodCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">14.</span>
                <span class="txt-name">Pre-match Period</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enablePreMatchPeriodCriteria" />
            </div>
            <div class="detail-rule value-range">
                <div class="pre-match-hour-value vr-text-field">
                    <input type="text" id="txt-preMatchPeriod" autocomplete="off" maxlength="4" />
                    <span>hours</span>
                </div>
            </div>
        </div>`;
    var chkPreMatchPeriodCriteria, txtPreMatchPeriod;

    function cacheElements(rule, isHideEnabledCheckbox) {
        txtPreMatchPeriod = $("#txt-preMatchPeriod");

        var isEnabled = rule && rule.EnabledPreMatchPeriod;
        if (isEnabled) {
            txtPreMatchPeriod.val(rule.PreMatchPeriod);
        }

        chkPreMatchPeriodCriteria = $("#chk-enablePreMatchPeriodCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkPreMatchPeriodCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkPreMatchPeriodCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        txtPreMatchPeriod
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtPreMatchPeriod.prop("disabled", isDisabled);
    }

    function isEnableCriteria() {
        return chkPreMatchPeriodCriteria && chkPreMatchPeriodCriteria.is(":checked");
    }

    function validateFormData() {
        var inputPeriod = VREngineHelper.validateNumber(txtPreMatchPeriod);
        if (typeof inputPeriod === "undefined") {
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtPreMatchPeriod);

        return {
            name: "PreMatchPeriod",
            PreMatchPeriod: inputPeriod
        };
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkPreMatchPeriodCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFormData();
        }
    };
}
