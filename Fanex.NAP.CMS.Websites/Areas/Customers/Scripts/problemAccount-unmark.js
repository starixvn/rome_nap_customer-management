"use strict";
function ProblemAccountUnmark(form, reloadData) {
    var inlineMessage = null,
        ddlLevel = null,
        spanLevel = null,
        txtUsernames = null,
        divSMAUnmarkExtension = null,
        divUnmarkNote = null,
        checkboxExcludeAbnormalMember = null,
        checkboxExcludeDirectMark = null;

    function initForm(usernames, level) {
        if (!usernames.length) {
            level = "0";
        }

        inlineMessage = form.find("#div-inlineMessage");
        ddlLevel = form.find("#ddl-level").combobox();
        ddlLevel.val(level).combobox("refresh");
        spanLevel = form.find('span[id="ddl-level"]');
        txtUsernames = form.find("#txt-usernames");
        txtUsernames.val(usernames.join(","));
        divSMAUnmarkExtension = form.find("#div-smaUnmarkExtension");
        divUnmarkNote = form.find("#divUnmarkNote");
        checkboxExcludeAbnormalMember = form.find("#chk-excludeAbnormalMember");
        checkboxExcludeDirectMark = form.find("#chk-excludeDirectMark");

        divUnmarkNote.tipsy({
            className: "unmark-note",
            html: true,
            opacity: 1,
            gravity: "w"
        });
    }

    function bindEvents() {
        ddlLevel.off("change").on("change", function () {
            var levelId = this.value;
            disableSMAUnmarkExtensionDiv(levelId === "1" || levelId === "0");
        });

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

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            CMS.hideInlineMsg(inlineMessage);

            if (validateLevel() && validateUsername()) {
                CMS.disableInput();
                $.post(form.attr("action"), form.serialize(), successHandler, "json")
                    .fail(CMS.handleAjaxError)
                    .always(CMS.enableInput);
            }
        });

        ddlLevel.trigger("change");
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

    function validateUsername() {
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

    function validateLevel() {
        if (ddlLevel.val() === "0") {
            CMS.showTipsyError(spanLevel, resources.requiredFieldError, "w");
            return false;
        }

        CMS.clearTipsyError(spanLevel);
        return true;
    }

    function disableSMAUnmarkExtensionDiv(isDisabled) {
        if (isDisabled) {
            divSMAUnmarkExtension.attr("disabled", "disabled");
            checkboxExcludeAbnormalMember.attr("disabled", "disabled");
            checkboxExcludeDirectMark.attr("disabled", "disabled");
            divUnmarkNote.removeAttr("original-title");
        } else {
            divSMAUnmarkExtension.removeAttr("disabled");
            checkboxExcludeAbnormalMember.removeAttr("disabled");
            checkboxExcludeDirectMark.removeAttr("disabled");
            divUnmarkNote.attr("original-title", "<p>Example:</p><p>- Master1: Directly marked by OPS</p><p>&nbsp;&nbsp;- Agent1: Directly marked by OPS</p><p>&nbsp;&nbsp;&nbsp;&nbsp;- Member1: Directly marked by OPS</p><p>&nbsp;&nbsp;&nbsp;&nbsp;- Member2: Indirectly marked because of Agent1</p><p>&nbsp;&nbsp;- Agent2: Indirectly marked because of Master1</p><p>Unmarking Master1 using this option will unmark Master1 and Agent2.<br/>Member2 will still be marked for it belongs to a directly marked upline (Agent1).</p>");  
        }
    }

    this.init = function (modalTitle, usernames, level) {
        CMS.modal(modalTitle, form, { height: "auto" });

        initForm(usernames, level);
        bindEvents();
    };
}