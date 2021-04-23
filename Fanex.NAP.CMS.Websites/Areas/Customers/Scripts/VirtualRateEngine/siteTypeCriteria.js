"use strict";
function SiteTypeCriteria($selector, siteTypeList, notInList, siteList) {
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
                    <select class="combobox-autocomplete width-60" id="ddl-siteNotIn"></select>
                </div>
                <div class="ddl-second site-type-box">
                    <div id="ddl-site"></div>
                </div>
            </div>
        </div>`
    var chkEnableSiteTypeCriteria, ddlSiteType, ddlSiteNotIn, ddlSite;

    function renderHtml() {
        $selector.append(htmlTemplate);
        ddlSiteType = virtualRateEngineHelper.renderCombobox("ddl-siteType", siteTypeList);
        ddlSiteNotIn = virtualRateEngineHelper.renderCombobox("ddl-siteNotIn", notInList);
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
            ddlSiteNotIn.combobox("enable");
            //ddlSite
        } else {
            ddlSiteType.combobox("disable");
            ddlSiteNotIn.combobox("disable");
            //ddlSite
        }
    }

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableCustomerClassCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        return {
            siteType: ddlSiteType.val(),
            site: ddlSite.getCheckedItems().join(","),
            notIn: ddlSiteNotIn.val()
        };
    };
}
