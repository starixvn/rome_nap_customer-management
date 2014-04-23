/* File Created: April 17, 2014 */
/* global Namespace, Fanex, CMS, labels */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.LeaguePage');
Fanex.NAP.CMS.ConfigureTOByEvent.LeaguePage = function () {
    'use strict';

    var $this = this;
    var leagueReport = null;
    var matchReport = null;
    var domElements = {
        divLeagueContent: null,
        ddlSport: null,
        ddlLeague: null,
        btnSearch: null,
        divLeagueTurnoverContainer: null,
        divLeagueTurnover: null,
        divMatchTurnoverContainer: null,
        divMatchTurnover: null
    };
    var urls = {
        GetInitializedSportData: CMS.resolveRootUrl('ConfigureTOByEvent/GetInitializedSportData'),
        GetLeaguesOfSport: CMS.resolveRootUrl('ConfigureTOByEvent/GetLeaguesOfSport')
    };

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        // Use class selector for Sport and League to overcome the duplication issue caused by jquery.combobox.
        domElements.divLeagueContent = domElements.divLeagueContent || $('#divLeagueContent');
        domElements.ddlSport = domElements.divLeagueContent.find('select.sport');
        domElements.ddlLeague = domElements.divLeagueContent.find('select.league');
        domElements.btnSearch = domElements.divLeagueContent.find('#btnSearch');
        domElements.divLeagueTurnoverContainer = domElements.divLeagueContent.find('#divLeagueTurnoverContainer');
        domElements.divLeagueTurnover = domElements.divLeagueContent.find('#divLeagueTurnover');
        domElements.divMatchTurnoverContainer = domElements.divLeagueContent.find('#divMatchTurnoverContainer');
        domElements.divMatchTurnover = domElements.divLeagueContent.find('#divMatchTurnover');
    };

    // Bind event handlers.
    var bindEventHandlers = function () {
        domElements.ddlSport.on('change', function () {
            var sportID = $(this).val();

            getLeaguesOfSport(sportID);
        });

        domElements.btnSearch.on('click', function () {
            var sportID = domElements.ddlSport.val();
            var leagueID = domElements.ddlLeague.val();

            hideGrids();

            if (leagueID === 'null') {
                leagueReport = leagueReport || new Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover();
                leagueReport.Initialize(domElements, {
                    PreviousReportObjects: [],
                    SportID: sportID,
                    AllowShowInstruction: true
                });
                leagueReport.GetReportData();
            } else {
                leagueReport = leagueReport || new Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover();
                leagueReport.Initialize(domElements, {
                    PreviousReportObjects: [],
                    SportID: sportID,
                    AllowShowInstruction: true
                });
                matchReport = matchReport || new Fanex.NAP.CMS.ConfigureTOByEvent.MatchTurnover();
                matchReport.Initialize(domElements, {
                    PreviousReportObjects: [leagueReport],
                    SportID: sportID,
                    LeagueID: leagueID,
                    AllowShowInstruction: true
                });
                matchReport.GetReportData();
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
                    domElements.ddlSport.next().children('input').val(item.SportName);
                }
            });
        }
    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data) {
        // Clear old values.
        domElements.ddlLeague.html('');

        // Add default item.
        domElements.ddlLeague.append($('<option>').attr('value', 'null').text(labels.AllLeagues));
        domElements.ddlLeague.val(null).next().children('input').val(labels.AllLeagues);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlLeague.append($('<option>').attr('value', item.LeagueID).text(item.LeagueName));
            });
        }
    };

    // Hide grids.
    var hideGrids = function () {
        domElements.divLeagueTurnoverContainer.addClass('display-none');
        domElements.divMatchTurnoverContainer.addClass('display-none');
    };

    // Initialize filter form data.
    var intializeFilterData = function () {
        var parameters = {
            Mode: 1
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedSportData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
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

    // Get list of leagues of a specific sport.
    var getLeaguesOfSport = function (sportID) {
        var parameters = {
            SportID: sportID,
            Mode: 1
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

    $this.InitPage = function (divContent) {
        domElements.divLeagueContent = divContent;
        cacheDOMElements();
        hideGrids();
        intializeFilterData();
        bindEventHandlers();
    };
};