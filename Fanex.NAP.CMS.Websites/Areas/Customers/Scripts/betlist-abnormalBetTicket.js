(function ($, CMS, resources, abnormalBetTickets) {
    "use strict";
    var dataGrid = $("#freegrid");
    var gridOptions = null;

    function initGridOptions() {
        var columns = [
            { HeaderText: resources.voidDate, Width: 160, Name: "colVoidDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.betId, Width: 160, Name: "colBetId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.abnormalReason, Width: 430, Name: "colAbnormalReason", HeaderAlign: "center", CellAlign: "left" }
        ];

        return {
            showTitle: true,
            titleInfo: {
                text: resources.abnormalBetTicketTitle,
                showBackButton: true,
                backFunction: function () {
                    window.location.href = CMS.resolveRootUrl(getProblemAccountURL());
                },
                hideWhenNoData: false
            },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 240,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: filter.pageSize,
                itemsPerPageArray: [10, 20, 50, 100],
                totalItem: totalAbnormalBetTickets,
                pagerSelectCallBack: getAbnormalBetTickets
            }
        };
    }

    function getProblemAccountURL() {
        if (filter.siteType === 0) {
            return "Customers/ProblemAccount/Index";
        }

        if (filter.siteType === 1) {
            return "Customers/ProblemAccountCreditSites/Index";
        }

        if (filter.siteType === 2) {
            return "Customers/ProblemAccountDepositSites/Index";
        }
    }

    function getAbnormalBetTickets(pageIndex, pageSize) {
        // PageIndex of Fanex.Grid begin at 0 whereas server site begin at 1.
        var queryString = $.param({
            custId: filter.custId,
            fromDate: filter.fromDate,
            toDate: filter.toDate,
            pageIndex: pageIndex + 1,
            pageSize: pageSize,
            siteType: filter.siteType
        });

        CMS.getJson(
            CMS.resolveRootUrl("Customers/BetList/GetAbnormalBetTicketPaging?" + queryString),
            function (data) {
                var jsonData = $.parseJSON(data);

                gridOptions.bodyRows = renderAbnormalBetTickets(jsonData.Item2);
                gridOptions.pagerOption.currentPage = pageIndex;
                gridOptions.pagerOption.itemsPerPage = pageSize;
                gridOptions.pagerOption.totalItem = jsonData.Item1;

                dataGrid.fanexGrid(gridOptions);
            });
    }

    function renderAbnormalBetTickets(data) {
        var bodyRows = [];
        var cell, ticket;
        var dataLength = data ? data.length : 0;
        for (var i = 0; i < dataLength; i++) {
            ticket = data[i];
            cell = [
                { Name: "colVoidDate", Value: ticket.VoidDateString },
                { Name: "colBetId", Value: ticket.BetId },
                { Name: "colAbnormalReason", Value: ticket.Reason }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function initialize() {
        gridOptions = initGridOptions();
        gridOptions.bodyRows = renderAbnormalBetTickets(abnormalBetTickets);
        dataGrid.removeClass("display-none").fanexGrid(gridOptions);
    }

    initialize();
})(jQuery, CMS, resources, abnormalBetTickets);