"use strict";
function EventCriteria(eventList) {
    var htmlTemplate =
        `<div class="form-row-sub event-row">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">7.</span>
                <span class="txt-name">Event</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableEventCriteria" />
            </div>
            <div class="detail-rule edit-with-error-field">
                <select class="combobox-autocomplete width-60" id="ddl-event"></select>
            </div>
        </div>`;
    var chkEnableEventCriteria, ddlEvent;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedLiveIndicator;
        var isEnabled = rule && rule.EnabledEvent;
        if (isEnabled) {
            selectedLiveIndicator = rule.LiveIndicator;
        }

        ddlEvent = VREngineHelper.renderCombobox("ddl-event", eventList, selectedLiveIndicator);
        chkEnableEventCriteria = $("#chk-enableEventCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableEventCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableEventCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        ddlEvent.combobox(isEnabled ? "enable" : "disable");
    }

    function isEnableCriteria() {
        return chkEnableEventCriteria && chkEnableEventCriteria.is(":checked");
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableEventCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                name: "LiveIndicator",
                liveIndicator: ddlEvent.val(),
            };
        }
    };
}
