function DeleteGlobalSetting(table, resultMessageDiv, reloadGridFunction, sportType) {
    "use strict";
    var deleteUrl = rootUrl + "GlobalSetting/Delete";

    function deleteHandler(custId, isGroupHeadline) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            deleteUrl,
            { CustId: custId, IsGroupHeadline: isGroupHeadline, SportType: sportType },
            function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, resultMessageDiv);
                }
                else {
                    CMS.showInlineSuccessMsg(
                        result.Message,
                        resultMessageDiv,
                        true,
                        reloadGridFunction);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".ico-delete", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var userName = $this.data("username");
            var isGroupHeadline = $this.data("isgroupheadline");

            CMS.showConfirmMessage(
                viewResources.GlobalSetting_DeleteMaxTurnover_Confirm.format(userName),
                function () {
                    deleteHandler(custId, isGroupHeadline);
                });
        });
    };
}
