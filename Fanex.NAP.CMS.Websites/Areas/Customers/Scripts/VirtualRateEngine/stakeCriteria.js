"use strict";
function StakeCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">10.</span>
                <span class="txt-name">Stake (RM)</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableStakeCriteria" />
                <span class="note-info-message" id="span-noteStakeCriteria" title="<p>Stake (RM):</p><p>- Apply for Stake in this range [From;To] (RM).</p><p>- If you want to input the unlimited stake on To, you can leave this field blank.</p>"></span>
            </div>
            <div class="detail-rule value-range">
                <span class="txt">From</span>
                <div class="from-value vr-text-field">
                    <input type="text" id="txt-stakeFrom" autocomplete="off" maxlength="10" />
                </div>
                <span class="txt">To</span>
                <div class="to-value vr-text-field">
                    <input type="text" id="txt-stakeTo" autocomplete="off" maxlength="10" />
                </div>
            </div>
        </div>`;
    var chkEnableStakeCriteria, txtStakeFrom, txtStakeTo;

    function cacheElements(rule, isHideEnabledCheckbox) {
        txtStakeFrom = $("#txt-stakeFrom");
        txtStakeTo = $("#txt-stakeTo");

        var isEnabled = rule && rule.EnabledStake;
        if (isEnabled) {
            txtStakeFrom.val(rule.StakeFrom);
            txtStakeTo.val(rule.StakeTo === 0 ? "" : rule.StakeTo);
        } else {
            txtStakeFrom.val("0");
        }

        chkEnableStakeCriteria = $("#chk-enableStakeCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableStakeCriteria.css("visibility", "hidden");

        $("#span-noteStakeCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function bindEvents() {
        chkEnableStakeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        txtStakeFrom
            .off("keypress")
            .on("keypress", CMS.forceNumericInput);

        txtStakeTo
            .off("keypress")
            .on("keypress", CMS.forceNumericInput);
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtStakeFrom.prop("disabled", isDisabled);
        txtStakeTo.prop("disabled", isDisabled);
    }

    function isEnableCriteria() {
        return chkEnableStakeCriteria && chkEnableStakeCriteria.is(":checked");
    }

    function validateFormData() {
        var inputStakeFrom = validateStakeValue(txtStakeFrom);
        if (typeof inputStakeFrom === "undefined") {
            return { isInvalid: true };
        }

        var inputStakeTo = validateStakeValue(txtStakeTo);
        if (typeof inputStakeTo === "undefined") {
            return { isInvalid: true };
        }

        if (inputStakeFrom === null && inputStakeTo === null) {
            CMS.showTipsyError(txtStakeFrom, resources.requiredFieldError, "nw");
            return { isInvalid: true };
        } else if ((inputStakeFrom !== null && inputStakeTo !== null && inputStakeFrom > inputStakeTo)
            || (inputStakeFrom === null && inputStakeTo !== null)) {
            CMS.showTipsyError(txtStakeFrom, resources.fromStakeGreaterThanToStake, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtStakeFrom);
        CMS.clearTipsyError(txtStakeTo);

        return {
            name: "StakeRM",
            stakeFrom: inputStakeFrom,
            stakeTo: inputStakeTo
        };
    }

    function validateStakeValue(txtStake) {
        var inputStake = $.trim(txtStake.val());

        if (inputStake.length) {
            if (!CMS.isNumeric(inputStake)) {
                CMS.showTipsyError(txtStake, resources.invalidValueError, "nw");
                return;
            }

            return parseFloat(inputStake);
        }

        return null;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableStakeCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFormData();
        }
    };
}
