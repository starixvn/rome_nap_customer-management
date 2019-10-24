VirtualRateDetailsIndex = function () {
    var self = this;
    this.elements = {
        txtUsername: null,
        reportContainer: null,

        reportTitle: null,
        btnSubmitChanges: null,
        lnkViewUpline: null,
        lnkViewRealValues: null,
        btnExportExcel: null,

        btnViewCompactMode: null,
        btnViewFullMode: null,
        chkNonLive: null,
        chkLive: null,

        tblVirtualRateReport: null,
        iconCollapse: null,

        divAssociatedAccounts: null,
        chkAssociatedAccounts: null,
        btnAddAssociatedAccounts: null,
        txtAssociatedAccounts: null,
        btnSubmitAssociatedAccounts: null
    };
    this.oldReport = null;
    this.followParentValue = -200;
    this.realValueCustId = 0;
    this.isFirstSearch = false;

    this.init = function () {
        this.cachedElements();
        this.bindEvents();
        if (resources.isGlobalSetting) {
            this.getVirtualRateDetails(undefined, 0);
        } else if (username != "") {
            this.elements.txtUsername.val(username);
            this.getVirtualRateDetails(username);
        }
    };

    this.cachedElements = function () {
        this.elements.txtUsername = $("#txt-username");
        this.elements.reportContainer = $("#report-container");

        this.elements.reportTitle = $("#report-tile");
        this.elements.btnSubmitChanges = $(".btn-submit-changes");
        this.elements.lnkViewUpline = $("#lnk-view-upline");
        this.elements.lnkViewRealValues = $("#lnk-view-real-values");
        this.elements.btnExportExcel = $("#btn-export-excel");

        this.elements.btnViewCompactMode = $("#btn-view-compact-mode");
        this.elements.btnViewFullMode = $("#btn-view-full-mode");
        this.elements.chkNonLive = $("#chk-nonlive");
        this.elements.chkLive = $("#chk-live");

        this.elements.tblVirtualRateReport = $("#tbl-virtual-rate-report");
        this.elements.iconCollapse = $("#ico-collapse");

        this.elements.divAssociatedAccounts = $(".div-associated-accounts");
        this.elements.chkAssociatedAccounts = $("#chk-apply-setting");
        this.elements.btnAddAssociatedAccounts = $("#btn-add-associated-accounts");
        this.elements.txtAssociatedAccounts = $("#txt-associated-accounts");
        this.elements.btnSubmitAssociatedAccounts = $("#btn-submit-associated-accounts");
    };

    this.bindEvents = function () {
        $("#btn-search").on("click touchend", function () { self.enterSearch(); });

        this.elements.btnSubmitChanges.on("click touchend", function () {
            self.saveChanges();
        });

        this.elements.btnExportExcel.on("click touchend", function () {
            self.exportExcel($(this).attr("username"));
        });
        this.elements.lnkViewUpline.on("click touchend", function () {
            self.viewUpline($(this).attr("custname"));
        });
        this.elements.lnkViewRealValues.on("click touchend", function () {
            self.viewRealValues($(this).attr("cust-id"));
        });
        this.elements.btnViewCompactMode.on("click touchend", function () { self.viewCompactMode(); });
        this.elements.btnViewFullMode.on("click touchend", function () { self.viewFullMode(); });
        this.elements.chkNonLive.on("click touchend", function () {
            self.checkNonLive();
        });
        this.elements.chkLive.on("click touchend", function () {
            self.checkLive();
        });

        this.elements.iconCollapse.on("click touchend", function () {
            self.collapseReport();
        });

        this.elements.chkAssociatedAccounts.on("click touchend", function () {
            self.enableAssociatedAccounts();
        });

        this.elements.txtAssociatedAccounts.on("blur", function () {
            self.formatAssociatedAccounts();
        });

        window.onbeforeunload = function () {
            if (self.oldReport == null) {
                return;
            }

            var currentValue = self.getCurrentValue();
            if (self.hasChanges(self.oldReport, currentValue)) {
                return resources.VirtualRateDetails_LeaveSiteWarning;
            }
        };
        this.elements.txtUsername.on("keyup", function (e) {
            var code = e.keyCode || e.which;
            if (code === keyboardManager.Enums.KeyCode.Enter) {
                self.enterSearch();
            }
        });
    };
    this.exportExcel = function (username) {
        CMS.downloadFile(CMS.resolveRootUrl("VirtualRateDetails/ExportVirtualRateDetails"), { username: username, sportType: resources.SportType });
    };
    this.checkNonLive = function () {
        this.updateEnableNonlive();
    };
    this.checkLive = function () {
        this.updateEnableLive();
    };

    this.enterSearch = function () {
        var username = this.elements.txtUsername.val().trim();
        if (username == "") {
            this.elements.reportContainer.addClass("display-none");
            Validator.showError(this.elements.txtUsername, resources.RequiredField, "n");
            return;
        }

        this.getVirtualRateDetails(username);
    };
    this.getVirtualRateDetails = function (username, custId) {
        Validator.clearError(self.elements.txtUsername);
        CMS.closeMsgBox();
        this.isFirstSearch = true;

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/GetVirtualRateDetails"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ username: username, sportType: resources.SportType, custId: custId }),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                if (rs.ResultCode != 0) {
                    self.elements.reportContainer.addClass("display-none");
                    Validator.showError(self.elements.txtUsername, rs.ResultMessage, "n");
                    return;
                }

                self.renderReport(rs);
                if (!self.isGlobalSetting()) {
                    self.viewFullMode();
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    this.renderReport = function (rs) {
        this.oldReport = rs;

        if (this.isGlobalSetting()) {
            this.elements.lnkViewUpline.addClass("display-none");
            this.elements.lnkViewRealValues.addClass("display-none");
            this.elements.divAssociatedAccounts.addClass("display-none");
        } else {
            this.elements.lnkViewUpline.attr("custname", rs.UplineCustName).removeClass("display-none");
            this.elements.lnkViewRealValues.attr("cust-id", rs.CustId).removeClass("display-none");
            this.elements.divAssociatedAccounts.removeClass("display-none");
        }

        this.elements.btnExportExcel.attr("username", rs.Username);

        var title = resources.VirtualRateDetails +
            (this.isGlobalSetting() ? " - " + resources.GlobalSetting : " - " + rs.Username + "   " + "<span class='lbl-curreny'>" + resources.Currency + ": " + rs.Currency + "</span>");
        this.elements.reportTitle.html(title);
        this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");

        this.elements.chkNonLive.attr("checked", rs.EnableNonLive ? "checked" : null);
        this.elements.chkLive.attr("checked", rs.EnableLive ? "checked" : null);

        var html = "";

        if (rs.Sports.length > 0) {
            for (var i = 0; i < rs.Sports.length; i++) {
                var sport = rs.Sports[i];
                html += this.renderSport(sport, rs);
            }
        }
        else {
            html += "<tr><td colspan='3' align='center'>" + resources.Message_NoReportData + "</td></tr>";
        }

        this.elements.tblVirtualRateReport.find("tbody").empty().html(html);

        $(".group-toggle").on("click touchend", function () {
            self.toggleGroup($(this));
        });

        $(".icon-copy2child").on("click touchend", function () {
            self.copyToChildren($(this));
        });

        $(".icon-copy2related").on("click touchend", function () {
            self.copyToRelated($(this));
        });

        this.elements.divAssociatedAccounts.attr("disabled", "disabled");
        this.elements.chkAssociatedAccounts.attr("checked", null);
        this.elements.txtAssociatedAccounts.val("");
        this.enableAssociatedAccounts();

        this.elements.reportContainer.removeClass("display-none");

        this.elements.btnViewCompactMode.click();
    };
    this.renderSport = function (sport, rs) {
        var html = "";
        html += this.renderSportRow(sport, rs);

        for (var i = 0; i < sport.Childs.length; i++) {
            var game = sport.Childs[i];
            html += this.renderGame(sport, game, rs);
        }

        return html;
    };
    this.renderGame = function (sport, game, rs) {
        var html = "";
        html += this.renderGameRow(sport, game, rs);

        for (var i = 0; i < game.Childs.length; i++) {
            var betTypeGroup = game.Childs[i];
            html += this.renderBetTypeGroup(sport, game, betTypeGroup, rs);
        }

        return html;
    };
    this.renderBetTypeGroup = function (sport, game, betTypeGroup, rs) {
        var html = "";
        html += this.renderBetTypeGroupRow(sport, game, betTypeGroup, rs);

        for (var i = 0; i < betTypeGroup.Childs.length; i++) {
            var betType = betTypeGroup.Childs[i];
            html += this.renderBetTypeRow(sport, game, betTypeGroup, betType, rs);
        }

        return html;
    };
    this.renderSportRow = function (sport, rs) {
        var ddlNonlive = this.renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            id: sport.NonliveId,
            value: sport.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: this.isGlobalSetting() ? "" : resources.FollowUpline,
            copyToRelatedText: resources.CopyToAll + " \"" + sport.GroupName + "\""
        });

        var ddlLive = this.renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            id: sport.LiveId,
            value: sport.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: this.isGlobalSetting() ? "" : resources.FollowUpline,
            copyToRelatedText: resources.CopyToAll + " \"" + sport.GroupName + "\""
        });

        var html = "<tr data-level='1' data-sportid='" + sport.GroupId + "'>\
                        <td data-col='username'>" + sport.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                    </tr>";
        return html;
    };
    this.renderGameRow = function (sport, game, rs) {
        var ddlNonlive = this.renderDropdownRate({
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

        var ddlLive = this.renderDropdownRate({
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

        var html = "<tr data-level='2' data-sportid='" + sport.GroupId + "' data-gameid='" + game.GroupId + "'>\
                        <td data-col='username'>\
                            <span class='group-toggle group-expand'>[−]</span>" + game.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                    </tr>";
        return html;
    };
    this.renderBetTypeGroupRow = function (sport, game, betTypeGroup, rs) {
        var ddlNonlive = this.renderDropdownRate({
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

        var ddlLive = this.renderDropdownRate({
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

        var html = "<tr data-level='3' data-sportid='" + sport.GroupId + "' data-gameid='" + game.GroupId + "' data-bettypegroupid='" + betTypeGroup.GroupId + "'>\
                        <td data-col='username'>\
                            <span class='group-toggle group-expand'>[−]</span>" + betTypeGroup.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                    </tr>";
        return html;
    };
    this.renderBetTypeRow = function (sport, game, betTypeGroup, betType, rs) {
        var ddlNonlive = this.renderDropdownRate({
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

        var ddlLive = this.renderDropdownRate({
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

        var html = "<tr data-level='4' data-sportid='" + sport.GroupId + "' data-gameid='" + game.GroupId + "' data-bettypegroupid='" + betTypeGroup.GroupId + "' data-bettypeid='" + betType.GroupId + "'>\
                        <td data-col='username'>" + betType.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                    </tr>";
        return html;
    };
    this.renderDropdownRate = function (options) {
        var html = "";

        if (options.copyToRelated) {
            html += "<span class='icon-copy2related' title='" + options.copyToRelatedText + "'></span>";
        } else {
            html += "<span class='padding-icon-copy2related'></span>";
        }

        html += "<select class='ddl-rate'>";

        if (options.followParentText) {
            html += "<option" + (options.id == 0 ? " selected='selected'" : "") + " value='" + this.followParentValue + "'" + " data-level-id='0'>" +
                options.followParentText + "</option>";
        }

        for (var i = 0; i < options.rateRanges.length; i++) {
            var range = options.rateRanges[i];
            if (range.Value < options.minRate || options.maxRate < range.Value) {
                continue;
            }

            var rangeText = numeral(range.Value).format("#,##0");
            var value = (range.Value + 100) * options.actualRate / 100;
            var rateText = this.isGlobalSetting() ? rangeText : rangeText + ' (' + numeral(value).format("#,##0.0000") + ')';

            html += "<option" + (options.value == range.Value ? " selected='selected'" : "") +
                " value='" + range.Value + "'" + " data-level-id='" + range.Id + "'>" + rateText + "</option>";
        }

        html += "</select>";

        if (options.copyToChildren) {
            html += "<span class='icon-copy2child' title='" + resources.CopyToAllChildren + "'></span>";
        }

        return html;
    };

    this.updateEnableNonlive = function () {
        var isEnable = this.elements.chkNonLive.is(":checked");
        this.onOffSelection(isEnable, "non-live");
    };
    this.updateEnableLive = function () {
        var isEnable = this.elements.chkLive.is(":checked");
        this.onOffSelection(isEnable, "live");
    };
    this.onOffSelection = function (isEnable, columnName) {
        var disabled = isEnable ? null : " disabled='disabled'";

        if (isEnable) {
            $("td[data-col='" + columnName + "']").removeClass("status-disabled");
        } else {
            $("td[data-col='" + columnName + "']").addClass("status-disabled");
        }
    }
    this.toggleGroup = function (elem) {
        var isExpand = elem.hasClass("group-expand");
        var parent = elem.parent().parent();
        var level = parseInt(parent.attr("data-level"));

        if (isExpand) {
            this.hideAllChild(level, parent);
            elem.removeClass("group-expand").addClass("group-collapse");
            elem.html("[+]");
        } else {
            this.showAllChild(level, parent);
            elem.removeClass("group-collapse").addClass("group-expand");
            elem.html("[−]");
        }

        this.checkCollapseExpandAll();
    };
    this.hideAllChild = function (level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");
        if (level == 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            $("#tbl-virtual-rate-report tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'][data-bettypegroupid='" + betTypeGroupId + "']").addClass("display-none");
            return;
        }

        $("#tbl-virtual-rate-report tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").addClass("display-none");
        $("#tbl-virtual-rate-report tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").addClass("display-none");
    };
    this.showAllChild = function (level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");

        if (level == 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            $("#tbl-virtual-rate-report tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'][data-bettypegroupid='" + betTypeGroupId + "']").removeClass("display-none");
            return;
        }

        var toggles = $("tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            this.forceToggle($(toggles[i]), true);
        }

        $("#tbl-virtual-rate-report tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").removeClass("display-none");
        $("#tbl-virtual-rate-report tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").removeClass("display-none");
    };
    this.forceToggle = function (elem, isExpand) {
        var parent = elem.parent().parent();

        if (isExpand) {
            elem.removeClass("group-collapse").addClass("group-expand");
        } else {
            elem.removeClass("group-expand").addClass("group-collapse");
        }

        var text = elem.parent().parent().hasClass("group-nonchild-compact-mode") ? "" : isExpand ? "[−]" : "[+]";
        elem.html(text);
    };

    this.copyToChildren = function (elem) {
        var parent = elem.parent().parent();
        var level = parseInt(parent.attr("data-level"));

        var colName = elem.parent().attr("data-col");
        var value = parent.find("[data-col='" + colName + "'] .ddl-rate").val();
        var sportId = parseInt(parent.attr("data-sportid"));
        var gameId = parseInt(parent.attr("data-gameid"));
        var betTypeGroupId = parseInt(parent.attr("data-bettypegroupid"));

        var selector = "tr:not(.hide-compact-mode)[data-sportid='" + sportId + "']";
        if (level >= 2) {
            selector += "[data-gameid='" + gameId + "']";
        }

        if (level >= 3) {
            selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
        }

        selector += " [data-col='" + colName + "'] .ddl-rate";
        this.elements.tblVirtualRateReport.find(selector).val(value);
    };
    this.copyToRelated = function (elem) {
        var parent = elem.parent().parent();
        var level = parseInt(parent.attr("data-level"));
        var colName = elem.parent().attr("data-col");
        var value = parent.find("[data-col='" + colName + "'] .ddl-rate").val();

        var sportId = parseInt(parent.attr("data-sportid"));
        var gameId = parseInt(parent.attr("data-gameid"));
        var betTypeGroupId = parseInt(parent.attr("data-bettypegroupid"));
        var betTypeId = parseInt(parent.attr("data-bettypeid"));

        var selector = "tr:not(.hide-compact-mode)[data-level='" + level + "']";
        switch (level) {
            case 1:
                selector += "[data-sportid='" + sportId + "']";
                break;
            case 2:
                selector += "[data-gameid='" + gameId + "']";
                break;
            case 3:
                selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
                break;
            case 4:
                selector += "[data-bettypeid='" + betTypeId + "']";
                break;
        }

        selector += " [data-col='" + colName + "'] .ddl-rate";
        this.elements.tblVirtualRateReport.find(selector).val(value);
    };
    this.viewUpline = function (custName) {
        if (custName == "") {
            window.open(CMS.resolveRootUrl("VirtualRateDetails/GlobalSetting"), "_blank");

        } else {
            window.open(CMS.resolveRootUrl("VirtualRateDetails/Index?username=" + custName), "_blank");
        }
    };

    this.getCurrentValue = function () {
        var newValue = {
            CustId: this.oldReport.CustId,
            Username: this.oldReport.Username,
            EnableNonLive: this.elements.chkNonLive.is(":checked"),
            EnableLive: this.elements.chkLive.is(":checked"),
            Sports: this.getSports(this.oldReport.Sports)
        };

        return newValue;
    };
    this.getSports = function (sports) {
        var result = [];
        for (var i = 0; i < sports.length; i++) {
            var sport = sports[i];

            var nonlive = this.getRateValue(1, "non-live", sport.GroupId);
            var live = this.getRateValue(1, "live", sport.GroupId);
            result.push({
                GroupId: sport.GroupId,
                LevelId: sport.LevelId,
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: this.getGames(sport, sport.Childs)
            });
        }

        return result;
    };
    this.getGames = function (sport, games) {
        var result = [];
        for (var i = 0; i < games.length; i++) {
            var game = games[i];

            var nonlive = this.getRateValue(2, "non-live", sport.GroupId, game.GroupId);
            var live = this.getRateValue(2, "live", sport.GroupId, game.GroupId);
            result.push({
                GroupId: game.GroupId,
                LevelId: game.LevelId,
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: this.getBetTypeGroups(sport, game, game.Childs)
            });
        }

        return result;
    };
    this.getBetTypeGroups = function (sport, game, betTypeGroups) {
        var result = [];
        for (var i = 0; i < betTypeGroups.length; i++) {
            var betTypeGroup = betTypeGroups[i];

            var nonlive = this.getRateValue(3, "non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId);
            var live = this.getRateValue(3, "live", sport.GroupId, game.GroupId, betTypeGroup.GroupId);
            result.push({
                GroupId: betTypeGroup.GroupId,
                LevelId: betTypeGroup.LevelId,
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                Childs: this.getBetTypes(sport, game, betTypeGroup, betTypeGroup.Childs)
            });
        }

        return result;
    };
    this.getBetTypes = function (sport, game, betTypeGroup, betTypes) {
        var result = [];
        for (var i = 0; i < betTypes.length; i++) {
            var betType = betTypes[i];

            var nonlive = this.getRateValue(4, "non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);
            var live = this.getRateValue(4, "live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);
            result.push({
                GroupId: betType.GroupId,
                LevelId: betType.LevelId,
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value
            });
        }

        return result;
    };
    this.getRateValue = function (level, colName, sportId, gameId, betTypeGroupId, betTypeId) {
        var selector = "tr[data-sportid='" + sportId + "']";

        if (level >= 2) {
            selector += "[data-gameid='" + gameId + "']";
        }

        if (level >= 3) {
            selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
        }

        if (level >= 4) {
            selector += "[data-bettypeid='" + betTypeId + "']";
        }

        selector += " [data-col='" + colName + "'] .ddl-rate";
        var dropdown = this.elements.tblVirtualRateReport.find(selector);
        return {
            id: dropdown.find("option:selected").attr("data-level-id"),
            value: dropdown.val()
        };
    };

    this.hasChanges = function (oldReport, newReport) {
        if (this.rateChanged(oldReport, newReport)) {
            return true;
        }

        return this.childChanged(oldReport.Sports, newReport.Sports);
    };
    this.childChanged = function (oldChilds, newChilds) {
        for (var i = 0; i < oldChilds.length; i++) {
            var oldChild = oldChilds[i];
            var newChild = newChilds[i];

            if (this.rateChanged(oldChild, newChild)) {
                return true;
            }

            if (!oldChild.Childs || oldChild.Childs.length == 0) {
                continue;
            }

            if (this.childChanged(oldChild.Childs, newChild.Childs)) {
                return true;
            }
        }

        return false;
    };
    this.rateChanged = function (oldRate, newRate) {
        return oldRate.NonLiveValue != newRate.NonLiveValue ||
            oldRate.LiveValue != newRate.LiveValue;
    };
    this.isGlobalSetting = function () {
        return this.oldReport.ReportLevel == 0;
    };

    this.collapseReport = function () {
        var isCollapse = this.elements.iconCollapse.hasClass("ico-collapse");

        if (isCollapse) {
            this.elements.iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        var toggles = $("tr[data-level='2'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            var elem = $(toggles[i]);
            this.forceToggle(elem, !isCollapse);

            var parent = elem.parent().parent();
            if (isCollapse) {
                this.hideAllChild(2, parent);
            } else {
                this.showAllChild(2, parent);
            }
        }
    };
    this.checkCollapseExpandAll = function () {
        var isCollapseAll = $("#tbl-virtual-rate-report tr[data-level='2'] .group-toggle.group-collapse").length == $("tr[data-level='2'] .group-toggle").length;
        if (isCollapseAll) {
            this.elements.iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
            return;
        }

        var isExpandAll = $("#tbl-virtual-rate-report tr .group-toggle.group-expand").length == $("tr .group-toggle").length;
        if (isExpandAll) {
            this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }
    };
    this.viewCompactMode = function () {
        for (var i = 4; i >= 2; i--) {
            this.hideFollowParentRows(i);
        }

        var copyRelateds = $("#tbl-virtual-rate-report tr:not(.hide-compact-mode) .icon-copy2related");
        for (var i = 0; i < copyRelateds.length; i++) {
            var elem = $(copyRelateds[i]);

            var row = elem.parent().parent();
            var level = parseInt(row.attr("data-level"));
            var sportId = row.attr("data-sportid");
            var selector = "";
            switch (level) {
                case 2:
                    var gameId = row.attr("data-gameid");
                    selector = "#tbl-virtual-rate-report tr[data-level='2'][data-gameid='" + gameId + "']";
                    break;
                case 3:
                    var betTypeGroupId = row.attr("data-bettypegroupid");
                    selector = "#tbl-virtual-rate-report tr[data-level='3'][data-bettypegroupid='" + betTypeGroupId + "']";
                    break;
                case 4:
                    var betTypeId = row.attr("data-bettypeid");
                    selector = "#tbl-virtual-rate-report tr[data-level='4'][data-bettypeid='" + betTypeId + "']";
                    break;
            }

            selector += ":not(.hide-compact-mode)";
            if ($(selector).length <= 1) {
                elem.parent().addClass("no-repetition-compact-mode");
            }
        }

        this.elements.btnViewCompactMode.addClass("display-none");
        this.elements.btnViewFullMode.removeClass("display-none");

        if (this.isFirstSearch || this.isGlobalSetting()) {
            return;
        }

        this.expandAllChild();
    };
    this.hideFollowParentRows = function (layerId) {
        var rows = this.elements.tblVirtualRateReport.find("tbody tr[data-level='" + layerId + "'] ");
        for (var i = 0; i < rows.length; i++) {
            var elem = $(rows[i]);
            var isFollowParent = elem.find("td[data-col='non-live'] .ddl-rate").val() == this.followParentValue &&
                elem.find("td[data-col='live'] .ddl-rate").val() == this.followParentValue;

            var sportId = elem.attr("data-sportid");
            var gameId = elem.attr("data-gameid");
            var betTypeGroupId = elem.attr("data-bettypegroupid");
            var betTypeId = elem.attr("data-bettypegroupid");

            var selector = "#tbl-virtual-rate-report tr[data-level='" + (layerId + 1) + "'][data-sportid='" + sportId + "']";
            if (layerId >= 2) {
                selector += "[data-gameid='" + gameId + "']";
            }

            if (layerId >= 3) {
                selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
            }

            if (layerId >= 4) {
                selector += "[data-bettypeid='" + betTypeId + "']";
            }

            selector += ":not(.hide-compact-mode)";

            if (isFollowParent && $(selector).length == 0) {
                elem.addClass("hide-compact-mode");
                continue;
            }

            if ($(selector).length == 0) {
                var toggle = elem.find(".group-toggle");
                toggle.attr("data-original-text", toggle.html()).html("&nbsp;");
                elem.addClass("group-nonchild-compact-mode");
            }
        }
    };
    this.viewFullMode = function () {
        $(".hide-compact-mode").removeClass("hide-compact-mode");
        $(".no-repetition-compact-mode").removeClass("no-repetition-compact-mode");

        var groupToggles = this.elements.tblVirtualRateReport.find(".group-nonchild-compact-mode .group-toggle");
        for (var i = 0; i < groupToggles.length; i++) {
            var elem = $(groupToggles[i]);
            elem.html(elem.attr("data-original-text"));
            elem.parent().parent().removeClass("group-nonchild-compact-mode");
        }

        this.elements.btnViewFullMode.addClass("display-none");
        this.elements.btnViewCompactMode.removeClass("display-none");

        if (this.isGlobalSetting()) {
            return;
        }

        this.expandAllChild();
    };
    this.expandAllChild = function () {
        var toggles = $("tr[data-level='2'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            var elem = $(toggles[i]);
            this.forceToggle(elem, true);
            var parent = elem.parent().parent();
            this.showAllChild(2, parent);
        }

        toggles = $("tr[data-level='3'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            var elem = $(toggles[i]);
            this.forceToggle(elem, !this.isFirstSearch);
            var parent = elem.parent().parent();

            if (this.isFirstSearch) {
                this.hideAllChild(3, parent);
            } else {
                this.showAllChild(3, parent);
            }
        }

        if (!this.isFirstSearch) {
            this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        this.isFirstSearch = false;
    };

    this.saveChanges = function () {
        CMS.showConfirmMessage(
            resources.VirtualRateSubmitChangesConfirm,
            function () {
                self.submitChanges();
            }
        );
    };
    this.submitChanges = function () {
        var currentValue = this.getCurrentValue();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/SaveVirtualRateDetails"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({
                Rates: currentValue,
                sportType: resources.SportType,
                custId: this.oldReport.CustId
            }),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                if (rs.Code == 0) {
                    self.oldReport = currentValue;
                    CMS.showSuccessMessage(rs.Message);
                } else {
                    CMS.showErrorMessage(rs.Message);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    this.enableAssociatedAccounts = function () {
        var enable = this.elements.chkAssociatedAccounts.is(":checked");
        if (enable) {
            this.elements.divAssociatedAccounts.attr("disabled", null);
            this.elements.txtAssociatedAccounts.attr("disabled", null);
            this.elements.btnSubmitAssociatedAccounts.on("click touchend", function () {
                self.validateAssociatedAccounts();
            });

        } else {
            this.elements.divAssociatedAccounts.attr("disabled", "disabled");
            this.elements.txtAssociatedAccounts.attr("disabled", "disabled");
            this.elements.btnSubmitAssociatedAccounts.off("click touchend");
        }
    };
    this.validateAssociatedAccounts = function () {
        var accounts = this.elements.txtAssociatedAccounts.val().trim();
        if (accounts.trim() == "") {
            Validator.showError(this.elements.txtAssociatedAccounts, resources.VirtualRate_AssociatedAccounts_Empty, "s");
            return;
        }

        if (accounts.split(",").length > 50) {
            Validator.showError(this.elements.txtAssociatedAccounts, resources.VirtualRate_AssociatedAccounts_Error, "s");
            return;
        }

        Validator.clearError(this.elements.txtAssociatedAccounts);
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/ValidateAssociatedAccounts"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ accounts: accounts }),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                var message = resources.InvalidAccounts + ": " + self.escapeHtml(rs.InvalidAccountsText) + "</br></br>";
                if (rs.ValidAccountsText) {
                    message += resources.ValidAccounts + ": " + self.escapeHtml(rs.ValidAccountsText) + "</br></br>";
                    message += resources.Apply2AssociatedAccounts;

                    CMS.showConfirmMessage(
                        message,
                        function () {
                            self.submitAssociatedAccounts(rs.ValidAccountIdsText);
                        }
                    );
                } else {
                    CMS.showErrorBox(message);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.submitAssociatedAccounts = function (accounts) {
        var currentValue = this.getCurrentValue();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/SaveAssociatedAccounts"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({
                Accounts: accounts,
                Rates: currentValue,
                sportType: resources.SportType
            }),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                if (rs.Code == 0) {
                    CMS.showSuccessMessage(rs.Message);
                } else {
                    CMS.showErrorMessage(rs.Message);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.formatAssociatedAccounts = function () {
        var accounts = this.elements.txtAssociatedAccounts.val();
        accounts = accounts.replace(/\r\n/g, "\n").replace(/\n/g, ",");
        this.elements.txtAssociatedAccounts.val(accounts);
    };
    this.escapeHtml = function (unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    };

    this.viewRealValues = function (custId) {
        this.realValueCustId = custId;
        CMS.closeMsgBox();
        var url = CMS.resolveRootUrl("VirtualRateDetails/ViewRealValues?custId=" + custId + "&sportType=" + resources.SportType);
        CMS.getHtml(url, this.showRealValues);
    };
    this.showRealValues = function (html) {
        CMS.nonResizableModal(resources.VirtualRateDetails, html, { height: 600 });

        $("#btn-export-real-values").on("click touchend", function () {
            self.exportRealValue();
        });

        $(".tbl-real-values .group-toggle").on("click touchend", function () {
            self.toggleRealValue($(this));
        });
    };
    this.exportRealValue = function () {
        var param = { custId: this.realValueCustId, sportType: resources.SportType };
        CMS.downloadFile(CMS.resolveRootUrl("VirtualRateDetails/ExportRealValues"), param);
    };
    this.toggleRealValue = function (elem) {
        var isExpand = elem.hasClass("group-expand");
        var parent = elem.parent().parent();
        var level = parseInt(parent.attr("data-level"));

        if (isExpand) {
            this.hideAllChildValue(level, parent);
            elem.removeClass("group-expand").addClass("group-collapse");
            elem.html("[+]");
        } else {
            this.showAllChildValue(level, parent);
            elem.removeClass("group-collapse").addClass("group-expand");
            elem.html("[−]");
        }
    };
    this.hideAllChildValue = function (level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");
        if (level == 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            $(".tbl-real-values tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'][data-bettypegroupid='" + betTypeGroupId + "']").addClass("display-none");
            return;
        }

        $(".tbl-real-values tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").addClass("display-none");
        $(".tbl-real-values tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").addClass("display-none");
    };
    this.showAllChildValue = function (level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");

        if (level == 3) {
            var betTypeGroupId = parent.attr("data-bettypegroupid");
            $(".tbl-real-values tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'][data-bettypegroupid='" + betTypeGroupId + "']").removeClass("display-none");
            return;
        }

        var toggles = $(".tbl-real-values tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            this.forceToggleValue($(toggles[i]), true);
        }

        $(".tbl-real-values tr[data-level='4'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").removeClass("display-none");
        $(".tbl-real-values tr[data-level='3'][data-sportid='" + sportId + "'][data-gameid='" + gameId + "']").removeClass("display-none");
    };
    this.forceToggleValue = function (elem, isExpand) {
        var parent = elem.parent().parent();

        if (isExpand) {
            elem.removeClass("group-collapse").addClass("group-expand");
        } else {
            elem.removeClass("group-expand").addClass("group-collapse");
        }

        var text = elem.parent().parent().hasClass("group-nonchild-compact-mode") ? "" : isExpand ? "[−]" : "[+]";
        elem.html(text);
    };
};

$(document).ready(function () {
    var virtualRateDetailsIndex = new VirtualRateDetailsIndex();

    virtualRateDetailsIndex.init();
});
