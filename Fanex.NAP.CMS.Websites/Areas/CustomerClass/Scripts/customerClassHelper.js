"use strict";

const customerClassHelper = (function ($) {
    function renderMultiSelect(selectElementId, options) {
        return $(selectElementId).fanexMultiSelect($.extend({
            checkAllText: "All",
            noneSelectedText: "Please select",
            noItemText: "There is no data",
            noItemFoundText: "No item found",
            filterPlaceHolder: "",
            isCheckAll: true
        }, options));
    }

    function showInlineMessageAndClosePopup(result, inlineMessage, successfullyCallbackFunc) {
        const resultJson = typeof result === "string" ? JSON.parse(result) : result;
        if (resultJson.IsSuccess) {
            CMS.showInlineSuccessMsg(
                resultJson.Message,
                inlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    successfullyCallbackFunc();
                });
        } else {
            CMS.showInlineErrorMsg(resultJson.Message, inlineMessage);
        }
    }

    function isASCII(text) {
        for (let i = 0, max = text.length; i < max; i++) {
            if (text.charCodeAt(i) > 127) {
                return false;
            }
        }

        return true;
    }

    function getSelectedReplaceAllValue(divId, allvalue, allItemsLength) {
        var selectedItems = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedItems.length === allItemsLength) {
            return allvalue;
        }

        return selectedItems.join();
    }

    function buildGroupSiteList(isAllCredit, isAllLicensee, selectedValue, cachedCreditSites, cachedLicenseeSites) {
        let creditOptions = [];
        let licenseeOptions = [];
        let selectedValueList = selectedValue.split(",");
        cachedCreditSites.forEach(function (item) {
            creditOptions.push({
                value: item.value,
                text: item.text,
                checked: selectedValue === "All" || isAllCredit || selectedValueList.includes(item.value),
                enable: true
            });
        });
        cachedLicenseeSites.forEach(function (item) {
            licenseeOptions.push({
                value: item.value,
                text: item.text,
                checked: selectedValue === "All" || isAllLicensee || selectedValueList.includes(item.value),
                enable: true
            });
        });

        return [
            {
                value: "credit",
                text: "Credit",
                checked: isAllCredit,
                enable: true,
                items: creditOptions
            },
            {
                value: "licensee",
                text: "Licensee",
                checked: isAllLicensee,
                enable: true,
                items: licenseeOptions
            }
        ];
    }

    return {
        renderMultiSelect: renderMultiSelect,
        showInlineMessageAndClosePopup: showInlineMessageAndClosePopup,
        isASCII: isASCII,
        getSelectedReplaceAllValue: getSelectedReplaceAllValue,
        buildGroupSiteList: buildGroupSiteList
    };
})(jQuery);