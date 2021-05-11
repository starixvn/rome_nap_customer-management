var VREngineHelper = (function ($) {
    function renderCombobox(id, dataList, selectedValue) {
        var datum, html = [];
        if (typeof selectedValue === "undefined") {
            selectedValue = dataList[0].Id;
        }

        for (var i = 0, max = dataList.length; i < max; i++) {
            datum = dataList[i];
            html.push('<option value="');
            html.push(datum.Id);
            html.push('"');

            if (datum.Id === selectedValue) {
                html.push(' selected');
            }

            html.push('>');
            html.push(datum.Name);
            html.push('</option>');
        }

        return $("#" + id).html(html.join("")).combobox();
    }

    function renderMultiSelect(id, dataList, selectedList, maxHeight) {
        var datum,
            selectedObject,
            data = [],
            max = dataList.length;
        var isCheckAll = typeof selectedList === "undefined" || selectedList === "0" || selectedList === "-1";

        if (!isCheckAll) {
            selectedObject = toObject(selectedList.split(","));
        }

        for (var i = 0; i < max; i++) {
            datum = dataList[i];
            data.push({
                Id: id + "-" + datum.Id,
                Value: datum.Id,
                Text: datum.Name,
                Checked: isCheckAll || selectedObject.hasOwnProperty(datum.Id),
                Enable: true
            });
        }

        var selector = "#" + id;
        $(selector).multiSelect(getMultiSelectOptions(isCheckAll, data, maxHeight));
        return $(selector);
    }

    function updateMultiSelect($ddl, dataList) {
        var datum,
            data = [],
            id = $ddl.attr("id");
        for (var i = 0, max = dataList.length; i < max; i++) {
            datum = dataList[i];
            data.push({
                Id: id + "-" + datum.Id,
                Value: datum.Id,
                Text: datum.Name,
                Checked: true,
                Enable: true
            });
        }

        $ddl.multiSelect("updateItems", getMultiSelectOptions(true, data));
        return $("#" + id);
    }

    function getMultiSelectOptions(isCheckAll, listData, maxHeight) {
        return {
            showTooltip: false,
            shadow: true,
            maxHeight: maxHeight || 350,
            isCheckAll: isCheckAll,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            tooltipNoItem: "Please select at least one option",
            listData: listData
        };
    }

    function toObject(array) {
        var object = {};

        for (var i = 0, max = array.length; i < max; i++) {
            object[array[i]] = true;
        }

        return object;
    }

    function validateNumber(txtNumber) {
        var inputNumber = $.trim(txtNumber.val());

        if (!inputNumber.length) {
            CMS.showTipsyError(txtNumber, resources.requiredFieldError, "nw");
            return;
        }

        if (!CMS.isNumeric(inputNumber)) {
            CMS.showTipsyError(txtNumber, resources.invalidValueError, "nw");
            return;
        }

        return parseInt(inputNumber);
    }

    function extractHourMinute(time) {
        return time.substring(0, 5);
    }

    return {
        renderCombobox: renderCombobox,
        renderMultiSelect: renderMultiSelect,
        updateMultiSelect: updateMultiSelect,
        validateNumber: validateNumber,
        extractHourMinute: extractHourMinute
    };
})(jQuery);