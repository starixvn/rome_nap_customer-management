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
    var $siteType,
        $siteNotIn,
        $siteSelection;

    this.renderHtml = function ($selector, siteTypeList, notInList, siteList) {
        $selector.append(htmlTemplate);
        $siteType = virtualRateEngineHelper.renderCombobox("ddl-siteType", siteTypeList);
        $siteNotIn = virtualRateEngineHelper.renderCombobox("ddl-siteNotIn", notInList);
        $siteSelection = virtualRateEngineHelper.renderMultiSelect("ddl-site", siteList);
    };

    this.bindEvents = function () {
        $("#chk-enableSiteTypeCriteria")
            .off("change")
            .on("change", function () {
                if (this.checked) {
                    $siteType.combobox("enable");
                    $siteNotIn.combobox("enable");
                } else {
                    $siteType.combobox("disable");
                    $siteNotIn.combobox("disable");
                }
            });

        $("#span-noteSiteTypeCriteria")
            .off("click")
            .on("click", function () { });

        $siteType
            .off("change")
            .on("change", function () {
                alert(this.value);
            });
    };
}
