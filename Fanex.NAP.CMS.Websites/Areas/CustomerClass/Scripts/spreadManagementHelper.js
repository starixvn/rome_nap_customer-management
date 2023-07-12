"use strict";

const spreadManagementHelper = (function () {
    function isEmptyValue(value) {
        return typeof value === "undefined" || value === null || value === "";
    }

    function validateRequiredField(div, value) {
        if (isEmptyValue(value)) {
            CMS.showTipsyError(div, resources.requiredField, "ne");
            return;
        }

        return value;
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
        isValidRule: isValidRule
    };
})();