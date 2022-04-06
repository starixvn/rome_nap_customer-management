"use strict";
(function ($) {
    function getGridOptions(data) {
        var bodyRows = [];
        $.each(data, function (index, datum) {
            bodyRows.push(renderBodyRow(datum.Id, datum.Name));
        });

        return {
            showDistinctRow: false,
            gridExpandHeight: 100,
            columns: [
                { HeaderText: "Class", Width: 120, Name: "colClass", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Category", Width: 320, Name: "colCategory", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows
        };
    }

    function renderBodyRow(classValue, category) {
        return {
            Columns: [
                { Name: "colClass", Value: classValue },
                { Name: "colCategory", Value: category }
            ]
        };
    }

    $(function () {
        $("#freegrid")
            .removeClass("display-none")
            .fanexGrid(getGridOptions(customerClasses));
    });
})(jQuery);