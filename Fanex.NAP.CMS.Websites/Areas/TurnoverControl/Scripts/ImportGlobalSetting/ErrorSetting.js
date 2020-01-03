"use strict";
function ErrorSetting() {
    var table = $("#errorsetting-list");
    var gridOptions = null;

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: viewResources.Symbol_No,
                Width: 60,
                Name: "ColNo",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.ExcelRow,
                Width: 60,
                Name: "ExcelRow",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.UserName,
                Width: 160,
                Name: "Username",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: viewResources.Field,
                Width: 120,
                Name: "Field",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.Value,
                Width: 120,
                Name: "Value",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.Reason,
                Width: 250,
                Name: "Reason",
                HeaderAlign: "center",
                CellAlign: "left"
            }];

        return {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                text: viewResources.ErrorSetting
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: true,
            customPager: false,
            pagerOption: {
                itemsPerPage: 100,
                itemsPerPageArray: CMS.pagerRanges,
                currentPage: 0
            },
            gridExpandHeight: 680
        };
    }

    function renderSettings(data) {
        var bodyData = [];
        var customer;

        for (var i = 0, max = data.length; i < max; i++) {
            customer = data[i];

            bodyData.push({
                Columns: [
                    { Name: "ColNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "ExcelRow", Value: customer.ExcelRow },
                    { Name: "Username", Value: customer.Username },
                    { Name: "Field", Value: customer.Field },
                    { Name: "Value", Value: customer.Value },
                    { Name: "Reason", Value: renderReason(customer.Reason) }]
            });
        }

        return bodyData;
    }

    function renderReason(reason) {
        var html = ['<ul class="col-reason">'];

        var reasonArray = reason.split(",");

        for (var i = 0, max = reasonArray.length; i < max; i++) {
            html.push("<li>");
            html.push(reasonArray[i]);
            html.push("</li>");
        }

        html.push("</ul>");

        return html.join("");
    }

    this.initialize = function (data) {
        gridOptions = gridOptions || getGridOptions();
        gridOptions.bodyRows = renderSettings(data);

        table.removeClass("display-none").fanexGrid(gridOptions);
    };
}