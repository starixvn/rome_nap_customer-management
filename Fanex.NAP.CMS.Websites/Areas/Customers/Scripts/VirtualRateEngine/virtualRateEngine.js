"use strict";
(function ($) {
    var $formAddNewRule = $("#form-addNewRule");
    var $siteType;
    var $siteSelection;
    var siteTypeList = [
        { Value: 0, Text: "All" },
        { Value: 1, Text: "Credit" },
        { Value: 2, Text: "Licensee" }
    ];
    var notInList = [
        { Value: 0, Text: "in" },
        { Value: 1, Text: "not in" }
    ];
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
    var adjustmentList = [
        { Value: 1, Text: "Adjust Current Virtual Rate" },
        { Value: 2, Text: "Fixed Value" }
    ];
    var activeList = [
        { Value: 1, Text: "UAT" },
        { Value: 2, Text: "PRO" },
        { Value: 3, Text: "Inactive" },
    ];

    $(function () {
        $("#btnAddNewRule").click(function () {
            var form = $formAddNewRule.clone();
            CMS.modal("Update Rule", form, { height: "auto", resizable: false });

            var buttonCancel = form.find("#btn-cancel");
            buttonCancel.off("click").on("click", CMS.closePopup);

            var $criteriaContainer = form.find("#div-criteriaContainer");

            var siteTypeCriteria = new SiteTypeCriteria($criteriaContainer, siteTypeList, notInList, siteList);
            siteTypeCriteria.initAddNew();

            var customerClassCriteria = new CustomerClassCriteria($criteriaContainer, notInList, customerClassList);
            customerClassCriteria.initAddNew();

            var currentVRCriteria = new CurrentVRCriteria($criteriaContainer);
            currentVRCriteria.initAddNew();

            var currentVRStatusCriteria = new CurrentVRStatusCriteria($criteriaContainer, vrStatusList);
            currentVRStatusCriteria.initAddNew();

            var sportCriteria = new SportCriteria($criteriaContainer, notInList, sportList);
            sportCriteria.initAddNew();

            var betTypeCriteria = new BetTypeCriteria($criteriaContainer, notInList, betTypeList);
            betTypeCriteria.initAddNew();

            var eventCriteria = new EventCriteria($criteriaContainer, eventList);
            eventCriteria.initAddNew();

            var leagueGroupCriteria = new LeagueGroupCriteria($criteriaContainer, notInList, leagueGroupList, leagueList);
            leagueGroupCriteria.initAddNew();

            var betfromCriteria = new BetfromCriteria($criteriaContainer, betfromList);
            betfromCriteria.initAddNew();

            var stakeCriteria = new StakeCriteria($criteriaContainer);
            stakeCriteria.initAddNew();

            var ticketDoWCriteria = new TicketDoWCriteria($criteriaContainer, notInList, dowList);
            ticketDoWCriteria.initAddNew();

            var ticketTimeCriteria = new TicketTimeCriteria($criteriaContainer);
            ticketTimeCriteria.initAddNew();

            var ddlAdjustment = virtualRateEngineHelper.renderCombobox("ddl-adjustment", adjustmentList);
            var ddlActive = virtualRateEngineHelper.renderCombobox("ddl-active", activeList);
        });

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    });
})(jQuery);