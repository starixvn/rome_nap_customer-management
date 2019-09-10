$(document).ready(function () {
    betRestriction = new Fanex.CMS.BetRestriction();
    betRestriction.Init();
});

Namespace.Register('Fanex.CMS.BetRestriction');

Fanex.CMS.BetRestriction = function () {
    var $this = $(this);

    var DOMElements = {
        txtUserName: null,
        ddlLevel: null,
        ddlStatus: null,
        ddlSite: null,
        ddlCurrency: null,
        btnSearch: null,
        btnAddAccount: null,
        btnAddSite: null,
        btnAddCurrency: null,
        content: null,
        resultContent: null,
        grdAccount: null,
        grdSite: null,
        grdCurrency: null
    };

    var cacheParamsForPaging = {};

    var cacheDOMElements = function () {
        DOMElements.txtUserName = $('#txtUserName');
        DOMElements.ddlLevel = $('#ddlLevel');
        DOMElements.ddlStatus = $('#ddlStatus');
        DOMElements.ddlSite = $('#ddlSite');
        DOMElements.ddlCurrency = $('#ddlCurrency');
        DOMElements.btnSearch = $('#btnSearch');
        DOMElements.btnAddAccount = $('#btnAddAccount');
        DOMElements.btnAddSite = $('#btnAddSite');
        DOMElements.btnAddCurrency = $('#btnAddCurrency');
        DOMElements.grdAccount = $('#grdAccount');
        DOMElements.grdSite = $('#grdSite');
        DOMElements.grdCurrency = $('#grdCurrency');
        DOMElements.resultContent = $('#result-content');
        DOMElements.content = $('.content').eq(0);
    }

    var bindingEvents = function () {
        DOMElements.txtUserName.keypress(function (e) {
            var key = e.which;
            if (key == 13) {
                betRestriction.Search();
                return false;
            }
        });

        DOMElements.btnSearch.click(betRestriction.Search);
    }

    var bindingGridEvents = function (data) {
        $('.grid-toggle').off();
        $('.grid-toggle').click(function () {
            $('#' + $(this).attr('data-grid-id')).toggle();
        });

        $('.delete-account').off();
        $('.delete-account').click(function () {
            var $this = $(this);
            var custId = $this.attr('data-cust-id');
            var userName = $this.attr('data-user-name');
            accountManagement.RemoveAccount(custId, userName);
        });

        $('.edit-account').off();
        $('.edit-account').click(function () {
            var $this = $(this);
            var custId = $this.attr('data-cust-id');
            accountManagement.ShowEditAccountPopup(custId);
        });

        $('#btnAddAccount').off();
        $('#btnAddAccount').click(function () {
            accountManagement.ShowAddAccountPopup();
        });

        $('.delete-site').off();
        $('.delete-site').click(function () {
            var $this = $(this);
            var siteId = $this.attr('data-site-id');
            var siteName = $this.attr('data-site-name');
            siteManagement.RemoveSite(siteId, siteName);
        });

        $('.edit-site').off();
        $('.edit-site').click(function () {
            var $this = $(this);
            var siteId = $this.attr('data-site-id');
            siteManagement.ShowEditSitePopup(siteId);
        });

        $('#btnAddSite').off();
        $('#btnAddSite').click(function () {
            siteManagement.ShowAddSitePopup();
        });

        $('.delete-currency').off();
        $('.delete-currency').click(function () {
            var $this = $(this);
            var exchangeId = $this.attr('data-exchange-id');
            var currencyName = $this.attr('data-currency-name');
            var type = $this.attr('data-type');
            currencyManagement.RemoveCurrency(exchangeId, currencyName, type);
        });

        $('.edit-currency').off();
        $('.edit-currency').click(function () {
            var $this = $(this);
            var exchangeId = $this.attr('data-exchange-id');
            var type = $this.attr('data-type');
            currencyManagement.ShowEditCurrencyPopup(exchangeId, type);
        });

        $('#btnAddCurrency').off();
        $('#btnAddCurrency').click(function () {
            currencyManagement.ShowAddCurrencyPopup();
        });

        DOMElements.resultContent.height(DOMElements.content.height() * 0.95);
    }

    this.Search = function () {
        CMS.disableInput();
        cacheParamsForPaging = {
            UserName: DOMElements.txtUserName.val(),
            RoleId: DOMElements.ddlLevel.val(),
            Status: DOMElements.ddlStatus.val(),
            SiteId: DOMElements.ddlSite.val(),
            CurrencyId: DOMElements.ddlCurrency.val(),
            PageSize: CMS.pageSize,
            PageIndex: 1
        };

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestriction/Search'),
            cacheParamsForPaging,
            function (data) {
                renderAccountGrid(data.AccountViewModel, 0, CMS.pageSize);
                renderSiteGrid(data.SiteViewModel, 0, CMS.pageSize);
                renderCurrencyGrid(data.CurrencyViewModel, 0, CMS.pageSize);
                bindingGridEvents();
            }
        );
    }

    var renderAccountGrid = function (viewModel, pageIndex, pageSize) {
        var accountGridOptions = {
            fullHeight: false,
            gridHeight: window.innerHeight * 0.3,
            //gridExpandHeight: window.innerHeight * 0.61,
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        { HeaderText: 'No.', Name: 'RowNum', MergeRowNumber: 2 },
                        { HeaderText: 'Headline', Name: 'Headline', MergeRowNumber: 2 },
                        { HeaderText: 'Account', Name: 'Account', MergeRowNumber: 2 },
                        { HeaderText: 'Level', Name: 'Level', MergeRowNumber: 2 },
                        { HeaderText: 'Status', Name: 'Status', MergeRowNumber: 2 },
                        { HeaderText: 'Site', Name: 'Site', MergeRowNumber: 2 },
                        { HeaderText: 'Live Bet Accept (Secs)', Name: 'LiveBetAccept', MergeRowNumber: 2 },
                        { HeaderText: 'Seize Bet Ticket (%)', Name: 'SeizeBetTicket', MergeRowNumber: 2 },
                        { HeaderText: 'Currency', Name: 'Currency', MergeRowNumber: 2 },
                        { HeaderText: 'Bet Limit Control', Name: 'MaxBet', MergeColNumber: 2 },
                        { HeaderText: 'Problem Account', Name: 'ProblemAccount', MergeRowNumber: 2 },
                        { HeaderText: '8RM Account', Name: 'FollowAccount', MergeRowNumber: 2 },
                        { HeaderText: 'Action', Name: 'Action', MergeRowNumber: 2 }
                    ]
                },
            ],
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Headline', Width: 200, Name: 'Headline', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Account', Width: 330, Name: 'Account', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Level', Width: 70, Name: 'Level', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Status', Width: 70, Name: 'Status', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Site', Width: 80, Name: 'Site', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Live Bet Accept (Secs)', Width: 80, Name: 'LiveBetAccept', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket (%)', Width: 85, Name: 'SeizeBetTicket', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Currency', Width: 60, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Max Bet', Width: 100, Name: 'MaxBet', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: "0,0.00" },
                { HeaderText: 'Max Per Match', Width: 100, Name: 'MaxPerMatch', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: "0,0.00" },
                { HeaderText: 'Problem Account', Width: 60, Name: 'ProblemAccount', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Follow Account', Width: 200, Name: 'FollowAccount', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Action', Width: 60, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: '<span class="grid-toggle" data-grid-id="grdAccount">[−]</span> By Super/Master/Agent/Member',
                    customButtons: '<input id="btnAddAccount" type="button" class="title-button" value="Add Account">'
                },
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: reRenderAfterPagedAccountGrid,
                totalItem: viewModel.TotalItems
            }
        };

        accountGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.DataList.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: i + pageIndex * pageSize + 1 },
                    { Name: 'Headline', Value: viewModel.DataList[i].Headline },
                    { Name: 'Account', Value: viewModel.DataList[i].Account },
                    { Name: 'Level', Value: viewModel.DataList[i].Level },
                    { Name: 'Status', Value: viewModel.DataList[i].Status },
                    { Name: 'Site', Value: viewModel.DataList[i].Site },
                    { Name: 'LiveBetAccept', Value: viewModel.DataList[i].LiveBetAccept },
                    { Name: 'SeizeBetTicket', Value: viewModel.DataList[i].SeizeBetTicket == null ? '' : viewModel.DataList[i].SeizeBetTicket },
                    { Name: 'Currency', Value: viewModel.DataList[i].Currency },
                    { Name: 'MaxBet', Value: viewModel.DataList[i].MaxBet == null ? '' : viewModel.DataList[i].MaxBet },
                    { Name: 'MaxPerMatch', Value: viewModel.DataList[i].MaxPerMatch == null ? '' : viewModel.DataList[i].MaxPerMatch },
                    { Name: 'ProblemAccount', Value: viewModel.DataList[i].ProblemAccount },
                    { Name: 'FollowAccount', Value: viewModel.DataList[i].FollowAccount },
                    { Name: 'Action', Value: '<span class="ico-edit edit-account" data-cust-id="' + viewModel.DataList[i].CustId + '"></span>&nbsp;&nbsp;&nbsp;<span class="ico-delete delete-account" data-cust-id="' + viewModel.DataList[i].CustId + '" data-user-name="' + viewModel.DataList[i].Account + '"></span>' }
                ]
            };

            accountGridOptions.bodyRows.push(row);
        }

        DOMElements.grdAccount.fanexGrid(accountGridOptions);
    }

    var reRenderAfterPagedAccountGrid = function (currentPageIndex, currentPageSize) {
        CMS.disableInput();
        cacheParamsForPaging.PageSize = currentPageSize;
        cacheParamsForPaging.PageIndex = currentPageIndex + 1;

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestriction/SearchAccountPaging'),
            cacheParamsForPaging,
            function (data) {
                renderAccountGrid(data, currentPageIndex, currentPageSize);
                bindingGridEvents();
            }
        );
    }

    var renderSiteGrid = function (viewModel, pageIndex, pageSize) {
        var siteGridOptions = {
            fullHeight: false,
            gridHeight: window.innerHeight * 0.15,
            //gridExpandHeight: window.innerHeight * 0.83,
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Site', Width: 200, Name: 'Site', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Live Bet Accept (Secs)', Width: 100, Name: 'LiveBetAccept', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket (%)', Width: 100, Name: 'SeizeBetTicket', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: '8RM Account', Width: 100, Name: 'FollowAccount', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Action', Width: 100, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: '<span class="grid-toggle" data-grid-id="grdSite">[−]</span> By Site',
                    customButtons: '<input id="btnAddSite" type="button" class="title-button" value="Add Site">'
                },
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: reRenderAfterPagedSiteGrid,
                totalItem: viewModel.TotalItems
            }
        };

        siteGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.DataList.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: i + pageIndex * pageSize + 1 },
                    { Name: 'Site', Value: viewModel.DataList[i].Site },
                    { Name: 'LiveBetAccept', Value: viewModel.DataList[i].LiveBetAccept },
                    { Name: 'SeizeBetTicket', Value: viewModel.DataList[i].SeizeBetTicket == null ? '' : viewModel.DataList[i].SeizeBetTicket },
                    { Name: 'FollowAccount', Value: viewModel.DataList[i].FollowAccount },
                    { Name: 'Action', Value: '<span class="ico-edit edit-site" data-site-id="' + viewModel.DataList[i].SiteId + '"></span>&nbsp;&nbsp;&nbsp;<span class="ico-delete delete-site" data-site-id="' + viewModel.DataList[i].SiteId + '" data-site-name="' + viewModel.DataList[i].Site + '"></span>' }
                ]
            };

            siteGridOptions.bodyRows.push(row);
        }

        DOMElements.grdSite.fanexGrid(siteGridOptions);
    }

    var reRenderAfterPagedSiteGrid = function (currentPageIndex, currentPageSize) {
        CMS.disableInput();
        cacheParamsForPaging.PageSize = currentPageSize;
        cacheParamsForPaging.PageIndex = currentPageIndex + 1;

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestriction/SearchSitePaging'),
            cacheParamsForPaging,
            function (data) {
                renderSiteGrid(data, currentPageIndex, currentPageSize);
                bindingGridEvents();
            }
        );

    }

    var renderCurrencyGrid = function (viewModel, pageIndex, pageSize) {
        var currencyGridOptions = {
            fullHeight: false,
            gridHeight: window.innerHeight * 0.15,
            //gridExpandHeight: window.innerHeight * 0.83,
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Currency', Width: 200, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Type', Width: 100, Name: 'Type', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Live Bet Accept (Secs)', Width: 100, Name: 'LiveBetAccept', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket (%)', Width: 100, Name: 'SeizeBetTicket', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: '8RM Account', Width: 100, Name: 'FollowAccount', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Action', Width: 100, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    showButtonsWhenNoData: true,
                    text: '<span class="grid-toggle" data-grid-id="grdCurrency">[−]</span> By Currency',
                    customButtons: '<input id="btnAddCurrency" type="button" class="title-button" value="Add Currency">'
                },
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: reRenderAfterPagedCurrencyGrid,
                totalItem: viewModel.TotalItems
            }
        };

        currencyGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.DataList.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: i + pageIndex * pageSize + 1 },
                    { Name: 'Currency', Value: viewModel.DataList[i].Currency },
                    { Name: 'Type', Value: viewModel.DataList[i].TypeName },
                    { Name: 'LiveBetAccept', Value: viewModel.DataList[i].LiveBetAccept },
                    { Name: 'SeizeBetTicket', Value: viewModel.DataList[i].SeizeBetTicket == null ? '' : viewModel.DataList[i].SeizeBetTicket },
                    { Name: 'FollowAccount', Value: viewModel.DataList[i].FollowAccount },
                    { Name: 'Action', Value: '<span class="ico-edit edit-currency" data-exchange-id="' + viewModel.DataList[i].ExchangeId + '" data-type="' + viewModel.DataList[i].Type + '"></span>&nbsp;&nbsp;&nbsp;<span class="ico-delete delete-currency" data-exchange-id="' + viewModel.DataList[i].ExchangeId + '" data-currency-name="' + viewModel.DataList[i].Currency + '" data-type="' + viewModel.DataList[i].Type + '"></span>' }
                ]
            };

            currencyGridOptions.bodyRows.push(row);
        }

        DOMElements.grdCurrency.fanexGrid(currencyGridOptions);
    }

    var reRenderAfterPagedCurrencyGrid = function (currentPageIndex, currentPageSize) {
        CMS.disableInput();
        cacheParamsForPaging.PageSize = currentPageSize;
        cacheParamsForPaging.PageIndex = currentPageIndex + 1;

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestriction/SearchCurrencyPaging'),
            cacheParamsForPaging,
            function (data) {
                renderCurrencyGrid(data, currentPageIndex, currentPageSize);
                bindingGridEvents();
            }
        );
    }

    this.Init = function () {
        cacheDOMElements();
        bindingEvents();
    }
}