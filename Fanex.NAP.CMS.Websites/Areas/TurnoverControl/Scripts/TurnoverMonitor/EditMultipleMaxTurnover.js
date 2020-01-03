"use strict";
function EditMultipleMaxTurnover(table, reloadGridFunction) {
    var domElements = {
        allCheckbox: $("#checkbox-all"),
        editMaxTurnoverIcon: $("#icon-edit-maxturnover"),
        multipleEditMaxTOPopup: $("#multiple-edit-maxto-popup"),
        closeIcon: $("#icon-close"),
        resultMessageDiv: $("#div-result-message"),
        maxTurnoverTextbox: $("#txt-popup-maxto"),
        submitButton: $("#btn-popup-editmaxto")
    };
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
        } else {
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
                editParams = checkboxItems;
            }
        } else {
            closePopup();
        }
    }

    function getMaxTurnoverParam(custId, percentage, maxIntValue, maxTurnoverContainer) {
        var maxTurnoverText = maxTurnoverContainer.find("#link" + custId).text();
        var maxTurnover = parseInt(maxTurnoverText.replace(/,/g, ""));
        var maxTOTemp = Math.ceil(maxTurnover * percentage);

        if (maxTOTemp > maxIntValue) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.ExceedMaxTurnover, "sw");
            return null;
        }

        return {
            ProfileId: maxTurnoverContainer.data("profileid"),
            MaxTurnover: maxTOTemp
        };
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
            var checkbox = $(editParams[i]);
            var custId = checkbox.data("custid");
            var matchId = checkbox.data("matchid");
            var isGroupHeadline = checkbox.data("isgroupheadline");
            var sportType = checkbox.data("sporttype");

            var maxTurnoverContainers = checkbox.closest("#tr" + custId).find(".max-turnover-container");

            for (var j = 0, max = maxTurnoverContainers.length; j < max; j++) {
                var maxTurnoverParam = getMaxTurnoverParam(custId, percentage, maxIntValue, $(maxTurnoverContainers[j]));

                if (!maxTurnoverParam) {
                    return null;
                }

                editParamsTemp.push({
                    CustId: custId,
                    MatchId: matchId,
                    IsGroupHeadline: isGroupHeadline,
                    SportType: sportType,
                    ProfileId: maxTurnoverParam.ProfileId,
                    MaxTurnover: maxTurnoverParam.MaxTurnover
                });
            }
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
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/UpdateMultipleMaxTurnover"),
            editParams,
            function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, domElements.resultMessageDiv);
                } else {
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