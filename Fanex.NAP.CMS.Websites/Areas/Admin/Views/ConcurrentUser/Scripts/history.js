"use strict";
(function ($) {
    var fromToDatePicker = $("#fromtodate-picker");
    var formSearch = $("#formSearch");
    var detailFreegrid = $("#detailFreegrid");

    function initDateTimeFields() {
        fromToDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            blankable: true,
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                maxDate: "",
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: "From",
                LabelCss: "lbl-field lblFilter col_2",
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "To",
                LabelCss: "lbl-field lblFilter col_2",
                TextboxCss: "edit-field col_6"
            },
        });

        $("#dpk-fromdate").datepicker("setDate", resources.defaultFromDate);
        $("#dpk-todate").datepicker("setDate", resources.defaultToDate);
    }

    function bindEvents() {

        formSearch.submit(function (e) {
            e.preventDefault();
            getDataHistories();
        })

        $("#noteMessage").on("click touched", function () {
            var title = "Report Definition & Rules";
            var content = $("#divPopupContent").clone();
            content.removeClass("display-none");
            CMS.modal(title, content);
        })
    }

    function SummaryGrid(summary) {
        var maxCounter = 0;
        var minCounter = findUserCount(summary, 0).UserCount;

        function getGridOptions() {
            var columns = [
                { HeaderText: "GMT+8", Width: 130, Name: "colTitle", HeaderAlign: "center", CellAlign: "center" }
            ]
            for (var i = 0; i < 24; i++) {
                var column = {
                    HeaderText: i + "H" + "-" + (i + 1) + "H",
                    Width: 60,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                columns.push(column);
            }

            return {
                columns: columns,
                fullHeight: true,
                gridHeight: 600,
                showTitle: true,
                titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: 'MAX CONCURRENT USERS'
                }
            }
        }

        function getBodyRows(summary) {
            var columns = [];
            if (summary.length > 0) {
                columns.push({
                    Name: "colTitle",
                    Value: "Max Concurrent Users",
                    Width: 130
                });

                for (var i = 0; i < 24; i++) {
                    var item = findUserCount(summary, i);
                    if (item.UserCount != "-" & item.UserCount > maxCounter) {
                        maxCounter = item.UserCount;
                    }
                    if (item.UserCount != "-" & item.UserCount < minCounter) {
                        minCounter = item.UserCount;
                    }
                    var column = {
                        Name: "col" + i,
                        Value: item.UserCount,
                        Width: 60

                    }

                    columns.push(column);
                }

                if (minCounter != maxCounter) {
                    for (var i = 0; i < columns.length; i++) {
                        if (columns[i].Value != "-" && columns[i].Value == maxCounter) {
                            columns[i].TdClass = "maxColor";
                        }
                        if (columns[i].Value != "-" && columns[i].Value == minCounter) {
                            columns[i].TdClass = "minColor";
                        }
                    }
                }

                return [{ Columns: columns }]
            }

            return columns;
        }

        function findUserCount(list, hour) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Hour == hour)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function renderGrid(summary) {
            var gridOptions = getGridOptions();
            gridOptions.bodyRows = getBodyRows(summary);
            $("#summaryFreegrid").removeClass("display-none").fanexGrid(gridOptions);
        }

        return renderGrid(summary);
    }

    function DetailGrid(jsonData) {
        function renderColor(bodyRows) {
            for (var i = 0; i < bodyRows.length; i++) {
                var minCounter = bodyRows[i].Columns[1].Value;
                var maxCounter = 0;
                for (var c = 1; c < 24; c++) {
                    if (bodyRows[i].Columns[c].Value != "-" && bodyRows[i].Columns[c].Value > maxCounter) {
                        maxCounter = bodyRows[i].Columns[c].Value;
                    }

                    if (bodyRows[i].Columns[c].Value != "-" && bodyRows[i].Columns[c].Value < minCounter) {
                        minCounter = bodyRows[i].Columns[c].Value;
                    }
                }

                if (maxCounter != minCounter) {
                    for (var c = 1; c < 24; c++) {
                        if (bodyRows[i].Columns[c].Value != "-" && bodyRows[i].Columns[c].Value == maxCounter) {
                            bodyRows[i].Columns[c].TdClass = "maxColor";
                        }

                        if (bodyRows[i].Columns[c].Value != "-" && bodyRows[i].Columns[c].Value == minCounter) {
                            bodyRows[i].Columns[c].TdClass = "minColor";
                        }
                    }
                }
            }
        }

        function findUserCount(list, hour) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Hour == hour)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function findUserCountBySiteLabel(list, siteLabel, hour) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].SiteLabel == siteLabel && list[i].Hour == hour)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function findUserCountBySiteName(list, siteLabel, siteName, hour) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].SiteLabel == siteLabel &&
                    list[i].SiteName == siteName &&
                    list[i].Hour == hour)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function getBodyRows(dataBySiteLabel, dataBySiteName, dataByRoleName) {
            var rows = [];
            for (var sl = 0; sl < dataBySiteLabel.length; sl++) {
                var detailjs = '<span>' + dataBySiteLabel[sl][0].SiteLabel + '</span>'
                detailjs += '<span class="js-more-detail grid-collapse" data-sitelabel="' + dataBySiteLabel[sl][0].SiteLabel + '" data-check="0"></span>';
                var columns = [
                    {
                        Name: "colTitle",
                        Value: detailjs,
                        Width: 130
                    }
                ];
                for (var h = 0; h < 24; h++) {
                    var item = findUserCount(dataBySiteLabel[sl], h);
                    var column = {
                        Name: "col" + h,
                        Value: item.UserCount,
                        CssClass: item.Color,
                        Width: 60
                    }
                    columns.push(column);
                }
                rows.push({ Columns: columns });

                //Site name
                for (var sn = 0; sn < dataBySiteName.length; sn++) {

                    if (dataBySiteName[sn][0].SiteLabel == dataBySiteLabel[sl][0].SiteLabel) {
                        detailjs = '<span>' + dataBySiteName[sn][0].SiteName + '</span>'
                        detailjs += '<span class="js-more-detail-site-name grid-collapse" data-sitelabel="' + dataBySiteName[sn][0].SiteLabel + '" data-sitename="' + dataBySiteName[sn][0].SiteName + '" data-check="0"></span>';
                        columns = [
                            {
                                Name: "colTitle",
                                Value: detailjs,
                                Width: 130,
                                CssClass: "child"
                            }
                        ];
                        for (var h = 0; h < 24; h++) {
                            var item = findUserCountBySiteLabel(dataBySiteName[sn], dataBySiteName[sn][0].SiteLabel, h);
                            var column = {
                                Name: "col" + h,
                                Value: item.UserCount,
                                Width: 60
                            }
                            columns.push(column);
                        }
                        rows.push(
                            {
                                Columns: columns,
                                CssClass: "display-none " + "row_child_" + dataBySiteName[sn][0].SiteLabel
                            });

                        //Role name
                        for (var r = 0; r < dataByRoleName.length; r++) {
                            for (var j = 0; j < dataByRoleName[r].length; j++) {
                                if (dataByRoleName[r][j].SiteLabel == dataBySiteLabel[sl][0].SiteLabel &&
                                    dataByRoleName[r][j].SiteName == dataBySiteName[sn][0].SiteName) {
                                    columns = [
                                        {
                                            Name: "colTitle",
                                            Value: dataByRoleName[r][j].RoleName,
                                            Width: 130,
                                            CssClass: "roleNameChild"
                                        }
                                    ];
                                    for (var h = 0; h < 24; h++) {
                                        var item = findUserCountBySiteName(dataByRoleName[r], dataByRoleName[r][j].SiteLabel, dataByRoleName[r][j].SiteName, h);
                                        var column = {
                                            Name: "col" + h,
                                            Value: item.UserCount,
                                            Width: 60
                                        }
                                        columns.push(column);
                                    }
                                    rows.push(
                                        {
                                            Columns: columns,
                                            CssClass: "display-none " + "row_child_" + dataByRoleName[r][j].SiteLabel + "_" + dataByRoleName[r][j].SiteName
                                        });
                                    break;
                                }
                            }
                        }
                    }
                }

            }

            renderColor(rows);

            return rows;
        }

        function getOptions() {
            var columns = [
                { HeaderText: "", Width: 130, Name: "colTitle", HeaderAlign: "center", CellAlign: "left" }
            ]
            for (var i = 0; i < 24; i++) {
                var column = {
                    HeaderText: i + "H" + "-" + (i + 1) + "H",
                    Width: 60,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                columns.push(column);
            }

            return {
                columns: columns,
                fullHeight: false,
                gridHeight: 600,
                showTitle: true,
                titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: 'MAX CONCURRENT USERS DETAILS'
                },
            }
        }

        function bindEvents() {
            $(".js-more-detail").on("click touched", function () {
                var sitelabel = $(this).data("sitelabel");
                var check = $(this).data("check");
                if (check == "0") {
                    $(".row_child_" + sitelabel).removeClass("display-none");
                    $(this).data("check", 1);
                    $(this).removeClass("grid-collapse");
                    $(this).addClass("grid-expand");
                } else {
                    // hide row site name
                    $(".row_child_" + sitelabel).addClass("display-none");
                    $(".row_child_" + sitelabel).find(".js-more-detail-site-name").data("check", 0);
                    $(".row_child_" + sitelabel).find(".js-more-detail-site-name").removeClass("grid-expand");
                    $(".row_child_" + sitelabel).find(".js-more-detail-site-name").addClass("grid-collapse");
                    $(this).data("check", 0);
                    $(this).removeClass("grid-expand");
                    $(this).addClass("grid-collapse");

                    // hide row role name
                    $('tr[class*="row_child_' + sitelabel +'"]').addClass("display-none");
                }
                detailFreegrid.fanexGrid("resizeGrid");
            });

            $(".js-more-detail-site-name").on("click touched", function () {
                var sitelabel = $(this).data("sitelabel");
                var sitename = $(this).data("sitename");
                var check = $(this).data("check");
                if (check == "0") {
                    $(".row_child_" + sitelabel + "_" + sitename).removeClass("display-none");
                    $(this).data("check", 1);
                    $(this).removeClass("grid-collapse");
                    $(this).addClass("grid-expand");
                } else {
                    $(".row_child_" + sitelabel + "_" + sitename).addClass("display-none");
                    $(this).data("check", 0);
                    $(this).removeClass("grid-expand");
                    $(this).addClass("grid-collapse");

                }
                detailFreegrid.fanexGrid("resizeGrid");
            });
        }

        function RenderGrid(jsonData) {
            var gridDetailOptions = getOptions();
            gridDetailOptions.bodyRows = getBodyRows(jsonData.DataBySiteLabel, jsonData.DataBySiteName, jsonData.DataByRoleName);
            $("#detailFreegrid").removeClass("display-none").fanexGrid(gridDetailOptions);

            bindEvents();
        }

        return RenderGrid(jsonData);
    }

    function getDataHistories() {
        var dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        var url = formSearch.attr("action")
            + "?fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate;

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);
            SummaryGrid(jsonData.Summary);
            DetailGrid(jsonData)
        });
    }

    $(function () {
        bindEvents();
        initDateTimeFields();
    });
})(jQuery);