var virtualRateEngineHelper = (function ($) {
    function renderCombobox(id, dataList, selectedValue) {
        var html = [];
        $.each(dataList, function (index, datum) {
            html.push('<option value="');
            html.push(datum.Value);
            html.push('">');
            html.push(datum.Text);
            html.push('</option>');
        });

        var $combobox = $("#" + id).html(html.join("")).combobox();
        selectedValue && $combobox.val(selectedValue).combobox("refresh");

        return $combobox;
    }

    function renderMultiSelect(id, dataList) {
        var data = [];
        $.each(dataList, function (index, datum) {
            data.push({
                Id: datum.Value,
                Value: datum.Value,
                Text: datum.Text,
                Checked: true,
                Enable: true
            });
        });

        var $multiSelect = $("#" + id).multiSelect({
            minWidth: 255,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            listData: data,
            noItemText: "No item selected."
        });

        return $multiSelect;
    }

    return {
        renderCombobox: renderCombobox,
        renderMultiSelect: renderMultiSelect
    };
})(jQuery);