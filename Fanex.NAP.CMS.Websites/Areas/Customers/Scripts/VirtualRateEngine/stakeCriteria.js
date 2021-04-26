"use strict";
function StakeCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[10].</span>
                <span class="txt-name">Stake (RM)</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableStakeCriteria" />
                <span class="note-info-message" id="span-noteStakeCriteria"></span>
            </div>
            <div class="detail-rule value-range">
                <span class="txt">From</span>
                <div class="from-value">
                    <input type="text" id="txt-stakeFrom" autocomplete="off" maxlength="4" />
                </div>
                <span class="txt">To</span>
                <div class="to-value">
                    <input type="text" id="txt-stakeTo" autocomplete="off" maxlength="4" />
                </div>
            </div>
        </div>`
    var chkEnableStakeCriteria, txtStakeFrom, txtStakeTo;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        txtStakeFrom = $("#txt-stakeFrom").clearfield();
        txtStakeTo = $("#txt-stakeTo").clearfield();
        chkEnableStakeCriteria = $("#chk-enableStakeCriteria");
    }

    function bindEvents() {
        chkEnableStakeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        txtStakeFrom
            .off("keypress")
            .on("keypress", CMS.forceNumberInput);

        txtStakeTo
            .off("keypress")
            .on("keypress", CMS.forceNumberInput);

        $("#span-noteStakeCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtStakeFrom.prop("disabled", isDisabled);
        txtStakeTo.prop("disabled", isDisabled);
    }

    function isEnableCriteria() {
        return chkEnableStakeCriteria.is(":checked");
    }

    function validateFormData() {
        var inputStakeFrom = virtualRateEngineHelper.validateNumber(txtStakeFrom);
        var inputStakeTo = virtualRateEngineHelper.validateNumber(txtStakeTo);

        if (typeof inputStakeFrom === "undefined" || typeof inputStakeTo === "undefined") {
            return { isInvalid: true };
        }

        if (inputStakeFrom > inputStakeTo) {
            CMS.showTipsyError(txtStakeFrom, resources.fromStakeGreaterThanToStake, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtStakeFrom);
        CMS.clearTipsyError(txtStakeTo);

        return {
            stakeFrom: inputStakeFrom,
            stakeTo: inputStakeTo
        };
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableStakeCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFormData();
        }
    };
}
