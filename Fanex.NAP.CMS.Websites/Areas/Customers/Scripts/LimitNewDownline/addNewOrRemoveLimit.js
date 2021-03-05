"use strict";
function AddNewOrRemoveLimit(div, postUrl, reloadData) {
    var maxInputUsernames = 500;
    var divInlineMessage = null,
        textboxUsernames = null;

    function cacheElements(checkedUsernames) {
        divInlineMessage = div.find("#inline-message");
        textboxUsernames = div.find("#txt-usernames");
        textboxUsernames.val(checkedUsernames);
    }

    function bindEvents() {
        textboxUsernames.off("change").on("change", function () {
            var usernamesValue = $.trim(textboxUsernames.val());

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

                textboxUsernames.val(usernamesArray.join(","));
            }
        });

        var buttonCancel = div.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        var buttonSave = div.find("#btn-save");
        buttonSave.off("click").on("click", function () {
            var inputUsernames = getInputUsernames();
            inputUsernames.length > 0 && submitForm(inputUsernames);
        });
    }

    function getInputUsernames() {
        var inputUsernames = $.trim(textboxUsernames.val());
        if (inputUsernames.length === 0) {
            CMS.showTipsyError(textboxUsernames, resources.requiredUsernamesError, "nw");
            return "";
        }

        // Check usernames contains '\'.
        if (inputUsernames.indexOf("\\") >= 0) {
            CMS.showTipsyError(textboxUsernames, resources.inputInvalidUsernamesError.replace("{0}", '\\'), "nw");
            return "";
        }

        var usernamesArray = inputUsernames.split(",");
        if (usernamesArray.length > maxInputUsernames) {
            CMS.showTipsyError(textboxUsernames, resources.overUsernamesLimitationError, "nw");
            return "";
        }

        CMS.clearTipsyError(textboxUsernames);
        return inputUsernames;
    }

    function submitForm(inputUsernames) {
        CMS.disableInput();
        $.post(postUrl, { usernames: inputUsernames }, successHandler, "json")
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

    this.init = function (modalTitle, checkedUsernames) {
        CMS.modal(modalTitle, div, { resizable: false });

        cacheElements(checkedUsernames || "");
        bindEvents();
    };
}
