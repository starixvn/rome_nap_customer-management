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

    this.collapseReport = function () {
        var isCollapse = this.elements.iconCollapse.hasClass("ico-collapse");
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
        $(".group-toggle").on("click touchend", function () {
            self.toggleGroup($(this));
        });

        this.elements.reportContainer.removeClass("display-none");
    };
    this.renderLog = function (log) {
        var html = "";
        html += "<tr data-level='0' data-log-date='" + log.UpdatedDateText + "'>\
                    <td colspan='3'>\
                        <span class='group-toggle group-expand'>[−]</span>" + log.UpdatedDateText + " - " + resources.Update + " " + log.Username + " - " + resources.By + " " + log.Editor +
            "<span class='lbl-exchange-rate'>" + resources.ExchangeRate + ": " + numeral(log.ActualRate).format("#,##0.0000") + "</span>\
                    </td>\
                </tr>";

        for (var i = 0; i < log.NewSports.length; i++) {
            html += this.renderSportLog(log, log.NewSports[i], log.OldSports[i]);
        }

        return html;
    };
    this.renderSportLog = function (log, newSport, oldSport) {
        var html = "";
        html += "<tr data-level='1' data-log-date='" + log.UpdatedDateText + "' data-sport-id='" + newSport.GroupId + "'>\
                    <td><span class='group-toggle group-expand'>[−]</span>" + newSport.GroupName + "</td>\
                    <td>" + this.renderRate(newSport.NonliveLog, oldSport.NonliveLog) + "</td>\
                    <td>" + this.renderRate(newSport.LiveLog, oldSport.LiveLog) + "</td>\
                </tr>";

        for (var i = 0; i < newSport.Childs.length; i++) {
            html += this.renderLeagueLog(log, newSport, oldSport, newSport.Childs[i], oldSport.Childs[i]);
        }

        return html;
    };
    this.renderRate = function (newRate, oldRate) {
        return newRate == oldRate ? newRate : "<div class='new-value'>" + newRate + "</div><div class='old-value'>" + oldRate + "</div>";
    };
    this.renderLeagueLog = function (log, newSport, oldSport, newLeague, oldLeague) {
        var html = "";
        html += "<tr data-level='2' data-log-date='" + log.UpdatedDateText +
            "' data-sport-id='" + newSport.GroupId + "'" +
            " data-league-id='" + newLeague.GroupId + "'>\
                    <td><span class='group-toggle group-expand'>[−]</span>" + newLeague.GroupName + "</td>\
                    <td>" + this.renderRate(newLeague.NonliveLog, oldLeague.NonliveLog) + "</td>\
                    <td>" + this.renderRate(newLeague.LiveLog, oldLeague.LiveLog) + "</td>\
                </tr > ";

        for (var i = 0; i < newLeague.Childs.length; i++) {
            html += this.renderBetTypeGroupLog(log, newSport, oldSport, newLeague, oldLeague, newLeague.Childs[i], oldLeague.Childs[i]);
        }

        return html;
    };
    this.renderBetTypeGroupLog =
        function (log, newSport, oldSport, newLeague, oldLeague, newBetTypeGroup, oldBetTypeGroup) {
            var html = "";
            html += "<tr data-level='3' data-log-date='" + log.UpdatedDateText +
                "' data-sport-id='" + newSport.GroupId + "'" +
                " data-league-id='" + newLeague.GroupId + "'" +
                " data-bet-type-group-id='" + newBetTypeGroup.GroupId + "'" +
                ">\
                    <td><span class='group-toggle group-expand'>[−]</span>" + newBetTypeGroup.GroupName + "</td>\
                    <td>" + this.renderRate(newBetTypeGroup.NonliveLog, oldBetTypeGroup.NonliveLog) + "</td>\
                    <td>" + this.renderRate(newBetTypeGroup.LiveLog, oldBetTypeGroup.LiveLog) + "</td>\
                </tr > ";

            for (var i = 0; i < newBetTypeGroup.Childs.length; i++) {
                html += this.renderBetTypeLog(log, newSport, oldSport, newLeague, oldLeague, newBetTypeGroup, oldBetTypeGroup, newBetTypeGroup.Childs[i], oldBetTypeGroup.Childs[i]);
            }

            return html;
        };
    this.renderBetTypeLog =
        function (log, newSport, oldSport, newLeague, oldLeague, newBetTypeGroup, oldBetTypeGroup, newBetType, oldBetType) {
            var html = "";
            html += "<tr data-level='4' data-log-date='" + log.UpdatedDateText +
                "' data-sport-id='" + newSport.GroupId + "'" +
                " data-league-id='" + newLeague.GroupId + "'" +
                " data-bet-type-group-id='" + newBetTypeGroup.GroupId + "'" +
                ">\
                    <td>" + newBetType.GroupName + "</td>\
                    <td>" + this.renderRate(newBetType.NonliveLog, oldBetType.NonliveLog) + "</td>\
                    <td>" + this.renderRate(newBetType.LiveLog, oldBetType.LiveLog) + "</td>\
                </tr > ";

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
        var logDate = row.attr("data-log-date");
        var selector = "[data-log-date='" + logDate + "']";
        if (selector >= 1) {
            var sportId = row.attr("data-sport-id");
            selector += "[data-sport-id='" + sportId + "']";
        }

        if (selector >= 2) {
            var leagueId = row.attr("data-league-id");
            selector += "[data-league-id='" + leagueId + "']";
        }

        if (selector >= 3) {
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
};

$(document).ready(function () {
    var virtualRateDetailsViewLog = new VirtualRateDetailsViewLog();

    virtualRateDetailsViewLog.init();
});
