/* global winLimitControlLabels, CMS,
winlimitctrl */
/* exported AddGame */
function AddGame(formContainer, renderGlobalSettingFunc) {
    'use strict';
    var self = this;
    var gameElement = formContainer.find('#ddlGame'),
        tblWinLimit = formContainer.find('.tblWinLimit > tbody');
    var identity = -1;
    var games = null, originalCurrencies = null, formData = null;

    function diffOfCurrencyAndFormData() {
        var diff = [];
        var needPush, currencyId;
        for (var i = 0, max = originalCurrencies.length; i < max; i++) {
            currencyId = originalCurrencies[i].Id;
            needPush = true;
            for (var j = 0, formDataLength = formData.length; j < formDataLength; j++) {
                if (currencyId === formData[j].CurrencyId) {
                    needPush = false;
                    break;
                }
            }

            if (needPush) {
                diff.push(originalCurrencies[i]);
            }
        }

        return diff;
    }

    function renderWinLimit(currencies) {
        if (currencies.length === 0) {
            tblWinLimit.append('<tr style="width:200px;"><td colspan="4">' + winLimitControlLabels.NoMoreGameToSetWinLimitError + '</td></tr>');
            return;
        }

        var html = [];
        html.push('<tr data-identity="' + identity + '">');
        html.push('<td><span class="spanNumber">1</span></td>');

        html.push('<td><select class="combobox-autocomplete width-140" id="ddlCurrency');
        html.push(identity);
        html.push('">');
        html.push(winlimitctrl.renderSelectHtml(currencies));
        html.push('</select></td>');

        html.push('<td><div class="edit-field"><input type="text" class="text-right width-160" autocomplete="off" maxlength="10" value="1000" ');
        html.push('onkeypress="return CMS.forceNumberInput(event)" id="inputWinLimit');
        html.push(identity);
        html.push('"/></div></td>');

        html.push('<td><span class="');
        html.push(identity === 0 ? 'ico-plus' : 'ico-delete');
        html.push('" id="delWinLimit');
        html.push(identity);
        html.push('" title="');
        html.push(winLimitControlLabels.AddMoreCurrency);
        html.push('"></span></td>');
        html.push('</tr>');
        tblWinLimit.append(html.join(''));

        $('#ddlCurrency' + identity).combobox();
        if (identity !== 0) {
            $('#delWinLimit' + identity).click(function () { removeCurrency($(this)); });
        }
    }

    function setFormData() {
        // Game combobox is empty
        if (!gameElement.combobox('validInput')) {
            return false;
        }

        if (winlimitctrl.duplicatedSelectValue(tblWinLimit.find('td > select'), winLimitControlLabels.DuplicateCurrencyError)) {
            return false;
        }

        formData = [];
        var row, ddlCurrency, inputWinLimit;
        var dataIdentity, currencyId, winlimit;

        var winLimitRows = tblWinLimit.find('tr');
        for (var i = 0, max = winLimitRows.length; i < max; i++) {
            row = $(winLimitRows[i]);
            dataIdentity = row.data('identity');
            inputWinLimit = row.find('#inputWinLimit' + dataIdentity);
            winlimit = winlimitctrl.parseFloat(inputWinLimit.val());
            if (winlimit > 0) {
                ddlCurrency = row.find('#ddlCurrency' + dataIdentity);
                // Currency combobox is empty
                if (!ddlCurrency.combobox('validInput')) {
                    return false;
                }

                currencyId = parseInt(ddlCurrency.val());
                formData.push({ CurrencyId: currencyId, CurrencyName: ddlCurrency.find('option:selected').text(), MaxWinLimit: winlimit });
                continue;
            }

            CMS.showTipsyError(inputWinLimit, winLimitControlLabels.InvalidWinLimit, 'se');
            return false;
        }

        return true;
    }

    function addCurrency() {
        if (setFormData()) {
            var diffCurrencies = diffOfCurrencyAndFormData();
            if (diffCurrencies.length !== 0) {
                identity++;
                renderWinLimit(diffCurrencies);
                winlimitctrl.renderNumber(tblWinLimit);
            }
        }
    }

    function removeCurrency(source) {
        var parentRow = source.parent().parent();
        var dataIdentity = parentRow.data('identity');
        var currencyId = parseInt($('#ddlCurrency' + dataIdentity).val());
        for (var i = 0, max = formData.length; i < max; i++) {
            if (formData[i].CurrencyId === currencyId) {
                formData.splice(i, 1);
                break;
            }
        }

        parentRow.remove();
        winlimitctrl.renderNumber(tblWinLimit);
    }

    function resetForm() {
        identity = 0;
        formData = [];

        tblWinLimit.empty();
        renderWinLimit(originalCurrencies);
    }

    function getExcludedGames(game) {
        var successFunc = null;
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/GetExcludedGames';
        if (game) {
            url += '?game=' + game;
            successFunc = function (result) {
                originalCurrencies = result.Currencies || [];

                resetForm();
                tblWinLimit
                    .find('#delWinLimit0')
                    .unbind('click')
                    .bind('click', function () { addCurrency(); });
            };
        }
        else {
            successFunc = function (result) {
                games = result.Games || [];
                gameElement.html(winlimitctrl.renderSelectHtml(games));

                originalCurrencies = result.Currencies || [];
                resetForm();

                CMS.modal(winLimitControlLabels.AddGameModalTitle, formContainer);
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
            .bind('click', function () { addCurrency(); });

        gameElement
            .combobox()
            .unbind('change')
            .bind('change', function () { getExcludedGames(gameElement.val()); });

        formContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', function () { CMS.closePopup(); });

        formContainer
            .unbind('submit')
            .bind('submit', function (e) {
                e.preventDefault();

                var gameId = gameElement.val();
                if (!gameId) {
                    return;
                }

                if (setFormData()) {
                    var game = { Id: gameId, Name: gameElement.find('option:selected').text() };
                    var criteria = { game: game, globalWinLimits: formData };
                    CMS.callAjax(
                        self,
                        formContainer.attr('action'),
                        criteria,
                        function (results) { winlimitctrl.showAddedResults(formContainer, results, renderGlobalSettingFunc); });
                 }
            });
    }

    self.initialize = function () {
        getExcludedGames();
    };
}