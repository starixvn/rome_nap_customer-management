"use strict";
function BetfromCriteria(betfromList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">9.</span>
                <span class="txt-name">Bet From</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableBetfromCriteria" />
            </div>
            <div class="detail-rule edit-with-error-field">
                <select class="combobox-autocomplete" id="ddl-betfrom"></select>
            </div>
        </div>`;
    var chkEnableBetfromCriteria, ddlBetfrom;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedBetfrom;
        var isEnabled = rule && rule.EnabledBetfrom;
        if (isEnabled) {
            selectedBetfrom = rule.Betfrom;
        }

        ddlBetfrom = VREngineHelper.renderCombobox("ddl-betfrom", betfromList, selectedBetfrom, "sw");
        chkEnableBetfromCriteria = $("#chk-enableBetfromCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableBetfromCriteria.css("visibility", "hidden");
    }

    function bindEvents() {
        chkEnableBetfromCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        ddlBetfrom.combobox(isEnabled ? "enable" : "disable");
    }

    function isEnableCriteria() {
        return chkEnableBetfromCriteria && chkEnableBetfromCriteria.is(":checked");
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableBetfromCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return {
                name: "BetFrom",
                betfrom: ddlBetfrom.val()
            };
        }
    };
}
