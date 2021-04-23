var virtualRateEngineHelper = (function ($) {
    function renderCombobox(id, dataList, selectedValue) {
        var html = [];
        $.each(dataList, function (index, datum) {
            html.push('<option value="');
            html.push(datum.Id);
            html.push('">');
            html.push(datum.Name);
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
                Id: datum.Id,
                Value: datum.Id,
                Text: datum.Name,
                Checked: true,
                Enable: true
            });
        });

        var $multiSelect = $("#" + id).multiSelect({
            shadow: true,
            isCheckAll: true,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            listData: data
        });

        return $multiSelect;
    }

    return {
        renderCombobox: renderCombobox,
        renderMultiSelect: renderMultiSelect
    };
})(jQuery);