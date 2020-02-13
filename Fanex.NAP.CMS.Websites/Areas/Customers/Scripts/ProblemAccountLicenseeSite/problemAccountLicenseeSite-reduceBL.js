"use strict";
function ProblemAccountLicenseeSiteReduceBL(form, reloadData) {
    var inlineMessage = null,
        maxPerMatch = null,
        maxBet = null;

    function initForm(usernames, currency) {
        form.find("#txt-usernames").val(usernames.join(","));
        form.find("#span-selectedAccounts").text(usernames.join(", "));
        form.find(".span-currency").text(" (" + currency + ")");
        inlineMessage = form.find("#div-inlineMessage");
        maxPerMatch = form.find("#txt-maxPerMatch");
        maxBet = form.find("#txt-maxBet");
    }

    function bindEvents() {
        maxPerMatch.add(maxBet)
            .off()
            .on("keypress", CMS.forceNumberInput)
            .on("keyup", function () { formatNumber($(this)); });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();
            isValidMaxPerMatchMaxBet() && submitForm();
        });
    }

    function formatNumber(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var number = parseInt(tagValue);

        if (number.toString() === tagValue) {
            inputTag.val(numeral(number).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function isValidMaxPerMatchMaxBet() {
        var maxPerMatchValue = validateNumber(maxPerMatch),
            maxBetValue = validateNumber(maxBet);

        if (maxPerMatchValue === -1 || maxBetValue === -1) {
            return false;
        }

        if (maxBetValue > maxPerMatchValue) {
            CMS.showTipsyError(maxBet, resources.invalidMaxBetError, "w");
            return false;
        }

        CMS.clearTipsyError(maxPerMatch);
        CMS.clearTipsyError(maxBet);
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

    this.init = function (modalTitle, usernames, currency) {
        CMS.modal(modalTitle, form, { height: "auto" });

        initForm(usernames, currency);
        bindEvents();
    };
}
