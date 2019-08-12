/* global CMS, resources */
(function ($) {
    "use strict";
    var gridOptions = null;
    var elements = {
        dataGrid: $("#freegrid")
    };

    function initGridOptions() {
        var columns = [
            { HeaderText: resources.voidDate, Width: 160, Name: "colVoidDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betId, Width: 160, Name: "colBetId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.abnormalReason, Width: 320, Name: "colAbnormalReason", HeaderAlign: "center", CellAlign: "left" }
        ];

        return {
            showTitle: true,
            titleInfo: {
                text: resources.abnormalBetTicketTitle,
                showBackButton: true,
                backFunction: function () {
                    window.location.href = CMS.resolveRootUrl("Customers/ProblemAccount/Index");
                },
                hideWhenNoData: false
            },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 190,
            columns: columns,
            showPager: false,
        };
    }

    function renderAbnormalBetTickets(data) {
        var bodyRows = [];
        var cell, ticket;
        var dataLength = data ? data.length : 0;
        for (var i = 0; i < dataLength; i++) {
            ticket = data[i];
            cell = [
                { Name: "colVoidDate", Value: ticket.VoidDate },
                { Name: "colBetId", Value: ticket.BetId },
                { Name: "colAbnormalReason", Value: ticket.AbnormalReason }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function initialize() {
        gridOptions = gridOptions || initGridOptions();
        gridOptions.bodyRows = renderAbnormalBetTickets([
            {
                VoidDate: "01/31/2019 23:59:59",
                BetId: "123456789",
                AbnormalReason: "Group Betting"
            },
            {
                VoidDate: "02/15/2019 23:59:59",
                BetId: "987654321",
                AbnormalReason: "Arbitrage"
            }
        ]);

        elements.dataGrid.removeClass("display-none").fanexGrid(gridOptions);
        //CMS.getJson(
        //    CMS.resolveRootUrl("customers/betlist/abnormalbetticket"),
        //    function (data) {
        //        var jsonData = $.parseJSON(data);
        //        //var jsonData = {
        //        //    ProblemAccounts: [],
        //        //    Page: 1,
        //        //    Size: 50,
        //        //    TotalItems: 0
        //        //};
        //        gridOptions = gridOptions || initGridOptions();
        //        gridOptions.bodyRows = []; //renderProblemAccounts(jsonData.ProblemAccounts);

        //        elements.dataGrid.fanexGrid(gridOptions).removeClass("display-none");
        //    });
    }

    initialize();
})(jQuery);