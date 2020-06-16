"use strict";
(function ($, gridEvents) {
    var oldSports;

    function saveGlobalSettings() {
        CMS.disableInput();
        var newSports = gridEvents.getSports();
        var formData = {
            rates: {
                sports: newSports
            }
        };

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/SaveGlobalSettings"),
            formData,
            function (result) {
                if (result.IsSuccess) {
                    oldSports = newSports;
                    CMS.showSuccessMessage(result.Message);
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    $(function () {
        gridEvents.bindEventsGlobalSettings();

        $(".btn-submit-changes").click(function () {
            CMS.showConfirmMessage(resources.VirtualRateSubmitChangesConfirm, saveGlobalSettings);
        });

        oldSports = gridEvents.getSports();

        $("#report-container").removeClass("display-none");

        window.onbeforeunload = function () {
            if (gridEvents.isSportsChanged(oldSports)) {
                return resources.VirtualRateDetails_LeaveSiteWarning;
            }
        };
    });
})(jQuery, virtualRateGridEvents);