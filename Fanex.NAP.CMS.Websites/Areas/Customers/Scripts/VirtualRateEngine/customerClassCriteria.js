"use strict";
function CustomerClassCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[2].</span>
                <span class="txt-name">Customer Class</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCustomerClassCriteria" />
                <span class="note-info-message" id="span-noteCustomerClassCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-customerClasNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-customerClass"></div>
                </div>
            </div>
        </div>`
    var $customerClasNotIn,
        $customerClass;

    this.renderHtml = function ($selector, notInList, customerClassList) {
        $selector.append(htmlTemplate);
        $customerClasNotIn = virtualRateEngineHelper.renderCombobox("ddl-customerClasNotIn", notInList);
        $customerClass = virtualRateEngineHelper.renderCombobox("ddl-customerClass", customerClassList);
    };

    this.bindEvents = function () {
        $("#chk-enableCustomerClassCriteria")
            .off("change")
            .on("change", function () {
                if (this.checked) {
                    $customerClasNotIn.combobox("enable");
                    $customerClass.combobox("enable");
                } else {
                    $customerClasNotIn.combobox("disable");
                    $customerClass.combobox("disable");
                }
            });

        $("#span-noteCustomerClassCriteria")
            .off("click")
            .on("click", function () { });
    };
}
