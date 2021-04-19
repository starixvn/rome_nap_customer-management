"use strict";
(function ($) {
    var $formAddNewRule = $("#form-addNewRule");
    var $siteType;
    var $siteSelection;

    $(function () {
        $("#btnAddNewRule").click(function () {
            var form = $formAddNewRule.clone();
            CMS.modal("Update Rule", form, { height: "auto", resizable: false });

            var buttonCancel = form.find("#btn-cancel");
            buttonCancel.off("click").on("click", CMS.closePopup);

            $siteType = form.find("#ddl-siteType").combobox();
            $siteSelection = form.find("#ddl-siteSelection").combobox();
        });

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    });
})(jQuery);