"use strict";
function TicketTimeCriteria() {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">12.</span>
                <span class="txt-name">Ticket Time (GMT+8)</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketTimeCriteria" />
                <span class="note-info-message" id="span-noteTicketTimeCriteria" title="<p>Ticket Time (GMT +8):</p><p>- Apply virtual rate rule from time value to time value.</p><p>- To time should be greater than From Time.</p><p>- You should select time range in 1 day.</p>"></span>
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
        $("#span-noteTicketTimeCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
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

    function isEnableCriteria() {
        return chkEnableTicketTimeCriteria.is(":checked");
    }

    function validateFormData() {
        var beginTime = validateHourMinute(txtTicketTimeFrom);
        var endTime = validateHourMinute(txtTicketTimeTo);

        if (!beginTime || !endTime) {
            return { isInvalid: true };
        }

        var beginTimeArray = beginTime.split(":");
        var beginHour = parseInt(beginTimeArray[0]);
        var beginMinute = parseInt(beginTimeArray[1]);

        var endTimeArray = endTime.split(":");
        var endHour = parseInt(endTimeArray[0]);
        var endMinute = parseInt(endTimeArray[1]);

        if (beginHour > endHour || (beginHour === endHour && beginMinute > endMinute)) {
            CMS.showTipsyError(txtTicketTimeFrom, resources.fromTimeGreaterThanToTime, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtTicketTimeFrom);
        CMS.clearTipsyError(txtTicketTimeTo);
        return {
            name: "TicketTime",
            beginTime: beginTime,
            endTime: endTime
        };
    }

    function validateHourMinute(txtTime) {
        var inputTime = $.trim(txtTime.val());

        if (!/^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/.test(inputTime)) {
            CMS.showTipsyError(txtTime, resources.invalidTimeFormat, "nw");
            return;
        }

        return inputTime;
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
            return validateFormData();
        }
    };
}
