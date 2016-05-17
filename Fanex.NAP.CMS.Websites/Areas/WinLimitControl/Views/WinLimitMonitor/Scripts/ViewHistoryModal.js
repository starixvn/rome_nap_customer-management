/* global winLimitControlLabels, CMS, CMSConstants, numeral */
/* exported ViewHistoryModal */
function ViewHistoryModal(divContainer, closedPopupFunc) {
    'use strict';
    var self = this;
    var maxPercentageUsed = 100;
    var freegrid = divContainer.find('#freegrid-history > tbody');

    function formatNumber(number) {
        if (number === null) {
            return 'N/A';
        }

        return numeral(number).format(CMSConstants.NumberFormat.IntNumber);
    }

    function formatDecimal(number) {
    	if (number === null) {
    		return 'N/A';
    	}

    	return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function buildBodyRows(results) {
        var bodyRows = [];

        var resultsLength = results ? results.length : 0;
        if (resultsLength === 0) {
            bodyRows.push('<tr><td colspan="7" class="nodata">');
            bodyRows.push(winLimitControlLabels.NoData);
            bodyRows.push('</td></tr>');
        }
        else {
            var history;
            for (var i = 0, max = results.length; i < max; i++) {
                history = results[i];
                bodyRows.push(buildSpecificRow(history, i + 1));
            }
        }

        return bodyRows;
    }

    function buildSpecificRow(history, number) {
        var row = ['<tr><td class="align-right">'];
        row.push(number);
        row.push('</td>');

        row.push('<td class="align-center">');
        row.push(history.DateDisplay);
        row.push('</td>');

        row.push('<td class="align-right">');
        row.push(formatNumber(history.WinLimit));
        row.push('</td>');

        row.push('<td class="align-right">');
        row.push(formatDecimal(history.TotalWinningAmount));
        row.push('</td>');

        row.push('<td class="align-center">');
        row.push(getPercentageUsedBox(history.PercentageUsed));
        row.push('</td>');

        row.push('<td class="align-center">');
        row.push(history.Action);
        row.push('</td>');

        row.push('<td class="align-left">');
        row.push(history.ChangedByDisplay);
        row.push('</td></tr>');
        return row.join('');
    }

    function getPercentageUsedBox(percentageUsed) {
        var displayPercent = percentageUsed.toFixed(2);
        if (displayPercent >= maxPercentageUsed) {
            return '<div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + displayPercent + '%</div>';
        }

        return '<div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
            (maxPercentageUsed - displayPercent) + 'px">' + displayPercent + '%</div>';
    }

    self.initialize = function (player, game) {
        var url = window.rootUrl + 'WinLimitControl/WinLimitMonitor/ViewHistory?player=' + player.Id + '&game=' + game.Id;
        CMS.getJson(url, function (results) {
            var title = winLimitControlLabels.WinLimitHistoryTitle
                .replace('{0}', game.Name)
                .replace('{1}', player.Name);
            CMS.modal(title, divContainer, { resizable: false, close: closedPopupFunc });

            var tbodyHtml = buildBodyRows(results);
            freegrid.html(tbodyHtml.join(''));
        });
    };
}