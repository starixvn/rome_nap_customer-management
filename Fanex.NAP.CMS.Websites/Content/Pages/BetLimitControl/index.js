$(document).ready(function () {
    var betLimitControl = new Fanex.CMS.BetLimitControl();
    betLimitControl.InitPage();
});

Namespace.Register("Fanex.CMS.BetLimitControl");
Fanex.CMS.BetLimitControl = function () {
    var self = this;

    this.elements = {
        divBetLimitControl: null,
        reportContainer: null,
    };

    this.cachedElements = function () {
        this.elements.divBetLimitControl = $("#div-bet-limit-control");
        this.elements.reportContainer = $("#report-container");
    };

    this.InitPage = function () {
        this.cachedElements();
        this.renderBetLimit();
    }

    this.renderBetLimit = function (rs) {
        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        { HeaderText: 'Username', Name: 'Username', MergeRowNumber: 3 },
                        { HeaderText: 'VIP', Name: 'VIP', MergeRowNumber: 3 },
                        { HeaderText: 'Level', Name: 'Level', MergeRowNumber: 3 },
                        { HeaderText: 'Currency', Name: 'Currency', MergeRowNumber: 3 },
                        { HeaderText: 'Site', Name: 'Site', MergeRowNumber: 3 },
                        { HeaderText: 'Bet Limit Type', Name: 'BetLimitType', MergeRowNumber: 3 },
                        { HeaderText: 'Selected Account', Name: 'MaxPerMatch', MergeRowNumber: 2, MergeColNumber: 2 },
                        { HeaderText: 'Upline', Name: 'AgentMaxPerMatch', MergeColNumber: 6 },
                        { HeaderText: 'Action', Name: 'Action', MergeRowNumber: 3 }
                    ]
                },
                {
                    Columns: [
                        { HeaderText: 'Agent', Name: 'AgentMaxPerMatch', MergeColNumber: 2 },
                        { HeaderText: 'Master', Name: 'MasterMaxPerMatch', MergeColNumber: 2 },
                        { HeaderText: 'Super', Name: 'SuperMaxPerMatch', MergeColNumber: 2 }
                    ]
                },
            ],
            columns: [
                { HeaderText: "Username", Width: 50, Name: 'Username', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'VIP', Width: 80, Name: 'VIP', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Level', Width: 200, Name: 'Level', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true},
                { HeaderText: 'Currency', Width: 80, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Site', Width: 80, Name: 'Site', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true},
                { HeaderText: 'Bet Limit Type', Width: 80, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'MaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 80, Name: 'MaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'AgentMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 80, Name: 'AgentMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'MasterMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 80, Name: 'MasterMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'SuperMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 80, Name: 'SuperMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Action', Width: 100, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true }
            ],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            showTitle: false,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: self.changePager
            },
            gridExpandHeight: 200,
            titleInfo: {
                text: "",
                showExcelButton: false
            },
        };

        var bodyData = [];

        gridOptions.bodyRows = bodyData;
        gridOptions.pagerOption.totalItem = 0;

        this.elements.divBetLimitControl.fanexGrid(gridOptions);

        this.elements.divBetLimitControl.find(".ico-delete").on("click touchend", function () {
        });

        this.elements.reportContainer.removeClass("display-none");
    };
}


