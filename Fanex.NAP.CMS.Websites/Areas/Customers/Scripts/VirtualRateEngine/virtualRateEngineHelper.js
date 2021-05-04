var virtualRateEngineHelper = (function ($) {
    function renderCombobox(id, dataList, selectedValue) {
        var datum, html = [];
        selectedValue = selectedValue || dataList[0].Id;

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
        var isCheckAll = !selectedList || selectedList.length === max;

        if (!isCheckAll) {
            selectedObject = toObject(selectedList);
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
        $(selector).multiSelect({
            showTooltip: false,
            shadow: true,
            maxHeight: maxHeight || 350,
            isCheckAll: isCheckAll,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            tooltipNoItem: "Please select at least one option",
            listData: data
        });

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

        $ddl.multiSelect("updateItems", {
            shadow: true,
            isCheckAll: true,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            tooltipNoItem: "Please select at least one option",
            listData: data
        });

        return $("#" + id);
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

        return parseInt(inputNumber.replace(/,/g, ""));
    }

    return {
        renderCombobox: renderCombobox,
        renderMultiSelect: renderMultiSelect,
        updateMultiSelect: updateMultiSelect,
        validateNumber: validateNumber
    };
})(jQuery);