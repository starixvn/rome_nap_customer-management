"use strict";
function AddGlobalSetting(viewResources, form, reloadData, sportType) {
    var inlineMessage = null,
        smaRadio = null,
        headlineRadio = null,
        usernameTextbox = null,
        maxTurnoverLabel = null,
        maxTurnoverTextboxes = null;

    function cacheElements() {
        inlineMessage = form.find("#div-inlineMessage");
        smaRadio = form.find("#rdoSMA");
        headlineRadio = form.find("#rdoHeadline");
        usernameTextbox = form.find("#txt-addUsername").focus();
        maxTurnoverLabel = form.find("#lbl-maxturnover");
        maxTurnoverTextboxes = form.find(".txt-maxturnover");
    }

    function bindEvents() {
        smaRadio
            .off("change")
            .on("change", function (event) {
                maxTurnoverLabel.text(viewResources.Max_Turnover);
            });

        headlineRadio
            .off("change")
            .on("change", function (event) {
                maxTurnoverLabel.text(viewResources.Max_Turnover + " " + viewResources.RM);
            });

        maxTurnoverTextboxes
            .off()
            .on("keypress", CMS.forceNumberInput)
            .on("keyup", function () { formatNumber($(this)); });

        form
            .off("submit")
            .on("submit", function (event) {
                event.preventDefault();
                var formData = getFormData();
                formData && submitForm(formData);
            });

        var buttonCancel = form.find(".red-button");
        buttonCancel
            .off("click")
            .on("click", CMS.closePopup);
    }

    function formatNumber(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var number = parseInt(tagValue);

        if (number.toString() === tagValue) {
            inputTag.val(numeral(number).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function getFormData() {
        var username = getUsername();

        if (username) {
            var maxTurnovers = getAllMaxTurnovers();
            
            if (maxTurnovers.length) {
                return {
                    isGroupHeadline: headlineRadio.is(":checked"),
                    username: username,
                    sportType: sportType,
                    maxTurnovers: maxTurnovers
                };
            }
        }
    }

    function getUsername() {
        var username = $.trim(usernameTextbox.val());

        if (username.length === 0) {
            CMS.showTipsyError(usernameTextbox, viewResources.Error_RequiredField, "w");
        } else {
            CMS.clearTipsyError(usernameTextbox);
        }

        return username;
    }

    function getAllMaxTurnovers() {
        var maxTurnovers = [];

        $.each(maxTurnoverTextboxes, function (index, item) {
            var $textbox = $(item);
            var maxTurnoverValue = getMaxTurnover($textbox);
            maxTurnoverValue >= 0 && maxTurnovers.push({ profileId: $textbox.data("profileid"), maxTurnover: maxTurnoverValue });
        });

        return maxTurnovers.length === maxTurnoverTextboxes.length ? maxTurnovers : [];
    }

    function getMaxTurnover(inputTag) {
        var inputValue = $.trim(inputTag.val());

        if (inputValue.length === 0) {
            CMS.showTipsyError(inputTag, viewResources.Error_RequiredField, "w");
            return -1;
        }

        if (!CMS.isNumeric(inputValue)) {
            CMS.showTipsyError(inputTag, viewResources.Error_InvalidValue, "w");
            return -1;
        }

        var number = parseInt(inputValue.replace(/,/g, ""));

        if (number < 0) {
            CMS.showTipsyError(inputTag, viewResources.Error_InvalidValue, "w");
            return -1;
        }

        if (number > maxIntValue) {
            CMS.showTipsyError(inputTag, viewResources.GlobalSetting_MaxTurnover_ExceedMaxTurnover, "w");
            return -1;
        }

        CMS.clearTipsyError(inputTag);
        return number;
    }

    function submitForm(formData) {
        CMS.disableInput();
        CMS.callAjax(this, form.attr("action"), formData, function (data) {
            var jsonData = $.parseJSON(data);

            if (jsonData.Code === 0) {
                CMS.showInlineSuccessMsg(jsonData.Message, inlineMessage, true, function () {
                    CMS.closePopup();
                    reloadData();
                });
            } else {
                CMS.showInlineErrorMsg(jsonData.Message, inlineMessage);
            }
        });
    }

    this.initialize = function () {
        CMS.modal(viewResources.GlobalSetting_AddNewSMA_Title, form, { resizable: false });

        cacheElements();
        bindEvents();
    };
}