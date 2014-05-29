/* File Created: April 16, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.TurnoverMonitor();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.TurnoverMonitor");
Fanex.CMS.TurnoverMonitor = function () {
    var $this = this;
    var urls = {
        GetInitializedData: CMS.resolveRootUrl('TurnoverMonitor/GetInitializedData'),
        GetLeaguesOfSport: CMS.resolveRootUrl('TurnoverMonitor/GetLeaguesOfSport'),
        GetMatchesOfLeague: CMS.resolveRootUrl('TurnoverMonitor/GetMatchesOfLeague'),
        GetTurnoverReport: CMS.resolveRootUrl('TurnoverMonitor/GetTurnoverReport')
    };
    var domElements = {
        divTurnoverMonitorForm: null,
        ddlSport: null,
        txtSport: null,
        ddlLeague: null,
        txtLeague: null,
        txtPercentageUsed: null,
        btnSearch: null,
        divLiveTurnoverMonitor: null,
        divLiveTurnoverMonitorContainer: null,
        divNoneLiveTurnoverMonitor: null,
        divNoneLiveTurnoverMonitorContainer: null,
        divRefresh: null
    };
    var parameters = {
        SportType: null,
        LeagueID: null,
        UsedPercent: null
    };
    var cachedData = {
        LiveData: null,
        NonLiveData: null,
        LiveDataLength: null,
        NonLiveDataLength: null,
        Length: 0,
        PageSize: CMS.pageSize,
        PageIndex: 0
    };
    var refreshTimer = {
        Interval: [30, 60, 90, 120],
        Default: 30
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divTurnoverMonitorForm = $('#divTurnoverMonitorForm');
        domElements.ddlSport = $('#ddlSport');
        domElements.txtSport = domElements.ddlSport.next().children('input');
        domElements.ddlLeague = $('#ddlLeague');
        domElements.txtLeague = domElements.ddlLeague.next().children('input');
        domElements.txtPercentageUsed = $('#txtPercentageUsed');
        domElements.divRefresh = $('#divAutoRefresh');
        domElements.btnSearch = $('#btnSearch');
        domElements.divLiveTurnoverMonitor = $('#divLiveTurnoverMonitor');
        domElements.divLiveTurnoverMonitorContainer = $('#divLiveTurnoverMonitorContainer');
        domElements.divNoneLiveTurnoverMonitor = $('#divNoneLiveTurnoverMonitor');
        domElements.divNoneLiveTurnoverMonitorContainer = $('#divNoneLiveTurnoverMonitorContainer');
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        domElements.txtPercentageUsed.numberField(0, false, null).on('focusout', function () {
            var value = $.trim(domElements.txtPercentageUsed.val());

            domElements.txtPercentageUsed.val(value);
            Validator.clearError(domElements.txtPercentageUsed);
        });

        domElements.ddlSport.on('change', function () {
            var sportID = domElements.ddlSport.val();

            getLeaguesOfSport(sportID);
        });

        domElements.txtPercentageUsed.on('keyup', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.divRefresh = domElements.divRefresh.autoRefresh(refreshTimer.Interval, refreshTimer.Default, getTurnoverReport);

        domElements.btnSearch.on('click', function () {
            cachedData.PageIndex = 0;
            cachedData.PageSize = CMS.pageSize;

            domElements.divRefresh.removeClass('disabled');

            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (Validator.countErrors(domElements.divTurnoverMonitorForm) === 0 && validateFilterForm()) {
                    parameters.SportType = domElements.ddlSport.val();
                    parameters.LeagueID = domElements.ddlLeague.val();
                    parameters.UsedPercent = domElements.txtPercentageUsed.val().replace(/,/g, '');

                    getTurnoverReport();
                }
            }, 70);
        });
    };

    // Validate filter form.
    var validateFilterForm = function () {
        var percentageUsed = domElements.txtPercentageUsed.val().replace(/,/g, '');

        if (percentageUsed !== '' && (!$.isNumeric(percentageUsed) || parseInt(percentageUsed) < 0)) {
            Validator.showError(domElements.txtPercentageUsed, messages.InvalidValueError, 'w');

            return false;
        } else {
            return true;
        }
    };

    // Render grid.
    var renderGrid = function (isLive, data, totalItems, container) {
        var groupFormat = '<span class="group-title">' +
                              '<span class="league" title="{0}">{1}</span> -- ' +
                              '<span>{2}: {3}</span> -- ' +
                              '<span class="match-name" title="{4}">{5}</span>' +
                          '</span>';
        var progressBarFormat = '<div class="percentage-bar" data-value="{0}"><div class="percentage-label"><span>{1}%<span></div></div>';
        var percentageCol = '';
        var groupRow = '';
        var leagueName = '';
        var matchName = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left', ShowCollapseExpand: true },
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
            //gridExpandHeight: 520,
            fullHeight: false,
            gridHeight: 350,
            showDistinctRow: false,
            showTree: true,
            showTitle: true,
            titleInfo: {
                text: isLive ? labels.LiveEvents : labels.NonLiveEvents
            },
            showPager: true,
            customPager: true,
            pagerOption: {
                currentPage: cachedData.PageIndex,
                itemsPerPage: cachedData.PageSize,
                itemsPerPageArray: CMS.pagerRanges,
                totalItem: totalItems,
                pagerSelectCallBack: function (pageSize, pageIndex) {
                    reloadPage(isLive, pageSize, pageIndex, container);
                }
            }
        };
        var bodyData = [];
        var row;
        var subRows;

        if (data && data.length > 0) {
            $.each(data, function (groupIndex, group) {
                leagueName = group.LeagueName.length > 65 ? group.LeagueName.substring(0, 62) + '...' : group.LeagueName;
                matchName = group.MatchName.length > 65 ? group.MatchName.substring(0, 62) + '...' : group.MatchName;
                groupRow = groupFormat.format(group.LeagueName, leagueName, labels.MatchCode, group.MatchCode, group.MatchName, matchName);
                subRows = [];

                $.each(group.SMAItems, function (index, item) {
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
                    subRows.push(row);
                });

                row = {
                    Columns: [{ Name: 'ColSMAName', Value: groupRow, CellAlign: 'left', MergeColNumber: 8}],
                    SubRows: subRows
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        container.fanexGrid(gridOptions);
        handleGridEvents(container);
    };

    var handleGridEvents = function (container) {
        container.find('div.percentage-bar').each(function () {
            var $obj = $(this);
            var percentage = parseInt($obj.attr('data-value'));

            $obj.progressbar({
                value: percentage
            });

            if (percentage >= 100) {
                $obj.addClass('full-percents');
            }
        });
        container.fanexGrid('resizeGrid');
    };

    // Customize reloading page.
    var reloadPage = function (isLive, pageIndex, pageSize, container) {
        var pageData = [];
        var startIndex = 0;
        var endIndex = 0;
        var counter = 0;
        var data = isLive ? cachedData.LiveData : cachedData.NonLiveData;
        var totalItems = isLive ? cachedData.NoneLive : cachedData.NoneLiveLength;

        if (data && data.length > 0) {
            pageIndex = parseInt(pageIndex);
            pageSize = parseInt(pageSize);
            startIndex = pageIndex * pageSize;
            endIndex = startIndex + pageSize - 1;

            $.each(data, function (groupIndex, group) {
                var temCounter = counter + group.SMAItems.length - 1;
                var currentGroup = null;

                if (temCounter < startIndex || counter > endIndex) {
                    counter = temCounter + 1;
                } else {
                    currentGroup = {
                        LeagueName: group.LeagueName,
                        MatchCode: group.MatchCode,
                        MatchName: group.MatchName,
                        SMAItems: []
                    };

                    $.each(group.SMAItems, function (index, item) {
                        if (counter >= startIndex && counter <= endIndex) {
                            currentGroup.SMAItems.push(item);
                        }

                        counter++;
                    });

                    pageData.push(currentGroup);
                }
            });
        }

        cachedData.PageIndex = pageIndex;
        cachedData.PageSize = pageSize;
        renderGrid(isLive, pageData, totalItems, container);
    };

    // Count number of sub-items of cached data.
    var countSubItems = function (data) {
        var counter = 0;

        if (data && data.length > 0) {
            $.each(data, function (groupIndex, group) {
                if (group.SMAItems) {
                    counter += group.SMAItems.length;
                }
            });
        }

        return counter;
    };

    // Get list of leagues of a specific sport.
    var getLeaguesOfSport = function (sportID) {
        var parameters = {
            SportID: sportID
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetLeaguesOfSport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
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

    // Initialize filter form data.
    var intializeFilterData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                if (result.Sports) {
                    updateSportDropdown(result.Sports);
                }

                if (result.Leagues) {
                    updateLeagueDropdown(result.Leagues);
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

    // Update Sport drop-down list.
    var updateSportDropdown = function (data, sportID) {
        sportID = parseInt(sportID);

        // Clear old values.
        domElements.ddlSport.html('');

        // Remove old error.
        Validator.clearError(domElements.txtSport.parent());

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlSport.append($('<option></option>').attr('value', item.SportType).text(item.SportName));

                // Select default league.
                if ((sportID && sportID === item.SportType) || (index === 0)) {
                    domElements.ddlSport.val(item.SportType);
                    domElements.txtSport.val(item.SportName);
                }
            });
        }
    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data, leagueID) {
        leagueID = parseInt(leagueID);

        // Clear old values.
        domElements.ddlLeague.html('');

        // Remove old error.
        Validator.clearError(domElements.txtLeague.parent());

        // Add a default value.
        domElements.ddlLeague.append($('<option>').attr('value', '0').text(labels.AllLeagues));
        domElements.txtLeague.val(labels.AllLeagues);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlLeague.append($('<option>').attr('value', item.LeagueID).text(item.LeagueName));

                // Select default league.
                if (leagueID && leagueID === item.LeagueID) {
                    domElements.ddlLeague.val(item.LeagueID);
                    domElements.txtLeague.val(item.LeagueName);
                }
            });
        }
    };

    // Get SMA turnover report.
    var getTurnoverReport = function () {
        CMS.disableInput();

        if (domElements.divRefresh && domElements.divRefresh.stop) {
            domElements.divRefresh.stop();
        }

        $.ajax({
            type: 'POST',
            url: urls.GetTurnoverReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (domElements.divRefresh && domElements.divRefresh.run) {
                    domElements.divRefresh.run();
                }

                cachedData.LiveData = result.Report.Live;
                cachedData.NonLiveData = result.Report.NonLive;
                cachedData.LiveLength = countSubItems(cachedData.LiveData);
                cachedData.NoneLiveLength = countSubItems(cachedData.NonLiveData);
                reloadPage(true, cachedData.PageIndex, cachedData.PageSize, domElements.divLiveTurnoverMonitor);
                reloadPage(false, cachedData.PageIndex, cachedData.PageSize, domElements.divNoneLiveTurnoverMonitor);
                parameters.SportType = result.SportType;
                parameters.LeagueID = result.LeagueID;

                if (result.Sports) {
                    updateSportDropdown(result.Sports, parameters.SportType);
                }

                if (result.Leagues) {
                    updateLeagueDropdown(result.Leagues, parameters.LeagueID);
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

    $this.InitPage = function () {
        cacheDOM();
        intializeFilterData();
        initializeFields();
    };
};