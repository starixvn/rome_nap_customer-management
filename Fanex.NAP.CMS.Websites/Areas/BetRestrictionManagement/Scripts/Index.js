$(document).ready(function () {
    betRestrictionManagement = new Fanex.CMS.BetRestrictionManagement();
    betRestrictionManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionManagement');

Fanex.CMS.BetRestrictionManagement = function () {
    var $this = $(this);

    var DOMElements = {
        btnSubmit: null,
        grdAccount: null,
        grdSite: null,
        grdCurrency: null
    };

    var cacheDOMElements = function () {
        DOMElements.btnSubmit = $('#btnSubmit');
        DOMElements.grdAccount = $('#grdAccount');
        DOMElements.grdSite = $('#grdSite');
        DOMElements.grdCurrency = $('#grdCurrency');
    }

    var bindingEvents = function () {
        DOMElements.btnSubmit.click(editSettings);
    }

    var bindingGridEvents = function (data) {
        $('.grid-toggle').off();
        $('.grid-toggle').click(function () {
            $('#' + $(this).attr('data-grid-id')).toggle();
        });

        $('.toggle-on-off').click(function () {
            $(this).toggleClass('status-on status-off');
        });
    }

    var search = function () {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestrictionManagement/GetAllSettings'),
            null,
            function (data) {
                renderAccountGrid(data.AccountSession);
                renderSiteGrid(data.SiteSession);
                renderCurrencyGrid(data.CurrencySession);
                bindingGridEvents();
            }
        );
    }

    var renderAccountGrid = function (viewModel) {
        var accountGridOptions = {
            gridExpandHeight: 565,
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Sport Type', Width: 100, Name: 'SportName', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Type', Width: 100, Name: 'Type', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Live Bet Accept', Width: 100, Name: 'LiveBetAcceptStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket', Width: 100, Name: 'SeizeBetTicketStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Bet Limit', Width: 100, Name: 'BetLimitStatus', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    text: '<span class="grid-toggle" data-grid-id="grdAccount">[−]</span> By Super/Master/Agent/Member',
                }
        };

        accountGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: '<span data-setting-id="' + viewModel[i].SettingId + '">' + (i + 1) + '</span>' },
                    { Name: 'SportName', Value: viewModel[i].SportName },
                    { Name: 'Type', Value: viewModel[i].Type },
                    { Name: 'LiveBetAcceptStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].LiveBetAcceptStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-live-bet-accept-status="' + viewModel[i].LiveBetAcceptStatus + '" />' },
                    { Name: 'SeizeBetTicketStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].SeizeBetTicketStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-seize-bet-ticket-status="' + viewModel[i].SeizeBetTicketStatus + '" />' },
                    { Name: 'BetLimitStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].BetLimitStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-bet-limit-status="' + viewModel[i].BetLimitStatus + '" />' }
                ]
            };

            accountGridOptions.bodyRows.push(row);
        }

        DOMElements.grdAccount.fanexGrid(accountGridOptions);
    }

    var renderSiteGrid = function (viewModel) {
        var siteGridOptions = {
            gridExpandHeight: 772,
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Sport Type', Width: 100, Name: 'SportName', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Type', Width: 100, Name: 'Type', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Live Bet Accept', Width: 100, Name: 'LiveBetAcceptStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket', Width: 100, Name: 'SeizeBetTicketStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Bet Limit', Width: 100, Name: 'BetLimitStatus', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    text: '<span class="grid-toggle" data-grid-id="grdSite">[−]</span> By Site'
                }
        };

        siteGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: '<span data-setting-id="' + viewModel[i].SettingId + '">' + (i + 1) + '</span>' },
                    { Name: 'SportName', Value: viewModel[i].SportName },
                    { Name: 'Type', Value: viewModel[i].Type },
                    { Name: 'LiveBetAcceptStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].LiveBetAcceptStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-live-bet-accept-status="' + viewModel[i].LiveBetAcceptStatus + '" />' },
                    { Name: 'SeizeBetTicketStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].SeizeBetTicketStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-seize-bet-ticket-status="' + viewModel[i].SeizeBetTicketStatus + '" />' },
                    { Name: 'BetLimitStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].BetLimitStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-bet-limit-status="' + viewModel[i].BetLimitStatus + '" />' }
                ]
            };

            siteGridOptions.bodyRows.push(row);
        }

        DOMElements.grdSite.fanexGrid(siteGridOptions);
    }

    var renderCurrencyGrid = function (viewModel) {
        var currencyGridOptions = {
            gridExpandHeight: 772,
            columns: [
                { HeaderText: 'No.', Width: 50, Name: 'RowNum', HeaderAlign: 'center', CellAlign: 'center', FormatNumber: "0,0" },
                { HeaderText: 'Sport Type', Width: 100, Name: 'SportName', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Type', Width: 100, Name: 'Type', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Live Bet Accept', Width: 100, Name: 'LiveBetAcceptStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Seize Bet Ticket', Width: 100, Name: 'SeizeBetTicketStatus', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Bet Limit', Width: 100, Name: 'BetLimitStatus', HeaderAlign: 'center', CellAlign: 'center' }
            ],
            showTitle: true,
            titleInfo:
                {
                    text: '<span class="grid-toggle" data-grid-id="grdCurrency">[−]</span> By Currency'
                }
        };

        currencyGridOptions.bodyRows = [];
        for (var i = 0; i < viewModel.length; i++) {
            var row = {
                Columns: [
                    { Name: 'RowNum', Value: '<span data-setting-id="' + viewModel[i].SettingId + '">' + (i + 1) + '</span>' },
                    { Name: 'SportName', Value: viewModel[i].SportName },
                    { Name: 'Type', Value: viewModel[i].Type },
                    { Name: 'LiveBetAcceptStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].LiveBetAcceptStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-live-bet-accept-status="' + viewModel[i].LiveBetAcceptStatus + '" />' },
                    { Name: 'SeizeBetTicketStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].SeizeBetTicketStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-seize-bet-ticket-status="' + viewModel[i].SeizeBetTicketStatus + '" />' },
                    { Name: 'BetLimitStatus', Value: '<div class="toggle-on-off ' + (viewModel[i].BetLimitStatus ? 'status-on' : 'status-off') + '" data-setting-' + viewModel[i].SettingId + '-bet-limit-status="' + viewModel[i].BetLimitStatus + '" />' }
                ]
            };

            currencyGridOptions.bodyRows.push(row);
        }

        DOMElements.grdCurrency.fanexGrid(currencyGridOptions);
    }

    var editSettings = function () {
        CMS.disableInput();
        var params = getEditSettingsParams();
        if (params.length == 0) {
            CMS.showErrorMessage(messages.NothingChanged);
            CMS.enableInput();
            return;
        }

        CMS.callAjax(
            this,
            CMS.resolveRootUrl('BetRestrictionManagement/EditSettings'),
            { EditSettings: params },
            function (result) {
                if (result.Code == 0) {
                    CMS.showSuccessMessage(messages.UpdateSuccess);
                }
                else {
                    CMS.showErrorMessage(result.Message);
                }
            },
            null,
            function () {
                CMS.disableInput();
                setTimeout(search, 3000);
            }
        );
    }

    var getEditSettingsParams = function () {
        var params = [];
        $('span[data-setting-id]').each(function () {
            $this = $(this);
            var settingId = $this.attr('data-setting-id');
            var newLiveBetAcceptStatus = getSettingStatusChange('data-setting-' + settingId + '-live-bet-accept-status');
            var newSeizeBetTicketStatus = getSettingStatusChange('data-setting-' + settingId + '-seize-bet-ticket-status');
            var newBetLimitStatus = getSettingStatusChange('data-setting-' + settingId + '-bet-limit-status');

            if (newLiveBetAcceptStatus != null ||
                newSeizeBetTicketStatus != null ||
                newBetLimitStatus != null)
                params.push({ SettingId: settingId, LiveBetAcceptStatus: newLiveBetAcceptStatus, SeizeBetTicketStatus: newSeizeBetTicketStatus, BetLimitStatus: newBetLimitStatus })
        });

        return params;
    }

    var getSettingStatusChange = function (statusElementId) {
        var $status = $('div[' + statusElementId + ']');
        var newStatus = $status.hasClass('status-on') ? true : false;
        var oldStatus = $status.attr(statusElementId) === 'true';
        if (newStatus != oldStatus)
            return newStatus;

        return null;
    }

    this.Init = function () {
        cacheDOMElements();
        bindingEvents();
        search();
    }
}