/* global winLimitControlLabels, numeral, CMS, CMSConstants */
/* exported EditPlayerModal */
/* jshint strict: false */
function EditPlayerModal(formContainer, player, renderPlayerSettingFunc) {
    'use strict';
    var self = this;
    var tblWinLimit = formContainer.find('.tblWinLimit > tbody'),
        messageContainer = formContainer.find('.inline-message');
    var playerWinLimit = null, formData = null;

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function showMessage(result) {
        if (result.Code !== 0) {
            CMS.showInlineErrorMsg(result.Message, messageContainer);
        }
        else {
            CMS.showInlineSuccessMsg(
                result.Message,
                messageContainer,
                true,
                function () {
                    CMS.closePopup();
                    renderPlayerSettingFunc();
                });
        }
    }

    function renderWinLimit(gameWinLimits) {
        var winlimit;
        var html = [];
        for (var i = 0, max = gameWinLimits.length; i < max; i++) {
            winlimit = gameWinLimits[i];

            html.push('<tr><td>');
            html.push(i + 1);
            html.push('</td>');

            html.push('<td>');
            html.push(winlimit.Name);
            html.push('</td>');

            html.push('<td><div class="edit-field"><input type="text" class="text-right width-160" autocomplete="off" maxlength="20" value="');
            html.push(formatNumber(winlimit.PlayerWinLimit));
            html.push('" onkeypress="return CMS.forceNumberInput(event)" data-global="');
            html.push(winlimit.GlobalWinLimit);
            html.push('" data-game="');
            html.push(winlimit.Id);
            html.push('"/></div></td>');

            html.push('<td class="text-right">');
            html.push(formatNumber(winlimit.GlobalWinLimit));
            html.push('</td></tr>');
        }

        tblWinLimit.html(html.join(''));
    }
    function setFormData() {
        formData = [];
        var isvalid = true;
        $.each(
            tblWinLimit.find('.edit-field > input[type="text"]'),
            function (index, value) {
                var inputWinLimit = $(value);
                var playerWinLimit = parseFloat(inputWinLimit.val().replace(/\,/g, ''));
                if (playerWinLimit >= 0) {
                    var globalWinLimit = parseFloat(inputWinLimit.data('global'));
                    if (playerWinLimit <= globalWinLimit) {
                        formData.push({ Id: inputWinLimit.data('game'), PlayerWinLimit: playerWinLimit });
                    }
                    else {
                        CMS.showTipsyError(inputWinLimit, '<= ' + formatNumber(globalWinLimit));
                        isvalid = false;
                    }
                }
                else {
                    CMS.showTipsyError(inputWinLimit, winLimitControlLabels.InvalidWinLimit);
                    isvalid = false;
                }
            });

        return isvalid;
    }

    function getPlayerWinLimit() {
        /*CMS.getJson(
        window.rootUrl + 'WinLimitControl/PlayerSetting/GetPlayerWinLimit?username=' + query,
        function (result) {
        playerWinLimit = result;
        renderWinLimit(playerWinLimit.WinLimits);
        });*/

        playerWinLimit = {
            Player: { Id: 1, Name: 'member01' },
            WinLimits: [
                { Id: 3, Name: 'Keno', PlayerWinLimit: 7000, GlobalWinLimit: 10000 },
                { Id: 5, Name: 'Virtual Sport', PlayerWinLimit: 8000, GlobalWinLimit: 20000 },
                { Id: 8, Name: 'Number Game', PlayerWinLimit: 9000, GlobalWinLimit: 30000 }
            ]
        };

        renderWinLimit(playerWinLimit.WinLimits);

        CMS.modal(winLimitControlLabels.EditPlayerModalTitle.replace('{0}', player.Name), formContainer);
        bindEvents();
    }

    function bindEvents() {
        formContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', CMS.closePopup);

        formContainer
            .unbind('submit')
            .bind('submit', function (e) {
                e.preventDefault();

                if (setFormData()) {
                    playerWinLimit.WinLimits = formData;
                    CMS.callAjax(
                        self,
                        formContainer.attr('action'),
                        playerWinLimit,
                        function (result) { showMessage(result); });
                }
            });
    }

    self.initialize = function () {
        getPlayerWinLimit();
    };
}