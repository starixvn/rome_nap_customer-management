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

    $(function () {
        $("#btnAddNewRule").click(function () {
            var form = $formAddNewRule.clone();
            CMS.modal("Update Rule", form, { height: "auto", resizable: false });

            var buttonCancel = form.find("#btn-cancel");
            buttonCancel.off("click").on("click", CMS.closePopup);

            var $criteriaContainer = form.find("#div-criteriaContainer");
            var siteTypeCriteria = new SiteTypeCriteria();
            siteTypeCriteria.renderHtml($criteriaContainer, siteTypeList, notInList, siteList);
        });

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    });
})(jQuery);