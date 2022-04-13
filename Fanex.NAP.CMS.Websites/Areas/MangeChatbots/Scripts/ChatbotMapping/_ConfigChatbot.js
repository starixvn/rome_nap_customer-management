"use strict";

function ConfigChatBotModule(viewResources, table, form, resultMessageDiv) {
    function handler(XXXId) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("ChatbotMapping/UpdateConfig"),
            {custId: custId},
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    CMS.showInlineSuccessMsg(jsonData.Message, resultMessageDiv, true);
                } else {
                    CMS.showInlineErrorMsg(jsonData.Message, resultMessageDiv);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".ico-config", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var botId = $this.data("botid");
            var subAccId = $this.data("subaccid");
            console.log("clicked icon config")
            console.log("custId, botid, subaccid", custId,botId,subAccId)
            //Todo refer DeleteGlobalSetting.js
            var addHandler = new AddGlobalSetting(form);
            addHandler.initialize();
        });
    };
}

function AddGlobalSetting(form) {

    this.initialize = function () {
        CMS.modal("Config Chatbot", form, {resizable: false});
    };
}