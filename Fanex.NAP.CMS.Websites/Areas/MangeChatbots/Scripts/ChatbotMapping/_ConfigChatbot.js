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

            showPopup(custId, botId, subAccId)

        });
    };
}

function PopupConfig(form) {
    function submitForm(formData) {
        CMS.disableInput();
        CMS.callAjax(this, CMS.resolveRootUrl("ChatbotMapping/UpdateChatBotConfig"), formData, function (data) {
            var jsonData = $.parseJSON(data);

            if (jsonData.Code === 0) {
                CMS.closePopup();
                /*  CMS.showInlineSuccessMsg(jsonData.Message, inlineMessage, true, function () {
                      //Todo đã upate dc, giờ sau khi update thì reload page., close popup
                      CMS.closePopup();
                      reloadData();
                  });*/
            } else {
                //Todo lam them phan thong bao kq update config
                //CMS.showInlineErrorMsg(jsonData.Message, inlineMessage);
            }
        });
    }

    const bindingEvents = () => {
        $('.toggle-on-off').click(function (event) {
            event.preventDefault();
            const $this = $(this);
            $this.toggleClass('status-on status-off');
        });
        $("#formChatBotConfig").submit(function (event) {
            event.preventDefault();
            submitForm({ChatBotFunctionConfig: getConfigChatBotFunctions(), UserID: getUserID()})
        });
        $('.btn-close').click(function (event) {
            event.preventDefault();
            CMS.closePopup();
        });
    };

    const getConfigChatBotFunctions = () => {
        const functionsConfig = [];
        $.each($("#functions")[0].children, function (index, item) {
            const func = (item.cells[1].children[0].children[0]);
            const functionsID = $(func).data("functid");
            const functionsIsEnable = $(func).hasClass("status-on");
            functionsConfig.push({
                FunctionId: functionsID, IsEnabled: functionsIsEnable
            })
        })
        return functionsConfig;
    };
    const getUserID = () => {
        return $("#username").data("userid");
    }

    this.initialize = function () {
        CMS.modal("Config Chatbot", form, {resizable: false});

        bindingEvents();
    };
}