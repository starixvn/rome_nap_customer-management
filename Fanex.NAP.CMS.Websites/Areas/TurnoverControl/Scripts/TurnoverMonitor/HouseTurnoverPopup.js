"use strict";
function HouseTurnoverPopup(parentTable) {
    var domElements = {
        houseTurnoverDiv: $("#divHouseTurnover")
    };
    var gridOptions = null;

    function getGridOptions() {
        var columns = [
            { HeaderText: viewResources.Bet_Type, Width: 160, Name: "ColBetType", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Live, Width: 200, Name: "ColLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: viewResources.NonLive, Width: 200, Name: "ColNonLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: viewResources.Total, Width: 200, Name: "ColTotal", HeaderAlign: "center", CellAlign: "center" }
        ];

        return {
            columns: columns,
            showTitle: true,
            titleInfo: {
                text: viewResources.Balance_Info
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: false,
            fullHeight: false,
            gridWidth: 900,
            gridHeight: 400,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25
        };
    }

    function renderHouseTurnover(data) {
        var bodyData = [];
        var turnover;

        for (var i = 0, max = data.length; i < max; i++) {
            turnover = data[i];

            bodyData.push({
                Columns: [
                    { Name: "ColBetType", Value: turnover.BetTypeName },
                    { Name: "ColLive", Value: turnover.Live },
                    { Name: "ColNonLive", Value: turnover.NonLive },
                    { Name: "ColTotal", Value: turnover.Total, CssClass: "bold" }
                ]
            });
        }

        return bodyData;
    }

    function getHouseTurnover(custId, matchId, isGroupHeadline, eventDate) {
        var table = $("#house-turnover-list").addClass("display-none");
        var queryString = $.param({
            CustId: custId,
            MatchId: matchId,
            IsGroupHeadline: isGroupHeadline,
            EventDate: eventDate
        });

        CMS.getJson(
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/GetHouseTurnovers?") + queryString,
            function (data) {
                var result = $.parseJSON(data);

                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = renderHouseTurnover(result);

                table.removeClass("display-none").fanexGrid(gridOptions);
            });
    }

    this.initialize = function () {
        parentTable.on("click", ".col-company-turnover-body .hand", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var matchId = $this.data("matchid");
            var isGroupHeadline = $this.data("isgroupheadline");
            var eventDate = $this.data("eventdate");

            CMS.modal(
                viewResources.TurnoverMonitor_CompanyTurnover_Title,
                domElements.houseTurnoverDiv.clone(),
                { resizable: false });

            getHouseTurnover(custId, matchId, isGroupHeadline, eventDate);
        });
    };
}