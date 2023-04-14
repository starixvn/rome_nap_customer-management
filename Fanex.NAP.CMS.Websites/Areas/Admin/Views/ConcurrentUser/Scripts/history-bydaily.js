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
                maxDate: resources.maxDate,
                minDate: resources.minDate
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
            onSelectDatePicker: function () {
                var minDate = new Date($("#dpk-todate").val());
                minDate.setDate(minDate.getDate() - resources.maxDaySupporting)
                minDate = minDate > resources.minDate ? minDate : resources.minDate;
                $("#dpk-fromdate").datepicker("option", "minDate", minDate);
            }
        });

        var minDate = new Date($("#dpk-todate").val());
        minDate.setDate(minDate.getDate() - resources.maxDaySupporting);
        minDate = minDate > resources.minDate ? minDate : resources.minDate;
        $("#dpk-fromdate").datepicker("setDate", resources.defaultFromDate);
        $("#dpk-fromdate").datepicker("option", "minDate", minDate);
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

    function SummaryGrid(viewModel) {
        var maxCounter = 0;
        var minCounter = Number.MAX_VALUE;

        function getGridOptions() {
            var columns = [
                { HeaderText: "GMT+8", Width: 130, Name: "colTitle", HeaderAlign: "center", CellAlign: "center" }
            ]
            var width = viewModel.Summary.length == 1 ? 60 : 45;
            for (var i = 0; i < viewModel.Summary.length; i++) {
                var column = {
                    HeaderText: viewModel.Summary[i].DayOfWeek,
                    Width: width,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                columns.push(column);
            }

            var groupColumns = [
                { HeaderText: "GMT+8", Width: 130, Name: "colTitle", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 }
            ]
            for (var i = 0; i < viewModel.Summary.length; i++) {
                var column = {
                    HeaderText: viewModel.Summary[i].Day,
                    Width: width,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                groupColumns.push(column);
            }
            return {
                columns: columns,
                customizeHeader: true,
                groupHeaderRows: [{ Columns: groupColumns }],                
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

        function getBodyRows(viewModel) {
            var columns = [];
            if (viewModel.Summary.length > 0) {
                columns.push({
                    Name: "colTitle",
                    Value: "Max Concurrent Users",
                    Width: 130
                });
                var width = viewModel.Summary.length == 1 ? 60 : 45;
                for (var i = 0; i < viewModel.Summary.length; i++) {
                    var item = viewModel.Summary[i];
                    if (item.UserCount != "-" & item.UserCount > maxCounter) {
                        maxCounter = item.UserCount;
                    }
                    if (item.UserCount != "-" & item.UserCount < minCounter) {
                        minCounter = item.UserCount;
                    }
                    var column = {
                        Name: "col" + i,
                        Value: item.UserCount,
                        Width: width
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

        function renderGrid(viewModel) {
            var gridOptions = getGridOptions();
            gridOptions.bodyRows = getBodyRows(viewModel);
            $("#summaryFreegrid").removeClass("display-none").fanexGrid(gridOptions);
        }

        return renderGrid(viewModel);
    }

    function DetailGrid(viewModel) {
        function renderColor(bodyRows) {
            for (var i = 0; i < bodyRows.length; i++) {
                var minCounter = Number.MAX_VALUE;
                var maxCounter = 0;
                for (var c = 0; c < viewModel.Summary.length; c++) {
                    if (bodyRows[i].Columns[c+1].Value != "-" && bodyRows[i].Columns[c+1].Value > maxCounter) {
                        maxCounter = bodyRows[i].Columns[c+1].Value;
                    }

                    if (bodyRows[i].Columns[c + 1].Value != "-" && bodyRows[i].Columns[c + 1].Value < minCounter) {
                        minCounter = bodyRows[i].Columns[c + 1].Value;
                    }
                }

                if (maxCounter != minCounter) {
                    for (var c = 0; c < viewModel.Summary.length; c++) {
                        if (bodyRows[i].Columns[c + 1].Value != "-" && bodyRows[i].Columns[c + 1].Value == maxCounter) {
                            bodyRows[i].Columns[c + 1].TdClass = "maxColor";
                        }

                        if (bodyRows[i].Columns[c + 1].Value != "-" && bodyRows[i].Columns[c + 1].Value == minCounter) {
                            bodyRows[i].Columns[c + 1].TdClass = "minColor";
                        }
                    }
                }
            }
        }

        function findUserCount(list, day) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Day == day)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function findUserCountBySiteLabel(list, siteLabel, day) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].SiteLabel == siteLabel && list[i].Day == day)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function findUserCountBySiteName(list, siteLabel, siteName, day) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].SiteLabel == siteLabel &&
                    list[i].SiteName == siteName &&
                    list[i].Day == day)
                    return list[i];
            }

            return { UserCount: "-", Color: "" };
        }

        function getBodyRows(viewModel) {
            var rows = [];
            for (var sl = 0; sl < viewModel.DataBySiteLabel.length; sl++) {
                var detailjs = '<span>' + viewModel.DataBySiteLabel[sl][0].SiteLabel + '</span>'
                detailjs += '<span class="js-more-detail grid-collapse" data-sitelabel="' + viewModel.DataBySiteLabel[sl][0].SiteLabel + '" data-check="0"></span>';
                var columns = [
                    {
                        Name: "colTitle",
                        Value: detailjs
                    }
                ];
                for (var h = 0; h < viewModel.Summary.length; h++) {
                    var item = findUserCount(viewModel.DataBySiteLabel[sl], viewModel.Summary[h].Day);
                    var column = {
                        Name: "col" + h,
                        Value: item.UserCount,
                        CssClass: item.Color
                    }
                    columns.push(column);
                }
                rows.push({ Columns: columns });

                //Site name
                for (var sn = 0; sn < viewModel.DataBySiteName.length; sn++) {

                    if (viewModel.DataBySiteName[sn][0].SiteLabel == viewModel.DataBySiteLabel[sl][0].SiteLabel) {
                        detailjs = '<span>' + viewModel.DataBySiteName[sn][0].SiteName + '</span>'
                        detailjs += '<span class="js-more-detail-site-name grid-collapse" data-sitelabel="' + viewModel.DataBySiteName[sn][0].SiteLabel + '" data-sitename="' + viewModel.DataBySiteName[sn][0].SiteName + '" data-check="0"></span>';
                        columns = [
                            {
                                Name: "colTitle",
                                Value: detailjs,
                                Width: 130,
                                CssClass: "child"
                            }
                        ];
                        for (var h = 0; h < viewModel.Summary.length; h++) {
                            var item = findUserCountBySiteLabel(viewModel.DataBySiteName[sn], viewModel.DataBySiteName[sn][0].SiteLabel, viewModel.Summary[h].Day);
                            var column = {
                                Name: "col" + h,
                                Value: item.UserCount
                            }
                            columns.push(column);
                        }
                        rows.push(
                            {
                                Columns: columns,
                                CssClass: "display-none " + "row_child_" + viewModel.DataBySiteName[sn][0].SiteLabel
                            });

                        //Role name
                        for (var r = 0; r < viewModel.DataByRoleName.length; r++) {
                            for (var j = 0; j < viewModel.DataByRoleName[r].length; j++) {
                                if (viewModel.DataByRoleName[r][j].SiteLabel == viewModel.DataBySiteLabel[sl][0].SiteLabel &&
                                    viewModel.DataByRoleName[r][j].SiteName == viewModel.DataBySiteName[sn][0].SiteName) {
                                    columns = [
                                        {
                                            Name: "colTitle",
                                            Value: viewModel.DataByRoleName[r][j].RoleName,
                                            CssClass: "roleNameChild"
                                        }
                                    ];
                                    for (var h = 0; h < viewModel.Summary.length; h++) {
                                        var item = findUserCountBySiteName(viewModel.DataByRoleName[r], viewModel.DataByRoleName[r][j].SiteLabel, viewModel.DataByRoleName[r][j].SiteName, viewModel.Summary[h].Day);
                                        var column = {
                                            Name: "col" + h,
                                            Value: item.UserCount
                                        }
                                        columns.push(column);
                                    }
                                    rows.push(
                                        {
                                            Columns: columns,
                                            CssClass: "display-none " + "row_child_" + viewModel.DataByRoleName[r][j].SiteLabel + "_" + viewModel.DataByRoleName[r][j].SiteName
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
            var width = viewModel.Summary.length == 1 ? 60 : 45;
            for (var i = 0; i < viewModel.Summary.length; i++) {
                var column = {
                    HeaderText: viewModel.Summary[i].DayOfWeek,
                    Width: width,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                columns.push(column);
            }

            var groupColumns = [
                { HeaderText: "", Width: 130, Name: "colTitle", HeaderAlign: "center", CellAlign: "left", MergeRowNumber: 2 }
            ]
            for (var i = 0; i < viewModel.Summary.length; i++) {
                var column = {
                    HeaderText: viewModel.Summary[i].Day,
                    Width: width,
                    Name: "col" + i,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };

                groupColumns.push(column);
            }
            return {
                columns: columns,
                customizeHeader: true,
                groupHeaderRows: [{ Columns: groupColumns }],
                fullHeight: false,
                gridHeight: 550,
                showTitle: true,
                titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: 'MAX CONCURRENT USERS DETAILS'
                }
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
                    $('tr[class*="row_child_' + sitelabel + '"]').addClass("display-none");
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

        function RenderGrid(viewModel) {
            var gridDetailOptions = getOptions();
            gridDetailOptions.bodyRows = getBodyRows(viewModel);
            $("#detailFreegrid").removeClass("display-none").fanexGrid(gridDetailOptions);            
            bindEvents();
        }

        return RenderGrid(viewModel);
    }

    function getDataHistories() {
        var dateRange = fromToDatePicker.nexDatePicker("getDateRange");

        var url = formSearch.attr("action")
            + "?fromdate=" + dateRange.FromDate
            + "&todate=" + dateRange.ToDate;

        CMS.getJson(url, function (data) {
            var viewModel = $.parseJSON(data);
            if (viewModel.Summary.length == 0) {
                $("#div-result-message").removeClass("display-none");
                $("#summaryFreegrid").addClass("display-none");
                $("#detailFreegrid").addClass("display-none");
                $("#summaryFreegrid-grid-title").addClass("display-none");
                $("#detailFreegrid-grid-title").addClass("display-none");
            } else {
                $("#div-result-message").addClass("display-none");
                SummaryGrid(viewModel);
                DetailGrid(viewModel)
            }
            
        });
    }

    $(function () {
        bindEvents();
        initDateTimeFields();
    });
})(jQuery);