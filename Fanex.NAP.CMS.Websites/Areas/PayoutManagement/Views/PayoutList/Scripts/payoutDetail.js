/* global payoutManagementLabels, payoutDetail, currentStatus, CMS, numeral, CMSConstants */
(function () {
    'use strict';
    var self = this;
    var gridOptions = null,
        freegrid = $('#freegrid');
    var rootUrl = $('#CMSRoot').val();
    var $payoutlistLink = $('#payoutlist-link'),
        $refno = $('#refno'),
        $sitelicensee = $('#sitelicensee'),
        $seccodeBox = $('.seccode-inner'),
        $seccodeButton = $('#seccode-button'),
        $seccodeText = $('#seccode-text'),
        $amount = $('#amount'),
        $currency = $('#currency'),
        $ticketNote = $('#ticketNote'),
        $selectTicketStatus = $('#selectTicketStatus'),
        $updateButton = $('#update-button'),
        $ticketDetailContainer = $('#ticketDetailContainer');

    function trimField(value) {
        var html = value.replace(/^\s+|\s+$/g, '');
        if (html.length !== 0) {
            var tmp = document.createElement('div');
            tmp.innerHTML = html;
            html = tmp.textContent || tmp.innerText || '';
        }

        return html;
    }

    function stripLinebreak(value) {
        return value.replace(/\n/g, '<br/>')
            .replace(/\\r\\n/g, '<br/>');
    }

    function showMessage(result) {
        if (result.Code !== 0) {
            CMS.showErrorMessage(result.Message);
        }
        else {
            CMS.showSuccessMessage(result.Message, getPayoutDetail);
        }
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: payoutManagementLabels.Date, Width: 160, Name: 'colDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: payoutManagementLabels.Status, Width: 100, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.Note, Width: 440, Name: 'colNote', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.Handler, Width: 200, Name: 'colHandler', HeaderAlign: 'center', CellAlign: 'left' }
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 25,
            gridExpandHeight: 500,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(payoutNotes) {
        var bodyRows = [];
        var cell, note;
        var resultLength = payoutNotes ? payoutNotes.length : 0;
        for (var i = 0; i < resultLength; i++) {
            note = payoutNotes[i];
            cell = [
                { Name: 'colNumber', Value: (i + 1) },
                { Name: 'colDate', Value: note.DateDisplay },
                { Name: 'colStatus', Value: note.Status },
                { Name: 'colNote', Value: stripLinebreak(note.Note), CssClass: 'word-wrap' },
                { Name: 'colHandler', Value: note.HandlerDisplay }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function getPayoutDetail() {
        var successFunc = function (results) {
            var payoutNotes = $.parseJSON(results);
            gridOptions = gridOptions || initGridOptions();
            gridOptions.bodyRows = buildBodyRows(payoutNotes);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
        };

        var url = rootUrl + 'PayoutManagement/PayoutList/GetPayoutDetail?id=' + payoutDetail.Id;
        CMS.getJson(url, successFunc);
    }

    function renderPayoutDetailOnUI(payoutDetail) {
        $refno.html(payoutDetail.RefNo);
        $sitelicensee.html(payoutDetail.Site);
        if (payoutDetail.SecurityCode && payoutDetail.SecurityCode.length > 0) {
            $seccodeBox.removeClass('display-none');
            $seccodeText.html('******');
        }
        else {
            $seccodeBox.addClass('display-none');
        }

        $amount.attr('data-transid', payoutDetail.Id)
            .attr('data-refno', payoutDetail.RefNo)
            .attr('data-member', payoutDetail.CustomerId)
            .attr('data-product', payoutDetail.ProductId)
            .html(numeral(payoutDetail.Amount).format(CMSConstants.NumberFormat.TwoDecimalNumber))
            .click(viewBetTicket);

        $currency.html(payoutDetail.Currency);
    }

    function viewBetTicket() {
        var refno = $amount.data('refno'),
                product = parseInt($amount.data('product')),
                id = $amount.data('transid'),
                member = $amount.data('member');

        var successFunc = function (data) {
            $ticketDetailContainer.html(data);
            CMS.modal(payoutManagementLabels.ViewTicketDetailTitle + ' ' + refno, $ticketDetailContainer, { resizable: false });
        };

        var action;
        if (product === 204) {
            action = 'ColossusBets';
        }
        else if (product === 205) {
            action = 'Microgaming';
        }
        else {
            return;
        }

        var url = rootUrl + 'PayoutManagement/BetTicket/' + action + '?id=' + id + '&member=' + member;
        CMS.getHtml(url, function (data) { successFunc(data); });
    }

    function updateTicketStatus() {
        var note = trimField($ticketNote.val());
        var statusId = parseInt($selectTicketStatus.val());
        if (currentStatus === statusId && note.length === 0) {
            CMS.showTipsyError($ticketNote, 'Invalid Note', 'se');
            return;
        }

        var url = rootUrl + 'PayoutManagement/PayoutList/UpdateTicketStatus';
        var formData = {
            id: payoutDetail.Id,
            status: { id: statusId, name: $selectTicketStatus.find(':selected').text() },
            note: note
        };

        CMS.callAjax(self, url, formData, function (data) {
            var result = $.parseJSON(data);
            showMessage(result);
        });
    }

    function initialize() {
        getPayoutDetail();
        renderPayoutDetailOnUI(payoutDetail);

        $seccodeButton.click(function () {
            if ($seccodeText.html() === '******') {
                $seccodeText.html(payoutDetail.SecurityCode);
                $seccodeButton.text(payoutManagementLabels.Hide);
            }
            else {
                $seccodeText.html('******');
                $seccodeButton.text(payoutManagementLabels.Show);
            }
        });

        $updateButton.click(function () {
            updateTicketStatus();
        });

        $selectTicketStatus.val(currentStatus);
        $payoutlistLink.click(function () { window.location = rootUrl + 'PayoutManagement/PayoutList/Index'; });

        $('#divModals').empty();
    }

    initialize();
})();
