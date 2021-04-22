"use strict";
function LeagueGroupCriteria($selector, notInList, leagueGroupList, leagueList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[8].</span>
                <span class="txt-name">League Group</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableLeagueGroupCriteria" />
                <span class="note-info-message" id="span-noteLeagueGroupCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-leagueGroupNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-leagueGroup"></div>
                </div>
            </div>
        </div>
        <div class="form-row-sub">
            <div class="lbl-field">
                <span class="txt-name"League</span>
            </div>
            <div class="enable-criteria-container"></div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-leagueNotIn"></select>
                </div>
                <div class="ddl-second">
                    <div id="ddl-league"></div>
                </div>
            </div>
        </div>`
    var chkEnableLeagueGroupCriteria, ddlLeagueGroupNotIn, ddlLeagueGroup, ddlLeagueNotIn, ddlLeague;

    function renderHtml() {
        $selector.append(htmlTemplate);
        ddlLeagueGroupNotIn = virtualRateEngineHelper.renderCombobox("ddl-leagueGroupNotIn", notInList);
        ddlLeagueGroup = virtualRateEngineHelper.renderMultiSelect("ddl-leagueGroup", leagueGroupList);
        ddlLeagueNotIn = virtualRateEngineHelper.renderCombobox("ddl-leagueNotIn", notInList);
        ddlLeague = virtualRateEngineHelper.renderMultiSelect("ddl-league", leagueList);
        chkEnableLeagueGroupCriteria = $("#chk-enableLeagueGroupCriteria");
    }

    function bindEvents() {
        chkEnableLeagueGroupCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked)
            });

        $("#span-noteLeagueGroupCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        if (isEnabled) {
            ddlLeagueGroupNotIn.combobox("enable");
            //ddlLeagueGroup
            ddlLeagueNotIn.combobox("enable");
            //ddlLeague
        } else {
            ddlLeagueGroupNotIn.combobox("disable");
            //ddlLeagueGroup
            ddlLeagueNotIn.combobox("disable");
            //ddlLeague
        }
    }

    this.initAddNew = function () {
        renderHtml();
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function () {
        renderHtml();
        chkEnableLeagueGroupCriteria.remove();
        bindEvents();
    };
}
