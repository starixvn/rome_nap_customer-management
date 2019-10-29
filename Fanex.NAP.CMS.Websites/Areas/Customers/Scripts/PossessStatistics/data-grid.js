Namespace.Register("Fanex.CMS.Customer.PossessStatistics.DataGrid");
Fanex.CMS.Customer.PossessStatistics.DataGrid = function () {
    var me = this;
    var pagerRanges = [10, 20, 50, 100];
    var grid = $("#freegrid");

    var initGroupHeaderRows = function () {
        return [
            {
                Columns: [
                    { HeaderText: "#", Name: "Numero", MergeRowNumber: 2 },
                    { HeaderText: resources.TotalPercent, Name: "TotalSB", MergeColNumber: 2 },
                    { HeaderText: resources.Super, Name: "SuperUsername", MergeColNumber: 3 },
                    { HeaderText: resources.Master, Name: "MasterUsername", MergeColNumber: 3 },
                    { HeaderText: resources.Agent, Name: "AgentUsername", MergeColNumber: 7 },
                ]
            }
        ];
    };

    var initColumns = function () {
        return [
            { HeaderText: "#", Width: 45, Name: "Numero", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Sportsbook, Width: 100, Name: "TotalSB", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.NonSport, Width: 100, Name: "TotalNS", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Username, Width: 200, Name: "SuperUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.Sportsbook, Width: 100, Name: "SuperSB", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.NonSport, Width: 100, Name: "SuperNS", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Username, Width: 200, Name: "MasterUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.Sportsbook, Width: 100, Name: "MasterSB", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.NonSport, Width: 100, Name: "MasterNS", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Username, Width: 200, Name: "AgentUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.Sportsbook, Width: 100, Name: "AgentSB", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.NonSport, Width: 100, Name: "AgentNS", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Status, Width: 100, Name: "AgentStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.ABIDangerLevel, Width: 100, Name: "DangerLevel", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.Monitored, Width: 80, Name: "Monitored", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.ThisPage + '<br/><input type="checkbox" id="chk-select-all">', Width: 70, Name: "ThisPage", HeaderAlign: "center", CellAlign: "center" },
        ];
    }

    var renderData = function (data) {
        var bodyRows = [];
        var length = data ? data.length : 0;

        for (var i = 0; i < length; i++) {
            var item = data[i];
            var row = {
                Columns: [
                    { Name: "Numero", Value: item.No },
                    { Name: "TotalSB", Value: '<span class="bold">' + item.TotalSB + '</span>' },
                    { Name: "TotalNS", Value: '<span class="bold">' + item.TotalNS + '</span>' },
                    { Name: "SuperUsername", Value: item.SuperUsername },
                    { Name: "SuperSB", Value: item.SuperSB },
                    { Name: "SuperNS", Value: item.SuperNS },
                    { Name: "MasterUsername", Value: item.MasterUsername },
                    { Name: "MasterSB", Value: item.MasterSB },
                    { Name: "MasterNS", Value: item.MasterNS },
                    { Name: "AgentUsername", Value: item.AgentUsername },
                    { Name: "AgentSB", Value: item.AgentSB },
                    { Name: "AgentNS", Value: item.AgentNS },
                    { Name: "AgentStatus", Value: item.StatusName },
                    { Name: "DangerLevel", Value: item.DangerLevel },
                    { Name: "Monitored", Value: item.MonitoredText },
                    { Name: "ThisPage", Value: renderSelectItemCheckbox(item.CustId, item.AgentUsername) },
                ]
            };

            bodyRows.push(row);
        }

        return bodyRows;
    };

    var renderSelectItemCheckbox = function (custId, username) {
        return '<input type="checkbox" class="chk-select-item" data-custid="' + custId + '" data-username="' + username + '" />';
    };

    var bindEvents = function () {
        var checkall = $("#chk-select-all"),
            checkitems = $(".chk-select-item");

        checkall.off("change").on("change", function () {
            checkitems.prop("checked", this.checked);
        });

        checkitems.off("change").on("change", function () {
            var numberOfCheckedCheckItems = $(".chk-select-item:checked").length;

            if (numberOfCheckedCheckItems === 0) {
                checkall.prop("indeterminate", false).prop("checked", false);
            } else if (numberOfCheckedCheckItems === checkitems.length) {
                checkall.prop("indeterminate", false).prop("checked", true);
            } else {
                checkall.prop("indeterminate", true);
            }
        });
    };

    me.render = function (data, pagerOptions, exportExcelFunction) {
        var gridOptions = {
            showTitle: true,
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: exportExcelFunction
            },
            customizeHeader: true,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 250,
            groupHeaderRows: initGroupHeaderRows(),
            columns: initColumns(),
            showPager: true,
            customPager: true,
            pagerOption: {
                currentPage: pagerOptions.pageIndex,
                itemsPerPage: pagerOptions.pageSize,
                itemsPerPageArray: pagerRanges,
                totalItem: pagerOptions.totalItems,
                pagerSelectCallBack: pagerOptions.pagerSelectCallBack,
            },
            bodyRows: renderData(data)
        };

        grid.removeClass("display-none").fanexGrid(gridOptions);

        if (pagerOptions.totalItems > 0) {
            bindEvents();
        }
    };

    me.getSelectedCustomers = function () {
        var selectedCustomers = [];

        $(".chk-select-item:checked").each(function () {
            selectedCustomers.push({ Key: $(this).data("custid"), Value: $(this).data("username") });
        });

        return selectedCustomers;
    };
};