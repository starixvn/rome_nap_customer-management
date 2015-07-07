/* global winLimitControlLabels, hostPermission, keyboardManager, numeral, winlimitctrl,
    CMSConstants, CMS, AddCurrency, AddGame, EditWinLimitModal */
(function () {
    'use strict';
    var gridOptions = null;
    var freegrid = $('#freegrid');
    var formGlobalSetting = $('#form-globalsetting'),
        formAddCurrency = $('#form-addcurrency'),
        formAddGame = $('#form-addgame');
    var isEditting = false,
        isHost = hostPermission === 1;

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function initGridOptions() {
        var customButtons = [];
        if (isHost) {
            customButtons = [
                '<div class="custom-buttons-container" id="linkAddGame">',
                '<a href="javascript:;">',
                winLimitControlLabels.AddGame,
                '</a>',
                '<span class="ico-plus"></span>',
                '</div>',
                '<div class="custom-buttons-container" id="linkAddCurrency">',
                '<a href="javascript:;">',
                winLimitControlLabels.AddCurrency,
                '</a>',
                '<span class="ico-plus"></span>',
                '</div>'];
        }

        var columns = [
            { HeaderText: '#', Width: 60, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Currency, Width: 120, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Game, Width: 200, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.WinLimit, Width: 200, Name: 'colWinLimit', HeaderAlign: 'center', CellAlign: 'right' }
        ];

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
            columns: columns
        };
    }

    function buildBodyRows(result) {
        var bodyRows = [];
        var cell, globalWinLimit, winlimit;
        var mergeRowNumber = 0;

        var resultLength = result ? result.length : 0;
        for (var i = 0; i < resultLength; i++) {
            globalWinLimit = result[i];

            mergeRowNumber = globalWinLimit.WinLimits ? globalWinLimit.WinLimits.length : 0;
            if (mergeRowNumber === 0) {
                continue;
            }

            for (var j = 0; j < mergeRowNumber; j++) {
                cell = [];
                winlimit = globalWinLimit.WinLimits[j];

                if (j === 0) {
                    cell.push({ Name: 'colNumber', Value: (i + 1), MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colCurrency', Value: globalWinLimit.Currency, MergeRowNumber: mergeRowNumber });
                }

                cell.push({ Name: 'colGame', Value: winlimit.Game.Name });
                cell.push({ Name: 'colWinLimit', Value: renderWinLimitRow(winlimit, globalWinLimit.Currency) });

                bodyRows.push({ Columns: cell });
            }
        }

        return bodyRows;
    }

    function renderWinLimitRow(winlimit, currency) {
        if (isHost) {
            var html = [
                '<a href="javascript:;" class="linkEdit" data-currencyid="',
                winlimit.Currency.Id,
                '" data-currency="',
                currency,
                '" data-gameid="',
                winlimit.Game.Id,
                '" data-game="',
                winlimit.Game.Name,
                '">',
                formatNumber(winlimit.MaxWinLimit),
                '</a>'
            ];

            return html.join('');
        }

        return formatNumber(winlimit.MaxWinLimit);
    }

    function renderGlobalSettings() {
        isEditting = false;
        var url = formGlobalSetting.attr('action');
        url += '?' + formGlobalSetting.serialize();

        var successFunc = function (result) {
            gridOptions.bodyRows = buildBodyRows(result);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (isHost) {
                if (gridOptions.bodyRows.length > 0) {
                    $('.linkEdit').unbind('click').bind('click', function () { inlineEditWinLimit($(this)); });
                }

                bindCustomButtons();
            }
        };

        CMS.getJson(url, successFunc);
    }

    function bindCustomButtons() {
        if (isHost) {
            $('#linkAddCurrency').unbind('click').bind('click', function () {
                var addCurrencyModal = new AddCurrency(formAddCurrency, renderGlobalSettings);
                addCurrencyModal.initialize();
            });

            $('#linkAddGame').unbind('click').bind('click', function () {
                var addGameModal = new AddGame(formAddGame, renderGlobalSettings);
                addGameModal.initialize();
            });
        }
    }

    function inlineEditWinLimit(editLink) {
        if (isEditting) {
            return;
        }

        isEditting = true;
        var currency = { Id: editLink.data('currencyid'), Name: editLink.data('currency') };
        var game = { Id: editLink.data('gameid'), Name: editLink.data('game') };
        var winlimit = winlimitctrl.parseFloat(editLink.text().replace(/\,/g, ''));

        var tdContainer = editLink.parent()
            .html('<input type="text" maxlength="10" autocomplete="off" class="text-right width-160" id="inputWinLimit" value="' + winlimit + '"/>');

        tdContainer.find('#inputWinLimit')
            .unbind()
            .bind('keypress', function (e) { return CMS.forceNumberInput(e); })
            .bind('keyup', function (e) {
                var code = e.which || e.keyCode;
                if (code === keyboardManager.Enums.KeyCode.Enter) {
                    var $this = $(this);
                    var newWinLimit = winlimitctrl.parseFloat($this.val());
                    if (newWinLimit > 0) {
                        if (winlimit !== newWinLimit) {
                            var editConfirmation = new EditWinLimitModal(currency, game, newWinLimit, renderGlobalSettings);
                            editConfirmation.initialize();
                        }
                    }
                    else {
                        $this.val(winlimit);
                    }
                }
                else if (code === keyboardManager.Enums.KeyCode.Escape) {
                    tdContainer.html(renderWinLimitRow({ Currency: currency, Game: game, MaxWinLimit: winlimit }, currency.Name))
                        .find('.linkEdit')
                        .unbind('click')
                        .bind('click', function () { inlineEditWinLimit($(this)); });

                    isEditting = false;
                }
            }).select();
    }

    function initialize() {
        gridOptions = initGridOptions();

        formGlobalSetting.submit(function (e) {
            e.preventDefault();
            renderGlobalSettings();
        });

        $('#divModals').empty();
    }

    initialize();
})();