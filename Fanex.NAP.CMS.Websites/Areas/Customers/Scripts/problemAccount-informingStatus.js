"use strict";
function ProblemAccountInformingStatus(form, reloadData) {
    var inlineMessage = null,
        rowDonwlines = null,
        ddlInformType = null,
        spanUpdater = null;

    function initForm(usernames, level, updater) {
        form.find("#txt-level").val(level);
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");
        rowDonwlines = form.find(".row-downlines");
        ddlInformType = form.find("#ddl-inform-type");

        if (level == 1) {
            rowDonwlines.addClass("display-none");
        }
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        ddlInformType.addClass('combobox-autocomplete').combobox();

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            submitForm();
        });
    }

    function submitForm() {
        CMS.disableInput();
        Validator.clearErrors(form);

        if (ddlInformType.val() === "0") {
            Validator.showError(form.find("#ddl-inform-type.ui-combobox"), resources.thisFieldIsRequired, 'w');
            CMS.enableInput();
            return;
        }

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

    this.init = function (modalTitle, usernames, level, updater) {
        CMS.modal(modalTitle, form, { height: "auto" });

        initForm(usernames, level, updater);
        bindEvents();
    };
}
