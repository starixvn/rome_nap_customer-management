"use strict";
function CurrentVRCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">3.</span>
                <span class="txt-name">Current Virtual Rate</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCurrentVRCriteria" />
                <span class="note-info-message" id="span-noteCurrentVRCriteria" title="<p>Current Virtual Rate:</p><p>- Apply from current virtual rate to current virtual rate.</p><p>- From and To value should be in this range [-100%;200%].</p>"></span>
            </div>
            <div class="detail-rule value-range">
                <span class="txt">From</span>
                <div class="from-value vr-text-field">
                    <input type="text" id="txt-currentVRFrom" autocomplete="off" maxlength="4" />
                    <span>%</span>
                </div>
                <span class="txt">To</span>
                <div class="to-value vr-text-field">
                    <input type="text" id="txt-currentVRTo" autocomplete="off" maxlength="4" />
                    <span>%</span>
                </div>
            </div>
        </div>`;
    var chkEnableCurrentVRCriteria, txtVRFrom, txtVRTo;

    function cacheElements(rule, isHideEnabledCheckbox) {
        txtVRFrom = $("#txt-currentVRFrom");
        txtVRTo = $("#txt-currentVRTo");

        var isEnabled = rule && rule.EnabledCurrentVR;
        if (isEnabled) {
            txtVRFrom.val(rule.VRFrom);
            txtVRTo.val(rule.VRTo);
        }

        chkEnableCurrentVRCriteria = $("#chk-enableCurrentVRCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableCurrentVRCriteria.css("visibility", "hidden");

        $("#span-noteCurrentVRCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function bindEvents() {
        chkEnableCurrentVRCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        txtVRFrom
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);

        txtVRTo
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtVRFrom.prop("disabled", isDisabled);
        txtVRTo.prop("disabled", isDisabled);
    }

    function isEnableCriteria() {
        return chkEnableCurrentVRCriteria && chkEnableCurrentVRCriteria.is(":checked");
    }

    function validateFormData() {
        var inputVRFrom = validateVRValue(txtVRFrom);
        if (typeof inputVRFrom === "undefined") {
            return { isInvalid: true };
        }

        var inputVRTo = validateVRValue(txtVRTo);
        if (typeof inputVRTo === "undefined") {
            return { isInvalid: true };
        }

        if (inputVRFrom > inputVRTo) {
            CMS.showTipsyError(txtVRFrom, resources.VRFromGreaterThanVRToError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtVRFrom);
        CMS.clearTipsyError(txtVRTo);

        return {
            name: "VRValue",
            VRFrom: inputVRFrom,
            VRTo: inputVRTo
        };
    }

    function validateVRValue(txtNumber) {
        var inputNumber = VREngineHelper.validateNumber(txtNumber);

        if (inputNumber && (inputNumber < -100 || inputNumber > 200)) {
            CMS.showTipsyError(txtNumber, resources.vrValueOutOfRangeError, "nw");
            return;
        }

        return inputNumber;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableCurrentVRCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFormData();
        }
    };
}
