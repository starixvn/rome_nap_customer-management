/* File Created: June 11, 2014 */
/*jshint strict: false*/
/* global Namespace, common, labels, numeral, Highcharts */

////'use strict';

$(document).ready(function () {
    var Dashboard = new Fanex.NAP.CMS.Reports.Dashboard();
    Dashboard.Initialize();
});

Namespace.Register("Fanex.NAP.CMS.Reports.Dashboard");
Fanex.NAP.CMS.Reports.Dashboard = function () {
    var $this = this;
    var defaultMonthRange = 3;
    var defaultChartType = 2;
    //    var monthNames = ["January", "February", "March", "April", "May", "June",
    //    "July", "August", "September", "October", "November", "December"];

    //{ variable-declare
    var ChartOptions = null;

    var currentChartType = defaultChartType;
    var currentMonthRange = defaultMonthRange;

    var urls = {
        GetDashboardData: common.ResolveRootUrl('CustomerDashboard/GetDashboardData')
    };

    var domElements = {
        divHorizontalTab: null,

        monthMarginContainer: null,
        monthWinlossContainer: null,
        monthBetCountContainer: null,

        divBestSports: null,
        divBestBettypes: null,
        divEventChoice: null,

        lblTurnOver: null,
        lblWinLoss: null,
        lblMargin: null,
        lblBetCount: null,

        divMonthSelect: null,
        titleMonthCount: null,

        toggleA: null
    };
    //}

    //{ DOM-Caching
    var cacheDomElements = function () {
        domElements.divHorizontalTab = $('#divHorizontalTab');

        domElements.monthMarginContainer = $('#monthMarginContainer');
        domElements.monthWinlossContainer = $('#monthWinlossContainer');
        domElements.monthBetCountContainer = $('#monthBetCountContainer');

        domElements.divBestSports = $('#divBestSports');
        domElements.divBestBettypes = $('#divBestBettypes');
        domElements.divEventChoice = $('#divEventChoice');

        domElements.lblTurnOver = $('#lblTurnOver');
        domElements.lblWinLoss = $('#lblWinLoss');
        domElements.lblMargin = $('#lblMargin');
        domElements.lblMatches = $('#lblBetCount');

        domElements.titleMonthCount = $('#titleMonthCount');

        domElements.divMonthSelect = $("#divTeamMonthPicker");

        domElements.toggleA = $("#toggleA");
    };

    //}

    //{ module-init
    var initialize = function () {
        ChartOptions = new Fanex.NAP.CMS.DashboardChart();
        getSettingFromCookies();
    };

    $this.Initialize = function () {
        setResponsiveHeight();
        initialize();
        cacheDomElements();
        handlePageEvents();
    };

    //}

    //{ page-events & make-form.
    var setResponsiveHeight = function () {
        var maxHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0) - (33 + 15 + 38); // menu + tabs + tabs padding + unknown
        $("#divTabSummaryContent").css({ height: maxHeight });
        $("#divTabTeamContent").css({ height: maxHeight });
    };

    var handlePageEvents = function () {
        $("#headline").hide();
        makeMonthDropdown();
        ajaxGetSummaryData();

        domElements.divMonthSelect.on('change', function () {
            currentMonthRange = parseInt(domElements.divMonthSelect.val());
            renderTitle(currentMonthRange);
            ajaxGetSummaryData();
            saveMonthRangeCookies();
        });

        $('#chk2D').on('change', function () {
            currentChartType = 2;
            domElements.toggleA.removeClass('type3D');
            reRenderChart(currentChartType);
            saveChartTypeCookies();
        });
        $('#chk3D').on('change', function () {
            currentChartType = 3;
            domElements.toggleA.addClass('type3D');
            reRenderChart(currentChartType);
            saveChartTypeCookies();
        });
    };
    //}

    //{ ajax functions
    var ajaxGetSummaryData = function () {
        var monthCriteria = {
            month: parseInt(domElements.divMonthSelect.val())
        };
        common.DisableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSummaryData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(monthCriteria),
            success: function (result) {
                if (result) {
                    renderDashboard(result, currentChartType);
                }
            },
            error: function (xhr) {
                common.HandleAjaxError(xhr);
            },
            complete: function () {
                common.EnableInput();
            }
        });
    };

    //}

    //{ helper functions
    //{ element-render
    var getSettingFromCookies = function () {
        currentChartType = parseInt($.cookie("dashboardChartType")) || defaultChartType;
        currentMonthRange = parseInt($.cookie("dashboardMonthRange")) || defaultMonthRange;

        if (currentChartType === ChartType._2D) {
            $('#chk2D').prop("checked", true);
        }
        else {
            $('#chk3D').prop("checked", true);
            $('#toggleA').addClass('type3D');
        }

        $('#titleMonthCount').text(currentMonthRange);

        // for new-new cookies
        saveChartTypeCookies();
        saveMonthRangeCookies();
    };

    var saveChartTypeCookies = function () {
        $.cookie("dashboardChartType", currentChartType, { expires: 7 });
    };

    var saveMonthRangeCookies = function () {
        $.cookie("dashboardMonthRange", currentMonthRange, { expires: 7 });
    };

    var makeMonthDropdown = function () {
        for (var i = 1; i <= 12; i++) {
            domElements.divMonthSelect.append($('<option></option>').attr('value', i).text(i + ' months'));
        }
        domElements.divMonthSelect.val(currentMonthRange);
    };

    var renderDashboard = function (data, chartType) {
        renderGeneralInfo(data.DashboardGeneralInfo);
        drawSummaryChart(data.DashboardMarginModels, data.DashboardBetCountByMonth, chartType);
        renderRankInfo(data.DashboardTopSports, data.DashboardTopBettypes, data.DashboardTopEventChoice);
    };

    var renderTitle = function (month) {
        domElements.titleMonthCount.text(month);
    };

    var renderGeneralInfo = function (data) {
        domElements.lblWinLoss.text(numeral(data.Winloss).format(common.Formats.DecimalFormat));
        if (data.Margin != null) {
            domElements.lblMargin.text(numeral(data.Margin).format(common.Formats.DecimalFormat) + "%");
        }
        else {
            domElements.lblMargin.text(labels.NA);
        }
        domElements.lblTurnOver.text(numeral(data.Turnover).format(common.Formats.DecimalFormat));
        domElements.lblBetCount.text(numeral(data.BetCount).format(common.Formats.IntegerFormat));
    };

    var createRankRowHtml = function (name, turnover, winloss, margin) {
        var template = '<div class="rank-row">' +
                        '<div class="name">{0}</div>' +
                       ' <div class="value"><div class="number-value">{1}</div></div>' +
                       ' <div class="value"><div class="number-value">{2}</div></div>' +
                       ' <div class="value"><div class="number-value">{3}</div></div>' +
                        '</div>';
        return template.format(name, turnover, winloss, margin);
    };

    var renderRankInfo = function (bestSports, bestBettypes, bestEventChoice) {
        //sports
        domElements.divBestSports.html('');
        if (bestSports) {
            $.each(bestSports, function (index, item) {
                domElements.divBestSports.append(createRankRowHtml(item.SportName
                    , numeral(item.Turnover).format(common.Formats.DecimalFormat)
                    , numeral(item.Winloss).format(common.Formats.DecimalFormat)
                    , numeral(item.Margin).format(common.Formats.DecimalFormat)
                    ));
            });
        }

        //bettypes
        domElements.divBestBettypes.html('');
        if (bestBettypes) {
            $.each(bestBettypes, function (index, item) {
                domElements.divBestTrader.append(createRankRowHtml(item.BetTypeName
                    , numeral(item.Turnover).format(common.Formats.DecimalFormat)
                    , numeral(item.Winloss).format(common.Formats.DecimalFormat)
                    , numeral(item.Margin).format(common.Formats.DecimalFormat)
                    ));
            });
        }

        //eventchoice
        domElements.divEventChoice.html('');
        if (bestEventChoice) {
            $.each(bestEventChoice, function (index, item) {
                domElements.divEventChoice.append(createRankRowHtml(item.LiveStatus
                    , numeral(item.Turnover).format(common.Formats.DecimalFormat)
                    , numeral(item.Winloss).format(common.Formats.DecimalFormat)
                    , numeral(item.Margin).format(common.Formats.DecimalFormat)
                    ));
            });
        }
    };

    var prepareSummaryColumnData = function (marginData, betcountData, chartData) {
        chartData.MonthMarginData = [{
            name: 'Margin %',
            data: []
        }];

        chartData.MonthMatchData = [{
            name: 'Bet Count',
            data: []
        }];
        
        chartData.TeamMixData = [{
            type: 'column',
            yAxis: 0,
            name: 'Turnover',
            zIndex: 1,
            data: []
        }, {
            type: 'spline',
            name: 'Win loss',
            yAxis: 1,
            marker: {
                lineWidth: 2,
                lineColor: Highcharts.getOptions().colors[3],
                //lineColor: '#7cb5ec'
                fillColor: 'white',
                width: 7,
                height: 7
            },
            zIndex: 2,
            data: []
        }];

        $.each(marginData, function (index, item) {
            chartData.MonthMarginData[0].data.push(item.Margin);
            chartData.MonthCategories.push(item.Month + "/" + item.Year);

            //month match data
            var pipeItem = {
                //name: Highcharts.getOptions().lang.shortMonths[item.Month - 1],
                name: item.Month + "/" + item.Year,
                y: item.Margin
            };
            chartData.MonthMatchData[0].data.push(pipeItem);
        });

        $.each(teamData, function (index, item) {
            chartData.Categories.push(item.TeamName);
            chartData.TeamMarginData[0].data.push(item.Margin);
            chartData.TeamMixData[0].data.push(item.Turnover);
            chartData.TeamMixData[1].data.push(item.Winlost);
            if (item.Turnover < chartData.MixDataMin) {
                chartData.MixDataMin = item.Turnover;
            }
            if (item.Winlost < chartData.MixDataMin) {
                chartData.MixDataMin = item.Winlost;
            }
        });
    };

    var reRenderChart = function (type) {
        drawSummaryChart(null, type);
    };

    var chartData = {};

    var ChartType = {
        _2D: 2,
        _3D: 3
    };

    var drawSummaryChart = function (data, type) {
        domElements.monthMarginContainer.html('');
        domElements.monthWinlossContainer.html('');
        domElements.monthBetCountContainer.html('');

        var summaryColumnOption = ChartOptions.ColumnChartOption();

        //make data
        if (data) {
            chartData = {
                MarginData: [],
                WinlossData: [],
                BetCountData: [],
                Categories: [],
                MonthCategories: [],
                MixDataMin: 0
            };

            prepareSummaryColumnData(data.MonthData, data.TeamData, chartData);
        }

        // month margin chart
        summaryColumnOption.title.text = 'Margin (%) By Month';
        summaryColumnOption.series = chartData.MarginData;
        summaryColumnOption.xAxis.categories = chartData.MonthCategories;
        if (type === ChartType._2D) {
            summaryColumnOption.chart.options3d.enabled = false;
        }
        domElements.monthMarginContainer.highcharts(summaryColumnOption);

        // month turnover&winloss chart
        var mixChartOption = ChartOptions.MixChartOption();
        mixChartOption.xAxis.categories = chartData.Categories;
        mixChartOption.title.text = 'WinLoss/Turnover By Team';
        mixChartOption.series = chartData.WinlossData;
        if (type === ChartType._2D) {
            mixChartOption.chart.options3d.enabled = false;
        }
        if (chartData.MixDataMin < 0) {
            mixChartOption.yAxis[0].min = chartData.MixDataMin;
            mixChartOption.yAxis[1].min = chartData.MixDataMin;
        }
        domElements.monthBetCountContainer.highcharts(mixChartOption);

        // month betcount chart
        var pipeChartOption = ChartOptions.PipeChartOption();
        pipeChartOption.title.text = 'Bet Count By Month';
        pipeChartOption.series = chartData.BetCountData;
        if (type === ChartType._2D) {
            pipeChartOption.chart.options3d.enabled = false;
        }
        domElements.monthMatchContainer.highcharts(pipeChartOption);
    };
    //}
};