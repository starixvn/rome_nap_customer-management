"use strict";
function OtherControls(adjustmentList, activeList) {
    var txtRuleTitle;
    var txtPriorityOrder;
    var ddlAdjustment, txtAdjustment;
    var ddlActive;

    function cacheElements(form, rule) {
        txtRuleTitle = form.find("#txt-ruleTitle");
        txtPriorityOrder = form.find("#txt-priorityOrder");
        txtAdjustment = form.find("#txt-adjustment");

        var selectedAdjustmentType, selectedStatus;
        if (rule) {
            txtRuleTitle.val(rule.RuleName);
            txtPriorityOrder.val(rule.Priority);
            txtAdjustment.val(rule.AdjustmentValue);

            selectedAdjustmentType = rule.AdjustmentType;
            selectedStatus = rule.Status;
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

        return isValid
            ? {
                ruleTitle: $.trim(txtRuleTitle.val()),
                priorityOrder: priorityOrder,
                adjustmentType: ddlAdjustment.val(),
                adjustmentValue: adjustmentValue,
                active: ddlActive.val()
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
