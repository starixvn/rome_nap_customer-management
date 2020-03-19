"use strict";
(function ($, CMS, resources) {
    var fromtoDatePicker = $("#fromtodate-picker");
    var reportContainer = $("#report-container");
    var reportTitle = $("#report-tile");
    var tableBody = $("#tbl-virtual-rate-report").find("tbody");
    var iconCollapse = $("#ico-collapse");

    function initDateRange() {
        fromtoDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: null,
                maxDate: null
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: resources.From,
                InitDate: initFromDate,
                CurrentDate: currentDate,
                LabelCss: "lbl-field col_2",
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: resources.To,
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: "lbl-field col_2",
                TextboxCss: "edit-field col_5"
            }
        });
        $("#dpk-fromdate").datepicker("setDate", initFromDate);
    }

    function renderReport(data, formData) {
        var html = [];
        var hasData = data.length;

        if (hasData) {
            $.each(data, function (index, log) {
                html.push(renderLog(log));
            });
        } else {
            html.push('<tr><td colspan="3" align="center">');
            html.push(resources.Message_NoReportData);
            html.push("</td></tr>");
        }

        reportTitle.html(buildReportTitle(formData));
        tableBody.html(html.join(""));

        if (hasData) {
            bindEvents();
            collapseReport(true);
        }

        reportContainer.removeClass("display-none");
    }

    function renderLog(log) {
        var html = [];
        html.push(renderLogRow(log));

        $.each(log.Sports, function (index, sport) {
            html.push(renderSport(log, sport));
        });

        return html.join("");
    }

    function renderLogRow(log) {
        return [
            '<tr data-level="0" data-logid="', log.LogId, '">',
            '<td colspan="3"><span class="group-toggle group-expand">[−]</span>',
            log.LogTimeString,
            " - ",
            resources.UpdateGlobalAutoSuspendThreshold,
            " - By ",
            log.Updatedby, "</td ></tr > "
        ].join("");
    }

    function renderSport(log, sport) {
        var html = [];
        html.push(renderSportRow(log, sport));

        $.each(sport.Childs, function (index, game) {
            html.push(renderGame(log, sport, game));
        });

        return html.join("");
    }

    function renderSportRow(log, sport) {
        return [
            '<tr data-level="', sport.GroupLevel, '" data-logid="', log.LogId, '" data-sportid="', sport.GroupId, '">',
            '<td data-col="groupname"><span class="group-toggle group-expand">[−]</span>', sport.GroupName, "</td>",
            renderThreshold(sport.NonliveAutoSuspendThreshold, sport.NonliveAutoSuspendThresholdOld),
            renderThreshold(sport.LiveAutoSuspendThreshold, sport.LiveAutoSuspendThresholdOld),
            "</tr>"
        ].join("");
    }

    function renderGame(log, sport, game) {
        var html = [];
        html.push(renderGameRow(log, sport, game));

        $.each(game.Childs, function (index, betTypeGroup) {
            html.push(renderBetTypeGroup(log, sport, game, betTypeGroup));
        });

        return html.join("");
    }

    function renderGameRow(log, sport, game) {
        return [
            '<tr data-level="', game.GroupLevel, '" data-logid="', log.LogId, '" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '">',
            '<td data-col="groupname"><span class="group-toggle group-expand">[−]</span>', game.GroupName, "</td>",
            renderThreshold(game.NonliveAutoSuspendThreshold, game.NonliveAutoSuspendThresholdOld),
            renderThreshold(game.LiveAutoSuspendThreshold, game.LiveAutoSuspendThresholdOld),
            "</tr>"
        ].join("");
    }

    function renderBetTypeGroup(log, sport, game, betTypeGroup) {
        var html = [];
        html.push(renderBetTypeGroupRow(log, sport, game, betTypeGroup));

        $.each(betTypeGroup.Childs, function (index, betType) {
            html.push(renderBetTypeRow(log, sport, game, betTypeGroup, betType));
        });

        return html.join("");
    }

    function renderBetTypeGroupRow(log, sport, game, betTypeGroup) {
        return [
            '<tr data-level="', betTypeGroup.GroupLevel, '" data-logid="', log.LogId, '" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '" data-bettypegroupid="', betTypeGroup.GroupId, '">',
            '<td data-col="groupname"><span class="group-toggle group-expand">[−]</span>', betTypeGroup.GroupName, "</td>",
            renderThreshold(betTypeGroup.NonliveAutoSuspendThreshold, betTypeGroup.NonliveAutoSuspendThresholdOld),
            renderThreshold(betTypeGroup.LiveAutoSuspendThreshold, betTypeGroup.LiveAutoSuspendThresholdOld),
            "</tr>"
        ].join("");
    }

    function renderBetTypeRow(log, sport, game, betTypeGroup, betType) {
        return [
            '<tr data-level="', betType.GroupLevel, '" data-logid="', log.LogId, '" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '" data-bettypegroupid="', betTypeGroup.GroupId, '" data-bettypeid="', betType.GroupId, '">',
            '<td data-col="groupname">', betType.GroupName, "</td>",
            renderThreshold(betType.NonliveAutoSuspendThreshold, betType.NonliveAutoSuspendThresholdOld),
            renderThreshold(betType.LiveAutoSuspendThreshold, betType.LiveAutoSuspendThresholdOld),
            "</tr>"
        ].join("");
    }

    function renderThreshold(value, oldValue) {
        return value === oldValue
            ? ("<td>" + value + "</td>")
            : ('<td class="has-changes"><div class="new-value">' + value + '</div><div class="old-value">' + oldValue + "</div></td>");
    }

    function buildReportTitle(formData) {
        return [
            resources.ReportTitle,
            " - ",
            resources.From,
            " ",
            moment(formData.from).format("MMM DD, YYYY"),
            " ",
            resources.To,
            " ",
            moment(formData.to).format("MMM DD, YYYY")
        ].join("");
    }

    function bindEvents() {
        $(".group-toggle")
            .off("click").
            on("click", function () {
                toggleGroup($(this));
            });
    }

    function toggleGroup(element) {
        var isExpand = element.hasClass("group-expand");
        var parent = element.parent().parent();
        var level = parseInt(parent.attr("data-level"));

        if (isExpand) {
            hideChildren(level, parent);
            element.removeClass("group-expand").addClass("group-collapse").html("[+]");
        } else {
            showChilden(level, parent);
            element.removeClass("group-collapse").addClass("group-expand").html("[−]");
        }

        checkCollapseExpandAll();
    }

    function hideChildren(level, parent) {
        var selector = getRowSelector(parent, level);

        for (var i = level + 1; i <= 4; i++) {
            $('tr[data-level="' + i + '"]' + selector).addClass("display-none");
        }
    }

    function showChilden(level, parent) {
        var selector = getRowSelector(parent, level);

        for (var i = level + 1; i <= 4; i++) {
            $('tr[data-level="' + i + '"]' + selector).removeClass("display-none");

            tableBody
                .find('tr[data-level="' + i + '"]' + selector + " .group-toggle")
                .each(function () {
                    forceToggle($(this), true);
                });
        }
    }

    function getRowSelector(row, level) {
        var logId = row.attr("data-logid");
        var selector = '[data-logid="' + logId + '"]';

        if (level >= 1) {
            var sportId = row.attr("data-sportid");
            selector += '[data-sportid="' + sportId + '"]';
        }

        if (level >= 2) {
            var gameId = row.attr("data-gameid");
            selector += '[data-gameid="' + gameId + '"]';
        }

        if (level >= 3) {
            var betTypeGroupId = row.attr("data-bettypegroupid");
            selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
        }

        return selector;
    }

    function forceToggle(element, isExpand) {
        if (isExpand) {
            element.removeClass("group-collapse").addClass("group-expand");
        } else {
            element.removeClass("group-expand").addClass("group-collapse");
        }

        var text = element.parent().parent().hasClass("group-nonchild-compact-mode") ? "" : isExpand ? "[−]" : "[+]";
        element.html(text);
    }

    function checkCollapseExpandAll() {
        var isCollapseAll = tableBody.find('tr[data-level="0"] .group-toggle.group-collapse').length === tableBody.find('tr[data-level="0"] .group-toggle').length;

        if (isCollapseAll) {
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
            return;
        }

        var isExpandAll = tableBody.find("tr .group-toggle.group-expand").length === tableBody.find("tr .group-toggle").length;

        if (isExpandAll) {
            iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }
    }

    function collapseReport(isCollapse) {
        var showHideChildrenFunc;
        isCollapse = isCollapse || iconCollapse.hasClass("ico-collapse");

        if (isCollapse) {
            showHideChildrenFunc = hideChildren;
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            showHideChildrenFunc = showChilden;
            iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        tableBody
            .find('tr[data-level="0"] .group-toggle')
            .each(function () {
                var elem = $(this);
                forceToggle(elem, !isCollapse);
                showHideChildrenFunc(0, elem.parent().parent());
            });
    }

    $(document).ready(function () {
        initDateRange();

        var formViewLog = $("#formViewLog").submit(function (event) {
            event.preventDefault();

            var dateRange = fromtoDatePicker.nexDatePicker("getDateRange");
            var formData = {
                sportType: 43,
                from: dateRange.FromDate,
                to: dateRange.ToDate
            };

            CMS.getJson(
                formViewLog.attr("action") + "?" + $.param(formData),
                function (data) {
                    renderReport($.parseJSON(data), formData);
                });
        });

        iconCollapse.click(function () {
            collapseReport();
        });
    });
})(jQuery, CMS, resources);