"use strict";
(function ($) {
    var totalColumns;
    var readPermissionColumnNumber = 10;
    var hostPermissionColumnNumber = 11;
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
        customerTurnover: ""
    };

    var maxTurnoverPercentage = 100;
    var isResetScroller = true;
    var filterParams = {};
    var gridOptions = null;

    var autoRefresh = new AutoRefresh(domElements.tableTitleDiv, [0, 30, 60, 90, 120], getCustomerTurnovers);

    function getSports() {
        var sportUrl = CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/GetSports");

        if (globalSportType) {
            sportUrl += "?sportType=" + globalSportType;
        }

        CMS.getJson(
            sportUrl,
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
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/GetLeagues?") + queryString,
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
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/GetMatches?") + queryString,
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
            gridHeight: 250,
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
        try {
            domElements.tableBody.empty();

            var html = [];
            var reachedMaxPercentageCount = 0;

            var league;
            var leagueLength = data.length;

            var match, matchLength;
            var customer, customerLength;

            for (var i = 0; i < leagueLength; i++) {
                league = data[i];
                html.push(renderLeagueHeadline(league.LeagueName));

                matchLength = league.MatchTurnovers.length;

                for (var j = 0; j < matchLength; j++) {
                    match = league.MatchTurnovers[j];
                    html.push(renderMatchHeadline(match));

                    customerLength = match.CustomerTurnovers.length;

                    for (var k = 0; k < customerLength; k++) {
                        customer = match.CustomerTurnovers[k];
                        html.push(renderCustomer(customer, match.EventDate));

                        if (customer.PercentageUsed >= maxTurnoverPercentage) {
                            reachedMaxPercentageCount++;
                        }
                    }
                }
            }

            domElements.tableBody.append(html.join(""));
            renderCustomerTurnover_AfterHtmlCreated(reachedMaxPercentageCount);
        } catch (e) {
            CMS.showErrorMessage(e.message);
            CMS.enableInput();
        }
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
            match.MatchName,
            '</span><span class="rightArrow_MatchInfo"></span><span>',
            viewResources.Event_Date,
            ": ",
            match.EventDate,
            '</span><span class="rightArrow_MatchInfo"></span><span class="eventStatus">',
            match.EventStatus,
            "</span></td></tr>"
        ].join("");
    }

    function renderCustomer(customer, eventDate) {
        var html = [];

        var eventStatus = customer.EventStatus.toLowerCase();
        var hostPermission = customer.SportType === 1 ? soccerHostPermission : otherSportsHostPermission;
        var canEditMaxTurnover = hostPermission && eventStatus !== "completed";

        html.push('<tr id="tr');
        html.push(customer.CustId);
        html.push('">');

        if (hostPermission) {
            html.push('<td><div class="col-checkbox-body align-center">');

            if (eventStatus !== "completed") {
                html.push('<input type="checkbox" data-custid="');
                html.push(customer.CustId);
                html.push('" data-matchid="');
                html.push(customer.MatchId);
                html.push('" data-isgroupheadline="');
                html.push(customer.IsGroupHeadline);
                html.push('" data-sporttype="');
                html.push(customer.SportType);
                html.push('"/>');
            }

            html.push("</div></td>");
        }

        html.push('<td><div class="col-sma-name-body align-left">');
        html.push(customer.Username);
        html.push('</div></td>');

        html.push('<td><div class="col-level-body align-center">');
        html.push(customer.Level);
        html.push('</div></td>');

        html.push('<td><div class="col-currency-body align-center">');
        html.push(customer.Currency);
        html.push('</div></td>');

        html.push('<td><div class="col-member-turnover-body align-right">');
        html.push(numeral(customer.TotalMemberTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('</div></td>');

        html.push('<td><div class="col-company-turnover-body align-right"><a class="c hand" href="javascript:;" data-custid="');
        html.push(customer.CustId);
        html.push('" data-matchid="');
        html.push(customer.MatchId);
        html.push('" data-isgroupheadline="');
        html.push(customer.IsGroupHeadline);
        html.push('" data-eventdate="');
        html.push(eventDate);
        html.push('">');
        html.push(numeral(customer.TotalHouseTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('</a></div></td>');

        renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, customer.HDPML);
        renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, customer.OU);
        renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, customer._1HHDP1HML);
        renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, customer._1HOU);
        renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, customer.Others);

        return html.join("");
    }

    function renderLiveAndNonLiveMaxTurnover(html, canEditMaxTurnover, customer, maxTurnovers) {
        html.push('<td><div class="col-max-turnover-body">');
        renderMaxTurnoverCell(html, canEditMaxTurnover, customer, maxTurnovers[0], viewResources.Live);

        html.push('<div class="clear-both" style="padding:4px 0"></div>');
        renderMaxTurnoverCell(html, canEditMaxTurnover, customer, maxTurnovers[1], viewResources.NL);

        html.push('<div class="clear-both"></div></div></td>');
        return html;
    }

    function renderMaxTurnoverCell(html, canEditMaxTurnover, customer, turnover, label) {
        var percentageUsed = Math.floor(turnover.PercentageUsed * 100) / 100;

        html.push('<div><div class="float-left"><span class="live-nl">');
        html.push(label);
        html.push('</span><div class="boxPercentUsed boxPercentUsed_');
        html.push(mapPercentUsedToCssClass(percentageUsed))
        html.push('"');

        if (percentageUsed < maxTurnoverPercentage) {
            html.push(' style="background-position: -');
            html.push(100 - percentageUsed);
            html.push('px"');
        }

        html.push(' title="');
        html.push(viewResources.HouseTO);
        html.push(numeral(turnover.HouseTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('">');
        html.push(numeral(percentageUsed).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        html.push('%</div></div>');

        if (canEditMaxTurnover && turnover.MaxTurnover) {
            html.push('<div class="float-right" id="div');
            html.push(customer.CustId);
            html.push('"><span class="max-turnover-container" data-custid="');
            html.push(customer.CustId);
            html.push('" data-username="');
            html.push(customer.Username);
            html.push('" data-matchid="');
            html.push(customer.MatchId);
            html.push('" data-isgroupheadline="');
            html.push(customer.IsGroupHeadline);
            html.push('" data-sporttype="');
            html.push(customer.SportType);
            html.push('" data-profileid="');
            html.push(turnover.ProfileId);
            html.push('"><a class="c hand" href="javascript:;" id="link');
            html.push(customer.CustId);
            html.push('">');
            html.push(numeral(turnover.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('</a><span class="ico-edit" title="');
            html.push(viewResources.Edit);
            html.push('"></span></span></div>');
        } else {
            html.push('<div class="float-right table-cell">');
            html.push(numeral(turnover.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            html.push('</div>');
        }

        html.push("</div>");
        return html;
    }

    function mapPercentUsedToCssClass(percentageUsed) {
        if (percentageUsed >= maxTurnoverPercentage) {
            return "100";
        }

        if (percentageUsed >= 90) {
            return "90_100";
        }

        if (percentageUsed >= 80) {
            return "80_90";
        }

        if (percentageUsed >= 70) {
            return "70_80";
        }

        if (percentageUsed >= 60) {
            return "60_70";
        }

        if (percentageUsed >= 50) {
            return "50_60";
        }

        if (percentageUsed >= 40) {
            return "40_50";
        }

        if (percentageUsed >= 30) {
            return "30_40";
        }

        if (percentageUsed >= 20) {
            return "20_30";
        }

        return "0_20";
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

    $(function () {
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
    });
})(jQuery);