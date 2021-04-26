"use strict";
function CustomerClassCriteria(conditionList, customerClassList) {
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
                    <select class="combobox-autocomplete width-60" id="ddl-customerClassCondition"></select>
                </div>
                <div class="ddl-second class-customer-box">
                    <div id="ddl-customerClass"></div>
                </div>
            </div>
        </div>`
    var chkEnableCustomerClassCriteria, ddlCustomerClassCondition, ddlCustomerClass;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlCustomerClassCondition = virtualRateEngineHelper.renderCombobox("ddl-customerClassCondition", conditionList);
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
        var action = isEnabled ? "enable" : "disable";
        ddlCustomerClassCondition.combobox(action);
        ddlCustomerClass.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableCustomerClassCriteria.is(":checked");
    }

    function validateFromData() {
        var customerClass = ddlCustomerClass.multiSelect("getCheckedItems").join(",");

        if (!customerClass.length) {
            CMS.showTipsyError(ddlCustomerClass, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlCustomerClass);
        return {
            customerClass: customerClass,
            customerClassCondition: ddlCustomerClassCondition.val()
        };
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableCustomerClassCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria) {
            return validateFromData();
        }
    };
}
