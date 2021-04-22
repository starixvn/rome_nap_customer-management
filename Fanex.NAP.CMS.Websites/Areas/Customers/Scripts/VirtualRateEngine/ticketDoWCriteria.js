"use strict";
function TicketDoWCriteria($selector, notInList, dowList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[11].</span>
                <span class="txt-name">Ticket Date of Week</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketDoWCriteria" />
                <span class="note-info-message" id="span-noteTicketDoWCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-ticketDoWNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-ticketDoW"></div>
                </div>
            </div>
        </div>`
    var chkEnableTicketDoWCriteria, ddlTicketDoWNotIn, ddlTicketDoW;

    function renderHtml() {
        $selector.append(htmlTemplate);
        ddlTicketDoWNotIn = virtualRateEngineHelper.renderCombobox("ddl-ticketDoWNotIn", notInList);
        ddlTicketDoW = virtualRateEngineHelper.renderMultiSelect("ddl-ticketDoW", dowList);
        chkEnableTicketDoWCriteria = $("#chk-enableTicketDoWCriteria");
    }

    function bindEvents() {
        chkEnableTicketDoWCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteTicketDoWCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        if (isEnabled) {
            ddlTicketDoWNotIn.combobox("enable");
            //ddlTicketDoW.
        } else {
            ddlTicketDoWNotIn.combobox("disable");
            //ddlTicketDoW.
        }
    }

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableTicketDoWCriteria.remove();
        bindEvents();
    };
}
