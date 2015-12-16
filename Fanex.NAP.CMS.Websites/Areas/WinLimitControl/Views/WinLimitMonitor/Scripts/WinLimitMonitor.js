/* global CMS, CMSConstants, winLimitControlLabels, ViewHistoryModal */
/* jshint strict: false,
camelcase: false */
var winLimitMonitor;
$(document).ready(function () {
    winLimitMonitor.initPage();
});

var WinLimitMonitor = function () {
    var self = this;
    var $formFilter = null,
        $username = null,
        $game = null,
        $currency = null,
        $status = null,
        $freegrid = null,
        $divButtons = null,
        $btnUnblocked = null,
        $divResult = null,
        $divNotesContainer = null,
        $liBlockedNote = null,
        $liUnblockedNote = null,
        $divViewHistoryContainer = null;

    var searchUrl = '',
        searchParameters = null,
        gridOptions = null,
        maxPercentageUsed = 100;

    var viewHistoryModal = null;
    var isSoundAlert = false;

    var timer = {
        id: 0,
        pausing: false,
        refresh: {
            intervals: [90, 120, 150, 180],
            selected: 90
        }
    };

    self.initPage = function () {
        cacheElements();
        bindEvents();
        gridOptions = initGridOptions();
        searchUrl = $formFilter.attr('action') + '?';
        $username.autocomplete({
            minLength: 3,
            delay: 300,
            source: window.rootUrl + 'WinLimitControl/PlayerSetting/AutoSuggestion'
        });

        if ($.cookie('mute-sound') == null) {
            $.cookie('mute-sound', 'false');
        }

        $('#divModals').empty();
        viewHistoryModal = new ViewHistoryModal($divViewHistoryContainer, function () { timer.pausing = false; });

        $formFilter.trigger('submit');
    };

    function cacheElements() {
        $formFilter = $('#form-filterWinLimit');
        $username = $('#username');
        $game = $('#game');
        $currency = $('#currency');
        $status = $('#status');
        $freegrid = $('#freegrid');
        $divButtons = $('#divButtons');
        $btnUnblocked = $('#btnUnblocked');
        $divResult = $('#div-result-message');
        $divNotesContainer = $('#divNotesContainer');
        $liBlockedNote = $divNotesContainer.find('.blockedNote');
        $liUnblockedNote = $divNotesContainer.find('.unblockedNote');
        $divViewHistoryContainer = $('#div-ViewHistory');
    }

    function isBlockedSearching() {
        return searchParameters.Status === '1';
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
            gridExpandHeight: 300,
            columns: [],
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            }
        };
    }

    function getPlayerSettings() {
        CMS.hideInlineMsg($divResult);
        var url = searchUrl + $.param(searchParameters);
        var successFunc = function (result) {
            var isBlocked = isBlockedSearching();

            gridOptions.pagerOption.currentPage = searchParameters.PageIndex;
            gridOptions.pagerOption.itemsPerPage = searchParameters.PageSize;
            gridOptions.pagerOption.totalItem = result.TotalRow;

            gridOptions.titleInfo.customButtons = renderAutoRefresh();
            gridOptions.columns =
                [
                    { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: winLimitControlLabels.Username, Width: 200, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: winLimitControlLabels.Currency, Width: 75, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: winLimitControlLabels.Game, Width: 200, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
                    { HeaderText: winLimitControlLabels.WinLimit, Width: 100, Name: 'colWinLimit', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.IntNumber },
                    { HeaderText: winLimitControlLabels.WinningAmount, Width: 100, Name: 'colTotalWinningAmount', HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.IntNumber },
                    { HeaderText: winLimitControlLabels.ReachedLimitTime, Width: 145, Name: 'colBlockedTime', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: winLimitControlLabels.PercentageUsed, Width: 110, Name: 'colPercentageUsed', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: '', Width: 40, Name: 'colViewHistory', HeaderAlign: 'center', CellAlign: 'center' }
                ];

            gridOptions.bodyRows = buildBodyRows(result.Winlimits, isBlocked);

            $freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (result.TotalRow > 0) {
                bindAutoRefreshEvents();
                bindViewHistory();

                $btnUnblocked.val(
                    isBlocked ?
                    winLimitControlLabels.Unblock_Accounts :
                    winLimitControlLabels.Block_Accounts
                );
                $divButtons.removeClass('display-none');
            }
            else {
                $divButtons.addClass('display-none');
            }

            $divNotesContainer.removeClass('display-none');
            if (isBlocked) {
                $liBlockedNote.removeClass('display-none');
                $liUnblockedNote.addClass('display-none');
            }
            else {
                $liUnblockedNote.removeClass('display-none');
                $liBlockedNote.addClass('display-none');
            }
        };

        CMS.getJson(url, successFunc);
    }

    function buildBodyRows(data, isBlocked) {
        var bodyRows = [];
        var startIndex = searchParameters.PageIndex * searchParameters.PageSize;

        var cell, winlimit, customData;
        isSoundAlert = false;
        for (var i = 0, max = data.length; i < max; i++) {
            winlimit = data[i];
            customData = ' data-playerid="' + winlimit.Player.Id + '"' + ' data-gameid="' + winlimit.Game.Id + '"';
            cell = [
                { Name: 'colNumber', Value: (startIndex + i + 1) },
                { Name: 'colUsername', Value: winlimit.Player.Name },
                { Name: 'colCurrency', Value: winlimit.Currency.Name },
                { Name: 'colGame', Value: winlimit.Game.Name },
                { Name: 'colWinLimit', Value: winlimit.WinLimit },
                { Name: 'colTotalWinningAmount', Value: winlimit.TotalWinningAmount },
                { Name: 'colBlockedTime', Value: winlimit.BlockedTimeDisplay },
                { Name: 'colPercentageUsed', Value: getPercentageUsedBox(winlimit.PercentageUsed) },
                { Name: 'colViewHistory', Value: renderViewHistoryRow(winlimit.Player.Name, winlimit.Game.Name, customData) }
            ];

            if (winlimit.IsNotified == true) {
                bodyRows.push({ Columns: cell });
            }
            else {
                bodyRows.push({ Columns: cell, CssClass: 'new-data-row' });
                if (winlimit.IsModified == 0) {
                    isSoundAlert = true;
                }
            }
        }

        return bodyRows;
    }

    function getPercentageUsedBox(percentageUsed) {
        var displayPercent = parseInt(percentageUsed);
        if (displayPercent >= maxPercentageUsed) {
            return '<div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + displayPercent + '%</div>';
        }

        return '<div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
            (maxPercentageUsed - displayPercent) + 'px">' + displayPercent + '%</div>';
    }

    function renderViewHistoryRow(playerName, gameName, customData) {
        var customAttr = [
            '<a href="javascript:;" class="ico-remark" ',
            customData,
            ' data-playername="',
            playerName,
            '" data-gamename="',
            gameName,
            '" title="',
            winLimitControlLabels.ViewHistory,
            '" ></a>'
        ];

        return customAttr.join('');
    }

    function handleChangingPage(pageIndex, itemsPerPage) {
        searchParameters.PageIndex = parseInt(pageIndex);
        searchParameters.PageSize = parseInt(itemsPerPage);
        getPlayerSettings();
    }

    function renderAutoRefresh() {
        var intervals = timer.refresh.intervals,
            selected = timer.refresh.selected;

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

                '</div>' +
                '<img id="sound" class="' + ($.cookie('mute-sound') == 'true' ? "sound-disabled" : "sound-enable") + '" alt="X" >';
    }

    function bindAutoRefreshEvents() {
        if (timer.id) {
            clearTimeout(timer.id);
        }

        var refreshContainer = $('#refresh-container');
        var countdownContainer = refreshContainer.find('.countdown');
        var dropdownContainer = refreshContainer.find('.dropdown-container');

        var autoRefreshFunc = function (countdownContainer) {
            timer.id = setTimeout(function () {
                if (!timer.pausing) {
                    var currentValue = parseInt(countdownContainer.text());
                    if (currentValue === 1) {
                        countdownContainer.html('...');
                        getPlayerSettings();
                        return;
                    }

                    countdownContainer.html(--currentValue);
                }

                autoRefreshFunc(countdownContainer);
            }, 1000);
        };

        countdownContainer.unbind('click').bind('click', getPlayerSettings);

        dropdownContainer.find('li')
            .unbind('click')
            .bind('click', function () {
                dropdownContainer.find('li.checked').removeClass('checked');

                var $this = $(this);
                $this.addClass('checked');
                timer.refresh.selected = parseInt($this.data('value'));
                countdownContainer.html(timer.refresh.selected);
            });

        autoRefreshFunc(countdownContainer);


        $('#sound').click(function () {

            $(this).toggleClass('sound-enable sound-disabled');
            var muteSound = $.cookie('mute-sound');
            if (muteSound == 'true')
                muteSound = 'false';
            else
                muteSound = 'true';

            $.cookie('mute-sound', muteSound);
        });

        alertDataChange();
    }

    function bindViewHistory() {
        $freegrid.find('.ico-remark')
            .unbind('click')
            .bind('click', function () {
                var $this = $(this);
                var player = { Id: $this.data('playerid'), Name: $this.data('playername') };
                var game = { Id: $this.data('gameid'), Name: $this.data('gamename') };

                timer.pausing = true;
                viewHistoryModal.initialize(player, game);
            });
    }

    function bindEvents() {
        $formFilter.submit(function (e) {
            e.preventDefault();
            searchParameters = getSearchParams();
            getPlayerSettings();
        });

        $btnUnblocked.hover(
        function () {
            checkSelectedAccounts();
        },
        function () {
            setTimeout(function () {
                CMS.clearTipsyError($btnUnblocked);
                $btnUnblocked.removeAttr('original-title');
            }, 100);
        });
    }

    function getSearchParams() {
        return {
            Username: $username.val().trim(),
            Game: $game.val(),
            Currency: $currency.val(),
            Status: $status.val(),
            PageIndex: 0,
            PageSize: CMS.pageSize
        };
    }

    function checkSelectedAccounts() {
        var isBlock = !isBlockedSearching();
        var blocksUnblocks, message;
        if (isBlock) {
            blocksUnblocks = $('.checkbox-blocked:checked');
            message = winLimitControlLabels.WinLimitMonitor_ChooseOneAccountToBlock;
        }
        else {
            blocksUnblocks = $('.checkbox-blocked:not(:checked)');
            message = winLimitControlLabels.WinLimitMonitor_ChooseOneAccountToUnblock;
        }

        if (blocksUnblocks.length === 0) {
            CMS.showTipsyError($btnUnblocked, message, 'w');
            $btnUnblocked.unbind('click');
        }
        else {
            $btnUnblocked.unbind('click');
            $btnUnblocked.click(function () { blockUnblockAccounts(isBlock, blocksUnblocks); });
        }
    }

    function blockUnblockAccounts(isBlock, blocksUnblocks) {
        var message = isBlock ? winLimitControlLabels.WinLimitMonitor_ConfirmBlockAccounts : winLimitControlLabels.WinLimitMonitor_ConfirmUnblockAccounts;
        var okFunc = function () {
            var accounts = [];
            blocksUnblocks.each(function () {
                var $this = $(this);
                accounts.push({ Player: $this.data('playerid'), Game: $this.data('gameid') });
            });

            var updateParams = { IsBlock: isBlock, Accounts: accounts };
            CMS.callAjax(self, window.rootUrl + 'WinLimitControl/WinLimitMonitor/BlockUnblockPlayers', updateParams, renderUnblockResult);
        };

        CMS.showConfirmMessage(message, okFunc);
    }

    function renderUnblockResult(result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, $divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, $divResult, true, getPlayerSettings);
    }

    function alertDataChange() {
        if (isSoundAlert == true) {
            CMS.showErrorMessage('Players have reached Company Win Limit. Please check again.');
            if ($.cookie('mute-sound') == 'false') {
                alertSound.play();
            }

            setTimeout(CMS.closeMsgBox, 120000);
        }
    }
};

winLimitMonitor = new WinLimitMonitor();