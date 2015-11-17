/* global winLimitControlLabels, numeral, CMS, CMSConstants */
/* exported AddPlayerModal */
function AddPlayerModal(formContainer, renderPlayerSettingFunc) {
    'use strict';
    var self = this;
    var username = formContainer.find('#txt-query'),
        tblWinLimitContainer = formContainer.find('#tblWinLimit-container'),
        tblWinLimit = formContainer.find('.tblWinLimit > tbody'),
        messageContainer = formContainer.find('.inline-message');
    var undefinedWinLimits = [], formData;

    function checkParseInt(value) {
        if (isNaN(value)) {
            return -1;
        }

        return parseInt(value);
    }

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.IntNumber);
    }

    function showWinLimitGrid(show) {
        if (show) {
            tblWinLimitContainer.removeClass('display-none');
        }
        else {
            tblWinLimitContainer.addClass('display-none');
        }
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

    function renderWinLimit(undefinedWinLimits) {
        var html = [];
        var winlimit;
        for (var i = 0, gamesLength = undefinedWinLimits.length; i < gamesLength; i++) {
            winlimit = undefinedWinLimits[i];

            html.push('<tr><td class="align-center">');
            html.push(i + 1);
            html.push('</td>');
            if (i === 0) {
                html.push('<td rowspan="');
                html.push(gamesLength);
                html.push('">');
                html.push(winlimit.Player.Name);
                html.push('</td>');

                html.push('<td class="align-center" rowspan="');
                html.push(gamesLength);
                html.push('">');
                html.push(winlimit.Currency);
                html.push('</td>');
            }

            html.push('<td>');
            html.push(winlimit.Game.Name);
            html.push('</td>');
            
            html.push('<td class="align-right">');
            html.push('<input type="text" maxlength="10" autocomplete="off" class="align-right width-160" value="N/A" ');
            html.push('data-game="');
            html.push(winlimit.Game.Id);
            html.push('" data-maxwinlimit="');
            html.push(winlimit.GlobalWinLimit);
            html.push('" onkeydown="return CMS.forceNumericInput(event);" /></td>');

            html.push('<td class="align-right">');
            html.push(winlimit.GlobalWinLimit === 0 ? '<span class="text-red">0.00</span>' : formatNumber(winlimit.GlobalWinLimit));
            html.push('</td></tr>');
        }

        tblWinLimit.append(html.join(''));
    }

    function setFormData() {
        if (!undefinedWinLimits || undefinedWinLimits.length === 0) {
            return false;
        }

        var isValidFormData = true;
        var playerSettings = [];
        $.each(tblWinLimit.find('td > input'), function (index, input) {
            var $input = $(input);
            var winlimit = $input.val();
            if (winlimit !== 'N/A') {
                var playerWinlimit = checkParseInt(winlimit);
                if (playerWinlimit >= 0) {
                    var maxWinlimit = checkParseInt($input.data('maxwinlimit'));
                    if (playerWinlimit <= maxWinlimit) {
                        playerSettings.push({
                            Game: $input.data('game'),
                            PlayerWinLimit: playerWinlimit,
                            GlobalWinLimit: maxWinlimit 
                        });
                    }
                    else {
                        isValidFormData = false;
                        CMS.showTipsyError($input, '<= ' + formatNumber(maxWinlimit), 'w');
                    }
                }
                else {
                    isValidFormData = false;
                    CMS.showTipsyError($input, winLimitControlLabels.InvalidWinLimit, 'w');
                }
            }
        });

        formData = isValidFormData ? { player: undefinedWinLimits[0].Player.Id, playerSettings: playerSettings } : undefined;
        return isValidFormData;
    }

    function resetForm() {
        formData = undefined;
        tblWinLimit.empty();
        CMS.clearTipsyError(username);
        CMS.hideInlineMsg(messageContainer);
    }

    function searchPlayer(custname) {
        if (!custname) {
            custname = username.val().trim();
        }

        if (custname.length === 0) {
            CMS.showTipsyError(username, winLimitControlLabels.RequiredFieldError, 'nw');
            showWinLimitGrid(false);
            return;
        }

        var successFunc = function (result) {
            if (result.Code) {
                if (result.Code === 2 || result.Code === 3) {
                    CMS.showTipsyError(username, result.Message, 'nw');
                }
                else if (result.Code === 4) {
                    showMessage(result);
                }

                return;
            }

            undefinedWinLimits = result;
            resetForm();
            renderWinLimit(undefinedWinLimits);
            showWinLimitGrid(true);
        };

        CMS.getJson(
            window.rootUrl + 'WinLimitControl/PlayerSetting/SearchPlayer?custname=' + encodeURIComponent(custname),
            function (result) { successFunc(result); });
    }

    function bindEvents() {
        formContainer
            .unbind('submit')
            .bind('submit', function (e) {
                e.preventDefault();
                searchPlayer(); 
            });
        
        formContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', CMS.closePopup);

        formContainer
            .find('#button-addplayer')
            .unbind('click')
            .bind('click', function () {
                if (setFormData() && formData && formData.playerSettings.length > 0) {
                    CMS.callAjax(
                        self,
                        window.rootUrl + 'WinLimitControl/PlayerSetting/Add',
                        formData,
                        function (result) { showMessage(result); });
                }
            });
    }

    self.initialize = function () {
        showWinLimitGrid(false);
        resetForm();
        CMS.modal(winLimitControlLabels.AddPlayerModalTitle, formContainer);

        bindEvents();

        username.autocomplete({
            minLength: 3,
            delay: 300,
            source: window.rootUrl + 'WinLimitControl/PlayerSetting/AutoSuggestion',
            select: function (event, ui) {
                searchPlayer(ui.item.value);
            }
        })
        .clearfield()
        .val('');
    };
}