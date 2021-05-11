"use strict";
function TicketDoWCriteria(conditionList, dowList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">11.</span>
                <span class="txt-name">Ticket Day of Week</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableTicketDoWCriteria" />
                <span class="note-info-message" id="span-noteTicketDoWCriteria" title="<p>Ticket Day of Week:</p><p>- Select days to apply the virtual rate rule.</p><p>- You can select from Monday to Sunday.</p>"></span>
            </div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-ticketDoWCondition"></select>
                </div>
                <div class="ddl-second ticket-box">
                    <div id="ddl-ticketDoW"></div>
                </div>
            </div>
        </div>`;
    var chkEnableTicketDoWCriteria, ddlTicketDoWCondition, ddlTicketDoW;

    function cacheElements(rule, isHideEnabledCheckbox) {
        var selectedDoW, condition;
        var isEnabled = rule && rule.EnabledTicketDoW;
        if (rule) {
            condition = rule.DoWListNotIn ? 1 : 0;
            selectedDoW = rule.DoWListNotIn || rule.DoWList;
        }

        ddlTicketDoWCondition = VREngineHelper.renderCombobox("ddl-ticketDoWCondition", conditionList, condition);
        ddlTicketDoW = VREngineHelper.renderMultiSelect("ddl-ticketDoW", dowList, selectedDoW);

        chkEnableTicketDoWCriteria = $("#chk-enableTicketDoWCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableTicketDoWCriteria.css("visibility", "hidden");

        $("#span-noteTicketDoWCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function bindEvents() {
        chkEnableTicketDoWCriteria
            .off("change")
            .on("change", function () {
                enableCriteria(this.checked);
            });
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlTicketDoWCondition.combobox(action);
        ddlTicketDoW.multiSelect(action);
    }

    function isEnableCriteria() {
        return chkEnableTicketDoWCriteria && chkEnableTicketDoWCriteria.is(":checked");
    }

    function validateFromData() {
        var dow = ddlTicketDoW.multiSelect("isCheckedAllItems")
            ? "0"
            : ddlTicketDoW.multiSelect("getCheckedItems").join(",");

        if (!dow.length) {
            CMS.showTipsyError(ddlTicketDoW, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlTicketDoW);

        var formData = { name: "DateOfWeek" };
        formData[ddlTicketDoWCondition.val() === "0" ? "dayOfWeek" : "dayOfWeekNotIn"] = dow;

        return formData;
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableTicketDoWCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
