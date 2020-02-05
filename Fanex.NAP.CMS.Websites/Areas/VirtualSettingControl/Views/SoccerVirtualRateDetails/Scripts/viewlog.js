VirtualRateDetailsViewLog = function () {
    var self = this;
    this.elements = {
        txtUsername: null,
        fromtoDatePicker: null,
        iconCollapse: null,
        reportContainer: null,

        chkIncludeUpline: null,
        lblCurrency: null,
        tblVirtualRateReport: null
    };

    this.init = function () {
        this.cachedElements();
        this.bindEvents();
    };

    this.cachedElements = function () {
        this.elements.txtUsername = $("#txt-username");
        this.elements.fromtoDatePicker = $('#fromtodate-picker');
        this.elements.chkIncludeUpline = $('#chk-include-upline');

        this.elements.reportContainer = $("#report-container");
        this.elements.iconCollapse = $("#ico-collapse");
        this.elements.reportTitle = $("#report-tile");
        this.elements.lblCurrency = $(".lbl-curreny");
        this.elements.tblVirtualRateReport = $("#tbl-virtual-rate-report");
    };
    this.bindEvents = function () {
        this.elements.fromtoDatePicker.nexDatePicker({
            nexDatePickerType: 'daterange',
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
                Id: 'dpk-fromdate',
                Label: resources.From,
                InitDate: initFromDate,
                CurrentDate: currentDate,
                LabelCss: 'lbl-field col_2',
                TextboxCss: 'edit-field col_5'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: resources.To,
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: 'lbl-field col_2',
                TextboxCss: 'edit-field col_4'
            }
        });
        $('#dpk-fromdate').datepicker('setDate', initFromDate);

        $("#btn-search").on("click touchend", function () { self.enterSearch(); });
        this.elements.iconCollapse.on("click touchend", function () {
            self.collapseReport();
        });

        this.elements.txtUsername.on("keyup", function (e) {
            var code = e.keyCode || e.which;
            if (code === keyboardManager.Enums.KeyCode.Enter) {
                self.enterSearch();
            }
        });
    };

    this.enterSearch = function () {
        var username = this.elements.txtUsername.val().trim();
        if (username == "") {
            this.elements.reportContainer.addClass("display-none");
            Validator.showError(this.elements.txtUsername, resources.RequiredField, "n");
            return;
        }

        var fromtoDate = this.elements.fromtoDatePicker.nexDatePicker('getDateRange');
        var param = {
            username: username,
            sportType: resources.SportType,
            includeUpline: this.elements.chkIncludeUpline.is(":checked"),
            fromDate: fromtoDate.FromDate,
            toDate: fromtoDate.ToDate
        };

        this.getLogs(param);
    };
    this.getLogs = function (param) {
        Validator.clearError(self.elements.txtUsername);
        CMS.closeMsgBox();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl("VirtualRateDetails/GetLogs"),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(param),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (rs) {
                if (rs.ResultCode == 0) {
                    self.renderReport(rs);
                } else {
                    self.elements.reportContainer.addClass("display-none");
                    Validator.showError(self.elements.txtUsername, rs.ResultMessage, "n");
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
        var title = resources.VirtualRateLog +
            " - " + rs.Username + " - " + resources.From + " " + rs.FromDateText + " " + resources.To + " " + rs.ToDateText;
        this.elements.reportTitle.html(title);
        this.elements.lblCurrency.html(rs.Currency);
        this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");

        var html = "";
        if (rs.Logs.length == 0) {
            html += "<tr><td colspan='3' align='center'>" + resources.Message_NoReportData + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Logs.length; i++) {
                html += this.renderLog(rs.Logs[i]);
            }
        }

        this.elements.tblVirtualRateReport.find("tbody").empty().html(html);
        $("#tbl-virtual-rate-report tr").on("click touchend", function () {
            self.toggleGroup($(this).find(".group-toggle"));
        });
        this.collapseAll();
        this.elements.reportContainer.removeClass("display-none");
    };
    this.renderLog = function (log) {
        var html = "";
        html += "<tr data-level='0' data-log-id='" + log.LogId + "'>\
                    <td colspan='3'>\
                        <span class='group-toggle group-expand'>[−]</span>" + log.UpdatedDateText + " - " + resources.Update + " " + log.Username + " - " + resources.By + " " + log.Editor;

        if (log.ExchangeRate != -1) {
            html += "<span class='lbl-exchange-rate'>" + resources.ExchangeRate + ": " + log.ExchangeRateText + "</span>";
        }

        html += "</td></tr>";

        for (var i = 0; i < log.Sports.length; i++) {
            html += this.renderSportLog(log, log.Sports[i]);
        }

        return html;
    };
    this.renderSportLog = function (log, sport) {
        var html = "";
        html += "<tr data-level='1' data-log-id='" + log.LogId + "' data-sport-id='" + sport.GroupId + "'>\
                    <td><span class='group-toggle group-expand'>[−]</span>" + sport.GroupName + "</td>"
            + this.renderRate(sport.NewNonliveLog, sport.NonliveLog)
            + this.renderRate(sport.NewLiveLog, sport.LiveLog) +
            "</tr>";
        for (var i = 0; i < sport.Childs.length; i++) {
            html += this.renderLeagueLog(log, sport, sport.Childs[i]);
        }

        return html;
    };
    this.renderRate = function (newRate, oldRate) {
        return newRate == oldRate
            ? ("<td>" + newRate + "</td>")
            : ("<td class='has-changes'><div class='new-value'>" + newRate + "</div><div class='old-value'>" + oldRate + "</div></td>");
    };
    this.renderLeagueLog = function (log, sport, league) {
        var html = "";
        html += "<tr data-level='2' data-log-id='" + log.LogId +
            "' data-sport-id='" + sport.GroupId + "'" +
            " data-league-id='" + league.GroupId + "'>\
                    <td><span class='group-toggle group-expand'>[−]</span>" + league.GroupName + "</td>"
            + this.renderRate(league.NewNonliveLog, league.NonliveLog)
            + this.renderRate(league.NewLiveLog, league.LiveLog) +
            "</tr> ";

        for (var i = 0; i < league.Childs.length; i++) {
            html += this.renderBetTypeGroupLog(log, sport, league, league.Childs[i]);
        }

        return html;
    };
    this.renderBetTypeGroupLog =
        function (log, sport, league, betTypeGroup) {
            var html = "";
            html += "<tr data-level='3' data-log-id='" + log.LogId +
                "' data-sport-id='" + sport.GroupId + "'" +
                " data-league-id='" + league.GroupId + "'" +
                " data-bet-type-group-id='" + betTypeGroup.GroupId + "'" +
                ">\
                    <td><span class='group-toggle group-expand'>[−]</span>" + betTypeGroup.GroupName + "</td>"
                + this.renderRate(betTypeGroup.NewNonliveLog, betTypeGroup.NonliveLog)
                + this.renderRate(betTypeGroup.NewLiveLog, betTypeGroup.LiveLog) +
                "</tr> ";

            for (var i = 0; i < betTypeGroup.Childs.length; i++) {
                html += this.renderBetTypeLog(log, sport, league, betTypeGroup, betTypeGroup.Childs[i]);
            }

            return html;
        };
    this.renderBetTypeLog =
        function (log, sport, league, betTypeGroup, betType) {
            var html = "";
            html += "<tr data-level='4' data-log-id='" + log.LogId +
                "' data-sport-id='" + sport.GroupId + "'" +
                " data-league-id='" + league.GroupId + "'" +
                " data-bet-type-group-id='" + betTypeGroup.GroupId + "'" +
                ">\
                    <td>" + betType.GroupName + "</td>"
                + this.renderRate(betType.NewNonliveLog, betType.NonliveLog)
                + this.renderRate(betType.NewLiveLog, betType.LiveLog) +
                "</tr > ";

            return html;
        };
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
        var selector = this.getRowSelector(parent, level);
        for (var i = level + 1; i <= 4; i++) {
            $("tr[data-level='" + i + "']" + selector).addClass("display-none");
        }
    };
    this.showAllChild = function (level, parent) {
        var selector = this.getRowSelector(parent, level);
        for (var i = level + 1; i <= 4; i++) {
            $("tr[data-level='" + i + "']" + selector).removeClass("display-none");

            var toggles = $("tr[data-level='" + i + "']" + selector + " .group-toggle");
            for (var j = 0; j < toggles.length; j++) {
                this.forceToggle($(toggles[j]), true);
            }
        }
    };
    this.forceToggle = function (elem, isExpand) {
        var parent = elem.parent().parent();

        if (isExpand) {
            elem.removeClass("group-collapse").addClass("group-expand");
        } else {
            elem.removeClass("group-expand").addClass("group-collapse");
        }

        var text = isExpand ? "[−]" : "[+]";
        elem.html(text);
    };
    this.getRowSelector = function (row, level) {
        var logId = row.attr("data-log-id");
        var selector = "[data-log-id='" + logId + "']";
        if (level >= 1) {
            var sportId = row.attr("data-sport-id");
            selector += "[data-sport-id='" + sportId + "']";
        }

        if (level >= 2) {
            var leagueId = row.attr("data-league-id");
            selector += "[data-league-id='" + leagueId + "']";
        }

        if (level >= 3) {
            var betTypeGroupId = row.attr("data-bet-type-group-id");
            selector += "[data-bet-type-group-id='" + betTypeGroupId + "']";
        }

        return selector;
    };
    this.collapseReport = function () {
        var isCollapse = this.elements.iconCollapse.hasClass("ico-collapse");

        if (isCollapse) {
            this.elements.iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
        } else {
            this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }

        var toggles = $("tr[data-level='0'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            var elem = $(toggles[i]);
            this.forceToggle(elem, !isCollapse);

            var parent = elem.parent().parent();
            if (isCollapse) {
                this.hideAllChild(0, parent);
            } else {
                this.showAllChild(0, parent);
            }
        }
    };
    this.checkCollapseExpandAll = function () {
        var isCollapseAll = $("tr[data-level='0'] .group-toggle.group-collapse").length == $("tr[data-level='0'] .group-toggle").length;
        if (isCollapseAll) {
            this.elements.iconCollapse.removeClass("ico-collapse").addClass("ico-expand");
            return;
        }

        var isExpandAll = $("tr .group-toggle.group-expand").length == $("tr .group-toggle").length;
        if (isExpandAll) {
            this.elements.iconCollapse.removeClass("ico-expand").addClass("ico-collapse");
        }
    };
    this.collapseAll = function () {
        this.elements.iconCollapse.removeClass("ico-collapse").addClass("ico-expand");

        var toggles = $("tr[data-level='0'] .group-toggle");
        for (var i = 0; i < toggles.length; i++) {
            var elem = $(toggles[i]);
            this.forceToggle(elem, false);
            var parent = elem.parent().parent();
            this.hideAllChild(0, parent);
        }
    };
};

$(document).ready(function () {
    var virtualRateDetailsViewLog = new VirtualRateDetailsViewLog();

    virtualRateDetailsViewLog.init();
});
