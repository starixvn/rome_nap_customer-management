"use strict";
function BetfromCriteria($selector, betfromList) {
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

    function renderHtml() {
        $selector.append(htmlTemplate);
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

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableBetfromCriteria.remove();
        bindEvents();
    };
}
