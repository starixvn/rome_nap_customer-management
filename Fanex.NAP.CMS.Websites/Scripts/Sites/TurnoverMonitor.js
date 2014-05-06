/* File Created: April 16, 2014 */
/* global Namespace, Fanex, CMS, Layout, Validator, labels, datepickerSetting, messages */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.TurnoverMonitor();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.TurnoverMonitor");
Fanex.CMS.TurnoverMonitor = function () {
    var $this = this;
    var urls = {
        GetSportList: CMS.resolveRootUrl('TurnoverMonitor/GetSportList'),
        GetLeaguesOfSport: CMS.resolveRootUrl('TurnoverMonitor/GetLeaguesOfSport'),
        GetMatchesOfLeague: CMS.resolveRootUrl('TurnoverMonitor/GetMatchesOfLeague'),
        GetTurnoverReport: CMS.resolveRootUrl('TurnoverMonitor/GetTurnoverReport')
    };
    var domElements = {
        divDatepickerPlugin: null,
        ddlSelectTop: null,
        ddlSport: null,
        txtSport: null,
        ddlLeague: null,
        txtLeague: null,
        ddlMatch: null,
        txtMatch: null,
        txtSMAName: null,
        txtPercentageUsed: null,
        btnSearch: null,
        divTurnoverMonitor: null
    };
    var parameters = {
        FromDate: null,
        ToDate: null,
        SelectTop: null,
        SportID: null,
        LeagueID: null,
        MatchID: null,
        SMAName: null,
        Used: null
    };
    var runningMode = 2;

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divDatepickerPlugin = $('#divDatepickerPlugin');
        domElements.ddlSelectTop = $('#ddlSelectTop');
        domElements.ddlSport = $('#ddlSport');
        domElements.txtSport = domElements.ddlSport.next().children('input');
        domElements.ddlLeague = $('#ddlLeague');
        domElements.txtLeague = domElements.ddlLeague.next().children('input');
        domElements.ddlMatch = $('#ddlMatch');
        domElements.txtMatch = domElements.ddlMatch.next().children('input');
        domElements.txtSMAName = $('#txtSMAName');
        domElements.txtPercentageUsed = $('#txtPercentageUsed');
        domElements.btnSearch = $('#btnSearch');
        domElements.divTurnoverMonitor = $('#divTurnoverMonitor');
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        Layout.createDateRange(
            domElements.divDatepickerPlugin,
            labels.FromDate,
            'col_8',
            labels.ToDate,
            'col_12',
            datepickerSetting.Today,
            datepickerSetting.MoveBetsDay,
            null,
            refreshSportDropdown);
        refreshSportDropdown();

        domElements.txtPercentageUsed.numberField(0, false, null);

        domElements.ddlSport.on('change', function () {
            parameters.SportID = domElements.ddlSport.val();
            getLeaguesOfSport(parameters.SportID, parameters.FromDate, parameters.ToDate);
        });

        domElements.ddlLeague.on('change', function () {
            parameters.LeagueID = domElements.ddlLeague.val();
            getMatchesOfLeague(parameters.LeagueID, parameters.FromDate, parameters.ToDate);
        });

        domElements.btnSearch.on('click', function () {
            if (validateFilterForm()) {
                //var dateRange = domElements.divDatepickerPlugin.nexDatePicker('getDateRange');

                //parameters.FromDate = dateRange.FromDate;
                //parameters.ToDate = dateRange.ToDate;
                parameters.SelectTop = domElements.ddlSelectTop.val();
                //parameters.SportID = domElements.ddlSport.val();
                //parameters.LeagueID = domElements.ddlLeague.val();
                parameters.MatchID = domElements.ddlMatch.val();
                parameters.SMAName = domElements.txtSMAName.val();
                parameters.Used = domElements.txtPercentageUsed.val().replace(/,/g, '');

                getTurnoverReport();
            }
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var progressBarFormat = '<div class="percentage-used" data-value="{0}"><div class="percentage-label">{0}</div></div>';
        var gridOptions = {
            columns: [{ HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 70, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.GivenCredit, Width: 150, Name: 'ColGivenCredit', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MaxTORate, Width: 120, Name: 'ColMaxTORate', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.NumberFormat
                      },
                      { HeaderText: labels.MaxTOAmount, Width: 150, Name: 'ColMaxTOAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MemberTO, Width: 150, Name: 'ColMemberTO', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.PercentageUsed, Width: 150, Name: 'ColPercentageUsed', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 150,
            showDistinctRow: false
        };
        var bodyData = [];
        var row;

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                row = {
                    Columns: [{ Name: 'ColSMAName', Value: item.Username },
                              { Name: 'ColLevel', Value: item.Level },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColGivenCredit', Value: item.GivenCredit },
                              { Name: 'ColMaxTORate', Value: item.MaxTORate },
                              { Name: 'ColMaxTOAmount', Value: item.MaxTOAmount },
                              { Name: 'ColMemberTO', Value: item.MemberTO },
                              { Name: 'ColPercentageUsed', Value: progressBarFormat.format(item.PercentageUsed)}]
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divTurnoverMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divTurnoverMonitor.find('div.percentage-used').each(function () {
            var $obj = $(this);
            var percentage = $obj.attr('data-value');

            $obj.progressbar({
                value: parseInt(percentage)
            });
        });
    };

    // Update Sport dropdown list.
    var refreshSportDropdown = function () {
        var dateRange = domElements.divDatepickerPlugin.nexDatePicker('getDateRange');

        parameters.FromDate = dateRange.FromDate;
        parameters.ToDate = dateRange.ToDate;

        // Reset Match dropdown list.
        domElements.ddlLeague.html('');
        domElements.txtLeague.val('');

        getSportList(parameters.FromDate, parameters.ToDate);
    };

    // Get list of sports which are held in a date range.
    var getSportList = function (fromDate, toDate) {
        var gettingSportsParameters = {
            FromDate: fromDate,
            ToDate: toDate,
            Mode: runningMode
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSportList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(gettingSportsParameters),
            success: function (result) {
                updateSportDropdown(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Update Sport drop-down list.
    var updateSportDropdown = function (data) {
        // Clear old values.
        domElements.ddlSport.html('');

        // Add a default value.
        domElements.ddlSport.append($('<option>').attr('value', '').text(labels.SelectSport));
        domElements.txtSport.val(labels.SelectSport);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlSport.append($('<option></option>').attr('value', item.SportType).text(item.SportName));
            });
        }
    };

    // Get list of leagues of a specific sport.
    var getLeaguesOfSport = function (sportID, fromDate, toDate) {
        var gettingLeaguesParameters = {
            SportID: sportID,
            FromDate: fromDate,
            ToDate: toDate,
            Mode: runningMode
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetLeaguesOfSport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(gettingLeaguesParameters),
            success: function (result) {
                updateLeagueDropdown(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data) {
        // Clear old values.
        domElements.ddlLeague.html('');

        // Add a default value.
        domElements.ddlLeague.append($('<option>').attr('value', '').text(labels.SelectLeague));
        domElements.txtLeague.val(labels.SelectLeague);

        // Reset Match dropdown list.
        domElements.ddlMatch.html('');
        domElements.txtMatch.val('');

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlLeague.append($('<option>').attr('value', item.LeagueID).text(item.LeagueName));
            });
        }
    };

    // Get list of match of a specific league.
    var getMatchesOfLeague = function (leagueID, fromDate, toDate) {
        var gettingMatchesParameters = {
            LeagueID: leagueID,
            FromDate: fromDate,
            ToDate: toDate,
            Mode: runningMode
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetMatchesOfLeague,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(gettingMatchesParameters),
            success: function (result) {
                updateMatchDropdown(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Update Match drop-down list.
    var updateMatchDropdown = function (data) {
        // Clear old values.
        domElements.ddlMatch.html('');

        // Add new values.
        if (data && data.length > 0) {
            // Add a default value.
            domElements.ddlMatch.append($('<option>').attr('value', '').text(labels.SelectMatch));
            domElements.txtMatch.val(labels.SelectMatch);

            $.each(data, function (index, item) {
                domElements.ddlMatch.append($('<option>').attr('value', item.MatchID).text(item.MatchName));
            });
        }
    };

    // Validate filter form.
    var validateFilterForm = function () {
        var selectedSport = domElements.ddlSport.val();
        var selectedLeague = domElements.ddlLeague.val();
        var selectedMatch = domElements.ddlMatch.val();

        if (!selectedSport) {
            console.log(!selectedSport);
            Validator.showError(domElements.txtSport.parent(), messages.RequiredFieldError, 'w');

            return false;
        } else if (!selectedLeague) {
            Validator.showError(domElements.txtLeague.parent(), messages.RequiredFieldError, 'w');

            return false;
        } else if (!selectedMatch) {
            Validator.showError(domElements.txtMatch.parent(), messages.RequiredFieldError, 'w');

            return false;
        } else {
            return true;
        }
    };

    // Get SMA turnover report.
    var getTurnoverReport = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetTurnoverReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            //data: JSON.stringify(parameters),
            success: function (result) {
                renderGrid(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    $this.InitPage = function () {
        cacheDOM();
        initializeFields();
    };
};