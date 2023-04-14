ConcurrentUserIndex = function () {
    var self = this;

    this.init = function () {
        this.bindEvents();
    };

    this.getGridOptions = function () {
        var headerColumns = [
            { HeaderText: "Site Label", Width: 400, Name: "ColSiteLabel", HeaderAlign: "left", CellAlign: "left" },
            { HeaderText: "Total", Width: 150, Name: "ColTotal", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Super", Width: 150, Name: "ColSuper", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Master", Width: 150, Name: "ColMaster", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Agent", Width: 150, Name: "ColAgent", HeaderAlign: "center", CellAlign: "center" }];

        return {
            columns: headerColumns,
            gridExpandHeight: 110,
            gridHeight: 1000
        };
    }

    this.renderBodyRows = function (dataBySiteLabel, dataBySiteName) {
        var index = 0;
        var bodyData = [];

        if (dataBySiteLabel) {
            var row, childRow, subRows;
            $.each(dataBySiteLabel, function (i, item) {
                var detailjs = '<span>' + item.SiteLabel + '</span>'
                detailjs += '<span class="more-detail grid-expand" data-sitelabel="'+ item.SiteLabel+'" data-check="1"></span>';
                row = {
                    Columns: [
                        { Name: "ColSiteLabel", Value: detailjs, Width: 400},
                        { Name: "ColTotal", Value: item.Total, Width: 150},
                        { Name: "ColSuper", Value: item.Super, Width: 150 },
                        { Name: "ColMaster", Value: item.Master, Width: 150 },
                        { Name: "ColAgent", Value: item.Agent, Width: 150 }
                    ]
                };
                bodyData.push(row);
                $.each(dataBySiteName, function (j, child) {
                    if (child.SiteLabel == item.SiteLabel) {
                        var childSiteName = '<div class="child">' + child.SiteName + '</div>';
                        childRow = {
                            Columns: [
                                { Name: "ColSiteLabel", Value: childSiteName, CssClass: "child", Width: 400 },
                                { Name: "ColTotal", Value: child.Total, Width: 150 },
                                { Name: "ColSuper", Value: child.Super, Width: 150 },
                                { Name: "ColMaster", Value: child.Master, Width: 150 },
                                { Name: "ColAgent", Value: child.Agent, Width: 150 }
                            ],
                            CssClass: "child_" + item.SiteLabel
                        };
                        bodyData.push(childRow);
                    }
                });
                
            });
            
        };

        return bodyData;
    }

    this.bindEvents = function () {
        var gridOptions = this.getGridOptions();
        gridOptions.bodyRows = this.renderBodyRows(data.DataBySiteLabel, data.DataBySiteName);
        $("#divLasteFreegrid").removeClass("display-none").fanexGrid(gridOptions);
        $(".more-detail").on("click touched", function () {
            var sitelabel = $(this).data("sitelabel");
            var check = $(this).data("check");
            if (check == "0") {
                $(".child_" + sitelabel).removeClass("display-none");
                $(this).data("check", 1);
                $(this).removeClass("grid-collapse");
                $(this).addClass("grid-expand");
            } else {
                $(".child_" + sitelabel).addClass("display-none");
                $(this).data("check", 0);
                $(this).removeClass("grid-expand");
                $(this).addClass("grid-collapse");
            }
            $("#divLasteFreegrid").fanexGrid("resizeGrid");
        });

        $("#btn-search").on("click touched", function () {
            location.reload();
        })

        $("#noteMessage").on("click touched", function () {
            var title = "Report Definition & Rules";
            var content = $("#divPopupContent").clone();
            content.removeClass("display-none");
            CMS.modal(title, content);
        })
    };
};

$(document).ready(function () {
    var concurrentUserIndex = new ConcurrentUserIndex();

    concurrentUserIndex.init();
});
