AutoSuspendLog = function () {
    var self = this;
    this.elements = {
        txtUsername: null,
        fromtoDatePicker: null,
        iconCollapse: null,
        reportContainer: null,
        tblVirtualRateReport: null
    };

    this.init = function () {
        this.cachedElements();
        this.bindEvents();
    };
    this.cachedElements = function () {
        this.elements.txtUsername = $("#txt-username");
        this.elements.fromtoDatePicker = $('#fromtodate-picker');
        this.elements.reportContainer = $("#report-container");
        this.elements.iconCollapse = $("#ico-collapse");
        this.elements.reportTitle = $("#report-tile");
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
                TextboxCss: 'edit-field col_5'
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
            url: CMS.resolveRootUrl("VirtualRateDetails/GetAutoSuspendLogs"),
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
        var title = resources.AutoSuspendLog +
            " - " + rs.Username + " - " + resources.From + " " + rs.FromDateText + " " + resources.To + " " + rs.ToDateText;
        this.elements.reportTitle.html(title);
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

        this.elements.reportContainer.removeClass("display-none");
    };
    this.renderLog = function (log) {
        var html = "";
        html += "<tr data-level='0' data-log-id='" + log.LogId + "'>\
                    <td colspan='3'>\
                        <span class='group-toggle group-expand'>[−]</span>" + log.UpdatedDateText + " - " + resources.Update + " " + log.Username + " - " + resources.By + " " + log.Editor;
        html += "</td></tr>";

        for (var i = 0; i < log.Sports.length; i++) {
            html += this.renderBetTypeLog(log, log.Sports[i]);
        }

        return html;
    };
    this.renderBetTypeLog = function (log, betType) {
        var html = "";
        html += "<tr data-level='1' data-log-id='" + log.LogId + "'>\
                    <td>" + betType.SportName + " > " + betType.LeagueGroupName + " > " + betType.BetTypeGroupName + " > " + betType.BetTypeName + "</td>"
            + this.renderStatus(betType.NewNonliveAutoSuspend, betType.NonliveAutoSuspend)
            + this.renderStatus(betType.NewLiveAutoSuspend, betType.LiveAutoSuspend) +
            "</tr>";
        return html;
    };
    this.renderStatus = function (newStatus, oldStatus) {
        var text = this.getStatusText(oldStatus);
        var newText = this.getStatusText(newStatus);

        return newText == text
            ? ("<td class='align-center'>" + newText + "</td>")
            : ("<td class='has-changes align-center'><div class='new-value'>" + newText + "</div><div class='old-value'>" + text + "</div></td>");
    };
    this.getStatusText = function (status) {
        return status == undefined
            ? "-"
            : status ? resources.Yes : resources.No;
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
        for (var i = level + 1; i <= 1; i++) {
            $("tr[data-level='" + i + "']" + selector).addClass("display-none");
        }
    };
    this.showAllChild = function (level, parent) {
        var selector = this.getRowSelector(parent, level);
        for (var i = level + 1; i <= 1; i++) {
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
    this.getRowSelector = function (row, level) {
        var logId = row.attr("data-log-id");
        var selector = "[data-log-id='" + logId + "']";
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
};

$(document).ready(function () {
    var autoSuspendLog = new AutoSuspendLog();

    autoSuspendLog.init();
});
