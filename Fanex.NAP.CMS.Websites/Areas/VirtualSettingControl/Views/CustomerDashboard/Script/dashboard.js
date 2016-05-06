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

    //{ variable-declare
    var ChartOptions = null;

    var currentChartType = defaultChartType;
    var currentMonthRange = parseInt($("#lblMonthToCheck").text());// defaultMonthRange;

    var urls = {
        GetDashboardData: 'GetDashboardData',
        GetTeamData: '/GetTeamData'
    };

    var domElements = {
        divHorizontalTab: null,

        monthMarginContainer: null,
        monthBetCountContainer: null,
        monthWinlossContainer: null,
        monthMarginByStakeContainer: null,

        divBestSports: null,
        divBestBettypes: null,
        divEventChoices: null,

        lblTurnOver: null,
        lblWinLoss: null,
        lblMargin: null,
        lblBetCount: null,

        divMonthSelect: null,
        titleMonthCount: null,

        toggleA: null,

        custId: null,
        monthToCheck: null,
        sportId: null
    };
    //}

    //{ DOM-Caching
    var cacheDomElements = function () {
        domElements.divHorizontalTab = $('#divHorizontalTab');

        domElements.monthMarginContainer = $('#monthMarginContainer');
        domElements.monthBetCountContainer = $('#monthBetCountContainer');
        domElements.monthWinlossContainer = $('#monthWinlossContainer');
        domElements.monthMarginByStakeContainer = $('monthMarginByStakeContainer');

        domElements.divBestSports = $('#divBestSports');
        domElements.divBestBettypes = $('#divBestBettypes');
        domElements.divEventChoices = $('#divEventChoices');

        domElements.lblTurnOver = $('#lblTurnOver');
        domElements.lblWinLoss = $('#lblWinLoss');
        domElements.lblMargin = $('#lblMargin');
        domElements.lblBetCount = $('#lblBetCount');

        domElements.titleMonthCount = $('#titleMonthCount');
        domElements.divMonthSelect = $("#divTeamMonthPicker");
        domElements.toggleA = $("#toggleA");
        domElements.custId = $("#lblCustId");
        domElements.monthToCheck = $("#lblMonthToCheck");
        domElements.sportId = $("#lblSportId");
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
        ajaxGetDashboardData();

        domElements.divMonthSelect.on('change', function () {
            currentMonthRange = parseInt(domElements.divMonthSelect.val());
            renderTitle(currentMonthRange);
            ajaxGetDashboardData();
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
    var ajaxGetDashboardData = function () {
        var dashboardParams = {
            CustId: parseInt(domElements.custId.text()),
            MonthToCheck: parseInt(domElements.divMonthSelect.val()),
            SportId: parseInt(domElements.sportId.text())
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetDashboardData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(dashboardParams),
            success: function (result) {
                if (result) {
                    renderDashboard(result.DashboardData, currentChartType);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
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
            domElements.divMonthSelect.append($('<option' + (i == currentMonthRange ? ' selected=""' : '') + '></option>').attr('value', i).text(i + ' months'));
        }
        //domElements.divMonthSelect.val(currentMonthRange);
    };

    var renderDashboard = function (data, chartType) {
        renderGeneralInfo(data.DashboardGeneralInfo);
        drawSummaryChart(data, chartType);
        renderRankInfo(data.DashboardTopSports, data.DashboardTopBettypes, data.DashboardTopEventChoice);
    };

    var renderTitle = function (month) {
        domElements.titleMonthCount.text(month);
    };

    var formatTwoDecimalNumber = function(data)
    {
        return data.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    var renderGeneralInfo = function (data) {
        domElements.lblWinLoss.text(formatTwoDecimalNumber(data.Winloss));
        if (data.Margin != null) {
            domElements.lblMargin.text(formatTwoDecimalNumber(data.Margin) + "%");
        }
        else {
            domElements.lblMargin.text(labels.NA);
        }
        domElements.lblTurnOver.text(formatTwoDecimalNumber(data.Turnover));
        domElements.lblBetCount.text(data.BetCount);
    };

    var createRankRowHtml = function (name, value1, value2, value3) {
        var template = '<div class="rank-row">' +
                        '<div class="name">{0}</div>' +
                       ' <div class="value"><div class="number-value">{1}</div></div>' +
                       ' <div class="value"><div class="number-value">{2}</div></div>' +
                       ' <div class="value"><div class="number-value">{3}</div></div>' +
                        '</div>';
        return template.format(name, value1, value2, value3);
    };

    var renderRankInfo = function (bestSports, bestBettypes, eventChoices) {
        // sports
        domElements.divBestSports.html('');
        if (bestSports) {
            $.each(bestSports, function (index, item) {
                domElements.divBestSports.append(createRankRowHtml(item.SportName, formatTwoDecimalNumber(item.Turnover), formatTwoDecimalNumber(item.Winloss), formatTwoDecimalNumber(item.Margin)));
            });
        }

        // bettypes
        domElements.divBestBettypes.html('');
        if (bestBettypes) {
            $.each(bestBettypes, function (index, item) {
                domElements.divBestBettypes.append(createRankRowHtml(item.BetTypeName, formatTwoDecimalNumber(item.Turnover), formatTwoDecimalNumber(item.Winloss), formatTwoDecimalNumber(item.Margin)));
            });
        }

        // event choices
        domElements.divEventChoices.html('');
        if (eventChoices) {
            $.each(eventChoices, function (index, item) {
                domElements.divEventChoices.append(createRankRowHtml(item.LiveStatus, formatTwoDecimalNumber(item.Turnover), formatTwoDecimalNumber(item.Winloss), formatTwoDecimalNumber(item.Margin)));
            });
        }
    };

    var prepareSummaryColumnData = function (marginData, betcountData, chartData) {
        chartData.MonthMarginData = [{
            name: 'Margin %',
            data: []
        }];

        chartData.MonthBetCountData = [{
            name: 'Bet Count',
            data: []
        }];

        chartData.MonthTOWLData = [{
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
                fillColor: 'white',
                width: 7,
                height: 7
            },
            zIndex: 2,
            data: []
        }];

        chartData.MonthMarginByStakeData = [{
            name: 'Margin % By Stake Range',
            data: []
        }];

        $.each(betcountData, function (index, item) {

            var pipeItem = {
                name: item.ReportMonth + "/" + item.ReportYear,
                y: item.BetCount
            };
            chartData.MonthBetCountData[0].data.push(pipeItem);
        });
        
        $.each(marginData, function (index, item) {
            chartData.MonthCategories.push(item.Month + "/" + item.Year);
            chartData.MonthMarginData[0].data.push(item.Margin);
            chartData.MonthTOWLData[0].data.push(item.Turnover);
            chartData.MonthTOWLData[1].data.push(item.Winloss);
            if (item.Turnover < chartData.MixDataMin) {
                chartData.MixDataMin = item.Turnover;
            }
            if (item.Winloss < chartData.MixDataMin) {
                chartData.MixDataMin = item.Winloss;
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
        domElements.monthBetCountContainer.html('');
        domElements.monthWinlossContainer.html('');

        //make data
        if (data) {
            chartData = {
                MonthMarginData: [],
                MonthTOWLData: [],
                MonthBetCountData: [],
                Categories: [],
                MonthCategories: [],
                MixDataMin: 0
            };

            prepareSummaryColumnData(data.DashboardMarginModels, data.DashboardBetCountByMonth, chartData);
        }

        // margin
        var summaryColumnOption = ChartOptions.ColumnChartOption();
        summaryColumnOption.title.text = 'Margin (%) By Month';
        summaryColumnOption.series = chartData.MonthMarginData;
        summaryColumnOption.xAxis.categories = chartData.MonthCategories;
        if (type === ChartType._2D) {
            summaryColumnOption.chart.options3d.enabled = false;
        }
        domElements.monthMarginContainer.highcharts(summaryColumnOption);

        // bet count
        var pipeChartOption = ChartOptions.PipeChartOption();
        pipeChartOption.title.text = 'Bet Count By Month';
        pipeChartOption.series = chartData.MonthBetCountData;
        if (type === ChartType._2D) {
            pipeChartOption.chart.options3d.enabled = false;
        }
        domElements.monthBetCountContainer.highcharts(pipeChartOption);

        // turnover&winloss
        var mixChartOption = ChartOptions.MixChartOption();
        mixChartOption.title.text = 'WinLoss/Turnover By Month';
        mixChartOption.series = chartData.MonthTOWLData;
        mixChartOption.xAxis.categories = chartData.MonthCategories;
        if (type === ChartType._2D) {
            mixChartOption.chart.options3d.enabled = false;
        }
        domElements.monthWinlossContainer.highcharts(mixChartOption);

        // margin by stake range
        var marginByStakeRangeColumnOption = ChartOptions.ColumnChartOption();
        marginByStakeRangeColumnOption.title.text = 'Margin (%) By Stake Range';
        domElements.monthMarginByStakeContainer.highcharts(marginByStakeRangeColumnOption);
    };
    //}
};