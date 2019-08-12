var problemAccountMarkNew = (function ($, CMS, resources, reloadData) {
    "use strict";
    var elements = null;

    function cacheElements(form) {
        elements = {
            form: form,
            successInlineMessage: form.find("#div-successInlineMessage"),
            errorInlineMessage: form.find("#div-errorInlineMessage"),
            level: form.find("#ddl-level").combobox(),
            spanLevel: form.find('span[id="ddl-level"]'),
            usernames: form.find("#txt-usernames"),
            reason: form.find("#ddl-reason").combobox(),
            spanReason: form.find('span[id="ddl-reason"]')
        };
    }

    function bindEvents() {
        var buttonCancel = elements.form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        elements.form.off("submit").on("submit", function (event) {
            event.preventDefault();

            var validLevel = validateDropdownList(elements.level, elements.spanLevel),
                validUsernames = validateUsernames(elements.usernames),
                validReason = validateDropdownList(elements.reason, elements.spanReason);

            if (validLevel && validUsernames && validReason) {
                CMS.disableInput();
                var url = elements.form.attr("action"),
                    formData = elements.form.serialize();

                $.post(url, formData, successHandler, "json")
                    .fail(function (xhr) { CMS.handleAjaxError(xhr); })
                    .always(function () { CMS.enableInput(); });
            }
        });
    }

    function validateDropdownList(ddlTag, span) {
        if (ddlTag.val() === "0") {
            CMS.showTipsyError(span, resources.requiredFieldError, "w");
            return false;
        }

        CMS.clearTipsyError(span);
        return true;
    }

    function validateUsernames(inputTag) {
        var usernamesValue = $.trim(inputTag.val());
        if (usernamesValue.length === 0) {
            CMS.showTipsyError(inputTag, resources.requiredFieldError, "ne");
            return false;
        }

        // Check usernames contains '\'.
        if (usernamesValue.indexOf("\n") >= 0
            || usernamesValue.indexOf("\r") >= 0
            || usernamesValue.indexOf("\\") >= 0) {
            CMS.showTipsyError(inputTag, resources.inputInvalidUsernamesError, "ne");
            return false;
        }

        var usernamesArray = usernamesValue.split(",");
        if (usernamesArray.length > maxInputUsernames) {
            CMS.showTipsyError(inputTag, resources.usernamesOutOfRangeError.replace("{0}", maxInputUsernames), "ne");
            return false;
        }

        // Check duplicate.
        var trimUsernames = usernamesArray
            .map(function (item) {
                return $.trim(item);
            });

        var duplicatedUsernames = trimUsernames.filter(function (item, index) {
            return trimUsernames.indexOf(item) !== index;
        });

        if (duplicatedUsernames.length > 0) {
            CMS.showTipsyError(inputTag, resources.duplicatedUsernamesError.replace("{0}", duplicatedUsernames.join(", ")), "ne");
            return false;
        }

        CMS.clearTipsyError(inputTag);
        inputTag.val(trimUsernames.join(","));
        return true;
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);

        if (jsonData.Code === 0) {
            CMS.hideInlineMsg(elements.errorInlineMessage);
            CMS.showInlineSuccessMsg(
                jsonData.Message,
                elements.successInlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    reloadData();
                });
        } else {
            CMS.hideInlineMsg(elements.successInlineMessage);
            jsonData.Message && CMS.showInlineSuccessMsg(jsonData.Message, elements.successInlineMessage, false);
            jsonData.ErrorMessage && CMS.showInlineErrorMsg(jsonData.ErrorMessage, elements.errorInlineMessage);
        }
    }

    function init(modalTitle, form) {
        CMS.modal(modalTitle, form, { resizable: false });
        cacheElements(form);
        bindEvents();
    }

    return {
        init: init
    };
})(jQuery, CMS, resources, problemAccountSearch.reloadData);