"use strict";
(function ($, CMS, resources) {
    var oldReport = null;
    var iconCollapse = $("#ico-collapse");
    var tableBody = $("#tbl-virtual-rate-report").find("tbody");

    function renderReport(rs) {
        var html = [];

        $.each(rs.Sports, function (index, sport) {
            html.push(renderSport(sport, rs));
        });

        tableBody.html(html.join(""));
    }

    function renderSport(sport, rs) {
        var html = [];
        html.push(renderSportRow(sport, rs));

        for (var i = 0, max = sport.Childs.length; i < max; i++) {
            html.push(renderGame(sport, sport.Childs[i], rs));
        }

        return html.join("");
    }

    function renderSportRow(sport, rs) {
        var ddlNonlive = renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            value: sport.NonliveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: false,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + sport.GroupName + '&quot;'
        });

        var ddlLive = renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            value: sport.LiveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: false,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + sport.GroupName + '&quot;'
        });

        return [
            '<tr data-level="1" data-sportid="', sport.GroupId, '">',
            '<td data-col="groupname">', sport.GroupName, "</td>",
            '<td data-col="non-live">', ddlNonlive, "</td>",
            '<td data-col="live">', ddlLive, "</td></tr>"
        ].join("");
    }

    function renderGame(sport, game, rs) {
        var html = [];
        html.push(renderGameRow(sport, game, rs));

        for (var i = 0, max = game.Childs.length; i < max; i++) {
            html.push(renderBetTypeGroup(sport, game, game.Childs[i], rs));
        }

        return html.join("");
    }

    function renderGameRow(sport, game, rs) {
        var ddlNonlive = renderDropdownRate({
            minRate: game.MinRate,
            maxRate: game.MaxRate,
            value: game.NonliveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + game.GroupName + '&quot;'
        });

        var ddlLive = renderDropdownRate({
            minRate: game.MinRate,
            maxRate: game.MaxRate,
            value: game.LiveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + game.GroupName + '&quot;'
        });

        return [
            '<tr data-level="2" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '">',
            '<td data-col="groupname"><span class="group-toggle group-expand">[−]</span>', game.GroupName, "</td>",
            '<td data-col="non-live">', ddlNonlive, "</td>",
            '<td data-col="live">', ddlLive, "</td></tr>"
        ].join("");
    }

    function renderBetTypeGroup(sport, game, betTypeGroup, rs) {
        var html = [];
        html.push(renderBetTypeGroupRow(sport, game, betTypeGroup, rs));

        for (var i = 0, max = betTypeGroup.Childs.length; i < max; i++) {
            html.push(renderBetTypeRow(sport, game, betTypeGroup, betTypeGroup.Childs[i], rs));
        }

        return html.join("");
    }

    function renderBetTypeGroupRow(sport, game, betTypeGroup, rs) {
        var ddlNonlive = renderDropdownRate({
            minRate: betTypeGroup.MinRate,
            maxRate: betTypeGroup.MaxRate,
            value: betTypeGroup.NonliveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + betTypeGroup.GroupName + '&quot;'
        });

        var ddlLive = renderDropdownRate({
            minRate: betTypeGroup.MinRate,
            maxRate: betTypeGroup.MaxRate,
            value: betTypeGroup.LiveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + betTypeGroup.GroupName + '&quot;'
        });

        return [
            '<tr data-level="3" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '" data-bettypegroupid="', betTypeGroup.GroupId, '">',
            '<td data-col="groupname"><span class="group-toggle group-expand">[−]</span>', betTypeGroup.GroupName, "</td>",
            '<td data-col="non-live">', ddlNonlive, "</td>",
            '<td data-col="live">', ddlLive, "</td></tr>"
        ].join("");
    }

    function renderBetTypeRow(sport, game, betTypeGroup, betType, rs) {
        var ddlNonlive = renderDropdownRate({
            minRate: betType.MinRate,
            maxRate: betType.MaxRate,
            value: betType.NonliveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: false,
            copyToRelated: betType.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + betType.GroupName + '&quot;'
        });

        var ddlLive = renderDropdownRate({
            minRate: betType.MinRate,
            maxRate: betType.MaxRate,
            value: betType.LiveAutoSuspendThreshold,
            rateRanges: rs.VirtualRateValueRange,
            copyToChildren: false,
            copyToRelated: betType.HasRepetition,
            copyToRelatedText: resources.CopyToAll + ' &quot;' + betType.GroupName + '&quot;'
        });

        return [
            '<tr data-level="4" data-sportid="', sport.GroupId, '" data-gameid="', game.GroupId, '" data-bettypegroupid="', betTypeGroup.GroupId, '" data-bettypeid="', betType.GroupId, '">',
            '<td data-col="groupname">', betType.GroupName, "</td>",
            '<td data-col="non-live">', ddlNonlive, "</td >",
            '<td data-col="live">', ddlLive, "</td></tr>"
        ].join("");
    }

    function renderDropdownRate(options) {
        var html = options.copyToRelated
            ? ['<span class="icon-copy2related" title="', options.copyToRelatedText, '"></span>']
            : ['<span class="padding-icon-copy2related"></span>'];

        html.push('<select class="ddl-rate">');

        for (var i = 0, max = options.rateRanges.length; i < max; i++) {
            var range = options.rateRanges[i];

            if (range.Value < options.minRate || options.maxRate < range.Value) {
                continue;
            }

            var rangeText = numeral(range.Value).format("#,##0");
            html.push("<option");
            html.push(options.value === range.Value ? ' selected="selected"' : "");
            html.push(' value="');
            html.push(range.Value);
            html.push('" data-level-id="');
            html.push(range.Id);
            html.push('">');
            html.push(rangeText);
            html.push("</option>");
        }

        html.push("</select>");

        if (options.copyToChildren) {
            html.push('<span class="icon-copy2child" title="');
            html.push(resources.CopyToAllChildren);
            html.push('"></span>');
        }

        return html.join("");
    }

    function bindEvents() {
        iconCollapse.click(collapseReport);

        $(".group-toggle").click(function () {
            toggleGroup($(this));
        });

        $(".icon-copy2child").click(function () {
            copyToChildren($(this));
        });

        $(".icon-copy2related").click(function () {
            copyToRelated($(this));
        });

        $("#btn-export-excel").click(function () {
            CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/VirtualRateDetails/ExportGlobalAutoSuspendThreshold"), {});
        });

        $(".btn-submit-changes").click(function () {
            CMS.showConfirmMessage(resources.VirtualRateSubmitChangesConfirm, saveChanges);
        });
    }

    function collapseReport() {
        var hideOrShowFunc;
        var isCollapse = iconCollapse.hasClass("ico-collapse");

        if (isCollapse) {
            hideOrShowFunc = hideChildren;
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            hideOrShowFunc = showChilden;
            iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        tableBody
            .find('tr[data-level="2"] .group-toggle')
            .each(function () {
                var element = $(this);
                forceToggle(element, !isCollapse);
                hideOrShowFunc(2, element.parent().parent());
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
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");

        if (level === 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            tableBody
                .find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]')
                .addClass("display-none");
        } else {
            tableBody.find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').addClass("display-none");
            tableBody.find('tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').addClass("display-none");
        }
    }

    function showChilden(level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");

        if (level === 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            tableBody
                .find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]')
                .removeClass("display-none");
        } else {
            tableBody
                .find('tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"] .group-toggle')
                .each(function () {
                    forceToggle($(this), true);
                });

            tableBody.find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').removeClass("display-none");
            tableBody.find('tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').removeClass("display-none");
        }
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
        var isCollapseAll = tableBody.find('tr[data-level="2"] .group-toggle.group-collapse').length === tableBody.find('tr[data-level="2"] .group-toggle').length;

        if (isCollapseAll) {
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
            return;
        }

        var isExpandAll = tableBody.find("tr .group-toggle.group-expand").length === tableBody.find("tr .group-toggle").length;

        if (isExpandAll) {
            iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }
    }

    function copyToChildren(element) {
        var parent = element.parent().parent();
        var level = parent.data("level");

        var colName = element.parent().data("col");
        var value = parent.find('[data-col="' + colName + '"] .ddl-rate').val();
        var sportId = parent.data("sportid");

        var selector = 'tr:not(.hide-compact-mode)[data-sportid="' + sportId + '"]';

        if (level >= 2) {
            var gameId = parent.data("gameid");
            selector += '[data-gameid="' + gameId + '"]';
        }

        if (level >= 3) {
            var betTypeGroupId = parent.data("bettypegroupid");
            selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
        }

        selector += ' [data-col="' + colName + '"] .ddl-rate';
        tableBody.find(selector).val(value);
    }

    function copyToRelated(element) {
        var parent = element.parent().parent();
        var level = parent.data("level");
        var colName = element.parent().data("col");
        var value = parent.find('[data-col="' + colName + '"] .ddl-rate').val();

        var selector = 'tr:not(.hide-compact-mode)[data-level="' + level + '"]';

        switch (level) {
            case 1:
                var sportId = parent.data("sportid");
                selector += '[data-sportid="' + sportId + '"]';
                break;
            case 2:
                var gameId = parent.data("gameid");
                selector += '[data-gameid="' + gameId + '"]';
                break;
            case 3:
                var betTypeGroupId = parent.data("bettypegroupid");
                selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
                break;
            case 4:
                var betTypeId = parent.data("bettypeid");
                selector += '[data-bettypeid="' + betTypeId + '"]';
                break;
        }

        selector += ' [data-col="' + colName + '"] .ddl-rate';
        tableBody.find(selector).val(value);
    }

    function getSports(sports) {
        var result = [];

        for (var i = 0, max = sports.length; i < max; i++) {
            var sport = sports[i];
            var nonlive = getRateValue(1, "non-live", sport.GroupId);
            var live = getRateValue(1, "live", sport.GroupId);

            result.push({
                GroupId: sport.GroupId,
                LevelId: sport.LevelId,
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getGames(sport, sport.Childs)
            });
        }

        return result;
    }

    function getGames(sport, games) {
        var result = [];

        for (var i = 0, max = games.length; i < max; i++) {
            var game = games[i];
            var nonlive = getRateValue(2, "non-live", sport.GroupId, game.GroupId);
            var live = getRateValue(2, "live", sport.GroupId, game.GroupId);

            result.push({
                GroupId: game.GroupId,
                LevelId: game.LevelId,
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getBetTypeGroups(sport, game, game.Childs)
            });
        }

        return result;
    }

    function getBetTypeGroups(sport, game, betTypeGroups) {
        var result = [];

        for (var i = 0, max = betTypeGroups.length; i < max; i++) {
            var betTypeGroup = betTypeGroups[i];
            var nonlive = getRateValue(3, "non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId);
            var live = getRateValue(3, "live", sport.GroupId, game.GroupId, betTypeGroup.GroupId);

            result.push({
                GroupId: betTypeGroup.GroupId,
                LevelId: betTypeGroup.LevelId,
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getBetTypes(sport, game, betTypeGroup, betTypeGroup.Childs)
            });
        }

        return result;
    }

    function getBetTypes(sport, game, betTypeGroup, betTypes) {
        var result = [];

        for (var i = 0, max = betTypes.length; i < max; i++) {
            var betType = betTypes[i];
            var nonlive = getRateValue(4, "non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);
            var live = getRateValue(4, "live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);

            result.push({
                GroupId: betType.GroupId,
                LevelId: betType.LevelId,
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value
            });
        }

        return result;
    }

    function getRateValue(level, colName, sportId, gameId, betTypeGroupId, betTypeId) {
        var selector = 'tr[data-level="' + level + '"][data-sportid="' + sportId + '"]';

        if (level >= 2) {
            selector += '[data-gameid="' + gameId + '"]';
        }

        if (level >= 3) {
            selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
        }

        if (level >= 4) {
            selector += '[data-bettypeid="' + betTypeId + '"]';
        }

        selector += ' [data-col="' + colName + '"] .ddl-rate';
        var dropdown = tableBody.find(selector);

        return {
            id: dropdown.find("option:selected").data("level-id"),
            value: dropdown.val()
        };
    }

    function childrenChanges(oldChildren, currentChildren) {
        for (var i = 0, max = oldChildren.length; i < max; i++) {
            var oldChild = oldChildren[i];
            var currentChild = currentChildren[i];

            if (rateChanged(oldChild, currentChild)) {
                return true;
            }

            if (!oldChild.Childs || currentChild.Childs.length === 0) {
                continue;
            }

            if (childrenChanges(oldChild.Childs, currentChild.Childs)) {
                return true;
            }
        }

        return false;
    }

    function rateChanged(oldRate, newRate) {
        return oldRate.NonliveAutoSuspendThreshold != newRate.NonliveAutoSuspendThreshold || oldRate.LiveAutoSuspendThreshold != newRate.LiveAutoSuspendThreshold;
    }

    function saveChanges() {
        CMS.disableInput();
        var currentSports = getSports(oldReport.Sports);

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/VirtualRateDetails/UpdateGlobalAutoSuspendThreshold"),
            {
                Sports: currentSports
            },
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    oldReport.Sports = currentSports;
                    CMS.showSuccessMessage(jsonData.Message);
                } else {
                    CMS.showErrorMessage(jsonData.Message);
                }
            }
        );
    }

    $(document).ready(function () {
        CMS.disableInput();

        if (model.Sports.length) {
            oldReport = model;
            window.onbeforeunload = function () {
                if (oldReport) {
                    var currentSports = getSports(oldReport.Sports);

                    if (childrenChanges(model.Sports, currentSports)) {
                        return resources.VirtualRateDetails_LeaveSiteWarning;
                    }
                }
            };

            renderReport(model);
            bindEvents();
        } else {
            tableBody.html('<tr><td colspan="3" align="center">' + resources.Message_NoReportData + "</td></tr>");
        }

        CMS.enableInput();
    });
})(jQuery, CMS, resources);