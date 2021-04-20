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
                    <input type="text" id="txt-currentVRFrom" autocomplete="off" />
                    <span>%</span>
                </div>
                <span class="txt">To</span>
                <div class="to-value">
                    <input type="text" id="txt-currentVRTo" autocomplete="off" />
                    <span>%</span>
                </div>
            </div>
        </div>`
    var $currentVRFrom,
        $currentVRTo;

    this.renderHtml = function ($selector) {
        $selector.append(htmlTemplate);
        $currentVRFrom = $("#txt-currentVRFrom").clearfield();
        $currentVRTo = $("#txt-currentVRTo").clearfield();
    };

    this.bindEvents = function () {
        $("#chk-enableCurrentVRCriteria")
            .off("change")
            .on("change", function () {
                $currentVRFrom.prop("disabled", !this.value);
                $currentVRTo.prop("disabled", !this.value);
            });

        $("#span-noteCurrentVRCriteria")
            .off("click")
            .on("click", function () { });
    };
}
