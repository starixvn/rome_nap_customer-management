"use strict";

const policyManagementHelper = (function () {
    function renderCriteriaCell(rule, showEnableCriteria) {
        return [
            renderCustomerClass(rule, showEnableCriteria),
            renderSportTypeLeagueGroupIsLive(rule),
            renderDiffBalanceRatioBetDelay(rule),
            renderSeparateRuleSettings(resources.nextSuperSiteBetIntervalSec, rule.NextBetInterval, rule.SuperBetInterval, rule.SiteBetInterval),
            renderSeparateRuleSettings(resources.minMaxBetMPM, rule.MinBet, rule.MaxBet, rule.MaxPerMatch),
            renderRepeatedBetsSeizeBetDisableParlay(rule),
            //renderPeakHourSpread(rule)
        ].join("");
    }

    function renderCustomerClass(rule, showEnableCriteria) {
        let html = [renderRuleSetting(resources.customerClass, rule.CustomerClass)];

        if (showEnableCriteria) {
            html.push(renderRuleSetting(resources.enable, showEnableValue(rule.Enable)));
        }

        return `<div>${html.join(", ")}</div>`;
    }

    function renderSportTypeLeagueGroupIsLive(rule) {
        return [
            '<div>',
            renderRuleSetting(resources.sportType, rule.SportTypeName), ', ',
            renderRuleSetting(resources.leagueGroup, rule.IsAllLeagueGroups ? resources.all : rule.LeagueGroups.split(",").join(", ")), ', ',
            renderRuleSetting(resources.isLive, showIsLiveValue(rule.IsLive)),
            '</div>'
        ].join("");
    }

    function showIsLiveValue(isLive) {
        return !isLive
            ? resources.no
            : isLive === 1
                ? resources.yes
                : resources.all;
    }

    function renderDiffBalanceRatioBetDelay(rule) {
        let html = [];
        if (typeof rule.DiffBalanceRatio !== "undefined") {
            html.push(renderRuleSetting(resources.diffBalanceRatio, formatInteger(rule.DiffBalanceRatio) + "%"));
        }

        if (typeof rule.BetDelay !== "undefined") {
            html.push(renderRuleSetting(resources.betDelay, formatInteger(rule.BetDelay) + "%"));
        }

        return html.length
            ? `<div>${html.join(", ")}</div>`
            : "";
    }

    function renderSeparateRuleSettings(text, value1, value2, value3) {
        return typeof value1 !== "undefined" || typeof value2 !== "undefined" || typeof value3 !== "undefined"
            ? ['<div>', text, ": ",
                formatSeparate(value1, "text-green"), " | ",
                formatSeparate(value2, "text-red"), " | ",
                formatSeparate(value3, "text-blue"), '</div>'].join("")
            : "";
    }

    function formatSeparate(value, className) {
        return `<strong class="${className}">${typeof value !== "undefined" ? formatInteger(value) : "--"}</strong>`;
    }

    function renderRepeatedBetsSeizeBetDisableParlay(rule) {
        let html = [];
        if (typeof rule.RepeatedBets !== "undefined") {
            html.push(renderRuleSetting(resources.repeatedBets, rule.RepeatedBets));
        }

        if (typeof rule.SeizeBet !== "undefined") {
            html.push(renderRuleSetting(resources.seizeBet, rule.SeizeBet + "%"));
        }

        html.push(renderRuleSetting(resources.disableParlay, showEnableValue(rule.DisableParlay)));

        return `<div>${html.join(", ")}</div>`;
    }

    function showEnableValue(value) {
        return value ? resources.yes : resources.no;
    }

    //function renderPeakHourSpread(rule) {
    //    return typeof rule.PeakHourSpread !== "undefined"
    //        ? `<div>${renderRuleSetting(resources.peakHourSpread, formatInteger(rule.PeakHourSpread))}</div>`
    //        : "";
    //}

    function renderRuleSetting(text, value) {
        return `${text}: <strong>${value}</strong>`;
    }

    function formatInteger(value) {
        return numeral(value).format(CMS.formats.IntegerFormat);
    }

    return {
        renderCriteriaCell: renderCriteriaCell
    };
})();