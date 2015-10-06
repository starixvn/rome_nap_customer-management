/* global CMS, colossusBetsLabels */
/* exported UpdateTicketStatusModal */
function UpdateTicketStatusModal(updateTicketStatusForm, renderPaymentListFunc) {
    'use strict';
    var self = this;
    var ddlStatus = updateTicketStatusForm.find('#ddlStatus'),
        ticketNote = updateTicketStatusForm.find('#ticketNote'),
        lblCurrentTicketStatus = updateTicketStatusForm.find('#lblCurrentTicketStatus'),
        messageContainer = updateTicketStatusForm.find('.inline-message');
    var updateTicketStatusUrl = updateTicketStatusForm.attr('action');

    function showMessage (result) {
        if (result.Code !== 0) {
            CMS.showInlineErrorMsg(result.Message, messageContainer);
        }
        else {
            CMS.showInlineSuccessMsg(
                result.Message,
                messageContainer,
                true,
                function () {
                    CMS.closePopup();
                    renderPaymentListFunc();
                });
        }
    }

    function trimField(value) {
        return value.replace(/^\s+|\s+$/g, '');
    }

    function updateTicketStatus(e) {
        e.preventDefault();

        CMS.callAjax(
            self,
            updateTicketStatusUrl,
            { status: ddlStatus.val(), notes: trimField(ticketNote.val()) },
            function (result) { showMessage(result); });
    }

    self.initialize = function (ticketId, refno, currentStatus) {
        CMS.modal(colossusBetsLabels.UpdateTicketStatusTitle + ' ' + refno, updateTicketStatusForm, { resizable: false });

        ddlStatus.val(currentStatus).combobox();
        ticketNote.val('');
        lblCurrentTicketStatus.html(currentStatus);
        CMS.hideInlineMsg(messageContainer);

        updateTicketStatusForm.unbind('submit').bind('submit', updateTicketStatus);
        updateTicketStatusForm.find('.red-button').unbind('click').bind('click', CMS.closePopup);
    };
}