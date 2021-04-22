"use strict";
function CustomerClassCriteria($selector, notInList, customerClassList) {
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
    var chkEnableCustomerClassCriteria, ddlCustomerClasNotIn, ddlCustomerClass;

    function renderHtml() {
        $selector.append(htmlTemplate);
        ddlCustomerClasNotIn = virtualRateEngineHelper.renderCombobox("ddl-customerClasNotIn", notInList);
        ddlCustomerClass = virtualRateEngineHelper.renderMultiSelect("ddl-customerClass", customerClassList);
        chkEnableCustomerClassCriteria = $("#chk-enableCustomerClassCriteria");
    };

    function bindEvents() {
        chkEnableCustomerClassCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteCustomerClassCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var actionName = isEnabled ? "enable" : "disable";
        ddlCustomerClasNotIn.combobox(actionName);
        ddlCustomerClass.combobox(actionName);
    }

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableCustomerClassCriteria.remove();
        bindEvents();
    };
}
