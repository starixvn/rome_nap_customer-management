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
        viewHistoryModal = new ViewHistoryModal($divViewHistoryContainer, function () {});        
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
            showHighlightRow: true,
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
                    { HeaderText: "Member", Width: 200, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: "Virtual Rate", Width: 75, Name: 'colRate', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: CMSConstants.NumberFormat.IntNumber },
                    { HeaderText: "AVG. Performance", Width: 100, Name: 'colAvg', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber },
                    { HeaderText: result.MonthList[0], Width: 100, Name: 'colMonth1', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber },
                    { HeaderText: result.MonthList[1], Width: 100, Name: 'colMonth2', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber },
                    { HeaderText: result.MonthList[2], Width: 100, Name: 'colMonth3', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.TwoDecimalNumber },
                    { HeaderText: '', Width: 40, Name: 'colViewHistory', HeaderAlign: 'center', CellAlign: 'center' }
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
            var last3MonthsData = [customer.CustMargin[0], customer.CustMargin[1], customer.CustMargin[2]];
            customData = ' data-custid="' + customer.CustId + '" data-username="' + customer.Username + '" data-sportid="' + searchParameters.SportId + '" data-l3mdata="' + last3MonthsData + '"';
            cssClass = i % 2 == 0 ? '' : 'freegrid-even';

            bodyRows.push({
                Columns: [
                    { Name: 'colNumber', Value: (i + 1), MergeRowNumber: 3 },
                    { Name: 'colUsername', Value: customer.Username, MergeRowNumber: 3 },
                    { Name: 'colRate', Value: customer.VRate, MergeRowNumber: 3 },
                    { Name: 'colMonth1', Value: customer.CustMargin[3].Turnover },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Turnover },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Turnover },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Turnover },
                    { Name: 'colViewHistory', Value: renderViewHistoryRow(customer.CustId, customData), MergeRowNumber: 3 }
                ],
                CssClass: cssClass
            });

            bodyRows.push({
                Columns: [
                    { Name: 'colMonth1', Value: customer.CustMargin[3].Winloss },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Winloss },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Winloss },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Winloss }
                ],
                CssClass: cssClass
            });

            bodyRows.push({
                Columns: [
                    { Name: 'colMonth1', Value: customer.CustMargin[3].Margin },
                    { Name: 'colMonth1', Value: customer.CustMargin[0].Margin },
                    { Name: 'colMonth2', Value: customer.CustMargin[1].Margin },
                    { Name: 'colMonth3', Value: customer.CustMargin[2].Margin }
                ],
                CssClass: cssClass
            });
        }

        return bodyRows;
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
            MonthToCheck: 3,
            MonthToSum: $wonperiod.val(),
            PageIndex: 0,
            PageSize: CMS.pageSize
        };
    }
};

virtualSettingMonitor = new VirtualSettingMonitor();