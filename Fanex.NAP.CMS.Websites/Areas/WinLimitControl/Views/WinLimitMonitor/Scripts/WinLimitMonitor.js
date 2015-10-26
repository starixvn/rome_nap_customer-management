/* global CMS, winLimitControlLabels */
/* jshint strict: false,
camelcase: false */
var winLimitMonitor;
$(document).ready(function () {
    winLimitMonitor.initPage();
});

var WinLimitMonitor = function () {
    var self = this;
    self.documentElements = {
        formFilter: null,
        inputUsername: null,
        selectGame: null,
        selectCurrency: null,
        selectStatus: null,
        freegrid: null,
        divButtons: null,
        btnUnblocked: null
    };

    self.documentData = {
        searchUrl: '',
        gridOptions: null,
        results: null,
        maxPercentageUsed: 100,
        defaultPageSize: 50,
        searchParameters: {
            PageSize: 1,
            PageIndex: 0
        },
        refreshTimer: {
            Intervals: [90, 120, 150, 180],
            Selected: 90
        }
    };

    self.timer = null;

    self.initPage = function () {
        self.cacheElements();
        self.bindEvents();
        self.documentData.gridOptions = self.initGridOptions();
        self.documentData.searchUrl = self.documentElements.formFilter.attr('action');
        self.documentElements.inputUsername.autocomplete({
            minLength: 3,
            delay: 300,
            source: window.rootUrl + 'WinLimitControl/PlayerSetting/AutoSuggestion'
        });
    };

    self.cacheElements = function () {
        self.documentElements.formFilter = $('#form-filterWinLimit');
        self.documentElements.inputUsername = $('#username');
        self.documentElements.selectGame = $('#game');
        self.documentElements.selectCurrency = $('#currency');
        self.documentElements.selectStatus = $('#status');
        self.documentElements.freegrid = $('#freegrid');
        self.documentElements.divButtons = $('#divButtons');
        self.documentElements.btnUnblocked = $('#btnUnblocked');
        self.documentElements.divResult = $('#div-result-message');
    };

    self.initGridOptions = function () {
        var columns = [
            { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 200, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.Currency, Width: 80, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Game, Width: 140, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.WinLimit, Width: 150, Name: 'colWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat
            },
            { HeaderText: winLimitControlLabels.WinningAmount, Width: 150, Name: 'colWinningAmount',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat
            },
            { HeaderText: winLimitControlLabels.BlockedTime, Width: 120, Name: 'colBlockedTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.UnblockedTime, Width: 120, Name: 'colUnblockedTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.PercentageUsed, Width: 120, Name: 'colPercentageUsed', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Blocked, Width: 60, Name: 'colBlocked', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        return {
            showTitle: true,
            titleInfo: {},
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 230,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: self.documentData.searchParameters.PageSize,
                currentPage: self.documentData.searchParameters.PageIndex,
                totalItem: 0,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: self.handleChangingPage
            }
        };
    };

    self.getPlayerSettings = function () {
        CMS.hideInlineMsg(self.documentElements.divResult);
        var url = self.documentData.searchUrl + '?' + $.param(self.documentData.searchParameters);
        var successFunc = function (results) {
            self.documentData.results = results;
            self.documentData.gridOptions.bodyRows = self.buildBodyRows(results.PlayerWinLimitMonitorModels);
            self.documentData.gridOptions.pagerOption.totalItem = results ? results.TotalPlayers : 0;
            self.documentData.gridOptions.titleInfo.customButtons = renderAutoRefresh();

            self.documentElements.freegrid.removeClass('display-none').fanexGrid(self.documentData.gridOptions);
            if (self.documentData.gridOptions.pagerOption.totalItem) {
                bindAutoRefreshEvents();

                self.documentElements.btnUnblocked.val(
                    self.documentData.searchParameters.Status === "1" ?
                    winLimitControlLabels.Unblock_Accounts :
                    winLimitControlLabels.Block_Accounts
                );
                self.documentElements.divButtons.removeClass('display-none');
            } else {
                self.documentElements.divButtons.addClass('display-none');
            }
        };

        CMS.getJson(url, successFunc);
    };

    self.buildBodyRows = function (results) {
        var bodyRows = [];
        var cell, playerWinLimit, winlimit;
        var mergeRowNumber = 0;
        var resultLength = results ? results.length : 0;

        var startIndex = 0;
        var endIndex = startIndex + self.documentData.searchParameters.PageSize - 1;
        for (var i = startIndex; i < resultLength && i <= endIndex; i++) {
            playerWinLimit = results[i];

            mergeRowNumber = playerWinLimit.GameWinLimitMonitors ? playerWinLimit.GameWinLimitMonitors.length : 0;
            if (mergeRowNumber === 0) {
                continue;
            }

            for (var j = 0; j < mergeRowNumber; j++) {
                cell = [];
                winlimit = playerWinLimit.GameWinLimitMonitors[j];

                if (j === 0) {
                    cell.push({ Name: 'colNumber', Value: (i + 1), MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colUsername', Value: playerWinLimit.PlayerInfo.Name, MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colCurrency', Value: playerWinLimit.Currency.Name, MergeRowNumber: mergeRowNumber });
                }

                var checkBoxId = ' player-index="' + i + '"' +
                    ' game-index="' + j + '"';
                cell.push({ Name: 'colGame', Value: winlimit.Game.Name });
                cell.push({ Name: 'colWinLimit', Value: winlimit.MaxWinLimit });
                cell.push({ Name: 'colWinningAmount', Value: winlimit.WinningAmount });
                cell.push({ Name: 'colBlockedTime', Value: winlimit.BlockedTimeDisplay });
                cell.push({ Name: 'colUnblockedTime', Value: winlimit.UnblockedTimeDisplay });
                cell.push({ Name: 'colPercentageUsed', Value: self.getPercentageUsedBox(winlimit.PercentageUsed) });
                cell.push({ Name: 'colBlocked',
                    Value: '<input type="checkbox" class="checkbox-blocked"' + (winlimit.IsBlocked ? ' checked="checked" ' : '') + checkBoxId + '/>'
                });

                bodyRows.push({ Columns: cell });
            }
        }

        return bodyRows;
    };

    self.getPercentageUsedBox = function (percentageUsed) {
        var html = '';
        var displayPercent = parseInt(percentageUsed);
        if (displayPercent >= self.documentData.maxPercentageUsed) {
            html = '<div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + displayPercent + '%</div>';
        } else {
            html = '<div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
                                (100 - displayPercent) + 'px">' + displayPercent +
                   '%</div>';
        }

        return html;
    };

    self.handleChangingPage = function (pageIndex, itemsPerPage) {
        self.documentData.gridOptions.pagerOption.itemsPerPage = itemsPerPage;
        self.documentData.gridOptions.pagerOption.currentPage = pageIndex;
        self.documentData.searchParameters.PageSize = parseInt(itemsPerPage);
        self.documentData.searchParameters.PageIndex = parseInt(pageIndex) + 1;
        self.getPlayerSettings();
    };

    function renderAutoRefresh() {
        var intervals = self.documentData.refreshTimer.Intervals,
            selected = self.documentData.refreshTimer.Selected;

        var interval,
            dropdown = ['<ul class="dropdown-container">'];
        for (var i = 0, max = intervals.length; i < max; i++) {
            interval = intervals[i];
            dropdown.push('<li data-value="');
            dropdown.push(interval);
            dropdown.push('"');
            if (selected === interval) {
                dropdown.push(' class="checked"');
            }

            dropdown.push('>');
            dropdown.push(interval);
            dropdown.push(' ');
            dropdown.push('sec');
            dropdown.push('</li>');
        }

        dropdown.push('</ul>');

        return '<div class="refresh-container" id="refresh-container">' +
                    '<div class="timer">' +
                        '<div title="Refresh" class="countdown">' + selected + '</div>' +
                        '<div class="dropdown"></div>' +
                     '</div>' + dropdown.join('') +
                '</div>';
    }

    function bindAutoRefreshEvents() {
        if (self.timer) {
            clearTimeout(self.timer);
        }

        var refreshContainer = $('#refresh-container');
        var countdownContainer = refreshContainer.find('.countdown');
        var dropdownContainer = refreshContainer.find('.dropdown-container');

        var autoRefreshFunc = function (countdownContainer) {
            self.timer = setTimeout(function () {
                var currentValue = parseInt(countdownContainer.text());
                if (currentValue === 1) {
                    self.getPlayerSettings();
                    return;
                }

                countdownContainer.html(--currentValue);
                autoRefreshFunc(countdownContainer);
            }, 1000);
        };

        countdownContainer.unbind('click').bind('click', self.getPlayerSettings);

        dropdownContainer.find('li')
            .unbind('click')
            .bind('click', function () {
                dropdownContainer.find('li.checked').removeClass('checked');

                var $this = $(this);
                $this.addClass('checked');
                self.documentData.refreshTimer.Selected = parseInt($this.data('value'));
                countdownContainer.html(self.documentData.refreshTimer.Selected);
            });

        autoRefreshFunc(countdownContainer);
    }

    self.bindEvents = function () {
        self.documentElements.formFilter.submit(function (e) {
            e.preventDefault();
            self.resetPagingOptions();
            self.getPlayerSettings();
        });

        self.documentElements.btnUnblocked.hover(
        function () {
            self.checkSelectedAccounts();
        },
        function () {
            setTimeout(function () {
                CMS.clearTipsyError(self.documentElements.btnUnblocked);
                self.documentElements.btnUnblocked.removeAttr('original-title');
            }, 100);
        });
    };

    self.resetPagingOptions = function () {
        self.documentData.searchParameters.PageSize = self.documentData.defaultPageSize;
        self.documentData.gridOptions.pagerOption.itemsPerPage = self.documentData.defaultPageSize;
        self.documentData.searchParameters.PageIndex = 1;
        self.documentData.gridOptions.pagerOption.currentPage = 0;

        self.documentData.searchParameters.Username = self.documentElements.inputUsername.val();
        self.documentData.searchParameters.Game = self.documentElements.selectGame.val();
        self.documentData.searchParameters.Currency = self.documentElements.selectCurrency.val();
        self.documentData.searchParameters.Status = self.documentElements.selectStatus.val();
    };

    self.checkSelectedAccounts = function () {
        var isBlock = self.documentData.searchParameters.Status !== '1',
            blocksUnblocks, message;
        if (isBlock) {
            blocksUnblocks = $('.checkbox-blocked:checked');
            message = winLimitControlLabels.WinLimitMonitor_ChooseOneAccountToBlock;
        }
        else {
            blocksUnblocks = $('.checkbox-blocked:not(:checked)');
            message = winLimitControlLabels.WinLimitMonitor_ChooseOneAccountToUnblock;
        }

        if (blocksUnblocks.length === 0) {
            CMS.showTipsyError(self.documentElements.btnUnblocked, message, 'w');
            self.documentElements.btnUnblocked.unbind('click');
        }
        else {
            self.documentElements.btnUnblocked.unbind('click');
            self.documentElements.btnUnblocked.click(function () { blockUnblockAccounts(isBlock, blocksUnblocks); });
        }
    };

    function blockUnblockAccounts(isBlock, blocksUnblocks) {
        var message = isBlock ? winLimitControlLabels.WinLimitMonitor_ConfirmBlockAccounts : winLimitControlLabels.WinLimitMonitor_ConfirmUnblockAccounts;
        var okFunc = function () {
            var winlimitMonitor = [];
            blocksUnblocks.each(function () {
                var $this = $(this);
                var playerIndex = parseInt($this.attr('player-index'));
                var gameIndex = parseInt($this.attr('game-index'));
                var player = self.documentData.results.PlayerWinLimitMonitorModels[playerIndex];
                winlimitMonitor.push({
                    PlayerInfo: player.PlayerInfo,
                    Currency: player.Currency,
                    Game: player.GameWinLimitMonitors[gameIndex].Game
                });
            });

            var updateParams = { IsBlock: isBlock, WinLimitMonitor: winlimitMonitor };
            CMS.callAjax(self, window.rootUrl + 'WinLimitControl/WinLimitMonitor/UnblockAccounts', updateParams, self.renderUnblockResult);
        };

        CMS.showConfirmMessage(message, okFunc);
    }

    self.renderUnblockResult = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.documentElements.divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.documentElements.divResult, true, self.getPlayerSettings);
    };
};

winLimitMonitor = new WinLimitMonitor();