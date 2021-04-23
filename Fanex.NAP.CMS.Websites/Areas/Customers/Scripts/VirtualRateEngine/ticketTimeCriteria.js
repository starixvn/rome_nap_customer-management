"use strict";
function TicketTimeCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[12].</span>
                <span class="txt-name">Ticket Time (GMT+8)</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketTimeCriteria" />
                <span class="note-info-message" id="span-noteTicketTimeCriteria"></span>
            </div>
            <div class="detail-rule value-range">
                <span class="txt">From</span>
                <div class="from-value">
                    <input type="text" id="txt-ticketTimeFrom" autocomplete="off" maxlength="5" placeholder="hh:mm" value="00:00" />
                </div>
                <span class="txt">To</span>
                <div class="to-value">
                    <input type="text" id="txt-ticketTimeTo" autocomplete="off" maxlength="5" placeholder="hh:mm" value="00:00" />
                </div>
            </div>
        </div>`
    var chkEnableTicketTimeCriteria, txtTicketTimeFrom, txtTicketTimeTo;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        txtTicketTimeFrom = $("#txt-ticketTimeFrom").clearfield();
        txtTicketTimeTo = $("#txt-ticketTimeTo").clearfield();
        chkEnableTicketTimeCriteria = $("#chk-enableTicketTimeCriteria");
    }

    function bindEvents() {
        chkEnableTicketTimeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteTicketTimeCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var isDisabled = !isEnabled;
        txtTicketTimeFrom.prop("disabled", isDisabled);
        txtTicketTimeTo.prop("disabled", isDisabled);
    }

    function validateFormData() { }

    function isEnableCriteria() {
        return chkEnableTicketTimeCriteria.is(":checked");
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableTicketTimeCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                beginTime: txtTicketTimeFrom.val(),
                endTime: txtTicketTimeTo.val()
            };
        }
    };
}
