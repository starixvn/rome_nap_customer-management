"use strict";
function AddNewFollowBetAccount(div, postUrl, reloadData) {
    var maxInputUsernames = 100;
    var ddlPopupDummyAgent = null,
        txtUsernames = null,
        buttonCancel = null,
        buttonSave = null,
        divInlineMessage = null;

    function cacheElements() {
        divInlineMessage = div.find("#inline-message");

        ddlPopupDummyAgent = div.find("#ddl-popup-dummy-agent");
        txtUsernames = div.find("#txt-usernames");

        buttonCancel = div.find("#btn-cancel");
        buttonSave = div.find("#btn-save");
    }

    function bindEvents() {
        ddlPopupDummyAgent.combobox();
        txtUsernames.off("change").on("change", function () {
            var usernamesValue = $.trim(txtUsernames.val());

            if (usernamesValue.length > 0) {
                if (usernamesValue.indexOf("\n") >= 0) {
                    usernamesValue = usernamesValue.replace(/\n/g, " ");
                }

                if (usernamesValue.indexOf("\r") >= 0) {
                    usernamesValue = usernamesValue.replace(/\r/g, " ");
                }

                if (usernamesValue.indexOf("\t") >= 0) {
                    usernamesValue = usernamesValue.replace(/\t/g, " ");
                }

                usernamesValue = usernamesValue.replace(/\s/g, ",");
                var usernamesArray = usernamesValue.split(",").filter(item => item.length > 0);

                txtUsernames.val(usernamesArray.join(","));
            }
        });
        buttonCancel.off("click").on("click", CMS.closePopup);
        buttonSave.off("click").on("click", function () {
            var dummyAgentId = getDummyAgentId();
            var usernames = getInputUsernames();

            if (dummyAgentId === "" || usernames === "") return;

            submitForm({ dummyAgentId, custList: usernames });
        });
    }

    function getDummyAgentId() {
        var selectedDummyAgentId = ddlPopupDummyAgent.val();
        var spanPopupDummyAgent = div.find('span[id="ddl-popup-dummy-agent"]');

        if (selectedDummyAgentId === "0") {
            CMS.showTipsyError(spanPopupDummyAgent, resources.dummyAgentIsRequired, "w");
            return "";
        }

        CMS.clearTipsyError(spanPopupDummyAgent);
        return selectedDummyAgentId;
    }

    function getInputUsernames() {
        var inputUsernames = $.trim(txtUsernames.val());

        if (inputUsernames.length === 0) {
            CMS.showTipsyError(txtUsernames, resources.usernameIsRequired, "ne");
            return "";
        }

        // Check usernames contains '\'.
        if (inputUsernames.indexOf("\\") >= 0) {
            CMS.showTipsyError(txtUsernames, resources.usernameWarning, "ne");
            return "";
        }

        // Check max input
        var usernamesArray = inputUsernames.split(",");
        if (usernamesArray.length > maxInputUsernames) {
            CMS.showTipsyError(txtUsernames, resources.usernameLimitWarning, "ne");
            return "";
        }

        CMS.clearTipsyError(txtUsernames);
        return inputUsernames;
    }

    function submitForm(formData) {
        CMS.disableInput();
        $.post(postUrl, formData, successHandler, "json")
            .fail(CMS.handleAjaxError)
            .always(CMS.enableInput);
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);
        if (jsonData.IsSuccess) {
            CMS.showInlineSuccessMsg(
                jsonData.Message,
                divInlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    reloadData();
                });
        } else {
            CMS.showInlineErrorMsg(jsonData.Message, divInlineMessage);
        }
    }

    this.init = function (modalTitle) {
        CMS.modal(modalTitle, div, { resizeable: false });
        cacheElements();
        bindEvents();
    }
}