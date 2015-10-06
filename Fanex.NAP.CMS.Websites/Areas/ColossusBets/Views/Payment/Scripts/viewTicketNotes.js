/* global CMS, colossusBetsLabels */
/* exported ViewTicketNotesModal */
function ViewTicketNotesModal(ticketNoteContainer) {
    'use strict';
    var self = this;
    var tableNote = ticketNoteContainer.find('.tblColossusBets > tbody');
    var rootUrl = $('#CMSRoot').val();

    function renderNotes(ticketId) {
        var successFunc = function (results) {
            var resultsJson = $.parseJSON(results);
            var html = [];
            var resultLength = resultsJson ? resultsJson.length : 0;
            if (resultLength === 0) {
                html.push('<tr><td colspan="4" class="text-red">' + colossusBetsLabels.NoData + '</td></tr>');
                return;
            }

            var note;
            for (var i = 0; i < resultLength; i++) {
                note = resultsJson[i];
                html.push('<tr><td>');
                html.push(i + 1);
                html.push('</td><td>');
                html.push(note.Date);
                html.push('</td><td>');
                html.push(note.Author);
                html.push('</td><td>');
                html.push(note.Description);
                html.push('</td><tr>');
            }

            tableNote.html(html.join(''));
        };

        CMS.getJson(rootUrl + 'ColossusBets/Payment/GetTicketNotes?ticketId=' + ticketId, successFunc);
    }

    self.initialize = function (ticketId, refno) {
        CMS.modal(colossusBetsLabels.ViewTicketNoteTitle + ' ' + refno, ticketNoteContainer, { resizable: false });

        renderNotes(ticketId);
        ticketNoteContainer
            .find('.red-button')
            .unbind('click')
            .bind('click', CMS.closePopup);
    };
}