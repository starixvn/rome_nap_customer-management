"use strict";
function DeleteGlobalSetting(viewResources, table, resultMessageDiv, reloadGridFunction, sportType) {
    function deleteHandler(custId, isGroupHeadline) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("TurnoverControl/GlobalSetting/Delete"),
            { custId: custId, isGroupHeadline: isGroupHeadline, sportType: sportType },
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    CMS.showInlineSuccessMsg(jsonData.Message, resultMessageDiv, true, reloadGridFunction);
                } else {
                    CMS.showInlineErrorMsg(jsonData.Message, resultMessageDiv);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".ico-delete", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var username = $this.data("username");
            var isGroupHeadline = $this.data("isgroupheadline");

            CMS.showConfirmMessage(
                viewResources.GlobalSetting_DeleteMaxTurnover_Confirm.format(username),
                function () { deleteHandler(custId, isGroupHeadline); });
        });
    };
}
