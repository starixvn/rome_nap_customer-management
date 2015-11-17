/* global winLimitControlLabels, hostPermission, maxWinLimit, keyboardManager, numeral, 
CMSConstants, CMS, EditWinLimitModal */
(function () {
    'use strict';
    var gridOptions = null,
        freegrid = $('#freegrid');

    var formCurrencySetting = $('#form-currencysetting'),
        gameControl = formCurrencySetting.find('#game'),
        currencyControl = formCurrencySetting.find('#currency'),
        filterUrl = formCurrencySetting.attr('action') + '?';

    var selectedGame = { Id: '', Name: '' };
    var editConfirmation = null;

    var isEditting = false,
        isHost = hostPermission === 1;

    var searchData = {
        sortColumnName: 'colCurrency',
        sortDirection: 'asc'
    };

    function formatWinLimit(winlimit) {
        if (winlimit === 'N/A' || winlimit === -1) {
            return 'N/A';
        }

        return numeral(winlimit).format(CMSConstants.NumberFormat.IntNumber);
    }

    function getInt(value) {
        if (value === 'N/A' || isNaN(value)) {
            return 'N/A';
        }

        return parseInt(value);
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 60, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Currency, Width: 180, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center', Sort: true },
            { HeaderText: winLimitControlLabels.WinLimit, Width: 200, Name: 'colWinLimit', HeaderAlign: 'center', CellAlign: 'right', Sort: true }
        ];

        return {
            showTitle: true,
            titleInfo: { text: '' },
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 260,
            showNumberColumn: true,
            columns: columns,
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    searchData.sortColumnName = sortColumnName;
                    searchData.sortDirection = sortDirection;
                    renderCurrencySettings();
                }
            }
        };
    }

    function buildBodyRows(result) {
        var bodyRows = [];
        var winlimit;

        var resultLength = result ? result.length : 0;
        for (var i = 0; i < resultLength; i++) {
            winlimit = result[i];

            bodyRows.push({ Columns: [
                { Name: 'colNumber' },
                { Name: 'colCurrency', Value: winlimit.Currency.Name },
                { Name: 'colWinLimit', Value: renderWinLimitRow(winlimit.MaxWinLimit, winlimit.Currency)}]
            });
        }

        return bodyRows;
    }

    function renderWinLimitRow(winlimit, currency) {
        var formated = formatWinLimit(winlimit);
        if (!isHost) {
            return formated;
        }

        var html = [
            '<a href="javascript:;" class="linkEdit',
            winlimit === 0 ? ' text-red' : '',
            '" data-currencyid="',
            currency.Id,
            '" data-currency="',
            currency.Name,
            '">',
            formated,
            '</a>'
        ];

        return html.join('');
    }

    function renderCurrencySettings() {
        isEditting = false;

        selectedGame.Id = gameControl.val();
        selectedGame.Name = gameControl.find(':selected').text();

        searchData.game = selectedGame.Id;
        searchData.currency = currencyControl.val();
        var url = filterUrl + $.param(searchData);

        editConfirmation = new EditWinLimitModal(selectedGame, renderCurrencySettings);

        var successFunc = function (data) {
            gridOptions.bodyRows = buildBodyRows(data);
            gridOptions.titleInfo.text = selectedGame.Name + ' - ' + winLimitControlLabels.CurrencyWinLimitSetting;
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            if (isHost) {
                if (gridOptions.bodyRows.length > 0) {
                    $('.linkEdit').unbind('click').bind('click', function () { inlineEditWinLimit($(this)); });
                }
            }
        };

        CMS.getJson(url, successFunc);
    }

    function addCurrencySetting(game, currency, winlimit) {
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/Add';
        var formData = {
            Game: game,
            Currency: currency,
            MaxWinLimit: winlimit
        };

        CMS.callAjax(self, url, formData, function (result) {
            renderCurrencySettings();
            if (result.Code === 0) {
                CMS.showSuccessMessage(result.Message);
            }
            else {
                CMS.showErrorMessage(result.Message);
            }
        });
    }

    function inlineEditWinLimit(editLink) {
        if (isEditting) {
            return;
        }

        isEditting = true;
        var currency = { Id: editLink.data('currencyid'), Name: editLink.data('currency') };
        var winlimit = getInt(editLink.text().replace(/\,/g, ''));

        var tdContainer = editLink.parent()
            .html('<input type="text" maxlength="10" autocomplete="off" class="align-right width-160" id="inputWinLimit"' +
            ' onkeydown="return CMS.forceNumericInput(event);" value="' + winlimit + '"/>');

        tdContainer.find('#inputWinLimit')
            .unbind()
            .bind('keyup', function (e) {
                var code = e.which || e.keyCode;
                if (code === keyboardManager.Enums.KeyCode.Enter) {
                    var $this = $(this);
                    var newWinLimit = getInt($this.val());
                    if (newWinLimit >= 0 && newWinLimit <= maxWinLimit) {
                        if (winlimit !== newWinLimit) {
                            if (winlimit === 'N/A') {
                                addCurrencySetting(selectedGame, currency, newWinLimit);
                            }
                            else {
                                editConfirmation.initialize(currency, newWinLimit);
                            }
                        }
                    }
                    else {
                        $this.val(winlimit);
                    }
                }
                else if (code === keyboardManager.Enums.KeyCode.Escape) {
                    tdContainer.html(renderWinLimitRow(winlimit, currency))
                        .find('.linkEdit')
                        .unbind('click')
                        .bind('click', function () { inlineEditWinLimit($(this)); });

                    isEditting = false;
                }
            }).select();
    }

    function initialize() {
        gridOptions = initGridOptions();

        formCurrencySetting.submit(function (e) {
            e.preventDefault();
            renderCurrencySettings();
        });

        $('#divModals').empty();
    }

    initialize();
})();