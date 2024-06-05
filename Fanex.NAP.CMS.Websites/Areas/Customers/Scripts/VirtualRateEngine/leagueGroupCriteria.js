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
                <span class="note-info-message" id="span-noteLeagueGroupCriteria" title="<p>League Group:</p><p>- Apply for one or more league groups.</p><p>League:</p><p>- Load all leagues follow selected league group.</p><p>- You can leave this field blank to apply all leagues of selected league group(s).</p>"></span>
            </div>
            <div class="detail-rule edit-with-error-field">
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
                <span class="txt-name">League</span>
            </div>
            <div class="enable-criteria-container"></div>
            <div class="detail-rule edit-with-error-field">
                <div class="ddl-primary">
                    <select class="combobox-autocomplete width-60" id="ddl-leagueCondition"></select>
                </div>
                <div class="ddl-second league-box-2">
                    <textarea id="txt-league"></textarea>
                </div>
            </div>
        </div>`;
    var chkEnableLeagueGroupCriteria, ddlLeagueGroupCondition, ddlLeagueGroup, ddlLeagueCondition, txtLeague;

    function cacheElements(rule, isHideEnabledCheckbox) {
        txtLeague = $("#txt-league");

        var selectedLeagueGroup, conditionLeagueGroup, selectedLeague, conditionLeague;
        var isEnabled = rule && rule.EnabledLeagueGroup;
        if (isEnabled) {
            conditionLeagueGroup = rule.LeagueGroupListNotIn ? 1 : 0;
            selectedLeagueGroup = rule.LeagueGroupListNotIn || rule.LeagueGroupList;
            conditionLeague = rule.LeagueListNotIn ? 1 : 0;
            selectedLeague = rule.LeagueListNotIn || rule.LeagueList;
            if (selectedLeague !== "-1") {
                txtLeague.val(selectedLeague);
            }
        }

        ddlLeagueGroupCondition = VREngineHelper.renderCombobox("ddl-leagueGroupCondition", conditionList, conditionLeagueGroup);
        let sabaLeagueIds = [42,74,108,113,122,151];
        var sabaLeagues = leagueGroupList.filter(league => sabaLeagueIds.includes(league.Id));
        var otherLeagues = leagueGroupList.filter(league => !sabaLeagueIds.includes(league.Id));
        var options = [
            {
                Name: "Saba Sports",
                Items: sabaLeagues
            },
            {
                Name: "Others",
                Items: otherLeagues
            }];

        ddlLeagueGroup = VREngineHelper.renderGroupMultiSelect("ddl-leagueGroup", options, selectedLeagueGroup, 220);
        ddlLeagueCondition = VREngineHelper.renderCombobox("ddl-leagueCondition", conditionList, conditionLeague);

        initLeagueAutoSuggestion();

        chkEnableLeagueGroupCriteria = $("#chk-enableLeagueGroupCriteria").prop("checked", isEnabled);
        isHideEnabledCheckbox && chkEnableLeagueGroupCriteria.css("visibility", "hidden");

        $("#span-noteLeagueGroupCriteria").tipsy({
            className: "criteria-hint",
            html: true,
            opacity: 1
        });
    }

    function initLeagueAutoSuggestion() {
        // don't navigate away from the field on tab when selecting an item
        txtLeague.on("keydown", function (event) {
            if (event.keyCode === $.ui.keyCode.TAB
                && $(this).autocomplete("instance").menu.active) {
                event.preventDefault();
            }
        }).autocomplete({
            source: function (request, response) {
                var getLeagueFormData = {
                    leagues: $.trim(extractLast(request.term))
                };
                getLeagueFormData[ddlLeagueGroupCondition.val() === "0" ? "leagueGroupIds" : "leagueGroupListNotIn"] = ddlLeagueGroup.fanexMultiSelect("getItems", true).join(",");

                $.getJSON(CMS.resolveRootUrl("Customers/VirtualRateEngine/GetLeagues"),
                    getLeagueFormData,
                    function (dataString) {
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
    }

    function enableCriteria(isEnabled) {
        var action = isEnabled ? "enable" : "disable";
        ddlLeagueGroupCondition.combobox(action);
        ddlLeagueGroup.fanexMultiSelect(action);
        ddlLeagueCondition.combobox(action);
        txtLeague.autocomplete(action);
        txtLeague.prop("disabled", !isEnabled);
    }

    function isEnableCriteria() {
        return chkEnableLeagueGroupCriteria && chkEnableLeagueGroupCriteria.is(":checked");
    }

    function validateFromData() {
        var leagueGroup = validateLeagueGroup();
        if (!leagueGroup) {
            CMS.showTipsyError(ddlLeagueGroup, resources.noneSelectedOptionError, "nw");
            return { isInvalid: true };
        }

        var league = validateLeague();
        if (!league) {
            return { isInvalid: true };
        }

        var formData = { name: "LeagueGroup" };
        formData[ddlLeagueGroupCondition.val() === "0" ? "leagueGroup" : "leagueGroupNotIn"] = leagueGroup;
        formData[ddlLeagueCondition.val() === "0" ? "league" : "leagueNotIn"] = league;

        return formData;
    }

    function validateLeagueGroup() {
        var leagueGroup = ddlLeagueGroup.fanexMultiSelect("isCheckAll")
            ? "-1"
            : ddlLeagueGroup.fanexMultiSelect("getItems", true).join(",");

        if (!leagueGroup.length) {
            CMS.showTipsyError(ddlLeagueGroup, resources.noneSelectedOptionError, "nw");
            return;
        }

        CMS.clearTipsyError(ddlLeagueGroup);
        return leagueGroup;
    }

    function validateLeague() {
        var intputLeague = $.trim(txtLeague.val());
        if (!intputLeague.length) {
            return "-1";
        }

        var validLeagues = [];
        var invalidLeagues = [];
        var inputLeagueArray = intputLeague.replace(/\s/g, "").split(",");
        $.each(inputLeagueArray, function (index, league) {
            if (league) {
                if (CMS.isNumeric(league)) {
                    validLeagues.push(league);
                } else {
                    invalidLeagues.push(league);
                }
            }
        });

        if (invalidLeagues.length) {
            CMS.showTipsyError(txtLeague, "LeagueID " + invalidLeagues.join(", ") + " do(es) not exist or not belong with selected LeagueGroup(s)", "nw");
            return;
        }

        CMS.clearTipsyError(ddlLeagueGroup);
        return validLeagues.join(",");
    }

    this.init = function (criteriaContainer, rule, isHideEnabledCheckbox) {
        criteriaContainer.append(htmlTemplate);

        cacheElements(rule, isHideEnabledCheckbox);
        bindEvents();

        chkEnableLeagueGroupCriteria.trigger("change");
    };

    this.getFormData = function () {
        if (isEnableCriteria()) {
            return validateFromData();
        }
    };
}
