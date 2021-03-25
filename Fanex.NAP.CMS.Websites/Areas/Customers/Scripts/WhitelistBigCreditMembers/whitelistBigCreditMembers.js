"use strict";
(function ($, resources) {
    var maxInputUsernames = 500;
    var formAddMembers = $("#formAddMembers"),
        successMessage = $("#success-message"),
        warnMessage = $("#warn-message"),
        errorMessage = $("#error-message"),
        textboxUsernames = $("#txt-usernames");

    function bindEvents() {
        formAddMembers.submit(function (e) {
            e.preventDefault();

            CMS.hideInlineMsg(errorMessage);
            CMS.hideInlineMsg(warnMessage);
            CMS.hideInlineMsg(successMessage);

            var inputUsernames = getInputUsernames();
            inputUsernames.length && addMembers(formAddMembers.attr("action"), inputUsernames);
        });

        textboxUsernames.change(function () {
            var inputUsernames = $.trim(textboxUsernames.val());

            if (inputUsernames.length > 0) {
                if (inputUsernames.indexOf("\n") >= 0) {
                    inputUsernames = inputUsernames.replace(/\n/g, " ");
                }

                if (inputUsernames.indexOf("\r") >= 0) {
                    inputUsernames = inputUsernames.replace(/\r/g, " ");
                }

                if (inputUsernames.indexOf("\t") >= 0) {
                    inputUsernames = inputUsernames.replace(/\t/g, " ");
                }

                inputUsernames = inputUsernames.replace(/\s/g, ",");
                var usernamesArray = inputUsernames.split(",").filter(item => item.length > 0);

                textboxUsernames.val(usernamesArray.join(","));
            }
        });
    }

    function getInputUsernames() {
        var inputUsernames = $.trim(textboxUsernames.val());
        if (inputUsernames.length === 0) {
            CMS.showTipsyError(textboxUsernames, resources.requiredWhitelistBigCreditMembersError, "s");
            return "";
        }

        // Check usernames contains '\'.
        if (inputUsernames.indexOf("\\") >= 0) {
            CMS.showTipsyError(textboxUsernames, resources.inputInvalidUsernamesError.replace("{0}", '\\'), "s");
            return "";
        }

        var usernamesArray = inputUsernames.split(",");
        if (usernamesArray.length > maxInputUsernames) {
            CMS.showTipsyError(textboxUsernames, resources.overUsernamesLimitationError, "s");
            return "";
        }

        CMS.clearTipsyError(textboxUsernames);
        return inputUsernames;
    }

    function addMembers(postUrl, inputUsernames) {
        CMS.disableInput();
        $.post(postUrl, { usernames: inputUsernames }, successHandler, "json")
            .fail(CMS.handleAjaxError)
            .always(CMS.enableInput);
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);
        var errorMessageValue = "",
            warningMessageValue = "",
            successMessageValue = "";

        $.each(jsonData, function (index, result) {
            if (result.IsError) {
                errorMessageValue += result.Message + result.Username + "<br/>";
            } else if (result.IsWarning) {
                warningMessageValue += result.Message + result.Username + "<br/>";
            } else if (result.IsSuccess) {
                successMessageValue += result.Message + result.Username + "<br/>";
            }
        });

        errorMessageValue.length && CMS.showInlineErrorMsg(errorMessageValue, errorMessage);
        warningMessageValue.length && CMS.showInlineWarningMsg(warningMessageValue, warnMessage);
        successMessageValue.length && CMS.showInlineSuccessMsg(successMessageValue, successMessage, false);
    }

    $(function () {
        bindEvents();
        textboxUsernames.focus();
    });
})(jQuery, resources);
