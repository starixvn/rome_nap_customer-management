/* global winLimitControlLabels, numeral, CMS, CMSConstants */
/* exported AddPlayerModal */
function AddPlayerModal(formContainer, renderPlayerSettingFunc) {
    'use strict';
    var self = this;
    var username = formContainer.find('#txt-query'),
        tblWinLimitContainer = formContainer.find('#tblWinLimit-container'),
        tblWinLimit = formContainer.find('.tblWinLimit > tbody'),
        messageContainer = formContainer.find('.inline-message');
    var identity = -1;
    var playerWinLimit = {}, formData = null;

    function checkParseFloat(value) {
        if (isNaN(value)) {
            return -1;
        }

        return parseFloat(value);
    }

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
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

    function diffOfGameAndFormData() {
        var diff = [];
        var needPush, gameId;
        for (var i = 0, gameLength = playerWinLimit.WinLimits.length; i < gameLength; i++) {
            gameId = playerWinLimit.WinLimits[i].Id;
            needPush = true;
            for (var j = 0, formDataLength = formData.length; j < formDataLength; j++) {
                if (gameId === formData[j].Id) {
                    needPush = false;
                    break;
                }
            }

            if (needPush) {
                diff.push(playerWinLimit.WinLimits[i]);
            }
        }

        return diff;
    }

    function renderGameSelect (games) {
        var item, html = [];
        for (var i = 0, max = games.length; i < max; i++) {
            item = games[i];
            html.push('<option value="');
            html.push(item.Id);
            html.push('" data-global="');
            html.push(item.GlobalWinLimit);
            html.push('">');
            html.push(item.Name);
            html.push('</option>');
        }

        return html.join('');
    }

    function renderNumber() {
        var spanNumbers = tblWinLimit.find('.spanNumber');
        $.each(spanNumbers, function (index, value) {
            value.innerHTML = index + 1;
        });
    }

    function renderWinLimit(games) {
        if (games.length === 0) {
            tblWinLimit.html('<tr><td colspan="5">' + winLimitControlLabels.NoMoreGameToSetPlayerError + '</td></tr>');
            return;
        }

        var gameWinLimit = games[0];
        var html = [];
        html.push('<tr data-identity="' + identity + '">');
        html.push('<td><span class="spanNumber">1</span></td>');

        html.push('<td><select class="combobox-autocomplete width-140" id="ddlGame');
        html.push(identity);
        html.push('">');
        html.push(renderGameSelect(games));
        html.push('</select></td>');

        html.push('<td><div class="edit-field"><input type="text" class="text-right width-160" autocomplete="off" maxlength="10" value="');
        html.push(gameWinLimit.PlayerWinLimit);
        html.push('" onkeypress="return CMS.forceNumberInput(event)" id="inputWinLimit');
        html.push(identity);
        html.push('"/></div></td>');

        html.push('<td><div class="text-right" id="globalWinLimit');
        html.push(identity);
        html.push('">');
        html.push(formatNumber(gameWinLimit.GlobalWinLimit));
        html.push('</div></td>');

        html.push('<td><span class="');
        html.push(identity === 0 ? 'ico-plus' : 'ico-delete');
        html.push('" id="delWinLimit');
        html.push(identity);
        html.push('" title="');
        html.push(winLimitControlLabels.AddMoreGame);
        html.push('"></span></td>');
        html.push('</tr>');
        tblWinLimit.append(html.join(''));

        $('#ddlGame' + identity)
            .combobox()
            .unbind('change')
            .bind('change', function () {
                var $this = $(this);
                var identity = $this.parent().parent().data('identity');
                var global = $this.find('option:selected').data('global');
                $('#globalWinLimit' + identity).html(formatNumber(global));
            });

        if (identity === 0) {
            $('#delWinLimit' + identity).unbind('click').bind('click', addGame);
        }
        else {
            $('#delWinLimit' + identity).unbind('click').bind('click', function () { removeGame($(this)); });
        }
    }

    function duplicatedSelectValue(selects) {
        var uniques = {};

        var $select;
        var selectValue;
        for (var i = 0, max = selects.length; i < max; i++) {
            $select = $(selects[i]);
            selectValue = $select.val();
            if (!uniques[selectValue]) {
                uniques[selectValue] = true;
            }
            else {
                CMS.showTipsyError($select.next(), winLimitControlLabels.DuplicateGameError);
                return true;
            }
        }

        return false;
    }

    function setFormData() {
        if (duplicatedSelectValue(tblWinLimit.find('td > select'))) {
            return false;
        }

        formData = [];
        var row, ddlGame, selectedGame, inputWinLimit;
        var dataIdentity, gameId, winlimit;

        var winLimitRows = tblWinLimit.find('tr');
        for (var i = 0, max = winLimitRows.length; i < max; i++) {
            row = $(winLimitRows[i]);
            dataIdentity = row.data('identity');
            inputWinLimit = row.find('#inputWinLimit' + dataIdentity);
            winlimit = checkParseFloat(inputWinLimit.val());
            if (winlimit >= 0) {
                ddlGame = row.find('#ddlGame' + dataIdentity);
                selectedGame = ddlGame.find('option:selected');

                var globalWinLimit = checkParseFloat(selectedGame.data('global'));
                if (winlimit <= globalWinLimit) {
                    gameId = parseInt(ddlGame.val());
                    formData.push({ Id: gameId, Name: selectedGame.text(), PlayerWinLimit: winlimit });
                    continue;
                }

                CMS.showTipsyError(inputWinLimit, '<= ' + formatNumber(globalWinLimit));
                return false;
            }

            CMS.showTipsyError(inputWinLimit, winLimitControlLabels.InvalidWinLimit);
            return false;
        }

        return true;
    }

    function addGame() {
        if (setFormData()) {
            var diffGames = diffOfGameAndFormData();
            if (diffGames.length !== 0) {
                identity++;
                renderWinLimit(diffGames);
                renderNumber();
            }
        }
    }

    function removeGame(source) {
        var parentRow = source.parent().parent();
        var dataIdentity = parentRow.data('identity');
        var gameId = parseInt($('#ddlGame' + dataIdentity).val());
        for (var i = 0, max = formData.length; i < max; i++) {
            if (formData[i].Id === gameId) {
                formData.splice(i, 1);
                break;
            }
        }

        parentRow.remove();
        renderNumber();
    }

    function resetForm() {
        identity = 0;
        formData = [];
        tblWinLimit.empty();
        CMS.clearTipsyError(username);
        CMS.hideInlineMsg(messageContainer);
    }

    function searchPlayer(e) {
        e.preventDefault();
        
        var query = username.val().trim();
        if (query.length === 0) {
            CMS.showTipsyError(username, 'Required field!');
            showWinLimitGrid(false);
            return;
        }

        CMS.getJson(
            window.rootUrl + 'WinLimitControl/PlayerSetting/SearchPlayer?username=' + query,
            function (result) {
                playerWinLimit = result;
                if (playerWinLimit.DbResult.Code === 3) {
                    CMS.showTipsyError(username, playerWinLimit.DbResult.Message);
                    showWinLimitGrid(false);
                }
                else if (playerWinLimit.DbResult.Code === 4) {
                    showMessage(playerWinLimit.DbResult);
                    showWinLimitGrid(false);
                }
                else {
                    resetForm();
                    renderWinLimit(playerWinLimit.WinLimits);
                    showWinLimitGrid(true);
                }
            });
    }

    function bindEvents() {
        formContainer
            .unbind('submit')
            .bind('submit', searchPlayer);
        
        formContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', CMS.closePopup);

        formContainer
            .find('#button-addplayer')
            .unbind('click')
            .bind('click', function () {
                if (!playerWinLimit.DbResult || playerWinLimit.DbResult.Code !== 0 || playerWinLimit.WinLimits.length === 0) {
                    return;
                }

                if (setFormData()) {
                    playerWinLimit.WinLimits = formData;
                    CMS.callAjax(
                        self,
                        window.rootUrl + 'WinLimitControl/PlayerSetting/Add',
                        playerWinLimit,
                        function (result) { showMessage(result); });
                }
            });
    }

    self.initialize = function () {
        showWinLimitGrid(false);

        resetForm();
        username.val('');

        CMS.modal(winLimitControlLabels.AddPlayerModalTitle, formContainer);
        bindEvents();
    };
}