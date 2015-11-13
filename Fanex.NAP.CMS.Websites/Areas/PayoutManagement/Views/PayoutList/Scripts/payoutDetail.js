/* global colossusBetsLabels, id, currentStatus, CMS, numeral, CMSConstants */
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
        $updateButton = $('#update-button');
    var securityCode;

    function trimField(value) {
        return value.replace(/^\s+|\s+$/g, '');
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
            { HeaderText: colossusBetsLabels.Date, Width: 160, Name: 'colDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Status, Width: 100, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.Note, Width: 440, Name: 'colNote', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.Handler, Width: 200, Name: 'colHandler', HeaderAlign: 'center', CellAlign: 'left' }
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

    function buildBodyRows(results) {
        var bodyRows = [];
        var cell, note;
        var resultLength = results ? results.length : 0;
        for (var i = 0; i < resultLength; i++) {
            note = results[i];
            cell = [
                { Name: 'colNumber', Value: (i + 1) },
                { Name: 'colDate', Value: note.DateDisplay },
                { Name: 'colStatus', Value: note.Status },
                { Name: 'colNote', Value: stripLinebreak(note.Note), CssClass: 'word-wrap' },
                { Name: 'colHandler', Value: note.Handler }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function getPayoutDetail() {
        var successFunc = function (results) {
            var resultsJson = $.parseJSON(results);

            $refno.html(resultsJson.RefNo);
            $sitelicensee.html(resultsJson.Site);
            if (resultsJson.SecurityCode && resultsJson.SecurityCode.length > 0) {
                securityCode = resultsJson.SecurityCode;
                $seccodeBox.removeClass('display-none');
                $seccodeText.html('******');
            }
            else {
                securityCode = undefined;
                $seccodeBox.addClass('display-none');
            }

            $amount.html(numeral(resultsJson.Amount).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            $currency.html(resultsJson.Currency);

            gridOptions = gridOptions || initGridOptions();
            gridOptions.bodyRows = buildBodyRows(resultsJson.PayoutNotes);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
        };

        var url = rootUrl + 'PayoutManagement/PayoutList/GetPayoutDetail?id=' + id;
        CMS.getJson(url, successFunc);
    }

    function updateTicketStatus() {
        var note = trimField($ticketNote.val());
        var statusId = parseInt($selectTicketStatus.val());
        if (currentStatus === statusId && note.length === 0) {
            return;
        }

        var url = rootUrl + 'PayoutManagement/PayoutList/UpdateTicketStatus';
        var formData = {
            id: id,
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

        $seccodeButton.click(function () {
            if ($seccodeText.html() === '******') {
                $seccodeText.html(securityCode);
                $seccodeButton.text(colossusBetsLabels.Hide);
            }
            else {
                $seccodeText.html('******');
                $seccodeButton.text(colossusBetsLabels.Show);
            }
        });

        $updateButton.click(function () {
            updateTicketStatus();
        });

        $selectTicketStatus.val(currentStatus);
        $payoutlistLink.click(function () { window.location = rootUrl + 'PayoutManagement/PayoutList/Index'; });
    }

    initialize();
})();
