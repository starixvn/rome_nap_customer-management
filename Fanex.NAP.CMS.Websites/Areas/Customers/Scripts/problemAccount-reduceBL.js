"use strict";
function ProblemAccountReduceBL(form, reloadData) {
    var inlineMessage = null,
        maxPerEvent = null,
        maxPerBet = null;

    function initForm(usernames, level) {
        form.find("#txt-level").val(level);
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        inlineMessage = form.find("#div-inlineMessage");
        maxPerEvent = form.find("#txt-maxPerEvent");
        maxPerBet = form.find("#txt-maxPerBet");
    }

    function bindEvents() {
        maxPerEvent.add(maxPerBet)
            .off()
            .on("keypress", CMS.forceNumberInput)
            .on("keyup", function () { formatNumber($(this)); });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            isValidMaxPerEventMaxPerBet() && submitForm();
        });
    }

    function formatNumber(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var number = parseInt(tagValue);

        if (number.toString() === tagValue) {
            inputTag.val(numeral(number).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function isValidMaxPerEventMaxPerBet() {
        var maxPerEventValue = validateNumber(maxPerEvent),
            maxPerBetValue = validateNumber(maxPerBet);

        if (maxPerEventValue === -1 || maxPerBetValue === -1) {
            return false;
        }

        if (maxPerBetValue > maxPerEventValue) {
            CMS.showTipsyError(maxPerBet, resources.invalidMaxPerBetError, "w");
            return false;
        }

        CMS.clearTipsyError(maxPerEvent);
        CMS.clearTipsyError(maxPerBet);
        return true;
    }

    function validateNumber(inputTag) {
        var inputValue = $.trim(inputTag.val());

        if (inputValue.length === 0) {
            CMS.showTipsyError(inputTag, resources.requiredFieldError, "w");
            return -1;
        }

        if (!CMS.isNumeric(inputValue)) {
            CMS.showTipsyError(inputTag, resources.invalidValueError, "w");
            return -1;
        }

        var number = parseInt(inputValue.replace(/,/g, ""));

        if (number < 0) {
            CMS.showTipsyError(inputTag, resources.invalidValueError, "w");
            return -1;
        }

        return number;
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
