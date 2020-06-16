"use strict";
(function ($, gridEvents) {
    var oldSports;

    function saveGlobalAutoSuspendThreshold() {
        CMS.disableInput();
        var newSports = gridEvents.getSportsGlobalAutoSuspendThreshold();
        var formData = {
            sports: newSports
        };

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/SaveGlobalAutoSuspendThreshold"),
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
        gridEvents.bindEventsGlobalAutoSuspendThreshold();

        $(".btn-submit-changes").click(function () {
            CMS.showConfirmMessage(resources.VirtualRateSubmitChangesConfirm, saveGlobalAutoSuspendThreshold);
        });

        oldSports = gridEvents.getSportsGlobalAutoSuspendThreshold();

        $("#report-container").removeClass("display-none");

        window.onbeforeunload = function () {
            if (gridEvents.isSportsChangedGlobalAutoSuspendThreshold(oldSports)) {
                return resources.VirtualRateDetails_LeaveSiteWarning;
            }
        };
    });
})(jQuery, virtualRateGridEvents);