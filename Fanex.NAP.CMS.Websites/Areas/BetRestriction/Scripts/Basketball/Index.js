$(document).ready(function () {
    betRestriction = new Fanex.CMS.BetRestrictionBasketball();
    betRestriction.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionBasketball');

Fanex.CMS.BetRestrictionBasketball = function () {
    var DOMElements = {
        txtUserName: null,
        ddlLevel: null,
        ddlStatus: null,
        ddlSite: null,
        ddlCurrency: null,
        btnSearch: null,
        btnAddAccount: null,
        content: null,
        resultContent: null,
        grdAccount: null,
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
        DOMElements.grdAccount = $('#grdAccount');
        DOMElements.resultContent = $('#result-content');
        DOMElements.content = $('.content').eq(0);
    };

    var bindingEvents = function () {
        DOMElements.txtUserName.keypress(function (e) {
            var key = e.which;
            if (key == 13) {
                betRestriction.Search();
                return false;
            }
        });

        DOMElements.btnSearch.click(betRestriction.Search);

        DOMElements.btnAddAccount.off().click(function () {
            accountManagement.ShowAddAccountPopup();
        });
    };

    var bindingGridEvents = function (data) {
        $('.delete-account')
            .off()
            .click(function () {
                var $this = $(this);
                var custId = $this.attr('data-cust-id');
                var userName = $this.attr('data-user-name');
                accountManagement.RemoveAccount(custId, userName);
            });

        $('.edit-account')
            .off()
            .click(function () {
                var $this = $(this);
                var custId = $this.attr('data-cust-id');
                accountManagement.ShowEditAccountPopup(custId);
            });

        $('.link-leagueGroups').off().click(handleViewLeagueGroups);

        DOMElements.resultContent.height(DOMElements.content.height() * 0.95);
    };

    this.Search = function () {
        CMS.disableInput();

        cacheParamsForPaging = {
            UserName: DOMElements.txtUserName
                .val()
                .trim()
                .replace(/[\n\t]+/g, ''),
            RoleId: DOMElements.ddlLevel.val(),
            SiteId: DOMElements.ddlSite.val(),
            CurrencyIds: DOMElements.ddlCurrency.multiSelect('getCheckedItems').join(','),
            PageSize: CMS.pageSize,
            PageIndex: 1,
        };

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestrictionBasketball/Search'),
            cacheParamsForPaging,
            function (data) {
                renderAccountGrid(data.AccountViewModel, 0, CMS.pageSize);
                bindingGridEvents();
            }
        );
    };

    var renderAccountGrid = function (viewModel, pageIndex, pageSize) {
        var hiddenColumns = hasHostPermission ? [] : [12];

        var accountGridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        { HeaderText: 'No.', Name: 'RowNum', MergeRowNumber: 2 },
                        { HeaderText: 'Account', Name: 'Account', MergeRowNumber: 2 },
                        { HeaderText: 'Level', Name: 'Level', MergeRowNumber: 2 },
                        { HeaderText: 'Status', Name: 'Status', MergeRowNumber: 2 },
                        { HeaderText: 'Site', Name: 'Site', MergeRowNumber: 2 },
                        { HeaderText: 'Live Bet Accept (Secs)', Name: 'LiveBetAccept', MergeRowNumber: 2 },
                        { HeaderText: 'Seize Bet', Name: 'SeizeBetTicket', MergeColNumber: 5 },
                        { HeaderText: 'Currency', Name: 'Currency', MergeRowNumber: 2 },
                        { HeaderText: 'Action', Name: 'Action', MergeRowNumber: 2 }
                    ]
                },
            ],
            columns: [
                {
                    HeaderText: 'No.',
                    Width: 50,
                    Name: 'RowNum',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                    FormatNumber: '0,0',
                },
                {
                    HeaderText: 'Account',
                    Width: 330,
                    Name: 'Account',
                    HeaderAlign: 'center',
                    CellAlign: 'left',
                    AllowTruncate: true,
                },
                {
                    HeaderText: 'Level',
                    Width: 115,
                    Name: 'Level',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Status',
                    Width: 115,
                    Name: 'Status',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Site',
                    Width: 115,
                    Name: 'Site',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Live Bet Accept (Secs)',
                    Width: 170,
                    Name: 'LiveBetAccept',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Seize Bet Ticket (%)',
                    Width: 170,
                    Name: 'SeizeBetTicket',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'League Group',
                    Width: 150,
                    Name: 'LeagueGroup',
                    HeaderAlign: 'center',
                    CellAlign: 'center'
                },
                {
                    HeaderText: '8RM Account',
                    Width: 120,
                    Name: 'FollowAccount',
                    HeaderAlign: 'center',
                    CellAlign: 'left',
                    AllowTruncate: true,
                },
                {
                    HeaderText: 'Ticket Stake (RM)',
                    Width: 120,
                    Name: 'LeastStake',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Accumulate Balance',
                    Width: 80, Name: 'AccumulateBalance',
                    HeaderAlign: 'center',
                    CellAlign: 'center'
                },
                {
                    HeaderText: 'Currency',
                    Width: 115,
                    Name: 'Currency',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
                {
                    HeaderText: 'Action',
                    Width: 135,
                    Name: 'Action',
                    HeaderAlign: 'center',
                    CellAlign: 'center',
                },
            ],
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: reRenderAfterPagedAccountGrid,
                totalItem: viewModel.TotalItems,
            },
            enableHiddenColumns: false,
            hiddenColumns: hiddenColumns,
        };

        accountGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.DataList.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: i + pageIndex * pageSize + 1 },
                    { Name: 'Account', Value: viewModel.DataList[i].Account },
                    { Name: 'Level', Value: viewModel.DataList[i].Level },
                    { Name: 'Status', Value: viewModel.DataList[i].Status },
                    { Name: 'Site', Value: viewModel.DataList[i].Site },
                    {
                        Name: 'LiveBetAccept',
                        Value: getDisplayValueOfLiveBetAccept(viewModel.DataList[i].LiveBetAccept),
                    },
                    {
                        Name: 'SeizeBetTicket',
                        Value: getDisplayValueOfSeizeBetTicket(
                            viewModel.DataList[i].SeizeBetTicket
                        )
                    },
                    { Name: 'LeagueGroup', Value: renderLeagueGroupCell(viewModel.DataList[i].LeagueGroupID) },
                    { Name: 'FollowAccount', Value: viewModel.DataList[i].FollowAccount },
                    { Name: 'LeastStake', Value: viewModel.DataList[i].LeastStake },
                    { Name: 'AccumulateBalance', Value: viewModel.DataList[i].AccumulateBalance ? 'Yes' : 'No' },
                    { Name: 'Currency', Value: viewModel.DataList[i].Currency },
                    {
                        Name: 'Action',
                        Value:
                            '<span class="ico-edit edit-account" data-cust-id="' +
                            viewModel.DataList[i].CustId +
                            '"></span>&nbsp;&nbsp;&nbsp;<span class="ico-delete delete-account" data-cust-id="' +
                            viewModel.DataList[i].CustId +
                            '" data-user-name="' +
                            viewModel.DataList[i].Account +
                            '"></span>',
                    },
                ],
            };
            accountGridOptions.bodyRows.push(row);
        }

        DOMElements.grdAccount.fanexGrid(accountGridOptions);
    };

    var reRenderAfterPagedAccountGrid = function (currentPageIndex, currentPageSize) {
        CMS.disableInput();
        cacheParamsForPaging.PageSize = currentPageSize;
        cacheParamsForPaging.PageIndex = currentPageIndex + 1;

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestrictionBasketball/SearchAccountPaging'),
            cacheParamsForPaging,
            function (data) {
                renderAccountGrid(data, currentPageIndex, currentPageSize);
                bindingGridEvents();
            }
        );
    };

    var getDisplayValueOfLiveBetAccept = function (value) {
        if (value == undefined) return resources.FollowUpperLevel;

        return value;
    };

    var getDisplayValueOfSeizeBetTicket = function (value) {
        if (value == undefined) return resources.FollowUpperLevel;

        if (value == '0' || value == 0) return resources.None;

        return value;
    };

    var getCurrencyItems = function () {
        var result = [];
        DOMElements.ddlCurrency.find('option').each(function () {
            result.push({
                Id: this.value,
                Value: this.value,
                Text: this.text,
                Checked: this.text != 'UUS',
                Enable: true
            });
        });

        return result;
    };

    this.Init = function () {
        cacheDOMElements();
        bindingEvents();
        DOMElements.ddlCurrency.multiSelect({
            minWidth: DOMElements.ddlCurrency.width(),
            checkAllText: 'All',
            listData: getCurrencyItems(),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });
        DOMElements.ddlCurrency = $('#ddlCurrency');
    };
};

