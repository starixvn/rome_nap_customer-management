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
        </div>`;
    var chkEnableCurrentVRStatusCriteria, ddlCurrentVRStatus;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedVRStatus;
        var isEnabled = rule && rule.EnabledCurrentVRStatus;
        if (isEnabled) {
            selectedVRStatus = rule.VRStatus;
        }

        ddlCurrentVRStatus = VREngineHelper.renderCombobox("ddl-currentVRStatus", vrStatusList, selectedVRStatus);
        chkEnableCurrentVRStatusCriteria = $("#chk-enableCurrentVRStatusCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableCurrentVRStatusCriteria.css("visibility", "hidden");

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
    }

    function enableCriteria(isEnabled) {
        ddlCurrentVRStatus.combobox(isEnabled ? "enable" : "disable");
    }

    function isEnableCriteria() {
        return chkEnableCurrentVRStatusCriteria && chkEnableCurrentVRStatusCriteria.is(":checked");
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableCurrentVRStatusCriteria.trigger("change");
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
