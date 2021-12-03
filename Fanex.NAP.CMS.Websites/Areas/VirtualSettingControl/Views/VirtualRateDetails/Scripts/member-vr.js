MemberVirtualRate = function (pageHandler) {
    var self = this;
    this.pageHandler = pageHandler;
    this.isShown = false;
    this.checkAllButtonsHtml = null;
    this.oldReport = null;
    this.isFirstSearch = false;
    this.isCompactMode = false;
    this.elements = {
        btnAutoSuspendView: null,
        btnAutoSuspendEdit: null,

        lnkViewUpline: null,
        lnkViewRealValues: null,
        divAssociatedAccounts: null,

        reportTitle: null,
        iconCollapse: null,
        chkAssociatedAccounts: null,
        txtAssociatedAccounts: null,

        tblVirtualRateReportMember: null,
        reportContainer: null,
        gridTitle: null,

        lblApplyVROnly: null,
        lblApplyVR_AS: null,
        lblApplyVR: null,
    };

    this.cachedElements = function () {
        this.elements = this.pageHandler.elements;

        this.elements.btnAutoSuspendView = $(".btn-autosuspend-view");
        this.elements.btnAutoSuspendEdit = $(".btn-autosuspend-edit");
        this.elements.tblVirtualRateReportMember = $("#tbl-virtual-rate-report-member");
        this.elements.gridTitle = $("#virtual-rate-report-freegrid-grid-title");

        this.elements.lblApplyVROnly = $("#lbl-apply-vronly");
        this.elements.lblApplyVR_AS = $("#lbl-apply-vr-as");
        this.elements.lblApplyVR = $("#lbl-apply");

        this.checkAllButtonsHtml = this.checkAllButtons();
    };
    this.bindEvents = function () {
        this.elements.tblVirtualRateReportMember.find(".btn-view-compact-mode").off("click").on("click", function () {
            self.viewCompactMode();
        });
        this.elements.tblVirtualRateReportMember.find(".btn-view-full-mode").off("click").on("click", function () {
            self.viewFullMode();
        });

        this.elements.btnAutoSuspendView.off("click").on("click", function () {
            self.autosuspendView();
        });
        this.elements.btnAutoSuspendEdit.off("click").on("click", function () {
            self.autosuspendEdit();
        });
        this.elements.btnSubmitChanges.off("click touchend").on("click touchend", function () {
            self.saveChanges();
        });

        window.onbeforeunload = function () {
            if (self.oldReport == null) {
                return;
            }

            var currentValue = self.pageHandler.getCurrentValue();
            if (self.pageHandler.hasChanges(self.oldReport, currentValue)) {
                return resources.VirtualRateDetails_LeaveSiteWarning;
            }
        };

        $("#spnAutoSuspendNote").tipsy({
            html: true,
            opacity: 0.9,
            gravity: "sw",
            className: "auto-suspend-note"
        });
    };

    this.hideReport = function () {
        if (!this.isShown) {
            return;
        }

        this.elements.btnAutoSuspendView.addClass("display-none");
        this.elements.btnAutoSuspendEdit.addClass("display-none");
        this.elements.tblVirtualRateReportMember.addClass("display-none");

        this.elements.lblApplyVROnly.addClass("display-none");
        this.elements.lblApplyVR_AS.addClass("display-none");
        this.elements.lblApplyVR.removeClass("display-none");

        this.isShown = false;
    }
    this.renderReport = function (rs) {
        this.pageHandler.funcGetBetTypes = self.getBetTypes;
        this.pageHandler.funcGetRateValue = self.getRateValue;
        this.isFirstSearch = true;
        this.oldReport = rs;
        this.isShown = true;
        this.cachedElements();
        this.bindEvents();

        this.elements.btnAutoSuspendEdit.removeClass("display-none");
        this.elements.lnkViewUpline.attr("custname", rs.UplineCustName).removeClass("display-none");
        this.elements.lnkViewRealValues.attr("cust-id", rs.CustId).removeClass("display-none");
        this.elements.divAssociatedAccounts.removeClass("display-none");
        this.elements.btnExportExcel.attr("username", rs.Username);

        var title = resources.VirtualRateDetails + " - " + rs.Username + "   " + "<span class='lbl-curreny'>" + resources.Currency + ": " + rs.Currency + "</span>";
        this.elements.reportTitle.html(title);
        this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");

        var html = "";
        if (rs.Sports.length > 0) {
            for (var i = 0; i < rs.Sports.length; i++) {
                var sport = rs.Sports[i];
                html += this.renderSport(sport, rs);
            }
        }
        else {
            html += "<tr class='elm-compact-mode'><td colspan='3' align='center'>" + resources.Message_NoReportData + "</td></tr>";
            html += "<tr class='elm-full-mode'><td colspan='5' align='center'>" + resources.Message_NoReportData + "</td></tr>";
        }

        this.elements.tblVirtualRateReportMember.find("tbody").empty().html(html);
        this.elements.tblVirtualRateReportMember.removeClass("display-none");
        this.elements.lblApplyVR.addClass("display-none");

        this.elements.divAssociatedAccounts.attr("disabled", "disabled");
        this.elements.chkAssociatedAccounts.attr("checked", null);
        this.elements.txtAssociatedAccounts.val("");
        this.pageHandler.enableAssociatedAccounts();

        this.elements.btnViewFullMode.click();
        this.autosuspendView();
        this.bindGridEvents();

        this.elements.reportContainer.removeClass("display-none");
        this.isFirstSearch = false;
    };
    this.viewCompactMode = function () {
        this.elements.gridTitle.removeClass("full-mode").addClass("compact-mode");
        this.elements.btnAutoSuspendView.addClass("display-none");
        this.elements.btnAutoSuspendEdit.addClass("display-none");

        this.elements.tblVirtualRateReportMember.removeClass("full-mode").addClass("compact-mode");
        this.elements.btnViewCompactMode.addClass("display-none");
        this.elements.btnViewFullMode.removeClass("display-none");

        if (this.pageHandler.isMember) {
            this.elements.lblApplyVROnly.removeClass("display-none");
            this.elements.lblApplyVR_AS.addClass("display-none");
        }

        this.pageHandler.viewCompactMode();
        this.isCompactMode = true;
    };
    this.viewFullMode = function () {
        this.elements.gridTitle.removeClass("compact-mode").addClass("full-mode");
        this.elements.tblVirtualRateReportMember.removeClass("compact-mode").addClass("full-mode");

        this.elements.btnViewFullMode.addClass("display-none");

        if (this.elements.tblVirtualRateReportMember.hasClass("autosuspend-edit")) {
            this.elements.btnAutoSuspendView.removeClass("display-none");
        } else {
            this.elements.btnAutoSuspendEdit.removeClass("display-none");
        }

        this.elements.btnViewCompactMode.removeClass("display-none");

        if (!this.isFirstSearch) {
            this.pageHandler.viewFullMode();
        }

        if (this.pageHandler.isMember) {
            this.elements.lblApplyVR_AS.removeClass("display-none");
            this.elements.lblApplyVROnly.addClass("display-none");
        }

        this.isCompactMode = false;
    };
    this.autosuspendView = function () {
        this.elements.tblVirtualRateReportMember.removeClass("autosuspend-edit").addClass("autosuspend-view");
        this.elements.btnAutoSuspendView.addClass("display-none");
        this.elements.btnAutoSuspendEdit.removeClass("display-none");

        $(".chk-autosuspend:not(.display-none)").each(function () {
            var column = $(this).closest("td");

            if (column.data("override")) {
                var iconCheckedOverrideAutosuspend = column.find(".icon-checked-override-autosuspend");

                if (this.checked) {
                    iconCheckedOverrideAutosuspend.removeClass("display-none");
                } else {
                    iconCheckedOverrideAutosuspend.addClass("display-none");
                }
            } else {
                var iconNonAutosuspend = column.find(".icon-non-autosuspend");

                if (this.checked) {
                    iconNonAutosuspend.addClass("display-none");
                } else {
                    iconNonAutosuspend.removeClass("display-none");
                }
            }
        });
    };
    this.autosuspendEdit = function () {
        this.elements.tblVirtualRateReportMember.removeClass("autosuspend-view").addClass("autosuspend-edit");
        this.elements.btnAutoSuspendEdit.addClass("display-none");
        this.elements.btnAutoSuspendView.removeClass("display-none");
        this.updateAutoSuspendVisible();
    };

    this.checkAllButtons = function () {
        var html = "<input type='button' class='submit btn-tick-all' value='" + resources.TickAll + "' />";
        html += "<input type='button' class='submit btn-untick-all' value='" + resources.UntickAll + "' />";

        return html;
    };
    this.renderSportRow = function (sport, rs) {
        var copyToRelatedText = resources.CopyToAll + " \"" + sport.GroupName + "\"";
        var ddlNonlive = this.pageHandler.renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            id: sport.NonliveId,
            value: sport.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: resources.FollowUpline,
            copyToRelatedText: copyToRelatedText
        });

        var ddlLive = this.pageHandler.renderDropdownRate({
            minRate: sport.MinRate,
            maxRate: sport.MaxRate,
            id: sport.LiveId,
            value: sport.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: false,
            followParentText: resources.FollowUpline,
            copyToRelatedText: copyToRelatedText
        });
        var html = "<tr data-level='1' data-sportid='" + sport.GroupId + "'>\
                        <td data-col='username'>" + sport.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='non-live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='non-live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                        <td data-col='live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                    </tr>";
        return html;
    };
    this.renderGameRow = function (sport, game, rs) {
        var copyToRelatedText = resources.CopyToAll + " \"" + game.GroupName + "\"";
        var ddlNonlive = this.pageHandler.renderDropdownRate({
            minRate: game.MinRate,
            maxRate: game.MaxRate,
            id: sport.NonliveId,
            value: game.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });

        var ddlLive = this.pageHandler.renderDropdownRate({
            minRate: game.MinRate,
            maxRate: game.MaxRate,
            id: sport.LiveId,
            value: game.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: game.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });

        var html = "<tr data-level='2' data-sportid='" + sport.GroupId + "' data-gameid='" + game.GroupId + "'>\
                        <td data-col='username'>\
                            <span class='group-toggle group-expand'>[−]</span>" + game.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='non-live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='non-live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                        <td data-col='live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                    </tr>";
        return html;
    };
    this.renderBetTypeGroupRow = function (sport, game, betTypeGroup, rs) {
        var copyToRelatedText = resources.CopyToAll + " \"" + betTypeGroup.GroupName + "\"";
        var ddlNonlive = this.pageHandler.renderDropdownRate({
            minRate: betTypeGroup.MinRate,
            maxRate: betTypeGroup.MaxRate,
            id: sport.NonliveId,
            value: betTypeGroup.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });

        var ddlLive = this.pageHandler.renderDropdownRate({
            minRate: betTypeGroup.MinRate,
            maxRate: betTypeGroup.MaxRate,
            id: sport.LiveId,
            value: betTypeGroup.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: true,
            copyToRelated: betTypeGroup.HasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });

        var html = "<tr data-level='3' data-sportid='" + sport.GroupId + "' data-gameid='" + game.GroupId + "' data-bettypegroupid='" + betTypeGroup.GroupId + "'>\
                        <td data-col='username'>\
                            <span class='group-toggle group-expand'>[−]</span>" + betTypeGroup.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='non-live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='non-live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                        <td data-col='live' data-override='0' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                        <td data-col='live' data-override='1' class='no-padding align-center elm-full-mode'>" + this.checkAllButtonsHtml + "</td>\
                    </tr>";
        return html;
    };
    this.renderBetTypeRow = function (sport, game, betTypeGroup, betType, rs) {
        var copyToRelatedText = resources.CopyToAll + " \"" + betType.GroupName + "\"";
        var hasRepetition = betType.HasRepetition;

        var ddlNonlive = this.pageHandler.renderDropdownRate({
            minRate: betType.MinRate,
            maxRate: betType.MaxRate,
            id: sport.NonliveId,
            value: betType.NonLiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: false,
            copyToRelated: hasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });

        var ddlLive = this.pageHandler.renderDropdownRate({
            minRate: betType.MinRate,
            maxRate: betType.MaxRate,
            id: sport.LiveId,
            value: betType.LiveValue,
            rateRanges: rs.VirtualRateValueRange,
            actualRate: rs.ActualRate,
            copyToChildren: false,
            copyToRelated: hasRepetition,
            followParentText: resources.FollowParent,
            copyToRelatedText: copyToRelatedText
        });
        var autoSuspendNonlive = this.renderAutoSuspend({
            copyToRelated: hasRepetition,
            canEditAutoSuspend: !betType.DisableNonliveAutoSuspend,
            isAutoSuspend: betType.NonliveAutoSuspend,
            copyToRelatedText: copyToRelatedText
        });
        var overrideAutoSuspendNonlive = this.renderOverrideAutoSuspend(
            betType.IsOverrideASNonlive,
            hasRepetition,
            copyToRelatedText);

        var autoSuspendLive = this.renderAutoSuspend({
            copyToRelated: hasRepetition,
            canEditAutoSuspend: !betType.DisableLiveAutoSuspend,
            isAutoSuspend: betType.LiveAutoSuspend,
            copyToRelatedText: copyToRelatedText
        });
        var overrideAutoSuspendLive = this.renderOverrideAutoSuspend(
            betType.IsOverrideASLive,
            hasRepetition,
            copyToRelatedText);

        var html = "<tr data-level='4' data-sportid='" + sport.GroupId
            + "' data-gameid='" + game.GroupId + "' data-bettypegroupid='" + betTypeGroup.GroupId
            + "' data-bettypeid='" + betType.GroupId + "'"
            + " data-non-live-threshold='" + betType.NonliveAutoSuspendThreshold + "'"
            + " data-live-threshold='" + betType.LiveAutoSuspendThreshold + "'"
            + ">\
                        <td data-col='username'>" + betType.GroupName + "</td>\
                        <td data-col='non-live'>" + ddlNonlive + "</td>\
                        <td data-col='non-live' data-override='0'  class='elm-full-mode col-autosuspend'>" + autoSuspendNonlive + "</td>\
                        <td data-col='non-live' data-override='1' class='elm-full-mode col-autosuspend'>" + overrideAutoSuspendNonlive + "</td>\
                        <td data-col='live'>" + ddlLive + "</td>\
                        <td data-col='live' data-override='0' class='elm-full-mode col-autosuspend'>" + autoSuspendLive + "</td>\
                        <td data-col='live' data-override='1' class='elm-full-mode col-autosuspend'>" + overrideAutoSuspendLive + "</td>\
                    </tr>";
        return html;
    };
    this.renderAutoSuspend = function (options) {
        var html = "<span class='icon-non-autosuspend elm-autosuspend-view "
            + (!options.canEditAutoSuspend || options.isAutoSuspend ? "display-none" : "") + "'></span>";

        html += options.copyToRelated
            ? "<span class='icon-copy2related-autosuspend elm-autosuspend-edit " + (!options.canEditAutoSuspend ? "display-none" : "") + "' title='" + options.copyToRelatedText + "'></span>"
            : "<span class='padding-icon-copy2related elm-autosuspend-edit " + (!options.canEditAutoSuspend ? "display-none" : "") + "'></span>";
        html += "<input class='chk-autosuspend elm-autosuspend-edit " + (!options.canEditAutoSuspend ? "display-none" : "") + "' type='checkbox' " +
            (options.isAutoSuspend ? "checked='checked'" : "") + " />";

        return html;
    };
    this.renderOverrideAutoSuspend = function (isOverrideAS, copyToRelated, copyToRelatedText) {
        var html = "<span class='icon-checked-override-autosuspend elm-autosuspend-view" + (isOverrideAS ? "" : " display-none") + "'></span>";
        html += copyToRelated
            ? "<span class='icon-copy2related-autosuspend elm-autosuspend-edit' title='" + copyToRelatedText + "'></span>"
            : "<span class='padding-icon-copy2related elm-autosuspend-edit'></span>";
        html += "<input class='chk-autosuspend elm-autosuspend-edit' type='checkbox'" + (isOverrideAS ? " checked='checked'" : "") + " />";

        return html;
    };
    this.renderSport = function (sport, rs) {
        var html = this.renderSportRow(sport, rs);

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
    this.bindGridEvents = function () {
        $(".group-toggle").on("click touchend", function () {
            self.pageHandler.toggleGroup($(this));
        });

        $(".icon-copy2child").on("click touchend", function () {
            self.pageHandler.copyToChildren($(this));
            self.updateAutoSuspendVisible();
        });

        $(".icon-copy2related").on("click touchend", function () {
            self.pageHandler.copyToRelated($(this));
            self.updateAutoSuspendVisible();
        });

        $(".btn-tick-all").on("click touchend", function () {
            self.checkAllChild($(this), true);
        });

        $(".btn-untick-all").on("click touchend", function () {
            self.checkAllChild($(this), false);
        });

        $(".icon-copy2related-autosuspend").on("click touchend", function () {
            self.copyToRelatedAutoSuspend($(this));
        });
        this.elements.tblVirtualRateReportMember.find(".ddl-rate").change(function () {
            self.rateChanged($(this));
        });
    };
    this.checkAllChild = function (elem, isChecked) {
        var td = elem.parent();
        var tr = td.parent();
        var level = tr.attr("data-level");
        var selector = "tr" + this.getRowSelector(level, tr);

        var colName = td.attr("data-col");
        var override = td.attr("data-override");
        selector += " [data-col='" + colName + "'][data-override='" + override + "'] .chk-autosuspend";

        this.elements.tblVirtualRateReportMember.find(selector).attr("checked", isChecked ? "checked" : null);
    };
    this.getRowSelector = function (level, parent) {
        var sportId = parent.attr("data-sportid");
        var gameId = parent.attr("data-gameid");
        var betTypeGroupId = parseInt(parent.attr("data-bettypegroupid"));

        var selector = "[data-sportid='" + sportId + "']";

        if (level >= 2) {
            selector += "[data-gameid='" + gameId + "']";
        }

        if (level >= 3) {
            selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
        }

        return selector;
    };
    this.copyToRelatedAutoSuspend = function (elem) {
        var td = elem.parent();
        var tr = td.parent();
        var colName = td.attr("data-col");
        var override = td.attr("data-override");
        var value = td.find(".chk-autosuspend").is(":checked");
        var betTypeId = parseInt(tr.attr("data-bettypeid"));
        var selector = "tr[data-level='4'][data-bettypeid='" + betTypeId + "'] [data-col='" + colName + "'][data-override='" + override + "'] .chk-autosuspend";
        this.elements.tblVirtualRateReportMember.find(selector).attr("checked", value ? "checked" : null);
    };
    this.rateChanged = function (elem) {
        var parent = elem.parent().parent();
        var level = parent.attr("data-level");
        if (level < 4) {
            this.updateAutoSuspendVisible();
            return;
        }

        var colName = elem.parent().attr("data-col");
        var threshold = parseInt(parent.attr("data-" + colName + "-threshold"));
        var td = parent.find("[data-col='" + colName + "'][data-override='0']");
        var value = this.getRealValue(elem);

        if (value < threshold) {
            td.find(".icon-non-autosuspend").addClass("display-none");
            td.find(".elm-autosuspend-edit").addClass("display-none");
            return;
        }

        td.find(".elm-autosuspend-edit").removeClass("display-none");
        if (!td.find(".chk-autosuspend").is(":checked")) {
            td.find(".icon-non-autosuspend").removeClass("display-none");
        }
    };
    this.getRealValue = function (elem) {
        var parent = elem.parent().parent();
        var colName = elem.parent().attr("data-col");
        var nonliveThreshold = parseInt(parent.attr("data-nonlive-threshold"));
        var liveThreshold = parseInt(parent.attr("data-live-threshold"));
        var value = parseInt(parent.find("[data-col='" + colName + "'] .ddl-rate").val());

        for (var i = 3; i > 0 && value == -200; i--) {
            var selector = "tr[data-level='" + i + "']" + this.getRowSelector(i, parent);
            var row = this.elements.tblVirtualRateReportMember.find(selector);
            value = parseInt(row.find("[data-col='" + colName + "'] .ddl-rate").val());
        }

        return value;
    };
    this.updateAutoSuspendVisible = function () {
        var ddlRates = $("tr[data-level='4'] .ddl-rate");
        ddlRates.each(function () {
            self.rateChanged($(this));
        });
    };
    this.getIsAutoSuspend = function (colName, sportId, gameId, betTypeGroupId, betTypeId) {
        var selector = "tr[data-sportid='" + sportId + "']";
        selector += "[data-gameid='" + gameId + "']";
        selector += "[data-bettypegroupid='" + betTypeGroupId + "']";
        selector += "[data-bettypeid='" + betTypeId + "']";

        var row = this.elements.tblVirtualRateReportMember.find(selector);
        var chk = row.find("[data-col='" + colName + "'][data-override='0'] .chk-autosuspend");
        var chkOverrideAS = row.find("[data-col='" + colName + "'][data-override='1'] .chk-autosuspend");

        var ddl = row.find("[data-col='" + colName + "'] .ddl-rate");
        var value = this.getRealValue(ddl);
        var threshold = parseInt(row.attr("data-" + colName + "-threshold"));

        return {
            isDisable: value < threshold,
            value: chk.is(":checked"),
            isOverrideAS: chkOverrideAS.is(":checked")
        };
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
        var currentValue = this.pageHandler.getCurrentValue();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/SaveVirtualRateDetails"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({
                Rates: currentValue,
                sportType: resources.SportType,
                custId: this.pageHandler.oldReport.CustId,
                IsGlobalSetting: false,
                isMember: true,
                isMemberCompactMode: this.isCompactMode
            }),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                if (rs.Code == 0) {
                    self.pageHandler.oldReport = currentValue;
                    self.oldReport = currentValue;
                    CMS.showSuccessMessage(rs.Message);
                    self.updateAutoSuspendIcon();
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
    this.getBetTypes = function (sport, game, betTypeGroup, betTypes) {
        var result = [];
        for (var i = 0; i < betTypes.length; i++) {
            var betType = betTypes[i];

            var nonlive = self.pageHandler.funcGetRateValue(4, "non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);
            var live = self.pageHandler.funcGetRateValue(4, "live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);

            var nonliveAutoSuspend = self.getIsAutoSuspend("non-live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);
            var liveAutoSuspend = self.getIsAutoSuspend("live", sport.GroupId, game.GroupId, betTypeGroup.GroupId, betType.GroupId);

            result.push({
                GroupId: betType.GroupId,
                LevelId: betType.LevelId,
                NonliveId: nonlive.id,
                NonLiveValue: nonlive.value,
                LiveId: live.id,
                LiveValue: live.value,
                NonliveAutoSuspend: nonliveAutoSuspend.value,
                LiveAutoSuspend: liveAutoSuspend.value,
                DisableNonliveAutoSuspend: nonliveAutoSuspend.isDisable,
                DisableLiveAutoSuspend: liveAutoSuspend.isDisable,
                IsOverrideASNonlive: nonliveAutoSuspend.isOverrideAS,
                IsOverrideASLive: liveAutoSuspend.isOverrideAS
            });
        }

        return result;
    };
    this.getRateValue = function (level, colName, sportId, gameId, betTypeGroupId, betTypeId) {
        var selector = "tr[data-level=" + level + "][data-sportid='" + sportId + "']";

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
        var dropdown = self.elements.tblVirtualRateReportMember.find(selector);
        return {
            id: dropdown.find("option:selected").attr("data-level-id"),
            value: dropdown.val()
        };
    };
    this.updateAutoSuspendIcon = function () {
        var chks = $("tr[data-level='4'] .chk-autosuspend:visible");
        chks.each(function () {
            self.autoSuspendChanged($(this));
        });
    };
    this.autoSuspendChanged = function (elem) {
        var parent = elem.parent();
        var isChecked = elem.is(":checked");
        if (isChecked) {
            parent.find(".icon-non-autosuspend").addClass("display-none");
        } else {
            parent.find(".icon-non-autosuspend").removeClass("display-none");
        }
    };
};
