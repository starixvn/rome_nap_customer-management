"use strict";
function ProblemAccountConfirmAction(form, reloadData) {
    var inlineMessage = null;

    function initForm(usernames, level) {
        form.find("#txt-level").val(level);
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            submitForm();
        });
    }

    function submitForm(confirmed) {
        CMS.disableInput();
        $.post(form.attr("action"), form.serialize() + (confirmed || ""), successHandler, "json")
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
        } else if (jsonData.Code === 101) {
            CMS.showConfirmMessage(jsonData.Message, function () {
                submitForm("&confirmed=true");
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
