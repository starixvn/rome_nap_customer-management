"use strict";
function CurrentVRCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[3].</span>
                <span class="txt-name">Current Virtual Rate</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCurrentVRCriteria" />
                <span class="note-info-message" id="span-noteCurrentVRCriteria"></span>
            </div>
            <div class="detail-rule value-range">
                <span class="txt">From</span>
                <div class="from-value">
                    <input type="text" id="txt-currentVRFrom" autocomplete="off" maxlength="4" />
                    <span>%</span>
                </div>
                <span class="txt">To</span>
                <div class="to-value">
                    <input type="text" id="txt-currentVRTo" autocomplete="off" maxlength="4" />
                    <span>%</span>
                </div>
            </div>
        </div>`
    var chkEnableCurrentVRCriteria, txtVRFrom, txtVRTo;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        txtVRFrom = $("#txt-currentVRFrom").clearfield();
        txtVRTo = $("#txt-currentVRTo").clearfield();
        chkEnableCurrentVRCriteria = $("#chk-enableCurrentVRCriteria");
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

        $("#span-noteCurrentVRCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtVRFrom.prop("disabled", isDisabled);
        txtVRTo.prop("disabled", isDisabled);
    }

    function validateNumber(txtNumber) {
        var inputNumber = $.trim(txtNumber.val());

        if (inputNumber.length === 0) {
            CMS.showTipsyError(txtNumber, resources.requiredFieldError, "nw");
            return;
        }

        if (!CMS.isNumeric(inputNumber)) {
            CMS.showTipsyError(txtNumber, resources.invalidValueError, "nw");
            return;
        }

        var number = parseInt(inputNumber.replace(/,/g, ""));

        if (number < -99 || number > 200) {
            CMS.showTipsyError(txtNumber, resources.invalidValueError, "nw");
            return;
        }

        CMS.clearTipsyError(txtNumber);
        return number;
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableCurrentVRCriteria.remove();
        bindEvents();
    };

    this.isEnableRule = function () {
        chkEnableCurrentVRCriteria.is(":checked");
    };

    this.getFormData = function () {
        if (chkEnableCurrentVRCriteria.is(":checked")) {
            var inputVRFrom = validateNumber(txtVRFrom);
            var inputVRTo = validateNumber(txtVRTo);

            if (inputVRFrom && inputVRTo) {
                if (inputVRFrom > inputVRTo) {
                    CMS.showTipsyError(txtVRFrom, resources.invalidValueError, "nw");
                    CMS.showTipsyError(txtVRTo, resources.invalidValueError, "nw");
                    return;
                }

                return {
                    VRFrom: inputVRFrom,
                    VRTo: inputVRTo
                };
            }
        }
    };
}
