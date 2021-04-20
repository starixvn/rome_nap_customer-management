"use strict";
function SiteTypeCriteria() {
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
                    <select class="combobox-autocomplete width-60" id="ddl-siteNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-site"></div>
                </div>
            </div>
        </div>`
    var $siteType;
    var $siteSelection;

    this.renderHtml = function ($selector, siteTypeList, notInList, siteList) {
        $selector.append(htmlTemplate);
        virtualRateEngineHelper.renderCombobox("ddl-siteType", siteTypeList);
        virtualRateEngineHelper.renderCombobox("ddl-siteNotIn", notInList);
        virtualRateEngineHelper.renderMultiSelect("ddl-site", siteList);
    };

    this.render = function (siteTypeList, siteList, notInList) {
        $("#chk-enableSiteTypeCriteria")
            .off("change")
            .on("change", function () { });

        $("#span-noteSiteTypeCriteria")
            .off("click")
            .on("click", function () { });
    };
}
