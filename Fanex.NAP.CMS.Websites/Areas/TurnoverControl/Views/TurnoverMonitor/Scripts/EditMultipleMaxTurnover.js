function EditMultipleMaxTurnover(table, reloadGridFunction) {
    "use strict";
    var domElements = {
        allCheckbox: $("#checkbox-all"),
        editMaxTurnoverIcon: $("#icon-edit-maxturnover"),
        multipleEditMaxTOPopup: $("#multiple-edit-maxto-popup"),
        closeIcon: $("#icon-close"),
        resultMessageDiv: $("#div-result-message"),
        maxTurnoverTextbox: $("#txt-popup-maxto"),
        submitButton: $("#btn-popup-editmaxto")
    };
    var editUrl = rootUrl + "TurnoverControl/TurnoverMonitor/UpdateMultipleMaxTurnover";
    var editParams = null;

    function formatMaxTurnover() {
        var tagValue = domElements.maxTurnoverTextbox.val().replace(/,/g, "");
        var maxTurnover = parseInt(tagValue);

        if (maxTurnover.toString() === tagValue) {
            domElements.maxTurnoverTextbox.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function closePopup() {
        CMS.clearTipsyError(domElements.maxTurnoverTextbox);
        domElements.maxTurnoverTextbox.val("");
        domElements.multipleEditMaxTOPopup.addClass("display-none");
        editParams = null;
    }

    function getEditParams(checkboxItems) {
        var postParams = [];
        var item;

        for (var i = 0, max = checkboxItems.length; i < max; i++) {
            item = $(checkboxItems[i]);

            postParams.push({
                CustId: item.data("custid"),
                MatchId: item.data("matchid"),
                IsGroupHeadline: item.data("isgroupheadline"),
                MaxTurnover: parseInt(item.data("maxturnover"))
            });
        }

        return postParams;
    }

    function allCheckboxChangedHandler() {
        var checkboxItems = table.find('.col-checkbox-body input[type="checkbox"]');
        var checked = this.checked;

        $.each(checkboxItems, function (index, item) {
            item.checked = checked;
        });
    }

    function checkboxChangedHandler() {
        var checkboxItems = table.find('.col-checkbox-body input[type="checkbox"]');
        var checkboxCount = checkboxItems.length;

        if (this.checked) {
            for (var i = 0; i < checkboxCount; i++) {
                if (!checkboxItems[i].checked) {
                    domElements.allCheckbox.prop("indeterminate", true);
                    return;
                }
            }

            domElements.allCheckbox
                .prop("indeterminate", false)
                .prop("checked", true);
        }
        else {
            for (var j = 0; j < checkboxCount; j++) {
                if (checkboxItems[j].checked) {
                    domElements.allCheckbox.prop("indeterminate", true);
                    return;
                }
            }

            domElements.allCheckbox
                .prop("indeterminate", false)
                .prop("checked", false);
        }
    }

    function openPopupHandler() {
        if (domElements.multipleEditMaxTOPopup.hasClass("display-none")) {
            var checkboxItems = table.find('.col-checkbox-body input[type="checkbox"]:checked');

            if (checkboxItems.length > 0) {
                domElements.multipleEditMaxTOPopup.removeClass("display-none");
                domElements.maxTurnoverTextbox.focus();
                editParams = getEditParams(checkboxItems);
            }
        }
        else {
            closePopup();
        }
    }

    function updateEditParams() {
        CMS.clearTipsyError(domElements.maxTurnoverTextbox);
        var percentageValue = $.trim(domElements.maxTurnoverTextbox.val());

        if (percentageValue.length === 0) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.Error_RequiredField, "sw");
            return null;
        }

        if (!CMS.isNumeric(percentageValue)) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.InvalidValueError, "sw");
            return null;
        }

        var percentage = parseInt(percentageValue.replace(/,/g, ""));

        if (percentage < -100 || percentage > 1000) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.InvalidValueError, "sw");
            return null;
        }

        if (percentage === 0) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.Error_NoActionOccured, "sw");
            return null;
        }

        percentage = 1 + percentage / 100;
        var editParamsTemp = [];
        var editParamsLength = editParams.length;

        for (var i = 0; i < editParamsLength; i++) {
            var setting = editParams[i];
            var maxTOTemp = Math.ceil(setting.MaxTurnover * percentage);

            if (maxTOTemp > maxIntValue) {
                CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.ExceedMaxTurnover, "sw");
                return null;
            }

            editParamsTemp.push({
                CustId: setting.CustId,
                MatchId: setting.MatchId,
                IsGroupHeadline: setting.IsGroupHeadline,
                MaxTurnover: maxTOTemp
            });
        }

        editParams = editParamsTemp;

        return editParams;
    }

    function multipleEditHandler() {
        if (!updateEditParams()) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            editUrl,
            { GlobalSettingItems: editParams },
            function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, domElements.resultMessageDiv);
                }
                else {
                    CMS.showInlineSuccessMsg(
                        result.Message,
                        domElements.resultMessageDiv,
                        true,
                        function () {
                            closePopup();
                            reloadGridFunction();
                        });
                }
            });
    }

    this.initialize = function () {
        domElements.allCheckbox
            .off("change")
            .on("change", allCheckboxChangedHandler);

        table.on("change", '.col-checkbox-body input[type="checkbox"]', checkboxChangedHandler);

        domElements.editMaxTurnoverIcon
            .off("click")
            .on("click", openPopupHandler);

        domElements.closeIcon
            .off("click")
            .on("click", closePopup);

        domElements.maxTurnoverTextbox
            .off()
            .on("keypress", CMS.forceNegativeNumberInput)
            .on("keyup", function (e) {
                var $this = $(this);
                var code = e.keyCode || e.which;

                if (code === keyboardManager.Enums.KeyCode.Enter) {
                    multipleEditHandler();
                    return;
                }

                formatMaxTurnover();
            });

        domElements.submitButton
            .off("click")
            .on("click", multipleEditHandler);
    };

    this.getAllCheckbox = function () {
        return domElements.allCheckbox;
    };
}