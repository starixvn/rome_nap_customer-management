"use strict";
function CustomerClassCriteria(conditionList) {
    var htmlTemplate =
        `<div class="form-row-sub cc-group">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">2.</span>
                <span class="txt-name">General Customer Class</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCustomerClassCriteria" />
            </div>
            <div class="detail-rule edit-with-error-field">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-customerClassCondition"></select>
                </div>
                <div class="sport-cc">
                    <textarea id="txtcustomerClass" rows="3" cols="27" placeholder="<CC1>,<CC2>,..." oninput="VREngineHelper.validateCCValue(this)" onblur="VREngineHelper.replaceCharCCValue(this)"></textarea>
                </div>
            </div>
        </div>`;
    var chkEnableCustomerClassCriteria, ddlCustomerClassCondition, txtCustomerClass;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedCustomerClass, condition;
        var isEnabled = rule && rule.EnabledCustomerClass;
        if (isEnabled) {
            condition = rule.CustomerClassListNotIn ? 1 : 0;
            selectedCustomerClass = rule.CustomerClassListNotIn || rule.CustomerClassList;
        }

        ddlCustomerClassCondition = VREngineHelper.renderCombobox("ddl-customerClassCondition", conditionList, condition);
        txtCustomerClass = $("#txtcustomerClass").val(selectedCustomerClass);

        chkEnableCustomerClassCriteria = $("#chk-enableCustomerClassCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableCustomerClassCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableCustomerClassCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlCustomerClassCondition.combobox(action);
        txtCustomerClass.prop("disabled", !isEnabled);
    }

    function isEnableCriteria() {
        return chkEnableCustomerClassCriteria && chkEnableCustomerClassCriteria.is(":checked");
    }

    function validateFromData() {
        var customerClass = txtCustomerClass.val().toLowerCase() === "all"
            ? "0"
            : txtCustomerClass.val();

        if (!customerClass.length) {
            CMS.showTipsyError(txtCustomerClass, resources.noneSelectedCCError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtCustomerClass);

        var formData = { name: "CustomerClass" };
        formData[ddlCustomerClassCondition.val() === "0" ? "customerClass" : "customerClassNotIn"] = customerClass;

        return formData;
    }
    
    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableCustomerClassCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
