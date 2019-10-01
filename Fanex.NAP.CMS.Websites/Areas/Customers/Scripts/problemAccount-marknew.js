"use strict";
function ProblemAccountMarkNew(form, reloadData) {
    var successInlineMessage = null,
        errorInlineMessage = null,
        level = null,
        spanLevel = null,
        usernames = null,
        reason = null,
        spanReason = null;

    function cacheElements() {
        successInlineMessage = form.find("#div-successInlineMessage");
        errorInlineMessage = form.find("#div-errorInlineMessage");
        level = form.find("#ddl-level").combobox();
        spanLevel = form.find('span[id="ddl-level"]');
        usernames = form.find("#txt-usernames");
        reason = form.find("#ddl-reason").combobox();
        spanReason = form.find('span[id="ddl-reason"]');
    }

    function bindEvents() {
        usernames.off("change").on("change", function () {
            var usernamesValue = $.trim(usernames.val());

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

                usernames.val(usernamesArray.join(","));
            }
        });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            isValidFormData() && submitForm();
        });
    }

    function isValidFormData() {
        var isValidLevel = isValidDropdownList(level, spanLevel),
            isValidInputUsernames = isValidUsernames(),
            isValidReason = isValidDropdownList(reason, spanReason);
        return isValidLevel && isValidInputUsernames && isValidReason;
    }

    function submitForm() {
        CMS.disableInput();
        $.post(form.attr("action"), form.serialize(), successHandler, "json")
            .fail(CMS.handleAjaxError)
            .always(CMS.enableInput);
    }

    function isValidDropdownList(ddlTag, span) {
        if (ddlTag.val() === "0") {
            CMS.showTipsyError(span, resources.requiredFieldError, "w");
            return false;
        }

        CMS.clearTipsyError(span);
        return true;
    }

    function isValidUsernames() {
        var usernamesValue = $.trim(usernames.val());
        if (usernamesValue.length === 0) {
            CMS.showTipsyError(usernames, resources.requiredFieldError, "ne");
            return false;
        }

        // Check usernames contains '\'.
        if (usernamesValue.indexOf("\\") >= 0) {
            CMS.showTipsyError(usernames, resources.inputInvalidUsernamesError.replace("{0}", "'\\'"), "ne");
            return false;
        }

        var usernamesArray = usernamesValue.split(",");
        if (usernamesArray.length > maxInputUsernames) {
            CMS.showTipsyError(usernames, resources.usernamesOutOfRangeError.replace("{0}", maxInputUsernames), "ne");
            return false;
        }

        // Check duplicate.
        var duplicatedUsernames = usernamesArray.filter(function (item, index) {
            return usernamesArray.indexOf(item) !== index;
        });

        if (duplicatedUsernames.length > 0) {
            CMS.showTipsyError(usernames, resources.duplicatedUsernamesError.replace("{0}", duplicatedUsernames.join(", ")), "ne");
            return false;
        }

        CMS.clearTipsyError(usernames);
        return true;
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);

        if (jsonData.Code === 0) {
            CMS.hideInlineMsg(errorInlineMessage);
            CMS.showInlineSuccessMsg(
                jsonData.Message,
                successInlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    reloadData();
                });
        } else {
            CMS.hideInlineMsg(successInlineMessage);
            jsonData.Message && CMS.showInlineSuccessMsg(jsonData.Message, successInlineMessage, false);
            jsonData.ErrorMessage && CMS.showInlineErrorMsg(jsonData.ErrorMessage, errorInlineMessage);
        }
    }

    this.init = function (modalTitle) {
        CMS.modal(modalTitle, form, { resizable: false });

        cacheElements();
        bindEvents();
    };
}
