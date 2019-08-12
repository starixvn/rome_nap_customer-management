var problemAccountHelper = (function ($, CMS, CMSConstants, resources, reloadData) {
    "use strict";
    var inlineMessage = null;

    function showSelectedAccounts(form, problemAccount) {
        form.find("#txt-usernames").val(problemAccount.usernames);
        form.find("#txt-level").val(problemAccount.level);
        form.find("#span-selectedAccounts").text(problemAccount.usernames);
    }

    function bindEvents(form, formValidationHandler) {
        inlineMessage = form.find("#div-inlineMessage");

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel.off("click").on("click", CMS.closePopup);

        form.off("submit").on("submit", function (event) {
            event.preventDefault();

            var isValidFormSubmit = (formValidationHandler && typeof (formValidationHandler) === "function")
                ? formValidationHandler()
                : true;

            if (isValidFormSubmit) {
                CMS.disableInput();
                var url = form.attr("action"),
                    formData = form.serialize();

                $.post(url, formData, successHandler, "json")
                    .fail(function (xhr) { CMS.handleAjaxError(xhr); })
                    .always(function () { CMS.enableInput(); });
            }
        });
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);

        if (jsonData.Code !== 0) {
            CMS.showInlineErrorMsg(jsonData.Message, inlineMessage);
        } else {
            CMS.showInlineSuccessMsg(jsonData.Message, inlineMessage, true, function () {
                CMS.closePopup();
                reloadData();
            });
        }
    }

    function restrictInputMaxPerEventMaxPerBet(maxPerEvent, maxPerBet) {
        maxPerEvent.add(maxPerBet)
            .off()
            .on("keypress", CMS.forceNumberInput)
            .on("keyup", function () { formatNumber($(this)); });
    }

    function formatNumber(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var number = parseInt(tagValue);

        if (number.toString() === tagValue) {
            inputTag.val(numeral(number).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function validateMaxPerEventMaxPerBet(maxPerEvent, maxPerBet) {
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

    return {
        showSelectedAccounts: showSelectedAccounts,
        bindEvents: bindEvents,
        restrictInputMaxPerEventMaxPerBet: restrictInputMaxPerEventMaxPerBet,
        validateMaxPerEventMaxPerBet: validateMaxPerEventMaxPerBet
    };
})(jQuery, CMS, CMSConstants, resources, problemAccountSearch.reloadData);