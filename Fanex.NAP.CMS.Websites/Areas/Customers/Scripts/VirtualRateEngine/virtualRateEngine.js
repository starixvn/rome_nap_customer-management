"use strict";
(function ($) {
    var formAddNewRule = $("#form-addNewRule");

    var txtRuleTitle;
    var txtPriorityOrder;
    var siteTypeCriteria = new SiteTypeCriteria(vm.SiteTypes, vm.Conditions, vm.CreditSites, vm.LicenseeSites);
    var customerClassCriteria = new CustomerClassCriteria(vm.Conditions, vm.CustomerClasses);
    var currentVRCriteria = new CurrentVRCriteria();
    var currentVRStatusCriteria = new CurrentVRStatusCriteria(vm.CurrentVRStatus);
    var sportCriteria = new SportCriteria(vm.Conditions, vm.Sports);
    var betTypeCriteria = new BetTypeCriteria(vm.Conditions, vm.BetTypes);
    var eventCriteria = new EventCriteria(vm.Events);
    var leagueGroupCriteria = new LeagueGroupCriteria(vm.Conditions, vm.LeagueGroups);
    var betfromCriteria = new BetfromCriteria(vm.BetfromList);
    var stakeCriteria = new StakeCriteria();
    var ticketDoWCriteria = new TicketDoWCriteria(vm.Conditions, vm.DayOfWeeks);
    var ticketTimeCriteria = new TicketTimeCriteria();
    var ddlAdjustment, txtAdjustment;
    var ddlActive;

    var ruleList = new VREngineHelperRuleList(hasHostFullPermission, hasHostVirtualRateEnginePermission);

    function initAddNewRule(form) {
        txtRuleTitle = form.find("#txt-ruleTitle");

        txtPriorityOrder = form.find("#txt-priorityOrder");
        txtPriorityOrder
            .off("keypress")
            .on("keypress", CMS.forceNumberInput);

        var $criteriaContainer = form.find("#div-criteriaContainer");
        siteTypeCriteria.initAddNew($criteriaContainer);
        customerClassCriteria.initAddNew($criteriaContainer);
        currentVRCriteria.initAddNew($criteriaContainer);
        currentVRStatusCriteria.initAddNew($criteriaContainer);
        sportCriteria.initAddNew($criteriaContainer);
        betTypeCriteria.initAddNew($criteriaContainer);
        eventCriteria.initAddNew($criteriaContainer);
        leagueGroupCriteria.initAddNew($criteriaContainer);
        betfromCriteria.initAddNew($criteriaContainer);
        stakeCriteria.initAddNew($criteriaContainer);
        ticketDoWCriteria.initAddNew($criteriaContainer, [2]);
        ticketTimeCriteria.initAddNew($criteriaContainer);

        ddlAdjustment = VREngineHelper.renderCombobox("ddl-adjustment", vm.Adjustments);
        txtAdjustment = form.find("#txt-adjustment");
        txtAdjustment
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);

        ddlActive = VREngineHelper.renderCombobox("ddl-active", vm.Actives);

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel
            .off("click")
            .on("click", CMS.closePopup);

        var buttonAddNewRule = form.find("#btn-addNewRule");
        buttonAddNewRule
            .off("click")
            .on("click", function () {
                addNewRuleHandler(form);
            });
    }

    function addNewRuleHandler(form) {
        var formData = validateAddNewRuleFormData(form);
        formData && CMS.callAjax(
            this,
            CMS.resolveRootUrl("Customers/VirtualRateEngine/AddNewRule"),
            formData,
            function (jsonString) {
                var result = JSON.parse(jsonString);
                if (result.Code) {
                    CMS.showInlineErrorMsg(result.Message, form.find("#divErrorSuccessMsg"));
                } else {
                    CMS.showInlineSuccessMsg(result.Message, form.find("#divErrorSuccessMsg"), true, function () {
                        CMS.closePopup();
                        ruleList.getRuleList();
                    });
                }
            }
        );
    }

    function validateAddNewRuleFormData() {
        var isValid = true;

        var priorityOrder = validatePriorityOrder();
        if (!priorityOrder) {
            isValid = false;
        }

        var formDataCriteria = [];
        $.each([
            siteTypeCriteria,
            customerClassCriteria,
            currentVRCriteria,
            currentVRStatusCriteria,
            sportCriteria,
            betTypeCriteria,
            eventCriteria,
            leagueGroupCriteria,
            betfromCriteria,
            stakeCriteria,
            ticketDoWCriteria,
            ticketTimeCriteria
        ], function (index, value) {
            var formData = value.getFormData();
            if (formData) {
                if (formData.isInvalid) {
                    isValid = false;
                } else {
                    formDataCriteria.push(formData);
                }
            }
        });

        var adjustmentValue = validateAdjustment();
        if (typeof adjustmentValue === "undefined") {
            isValid = false;
        }

        if (isValid) {
            var enabledCriteria = [];
            var addNewRuleFormData = {
                ruleTitle: $.trim(txtRuleTitle.val()),
                priorityOrder: priorityOrder,
                adjustmentType: ddlAdjustment.val(),
                adjustmentValue: adjustmentValue,
                active: ddlActive.val()
            };

            $.each(formDataCriteria, function (index, value) {
                enabledCriteria.push(value.name);

                delete value.name;
                $.extend(addNewRuleFormData, value);
            });

            addNewRuleFormData.enabledColumns = enabledCriteria.join(",");

            return addNewRuleFormData;
        }
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
        if (isFixedValue && (inputAdjustment < -99 || inputAdjustment > 200)) {
            CMS.showTipsyError(txtAdjustment, resources.vrValueOutOfRangeError, "nw");
            return;
        }

        CMS.clearTipsyError(txtAdjustment);
        return inputAdjustment;
    }

    $(function () {
        $("#btnAddNewRule").click(function () {
            var form = formAddNewRule.clone();
            CMS.modal("Add New Rule", form, {
                height: "auto",
                width: "640px",
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

            initAddNewRule(form);
        });

        ruleList = new VREngineHelperRuleList(hasHostFullPermission, hasHostVirtualRateEnginePermission);
        ruleList.getRuleList();

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    });
})(jQuery);