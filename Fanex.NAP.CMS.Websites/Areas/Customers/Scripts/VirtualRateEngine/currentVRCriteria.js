"use strict";
function CurrentVRCriteria($selector) {
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

    function renderHtml() {
        $selector.append(htmlTemplate);
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

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableCurrentVRCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        var inputVRFrom = $.trim(txtVRFrom.val());
        if (!inputVRFrom.length) {
            CMS.showTipsyError(txtVRFrom, resources.requiredField, "");
        }
    };
}
