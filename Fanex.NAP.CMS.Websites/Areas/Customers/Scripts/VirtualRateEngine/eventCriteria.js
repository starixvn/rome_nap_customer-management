"use strict";
function EventCriteria(eventList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[7].</span>
                <span class="txt-name">Event</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableEventCriteria" />
                <span class="note-info-message" id="span-noteEventCriteria"></span>
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete width-60" id="ddl-event"></select>
            </div>
        </div>`
    var chkEnableEventCriteria, ddlEvent;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlEvent = virtualRateEngineHelper.renderCombobox("ddl-event", eventList);
        chkEnableEventCriteria = $("#chk-enableEventCriteria");
    }

    function bindEvents() {
        chkEnableEventCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteEventCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        ddlEvent.combobox(isEnabled ? "enable" : "disable");
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableEventCriteria.remove();
        bindEvents();
    };
}