function renderLeagueGroupCell(leagueGroupIds) {
    if (leagueGroupIds === "All") {
        return `<a href="javascript:;" class="link-leagueGroups" data-league-groups="${leagueGroupIds}">All</a>`;
    }

    const leagueGroupArray = leagueGroupIds.split(",");
    const numOfLeagues = leagueGroupArray.length;
    if (numOfLeagues > 2) {
        return `<a href="javascript:;" class="link-leagueGroups" data-league-groups="${leagueGroupIds}">${numOfLeagues}</a>`;
    }

    var leagueGroupName = [];
    leagueGroupArray.forEach(function (item) {
        var lg = leagueGroups.find(leaG => leaG.Value === item);
        lg ? leagueGroupName.push(lg.Text) : leagueGroupName.push(item);
    });

    return leagueGroupName.join(", ");
}

function handleViewLeagueGroups() {
    let html = [
        '<div class="form viewLeagueGroupsForm">',
        '<table class="tbl-freegrid">',
        '<thead><tr><th class="width-60">#</th><th>', resources.leagueGroup, '</th></tr></thead>',
        '<tbody>'
    ];
    if ($(this).data("league-groups") === "All") {
        leagueGroups.forEach(function (league, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', league.Text, '</td></tr>');
        });
    } else {
        $(this).data("league-groups").split(",").forEach(function (item, index) {
            var lg = leagueGroups.find(leaG => leaG.Value === item);
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', lg ? lg.Text : item, '</td></tr>');
        });
    }

    html.push('</tbody></table></div>');

    CMS.modal(
        resources.leagueGroup,
        html.join(""),
        {
            resizable: false,
            position: {
                my: "center top",
                at: "center top",
                of: "#headline"
            }
        });
}
