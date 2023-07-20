"use strict";

const spreadManagementHelper = (function () {
    function isEmptyValue(value) {
        return typeof value === "undefined" || value === null || value === "";
    }


    function validateRequiredField(div, value) {
        if (isEmptyValue(value)) {
            CMS.showTipsyError(div, resources.requiredField, "n");
            return;
        }

        return value;
    }

    function validateTitle(inputElement, gravity) {
        const textValue = $.trim(inputElement.val());
        if (!textValue.length) {
            CMS.showTipsyError(inputElement, resources.requiredField, "nw");
            return;
        }

        if (!customerClassHelper.isASCII(textValue)) {
            CMS.showTipsyError(inputElement, `Title cannot contain symbols.`, "nw");
            return;
        }

        return textValue;
    }

    function isValidRule(rule) {
        return !isEmptyValue(rule.PolicyTitle)
            && !isEmptyValue(rule.CustomerClass)
            && !isEmptyValue(rule.Currencies)
            && !isEmptyValue(rule.Sites)
            && !isEmptyValue(rule.Platforms)
            && !isEmptyValue(rule.Spread);
    }

    return {
        isEmptyValue: isEmptyValue,
        validateRequiredField: validateRequiredField,
        validateTitle: validateTitle,
        isValidRule: isValidRule
    };
})();