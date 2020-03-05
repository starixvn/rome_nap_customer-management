"use strict";
(function ($, CMS, resources) {
    //var oldReport = null;
    var followParentValue = -200;
    var buttonViewCompactMode = $("#btn-view-compact-mode");
    var buttonViewFullMode = $("#btn-view-full-mode");
    var iconCollapse = $("#ico-collapse");
    var tableBody = $("#tbl-virtual-rate-report").find("tbody");

    function renderReport(rs, sportsLength) {
        //oldReport = rs;

        //this.funcGetBetTypes = self.getBetTypes;
        //this.funcGetRateValue = self.getRateValue;

        //this.elements.btnSubmitChanges.off("click touchend").on("click touchend", function () {
        //    self.saveChanges();
        //});
        //window.onbeforeunload = function () {
        //    if (oldReport == null) {
        //        return;
        //    }

        //    var currentValue = self.getCurrentValue();
        //    if (self.hasChanges(oldReport, currentValue)) {
        //        return resources.VirtualRateDetails_LeaveSiteWarning;
        //    }
        //};

        var html = [];
        for (var i = 0; i < sportsLength; i++) {
            html.push(renderSport(rs.Sports[i], rs));
        }

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
            id: sport.NonliveId,
            value: sport.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: "",
            copyToRelatedText: resources.CopyToAll + " \"" + sport.GroupName + "\""
        });

        var ddlLive = renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            id: sport.LiveId,
            value: sport.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: "",
            copyToRelatedText: resources.CopyToAll + " \"" + sport.GroupName + "\""
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
            id: sport.NonliveId,
            value: game.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + game.GroupName + "\""
        });

        var ddlLive = renderDropdownRate({
            minRate: game.MinRate,
            maxRate: game.MaxRate,
            id: sport.LiveId,
            value: game.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + game.GroupName + "\""
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
            id: sport.NonliveId,
            value: betTypeGroup.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + betTypeGroup.GroupName + "\""
        });

        var ddlLive = renderDropdownRate({
            minRate: betTypeGroup.MinRate,
            maxRate: betTypeGroup.MaxRate,
            id: sport.LiveId,
            value: betTypeGroup.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + betTypeGroup.GroupName + "\""
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
            id: sport.NonliveId,
            value: betType.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: false,
            copyToRelated: betType.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + betType.GroupName + "\""
        });

        var ddlLive = renderDropdownRate({
            minRate: betType.MinRate,
            maxRate: betType.MaxRate,
            id: sport.LiveId,
            value: betType.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: false,
            copyToRelated: betType.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: resources.CopyToAll + " \"" + betType.GroupName + "\""
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

        if (options.followParentText) {
            html.push("<option");
            html.push(options.id === 0 ? ' selected="selected"' : "");
            html.push(' value="');
            html.push(followParentValue);
            html.push('" data-level-id="0">');
            html.push(options.followParentText);
            html.push("</option>");
        }

        for (var i = 0, max = options.rateRanges.length; i < max; i++) {
            var range = options.rateRanges[i];

            if (range.Value < options.minRate || options.maxRate < range.Value) {
                continue;
            }

            var rangeText = numeral(range.Value).format("#,##0");
            html.push("<option");
            html.push(options.value === range.Value ? " selected='selected'" : "");
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

    function bindEventsAfterRenderSuccessfully() {
        buttonViewCompactMode.click(viewCompactMode);
        buttonViewFullMode.click(viewFullMode);

        iconCollapse.click(collapseReport);

        $(".group-toggle").click(function () {
            toggleGroup($(this));
        });

        //$(".icon-copy2child").on("click", function () {
        //    self.copyToChildren($(this));
        //});

        //$(".icon-copy2related").on("click", function () {
        //    self.copyToRelated($(this));
        //});

        //this.elements.divAssociatedAccounts.attr("disabled", "disabled");
        //this.elements.chkAssociatedAccounts.attr("checked", null);
        //this.elements.txtAssociatedAccounts.val("");
        //this.enableAssociatedAccounts();

        //this.elements.reportContainer.removeClass("display-none");

        //this.elements.btnViewCompactMode.click();

        $("#btn-export-excel").click(function () {
            var param = {
                /*username: username,
                sportType: resources.SportType,
                IsGlobalSetting: resources.isGlobalSetting*/
            };

            CMS.downloadFile(CMS.resolveRootUrl("VirtualRateDetails/ExportGlobalAutoSuspendThreshold"), param);
        });

        $(".btn-submit-changes").click(function () {
            alert(1);
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
                hideOrShowFunc(2, element.parent().parent())
            });
    }

    function viewCompactMode() {
        for (var i = 4; i >= 2; i--) {
            hideFollowParentRows(i);
        }

        tableBody
            .find("tr:not(.hide-compact-mode) .icon-copy2related")
            .each(function () {
                var element = $(this);

                var row = element.parent().parent();
                var level = row.data("level");
                var selector = "";

                switch (level) {
                    case 2:
                        var gameId = row.data("gameid");
                        selector = 'tr[data-level="2"][data-gameid="' + gameId + '"]';
                        break;
                    case 3:
                        var betTypeGroupId = row.data("bettypegroupid");
                        selector = 'tr[data-level="3"][data-bettypegroupid="' + betTypeGroupId + '"]';
                        break;
                    case 4:
                        var betTypeId = row.data("bettypeid");
                        selector = 'tr[data-level="4"][data-bettypeid="' + betTypeId + '"]';
                        break;
                }

                selector += ":not(.hide-compact-mode)";

                if (tableBody.find(selector).length <= 1) {
                    element.parent().addClass("no-repetition-compact-mode");
                }
            });

        buttonViewCompactMode.addClass("display-none");
        buttonViewFullMode.removeClass("display-none");
    }

    function hideFollowParentRows(layerId) {
        tableBody
            .find('tr[data-level="' + layerId + '"]')
            .each(function () {
                var element = $(this);
                var isFollowParent = element.find('td[data-col="non-live"] .ddl-rate').val() == followParentValue &&
                    element.find('td[data-col="live"] .ddl-rate').val() == followParentValue;

                var sportId = element.attr("data-sportid");
                var gameId = element.attr("data-gameid");
                var betTypeGroupId = element.attr("data-bettypegroupid");
                var betTypeId = element.attr("data-bettypegroupid");

                var selector = 'tr[data-level="' + (layerId + 1) + '"][data-sportid="' + sportId + '"]';

                if (layerId >= 2) {
                    selector += '[data-gameid="' + gameId + '"]';
                }

                if (layerId >= 3) {
                    selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
                }

                if (layerId >= 4) {
                    selector += '[data-bettypeid="' + betTypeId + '"]';
                }

                selector += ":not(.hide-compact-mode)";
                var selectorLength = tableBody.find(selector).length;

                if (isFollowParent && selectorLength === 0) {
                    element.addClass("hide-compact-mode");
                } else if (selectorLength === 0) {
                    var toggle = element.find(".group-toggle");
                    toggle.attr("data-original-text", toggle.html()).html("&nbsp;");
                    element.addClass("group-nonchild-compact-mode");
                }
            });
    }

    function viewFullMode() {
        $(".hide-compact-mode").removeClass("hide-compact-mode");
        $(".no-repetition-compact-mode").removeClass("no-repetition-compact-mode");

        tableBody
            .find(".group-nonchild-compact-mode .group-toggle")
            .each(function () {
                var element = $(this);
                element.html(elem.attr("data-original-text"));
                element.parent().parent().removeClass("group-nonchild-compact-mode");
            });

        buttonViewFullMode.addClass("display-none");
        buttonViewCompactMode.removeClass("display-none");
    }

    $(document).ready(function () {
        var sportLength = model.Sports.length;

        if (!sportLength) {
            tableBody.html('<tr><td colspan="3" align="center">' + resources.Message_NoReportData + "</td></tr>");
            return;
        }

        CMS.disableInput();

        renderReport(model, sportLength);
        bindEventsAfterRenderSuccessfully();
        viewCompactMode();

        CMS.enableInput();
    });
})(jQuery, CMS, resources);