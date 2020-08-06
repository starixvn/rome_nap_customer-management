"use strict";
function ToggleLockEventSetting(table, reloadGridFunction) {
    function toggleLockHandler(formData) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/Lock"),
            formData,
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    reloadGridFunction();
                } else {
                    CMS.showErrorMessage(jsonData.Message);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".event-setting-lock", function () {
            var $this = $(this);
            var isLock = $this.hasClass("ico-lock");
            var formData = {
                custId: $this.data("custid"),
                matchId: $this.data("matchid"),
                isGroupHeadline: $this.data("isgroupheadline"),
                sportType: $this.data("sporttype"),
                isLock: !isLock,
            };

            toggleLockHandler(formData);
        });
    };
}
