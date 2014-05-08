/* File Created: April 16, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels */

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
        ddlSport: null,
        txtSport: null,
        ddlLeague: null,
        txtLeague: null,
        txtPercentageUsed: null,
        btnSearch: null,
        divTurnoverMonitor: null,
        divTurnoverMonitorContainer: null
    };
    var parameters = {
        SportType: null,
        LeagueID: null,
        UsedPercent: null
    };
    var cachedData = {
        Data: null,
        Length: 0,
        PageSize: CMS.pageSize,
        PageIndex: 0,
        RefreshTime: null
    };
    var refreshTimer = {
        Interval: [10, 20, 30],
        Default: 10
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.ddlSport = $('#ddlSport');
        domElements.txtSport = domElements.ddlSport.next().children('input');
        domElements.ddlLeague = $('#ddlLeague');
        domElements.txtLeague = domElements.ddlLeague.next().children('input');
        domElements.txtPercentageUsed = $('#txtPercentageUsed');
        domElements.btnSearch = $('#btnSearch');
        domElements.divTurnoverMonitor = $('#divTurnoverMonitor');
        domElements.divTurnoverMonitorContainer = $('#divTurnoverMonitorContainer');
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        domElements.txtPercentageUsed.numberField(0, false, null);

        domElements.ddlSport.on('change', function () {
            var sportID = domElements.ddlSport.val();

            getLeaguesOfSport(sportID);
        });

        domElements.txtPercentageUsed.on('keyup', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.btnSearch.on('click', function () {
            cachedData.PageIndex = 0;
            cachedData.PageSize = CMS.pageSize;
            cachedData.RefreshTime = refreshTimer.Default;

            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (Validator.countErrors(domElements.divSMAByEventForm) === 0) {
                    parameters.SportType = domElements.ddlSport.val();
                    parameters.LeagueID = domElements.ddlLeague.val();
                    parameters.UsedPercent = domElements.txtPercentageUsed.val().replace(/,/g, '');

                    getTurnoverReport();
                }
            }, 70);
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var groupFormat = '<span class="group-title">' +
                              '<span class="league" title="{0}">{1}</span> -- ' +
                              '<span>{2}: {3}</span> -- ' +
                              '<span class="match-name" title="{4}">{5}</span>' +
                          '</span>';
        var progressBarFormat = '<div class="percentage-used" data-value="{0}"><div class="percentage-label"><span>{1}%<span></div></div>';
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
            gridExpandHeight: 200,
            showDistinctRow: false,
            showTitle: true,
            titleInfo: {
                showButtonsWhenNoData: true,
                customButtons: '<div id="divAutoRefresh"></div>'
            },
            showTree: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                currentPage: cachedData.PageIndex,
                itemsPerPage: cachedData.PageSize,
                itemsPerPageArray: CMS.pagerRanges,
                totalItem: cachedData.Length,
                pagerSelectCallBack: reloadPage
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
        domElements.divTurnoverMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divTurnoverMonitor.find('div.percentage-used').each(function () {
            var $obj = $(this);
            var percentage = parseInt($obj.attr('data-value'));

            $obj.progressbar({
                value: percentage
            });

            if (percentage >= 100) {
                $obj.addClass('full-percents');
            }
        });

        domElements.divTurnoverMonitorContainer.find('#divAutoRefresh')
            .autoRefresh(refreshTimer.Interval, cachedData.RefreshTime, getTurnoverReport, false)
            .find('li').on('click', function () {
                var selectedValue = parseInt($(this).attr('data-value'));

                cachedData.RefreshTime = parseInt(selectedValue);
            });
    };

    // Customize reloading page.
    var reloadPage = function (pageIndex, pageSize) {
        var pageData = [];
        var startIndex = 0;
        var endIndex = 0;
        var counter = 0;

        if (cachedData.Data && cachedData.Data.length > 0) {
            pageIndex = parseInt(pageIndex);
            pageSize = parseInt(pageSize);
            startIndex = pageIndex * pageSize;
            endIndex = startIndex + pageSize - 1;

            $.each(cachedData.Data, function (groupIndex, group) {
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
        renderGrid(pageData);
    };

    // Count number of sub-items of cached data.
    var countSubItems = function () {
        var counter = 0;

        if (cachedData.Data && cachedData.Data.length > 0) {
            $.each(cachedData.Data, function (groupIndex, group) {
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
    var updateSportDropdown = function (data) {
        // Clear old values.
        domElements.ddlSport.html('');

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlSport.append($('<option></option>').attr('value', item.SportType).text(item.SportName));

                // Select first sport by default.
                if (index === 0) {
                    domElements.ddlSport.val(item.SportType);
                    domElements.txtSport.val(item.SportName);
                }
            });
        }
    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data) {
        // Clear old values.
        domElements.ddlLeague.html('');

        // Add a default value.
        domElements.ddlLeague.append($('<option>').attr('value', '').text(labels.AllLeagues));
        domElements.txtLeague.val(labels.AllLeagues);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlLeague.append($('<option>').attr('value', item.LeagueID).text(item.LeagueName));
            });
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
            data: JSON.stringify(parameters),
            success: function (result) {
                cachedData.Data = result;
                cachedData.Length = countSubItems();
                reloadPage(cachedData.PageIndex, cachedData.PageSize);
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