"use strict";

function ConfigChatBotModule(viewResources, table, form, resultMessageDiv) {
    function query(CusId, BotId, SubAccId) {
        //  CMS.disableInput();
        CMS.getHtml(
            CMS.resolveRootUrl("ChatbotMapping/ChatBotConfig?" + $.param({
                CusId: CusId,
                BotId: BotId,
                SubAccId: SubAccId
            })),
            function (html) {
                if (html.length) {
                    var addHandler = new AddGlobalSetting(html);
                    addHandler.initialize();
                }
                /*   var jsonData = $.parseJSON(data);
   
                   console.log("jsonData", jsonData);
                   /!*if (jsonData.Code === 0) {
                       CMS.showInlineSuccessMsg(jsonData.Message, resultMessageDiv, true);
                   } else {
                       CMS.showInlineErrorMsg(jsonData.Message, resultMessageDiv);
                   }*!/
                   const renderChatBotInfo = () => {
                       $("#Username").text = "aaaaaaaaaaaaaa";
                       //$("#Username").val("bababababababa")
                   }
                   if (jsonData.ChatBotModel) {
                       renderChatBotInfo();
   
                       var addHandler = new AddGlobalSetting($("#formChatBotConfig").clone());
                       addHandler.initialize();
                   }*/

            });
    }

    this.initialize = function () {
        table.on("click", ".ico-config", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var botId = $this.data("botid");
            var subAccId = $this.data("subaccid");
            console.log("custId, botid, subaccid", custId, botId, subAccId)
            query(custId, botId, subAccId)

        });
    };
}

function AddGlobalSetting(form) {

    const updateFunctionConfig = () => {
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("ChatbotMapping/UpdateChatBotConfig"),
            {Test: "jack update config "},
            function (data) {
                var jsonData = $.parseJSON(data);
                console.log(jsonData)

            });
    }

    this.initialize = function () {
        CMS.modal("Config Chatbot", form, {resizable: false});

        $("#formChatBotConfig").submit(function (event) {
            event.preventDefault();
            updateFunctionConfig();
        });
    };
}