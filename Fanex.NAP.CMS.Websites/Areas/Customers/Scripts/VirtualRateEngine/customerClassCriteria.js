"use strict";
function CustomerClassCriteria(conditionList, customerClassList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">2.</span>
                <span class="txt-name">Customer Class</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCustomerClassCriteria" />
                <span class="note-info-message" id="span-noteCustomerClassCriteria" title="Click to view Class and Category"></span>
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
    var chkEnableCustomerClassCriteria, spanHintCustomerClassCriteria, ddlCustomerClassCondition, ddlCustomerClass;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlCustomerClassCondition = VREngineHelper.renderCombobox("ddl-customerClassCondition", conditionList);
        ddlCustomerClass = VREngineHelper.renderMultiSelect("ddl-customerClass", customerClassList);
        chkEnableCustomerClassCriteria = $("#chk-enableCustomerClassCriteria");
        spanHintCustomerClassCriteria = $("#span-noteCustomerClassCriteria").tipsy({ className: "criteria-hint", opacity: 1 });
    };

    function bindEvents() {
        chkEnableCustomerClassCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        spanHintCustomerClassCriteria
            .off("click")
            .on("click", function () {
                window.open(CMS.resolveRootUrl("Customers/VirtualRateEngine/CustomerClassHint"), "_blank").focus();
            });
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
        var customerClass = ddlCustomerClass.multiSelect("isCheckedAllItems")
            ? "0"
            : ddlCustomerClass.multiSelect("getCheckedItems").join(",");

        if (!customerClass.length) {
            CMS.showTipsyError(ddlCustomerClass, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlCustomerClass);

        var formData = { name: "CustomerClass" };
        formData[ddlCustomerClassCondition.val() === "0" ? "customerClass" : "customerClassNotIn"] = customerClass;

        return formData;
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
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
