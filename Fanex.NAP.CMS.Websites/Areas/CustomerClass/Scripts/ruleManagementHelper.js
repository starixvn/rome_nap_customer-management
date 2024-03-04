"use strict";

const ruleManagementHelper = (function () {
    function renderIsLiveCell(value) {
        return !value ? "No" : value === 1 ? "Yes" : "All";
    }

    function renderPercentCell(value) {
        return typeof value !== "undefined" ? `${numeral(value).format(CMS.formats.IntegerFormat)}%` : "";
    }

    function renderSeparateCell(value1, value2, value3) {
        return [
            '<span class="text-green">',
            formatInteger(value1, "--"),
            '</span> | ',
            '<span class="text-red">',
            formatInteger(value2, "--"),
            '</span> | ',
            '<span class="text-blue">',
            formatInteger(value3, "--"),
            '</span>'
        ].join("");
    }

    function renderBooleanCell(value) {
        return value  ? "Yes" : "No";
    }

    function formatInteger(value, defaultValue) {
        return typeof value !== "undefined" ? numeral(value).format(CMS.formats.IntegerFormat) : defaultValue || "";
    }

    return {
        renderIsLiveCell: renderIsLiveCell,
        renderPercentCell: renderPercentCell,
        renderSeparateCell: renderSeparateCell,
        renderBooleanCell: renderBooleanCell,
        formatInteger: formatInteger
    };
})();