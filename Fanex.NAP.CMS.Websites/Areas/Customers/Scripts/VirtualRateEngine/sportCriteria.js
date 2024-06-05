"use strict";
function SportCriteria(conditionList, sportList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">5.</span>
                <span class="txt-name">Sport</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableSportCriteria" />
            </div>
            <div class="detail-rule edit-with-error-field">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-sportCondition"></select>
                </div>
                <div class="ddl-second sport-box">
                    <div id="ddl-sport"></div>
                </div>
            </div>
        </div>`;
    var chkEnableSportCriteria, ddlSportCondition, ddlSport;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedSportType, condition;
        var isEnabled = rule && rule.EnabledSport;
        if (isEnabled) {
            condition = rule.SportTypeListNotIn ? 1 : 0;
            selectedSportType = rule.SportTypeListNotIn || rule.SportTypeList;
        }

        ddlSportCondition = VREngineHelper.renderCombobox("ddl-sportCondition", conditionList, condition);
        ddlSport = VREngineHelper.renderMultiSelect("ddl-sport", sportList, selectedSportType, 260);

        chkEnableSportCriteria = $("#chk-enableSportCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableSportCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableSportCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlSportCondition.combobox(action);
        ddlSport.fanexMultiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableSportCriteria && chkEnableSportCriteria.is(":checked");
    }

    function validateFromData() {
        var sportType = ddlSport.fanexMultiSelect("isCheckAll")
            ? "0"
            : ddlSport.fanexMultiSelect("getItems", true).join(",");

        if (!sportType.length) {
            CMS.showTipsyError(ddlSport, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlSport);

        var formData = { name: "Sporttype" };
        formData[ddlSportCondition.val() === "0" ? "sportType" : "sportTypeNotIn"] = sportType;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableSportCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
