/* global CMS, CMSConstants, winLimitControlLabels, ViewHistoryModal */
/* jshint strict: false,
camelcase: false */
var virtualSettingMonitor;
$(document).ready(function () {
    virtualSettingMonitor.initPage();
});

var VirtualSettingMonitor = function () {
    var self = this;
    var $formFilter = null,
        $sport = null,
        $rate = null,
        $toprows = null,
        $wonperiod = null,
        $freegrid = null,

        $divButtons = null,
        $divResult = null,
        $divViewHistoryContainer = null;

    var searchUrl = '',
        searchParameters = null,
        gridOptions = null,
        maxPercentageUsed = 100;

    var viewHistoryModal = null;
    var fullReportData = null;

    self.initPage = function () {
        cacheElements();
        bindEvents();
        gridOptions = initGridOptions();
        searchUrl = $formFilter.attr('action') + '?';

        $('#divModals').empty();
        viewHistoryModal = new ViewHistoryModal($divViewHistoryContainer, function () { });
    };

    function cacheElements() {
        $formFilter = $('#form-virtualsetting');
        $sport = $('#sport');
        $rate = $('#rate');
        $toprows = $('#toprows');
        $wonperiod = $('#wonperiod');
        $freegrid = $('#freegrid');

        $divButtons = $('#divButtons');
        $divResult = $('#div-result-message');
        $divViewHistoryContainer = $('#div-ViewHistory');
    }

    function initGridOptions() {
        return {
            showTitle: true,
            titleInfo: {},
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 35,
            gridExpandHeight: 180,
            columns: [],
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: [10, 50, 100, 200],
                pagerSelectCallBack: reRenderAfterPaged
            }
        };
    }

    function getVirtualSettings() {
        CMS.hideInlineMsg($divResult);
        var url = searchUrl + $.param(searchParameters);
        var successFunc = function (result) {
            fullReportData = result;
            gridOptions.pagerOption.currentPage = searchParameters.PageIndex;
            gridOptions.pagerOption.itemsPerPage = searchParameters.PageSize;
            gridOptions.pagerOption.totalItem = result.TotalRow;

            gridOptions.columns =
                [
                    { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: "Member", Width: 150, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: "Virtual Rate", Width: 75, Name: 'colRate', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: CMSConstants.NumberFormat.IntNumber },
                    { HeaderText: "Figure", Width: 100, Name: 'colFigure', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: "AVG. Performance", Width: 100, Name: 'colAvg', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[0], Width: 100, Name: 'colMonth1', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[1], Width: 100, Name: 'colMonth2', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[2], Width: 100, Name: 'colMonth3', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[3], Width: 100, Name: 'colMonth4', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[4], Width: 100, Name: 'colMonth5', HeaderAlign: 'center', CellAlign: 'right',  },
                    { HeaderText: result.MonthList[5], Width: 100, Name: 'colMonth6', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[6], Width: 100, Name: 'colMonth7', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[7], Width: 100, Name: 'colMonth8', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[8], Width: 100, Name: 'colMonth9', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[9], Width: 100, Name: 'colMonth10', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[10], Width: 100, Name: 'colMonth11', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[11], Width: 100, Name: 'colMonth12', HeaderAlign: 'center', CellAlign: 'right', },
                    { HeaderText: result.MonthList[12], Width: 100, Name: 'colMonth13', HeaderAlign: 'center', CellAlign: 'right', }
                ];

            gridOptions.bodyRows = buildBodyRows();

            $freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (result.TotalRow > 0) {
                bindViewHistory();
                $divButtons.removeClass('display-none');
            }
            else {
                $divButtons.addClass('display-none');
            }
        };

        CMS.getJson(url, successFunc);
    }

    function buildBodyRows() {
        var data = fullReportData.VirtualSettings;
        var bodyRows = [];
        var cssClass = 'freegrid-even';
        var startIndex = searchParameters.PageIndex * searchParameters.PageSize;
        var endIndex = Math.min(data.CustMargin.length, startIndex + searchParameters.PageSize);

        var customer, customData;
        isSoundAlert = false;
        for (var i = startIndex; i < endIndex; i++) {
            customer = data.CustMargin[i];
            var last3MonthsData = [customer.CustMargin[0].Turnover, customer.CustMargin[0].Winloss, customer.CustMargin[0].Margin,
                customer.CustMargin[1].Turnover, customer.CustMargin[1].Winloss, customer.CustMargin[1].Margin,
                customer.CustMargin[2].Turnover, customer.CustMargin[2].Winloss, customer.CustMargin[2].Margin];
            customData = ' data-custid="' + customer.CustId + '" data-username="' + customer.Username + '" data-sportid="' + searchParameters.SportId + '" data-l3mdata="' + last3MonthsData + '"';
            cssClass = i % 2 == 0 ? '' : 'freegrid-even';

            bodyRows.push({
                Columns: [
                    { Name: 'colNumber', Value: (i + 1), MergeRowNumber: 4 },
                    { Name: 'colUsername', Value: renderCustomerDashboardRow(customer.Username, customer.CustId), MergeRowNumber: 4 },
                    { Name: 'colRate', Value: customer.VRate, MergeRowNumber: 4 },
                    { Name: 'colFigure', Value: "Turnover" },
                    { Name: 'colAvg', Value: customer.CustMargin[13].Turnover.toFixed(2) },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Turnover.toFixed(2) },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Turnover.toFixed(2) },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Turnover.toFixed(2) },
                    { Name: 'colMonth4', Value: customer.CustMargin[3].Turnover.toFixed(2) },
                    { Name: 'colMonth5', Value: customer.CustMargin[4].Turnover.toFixed(2) },
                    { Name: 'colMonth6', Value: customer.CustMargin[5].Turnover.toFixed(2) },
                    { Name: 'colMonth7', Value: customer.CustMargin[6].Turnover.toFixed(2) },
                    { Name: 'colMonth8', Value: customer.CustMargin[7].Turnover.toFixed(2) },
                    { Name: 'colMonth9', Value: customer.CustMargin[8].Turnover.toFixed(2) },
                    { Name: 'colMonth10', Value: customer.CustMargin[9].Turnover.toFixed(2) },
                    { Name: 'colMonth11', Value: customer.CustMargin[10].Turnover.toFixed(2) },
                    { Name: 'colMonth12', Value: customer.CustMargin[11].Turnover.toFixed(2) },
                    { Name: 'colMonth13', Value: customer.CustMargin[12].Turnover.toFixed(2) }
                ],
                CssClass: cssClass
            });

            bodyRows.push({
                Columns: [
                    { Name: 'colFigure', Value: "Winloss" },
                    { Name: 'colAvg', Value: customer.CustMargin[12].Winloss.toFixed(2) },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Winloss.toFixed(2) },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Winloss.toFixed(2) },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Winloss.toFixed(2) },
                    { Name: 'colMonth4', Value: customer.CustMargin[3].Winloss.toFixed(2) },
                    { Name: 'colMonth5', Value: customer.CustMargin[4].Winloss.toFixed(2) },
                    { Name: 'colMonth6', Value: customer.CustMargin[5].Winloss.toFixed(2) },
                    { Name: 'colMonth7', Value: customer.CustMargin[6].Winloss.toFixed(2) },
                    { Name: 'colMonth8', Value: customer.CustMargin[7].Winloss.toFixed(2) },
                    { Name: 'colMonth9', Value: customer.CustMargin[8].Winloss.toFixed(2) },
                    { Name: 'colMonth10', Value: customer.CustMargin[9].Winloss.toFixed(2) },
                    { Name: 'colMonth11', Value: customer.CustMargin[10].Winloss.toFixed(2) },
                    { Name: 'colMonth12', Value: customer.CustMargin[11].Winloss.toFixed(2) },
                    { Name: 'colMonth13', Value: customer.CustMargin[12].Turnover.toFixed(2) }
                ],
                CssClass: cssClass
            });

            bodyRows.push({
                Columns: [
                    { Name: 'colFigure', Value: "Margin" },
                    { Name: 'colAvg', Value: customer.CustMargin[13].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth4', Value: customer.CustMargin[3].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth5', Value: customer.CustMargin[4].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth6', Value: customer.CustMargin[5].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth7', Value: customer.CustMargin[6].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth8', Value: customer.CustMargin[7].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth9', Value: customer.CustMargin[8].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth10', Value: customer.CustMargin[9].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth11', Value: customer.CustMargin[10].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth12', Value: customer.CustMargin[11].Margin.toFixed(2) + "%" },
                    { Name: 'colMonth13', Value: customer.CustMargin[12].Margin.toFixed(2) + "%" }
                ],
                CssClass: cssClass
            });

            bodyRows.push({
                Columns: [
                    { Name: 'colFigure', Value: "BetCount" },
                    { Name: 'colAvg', Value: customer.CustMargin[13].BetCount },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].BetCount },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].BetCount },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].BetCount },
                    { Name: 'colMonth4', Value: customer.CustMargin[3].BetCount },
                    { Name: 'colMonth5', Value: customer.CustMargin[4].BetCount },
                    { Name: 'colMonth6', Value: customer.CustMargin[5].BetCount },
                    { Name: 'colMonth7', Value: customer.CustMargin[6].BetCount },
                    { Name: 'colMonth8', Value: customer.CustMargin[7].BetCount },
                    { Name: 'colMonth9', Value: customer.CustMargin[8].BetCount },
                    { Name: 'colMonth10', Value: customer.CustMargin[9].BetCount },
                    { Name: 'colMonth11', Value: customer.CustMargin[10].BetCount },
                    { Name: 'colMonth12', Value: customer.CustMargin[11].BetCount },
                    { Name: 'colMonth13', Value: customer.CustMargin[12].BetCount }
                ],
                CssClass: cssClass
            });
        }

        return bodyRows;
    }

    function renderCustomerDashboardRow(username, custid) {
        var html;
        html = [
                '<a class="linkEdit" href="CustomerDashboard/Index?CustId=',
                custid,
                '&MonthToCheck=',
                $wonperiod.val(),
                '&SportId=',
                $sport.val(),
                '">',
                username,
                '</a>'
        ];

        return html.join('');
    }

    function renderViewHistoryRow(custid, customData) {
        var customAttr = [
            '<a href="javascript:;" class="ico-remark" ',
            customData,
            '" title="',
            "View History",
            '" ></a>'
        ];

        return customAttr.join('');
    }

    function reRenderAfterPaged(pageIndex, itemsPerPage) {
        searchParameters.PageIndex = parseInt(pageIndex);
        searchParameters.PageSize = parseInt(itemsPerPage);

        gridOptions.pagerOption.itemsPerPage = itemsPerPage;
        gridOptions.pagerOption.currentPage = pageIndex;
        gridOptions.bodyRows = buildBodyRows();
        $freegrid.fanexGrid(gridOptions);
        bindViewHistory();
    }

    function bindViewDashboard() {
        $freegrid.find('.linkDashboard')
            .unbind('click')
            .bind('click', function () {
                var $this = $(this);
                var custid = $this.data('custid');
                var monthtocheck = $this.data('monthtocheck');

            });
    }

    function bindViewHistory() {
        $freegrid.find('.ico-remark')
            .unbind('click')
            .bind('click', function () {
                var $this = $(this);
                var custid = $this.data('custid');
                var username = $this.data('username');
                var sportid = $this.data('sportid');
                var l3mdata = $this.data('l3mdata');

                viewHistoryModal.initialize(custid, username, sportid, l3mdata);
            });
    }

    function bindEvents() {
        $formFilter.submit(function (e) {
            e.preventDefault();
            searchParameters = getSearchParams();
            getVirtualSettings();
        });
    }

    function getSearchParams() {
        return {
            SportId: $sport.val(),
            Rate: $rate.val(),
            TopRows: $toprows.val(),
            MonthToCheck: 6,
            MonthToSum: $wonperiod.val(),
            PageIndex: 0,
            PageSize: CMS.pageSize
        };
    }
};

virtualSettingMonitor = new VirtualSettingMonitor();