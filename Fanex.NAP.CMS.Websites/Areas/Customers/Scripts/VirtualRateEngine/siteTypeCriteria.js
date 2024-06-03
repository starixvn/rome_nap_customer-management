"use strict";
function SiteTypeCriteria(siteTypeList, conditionList, creditSiteList, licenseeSiteList, adminPermission) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">1.</span>
                <span class="txt-name">Site Type</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableSiteTypeCriteria" />
                <span class="note-info-message" id="span-noteSiteTypeCriteria" title="<p>Site Type:</p><p>- Load Site types.</p><p>- Options: All, Credit, Licensee.</p><p>Site:</p><p>- Load all sites follow selected site type.</p><p>- You can select one or more sites to apply virtual rate rule.</p>"></span>
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
        </div>`;
    var chkEnableSiteTypeCriteria, ddlSiteType, ddlSiteCondition, ddlSite;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedSiteType, condition, selectedSites, sites;
        var isEnabled = rule && rule.EnabledSiteType;
        if (isEnabled) {
            selectedSiteType = rule.SiteType;
            if (selectedSiteType === 1) { // Credit
                sites = creditSiteList;
            } else if (selectedSiteType === 2) { // Licensee
                sites = licenseeSiteList;
            } else { // All
                sites = [].concat(creditSiteList, licenseeSiteList);
            }

            condition = rule.SiteIdListNotIn ? 1 : 0;
            selectedSites = rule.SiteIdListNotIn || rule.SiteIdList;
        } else {
            sites = [].concat(creditSiteList, licenseeSiteList);
        }

        ddlSiteType = VREngineHelper.renderCombobox("ddl-siteType", siteTypeList, selectedSiteType);
        ddlSiteCondition = VREngineHelper.renderCombobox("ddl-siteCondition", conditionList, condition);
        ddlSite = VREngineHelper.renderMultiSelect("ddl-site", sites, selectedSites);

        chkEnableSiteTypeCriteria = $("#chk-enableSiteTypeCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableSiteTypeCriteria.css("visibility", "hidden");

        $("#span-noteSiteTypeCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function bindEvents() {
        chkEnableSiteTypeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked)
            });

        ddlSiteType
            .off("change")
            .on("change", function () {
                var siteType = this.value;
                if (siteType === "1") { // Credit
                    ddlSite = VREngineHelper.updateMultiSelect(ddlSite, creditSiteList);
                } else if (siteType === "2") { // Licensee
                    ddlSite = VREngineHelper.updateMultiSelect(ddlSite, licenseeSiteList);
                } else { // All
                    ddlSite = VREngineHelper.updateMultiSelect(ddlSite, [].concat(creditSiteList, licenseeSiteList));
                }
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlSiteType.combobox(action);
        ddlSiteCondition.combobox(action);
        ddlSite.fanexMultiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableSiteTypeCriteria && chkEnableSiteTypeCriteria.is(":checked");
    }

    function validateFromData() {
        var site = ddlSite.fanexMultiSelect("isCheckAll")
            ? "0"
            : ddlSite.fanexMultiSelect("getItems", true).join(",");
        if (!site.length) {
            CMS.showTipsyError(ddlSite, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlSite);

        var formData = { name: "Site", siteType: ddlSiteType.val() };
        formData[ddlSiteCondition.val() === "0" ? "site" : "siteNotIn"] = site;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableSiteTypeCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
