function generateData(numberRows, autoHeight) {
    var rows = [];
    for (var index = 0; index < numberRows; index++) {
        var row = {
            Columns: [
                { Name: "No", Value: index },
                { Name: "Username", Value: parseFloat((Math.random() > 0.5 ? (Math.random() * 500).toFixed(3) : (Math.random() * 500 * -1).toFixed(3))) },
                { Name: "Level", Value: "Agent" },
                { Name: "Lock", Value: "<span class='ico-lock lock-action'></span>" },
                { Name: "LeagueGroupLock", Value: "<span class='ico-unlock lock-action'></span>" },
                { Name: "Currency", Value: "UUS" },
                { Name: "LeagueGroup", Value: "Major League" },
                { Name: "LHDP", Value: "500,000,000" },
                { Name: "LOU", Value: "500,000,000" },
                { Name: "L1HDP", Value: "500,000,000" },
                { Name: "L1OU", Value: "500,000,000" },
                { Name: "LOthers", Value: "500,000,000" },
                { Name: "NLHDP", Value: "500,000,000" },
                { Name: "NLOU", Value: "500,000,000" },
                { Name: "NL1HDP", Value: "500,000,000" },
                { Name: "NL1OU", Value: "500,000,000" },
                { Name: "NLOthers", Value: "500,000,000" },
                { Name: "Action", Value: "<span class='ico-edit mid-margin-right' title='Edit' ></span>" },
                { Name: "Delete", Value: "<a class='c hand ico-delete' href='javascript:;' ></a>" }
            ],
            CssClass: 'custom-css-class',
            CustomAttr: '',
            SubRows: ''
        };
        if (autoHeight) {
            row.RowHeight = Math.random() * 100;
        }
        rows.push(row);
    }
    return rows;
}


function generateTreeData(numberParentRows) {
    var rows = [];
    for (var index = 0; index < numberParentRows; index++) {
        var currentRow = {
            Columns: [
                { Name: "No", Value: 'Username - ' + index, MergeColNumber: 19 }
            ],
            SubRows: []
        }
        for (var j = 0; j < 2; j++) {
            if (j == 0) {
                currentRow.SubRows.push(...generateData(1));
            } else {
                var currentSubRow = {
                    Columns: [
                        { Name: "No", Value: "League Group Setting", MergeColNumber: 19 }
                    ],
                    SubRows: generateData(5),
                    CssClass: 'sub-row-class'
                };
                currentRow.SubRows.push(currentSubRow);
            }
            
        }
        if (index == 4) {
            currentRow.SubRows = [];

        }
        rows.push(currentRow);
    }
    return rows;
}

var gridOptions =
{
    columns: [
        { HeaderText: "#", Width: 20, Name: "No", HeaderAlign: "center", CellAlign: "left", ShowCollapseExpand: true },
        { HeaderText: "Username", Width: 70, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Level", Width: 100, Name: "Level", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: "Lock", Width: 100, Name: "Lock", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: "League Group Lock(i)", Width: 160, Name: "LeagueGroupLock", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: "Currency", Width: 110, Name: "Currency" },
        { HeaderText: "Main/League Group", Width: 120, Name: "LeagueGroup" },
        { HeaderText: "Live - HDP", Width: 80, Name: "LHDP" },
        { HeaderText: "Live - OU", Width: 80, Name: "LOU" },
        { HeaderText: "Live - 1H HDP", Width: 80, Name: "L1HDP" },
        { HeaderText: "Live - 1H OU", Width: 80, Name: "L1OU" },
        { HeaderText: "Live - Others", Width: 80, Name: "LOthers" },
        { HeaderText: "NL - HDP", Width: 80, Name: "NLHDP" },
        { HeaderText: "NL - OU", Width: 80, Name: "NLOU" },
        { HeaderText: "NL - 1H HDP", Width: 80, Name: "NL1HDP" },
        { HeaderText: "NL - 1H OU", Width: 80, Name: "NL1OU" },
        { HeaderText: "NL - Others", Width: 80, Name: "NLOthers" },
        { HeaderText: "Action", Width: 80, Name: "Action" },
        { HeaderText: "Delete", Width: 80, Name: "Delete" },
    ],
    bodyRows: generateTreeData(10),
    fullHeight: true,
    gridHeight: 900,
    showTree: true,
    showTitle: true,
    titleInfo:
    {
        text: "Demo Title",
        showCollapseExpandButton: true
    },
    showDistinctRow: false,
    defaultCollapseAllGroups: true
};

$(function () {
    var currentGrid = $("#leagueGroup-list").fanexGrid(gridOptions);
});