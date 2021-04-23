"use strict";
function BetTypeCriteria(conditionList, betTypeList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[6].</span>
                <span class="txt-name">Bet Type</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableBetTypeCriteria" />
                <span class="note-info-message" id="span-noteBetTypeCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-betTypeNotIn"></select>
                </div>
                <div class="ddl-second bet-type-box">
                    <div id="ddl-betType"></div>
                </div>
            </div>
        </div>`
    var chkEnableBetTypeCriteria, ddlBetTypeNotIn, ddlBetType;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlBetTypeNotIn = virtualRateEngineHelper.renderCombobox("ddl-betTypeNotIn", conditionList);
        ddlBetType = virtualRateEngineHelper.renderMultiSelect("ddl-betType", betTypeList);
        chkEnableBetTypeCriteria = $("#chk-enableBetTypeCriteria");
    }

    function bindEvents() {
        chkEnableBetTypeCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteBetTypeCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        if (isEnabled) {
            ddlBetTypeNotIn.combobox("enable");
            //ddlBetType.
        } else {
            ddlBetTypeNotIn.combobox("disable");
            //ddlBetType.
        }
    }

    this.initAddNew = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableBetTypeCriteria.remove();
        bindEvents();
    };
}
