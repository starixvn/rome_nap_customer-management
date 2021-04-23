"use strict";
(function ($) {
    var formAddNewRule = $("#form-addNewRule");
    var siteList = [
        { Value: 1, Text: "BEST-ODDS" },
        { Value: 2, Text: "CN88" },
        { Value: 3, Text: "INDO" },
        { Value: 4, Text: "12Bet" },
        { Value: 5, Text: "TLC" },
        { Value: 6, Text: "Athena000" }
    ];
    var customerClassList = [
        { Value: 1, Text: "Group Betting" },
        { Value: 2, Text: "Hedging" },
        { Value: 3, Text: "Fixed" }
    ];
    var vrStatusList = [
        { Value: 0, Text: "Enabled" },
        { Value: 1, Text: "Disabled" }
    ];
    var sportList = [
        { Value: 1, Text: "Soccer" },
        { Value: 2, Text: "Basket Ball" }
    ];
    var betTypeList = [
        { Value: 1, Text: "Handicap" },
        { Value: 2, Text: "Over/Under" }
    ];
    var eventList = [
        { Value: 1, Text: "Live" },
        { Value: 2, Text: "None Live" }
    ];
    var leagueGroupList = [
        { Value: 1, Text: "aaaaaa" },
        { Value: 2, Text: "bbbbbb" }
    ];
    var leagueList = [
        { Value: 1, Text: "cccccc" },
        { Value: 2, Text: "dddddd" }
    ];
    var betfromList = [
        { Value: 1, Text: "p (phone betting)" },
        { Value: 2, Text: "z (china desktop)" }
    ];
    var dowList = [
        { Value: 1, Text: "Monday" },
        { Value: 2, Text: "Tuesday" },
        { Value: 3, Text: "Wednesday" },
        { Value: 4, Text: "Thursday" },
        { Value: 5, Text: "Friday" },
        { Value: 6, Text: "Sarturday" },
        { Value: 0, Text: "Sunday" }
    ];
    var siteTypeCriteria = new SiteTypeCriteria(vm.SiteTypes, vm.Conditions, vm.CreditSites.concat(vm.LicenseeSites));
    //var customerClassCriteria = new CustomerClassCriteria(notInList, customerClassList);
    //var currentVRCriteria = new CurrentVRCriteria();
    //var currentVRStatusCriteria = new CurrentVRStatusCriteria(vrStatusList);
    //var sportCriteria = new SportCriteria(notInList, sportList);
    //var betTypeCriteria = new BetTypeCriteria(notInList, betTypeList);
    //var eventCriteria = new EventCriteria(eventList);
    //var leagueGroupCriteria = new LeagueGroupCriteria(notInList, leagueGroupList, leagueList);
    //var betfromCriteria = new BetfromCriteria(betfromList);
    //var stakeCriteria = new StakeCriteria();
    //var ticketDoWCriteria = new TicketDoWCriteria(notInList, dowList);
    //var ticketTimeCriteria = new TicketTimeCriteria();

    function initAddNewRule(form) {
        var $criteriaContainer = form.find("#div-criteriaContainer");
        siteTypeCriteria.initAddNew($criteriaContainer);
        //customerClassCriteria.initAddNew($criteriaContainer);
        //currentVRCriteria.initAddNew($criteriaContainer);
        //currentVRStatusCriteria.initAddNew($criteriaContainer);
        //sportCriteria.initAddNew($criteriaContainer);
        //betTypeCriteria.initAddNew($criteriaContainer);
        //eventCriteria.initAddNew($criteriaContainer);
        //leagueGroupCriteria.initAddNew($criteriaContainer);
        //betfromCriteria.initAddNew($criteriaContainer);
        //stakeCriteria.initAddNew($criteriaContainer);
        //ticketDoWCriteria.initAddNew($criteriaContainer);
        //ticketTimeCriteria.initAddNew($criteriaContainer);

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

        form.off("submit")
            .on("submit", function (e) {
                e.preventDefault();
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
    }

    $(function () {
        $("#btnAddNewRule").click(function () {
            var form = formAddNewRule.clone();
            CMS.modal("Add New Rule", form, {
                height: "auto",
                width: "600px",
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