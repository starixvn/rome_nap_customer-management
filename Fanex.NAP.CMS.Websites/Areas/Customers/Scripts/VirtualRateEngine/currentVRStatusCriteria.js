"use strict";
function CurrentVRStatusCriteria(vrStatusList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">4.</span>
                <span class="txt-name">Current Virtual Rate Status</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableCurrentVRStatusCriteria" />
                <span class="note-info-message" id="span-noteCurrentVRStatusCriteria" title="<p>Current Virtual Rate Status:</p><p>- Status of Virtual Rate: Enabled, Disabled.</p>"></span>
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete width-60" id="ddl-currentVRStatus"></select>
            </div>
        </div>`
    var chkEnableCurrentVRStatusCriteria, ddlCurrentVRStatus;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlCurrentVRStatus = VREngineHelper.renderCombobox("ddl-currentVRStatus", vrStatusList);
        chkEnableCurrentVRStatusCriteria = $("#chk-enableCurrentVRStatusCriteria");
        $("#span-noteCurrentVRStatusCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
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
                name: "VRStatus",
                VRStatus: ddlCurrentVRStatus.val()
            };
        }
    };
}
