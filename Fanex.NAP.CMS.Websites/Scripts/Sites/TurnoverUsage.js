/* File Created: April 16, 2014 */
/* global Namespace, numeral, Fanex, CMS, Layout, Validator, labels, datepickerSetting, messages */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.TurnoverUsage();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.TurnoverUsage");
Fanex.CMS.TurnoverUsage = function () {
    var $this = this;
    var urls = {
        GetSportList: CMS.resolveRootUrl('TurnoverUsage/GetSportList'),
        GetLeaguesOfSport: CMS.resolveRootUrl('TurnoverUsage/GetLeaguesOfSport'),
        GetMatchesOfLeague: CMS.resolveRootUrl('TurnoverUsage/GetMatchesOfLeague'),
        GetTurnoverUsageReport: CMS.resolveRootUrl('TurnoverUsage/GetTurnoverUsageReport')
    };
    var domElements = {
        divTurnoverMonitorForm: null,
        divDatepickerPlugin: null,
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
        SportType: null,
        LeagueID: null,
        MatchID: null,
        SMAName: null,
        UsedPercent: null
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divTurnoverMonitorForm = $('#divTurnoverMonitorForm');
        domElements.divDatepickerPlugin = $('#divDatepickerPlugin');
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
            null,
            'col_8',
            labels.ToDate,
            'col_5',
            'col_5',
            datepickerSetting.Today,
            datepickerSetting.MoveBetsDay,
            null,
            refreshSportDropdown);
        refreshSportDropdown();

        domElements.txtPercentageUsed.numberField(0, false, null).on('focusout', function () {
            var value = $.trim(domElements.txtPercentageUsed.val());

            domElements.txtPercentageUsed.val(value);
            Validator.clearError(domElements.txtPercentageUsed);
        });

        domElements.ddlSport.on('change', function () {
            parameters.SportType = domElements.ddlSport.val();
            getLeaguesOfSport(parameters.SportType, parameters.FromDate, parameters.ToDate);
        });

        domElements.ddlLeague.on('change', function () {
            parameters.LeagueID = domElements.ddlLeague.val();
            getMatchesOfLeague(parameters.LeagueID, parameters.FromDate, parameters.ToDate);
        });

        domElements.btnSearch.on('click', function () {
            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (Validator.countErrors(domElements.divTurnoverMonitorForm) === 0 && validateFilterForm()) {
                    parameters.MatchID = domElements.ddlMatch.val();
                    parameters.SMAName = $.trim(domElements.txtSMAName.val());
                    parameters.UsedPercent = domElements.txtPercentageUsed.val().replace(/,/g, '');
                    domElements.txtSMAName.val(parameters.SMAName);

                    getTurnoverReport();
                }
            }, 70);
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var progressBarFormat = '<div class="percentage-bar" data-value="{0}"><div class="percentage-label"><span>{1}%<span></div></div>';
        var percentageCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 70, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.GivenCredit, Width: 150, Name: 'ColGivenCredit', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MaxTORate, Width: 120, Name: 'ColMaxTORate', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.IntegerFormat
                      },
                      { HeaderText: labels.MaxTOAmount, Width: 150, Name: 'ColMaxTOAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MemberTO, Width: 150, Name: 'ColMemberTO', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.PercentageUsed, Width: 150, Name: 'ColPercentageUsed', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 200,
            showDistinctRow: false,
            showPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges,
                afterPagedFunction: handleGridEvents
            }
        };
        var bodyData = [];
        var row;

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                percentageCol = progressBarFormat.format(item.PercentageUsed, numeral(item.PercentageUsed).format(CMS.formats.IntegerFormat));
                row = {
                    Columns: [{ Name: 'ColSMAName', Value: item.Username },
                              { Name: 'ColLevel', Value: item.Level },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColGivenCredit', Value: item.GivenCredit },
                              { Name: 'ColMaxTORate', Value: item.MaxTORate },
                              { Name: 'ColMaxTOAmount', Value: item.MaxTOAmount },
                              { Name: 'ColMemberTO', Value: item.MemberTO },
                              { Name: 'ColPercentageUsed', Value: percentageCol}]
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divTurnoverMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divTurnoverMonitor.find('div.percentage-bar').each(function () {
            var $obj = $(this);
            var percentage = parseInt($obj.attr('data-value'));

            $obj.progressbar({
                value: percentage
            });

            if (percentage >= 100) {
                $obj.addClass('full-percents');
            }
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
        Validator.clearError(domElements.txtLeague.parent());
        domElements.ddlMatch.html('');
        domElements.txtMatch.val('');
        Validator.clearError(domElements.txtMatch.parent());
        Validator.clearError(domElements.txtSport.parent());

        getSportList(parameters.FromDate, parameters.ToDate);
    };

    // Get list of sports which are held in a date range.
    var getSportList = function (fromDate, toDate) {
        var gettingSportsParameters = {
            FromDate: fromDate,
            ToDate: toDate
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
    var getLeaguesOfSport = function (sportType, fromDate, toDate) {
        var gettingLeaguesParameters = {
            SportID: sportType,
            FromDate: fromDate,
            ToDate: toDate
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
        Validator.clearError(domElements.txtLeague.parent());
        Validator.clearError(domElements.txtMatch.parent());

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
            ToDate: toDate
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
        Validator.clearError(domElements.txtMatch.parent());

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
        var percentageUsed = domElements.txtPercentageUsed.val().replace(/,/g, '');

        if (!selectedSport) {
            Validator.showError(domElements.txtSport.parent(), messages.SelectOneOptionMessage, 'w');

            return false;
        } else if (!selectedLeague) {
            Validator.showError(domElements.txtLeague.parent(), messages.SelectOneOptionMessage, 'w');

            return false;
        } else if (!selectedMatch) {
            Validator.showError(domElements.txtMatch.parent(), messages.SelectOneOptionMessage, 'w');

            return false;
        } else if (percentageUsed !== '' && (!$.isNumeric(percentageUsed) || parseInt(percentageUsed) < 0)) {
            Validator.showError(domElements.txtPercentageUsed, messages.InvalidValueError, 'w');

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
            url: urls.GetTurnoverUsageReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
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