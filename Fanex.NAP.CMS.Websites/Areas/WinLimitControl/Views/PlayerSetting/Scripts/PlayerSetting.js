/* global winLimitControlLabels, hostPermission, AddPlayerModal,
CMS, CMSConstants, keyboardManager,
numeral */
(function () {
    'use strict';
    var gridOptions = null,
        freegrid = $('#freegrid');

    var formFilter = $('#form-filterWinLimit'),
        formAddPlayerModal = $('#form-addplayer');
    var searchUrl = formFilter.attr('action') + '?',
        searchParams = {
            PageIndex: 0,
            PageSize: CMS.pageSize,
            SortColumnName: 'colUsername',
            SortDirection: 'asc'
        };

    var isPlayerWinLimitEditting = false,
        isHost = hostPermission === 1;

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function getFloat(value) {
        if (isNaN(value)) {
            return -1;
        }

        return parseFloat(value);
    }

    function afterChangedPage(pageIndex, itemsPerPage) {
        gridOptions.pagerOption.itemsPerPage = itemsPerPage;
        gridOptions.pagerOption.currentPage = pageIndex;

        searchParams.PageSize = itemsPerPage;
        searchParams.PageIndex = pageIndex;
        renderPlayerSettings();
    }

    function initGridOptions() {
        var customButtons = [];
        var columns = [
            { HeaderText: '#', Width: 50, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 200, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'center', Sort: true },
            { HeaderText: winLimitControlLabels.Currency, Width: 120, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Game, Width: 200, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.PlayerWinLimit, Width: 200, Name: 'colPlayerWinLimit', HeaderAlign: 'center', CellAlign: 'right', Sort: true },
            { HeaderText: winLimitControlLabels.CurrencyWinLimit, Width: 200, Name: 'colCurrencyWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', Sort: true
            }
        ];

        if (isHost) {
            customButtons = [
                '<div class="custom-buttons-container" id="linkAddPlayer">',
                '<a href="javascript:;">',
                winLimitControlLabels.AddPlayer,
                '</a>',
                '<span class="ico-plus"></span></div>'];

            columns.push({ HeaderText: winLimitControlLabels.Delete, Width: 60, Name: 'colDelete', HeaderAlign: 'center', CellAlign: 'center' });
        }

        return {
            showTitle: isHost,
            titleInfo: {
                customButtons: customButtons.join(''),
                showButtonsWhenNoData: true
            },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 240,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: searchParams.PageSize,
                currentPage: searchParams.PageIndex,
                totalItem: 0,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: afterChangedPage
            },
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    searchParams.SortColumnName = sortColumnName;
                    searchParams.SortDirection = sortDirection;
                    renderPlayerSettings();
                }
            }
        };
    }

    function buildBodyRows(results) {
        var bodyRows = [];
        var cell, playerWinLimit, winlimit;
        var mergeRowNumber = 0;

        var resultLength = results ? results.length : 0;
        for (var i = 0; i < resultLength; i++) {
            playerWinLimit = results[i];

            mergeRowNumber = playerWinLimit.WinLimits ? playerWinLimit.WinLimits.length : 0;
            if (mergeRowNumber === 0) {
                continue;
            }

            for (var j = 0; j < mergeRowNumber; j++) {
                cell = [];
                winlimit = playerWinLimit.WinLimits[j];

                if (j === 0) {
                    cell.push({ Name: 'colNumber', Value: (i + 1), MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colUsername', Value: playerWinLimit.Player.Name, MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colCurrency', Value: playerWinLimit.Currency, MergeRowNumber: mergeRowNumber });
                }

                cell.push({ Name: 'colGame', Value: winlimit.Game.Name });
                cell.push({
                    Name: 'colPlayerWinLimit',
                    Value: renderPlayerWinLimitLink(playerWinLimit.Player, winlimit.Game.Id, winlimit.PlayerWinLimit, winlimit.MaxWinLimit)
                });
                cell.push({ Name: 'colCurrencyWinLimit', Value: renderCurrencyWinLimitRow(winlimit.MaxWinLimit) });
                if (isHost) {
                    cell.push({ Name: 'colDelete', Value: renderDeleteLink(playerWinLimit.Player.Id, winlimit.Game.Id, true) });
                }

                bodyRows.push({ Columns: cell });
            }
        }

        return bodyRows;
    }

    function renderCurrencyWinLimitRow(winlimit) {
        var formatedWinLimit = formatNumber(winlimit);
        if (winlimit === 0) {
            return '<span class="text-red">' + formatedWinLimit + '</span>';
        }

        return formatedWinLimit;
    }

    function renderPlayerWinLimitLink(player, gameId, playerWinLimit, currencyWinLimit) {
        var html = [
            '<a href="javascript:;" class="editInlinePlayerWinLimit',
            playerWinLimit === 0 ? ' text-red' : '',
            '" data-playerid="',
            player.Id,
            '" data-player="',
            player.Name,
            '" data-game="',
            gameId,
            '" data-currencywinlimit="',
            currencyWinLimit,
            '">',
            formatNumber(playerWinLimit),
            '</a>'
        ];

        return html.join('');
    }

    function renderDeleteLink(playerId, gameId) {
        var html = [
            '<span class="ico-delete" data-playerid="',
            playerId,
            '" data-gameid="',
            gameId,
            '"></span>'
        ];

        return html.join('');
    }

    function renderPlayerSettings() {
        isPlayerWinLimitEditting = false;

        var successFunc = function (result) {
            gridOptions.pagerOption.totalItem = result.TotalRow;
            gridOptions.bodyRows = buildBodyRows(result.PlayerWinLimits);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (isHost) {
                if (gridOptions.bodyRows.length > 0) {
                    $('.editInlinePlayerWinLimit').unbind('click').bind('click', function () { editInlinePlayerWinLimit($(this)); });
                    $('.ico-delete').unbind('click').bind('click', function () { deleteWinLimit($(this)); });
                }

                bindCustomButtons();
            }
        };

        var params = formFilter.serialize() + '&' + $.param(searchParams);
        CMS.getJson(searchUrl + params, successFunc);
    }

    function editInlinePlayerWinLimit(editLink) {
        if (isPlayerWinLimitEditting) {
            return;
        }

        isPlayerWinLimitEditting = true;

        var player = { Id: editLink.data('playerid'), Name: editLink.data('player') },
            gameId = editLink.data('game'),
            playerWinlimit = getFloat(editLink.text().replace(/\,/g, '')),
            currencyWinlimit = getFloat(editLink.data('currencywinlimit'));

        var tdContainer = editLink.parent()
            .html('<input type="text" maxlength="10" autocomplete="off" class="align-right width-160" id="inputPlayerWinLimit"' +
            ' onkeydown="return CMS.forceNumericInput(event, true);" value="' + playerWinlimit + '"/>');

        tdContainer.find('#inputPlayerWinLimit')
            .unbind()
            .bind('keyup', function (e) {
                var code = e.which || e.keyCode;
                if (code === keyboardManager.Enums.KeyCode.Enter) {
                    updatePlayerWinLimit(player, gameId, playerWinlimit, currencyWinlimit, $(this));
                }
                else if (code === keyboardManager.Enums.KeyCode.Escape) {
                    tdContainer.html(renderPlayerWinLimitLink(player, gameId, playerWinlimit, currencyWinlimit))
                        .find('.editInlinePlayerWinLimit')
                        .unbind('click')
                        .bind('click', function () { editInlinePlayerWinLimit($(this)); });

                    isPlayerWinLimitEditting = false;
                }
            }).select();
    }

    function updatePlayerWinLimit(player, gameId, playerWinlimit, currencyWinlimit, inputPlayerWinlimit) {
        var newPlayerWinLimit = getFloat(inputPlayerWinlimit.val());
        if (newPlayerWinLimit >= 0) {
            if (playerWinlimit !== newPlayerWinLimit) {
                if (newPlayerWinLimit <= currencyWinlimit) {
                    var message = winLimitControlLabels.EditPlayerWinLimitConfirm.replace('{0}', player.Name);
                    var okFunction = function () {
                        var url = window.rootUrl + 'WinLimitControl/PlayerSetting/Update';
                        var formData = {
                            player: player.Id,
                            game: gameId,
                            playerWinLimit: newPlayerWinLimit
                        };

                        CMS.callAjax(this, url, formData, function (result) {
                            renderPlayerSettings();
                            if (result.Code === 0) {
                                CMS.showSuccessMessage(result.Message);
                            }
                            else {
                                CMS.showErrorMessage(result.Message);
                            }
                        });
                    };

                    CMS.showConfirmMessage(message, okFunction);
                }
                else {
                    CMS.showTipsyError(inputPlayerWinlimit, '<= ' + formatNumber(currencyWinlimit), 'w');
                }
            }
        }
        else {
            inputPlayerWinlimit.val(playerWinlimit);
        }
    }

    function deleteWinLimit(deleteElement) {
        var confirmOKFunc = function () {
            var url = window.rootUrl + 'WinLimitControl/PlayerSetting/Delete';
            var formData = {
                player: deleteElement.data('playerid'),
                game: deleteElement.data('gameid')
            };

            CMS.callAjax(this, url, formData, renderPlayerSettings);
        };

        CMS.showConfirmMessage(winLimitControlLabels.PlayerWinLimitConfirmDelete, confirmOKFunc);
    }

    function bindCustomButtons() {
        $('#linkAddPlayer').unbind('click').bind('click', function () {
            var addPlayerModal = new AddPlayerModal(formAddPlayerModal, renderPlayerSettings);
            addPlayerModal.initialize();
        });
    }

    function initialize() {
        gridOptions = initGridOptions();

        formFilter.submit(function (e) {
            e.preventDefault();

            searchParams.PageIndex = 0;
            searchParams.PageSize = CMS.pageSize;
            renderPlayerSettings();
        });

        $('#divModals').empty();

        $('#username').autocomplete({
            minLength: 3,
            delay: 300,
            source: window.rootUrl + 'WinLimitControl/PlayerSetting/AutoSuggestion'
        });
    }

    initialize();
})();