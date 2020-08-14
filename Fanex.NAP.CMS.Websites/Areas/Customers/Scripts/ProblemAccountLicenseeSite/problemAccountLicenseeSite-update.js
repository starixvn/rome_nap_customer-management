"use strict";
function ProblemAccountLicenseeSiteUpdate(form, reloadData) {
    var inlineMessage = null,
        ddlPreventiveAction = null,
        spanPreventiveAction = null,
        ddlReason = null,
        spanReason = null,
        ddlTime = null,
        spanTime = null,
        txtLimitAmount = null;

    function initForm(usernames) {
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");

        ddlPreventiveAction = form.find("#ddl-preventiveAction").combobox();
        spanPreventiveAction = form.find('span[id="ddl-preventiveAction"]');

        ddlReason = form.find("#ddl-reason").combobox();
        spanReason = form.find('span[id="ddl-reason"]');

        ddlTime = form.find("#ddl-time").combobox();
        ddlTime.combobox('disable');
        spanTime = form.find('span[id="ddl-time"]');

        txtLimitAmount = form.find("#txt-limitAmount");
    }

    function bindEvents() {
        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        ddlPreventiveAction.on("change", function () {
            var preventActionId = ddlPreventiveAction.val();
            if (preventActionId === '2') {
                $(".voidWinningOption").removeClass("display-none");
            } else {
                $(".voidWinningOption").addClass("display-none");
            }

            $("#ckb-time").prop("checked", false).change();
            $("#ckb-limitAmount").prop("checked", false).change();
        });

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            validateUpdate() && submitForm();
        });

        $(".note-info-message").tipsy({
            html: true,
            opacity: 0.9,
            gravity: "nw"
        });

        $("#ckb-time").change(function () {
            if (this.checked) {
                ddlTime.combobox('enable');
            } else {
                ddlTime.combobox('disable');
            }
            ddlTime.val('0').combobox('refresh');
        });

        $("#ckb-limitAmount").change(function () {
            if (this.checked) {
                txtLimitAmount.prop("disabled", false);
            } else {
                txtLimitAmount.prop("disabled", true);
            }
            txtLimitAmount.val("");
        });

        txtLimitAmount.keydown(function (e) { return CMS.forceNumericInput(e, true); });
        txtLimitAmount.on("cut copy paste", function (e) {
            e.preventDefault();
        });
    }

    function validateUpdate() {
        return isValidDropdownList(ddlPreventiveAction, spanPreventiveAction)
            && isValidDropdownList(ddlReason, spanReason)
            && isValidVoidTime();
    }

    function isValidVoidTime() {
        if ($("#ckb-time")[0].checked) {
            return isValidDropdownList(ddlTime, spanTime);
        } else {
            return true;
        }
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
