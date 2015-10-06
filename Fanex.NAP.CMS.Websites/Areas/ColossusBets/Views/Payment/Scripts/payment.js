/* global colossusBetsLabels, CMS, CMSConstants, numeral,
updateStatus, showSecurityCode,
UpdateTicketStatusModal, ViewTicketNotesModal, ViewTicketDetailModal */
(function () {
    'use strict';
    var gridOptions = null;
    var freegrid = $('#freegrid');
    var formFilter = $('#form-filterTicket'),
        ticketDetailContainer = $('#ticketDetailContainer'),
        updateTicketStatusForm = $('#form-updateTicketStatus'),
        ticketNotesContainer = $('#ticketNotesContainer');
    var searchUrl = formFilter.attr('action') + '?';

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Member, Width: 150, Name: 'colMember', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.Currency, Width: 100, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Site, Width: 120, Name: 'colSite', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.BetId, Width: 120, Name: 'colBetId', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.TotalPayout, Width: 120, Name: 'colTotalPayout', HeaderAlign: 'center', CellAlign: 'right' },
            { HeaderText: colossusBetsLabels.Status, Width: 100, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'left' }
        ];

        if (showSecurityCode) {
            columns.push({ HeaderText: colossusBetsLabels.SecurityCode, Width: 120, Name: 'colSecurityCode', HeaderAlign: 'center', CellAlign: 'left' });
        }

        columns.push({ HeaderText: colossusBetsLabels.Notes, Width: 50, Name: 'colNotes', HeaderAlign: 'center', CellAlign: 'center' });

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 150,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(results) {
        var bodyRows = [];
        var cell, ticket;
        var resultLength = results ? results.length : 0;
        for (var i = 0; i < resultLength; i++) {
            ticket = results[i];
            cell = [
                { Name: 'colNumber', Value: (i + 1) },
                { Name: 'colMember', Value: ticket.Member.Name },
                { Name: 'colCurrency', Value: ticket.Currency.Name },
                { Name: 'colSite', Value: ticket.Site.Name },
                { Name: 'colBetId', Value: ticket.BetId },
                { Name: 'colTotalPayout', Value: renderTotalPayout(ticket.Member.Id, ticket.BetId, ticket.TotalPayout) },
                { Name: 'colStatus', Value: updateStatus ? renderStatus(ticket.Id, ticket.BetId, ticket.Status) : ticket.Status }
            ];

            if (showSecurityCode) {
                cell.push({ Name: 'colSecurityCode', Value: ticket.SecurityCode });
            }

            cell.push({ Name: 'colNotes', Value: renderNotes(ticket.Id, ticket.BetId) });
            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function renderTotalPayout(memberId, betId, totalPayout) {
        var html = [
            '<span class="link totalPayout" data-member="',
            memberId,
            '" data-betid="',
            betId,
            '">',
            formatNumber(totalPayout),
            '</span>'
        ];

        return html.join('');
    }

    function renderStatus(ticketId, betId, status) {
        var html = [
            '<div class="link ticketStatus" data-ticket="',
            ticketId,
            '" data-betid="',
            betId,
            '">',
            status,
            '</div>'
        ];

        return html.join('');
    }

    function renderNotes(ticketId, betId) {
        var html = [
            '<span class="ico-remark" data-ticket="',
            ticketId,
            '" data-betid="',
            betId,
            '">',
            '</span>'
        ];

        return html.join('');
    }

    function renderPaymentList() {
        var successFunc = function (results) {
            var resultsJson = $.parseJSON(results);
            gridOptions.bodyRows = buildBodyRows(resultsJson);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            bindEvents();
        };

        var params = formFilter.serialize();
        CMS.getJson(searchUrl + params, successFunc);
    }

    function bindEvents() {
        var viewTicketDetailModal = new ViewTicketDetailModal(ticketDetailContainer);
        $('.totalPayout').unbind('click').bind('click', function () {
            var $this = $(this);
            viewTicketDetailModal.initialize($this.data('member'), $this.data('betid'));
        });

        var viewTicketNotesModal = new ViewTicketNotesModal(ticketNotesContainer);
        $('.ico-remark').unbind('click').bind('click', function () {
            var $this = $(this);
            viewTicketNotesModal.initialize($this.data('ticket'), $this.data('betid'));
        });

        if (updateStatus) {
            var updateTicketStatusModal = new UpdateTicketStatusModal(updateTicketStatusForm, renderPaymentList);
            $('.ticketStatus').unbind('click').bind('click', function () {
                var $this = $(this);
                updateTicketStatusModal.initialize($this.data('ticket'), $this.data('betid'), $(this).html());
            });
        }
    }

    function initialize() {
        gridOptions = initGridOptions();

        formFilter.submit(function (e) {
            e.preventDefault();
            renderPaymentList();
        });

        $('#divModals').empty();
        $('#member').focus();
    }

    initialize();
})();
