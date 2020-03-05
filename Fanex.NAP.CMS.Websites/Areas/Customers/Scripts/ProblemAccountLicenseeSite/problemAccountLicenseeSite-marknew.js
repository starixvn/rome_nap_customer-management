"use strict";
function ProblemAccountLicenseeSiteMarkNew(form, reloadData) {
    var inlineMessage = null,
        usernames = null,
        preventiveAction = null,
        spanPreventiveAction = null,
        reason = null,
        spanReason = null,
        inputReason = null;

    function cacheElements() {
        inlineMessage = form.find("#div-inlineMessage");
        usernames = form.find("#txt-usernames");
        preventiveAction = form.find("#ddl-preventiveAction").combobox();
        spanPreventiveAction = form.find('span[id="ddl-preventiveAction"]');
        reason = form.find("#ddl-reason").combobox();
        spanReason = form.find('span[id="ddl-reason"]');
        inputReason = spanReason.find(".ui-combobox-input");
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

        preventiveAction.change(function () {
            getReasonList(preventiveAction.val());
        });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            isValidFormData() && submitForm();
        });
    }

    function getReasonList(value) {
        var reasons = preventiveActionsReasons[value] || [];

        if (!reasons.length || reasons[0].Id !== 0) {
            reasons.unshift({ Id: 0, Name: resources.pleaseSelect });
        }

        fillReasonsIntoDropdown(reasons);
    }

    function fillReasonsIntoDropdown(reasons) {
        var html = [];

        $.each(reasons, function (index, item) {
            html.push('<option value="');
            html.push(item.Id);
            html.push('">');
            html.push(item.Name);
            html.push("<option>");
        });

        reason.html(html.join(""));

        var firstSelectedReason = reasons[0];
        reason.val(firstSelectedReason.Id);
        inputReason.val(firstSelectedReason.Name);
    }

    function isValidFormData() {
        return isValidUsernames()
            && isValidDropdownList(preventiveAction, spanPreventiveAction)
            && isValidDropdownList(reason, spanReason);
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
            CMS.showInlineSuccessMsg(jsonData.Message, inlineMessage, true, function () {
                CMS.closePopup();
                reloadData();
            });
        } else {
            CMS.showInlineErrorMsg(jsonData.Message, inlineMessage);
        }
    }

    this.init = function (modalTitle) {
        CMS.modal(modalTitle, form, { resizable: false });

        cacheElements();
        bindEvents();
    };
}
