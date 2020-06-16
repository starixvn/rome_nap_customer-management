"use strict";
var virtualRateLogGridEvents = (function ($) {
    var tblReport;
    var iconCollapse;
    var maxLevel;

    function bindEvents(defaultMaxLevel) {
        maxLevel = defaultMaxLevel;

        tblReport = $("#tbl-report");
        iconCollapse = tblReport.find("#ico-collapse")
            .off("click")
            .on("click", collapseReport);

        tblReport
            .find("tr")
            .off("click")
            .on("click", function () {
                toggleGroup($(this))
            });
    }

    function collapseReport() {
        var hideOrShowFunc;
        var isCollapse = iconCollapse.hasClass("ico-collapse");

        if (isCollapse) {
            hideOrShowFunc = hideChildren;
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            hideOrShowFunc = showChildren;
            iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        $('tr[data-level="0"] .group-toggle').each(function () {
            var element = $(this);
            forceToggle(element, !isCollapse);
            hideOrShowFunc(element.closest("tr"), 0);
        });
    }

    function toggleGroup(element) {
        var groupToggle = element.find(".group-toggle");
        var isExpand = groupToggle.hasClass("group-expand");
        var level = element.data("level");

        if (isExpand) {
            hideChildren(element, level);
            groupToggle
                .removeClass("group-expand")
                .addClass("group-collapse")
                .html("[+]");
        } else {
            showChildren(element, level);
            groupToggle
                .removeClass("group-collapse")
                .addClass("group-expand")
                .html("[−]");
        }

        checkCollapseExpandAll();
    }

    function hideChildren(element, level) {
        var selector = getRowSelector(element);

        for (var i = level + 1; i <= maxLevel; i++) {
            tblReport.find('tr[data-level="' + i + '"]' + selector).addClass("display-none");
        }
    }

    function showChildren(element, level) {
        var selector = getRowSelector(element);

        for (var i = level + 1; i <= maxLevel; i++) {
            tblReport.find('tr[data-level="' + i + '"]' + selector).removeClass("display-none");
            var groupToggles = tblReport.find('tr[data-level="' + i + '"]' + selector + " .group-toggle");

            for (var j = 0, max = groupToggles.length; j < max; j++) {
                forceToggle($(groupToggles[j]), true);
            }
        }
    }

    function checkCollapseExpandAll() {
        var isCollapseAll = tblReport.find('tr[data-level="0"] .group-toggle.group-collapse').length === tblReport.find('tr[data-level="0"] .group-toggle').length;

        if (isCollapseAll) {
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            var isExpandAll = tblReport.find("tr .group-toggle.group-expand").length === tblReport.find("tr .group-toggle").length;

            if (isExpandAll) {
                iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
            }
        }
    }

    function getRowSelector(element) {
        var logId = element.data("log-id");
        var selector = '[data-log-id="' + logId + '"]';
        var sportId = element.data("sport-id");

        if (sportId) {
            selector += '[data-sport-id="' + sportId + '"]';

            var leagueId = element.data("league-id");

            if (leagueId) {
                selector += '[data-league-id="' + leagueId + '"]';

                var betTypeGroupId = element.data("bet-type-group-id");

                if (betTypeGroupId) {
                    selector += '[data-bet-type-group-id="' + betTypeGroupId + '"]';
                }
            }
        }

        return selector;
    }

    function forceToggle(element, isExpand) {
        if (isExpand) {
            element
                .removeClass("group-collapse")
                .addClass("group-expand")
                .html("[−]");
        } else {
            element
                .removeClass("group-expand")
                .addClass("group-collapse")
                .html("[+]");
        }
    }

    return { bindEvents: bindEvents };
})(jQuery);