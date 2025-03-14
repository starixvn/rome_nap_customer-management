"use strict";

const betLimitIncreaseAddOrEdit = (function ($) {
    let addNewOrEditForm = null,
        isAddNew = null,
        inlineMessage = null,
        txtUsernameAdd = null,
        txtMultiplier = null,
        reloadBetLimitGrid = null;

    function init(reloadGridFunc) {
        reloadBetLimitGrid = reloadGridFunc;
        addNewOrEditForm = $("#frmAdd");
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    }

    function openModal(betLimitIncrease) {
        isAddNew = typeof betLimitIncrease === "undefined";
        CMS.modal(
            isAddNew ? resources.addBetLimitIncrease : resources.editBetLimitIncrease,
            addNewOrEditForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

        betLimitIncrease = betLimitIncrease || {};
        cacheElements(betLimitIncrease);
        bindEvents(betLimitIncrease);
    }

    function cacheElements(betLimitIncrease) {
        inlineMessage = $("#msgAdd");
        txtUsernameAdd = $("#txtUsernameAdd");
        $("#txtUsernameEdit").html(betLimitIncrease.Username);
        txtMultiplier = $("#txtMultiplier").val(betLimitIncrease.Multiplier);
        $("#txtNote").val(betLimitIncrease.Note);
    }

    function bindEvents(betLimitIncrease) {
        $("#ddlInputSportType").combobox().combobox("disable");
        $("#ddlInputBetType").combobox().combobox("disable");
        if (!isAddNew) {
            $("#edit-username-container").removeClass("display-none");
            $("#add-username-container").addClass("display-none");
        }
        txtUsernameAdd.blur(() => txtUsernameAdd.val(replaceCharsCommaInput(txtUsernameAdd.val())));
        txtMultiplier.keydown((e) => handleNumberCommanTextInput(e));
        $("#btnSave")
            .off("click")
            .click(function () {
                clearAllErrors();
                const formData = getFormData();
                formData && handleOnSave(formData, betLimitIncrease);
            });

        $("#btnCancel").off("click").click(CMS.closePopup);

        $(".light-note-info-message").tipsy({
            className: "custom-grid-colheader-note",
            html: true,
            opacity: 0.9,
            gravity: "n"
        });
    }

    function getFormData() {
        var username = validateUsername();
        var multiplier = validateMultiplier();
        var note = $("#txtNote").val();
        if ((username || !isAddNew) && multiplier) {
            return { username, multiplier, note };
        }
    }

    function validateMultiplier() {
        const multiplier = txtMultiplier.val();

        if (!multiplier.length) {
            CMS.showTipsyError(txtMultiplier, resources.requiredField, "w");
            return;
        }

        if (!CMS.isNumeric(multiplier)) {
            CMS.showTipsyError(txtMultiplier, resources.invalidValueError, "w");
            return;
        }

        var number = parseFloat(multiplier.replace(/,/g, ""));

        if (number <= 0) {
            CMS.showTipsyError(txtMultiplier, resources.invalidMinRangeError, "w");
            return;
        }

        if (number > 5) {
            CMS.showTipsyError(txtMultiplier, resources.invalidMaxRangeError, "w");
            return;
        }

        return number;
    }

    function validateUsername() {
        const username = $.trim(txtUsernameAdd.val());
        if (!username.length) {
            CMS.showTipsyError(txtUsernameAdd, resources.requiredField, "w");
            return;
        }

        if (/<|&|"/.test(username) || !isASCII(username)) {
            CMS.showTipsyError(txtUsernameAdd, `Username cannot contain special characters ('<', '&' and ' " '), symbol and punctuation.`, "w");
            return;
        }

        if (username.split(',').length > 200) {
            CMS.showTipsyError(txtUsernameAdd, `Only allow up to 200 usernames.`, "w");
            return;
        }

        return username;
    }

    function clearAllErrors() {
        CMS.clearTipsyError(txtMultiplier);
        CMS.clearTipsyError(txtUsernameAdd);
        CMS.hideInlineMsg(inlineMessage);
    }

    function handleOnSave(formData, betLimitIncrease) {
        let action;
        betLimitIncrease.Multiplier = formData.multiplier;
        betLimitIncrease.Note = formData.note;
        if (isAddNew) {
            action = "Add";
            betLimitIncrease.Username = formData.username;
        } else {
            action = "Update";
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`BetLimitIncrease/${action}`),
            betLimitIncrease,
            function (result) {
                showInlineMessageAndClosePopup(result, inlineMessage, reloadBetLimitGrid);
            });
    }

    function isASCII(text) {
        for (let i = 0, max = text.length; i < max; i++) {
            if (text.charCodeAt(i) > 127) {
                return false;
            }
        }

        return true;
    }

    function showInlineMessageAndClosePopup(result, messageDiv, successfullyCallbackFunc, duration = 2000) {
        const resultJson = typeof result === "string" ? JSON.parse(result) : result;
        if (resultJson.IsSuccess) {
            CMS.showInlineSuccessMsg(
                resultJson.Message,
                inlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    successfullyCallbackFunc();
                },
                duration);
        } else {
            CMS.showInlineErrorMsg(resultJson.Message, messageDiv);
        }
    }

    function replaceCharsCommaInput(str) {
        str = str.replace(/\s\s/g, ' ')
            .replace(/\s/g, ',')
            .replace(/\t/g, ',')
            .replace(/;/g, ',')
            .replace(/\\n|\\r\\n|\\r/g, ',')
            .replaceAll(/,+/g, ',');

        if (str.startsWith(',')) {
            str = str.substr(1);
        }

        if (str.endsWith(',')) {
            str = str.substr(0, str.length - 1);
        }

        return str;
    }

    function handleNumberCommanTextInput(e) {
        const keyCodeArray = [46, 8, 9, 27, 13, 188, 190];
        if (keyCodeArray.indexOf(e.keyCode) !== -1) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || (e.keyCode > 105 && e.keyCode != 110))) {
            e.preventDefault();
        }
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);