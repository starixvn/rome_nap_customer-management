"use strict";
function SportCriteria($selector, notInList, sportList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[5].</span>
                <span class="txt-name">Sport</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableSportCriteria" />
                <span class="note-info-message" id="span-noteSportCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-sportNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-sport"></div>
                </div>
            </div>
        </div>`
    var chkEnableSportCriteria, ddlSportNotIn, ddlSport;

    function renderHtml() {
        $selector.append(htmlTemplate);
        ddlSportNotIn = virtualRateEngineHelper.renderCombobox("ddl-sportNotIn", notInList);
        ddlSport = virtualRateEngineHelper.renderMultiSelect("ddl-sport", sportList);
        chkEnableSportCriteria = $("#chk-enableSportCriteria");
    }

    function bindEvents() {
        chkEnableSportCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteSportCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        if (isEnabled) {
            ddlSportNotIn.combobox("enable");
            //ddlSport.
        } else {
            ddlSportNotIn.combobox("disable");
            //ddlSport.
        }
    }

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableSportCriteria.remove();
        bindEvents();
    };
}
