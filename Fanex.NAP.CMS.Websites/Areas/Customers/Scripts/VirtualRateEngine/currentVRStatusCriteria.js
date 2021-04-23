"use strict";
function CurrentVRStatusCriteria(vrStatusList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[4].</span>
                <span class="txt-name">Current Virtual Rate Status</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCurrentVRStatusCriteria" />
                <span class="note-info-message" id="span-noteCurrentVRStatusCriteria"></span>
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete width-60" id="ddl-currentVRStatus"></select>
            </div>
        </div>`
    var chkEnableCurrentVRStatusCriteria, ddlCurrentVRStatus;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlCurrentVRStatus = virtualRateEngineHelper.renderCombobox("ddl-currentVRStatus", vrStatusList);
        chkEnableCurrentVRStatusCriteria = $("#chk-enableCurrentVRStatusCriteria");
    }

    function bindEvents() {
        chkEnableCurrentVRStatusCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteCurrentVRStatusCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        ddlCurrentVRStatus.combobox(isEnabled ? "enable" : "disable");
    }

    function isEnableCriteria() {
        return chkEnableCurrentVRStatusCriteria.is(":checked");
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
                VRStatus: ddlCurrentVRStatus.val()
            };
        }
    };
}
