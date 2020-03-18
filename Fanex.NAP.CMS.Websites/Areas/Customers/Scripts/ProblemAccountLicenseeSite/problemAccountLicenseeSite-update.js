"use strict";
function ProblemAccountLicenseeSiteUpdate(form, reloadData) {
    var inlineMessage = null,
        ddlPreventiveAction = null,
        spanPreventiveAction = null,
        ddlReason = null,
        spanReason = null;

    function initForm(usernames) {
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");

        ddlPreventiveAction = form.find("#ddl-preventiveAction").combobox();
        spanPreventiveAction = form.find('span[id="ddl-preventiveAction"]');

        ddlReason = form.find("#ddl-reason").combobox();
        spanReason = form.find('span[id="ddl-reason"]');
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            validateUpdate() && submitForm();
        });
    }

    function validateUpdate() {
        return isValidDropdownList(ddlPreventiveAction, spanPreventiveAction)
            && isValidDropdownList(ddlReason, spanReason);
    }

    function isValidDropdownList(ddlTag, span) {
        if (ddlTag.val() === "0") {
            CMS.showTipsyError(span, resources.requiredFieldError, "w");
            return false;
        }

        CMS.clearTipsyError(span);
        return true;
    }

    function submitForm() {
        CMS.disableInput();

        $.post(form.attr("action"), form.serialize(), successHandler, "json")
            .fail(CMS.handleAjaxError)
            .always(CMS.enableInput);
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

    this.init = function (modalTitle, usernames) {
        CMS.modal(modalTitle, form, { height: "auto" });

        initForm(usernames);
        bindEvents();
    };
}
