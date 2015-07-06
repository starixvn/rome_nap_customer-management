/* global winLimitControlLabels, hostPermission, CMS, AddPlayerModal, EditPlayerModal */
(function () {
    'use strict';
    var gridOptions = null;
    var freegrid = $('#freegrid');
    var formAddPlayerModal = $('#form-addplayer'),
        formEditlayerModal = $('#form-editplayer');
    var searchUrl = '',
        searchParams = { PageIndex: 0, PageSize: CMS.pageSize };
    var $username = $('#username');
    var isHost = hostPermission === 1;

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
            { HeaderText: '#', Width: 60, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 120, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Currency, Width: 120, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Game, Width: 200, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.WinLimit, Width: 200, Name: 'colWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat
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
            columns.push({ HeaderText: winLimitControlLabels.Edit, Width: 60, Name: 'colEdit', HeaderAlign: 'center', CellAlign: 'center' });
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
            gridExpandHeight: 180,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: searchParams.PageSize,
                currentPage: searchParams.PageIndex,
                totalItem: 0,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: afterChangedPage
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
                cell.push({ Name: 'colWinLimit', Value: winlimit.MaxWinLimit });
                if (isHost) {
                    cell.push({ Name: 'colDelete', Value: renderDeleteLink(playerWinLimit.Player.Id, winlimit.Game.Id, true) });

                    if (j === 0) {
                        cell.push({ Name: 'colEdit', Value: renderEditLink(playerWinLimit.Player), MergeRowNumber: mergeRowNumber });
                    }
                }

                bodyRows.push({ Columns: cell });
            }
        }

        return bodyRows;
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

    function renderEditLink(player) {
        var html = [
            '<span class="ico-edit" data-playerid="',
            player.Id,
            '" data-playername="',
            player.Name,
            '"></span>'
        ];

        return html.join('');
    }

    function renderPlayerSettings() {
        searchParams.Username = $username.val().trim();
        var url = searchUrl + $.param(searchParams);

        var successFunc = function (result) {
            gridOptions.pagerOption.totalItem = result.TotalRow;
            gridOptions.bodyRows = buildBodyRows(result.PlayerWinLimits);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (isHost) {
                if (gridOptions.bodyRows.length > 0) {
                    $('.ico-edit').unbind('click').bind('click', function () { editWinLimit($(this)); });
                    $('.ico-delete').unbind('click').bind('click', function () { deleteWinLimit($(this)); });
                }

                bindCustomButtons();
            }
        };

        CMS.getJson(url, successFunc);
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

    function editWinLimit(editElement) {
        var player = { Id: editElement.data('playerid'), Name: editElement.data('playername') };
        var editPlayerModal = new EditPlayerModal(formEditlayerModal, player, renderPlayerSettings);
        editPlayerModal.initialize();
    }

    function bindCustomButtons() {
        $('#linkAddPlayer').unbind('click').bind('click', function () {
            var addPlayerModal = new AddPlayerModal(formAddPlayerModal, renderPlayerSettings);
            addPlayerModal.initialize();
        });
    }

    function initialize() {
        gridOptions = initGridOptions();

        var formFilter = $('#form-filterWinLimit');
        formFilter.submit(function (e) {
            e.preventDefault();

            searchParams.PageIndex = 0;
            searchParams.PageSize = CMS.pageSize;
            renderPlayerSettings();
        });

        searchUrl = formFilter.attr('action') + '?';
        $('#divModals').empty();
    }

    initialize();
})();