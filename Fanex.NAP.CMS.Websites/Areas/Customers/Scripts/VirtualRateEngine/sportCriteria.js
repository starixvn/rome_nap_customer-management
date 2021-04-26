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

    function renderHtml(criteriaContainer, selectedSportList) {
        criteriaContainer.append(htmlTemplate);
        ddlSportCondition = virtualRateEngineHelper.renderCombobox("ddl-sportCondition", conditionList);
        ddlSport = virtualRateEngineHelper.renderMultiSelect("ddl-sport", sportList, selectedSportList);
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

    function validateFromData() {
        var sportType = ddlSport.multiSelect("getCheckedItems").join(",");

        if (!sportType.length) {
            CMS.showTipsyError(ddlSport, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlSport);
        return {
            sportType: sportType,
            sportTypeCondition: ddlSportCondition.val()
        };
    }

    this.initAddNew = function (criteriaContainer, selectedSportList) {
        renderHtml(criteriaContainer, selectedSportList);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer, selectedSportList);
        chkEnableSportCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
