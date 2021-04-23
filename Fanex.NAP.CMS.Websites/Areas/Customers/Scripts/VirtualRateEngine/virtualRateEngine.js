"use strict";
(function ($) {
    var formAddNewRule = $("#form-addNewRule");
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

    function initAddNewRule(form) {
        var txtPriorityOrder = form.find("#txt-priorityOrder");
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
        ticketDoWCriteria.initAddNew($criteriaContainer);
        ticketTimeCriteria.initAddNew($criteriaContainer);

        var ddlAdjustment = virtualRateEngineHelper.renderCombobox("ddl-adjustment", vm.Adjustments);
        var txtAdjustment = form.find("#txt-adjustment");
        txtAdjustment
            .off("keypress")
            .on("keypress", CMS.forceNegativeNumberInput);

        var ddlActive = virtualRateEngineHelper.renderCombobox("ddl-active", vm.Actives);

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel
            .off("click")
            .on("click", CMS.closePopup);

        var buttonAddNewRule = form.find("#btn-addNewRule");
        buttonAddNewRule
            .off("click")
            .on("click", function (e) {
                validateAddNewRuleFormData(form);
                addNewRuleHandler();
            });
    }

    function validateAddNewRuleFormData(form) {
        var txtPriorityOrder = form.find("#txt-priorityOrder");
        var inputPriorityOrder = $.trim(txtPriorityOrder.val());
        if (!inputPriorityOrder.length) {
            CMS.showTipsyError(txtPriorityOrder, resources.requiredFieldError, "nw");
            return;
        }

        CMS.clearTipsyError(txtPriorityOrder);
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