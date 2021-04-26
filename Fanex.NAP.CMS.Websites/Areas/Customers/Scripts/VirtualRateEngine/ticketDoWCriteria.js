"use strict";
function TicketDoWCriteria(conditionList, dowList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">[11].</span>
                <span class="txt-name">Ticket Date of Week</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketDoWCriteria" />
                <span class="note-info-message" id="span-noteTicketDoWCriteria"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-ticketDoWCondition"></select>
                </div>
                <div class="ddl-second ticket-box">
                    <div id="ddl-ticketDoW"></div>
                </div>
            </div>
        </div>`
    var chkEnableTicketDoWCriteria, ddlTicketDoWCondition, ddlTicketDoW;

    function renderHtml(criteriaContainer, selectedDoWList) {
        criteriaContainer.append(htmlTemplate);
        ddlTicketDoWCondition = virtualRateEngineHelper.renderCombobox("ddl-ticketDoWCondition", conditionList);
        ddlTicketDoW = virtualRateEngineHelper.renderMultiSelect("ddl-ticketDoW", dowList, selectedDoWList);
        chkEnableTicketDoWCriteria = $("#chk-enableTicketDoWCriteria");
    }

    function bindEvents() {
        chkEnableTicketDoWCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });

        $("#span-noteTicketDoWCriteria")
            .off("click")
            .on("click", function () { });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlTicketDoWCondition.combobox(action);
        ddlTicketDoW.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableTicketDoWCriteria.is(":checked");
    }

    function validateFromData() {
        var dow = ddlTicketDoW.multiSelect("getCheckedItems").join(",");

        if (!dow.length) {
            CMS.showTipsyError(ddlTicketDoW, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlTicketDoW);
        return {
            dateOfWeek: dow,
            dateOfWeekCondition: ddlTicketDoWCondition.val()
        };
    }

    this.initAddNew = function (criteriaContainer, selectedDoWList) {
        renderHtml(criteriaContainer, selectedDoWList);
        enableCriteria(false);
        bindEvents();
    };

    this.initUpdate = function (criteriaContainer) {
        renderHtml(criteriaContainer);
        chkEnableTicketDoWCriteria.remove();
        bindEvents();
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
