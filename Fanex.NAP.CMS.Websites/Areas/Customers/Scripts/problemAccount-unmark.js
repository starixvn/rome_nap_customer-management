"use strict";
function ProblemAccountUnmark(form, reloadData) {
    var inlineMessage = null,
        txtUsernames = null;

    function initForm(usernames, level) {
        form.find("#txt-level").val(level);
        txtUsernames = form.find("#txt-usernames");
        txtUsernames.val(usernames.join(","));
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

        txtUsernames.off("change").on("change", function () {
            var usernamesValue = $.trim(txtUsernames.val());

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

                txtUsernames.val(usernamesArray.join(",").replace(/(<([^>]+)>)/ig, ""));
            }
        });
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            CMS.hideInlineMsg(inlineMessage);
            if (!ValidateUsername()) {
                return;
            }
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

    function ValidateUsername() {
        var usernames = txtUsernames.val().replace(/(<([^>]+)>)/ig, "");
        if (!usernames || usernames.length === 0) {
            CMS.showTipsyError(txtUsernames, resources.requiredFieldError, "n");

            return false;
        } else if (usernames.includes('\\')) {
            CMS.showTipsyError(txtUsernames, resources.unallowBacksplash, "n");

            return false;
        } else if (usernames.split(",").length > 100) {
            CMS.showTipsyError(txtUsernames, resources.overUsernamesLimitation, "n");

            return false;
        }

        return true;
    }
}