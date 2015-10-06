/* global CMS, CMSConstants, colossusBetsLabels, numeral */
/* exported ViewTicketDetailModal */
function ViewTicketDetailModal(ticketDetailContainer) {
    'use strict';
    var self = this;
    var ticketDetailOwn = ticketDetailContainer.find('#ticketDetailOwn'),
        ticketDetailSold = ticketDetailContainer.find('#ticketDetailSold'),
        tableTicketDetail = ticketDetailContainer.find('.tblColossusBets > tbody');
    var rootUrl = $('#CMSRoot').val();

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function renderTicketDetail(memberId, refno) {
        var successFunc = function (result) {
            var resultJson = $.parseJSON(result);
            ticketDetailOwn.html(resultJson.Own);
            ticketDetailSold.html(resultJson.Sold);

            var html = [];
            var ticketDetailsLength = resultJson.TicketDetails ? resultJson.TicketDetails.length : 0;
            if (ticketDetailsLength === 0) {
                tableTicketDetail.html('<tr><td colspan="4" class="text-red">' + colossusBetsLabels.NoData + '</td></tr>');
                return;
            }

            var ticketDetail;
            for (var i = 0; i < ticketDetailsLength; i++) {
                ticketDetail = resultJson.TicketDetails[i];

                html.push('<tr><td>');
                html.push(ticketDetail.Date);
                html.push('</td><td>');
                html.push(ticketDetail.TransactionType);
                html.push('</td><td class="text-right bold');
                if (ticketDetail.WinLoss < 0) {
                    html.push(' text-red');
                }

                html.push('">');
                html.push(formatNumber(ticketDetail.WinLoss));
                html.push('</td><td>');
                html.push(ticketDetail.Status);
                html.push('</td><tr>');
            }

            html.push('<tr><td colspan="2" class="text-right">');
            html.push(colossusBetsLabels.Total);
            html.push('</td><td colspan="2" class="bold">');
            html.push(formatNumber(resultJson.TotalWinLoss));
            html.push('</td></tr>');

            tableTicketDetail.html(html.join(''));
        };

        var url = rootUrl + 'ColossusBets/Payment/GetTicketDetail?memberId=' + memberId + '&refno=' + refno;
        CMS.getJson(url, successFunc);
    }

    self.initialize = function (memberId, refno) {
        CMS.modal(colossusBetsLabels.ViewTicketDetailTitle + ' ' + refno, ticketDetailContainer, { resizable: false });

        renderTicketDetail(memberId, refno);
        ticketDetailContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', CMS.closePopup);
    };
}