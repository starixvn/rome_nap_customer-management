"use strict";
function LeagueGroupCriteria(conditionList, leagueGroupList) {
    var htmlTemplate =
        `<div class="form-row-sub">
            <div class="lbl-field">
                <span class="lbl-field-criteria-no">8.</span>
                <span class="txt-name">League Group</span>
            </div>
            <div class="enable-criteria-container">
                <input type="checkbox" id="chk-enableLeagueGroupCriteria" />
                <span class="note-info-message" id="span-noteLeagueGroupCriteria" title="<p>League Group:</p><p>- Apply for one or more league groups.</p><p>League:</p><p>- Load all leagues follow selected league group.</p>"></span>
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
        <div class="form-row-sub league-group-extra">
            <div class="lbl-field">
                <span class="txt-name"League</span>
            </div>
            <div class="enable-criteria-container"></div>
            <div class="detail-rule">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-leagueCondition"></select>
                </div>
                <div class="ddl-second league-box-2">
                    <textarea rows="6" id="txt-league"></textarea>
                </div>
            </div>
        </div>`
    var chkEnableLeagueGroupCriteria, ddlLeagueGroupCondition, ddlLeagueGroup, ddlLeagueCondition, txtLeague;

    function renderHtml(criteriaContainer) {
        criteriaContainer.append(htmlTemplate);
        ddlLeagueGroupCondition = VREngineHelper.renderCombobox("ddl-leagueGroupCondition", conditionList);
        ddlLeagueGroup = VREngineHelper.renderMultiSelect("ddl-leagueGroup", leagueGroupList, undefined, 220);
        ddlLeagueCondition = VREngineHelper.renderCombobox("ddl-leagueCondition", conditionList);
        txtLeague = $("#txt-league")
            // don't navigate away from the field on tab when selecting an item
            .on("keydown", function (event) {
                if (event.keyCode === $.ui.keyCode.TAB
                    && $(this).autocomplete("instance").menu.active) {
                    event.preventDefault();
                }
            })
            .autocomplete({
                source: function (request, response) {
                    $.getJSON(CMS.resolveRootUrl("Customers/VirtualRateEngine/GetLeagues"), {
                        //leagueGroupIds: ddlLeagueGroup.multiSelect("getCheckedItems").join(","),
                        leagues: $.trim(extractLast(request.term))
                    }, function (dataString) {
                        var data = JSON.parse(dataString);
                        response($.map(data, function (item) {
                            return {
                                value: item.Id,
                                label: item.Name
                            };
                        }));
                    });
                },
                search: function () {
                    // custom minLength
                    var term = extractLast(this.value);
                    if ($.trim(term).length < 3) {
                        return false;
                    }
                },
                focus: function () {
                    // prevent value inserted on focus
                    return false;
                },
                select: function (event, ui) {
                    var terms = split(this.value);
                    // remove the current input
                    terms.pop();
                    // add the selected item
                    terms.push(ui.item.value);
                    // add placeholder to get the comma-and-space at the end
                    terms.push("");
                    this.value = terms.join(", ");
                    return false;
                }
            });
        chkEnableLeagueGroupCriteria = $("#chk-enableLeagueGroupCriteria");
        $("#span-noteLeagueGroupCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
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
        txtLeague.autocomplete(action);
        txtLeague.prop("disabled", !isEnabled);
    }

    function isEnableCriteria() {
        return chkEnableLeagueGroupCriteria.is(":checked");
    }

    function validateFromData() {
        var leagueGroup = ddlLeagueGroup.multiSelect("isCheckedAllItems")
            ? "-1"
            : ddlLeagueGroup.multiSelect("getCheckedItems").join(",");

        if (!leagueGroup.length) {
            CMS.showTipsyError(ddlLeagueGroup, resources.noneSelectedOptionError);
            return { isInvalid: true };
        }

        CMS.clearTipsyError(ddlLeagueGroup);

        var league = $.trim(txtLeague.val());
        league = league.length ? VREngineHelper.removeLastComma(league) : "-1";

        var formData = { name: "LeagueGroup" };
        formData[ddlLeagueGroupCondition.val() === "0" ? "leagueGroup" : "leagueGroupNotIn"] = leagueGroup;
        formData[ddlLeagueCondition.val() === "0" ? "league" : "leagueNotIn"] = league;

        return formData;
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
            return validateFromData();
        }
    };
}
