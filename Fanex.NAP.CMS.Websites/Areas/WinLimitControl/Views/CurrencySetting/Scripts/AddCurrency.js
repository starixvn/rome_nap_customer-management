/* global winLimitControlLabels, CMS,
winlimitctrl */
/* exported AddCurrency */
function AddCurrency(formContainer, renderGlobalSettingFunc) {
    'use strict';
    var self = this;
    var currencyElement = formContainer.find('#ddlCurrency'),
        tblWinLimit = formContainer.find('.tblWinLimit > tbody');
    var identity = -1;
    var currencies = null, originalGames = null, formData = null;

    function diffOfGameAndFormData() {
        var diff = [];
        var needPush, gameId;
        for (var i = 0, gameLength = originalGames.length; i < gameLength; i++) {
            gameId = originalGames[i].Id;
            needPush = true;
            for (var j = 0, formDataLength = formData.length; j < formDataLength; j++) {
                if (gameId === formData[j].GameId) {
                    needPush = false;
                    break;
                }
            }

            if (needPush) {
                diff.push(originalGames[i]);
            }
        }

        return diff;
    }

    function renderWinLimit(games) {
        var html = [];
        html.push('<tr data-identity="' + identity + '">');
        html.push('<td><span class="spanNumber">1</span></td>');

        html.push('<td><select class="combobox-autocomplete width-140" id="ddlGame');
        html.push(identity);
        html.push('">');
        html.push(winlimitctrl.renderSelectHtml(games));
        html.push('</select></td>');

        html.push('<td><div class="edit-field"><input type="text" class="text-right width-160" autocomplete="off" maxlength="20" value="1000" ');
        html.push('onkeypress="return CMS.forceNumberInput(event)" id="inputWinLimit');
        html.push(identity);
        html.push('"/></div></td>');

        html.push('<td><span class="');
        html.push(identity === 0 ? 'ico-plus' : 'ico-delete');
        html.push('" id="delWinLimit');
        html.push(identity);
        html.push('" title="');
        html.push(winLimitControlLabels.AddMoreGame);
        html.push('"></span></td>');
        html.push('</tr>');
        tblWinLimit.append(html.join(''));

        $('#ddlGame' + identity).combobox();
        if (identity !== 0) {
            $('#delWinLimit' + identity).click(function () { removeGame($(this)); });
        }
    }

    function setFormData() {
        formData = [];
        var row, ddlGame, inputWinLimit;
        var dataIdentity, gameId, winlimit;
        
        var winLimitRows = tblWinLimit.find('tr');
        for (var i = 0, max = winLimitRows.length; i < max; i++) {
            row = $(winLimitRows[i]);
            dataIdentity = row.data('identity');
            inputWinLimit = row.find('#inputWinLimit' + dataIdentity);
            winlimit = parseFloat(inputWinLimit.val());
            if (winlimit > 0) {
                ddlGame = row.find('#ddlGame' + dataIdentity);
                ddlGame.combobox('disable');
                
                gameId = parseInt(ddlGame.val());
                formData.push({ GameId: gameId, GameName: ddlGame.find('option:selected').text(), MaxWinLimit: winlimit });
                continue;
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
                winlimitctrl.renderNumber(tblWinLimit);
            }
        }
    }

    function removeGame(source) {
        var parentRow = source.parent().parent();
        var dataIdentity = parentRow.data('identity');
        var gameId = parseInt($('#ddlGame' + dataIdentity).val());
        for (var i = 0, max = formData.length; i < max; i++) {
            if (formData[i].GameId === gameId) {
                formData.splice(i, 1);
                break;
            }
        }

        parentRow.remove();
        winlimitctrl.renderNumber(tblWinLimit);

        if (formData.length === 1) {
            var row = tblWinLimit.find('tr:first');
            row.find('#ddlGame' + row.data('identity')).combobox('enable');
        }
    }

    function resetForm() {
        identity = 0;
        formData = [];
        
        tblWinLimit.empty();
        renderWinLimit(originalGames);
    }

    function getExcludedCurrencies(currency) {
        var successFunc = null;
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/GetExcludedCurrencies';
        if (currency) {
            url += '?currency=' + currency;
            successFunc = function (result) {
                originalGames = result.Games || [];

                resetForm();
                tblWinLimit
                    .find('#delWinLimit0')
                    .unbind('click')
                    .bind('click', function () { addGame(); });
            };
        }
        else {
            successFunc = function (result) {
                currencies = result.Currencies || [];
                currencyElement.html(winlimitctrl.renderSelectHtml(currencies));

                originalGames = result.Games || [];
                resetForm();

                CMS.modal(winLimitControlLabels.AddCurrencyModalTitle, formContainer);
                initAutoCompleteAndBindEvents();
            };
        }

        CMS.getJson(url, successFunc);    
    }

    function initAutoCompleteAndBindEvents() {
        tblWinLimit.find('.combobox-autocomplete').each(function () {
            $(this).combobox();
        });
        tblWinLimit
            .find('#delWinLimit0')
            .unbind('click')
            .bind('click', function () { addGame(); });

        currencyElement
            .combobox()
            .unbind('change')
            .bind('change', function () { getExcludedCurrencies(currencyElement.val()); });

        formContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', function () { CMS.closePopup(); });

        formContainer
            .unbind('submit')
            .bind('submit', function (e) {
                e.preventDefault();
                if (setFormData()) {
                    var currency = { Id: currencyElement.val(), Name: currencyElement.find('option:selected').text() };
                    var criteria = { currency: currency, globalWinLimits: formData };
                    CMS.callAjax(
                        self,
                        formContainer.attr('action'),
                        criteria,
                        function (results) { winlimitctrl.showAddedResults(formContainer, results, renderGlobalSettingFunc); });
                }
            });
    }

    self.initialize = function () {
        getExcludedCurrencies();
    };
}