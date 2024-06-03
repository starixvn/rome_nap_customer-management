"use strict";
function SportCCCriteria(conditionList, customerClassDetailList, sportTypeList, sportType) {
    var htmlTemplate =
        `
        <div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">3.</span>
                <span class="txt-name">Sport Customer Class</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableSportCCCriteria" />
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete" id="ddl-sportTypeCC"></select>
            </div>
        </div>
        <div class="form-row-sub">
            <div class="lbl-field">
                <span class="txt-name">&nbsp;</span>
            </div>
            <div class="enable-criteria-container"></div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-sportCustomerClassCondition"></select>
                </div>
                <div class="sport-cc">
                    <textarea id="txtSportCC" rows="3" cols="27" placeholder="<CC1>,<CC2>,..." oninput="VREngineHelper.validateCCValue(this)" onblur="VREngineHelper.replaceCharCCValue(this)"></textarea>
                </div>
            </div>
        </div>`;
    var chkEnableSportCCCriteria, ddlSportCCCondition, ddlSportTypeCC, txtSportCC;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedSportCC, condition, selectedSportType;
        var isEnabled = rule && rule.EnabledCustomerClassDetail;
        if (isEnabled) {
            condition = rule.SportTypeCCListNotIn ? 1 : 0; 
            selectedSportType = rule.SportTypeCCListNotIn || rule.SportTypeCCList;
            selectedSportCC = rule.CustomerClassDetailListNotIn || rule.CustomerClassDetailList;
        }

        ddlSportCCCondition = VREngineHelper.renderCombobox("ddl-sportCustomerClassCondition", conditionList, condition);
        ddlSportTypeCC = VREngineHelper.renderCombobox("ddl-sportTypeCC", sportTypeList, +selectedSportType);
        txtSportCC = $("#txtSportCC").val(selectedSportCC);

        chkEnableSportCCCriteria = $("#chk-enableSportCCCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableSportCCCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableSportCCCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlSportCCCondition.combobox(action);
        ddlSportTypeCC.combobox(action);
        txtSportCC.prop("disabled", !isEnabled);
    }

    function isEnableCriteria() {
        return chkEnableSportCCCriteria && chkEnableSportCCCriteria.is(":checked");
    }

    function validateFromData() {
        var customerClassDetail = txtSportCC.val().toLowerCase() === "all"
            ? "0"
            : txtSportCC.val();

        var sportTypeCC = ddlSportTypeCC.val();

        if (!customerClassDetail.length) {
            CMS.showTipsyError(txtSportCC, resources.noneSelectedCCError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(txtSportCC);

        var formData = { name: "CustomerClassDetail" };
        formData[ddlSportCCCondition.val() === "0" ? "customerClassDetail" : "customerClassDetailNotIn"] = customerClassDetail;
        formData[ddlSportCCCondition.val() === "0" ? "cCSportType" : "cCSportTypeNotIn"] = sportTypeCC;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableSportCCCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
