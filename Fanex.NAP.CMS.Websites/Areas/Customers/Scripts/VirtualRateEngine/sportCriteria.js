"use strict";
function SportCriteria(conditionList, sportList) {
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
                    <select class="combobox-autocomplete width-60" id="ddl-sportCondition"></select>
                </div>
                <div class="ddl-second sport-box">
                    <div id="ddl-sport"></div>
                </div>
            </div>
        </div>`
    var chkEnableSportCriteria, ddlSportCondition, ddlSport;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlSportCondition = virtualRateEngineHelper.renderCombobox("ddl-sportCondition", conditionList);
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
        var action = isEnabled ? "enable" : "disable";
        ddlSportCondition.combobox(action);
        ddlSport.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableSportCriteria.is(":checked");
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableSportCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                sport: ddlSport.multiSelect("getCheckedItems").join(","),
                condition: ddlSportCondition.val()
            };
        }
    };
}
