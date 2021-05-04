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
                        //window.location.href = window.location.href;
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

        var siteTypeFormData = siteTypeCriteria.getFormData();
        if (siteTypeFormData && siteTypeFormData.isInvalid) {
            isValid = false;
        }

        var customerClassFormData = customerClassCriteria.getFormData();
        if (customerClassFormData && customerClassFormData.isInvalid) {
            isValid = false;
        }

        var currentVRFormData = currentVRCriteria.getFormData();
        if (currentVRFormData && currentVRFormData.isInvalid) {
            isValid = false;
        }

        var sportFormData = sportCriteria.getFormData();
        if (sportFormData && sportFormData.isInvalid) {
            isValid = false;
        }

        var betTypeFormData = betTypeCriteria.getFormData();
        if (betTypeFormData && betTypeFormData.isInvalid) {
            isValid = false;
        }

        var stakeFormData = stakeCriteria.getFormData();
        if (stakeFormData && stakeFormData.isInvalid) {
            isValid = false;
        }

        var ticketDoWFormData = ticketDoWCriteria.getFormData();
        if (ticketDoWFormData && ticketDoWFormData.isInvalid) {
            isValid = false;
        }

        var ticketTimeFormData = ticketTimeCriteria.getFormData();
        if (ticketTimeFormData && ticketTimeFormData.isInvalid) {
            isValid = false;
        }

        var adjustmentValue = validateAdjustment();
        if (typeof adjustmentValue === "undefined") {
            isValid = false;
        }

        if (isValid) {
            return $.extend(
                {
                    ruleTitle: $.trim(txtRuleTitle.val()),
                    priorityOrder: priorityOrder,
                    adjustmentType: ddlAdjustment.val(),
                    adjustmentValue: adjustmentValue,
                    active: ddlActive.val()
                },
                siteTypeFormData,
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
        }
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