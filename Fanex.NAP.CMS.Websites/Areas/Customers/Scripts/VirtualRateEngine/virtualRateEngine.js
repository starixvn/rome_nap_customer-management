"use strict";
(function ($) {
    var formAddNewRule = $("#form-addNewRule");

    var txtRuleTitle;
    var txtPriorityOrder;
    var siteTypeCriteria = new SiteTypeCriteria(vm.SiteTypes, vm.Conditions, vm.CreditSites.concat(vm.LicenseeSites));
    var customerClassCriteria = new CustomerClassCriteria(vm.Conditions, vm.CustomerClasses);
    var currentVRCriteria = new CurrentVRCriteria();
    var currentVRStatusCriteria = new CurrentVRStatusCriteria(vm.CurrentVRStatus);
    var sportCriteria = new SportCriteria(vm.Conditions, vm.Sports);
    var betTypeCriteria = new BetTypeCriteria(vm.Conditions, vm.BetTypes);
    var eventCriteria = new EventCriteria(vm.Events);
    var leagueGroupCriteria = new LeagueGroupCriteria(vm.Conditions, vm.LeagueGroups, vm.Leagues);
    var betfromCriteria = new BetfromCriteria(vm.BetfromList);
    var stakeCriteria = new StakeCriteria();
    var ticketDoWCriteria = new TicketDoWCriteria(vm.Conditions, vm.DateOfWeeks);
    var ticketTimeCriteria = new TicketTimeCriteria();
    var ddlAdjustment, txtAdjustment;
    var ddlActive;

    function initAddNewRule(form) {
        txtRuleTitle = form.find("#txt-ruleTitle");

        txtPriorityOrder = form.find("#txt-priorityOrder");
        txtPriorityOrder
            .off("keypress")
            .on("keypress", CMS.forceNumberInput);

        var $criteriaContainer = form.find("#div-criteriaContainer");
        //siteTypeCriteria.initAddNew($criteriaContainer);
        customerClassCriteria.initAddNew($criteriaContainer);
        currentVRCriteria.initAddNew($criteriaContainer);
        currentVRStatusCriteria.initAddNew($criteriaContainer);
        sportCriteria.initAddNew($criteriaContainer, [1]);
        betTypeCriteria.initAddNew($criteriaContainer, [1]);
        eventCriteria.initAddNew($criteriaContainer);
        //leagueGroupCriteria.initAddNew($criteriaContainer);
        betfromCriteria.initAddNew($criteriaContainer);
        stakeCriteria.initAddNew($criteriaContainer);
        ticketDoWCriteria.initAddNew($criteriaContainer, [1]);
        ticketTimeCriteria.initAddNew($criteriaContainer);

        ddlAdjustment = virtualRateEngineHelper.renderCombobox("ddl-adjustment", vm.Adjustments);
        txtAdjustment = form.find("#txt-adjustment");
        txtAdjustment
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);

        ddlActive = virtualRateEngineHelper.renderCombobox("ddl-active", vm.Actives);

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel
            .off("click")
            .on("click", CMS.closePopup);

        var buttonAddNewRule = form.find("#btn-addNewRule");
        buttonAddNewRule
            .off("click")
            .on("click", function (e) {
                validateAddNewRuleFormData(form);
                //addNewRuleHandler();
            });
    }

    function validateAddNewRuleFormData() {
        var priority = validatePriorityOrder();
        if (!priority) {
            return;
        }

        var customerClassFormData = customerClassCriteria.getFormData();
        if (customerClassFormData && customerClassFormData.isInvalid) {
            return;
        }

        var currentVRFormData = currentVRCriteria.getFormData();
        if (currentVRFormData && currentVRFormData.isInvalid) {
            return;
        }

        var sportFormData = sportCriteria.getFormData();
        if (sportFormData && sportFormData.isInvalid) {
            return;
        }

        var betTypeFormData = betTypeCriteria.getFormData();
        if (betTypeFormData && betTypeFormData.isInvalid) {
            return;
        }

        var stakeFormData = stakeCriteria.getFormData();
        if (stakeFormData && stakeFormData.isInvalid) {
            return;
        }

        var ticketDoWFormData = ticketDoWCriteria.getFormData();
        if (ticketDoWFormData && ticketDoWFormData.isInvalid) {
            return;
        }

        var ticketTimeFormData = ticketTimeCriteria.getFormData();
        if (ticketTimeFormData && ticketTimeFormData.isInvalid) {
            return;
        }

        var adjustmentValue = validateAdjustment();
        if (typeof adjustmentValue === "undefined") {
            return;
        }

        var formData = $.extend(
            {
                ruleName: $.trim(txtRuleTitle.val()),
                priority: priority,
                adjustmentType: ddlAdjustment.val(),
                adjustmentValue: adjustmentValue,
                status: ddlActive.val()
            },
            customerClassFormData,
            currentVRFormData,
            currentVRStatusCriteria.getFormData(),
            sportFormData,
            betTypeFormData,
            eventCriteria.getFormData(),
            betfromCriteria.getFormData(),
            stakeFormData,
            ticketDoWFormData,
            ticketTimeFormData);
        console.dir(formData);

        return formData;
    }

    function validatePriorityOrder() {
        var inputPriorityOrder = virtualRateEngineHelper.validateNumber(txtPriorityOrder);
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
        var inputAdjustment = virtualRateEngineHelper.validateNumber(txtAdjustment);
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

    function addNewRuleHandler() {
        currentVRCriteria.getFormData();
        sportCriteria.getFormData();
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

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    });
})(jQuery);