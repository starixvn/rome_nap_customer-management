"use strict";
function TimeBeforeKOCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">14.</span>
                <span class="txt-name">Time before KO</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTimeBeforeKOCriteria" />
            </div>
            <div class="detail-rule value-range">
                <div class="pre-match-hour-value vr-text-field">
                    <input type="text" id="txt-timeBeforeKO" autocomplete="off" maxlength="4" />
                    <span>hours</span>
                </div>
            </div>
        </div>`;
    var chkTimeBeforeKOCriteria, txtTimeBeforeKO;

    function cacheElements(rule, isHideEnabledCheckbox) {
        txtTimeBeforeKO = $("#txt-timeBeforeKO");

        var isEnabled = rule && rule.EnabledTimeBeforeKO;
        if (isEnabled) {
            txtTimeBeforeKO.val(rule.TimeBeforeKO);
        }

        chkTimeBeforeKOCriteria = $("#chk-enableTimeBeforeKOCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkTimeBeforeKOCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkTimeBeforeKOCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        txtTimeBeforeKO
            .off("keypress")
            .on("keypress", CMS.forceNumericInput);
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtTimeBeforeKO.prop("disabled", isDisabled);
    }

    function isEnableCriteria() {
        return chkTimeBeforeKOCriteria && chkTimeBeforeKOCriteria.is(":checked");
    }

    function validateFormData() {
        var inputTime = VREngineHelper.validateFloat(txtTimeBeforeKO);
        if (typeof inputTime === "undefined") {
            return { isInvalid: true };
        }
        if (inputTime <= 0) {
            CMS.showTipsyError(txtTimeBeforeKO, resources.timeBeforeKOError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtTimeBeforeKO);

        return {
            name: "TimeBeforeKO",
            TimeBeforeKO: inputTime
        };
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkTimeBeforeKOCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFormData();
        }
    };
}
