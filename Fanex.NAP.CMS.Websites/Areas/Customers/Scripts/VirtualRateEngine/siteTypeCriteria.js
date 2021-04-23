"use strict";
function SiteTypeCriteria(siteTypeList, conditionList, siteList) {
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
                <select class="combobox-autocomplete" id="ddl-siteType"></select>
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
        ddlSite = virtualRateEngineHelper.renderMultiSelect("ddl-site", siteList);
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
                alert(this.value);
            });
    }

    function enableCriteria(isEnabled) {
        if (isEnabled) {
            ddlSiteType.combobox("enable");
            ddlSiteCondition.combobox("enable");
            //ddlSite
        } else {
            ddlSiteType.combobox("disable");
            ddlSiteCondition.combobox("disable");
            //ddlSite
        }
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
        return {
            siteType: ddlSiteType.val(),
            site: ddlSite.getCheckedItems().join(","),
            condition: ddlSiteCondition.val()
        };
    };
}
