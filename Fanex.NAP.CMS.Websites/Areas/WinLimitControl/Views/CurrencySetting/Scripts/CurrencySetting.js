/* global winLimitControlLabels, hostPermission, keyboardManager, numeral, winlimitctrl,
    CMSConstants, CMS, EditWinLimitModal */
(function () {
    'use strict';
    var formCurrencySetting = $('#form-currencysetting');
    var filterUrl = formCurrencySetting.attr('action');
    var freegrid = $('#freegrid');
    var isEditting = false,
        isHost = hostPermission === 1;

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function renderCurrencySettings() {
        isEditting = false;
        var url = filterUrl + '?' + formCurrencySetting.serialize();

        var successFunc = function (data) {
            freegrid.html(data);
            if (isHost) {
                $('.linkEdit').unbind('click').bind('click', function () { inlineEditWinLimit($(this)); });
            }
        };

        CMS.getHtml(url, successFunc);
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
                            var editConfirmation = new EditWinLimitModal(currency, game, newWinLimit, renderCurrencySettings);
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
        formCurrencySetting.submit(function (e) {
            e.preventDefault();
            renderCurrencySettings();
        });

        $('#divModals').empty();
    }

    initialize();
})();