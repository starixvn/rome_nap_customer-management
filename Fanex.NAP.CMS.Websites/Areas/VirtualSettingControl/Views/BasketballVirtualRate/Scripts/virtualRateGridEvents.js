"use strict";
var virtualRateGridEvents = (function ($) {
    var followParentValue = "-200";
    var gridTitle;
    var tblReport;
    var tableBody;
    var iconCollapse;
    var btnViewCompactMode;
    var btnViewFullMode;
    var isAutoSuspendViewMode;
    var custId;
    var custName;
    var memberLevel;

    function bindEvents(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly) {
        custId = $("#txt-custId").val();
        custName = $("#txt-custname").val();
        memberLevel = $("#txt-report-level").val() === "1";

        gridTitle = $("#grid-title");
        tblReport = $("#tbl-report");
        tableBody = tblReport.find("tbody");

        $("#lnk-view-real-values")
            .off("click")
            .on("click", viewRealValues);

        $("#btn-export-excel")
            .off("click")
            .on("click", exportVirtualRateDetails);

        btnViewCompactMode = tblReport.find("#btn-view-compact-mode")
            .off("click")
            .on("click", function () {
                viewCompactMode(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly);
            });

        btnViewFullMode = tblReport.find("#btn-view-full-mode")
            .off("click")
            .on("click", function () {
                viewFullMode(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly);
            });

        iconCollapse = tblReport.find(".ico-collapse")
            .off("click")
            .on("click", function () {
                collapseReport();
            });

        tableBody.find(".group-toggle")
            .off("click")
            .on("click", function () {
                toggleGroup($(this));
            });

        tableBody.find(".icon-copy2child")
            .off("click")
            .on("click", function () {
                copyToChildren($(this));
            });

        tableBody.find(".icon-copy2related")
            .off("click")
            .on("click", function () {
                copyToRelated($(this));
            });

        if (memberLevel) {
            tableBody.find(".ddl-rate")
                .off("change")
                .on("change", function () {
                    rateChanged($(this));
                });

            tableBody.find(".icon-copy2related-autosuspend")
                .off("click")
                .on("click", function () {
                    autoSuspendCopyToRelated($(this));
                });

            tableBody.find(".icon-copy2child-autosuspend")
                .off("click")
                .on("click", function () {
                    autoSuspendCopyToChildren($(this));
                });

            isAutoSuspendViewMode = true;
        }
    }

    function viewRealValues(e) {
        e.preventDefault();
        CMS.getHtml(
            $(this).attr("href"),
            function (html) {
                if (html.length) {
                    CMS.nonResizableModal(resources.VirtualRateDetails, html, { height: 600 });

                    $("#btn-export-real-values")
                        .off("click")
                        .on("click", exportRealValues);

                    var tbodyRealValues = $("#tbl-real-values > tbody");
                    tbodyRealValues.find(".group-toggle")
                        .off("click")
                        .on("click", function () {
                            toggleGroup($(this), tbodyRealValues);
                        });
                } else {
                    CMS.showErrorMessage(resources.AccessDeniedMessage);
                }
            });
    }

    function exportRealValues() {
        CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/ExportRealValues"), { custId: custId });
    }

    function exportVirtualRateDetails() {
        CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/ExportVirtualRateDetails"), { username: custName });
    }

    function viewCompactMode(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly) {
        if (memberLevel) {
            gridTitle.removeClass("full-mode").addClass("compact-mode");
            tblReport.removeClass("full-mode").addClass("compact-mode");
            btnAutoSuspendView.addClass("display-none");
            btnAutoSuspendEdit.addClass("display-none");
            lblApplyVR_AS.addClass("display-none");
            lblApplyVROnly.removeClass("display-none");
        }

        hideFollowParentRowsLevel4();
        hideFollowParentRows(3);
        hideFollowParentRows(2);
        hideCopyToRelatedIcons();

        btnViewCompactMode.addClass("display-none");
        btnViewFullMode.removeClass("display-none");

        collapseReport(false);
    }

    function hideFollowParentRowsLevel4() {
        tableBody.find('tr[data-level="4"]').each(function () {
            var element = $(this);
            var isFollowParent = element.find('td[data-col="non-live"] .ddl-rate').val() === followParentValue
                && element.find('td[data-col="live"] .ddl-rate').val() === followParentValue;

            if (isFollowParent) {
                element.addClass("hide-compact-mode");
            }
        });
    }

    function hideFollowParentRows(level) {
        tableBody.find('tr[data-level="' + level + '"]').each(function () {
            var element = $(this);
            var isFollowParent = element.find('td[data-col="non-live"] .ddl-rate').val() === followParentValue
                && element.find('td[data-col="live"] .ddl-rate').val() === followParentValue;

            var gameId = element.data("gameid");
            var selector = 'tr[data-level="' + (level + 1) + '"][data-gameid="' + gameId + '"]';
            var betTypeGroupId = element.data("bettypegroupid");

            if (betTypeGroupId) {
                selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
            }

            selector += ":not(.hide-compact-mode)";

            if (!$(selector).length) {
                if (isFollowParent) {
                    element.addClass("hide-compact-mode");
                } else {
                    element.addClass("group-nonchild-compact-mode");
                }
            }
        });
    }

    function hideCopyToRelatedIcons() {
        tableBody.find("tr:not(.hide-compact-mode) .icon-copy2related").each(function () {
            var element = $(this);
            var row = element.closest("tr");
            var level = row.data("level");
            var selector = 'tr:not(.hide-compact-mode)[data-level="' + level + '"]';

            if (level === 2) {
                selector += '[data-gameid="' + row.data("gameid") + '"]';
            } else if (level === 3) {
                selector += '[data-bettypegroupid="' + row.data("bettypegroupid") + '"]';
            } else if (level === 4) {
                selector += '[data-bettypeid="' + row.data("bettypeid") + '"]';
            }

            if (tableBody.find(selector).length <= 1) {
                element.closest("td").addClass("no-repetition-compact-mode");
            }
        });
    }

    function viewFullMode(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly) {
        if (memberLevel) {
            gridTitle.removeClass("compact-mode").addClass("full-mode");
            tblReport.removeClass("compact-mode").addClass("full-mode");
            if (isAutoSuspendViewMode) {
                btnAutoSuspendEdit.removeClass("display-none");
            } else {
                btnAutoSuspendView.removeClass("display-none");
            }

            lblApplyVR_AS.removeClass("display-none");
            lblApplyVROnly.addClass("display-none");
        }

        tableBody.find(".hide-compact-mode").removeClass("hide-compact-mode");
        tableBody.find(".no-repetition-compact-mode").removeClass("no-repetition-compact-mode");

        tableBody.find(".group-nonchild-compact-mode .group-toggle").each(function () {
            $(this).closest("tr").removeClass("group-nonchild-compact-mode");
        });

        btnViewFullMode.addClass("display-none");
        btnViewCompactMode.removeClass("display-none");

        collapseReport(false);
    }

    function collapseReport(isCollapse) {
        var hideOrShowFunc;

        if (typeof isCollapse === "undefined") {
            isCollapse = iconCollapse.hasClass("ico-collapse");
        }

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
                var drow = element.closest("tr");

                if (isCollapse) {
                    element.removeClass("group-expand").addClass("group-collapse");
                } else {
                    element.removeClass("group-collapse").addClass("group-expand");
                }

                element.html(drow.hasClass("group-nonchild-compact-mode") ? "" : isCollapse ? "[+]" : "[−]");

                hideOrShowFunc(drow.data("sportid"), drow.data("gameid"));
            });
    }

    function toggleGroup(element, tbody) {
        var hideOrShowFunc;
        var isExpand = element.hasClass("group-expand");

        if (isExpand) {
            hideOrShowFunc = hideChildren;
            element.removeClass("group-expand").addClass("group-collapse").html("[+]");
        } else {
            hideOrShowFunc = showChilden;
            element.removeClass("group-collapse").addClass("group-expand").html("[−]");
        }

        var drow = element.closest("tr");
        hideOrShowFunc(drow.data("sportid"), drow.data("gameid"), drow.data("bettypegroupid"), tbody);

        tbody || checkCollapseExpandAll();
    }

    function hideChildren(sportId, gameId, betTypeGroupId, tbody) {
        tbody = tbody || tableBody;

        if (betTypeGroupId) {
            tbody
                .find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]')
                .addClass("display-none");
        } else {
            tbody.find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').addClass("display-none");
            tbody.find('tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').addClass("display-none");
        }
    }

    function showChilden(sportId, gameId, betTypeGroupId, tbody) {
        tbody = tbody || tableBody;

        if (betTypeGroupId) {
            tbody
                .find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]')
                .removeClass("display-none");
        } else {
            tbody.find('tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]').removeClass("display-none");
            tbody.find('tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]')
                .removeClass("display-none")
                .find(".group-toggle").each(function () {
                    var element = $(this);
                    element.removeClass("group-collapse").addClass("group-expand");

                    var text = element.closest("tr").hasClass("group-nonchild-compact-mode") ? "" : "[−]";
                    element.html(text);
                });
        }
    }

    function checkCollapseExpandAll() {
        var isCollapseAll = tableBody.find('tr[data-level="2"] .group-toggle.group-collapse').length === tableBody.find('tr[data-level="2"] .group-toggle').length;

        if (isCollapseAll) {
            iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            var isExpandAll = tableBody.find("tr .group-toggle.group-expand").length === tableBody.find("tr .group-toggle").length;

            if (isExpandAll) {
                iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
            }
        }
    }

    function copyToChildren(element) {
        var column = element.closest("td");
        var value = column.find(".ddl-rate").val();

        var row = element.closest("tr");
        var selector = 'tr:not(.hide-compact-mode)[data-sportid="' + row.data("sportid") + '"]';
        var gameId = row.data("gameid");

        if (gameId) {
            selector += '[data-gameid="' + gameId + '"]';

            var betTypeGroupId = row.data("bettypegroupid");

            if (betTypeGroupId) {
                selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
            }
        }

        selector += ' [data-col="' + column.data("col") + '"] .ddl-rate';
        tableBody.find(selector).val(value);
    }

    function copyToRelated(element) {
        var column = element.closest("td");
        var value = column.find(".ddl-rate").val();

        var row = element.closest("tr");
        var level = row.data("level");
        var selector = 'tr:not(.hide-compact-mode)[data-level="' + level + '"]';

        if (level === 1) {
            selector += '[data-sportid="' + row.data("sportid") + '"]';
        } else if (level === 2) {
            selector += '[data-gameid="' + row.data("gameid") + '"]';
        } else if (level === 3) {
            selector += '[data-bettypegroupid="' + row.data("bettypegroupid") + '"]';
        } else if (level === 4) {
            selector += '[data-bettypeid="' + row.data("bettypeid") + '"]';
        }

        selector += ' [data-col="' + column.data("col") + '"] .ddl-rate';
        tableBody.find(selector).val(value);
    }

    function autoSuspendEdit() {
        tblReport.removeClass("autosuspend-view").addClass("autosuspend-edit");
        isAutoSuspendViewMode = false;
    }

    function autoSuspendView() {
        tblReport.removeClass("autosuspend-edit").addClass("autosuspend-view");

        tableBody.find(".chk-autosuspend:not(.display-none)").each(function () {
            var column = $(this).closest("td");
            var iconNonAutosuspend = column.find(".icon-non-autosuspend");

            if (this.checked) {
                iconNonAutosuspend.addClass("display-none");
            } else {
                iconNonAutosuspend.removeClass("display-none");
            }
        });

        isAutoSuspendViewMode = true;
    }

    function rateChanged(element, threshold, value) {
        var row = element.closest("tr");
        var level = row.data("level");
        var column = element.closest("td");
        var colName = column.data("col");

        if (typeof threshold === "undefined") {
            threshold = parseInt(row.data(colName + "-threshold"));
        }

        if (typeof value === "undefined") {
            value = parseInt(column.find(".ddl-rate").val());
        }

        var showHideAutoSuspendControlFunc = value < threshold ? hideAutoSuspendControl : showAutoSuspendControl;
        showHideAutoSuspendControlFunc(row.find('.col-autosuspend[data-col="' + colName + '"]'))

        if (level < 4) {
            var selector = 'tr[data-level="' + (level + 1) + '"][data-sportid="' + row.data("sportid") + '"]';
            var gameId = row.data("gameid");

            if (gameId) {
                selector += '[data-gameid="' + gameId + '"]';

                var betTypeGroupId = row.data("bettypegroupid");

                if (betTypeGroupId) {
                    selector += '[data-bettypegroupid="' + betTypeGroupId + '"]';
                }
            }

            tableBody.find(selector).each(function () {
                var $this = $(this);
                var columns = $this.find('td[data-col="' + colName + '"]');
                var ddlRate = $(columns[0]).find(".ddl-rate");

                if (ddlRate.val() === followParentValue) {
                    rateChanged(ddlRate, threshold, value)
                }
            });
        }
    }

    function hideAutoSuspendControl(colAutoSuspend) {
        colAutoSuspend.find(".icon-non-autosuspend").addClass("display-none");
        colAutoSuspend.find(".elm-autosuspend-edit").addClass("display-none");
    }

    function showAutoSuspendControl(colAutoSuspend) {
        colAutoSuspend.find(".elm-autosuspend-edit").removeClass("display-none");

        if (!colAutoSuspend.find(".chk-autosuspend").is(":checked")) {
            colAutoSuspend.find(".icon-non-autosuspend").removeClass("display-none");
        }
    }

    function autoSuspendCopyToChildren(element) {
        var column = element.closest("td");
        var checked = column.find(".chk-autosuspend").is(":checked");

        var row = element.closest("tr");
        var selector = 'tr:not(.hide-compact-mode)[data-sportid="' + row.data("sportid") + '"]';
        var gameId = row.data("gameid");

        if (gameId) {
            selector += '[data-gameid="' + gameId + '"]';

            var betTypeGroupId = row.data("bettypegroupid");

            if (betTypeGroupId) {
                selector += '[data-bettypegroupid="' + row.data("bettypegroupid") + '"]';
            }
        }

        selector += ' [data-col="' + column.data("col") + '"]';
        tableBody.find(selector + " .chk-autosuspend").prop("checked", checked);
    }

    function autoSuspendCopyToRelated(element) {
        var column = element.closest("td");
        var checked = column.find(".chk-autosuspend").is(":checked");

        var row = element.closest("tr");
        var level = row.data("level");
        var selector = 'tr:not(.hide-compact-mode)[data-level="' + level + '"]';

        if (level === 1) {
            selector += '[data-sportid="' + row.data("sportid") + '"]';
        } else if (level === 2) {
            selector += '[data-gameid="' + row.data("gameid") + '"]';
        } else if (level === 3) {
            selector += '[data-bettypegroupid="' + row.data("bettypegroupid") + '"]';
        } else if (level === 4) {
            selector += '[data-bettypeid="' + row.data("bettypeid") + '"]';
        }

        selector += ' [data-col="' + column.data("col") + '"] .chk-autosuspend';
        tableBody.find(selector).prop("checked", checked);
    }

    function getSports() {
        var result = [];

        tableBody.find('tr[data-level="1"]').each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var sportId = element.data("sportid");
            var sportData = {
                GroupId: sportId,
                LevelId: element.data("levelid"),
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: getGames(sportId)
            };

            sportData = addIsAutoSuspendForMember(element, sportData);
            result.push(sportData);
        });

        return result;
    }

    function getGames(sportId) {
        var result = [];
        var selector = 'tr[data-level="2"][data-sportid="' + sportId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var gameId = element.data("gameid");
            var gameData = {
                GroupId: gameId,
                LevelId: element.data("levelid"),
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: getBetTypeGroups(sportId, gameId)
            };

            gameData = addIsAutoSuspendForMember(element, gameData);
            result.push(gameData);
        });

        return result;
    }

    function getBetTypeGroups(sportId, gameId) {
        var result = [];
        var selector = 'tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var betTypeGroupId = element.data("bettypegroupid");
            var betTypeGroupData = {
                GroupId: betTypeGroupId,
                LevelId: element.data("levelid"),
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: getBetTypes(sportId, gameId, betTypeGroupId)
            };

            betTypeGroupData = addIsAutoSuspendForMember(element, betTypeGroupData);
            result.push(betTypeGroupData);
        });

        return result;
    }

    function getBetTypes(sportId, gameId, betTypeGroupId) {
        var result = [];
        var selector = 'tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var betTypeData = {
                GroupId: element.data("bettypeid"),
                LevelId: element.data("levelid"),
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value
            };

            betTypeData = addIsAutoSuspendForMember(element, betTypeData);
            result.push(betTypeData);
        });

        return result;
    }

    function getRateValue(row, colName) {
        var ddlRate = row.find('td[data-col="' + colName + '"] .ddl-rate');

        return {
            id: ddlRate.find("option:selected").data("level-id"),
            value: ddlRate.val()
        };
    }

    function addIsAutoSuspendForMember(element, data) {
        if (memberLevel) {
            var nonliveAutoSuspend = getIsAutoSuspend(element, "non-live");
            data.NonliveAutoSuspend = nonliveAutoSuspend.value;
            data.DisableNonliveAutoSuspend = nonliveAutoSuspend.isDisable;

            var liveAutoSuspend = getIsAutoSuspend(element, "live");
            data.LiveAutoSuspend = liveAutoSuspend.value;
            data.DisableLiveAutoSuspend = liveAutoSuspend.isDisable;
        }

        return data;
    }

    function getIsAutoSuspend(row, colName) {
        var chkAutosuspend = row.find('td[data-col="' + colName + '"] .chk-autosuspend');
        var value = row.find('td[data-col="' + colName + '"] .ddl-rate');
        var threshold = parseInt(row.data(colName + "-threshold"));

        return {
            isDisable: isCompactMode() || (value < threshold),
            value: chkAutosuspend.is(":checked")
        };
    }

    function isSportsChanged(oldSports) {
        if (oldSports) {
            var newSports = getSports()

            return isRateChanged(oldSports, newSports)
                || isChildrenChanged(oldSports, newSports);
        }

        return false;
    }

    function isChildrenChanged(oldChildren, newChildren) {
        for (var i = 0, max = oldChildren.length; i < max; i++) {
            var oldChild = oldChildren[i];
            var newChild = newChildren[i];

            if (isRateChanged(oldChild, newChild)) {
                return true;
            }

            if (!oldChild.Childs || newChild.Childs.length === 0) {
                continue;
            }

            if (isChildrenChanged(oldChild.Childs, newChild.Childs)) {
                return true;
            }
        }

        return false;
    }

    function isRateChanged(oldRate, newRate) {
        return oldRate.NonLiveValue != newRate.NonLiveValue || oldRate.LiveValue != newRate.LiveValue;
    }

    function isCompactMode() {
        return btnViewCompactMode.hasClass("display-none");
    }

    function bindEventsGlobalSettings() {
        tblReport = $("#tbl-report");
        tableBody = tblReport.find("tbody");

        $("#btn-export-excel")
            .off("click")
            .on("click", exportVirtualRateGlobalSettings);

        btnViewCompactMode = tblReport.find("#btn-view-compact-mode")
            .off("click")
            .on("click", viewCompactMode);

        btnViewFullMode = tblReport.find("#btn-view-full-mode")
            .off("click")
            .on("click", viewFullMode);

        iconCollapse = tblReport.find(".ico-collapse")
            .off("click")
            .on("click", function () {
                collapseReport();
            });

        tableBody.find(".group-toggle")
            .off("click")
            .on("click", function () {
                toggleGroup($(this));
            });

        tableBody.find(".icon-copy2child")
            .off("click")
            .on("click", function () {
                copyToChildren($(this));
            });

        tableBody.find(".icon-copy2related")
            .off("click")
            .on("click", function () {
                copyToRelated($(this));
            });

        viewCompactMode()
    }

    function exportVirtualRateGlobalSettings() {
        CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/ExportGlobalSettings"), {});
    }

    function bindEventsGlobalAutoSuspendThreshold() {
        tblReport = $("#tbl-report");
        tableBody = tblReport.find("tbody");

        $("#btn-export-excel")
            .off("click")
            .on("click", exportGlobalAutoSuspendThreshold);

        iconCollapse = tblReport.find(".ico-collapse")
            .off("click")
            .on("click", function () {
                collapseReport();
            });

        tableBody.find(".group-toggle")
            .off("click")
            .on("click", function () {
                toggleGroup($(this));
            });

        tableBody.find(".icon-copy2child")
            .off("click")
            .on("click", function () {
                copyToChildren($(this));
            });

        tableBody.find(".icon-copy2related")
            .off("click")
            .on("click", function () {
                copyToRelated($(this));
            });
    }

    function exportGlobalAutoSuspendThreshold() {
        CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/ExportGlobalAutoSuspendThreshold"), {});
    }

    function getSportsGlobalAutoSuspendThreshold() {
        var result = [];

        tableBody.find('tr[data-level="1"]').each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var sportId = element.data("sportid");

            result.push({
                GroupId: sportId,
                LevelId: element.data("levelid"),
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getGamesGlobalAutoSuspendThreshold(sportId)
            });
        });

        return result;
    }

    function getGamesGlobalAutoSuspendThreshold(sportId) {
        var result = [];
        var selector = 'tr[data-level="2"][data-sportid="' + sportId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var gameId = element.data("gameid");

            result.push({
                GroupId: gameId,
                LevelId: element.data("levelid"),
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getBetTypeGroupsGlobalAutoSuspendThreshold(sportId, gameId)
            });
        });

        return result;
    }

    function getBetTypeGroupsGlobalAutoSuspendThreshold(sportId, gameId) {
        var result = [];
        var selector = 'tr[data-level="3"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");
            var betTypeGroupId = element.data("bettypegroupid");

            result.push({
                GroupId: betTypeGroupId,
                LevelId: element.data("levelid"),
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value,
                Childs: getBetTypesGlobalAutoSuspendThreshold(sportId, gameId, betTypeGroupId)
            });
        });

        return result;
    }

    function getBetTypesGlobalAutoSuspendThreshold(sportId, gameId, betTypeGroupId) {
        var result = [];
        var selector = 'tr[data-level="4"][data-sportid="' + sportId + '"][data-gameid="' + gameId + '"][data-bettypegroupid="' + betTypeGroupId + '"]';

        tableBody.find(selector).each(function () {
            var element = $(this);
            var nonlive = getRateValue(element, "non-live");
            var live = getRateValue(element, "live");

            result.push({
                GroupId: element.data("bettypeid"),
                LevelId: element.data("levelid"),
                NonliveAutoSuspendThreshold: nonlive.value,
                LiveAutoSuspendThreshold: live.value
            });
        });

        return result;
    }

    function isSportsChangedGlobalAutoSuspendThreshold(oldSports) {
        if (oldSports) {
            var newSports = getSportsGlobalAutoSuspendThreshold()

            return isRateChangedGlobalAutoSuspendThreshold(oldSports, newSports)
                || isChildrenChangedGlobalAutoSuspendThreshold(oldSports, newSports);
        }

        return false;
    }

    function isChildrenChangedGlobalAutoSuspendThreshold(oldChildren, newChildren) {
        for (var i = 0, max = oldChildren.length; i < max; i++) {
            var oldChild = oldChildren[i];
            var newChild = newChildren[i];

            if (isRateChangedGlobalAutoSuspendThreshold(oldChild, newChild)) {
                return true;
            }

            if (!oldChild.Childs || newChild.Childs.length === 0) {
                continue;
            }

            if (isChildrenChangedGlobalAutoSuspendThreshold(oldChild.Childs, newChild.Childs)) {
                return true;
            }
        }

        return false;
    }

    function isRateChangedGlobalAutoSuspendThreshold(oldRate, newRate) {
        return oldRate.NonliveAutoSuspendThreshold != newRate.NonliveAutoSuspendThreshold || oldRate.LiveAutoSuspendThreshold != newRate.LiveAutoSuspendThreshold;
    }

    return {
        bindEvents: bindEvents,
        autoSuspendView: autoSuspendView,
        autoSuspendEdit: autoSuspendEdit,
        getSports: getSports,
        isSportsChanged: isSportsChanged,
        getCustId: function () {
            return custId;
        },
        getCustName: function () {
            return custName;
        },
        isMemberLevel: function () {
            return memberLevel;
        },
        bindEventsGlobalSettings: bindEventsGlobalSettings,
        bindEventsGlobalAutoSuspendThreshold: bindEventsGlobalAutoSuspendThreshold,
        getSportsGlobalAutoSuspendThreshold: getSportsGlobalAutoSuspendThreshold,
        isSportsChangedGlobalAutoSuspendThreshold: isSportsChangedGlobalAutoSuspendThreshold
    };
})(jQuery);