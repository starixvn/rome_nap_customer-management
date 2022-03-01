"use strict";
(function ($) {
    function getGridOptions(data) {
        var cssClassNames = {
            111: "bg-yellow",
            112: "bg-yellow",
            113: "bg-yellow",
            121: "bg-yellow",
            122: "bg-yellow",
            123: "bg-yellow",
            31: "bg-yellow",
            32: "bg-yellow",
            33: "bg-yellow",
            51: "bg-yellow",
            52: "bg-yellow",
            53: "bg-yellow",
            61: "bg-yellow",
            62: "bg-yellow",
            63: "bg-yellow",
            71: "bg-yellow",
            72: "bg-yellow",
            73: "bg-yellow",
            8: "bg-red",
            9: "bg-red",
            10: "bg-red",
            11: "bg-red",
            12: "bg-red",
            13: "bg-red",
            131: "bg-red",
            132: "bg-red",
            100: "bg-green"
        };

        var bodyRows = [];
        $.each(data, function (index, datum) {
            bodyRows.push(renderBodyRow(datum.Id, datum.Name, cssClassNames[datum.Id]));
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

    function renderBodyRow(classValue, category, tdClass) {
        return {
            Columns: [
                { Name: "colClass", Value: classValue, TdClass: tdClass },
                { Name: "colCategory", Value: category, TdClass: tdClass }
            ]
        };
    }

    $(function () {
        $("#freegrid")
            .removeClass("display-none")
            .fanexGrid(getGridOptions(customerClasses));
    });
})(jQuery);