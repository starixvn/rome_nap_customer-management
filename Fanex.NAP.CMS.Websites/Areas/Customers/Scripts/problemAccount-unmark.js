"use strict";
function ProblemAccountUnmark(form, reloadData) {
    var inlineMessage = null;

    function initForm(usernames, level) {
        form.find("#txt-level").val(level);
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");

        if (level !== 1) {
            form.find("#divUnmarkNote").tipsy({
                className: "unmark-note",
                html: true,
                opacity: 1,
                gravity: "w"
            });
        }
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();

            CMS.disableInput();
            $.post(form.attr("action"), form.serialize(), successHandler, "json")
                .fail(CMS.handleAjaxError)
                .always(CMS.enableInput);
        });
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