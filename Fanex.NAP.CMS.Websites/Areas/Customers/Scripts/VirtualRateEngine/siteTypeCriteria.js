"use strict";
function SiteTypeCriteria(siteTypeList, conditionList, creditSiteList, licenseeSiteList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[1].</span>
                <span class="txt-name">Site Type</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableSiteTypeCriteria" />
                <span class="note-info-message" id="span-noteSiteTypeCriteria"></span>
            </div>
            <div class="detail-rule">
                <select class="combobox-autocomplete width-60" id="ddl-siteType"></select>
            </div>
        </div>
        <div class="form-row-sub">
            <div class="lbl-field">
                <span class="txt-name">Site</span>
            </div>
            <div class="enable-criteria-container"></div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-siteCondition"></select>
                </div>
                <div class="ddl-second site-type-box">
                    <div id="ddl-site"></div>
                </div>
            </div>
        </div>`
    var chkEnableSiteTypeCriteria, ddlSiteType, ddlSiteCondition, ddlSite;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlSiteType = virtualRateEngineHelper.renderCombobox("ddl-siteType", siteTypeList);
        ddlSiteCondition = virtualRateEngineHelper.renderCombobox("ddl-siteCondition", conditionList);
        ddlSite = virtualRateEngineHelper.renderMultiSelect("ddl-site", [].concat(creditSiteList, licenseeSiteList));
        chkEnableSiteTypeCriteria = $("#chk-enableSiteTypeCriteria");
    }

    function bindEvents() {
        chkEnableSiteTypeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked)
            });

        $("#span-noteSiteTypeCriteria")
            .off("click")
            .on("click", function () { });

        ddlSiteType
            .off("change")
            .on("change", function () {
                var siteType = this.value;
                if (siteType === "1") { // Credit
                    ddlSite = virtualRateEngineHelper.updateMultiSelect(ddlSite, creditSiteList);
                } else if (siteType === "2") { // Licensee
                    ddlSite = virtualRateEngineHelper.updateMultiSelect(ddlSite, licenseeSiteList);
                } else { // All
                    ddlSite = virtualRateEngineHelper.updateMultiSelect(ddlSite, [].concat(creditSiteList, licenseeSiteList));
                }
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlSiteType.combobox(action);
        ddlSiteCondition.combobox(action);
        ddlSite.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableSiteTypeCriteria.is(":checked");
    }

    function validateFromData() {
        var site = ddlSite.multiSelect("getCheckedItems").join(",");

        if (!site.length) {
            CMS.showTipsyError(ddlSite, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlSite);
        return {
            siteType: ddlSiteType.val(),
            site: site,
            siteCondition: ddlSiteCondition.val()
        };
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableCustomerClassCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
