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

    return {
        renderMultiSelect: renderMultiSelect,
        showInlineMessageAndClosePopup: showInlineMessageAndClosePopup,
        isASCII: isASCII
    };
})(jQuery);