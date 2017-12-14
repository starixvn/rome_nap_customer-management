(function ($) {
    "use strict";
    var totalColumns;
    var readPermissionColumnNumber = 7;
    var hostPermissionColumnNumber = 8;
    var domElements = {
        datePickerPlugin: null,
        sportDropdown: $("#ddl-sport"),
        leagueDropdown: $("#ddl-league"),
        matchDropdown: $("#ddl-match"),
        percentageUsedTextbox: $("#txt-percentageused"),
        userNameTextbox: $("#txt-username"),
        excludeCompletedEventsCheckbox: $("#chk-excludecompletedevents"),
        resultMessageDiv: $("#div-result-message"),
        tableTitleDiv: $("#div-smaturnover-grid-title"),
        maxLevelUsedCounterLabel: $("#lbl-max-level-used-counter"),
        tableBodyDiv: $("#sma-turnover-container"),
        tableBody: $("#sma-turnover-content tbody"),
        allCheckbox: null,
        multipleEditTHead: $(".th-multipleEdit-container")
    };
    var urls = {
        sport: rootUrl + "TurnoverControl/TurnoverMonitor/GetSports",
        league: rootUrl + "TurnoverControl/TurnoverMonitor/GetLeagues?",
        match: rootUrl + "TurnoverControl/TurnoverMonitor/GetMatches?",
        customerTurnover: ""
    };

    var maxTurnoverPercentage = 100;
    var isResetScroller = true;
    var filterParams = {};
    var gridOptions = null;

    var autoRefresh = new AutoRefresh(domElements.tableTitleDiv, [0, 30, 60, 90, 120], getCustomerTurnovers);

    function getSports() {
        if (globalSportType) {
            urls.sport += "?sportType=" + globalSportType;
        }

        CMS.getJson(
            urls.sport,
            function (data) {
                var sport;
                var html = [];
                var result = $.parseJSON(data);

                for (var i = 0, max = result.length; i < max; i++) {
                    sport = result[i];

                    html.push('<option value="');
                    html.push(sport.Id);
                    html.push('">');
                    html.push(sport.Name);
                    html.push("</option>");
                }

                domElements.sportDropdown.empty().append(html.join("")).val(result[0].Id);
                $("#ddl-sport.ui-combobox input").val(result[0].Name);

                getLeagues();
            });
    }

    function getLeagues() {
        var dateRange = domElements.datePickerPlugin.nexDatePicker("getDateRange");
        var queryString = $.param({
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            SportType: domElements.sportDropdown.val()
        });

        CMS.getJson(
            urls.league + queryString,
            function (data) {
                var league;
                var html = [];
                var result = $.parseJSON(data);

                for (var i = 0, max = result.length; i < max; i++) {
                    league = result[i];

                    html.push('<option value="');
                    html.push(league.Id);
                    html.push('">');
                    html.push(league.Name);
                    html.push("</option>");
                }

                domElements.leagueDropdown.empty().append(html.join("")).val(result[0].Id);
                $("#ddl-league.ui-combobox input").val(result[0].Name);

                getMatches();
            });
    }

    function getMatches() {
        var dateRange = domElements.datePickerPlugin.nexDatePicker("getDateRange");
        var queryString = $.param({
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            LeagueId: domElements.leagueDropdown.val()
        });

        CMS.getJson(
            urls.match + queryString,
            function (data) {
                var match;
                var html = [];
                var result = $.parseJSON(data);

                for (var i = 0, max = result.length; i < max; i++) {
                    match = result[i];

                    html.push('<option value="');
                    html.push(match.Id);
                    html.push('">');
                    html.push(match.Name);
                    html.push("</option>");
                }

                domElements.matchDropdown.empty().append(html.join("")).val(result[0].Id);
                $("#ddl-match.ui-combobox input").val(result[0].Name);
            });
    }

    function resetFilterParams() {
        var dateRange = domElements.datePickerPlugin.nexDatePicker("getDateRange");
        filterParams.FromDate = dateRange.FromDate;
        filterParams.ToDate = dateRange.ToDate;
        filterParams.SportType = domElements.sportDropdown.val();
        filterParams.LeagueId = domElements.leagueDropdown.val();
        filterParams.MatchId = domElements.matchDropdown.val();
        filterParams.PercentageUsed = parseInt($.trim(domElements.percentageUsedTextbox.val()));
        filterParams.UserName = $.trim(domElements.userNameTextbox.val());
        filterParams.IsExcludeCompletedEvent = domElements.excludeCompletedEventsCheckbox.is(":checked");
    }

    function getGridOptions() {
        return {
            tblContentId: "sma-turnover-content",
            manualCopyHeader: true,
            fullWidth: true,
            gridWidth: 925,
            gridHeight: 250,
            freezeWidth: 10,
            numberFreezeCol: 0,
            isResetScroller: true,
            isShowPager: false
        };
    }

    function getCustomerTurnovers() {
        CMS.hideInlineMsg(domElements.resultMessageDiv);
        isResetScroller = true;
        var hostPermission = filterParams.SportType === "1" ? soccerHostPermission : otherSportsHostPermission;

        if (hostPermission) {
            totalColumns = hostPermissionColumnNumber;
            domElements.multipleEditTHead.removeClass("display-none");
        } else {
            totalColumns = readPermissionColumnNumber;
            domElements.multipleEditTHead.addClass("display-none");
        }

        if (domElements.allCheckbox) {
            domElements.allCheckbox
                .prop("indeterminate", false)
                .prop("checked", false);
        }

        CMS.getJson(
            urls.customerTurnover + $.param(filterParams),
            function (data) {
                var result = $.parseJSON(data);
                var resultLength = result ? result.length : 0;

                if (resultLength === 0) {
                    renderNoData();

                    if (domElements.allCheckbox) {
                        domElements.allCheckbox.attr("disabled", true);
                    }
                }
                else {
                    renderCustomerTurnover(result);

                    if (domElements.allCheckbox) {
                        domElements.allCheckbox.removeAttr("disabled");
                    }
                }
            },
            undefined,
            function () {
                CMS.enableInput();

                if (autoRefresh.isPausing()) {
                    autoRefresh.start();
                }
            });
    }

    function renderNoData() {
        var html = [
            '<tr><td colspan="',
            totalColumns,
            '"><div class="align-center no-data-not-frozen">',
            viewResources.Message_NoReportData,
            "</div></td></tr>"
        ].join("");

        domElements.tableBody.empty().append(html);
        renderCustomerTurnover_AfterHtmlCreated(0);
    }

    function renderCustomerTurnover(data) {
        domElements.tableBody.empty();

        var html = [];
        var reachedMaxPercentageCount = 0;

        var league;
        var leagueLength = data.length;

        var match, matchLength;
        var turnover, turnoverLength;

        for (var i = 0; i < leagueLength; i++) {
            league = data[i];
            html.push(renderLeagueHeadline(league.LeagueName));

            matchLength = league.MatchTurnovers.length;

            for (var j = 0; j < matchLength; j++) {
                match = league.MatchTurnovers[j];
                html.push(renderMatchHeadline(match));

                turnoverLength = match.CustomerTurnovers.length;

                for (var k = 0; k < turnoverLength; k++) {
                    turnover = match.CustomerTurnovers[k];
                    html.push(renderTurnover(turnover, match.EventDate));

                    if (turnover.PercentageUsed >= maxTurnoverPercentage) {
                        reachedMaxPercentageCount++;
                    }
                }
            }
        }

        domElements.tableBody.append(html.join(""));
        renderCustomerTurnover_AfterHtmlCreated(reachedMaxPercentageCount);
    }

    function renderLeagueHeadline(leagueName) {
        return [
            '<tr><td class="rowLeague" colspan="',
            totalColumns,
            '"><div class="lblLeagueHeadline">',
            leagueName,
            "</div></td></tr>"
        ].join("");
    }

    function renderMatchHeadline(match) {
        return [
            '<tr><td class="rowmatch" colspan="',
            totalColumns,
            '"><span class="bold">',
            match.HomeName,
            match.IsNeutral ? " (N)" : "",
            " -vs- ",
            match.AwayName,
            '</span><span class="rightArrow_MatchInfo"></span><span>',
            viewResources.Event_Date,
            ": ",
            match.EventDate,
            '</span><span class="rightArrow_MatchInfo"></span><span class="eventStatus">',
            match.EventStatus,
            "</span></td></tr>"
        ].join("");
    }

    function renderTurnover(turnover, eventDate) {
        var html = [];

        var eventStatus = turnover.EventStatus.toLowerCase();
        var hostPermission = turnover.SportType === 1 ? soccerHostPermission : otherSportsHostPermission;
        var canEditMaxTurnover = hostPermission && eventStatus !== "completed";
        var isGroupHeadline = turnover.Level.toLowerCase() === "headline";

        html.push('<tr id="tr');
        html.push(turnover.CustId);
        html.push('">');

        if (hostPermission) {
            html.push('<td><div class="col-checkbox-body align-center">');

            if (eventStatus !== "completed") {
                html.push('<input type="checkbox" data-custid="');
                html.push(turnover.CustId);
                html.push('" data-maxturnover="');
                html.push(turnover.MaxTurnover);
                html.push('" data-matchid="');
                html.push(turnover.MatchId);
                html.push('" data-isgroupheadline="');
                html.push(isGroupHeadline);
                html.push('" data-sporttype="');
                html.push(turnover.SportType);
                html.push('"/>');
            }

            html.push("</div></td>");
        }

        html.push('<td><div class="col-sma-name-body align-left">');
        html.push(turnover.UserName);
        html.push('</div></td>');

        html.push('<td><div class="col-level-body align-center">');
        html.push(turnover.Level);
        html.push('</div></td>');

        html.push('<td><div class="col-currency-body align-center">');
        html.push(turnover.Currency);
        html.push('</div></td>');

        html.push('<td><div class="col-member-turnover-body align-right">');
        html.push(numeral(turnover.TotalTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('</div></td>');

        html.push('<td><div class="col-company-turnover-body align-right"><a class="c hand" href="javascript:;" data-custid="');
        html.push(turnover.CustId);
        html.push('" data-matchid="');
        html.push(turnover.MatchId);
        html.push('" data-isgroupheadline="');
        html.push(isGroupHeadline);
        html.push('" data-eventdate="');
        html.push(eventDate);
        html.push('">');
        html.push(numeral(turnover.HouseTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('</a></div></td>');

        if (canEditMaxTurnover) {
            html.push('<td><div class="col-max-turnover-body align-right" id="div');
            html.push(turnover.CustId);
            html.push('"><span class="max-turnover-container" data-custid="');
            html.push(turnover.CustId);
            html.push('" data-username="');
            html.push(turnover.UserName);
            html.push('" data-matchid="');
            html.push(turnover.MatchId);
            html.push('" data-isgroupheadline="');
            html.push(isGroupHeadline);
            html.push('" data-sporttype="');
            html.push(turnover.SportType);
            html.push('"><a class="c hand" href="javascript:;" id="link');
            html.push(turnover.CustId);
            html.push('">');
            html.push(numeral(turnover.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('</a><span class="ico-edit" title="');
            html.push(viewResources.Edit);
            html.push('"></span></span></div></td>');
        }
        else {
            html.push('<td class="td-maxturnover"><div class="col-max-turnover-body align-right table-cell">');
            html.push(numeral(turnover.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('</div></td>');
        }

        var percentageUsed = Math.floor(turnover.PercentageUsed * 100) / 100;

        if (percentageUsed >= maxTurnoverPercentage) {
            html.push('<td><div class="col-percentage-used-body align-center boxPercentUsed_100Percent">');
            html.push(numeral(percentageUsed).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('%</div></td>');
        }
        else {
            html.push('<td><div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -');
            html.push(100 - percentageUsed);
            html.push('px">');
            html.push(numeral(percentageUsed).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('%</div></td>');
        }

        return html.join("");
    }

    function renderCustomerTurnover_AfterHtmlCreated(reachedMaxPercentageCount) {
        domElements.maxLevelUsedCounterLabel.html(numeral(reachedMaxPercentageCount).format(CMSConstants.NumberFormat.IntNumber));
        domElements.tableTitleDiv.removeClass("display-none");

        gridOptions = gridOptions || getGridOptions();
        gridOptions.isResetScroller = isResetScroller;

        domElements.tableBodyDiv.removeClass("display-none").gridScroller(gridOptions);
    }

    function initDateRange() {
        $("#fromtodate-picker").nexDatePicker({
            nexDatePickerType: "daterange",
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: new Date(minDate)
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: viewResources.From_Date,
                InitDate: new Date(currentDate),
                LabelCss: "lbl-field col_3",
                TextboxCss: "edit-field col_7"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: "",
                InitDate: new Date(currentDate),
                CurrentDate: new Date(currentDate),
                LabelCss: "col_0",
                TextboxCss: "edit-field col_7"
            },
            onSelectDatePicker: getLeagues
        });

        domElements.datePickerPlugin = $("#fromtodate-picker");
    }

    function initialize() {
        domElements.sportDropdown.change(getLeagues);
        domElements.leagueDropdown.change(getMatches);
        domElements.percentageUsedTextbox.keypress(CMS.forceNumberInput);

        var turnoverMonitorForm = $("#formTurnoverMonitor");
        urls.customerTurnover = turnoverMonitorForm.attr("action") + "?";
        turnoverMonitorForm.submit(function (e) {
            e.preventDefault();

            if (autoRefresh.isRunning()) {
                autoRefresh.pause();
            }

            resetFilterParams();
            getCustomerTurnovers();
        });

        autoRefresh.initialize(0);

        if (hasHostPermission) {
            var editHandler = new EditMaxTurnover(domElements.tableBody, domElements.resultMessageDiv, getCustomerTurnovers);
            editHandler.initialize();

            var multipleEditHandler = new EditMultipleMaxTurnover(domElements.tableBody, getCustomerTurnovers);
            multipleEditHandler.initialize();

            domElements.allCheckbox = multipleEditHandler.getAllCheckbox();
        }

        var popupHandler = new HouseTurnoverPopup(domElements.tableBody);
        popupHandler.initialize();

        // Remove the #divPopupContent element since it was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").empty();

        setTimeout(function () {
            initDateRange();

            getSports();
        }, 100);
    }

    initialize();
})(jQuery);