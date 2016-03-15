/* global CMS, CMSConstants, numeral */
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

        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function buildBodyRows(results, l3mdata) {
        var bodyRows = [];

        var resultsLength = results ? results.MarginHistory.CustMargin.length : 0;
        if (resultsLength === 0) {
            bodyRows.push('<tr><td colspan="13" class="nodata">');
            bodyRows.push('No Data');
            bodyRows.push('</td></tr>');
        }
        else {

            var last3mData = l3mdata.split(',');
            for (var i = 0; i < 3; i++)
            {
                var row = ['<tr><td class="align-right"' + (i % 2 != 0 ? 'style="background-color:#ebf5fc' : '') + '">'];
                row.push(i == 0 ? "Turnover" : i == 1 ? "Winloss" : "Margin");
                row.push('</td>');

                row.push('<td class="align-right"' + (i % 2 != 0 ? 'style="background-color:#ebf5fc' : '') + '">');
                row.push(formatNumber(i == 0
                ? (results.MarginHistory.CustMargin[resultsLength - 1].Turnover + last3mData[0] + last3mData[3] + last3mData[6])
                : i == 1
                    ? (results.MarginHistory.CustMargin[resultsLength - 1].Winloss + last3mData[1] + last3mData[4] + last3mData[7])
                    : (results.MarginHistory.CustMargin[resultsLength - 1].Margin + last3mData[2] + last3mData[5] + last3mData[8])));
                row.push('</td>');

                for (var j = 0, max = resultsLength - 4; j < max; j++) {
                    var history = results.MarginHistory.CustMargin[j];
                    row.push('<td class="align-right"' + (i % 2 != 0 ? 'style="background-color:#ebf5fc' : '') + '">');
                    row.push(formatNumber(i == 0
                        ? history.Turnover
                        : i == 1 ? history.Winloss : history.Margin));
                    row.push('</td>');
                }

                // build from existed data
                for (var k = 0; k < 3; k++) {
                    row.push('<td class="align-right"' + (i % 2 != 0 ? 'style="background-color:#ebf5fc' : '') + '">');
                    row.push(formatNumber(i == 0
                        ? last3mData[k * 3]
                        : i == 1 ? last3mData[k * 3 + 1] : last3mData[k * 3 + 2]));
                    row.push('</td>');
                }

                bodyRows.push(row.join(''));
            }            
        }

        return bodyRows;
    }

    function buildTableHeader(headerList)
    {
        var header = "<tr> <th>&nbsp;</th> <th>AVG. Performance</th> ";
        for(var i = 0; i < headerList.length; i ++)
        {
            header += "<th>" + headerList[i] + "</th>";
        }

        header += "<th>&nbsp;</th></tr>";

        return header;
    }

    self.initialize = function (custid, username, sportid, l3mdata) {
        var url = window.rootUrl + 'VirtualSettingControl/VirtualSettingMonitor/ViewHistory?CustId=' + custid + '&Username=' + username + '&SportId=' + sportid;
        CMS.getJson(url, function (results) {
            var title = 'Customer Margin History: ' + username;
            CMS.modal(title, divContainer, { resizable: false, close: closedPopupFunc });

            var tbodyHtml = buildBodyRows(results, l3mdata);
            $('div#div-ViewHistory table.freetable-header').find("thead").html(buildTableHeader(results.MonthList));
            freegrid.html(tbodyHtml.join(''));
        });
    };
}