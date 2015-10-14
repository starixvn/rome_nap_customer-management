/* global CMS, colossusBetsLabels */
/* exported UpdateTicketStatusModal */
function UpdateTicketStatusModal(updateTicketStatusForm, renderPaymentListFunc) {
    'use strict';
    var self = this;
    self.betId = 0;
    self.currentStatus = 0;
    var ddlStatus = updateTicketStatusForm.find('#ddlStatus'),
        ticketNote = updateTicketStatusForm.find('#ticketNote'),
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

    function renderStatusSelect(statusList, currentStatus) {
        var html = [];
        for (var i = 0, max = statusList.length; i < max; i++) {
            var status = statusList[i];
            html.push('<option value="');
            html.push(status.Id);
            html.push('"');
            if (status.Id === currentStatus) {
                html.push(' selected="selected" ');
            }

            html.push('>');
            html.push(status.Name);
            html.push('</option>');
        }

        ddlStatus.html(html.join('')).combobox();
    }

    function updateTicketStatus(e) {
        e.preventDefault();

        var note = trimField(ticketNote.val());
        var statusId = parseInt(ddlStatus.val());
        if (self.currentStatus === statusId && note.length === 0) {
            return;
        }

        CMS.callAjax(
            self,
            updateTicketStatusUrl,
            { betId: self.betId, status: { id: statusId, name: ddlStatus.find(':selected').text() }, note: note },
            function (data) {
                var result = $.parseJSON(data);
                showMessage(result);
            });
    }

    self.initialize = function (betId, currentStatus, statusList) {
        self.betId = betId;
        self.currentStatus = currentStatus;
        CMS.modal(colossusBetsLabels.UpdateTicketStatusTitle + ' ' + betId, updateTicketStatusForm, { resizable: false });

        renderStatusSelect(statusList, currentStatus);
        ticketNote.val('');
        CMS.hideInlineMsg(messageContainer);

        updateTicketStatusForm.unbind('submit').bind('submit', updateTicketStatus);
        updateTicketStatusForm.find('.red-button').unbind('click').bind('click', CMS.closePopup);
    };
}