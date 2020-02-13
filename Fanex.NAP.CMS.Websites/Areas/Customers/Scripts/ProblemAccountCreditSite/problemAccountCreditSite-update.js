"use strict";
function ProblemAccountCreditSiteUpdate(form, reloadData) {
    var inlineMessage = null,
        ddlPreventiveAction = null,
        spanPreventiveAction = null,
        ddlReason = null,
        spanReason = null,
        inputReason = null;

    function initForm(usernames, level) {
        form.find("#txt-level").val(level);
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");

        ddlPreventiveAction = form.find("#ddl-preventiveAction").combobox();
        spanPreventiveAction = form.find('span[id="ddl-preventiveAction"]');

        ddlReason = form.find("#ddl-reason").combobox();
        spanReason = form.find('span[id="ddl-reason"]');
        inputReason = spanReason.find(".ui-combobox-input");
    }

    function bindEvents() {
        ddlPreventiveAction.change(function () {
            getReasonList(ddlPreventiveAction.val());
        });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            validateUpdate() && submitForm();
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

        ddlReason.html(html.join(""));

        var firstSelectedReason = reasons[0];
        ddlReason.val(firstSelectedReason.Id);
        inputReason.val(firstSelectedReason.Name);
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

    this.init = function (modalTitle, usernames, level) {
        CMS.modal(modalTitle, form, { height: "auto" });

        initForm(usernames, level);
        bindEvents();
    };
}
