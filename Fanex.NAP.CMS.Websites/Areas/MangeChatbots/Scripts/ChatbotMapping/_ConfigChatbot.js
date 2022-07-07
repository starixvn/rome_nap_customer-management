"use strict";

function ConfigChatBotModule(viewResources, table, reloadData) {
    function showPopup(cusId, botId, subAccId) {
        CMS.getHtml(CMS.resolveRootUrl("ChatbotMapping/ChatBotConfig?" + $.param({
            CusId: cusId, BotId: botId, SubAccId: subAccId
        })), function (html) {
            if (html.length) {
                const popup = new PopupConfig(html, reloadData, cusId, botId);
                popup.initialize();
            }
        }, function () {
            reloadData();
        })
    }

    this.initialize = function () {
        table.on("click", ".ico-config", function () {
            const $this = $(this);
            const custId = $this.data("custid");
            const botId = $this.data("botid");
            const subAccId = $this.data("subaccid");
            showPopup(custId, botId, subAccId)
        });
    };
}

function PopupConfig(form, reloadData, cusId, botId) {
    let catchBotID;
    let divInlineMessage;
    let oldChatBotFunctionConfig;
    let oldLanguage;

    const getConfigChatBotFunctions = () => {
        const functionsConfig = [];
        $.each($("#functions")[0].children, function (index, item) {
            const func = (item.cells[1].children[0].children[0]);
            const functionsID = $(func).data("functid");
            const functionsIsEnable = $(func).hasClass("status-on");
            functionsConfig.push({
                FunctionId: functionsID, IsEnabled: functionsIsEnable,
            })
        })
        return functionsConfig;
    };

    const getUserID = () => $("#username").data("userid");
    
    const getChatID = () => $("#chatId").data("chatid");
    
    const getLanguage = () => $("#ddl-Language").val();

    const updateStateSaveBtn = (isConfigChange) => {
        if (isConfigChange) {
            $("#btn-submit-popup").prop('disabled', false);
        } else {
            $("#btn-submit-popup").prop('disabled', true);
        }
    }

    const isChangeConfigFunctions = () => {
        const currentConfig = getConfigChatBotFunctions();
        return JSON.stringify(currentConfig) !== oldChatBotFunctionConfig;
    }

    const isChangeLanguage = () => {
        const currentLanguage = getLanguage();
        return currentLanguage !== oldLanguage;
    }

    const submitForm = (formData) => {
        CMS.disableInput();
        CMS.callAjax(this, CMS.resolveRootUrl("ChatbotMapping/UpdateChatBotConfig"), formData, function (data) {
            var jsonData = $.parseJSON(data);
            if(jsonData.Code===500){
                CMS.showErrorMessage(jsonData.Message);
                return;
            }
            if (jsonData.updateFunctionResult?.Code === 0 && jsonData.updateLangResult?.Code === 0) {
                const messageText = [jsonData.updateFunctionResult.Message, jsonData.updateLangResult.Message]
                CMS.showInlineSuccessMsg(messageText.join(" "), divInlineMessage, true, function () {
                    CMS.closePopup();
                    reloadData();
                });
            } 
            else if(jsonData.updateFunctionResult?.Code ===102 || jsonData.updateLangResult?.Code === 0){
                const messageText = [jsonData.updateFunctionResult.Message, jsonData.updateLangResult.Message]
                CMS.showInlineErrorMsg(messageText[0] || messageText[1], divInlineMessage);
            }
            else {
                const messageText = [jsonData.updateFunctionResult.Message, jsonData.updateLangResult.Message]
                CMS.showInlineErrorMsg(messageText.join(" "), divInlineMessage);
            }
        });
    }

    const unlinkBot = (formData) => {
        CMS.disableInput();
        CMS.callAjax(this, CMS.resolveRootUrl("ChatbotMapping/UnlinkBot"), formData, function (data) {
            var jsonData = $.parseJSON(data);
            if(jsonData.Code===500){
                CMS.showErrorMessage(jsonData.Message);
                return;
            }
            if (jsonData.Code === 0) {
                CMS.showInlineSuccessMsg(jsonData.Message, divInlineMessage, true, function () {
                    CMS.closePopup();
                    reloadData();
                });
            } else {
                CMS.showInlineErrorMsg(jsonData.Message, divInlineMessage);
            }
        });
    }

    const bindingEvents = () => {
        if ($("#Status").data("islink")==="True") {
            $('.toggle-on-off').click(function (event) {
                event.preventDefault();
                const $this = $(this);

                $this.toggleClass('status-on status-off');
            });
        }

        $('#btn-close-popup').click(function (event) {
            event.preventDefault();
            CMS.closePopup();
        });

        $('#btn-unlink').click(function (event) {
            event.preventDefault();
            CMS.showConfirmMessage("Are you sure you want to unlink?", () => unlinkBot({
                BotId: catchBotID,
                UserId: getUserID(),
                ChatId: getChatID(),
                Language: oldLanguage
            }));

        });

        const formPopup = $("#formChatBotConfig");

        formPopup.submit(function (event) {
            event.preventDefault();

            const changeLanguage = isChangeLanguage();
            const changeConfigFunctions = isChangeConfigFunctions();
            const formData = {UserID: getUserID(), BotId: catchBotID,};
            if (changeConfigFunctions) {
                formData.ChatBotFunctionConfig = getConfigChatBotFunctions();
            }
            if (changeLanguage) {
                formData.Language = getLanguage();
            }
            submitForm(formData);
        });

        formPopup.click(function () {
            const isFormChange = (isChangeLanguage() || isChangeConfigFunctions());
            updateStateSaveBtn(isFormChange);
        });
    };

    const cacheClientData = () => {
        catchBotID = botId;
        divInlineMessage = $("#div-inlineMessage");
        oldChatBotFunctionConfig = JSON.stringify(getConfigChatBotFunctions());
        oldLanguage = getLanguage();
    }

    this.initialize = function () {
        CMS.modal("Config Chatbot", form, {resizable: false});
        cacheClientData();
        bindingEvents();
    }
}