"use strict";
function AddOrUpdateRule(addOrUpdateForm, getRuleListHandler, matrix, adminPermission) {
    var otherControls = new OtherControls(matrix.Adjustments, matrix.Actives);
    var siteTypeCriteria = new SiteTypeCriteria(matrix.SiteTypes, matrix.Conditions, matrix.CreditSites, matrix.LicenseeSites, adminPermission);
    var customerClassCriteria = new CustomerClassCriteria(matrix.Conditions);
    var sportCCCriteria = new SportCCCriteria(matrix.Conditions, matrix.CustomerClassesDetail, matrix.DetailSports, matrix.SportCC);
    var currentVRCriteria = new CurrentVRCriteria();
    var sportCriteria = new SportCriteria(matrix.Conditions, matrix.Sports);
    var betTypeCriteria = new BetTypeCriteria(matrix.Conditions, matrix.BetTypes);
    var eventCriteria = new EventCriteria(matrix.Events);
    var leagueGroupCriteria = new LeagueGroupCriteria(matrix.Conditions, matrix.LeagueGroups);
    var betfromCriteria = new BetfromCriteria(matrix.BetfromList);
    var stakeCriteria = new StakeCriteria();
    var ticketDoWCriteria = new TicketDoWCriteria(matrix.Conditions, matrix.DayOfWeeks);
    var ticketCurCriteria = new TicketCurrencyCriteria(matrix.Conditions, matrix.Currencies);
    var ticketTimeCriteria = new TicketTimeCriteria();

    function init(form, rule) {
        otherControls.init(form, rule);

        var enabledCriteria = getEnabledCriteria(rule);
        var $criteriaContainer = form.find("#div-criteriaContainer");

        var isHideEnabledCheckbox = !adminPermission;
        $.each(enabledCriteria, function (index, criteria) {
            criteria.init($criteriaContainer, rule, isHideEnabledCheckbox);
        });

        var buttonCancel = form.find("#btn-cancel");
        buttonCancel
            .off("click")
            .on("click", CMS.closePopup);

        var buttonAddOrUpdateRule = form.find("#btn-addOrUpdateRule");
        buttonAddOrUpdateRule
            .off("click")
            .on("click", function () {
                addOrUpdateRule(enabledCriteria, rule ? rule.RuleId : undefined, form.find("#divErrorSuccessMsg"));
            });
    }

    function getEnabledCriteria(rule) {
        var enabledCriteria = [];
        var isShowAllCriteria = typeof rule === "undefined" || adminPermission;

        var enabledSiteType = isShowAllCriteria || rule.EnabledSiteType;
        enabledSiteType && enabledCriteria.push(siteTypeCriteria);

        var enabledCustomerClass = isShowAllCriteria || rule.EnabledCustomerClass;
        enabledCustomerClass && enabledCriteria.push(customerClassCriteria);

        var enabledCustomerClassDetail = isShowAllCriteria || rule.EnabledCustomerClassDetail;
        enabledCustomerClassDetail && enabledCriteria.push(sportCCCriteria);

        var enabledCurrentVR = isShowAllCriteria || rule.EnabledCurrentVR;
        enabledCurrentVR && enabledCriteria.push(currentVRCriteria);

        var enabledSport = isShowAllCriteria || rule.EnabledSport;
        enabledSport && enabledCriteria.push(sportCriteria);

        var enabledBetType = isShowAllCriteria || rule.EnabledBetType;
        enabledBetType && enabledCriteria.push(betTypeCriteria);

        var enabledEvent = isShowAllCriteria || rule.EnabledEvent;
        enabledEvent && enabledCriteria.push(eventCriteria);

        var enabledLeagueGroup = isShowAllCriteria || rule.EnabledLeagueGroup;
        enabledLeagueGroup && enabledCriteria.push(leagueGroupCriteria);

        var enabledBetfrom = isShowAllCriteria || rule.EnabledBetfrom;
        enabledBetfrom && enabledCriteria.push(betfromCriteria);

        var enabledStake = isShowAllCriteria || rule.EnabledStake;
        enabledStake && enabledCriteria.push(stakeCriteria);

        var enabledTicketDoW = isShowAllCriteria || rule.EnabledTicketDoW;
        enabledTicketDoW && enabledCriteria.push(ticketDoWCriteria);

        var enabledTicketTime = isShowAllCriteria || rule.EnabledTicketTime;
        enabledTicketTime && enabledCriteria.push(ticketTimeCriteria);

        var enabledTicketCur = isShowAllCriteria || rule.EnabledTicketCur;
        enabledTicketCur && enabledCriteria.push(ticketCurCriteria);

        return enabledCriteria;
    }

    function addOrUpdateRule(enabledCriteria, ruleId, divMessage) {
        CMS.disableInput();

        var formData = validateRuleFormData(enabledCriteria);
        if (!formData) {
            CMS.enableInput();
            return;
        }

        var url = "Customers/VirtualRateEngine/";
        if (ruleId) {
            url += "UpdateRule";
            formData.ruleId = ruleId;
        } else {
            url += "AddNewRule";
        }

        CMS.callAjax(
            this,
            CMS.resolveRootUrl(url),
            formData,
            function (jsonString) {
                var result = JSON.parse(jsonString);
                if (result.Code) {
                    CMS.showInlineErrorMsg(result.Message, divMessage);
                } else {
                    CMS.showInlineSuccessMsg(result.Message, divMessage, true, function () {
                        CMS.closePopup();
                        getRuleListHandler();
                    });
                }
            }
        );
    }

    function validateRuleFormData(enabledCriteria) {
        var isValid = true;
        var formDataCriteria = [];
        var addFromDataCriteria = function (criteria) {
            var formData = criteria.getFormData();

            if (formData) {
                if (formData.isInvalid) {
                    isValid = false;
                }

                isValid && formDataCriteria.push(formData);
            }
        };

        addFromDataCriteria(otherControls);
        $.each(enabledCriteria, function (index, criteria) {
            addFromDataCriteria(criteria);
        });

        if (isValid) {
            return getRuleFormData(formDataCriteria);
        }
    }

    function getRuleFormData(formDataCriteria) {
        var enabledColumns = [];
        var addOrUpdateFormData = {};

        $.each(formDataCriteria, function (index, criteria) {
            if (criteria.name) {
                enabledColumns.push(criteria.name);
                delete criteria.name;
            }

            $.extend(addOrUpdateFormData, criteria);
        });

        addOrUpdateFormData.enabledColumns = enabledColumns.join(",");

        return addOrUpdateFormData;
    }

    this.openModal = function (modalTitle, rule) {
        var form = addOrUpdateForm.clone();
        CMS.modal(modalTitle, form, {
            height: "auto",
            width: "650px",
            resizable: false,
            position: {
                my: "center top",
                at: "center top",
                of: "#headline"
            }
        });

        init(form, rule);
    };
}