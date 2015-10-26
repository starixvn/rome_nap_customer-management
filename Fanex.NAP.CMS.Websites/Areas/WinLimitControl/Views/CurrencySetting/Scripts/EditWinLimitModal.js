/* global winLimitControlLabels, CMS */
/* exported EditWinLimitModal */
function EditWinLimitModal(game, renderCurrencySettingFunc) {
    'use strict';
    var self = this;
    var gridOptions = null, freegrid = null;
    var formData = { gameId: game.Id, currencyId: 0, maxWinLimit: 0 }; 

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 45, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 160, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.CurrentWinLimit, Width: 140, Name: 'colCurrentWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat },
            { HeaderText: winLimitControlLabels.NewWinLimit, Width: 140, Name: 'colNewWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat }
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 400,
            columns: columns
        };
    }

    function buildBodyRows(results) {
        var bodyRows = [];

        var customer;
        for (var i = 0, max = results.length; i < max; i++) {
            customer = results[i];
            bodyRows.push({ Columns:
                [{ Name: 'colNumber', Value: (i + 1) },
                { Name: 'colUsername', Value: customer.Username },
                { Name: 'colCurrentWinLimit', Value: customer.CurrentWinLimit },
                { Name: 'colNewWinLimit', Value: formData.maxWinLimit}]
            });
        }

        return bodyRows;
    }

    function editWinLimit() {
        formData.GameName = game.Name;
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/Update';
        CMS.callAjax(self, url, formData, function (result) {
            renderCurrencySettingFunc();
            if (result.Code === 0) {
                CMS.showSuccessMessage(result.Message);
            }
            else {
                CMS.showErrorMessage(result.Message);
            }
        });
    }

    self.initialize = function (currency, newWinlimit) {
        formData.currencyId = currency.Id;
        formData.CurrencyName = currency.Name;
        formData.maxWinLimit = newWinlimit;
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/GetGreaterPlayerWinLimits?' + $.param(formData);
        CMS.getJson(url, function (results) {
            var message = winLimitControlLabels.EditWinLimitConfirm
                .replace('{0}', '<strong>' + currency.Name + '</strong>')
                .replace('{1}', '<strong>' + game.Name + '</strong>');

            if (results && results.length > 0) {
                gridOptions = initGridOptions();
                gridOptions.bodyRows = buildBodyRows(results);
                var html = [
                    winLimitControlLabels.EditWinLimitMessage
                        .replace('{0}', '<a href="javascript:;" id="linkEffectedCustomers">' + winLimitControlLabels.ViewDetails + '</a><br/>'),
                    message,
                    '<div class="freegrid" id="divEffectedCustomers"></div>'
                ];

                CMS.showConfirmMessage(html.join(''), function () { editWinLimit(); });
                $('#linkEffectedCustomers')
                    .unbind('click')
                    .bind('click', function () { freegrid.toggle(); });

                freegrid = $('#divEffectedCustomers')
                    .fanexGrid(gridOptions)
                    .hide();
            }
            else {
                CMS.showConfirmMessage(message, function () { editWinLimit(); });
            }
        });
    };
}