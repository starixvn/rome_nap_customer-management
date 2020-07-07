"use strict";
function ToggleLockGlobalSetting(table, sportType) {
    function toggleLockHandler(formData, toggleLockSuccessHandler) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("TurnoverControl/GlobalSetting/Lock"),
            formData,
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    toggleLockSuccessHandler();
                } else {
                    CMS.showErrorMessage(jsonData.Message);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".lock-action", function () {
            var $this = $(this);
            var isLock = $this.hasClass("ico-lock");
            var formData = {
                custId: $this.data("custid"),
                isGroupHeadline: $this.data("isgroupheadline"),
                sportType: sportType,
                isLock: !isLock
            };
            var toggleLockSuccessHandler = function () {
                if (isLock) {
                    $this.removeClass("ico-lock").addClass("ico-unlock");
                } else {
                    $this.removeClass("ico-unlock").addClass("ico-lock");
                }
            };

            toggleLockHandler(formData, toggleLockSuccessHandler);
        });
    };
}
