"use strict";

function ConfigChatBotModule(viewResources, table) {
    function showPopup(cusId, botId, subAccId) {
        CMS.getHtml(CMS.resolveRootUrl("ChatbotMapping/ChatBotConfig?" + $.param({
            CusId: cusId, BotId: botId, SubAccId: subAccId
        })), function (html) {
            if (html.length) {
                var popup = new PopupConfig(html);
                popup.initialize();
            }
        });
    }

    this.initialize = function () {
        table.on("click", ".ico-config", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var botId = $this.data("botid");
            var subAccId = $this.data("subaccid");
            console.log("custId, botid, subaccid", custId, botId, subAccId)
            showPopup(custId, botId, subAccId)

        });
    };
}

function PopupConfig(form) {

    const updateFunctionConfig = () => {
        CMS.callAjax(this, CMS.resolveRootUrl("ChatbotMapping/UpdateChatBotConfig"), {Test: "jack update config "}, function (data) {
            var jsonData = $.parseJSON(data);
            console.log(jsonData)

        });
    }
    const bindingEvents = () => {
        $('.toggle-on-off').click(function (event) {
            event.preventDefault();
            $(this).toggleClass('status-on status-off');
        });
        $("#formChatBotConfig").submit(function (event) {
            event.preventDefault();
            updateFunctionConfig();
        });
    };
    this.initialize = function () {
        CMS.modal("Config Chatbot", form, {resizable: false});

        bindingEvents();
    };
}