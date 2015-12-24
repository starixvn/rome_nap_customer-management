/* global winLimitControlLabels, CMS, CMSConstants */
/* exported EditWinLimitModal */
function EditWinLimitModal(game, renderCurrencySettingFunc) {
    'use strict';
    var self = this;
    var gridOptions = null;
    var formData = { Game: game, Currency: {}, MaxWinLimit: 0 }; 

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 45, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 160, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.CurrentWinLimit, Width: 140, Name: 'colCurrentWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.IntNumber
            },
            { HeaderText: winLimitControlLabels.NewWinLimit, Width: 140, Name: 'colNewWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMSConstants.NumberFormat.IntNumber
            }
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
                { Name: 'colNewWinLimit', Value: formData.MaxWinLimit}]
            });
        }

        return bodyRows;
    }

    function editWinLimit() {
        var url = window.rootUrl + 'WinLimitControl/CurrencySetting/Update';
        CMS.callAjax(self, url, formData, function (result) {
            if (result.Code === 0) {
                CMS.showSuccessMessage(result.Message);
            }
            else {
                CMS.showErrorMessage(result.Message);
            }
            setTimeout(function () { renderCurrencySettingFunc(); }, 3000);
        });
    }

    self.initialize = function (currency, newWinlimit) {
        formData.Currency = currency;
        formData.MaxWinLimit = newWinlimit;

        var queryParams = [
            window.rootUrl,
            'WinLimitControl/CurrencySetting/GetGreaterPlayerWinLimits?game=',
            game.Id,
            '&currency=',
            currency.Id,
            '&maxWinLimit=',
            newWinlimit
        ];
        var url = queryParams.join('');
        CMS.getJson(url, function (results) {
            var message = winLimitControlLabels.EditWinLimitConfirm
                .replace('{0}', '<strong>' + currency.Name + '</strong>')
                .replace('{1}', '<strong>' + game.Name + '</strong>');

            if (results && results.length > 0) {
                gridOptions = gridOptions || initGridOptions();
                gridOptions.bodyRows = buildBodyRows(results);
                var html = [
                    winLimitControlLabels.EditWinLimitMessage
                        .replace('{0}', '<a href="javascript:;" id="linkEffectedCustomers">' + winLimitControlLabels.ViewDetails + '</a><br/>'),
                    message,
                    '<div class="freegrid" id="divEffectedCustomers"></div>'
                ];

                CMS.showConfirmMessage(html.join(''), function () { editWinLimit(); });

                var freegrid = $('#divEffectedCustomers').fanexGrid(gridOptions).hide();
                $('#linkEffectedCustomers')
                    .unbind('click')
                    .bind('click', function () { freegrid.toggle(); });
            }
            else {
                CMS.showConfirmMessage(message, function () { editWinLimit(); });
            }
        });
    };
}