/// <reference path="../PayoutDetail.cshtml" />
/// <reference path="D:\Gitlab\NAP\customer-management\Fanex.NAP.CMS.Websites\Scripts/CMS.js" />
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
        $amount = $('#amount'),
        $currency = $('#currency'),
        $ticketNote = $('#ticketNote'),
        $selectTicketStatus = $('#selectTicketStatus'),
        $updateButton = $('#update-button'),
        $winningCodeLabel = $('#winning-code-label'),
        $winningCode = $('#WinningCode'),
        $verifyButton = $('#verify-button'),
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
        renderPayoutDetailOnUI(payoutDetail);
        var successFunc = function (results) {
            var payoutNotes = $.parseJSON(results);
            gridOptions = gridOptions || initGridOptions();
            gridOptions.bodyRows = buildBodyRows(payoutNotes);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);

            if (isDisableStatusDropdown(currentStatus)) {
                $selectTicketStatus.combobox('disable');
            }
            else {
                $selectTicketStatus.combobox('enable');
            }
            $selectTicketStatus.combobox('readonly');
        };

        var url = rootUrl + 'PayoutManagement/PayoutList/GetPayoutDetail?id=' + payoutDetail.Id;
        CMS.getJson(url, successFunc);
    }

    function renderPayoutDetailOnUI(payoutDetail) {
        $refno.html(payoutDetail.RefNo);
        $sitelicensee.html(payoutDetail.Site);
        if (payoutDetail.SecurityCode && payoutDetail.SecurityCode.length > 0) {
            $winningCode.removeClass('display-none');
            $winningCodeLabel.removeClass('display-none');
            $verifyButton.removeClass('display-none');
        }
        else {
            $winningCode.addClass('display-none');
            $winningCodeLabel.addClass('display-none');
            $verifyButton.addClass('display-none');
        }

        $amount.attr('data-transid', payoutDetail.Id)
            .attr('data-refno', payoutDetail.RefNo)
            .attr('data-member', payoutDetail.CustomerId)
            .attr('data-product', payoutDetail.ProductId)
            .html(numeral(payoutDetail.Amount).format(CMSConstants.NumberFormat.TwoDecimalNumber))
            .click(viewBetTicket);

        $currency.html(payoutDetail.Currency);

        if (isDisableStatusDropdown(currentStatus)) {
            $selectTicketStatus.combobox('disable');
        }
        else {
            $selectTicketStatus.combobox('enable');
        }
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
            action = 'ColossusBets?id=' + id + '&member=' + member;
        }
        else if (product === 205) {
            action = 'Microgaming?id=' + id + '&member=' + member;
        }
        else if (product === 208 || product === 209 || product === 210) {
            action = 'RNGCasinoJackpot?refno=' + refno;
        }
        else {
            return;
        }

        var url = rootUrl + 'PayoutManagement/BetTicket/' + action;
        CMS.getHtml(url, function (data) { successFunc(data); });
    }

    function updateTicketStatus() {
        var statusId = parseInt($selectTicketStatus.val());
        var url = rootUrl + 'PayoutManagement/PayoutList/UpdateTicketStatus';
        var formData = getUpdateTicketStatusParams();
        CMS.callAjax(self, url, formData, function (data) {
            var result = $.parseJSON(data);
            if (isDisableStatusDropdown(statusId)) {
                $selectTicketStatus.combobox('disable');
            }

            currentStatus = formData.status.id;
            showMessage(result);
        });
    }

    function checkWinningCode() {
        var url = rootUrl + 'PayoutManagement/PayoutList/CheckWinningCode';
        CMS.callAjax(self, url, { winningCode: $winningCode.val() }, function (result) {
            if (result == 'true') {
                CMS.showSuccessMessage('WINNING CODE IS VALID.');
            } else {
                CMS.showErrorMessage('WINNING CODE IS INVALID.');
            }
        })
    }

    function getUpdateTicketStatusParams() {
        return {
            id: payoutDetail.Id,
            status: { id: parseInt($selectTicketStatus.val()), name: $selectTicketStatus.find(':selected').text() },
            note: trimField($ticketNote.val())
        };
    }

    function isDisableStatusDropdown(statusId) {
        // Completed = 4
        // Expired = 5
        // Cancelled = 6
        return statusId === 4 || statusId === 5 || statusId === 6;
    }

    function isShowConfirmMessage(productId, statusId) {
        // Colossus Bets = 204
        // Completed = 4
        // Expired = 5
        return productId == 204 && (statusId === 4 || statusId === 5);
    }

    function initialize() {
        getPayoutDetail();
        $selectTicketStatus.val(currentStatus);

        $updateButton.click(function () {
            if (trimField($ticketNote.val()).length === 0) {
                CMS.showTipsyError($ticketNote, 'Required field!', 'se');
                return;
            }

            var slectedStatus = parseInt($selectTicketStatus.val());
            if (isShowConfirmMessage(payoutDetail.ProductId, slectedStatus)) {
                CMS.showConfirmMessage(payoutManagementLabels.ConfirmUpdateCompleteMessage, updateTicketStatus);
            } else {
                updateTicketStatus();
            }
        });

        $verifyButton.click(function () {
            if (($winningCode.val()).length === 0) {
                CMS.showTipsyError($winningCode, 'Required field!', 'se');
                return;
            }

            checkWinningCode();
        });

        $payoutlistLink.click(function () { window.location = rootUrl + 'PayoutManagement/PayoutList/Index'; });

        $('#divModals').empty();
    }

    initialize();
})();
