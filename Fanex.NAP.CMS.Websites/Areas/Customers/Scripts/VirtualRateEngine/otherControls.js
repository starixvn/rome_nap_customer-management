"use strict";
function OtherControls(adjustmentList, activeList) {
    var txtRuleTitle;
    var txtPriorityOrder;
    var ddlAdjustment, txtAdjustment;
    var ddlActive;
    var ckbVRPlush;
    var vRPlushField;

    function cacheElements(form, rule) {
        txtRuleTitle = form.find("#txt-ruleTitle");
        txtPriorityOrder = form.find("#txt-priorityOrder");
        txtAdjustment = form.find("#txt-adjustment");
        ckbVRPlush = form.find("#ckb-vrplush");
        vRPlushField = form.find("#vrplus-field");
        vRPlushField.hide();

        var selectedAdjustmentType, selectedStatus;
        if (rule) {
            txtRuleTitle.val(rule.RuleName);
            txtPriorityOrder.val(rule.Priority);
            txtAdjustment.val(rule.AdjustmentValue);

            selectedAdjustmentType = rule.AdjustmentType;
            selectedStatus = rule.Status;
            ckbVRPlush.prop("checked", rule.AccumulatedBalance);
            if (selectedAdjustmentType.toString() === "1" || selectedAdjustmentType.toString() === "3") {
                vRPlushField.show();
            }
        }

        ddlAdjustment = VREngineHelper.renderCombobox("ddl-adjustment", adjustmentList, selectedAdjustmentType);
        ddlActive = VREngineHelper.renderCombobox("ddl-active", activeList, selectedStatus);
    }

    function bindEvents() {
        txtPriorityOrder
            .off("keypress")
            .on("keypress", CMS.forceNumberInput);

        ddlAdjustment
            .off("change")
            .on("change", function () {
                txtAdjustment.val("");
                ckbVRPlush.prop("checked", false);
                if (ddlAdjustment.val() === "1" || ddlAdjustment.val() === "3") {
                    vRPlushField.show();
                } else {
                    vRPlushField.hide();
                }
            });

        txtAdjustment
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);
    }

    function validateFromData() {
        var isValid = true;

        var priorityOrder = validatePriorityOrder();
        if (!priorityOrder) {
            isValid = false;
        }

        var adjustmentValue = validateAdjustment();
        if (typeof adjustmentValue === "undefined") {
            isValid = false;
        }
        var isApplyVRPlush = ckbVRPlush.is(":checked");
        if (isApplyVRPlush && (ddlAdjustment.val() === "2" || adjustmentValue <= 0)) {
            isApplyVRPlush = false;
        }

        return isValid
            ? {
                ruleTitle: $.trim(txtRuleTitle.val()),
                priorityOrder: priorityOrder,
                adjustmentType: ddlAdjustment.val(),
                adjustmentValue: adjustmentValue,
                active: ddlActive.val(),
                accumulatedBalance: isApplyVRPlush
            }
            : { isInvalid: true };
    }

    function validatePriorityOrder() {
        var inputPriorityOrder = VREngineHelper.validateNumber(txtPriorityOrder);
        if (typeof inputPriorityOrder === "undefined") {
            return;
        }

        if (inputPriorityOrder < 1 || inputPriorityOrder > 10000) {
            CMS.showTipsyError(txtPriorityOrder, resources.priorityOrderValueOutOfRangeError, "nw");
            return;
        }

        CMS.clearTipsyError(txtPriorityOrder);
        return inputPriorityOrder;
    }

    function validateAdjustment() {
        var inputAdjustment = VREngineHelper.validateNumber(txtAdjustment);
        if (typeof inputAdjustment === "undefined") {
            return;
        }

        var isFixedValue = ddlAdjustment.val() === "1";
        if (isFixedValue && (inputAdjustment < -100 || inputAdjustment > 200)) {
            CMS.showTipsyError(txtAdjustment, resources.vrValueOutOfRangeError, "nw");
            return;
        }

        CMS.clearTipsyError(txtAdjustment);
        return inputAdjustment;
    }

    this.init = function (form, rule) {
        cacheElements(form, rule);
        bindEvents();
    };

    this.getFormData = function () {
        return validateFromData();
    };
}
