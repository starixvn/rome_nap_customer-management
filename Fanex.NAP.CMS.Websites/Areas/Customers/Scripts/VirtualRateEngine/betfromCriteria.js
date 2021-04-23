"use strict";
function BetfromCriteria(betfromList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[9].</span>
                <span class="txt-name">Betfrom</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableBetfromCriteria" />
                <span class="note-info-message" id="span-noteBetfromCriteria"></span>
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete" id="ddl-betfrom"></select>
            </div>
        </div>`
    var chkEnableBetfromCriteria, ddlBetfrom;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlBetfrom = virtualRateEngineHelper.renderCombobox("ddl-betfrom", betfromList);
        chkEnableBetfromCriteria = $("#chk-enableBetfromCriteria");
    }

    function bindEvents() {
        chkEnableBetfromCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked)
            });

        $("#span-noteBetfromCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        ddlBetfrom.combobox(isEnabled ? "enable" : "disable");
    }

    function isEnableCriteria() {
        return chkEnableBetfromCriteria.is(":checked");
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml();
        chkEnableBetfromCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                betfrom: ddlBetfrom.val(),
            };
        }
    };
}
