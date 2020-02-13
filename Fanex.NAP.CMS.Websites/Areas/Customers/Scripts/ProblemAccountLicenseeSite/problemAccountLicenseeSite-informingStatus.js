"use strict";
function ProblemAccountInformingStatusLicenseeSite(form, reloadData) {
    var inlineMessage = null,
        ddlInformType = null,
        spanInformType = null;

    function initForm(usernames) {
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");

        ddlInformType = form.find("#ddl-inform-type").combobox();
        spanInformType = form.find('span[id="ddl-inform-type"]');
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            validateInformType() && submitForm();
        });
    }

    function validateInformType() {
        if (ddlInformType.val() === "-1") {
            CMS.showTipsyError(spanInformType, resources.thisFieldIsRequired, "w");
            return false;
        }

        CMS.clearTipsyError(spanInformType);
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
