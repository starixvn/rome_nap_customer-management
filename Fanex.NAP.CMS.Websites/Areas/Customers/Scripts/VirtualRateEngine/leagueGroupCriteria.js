"use strict";
function LeagueGroupCriteria(conditionList, leagueGroupList, leagueList) {
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
                    <select class="combobox-autocomplete width-60" id="ddl-leagueGroupCondition"></select>
                </div>
                <div class="ddl-second league-box-1">
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
                    <select class="combobox-autocomplete width-60" id="ddl-leagueCondition"></select>
                </div>
                <div class="ddl-second league-box-2">
                    <div id="ddl-league"></div>
                </div>
            </div>
        </div>`
    var chkEnableLeagueGroupCriteria, ddlLeagueGroupCondition, ddlLeagueGroup, ddlLeagueCondition, ddlLeague;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlLeagueGroupCondition = virtualRateEngineHelper.renderCombobox("ddl-leagueGroupCondition", conditionList);
        ddlLeagueGroup = virtualRateEngineHelper.renderMultiSelect("ddl-leagueGroup", leagueGroupList);
        ddlLeagueCondition = virtualRateEngineHelper.renderCombobox("ddl-leagueCondition", conditionList);
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
        var action = isEnabled ? "enable" : "disable";
        ddlLeagueGroupCondition.combobox(action);
        ddlLeagueGroup.multiSelect(action);
        ddlLeagueCondition.combobox(action);
        ddlLeague.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableLeagueGroupCriteria.is(":checked");
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableLeagueGroupCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                leagueGroup: ddlLeagueGroup.multiSelect("getCheckedItems").join(","),
                leagueGroupCondition: ddlLeagueCondition.val(),
                league: ddlLeague.multiSelect("getCheckedItems").join(","),
                leagueCondition: ddlLeagueCondition.val()
            };
        }
    };
}
