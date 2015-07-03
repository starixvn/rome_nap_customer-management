/* global winLimitControlLabels, numeral, CMS, CMSConstants */
/* exported EditPlayerModal */
/* jshint strict: false */
function EditPlayerModal(formContainer, player, renderPlayerSettingFunc) {
    'use strict';
    var self = this;
    var tblWinLimit = formContainer.find('.tblWinLimit > tbody'),
        messageContainer = formContainer.find('.inline-message');
    var playerWinLimit = { Player: player }, formData = null;

    function checkParseFloat(value) {
        if (isNaN(value)) {
            return -1;
        }

        return parseFloat(value);
    }

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
                var playerWinLimit = checkParseFloat(inputWinLimit.val().replace(/\,/g, ''));
                if (playerWinLimit >= 0) {
                    var globalWinLimit = checkParseFloat(inputWinLimit.data('global'));
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
        CMS.getJson(
            window.rootUrl + 'WinLimitControl/PlayerSetting/GetPlayerWinLimits?player=' + playerWinLimit.Player.Id,
            function (results) {
                playerWinLimit.WinLimits = results;
        renderWinLimit(playerWinLimit.WinLimits);

                CMS.modal(winLimitControlLabels.EditPlayerModalTitle.replace('{0}', player.Name), formContainer);
                bindEvents();
            }); 
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