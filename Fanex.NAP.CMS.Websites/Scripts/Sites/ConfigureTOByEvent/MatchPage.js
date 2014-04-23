/* File Created: April 17, 2014 */
/* global Namespace, Fanex, CMS */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage');
Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage = function () {
    'use strict';

    var $this = this;
    var matchReport = null;
    var domElements = {
        divMatchContent: null,
        ddlSport: null,
        ddlLeague: null,
        txtMatchCode: null,
        btnSearch: null,
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
        domElements.divMatchContent = domElements.divMatchContent || $('#divMatchContent');
        domElements.ddlSport = domElements.divMatchContent.find('select.sport');
        domElements.ddlLeague = domElements.divMatchContent.find('select.league');
        domElements.txtMatchCode = domElements.divMatchContent.find('#txtMatchCode');
        domElements.btnSearch = domElements.divMatchContent.find('#btnSearch');
        domElements.divMatchTurnoverContainer = domElements.divMatchContent.find('#divMatchTurnoverContainer');
        domElements.divMatchTurnover = domElements.divMatchContent.find('#divMatchTurnover');
    };

    // Bind event handlers.
    var bindEventHandlers = function () {
        domElements.ddlSport.on('change', function () {
            var sportID = $(this).val();

            getLeaguesOfSport(sportID);
        });

        domElements.txtMatchCode.on('keydown', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.btnSearch.on('click', function () {
            var sportID = domElements.ddlSport.val();
            var leagueID = domElements.ddlLeague.val();
            var matchCode = domElements.txtMatchCode.val();

            hideGrids();

            matchReport = matchReport || new Fanex.NAP.CMS.ConfigureTOByEvent.MatchTurnover();
            matchReport.Initialize(domElements, {
                PreviousReportObjects: [{}],
                SportID: sportID,
                LeagueID: leagueID,
                MatchCode: matchCode,
                AllowShowInstruction: true
            });
            matchReport.GetReportData();
        });
    };

    // Initialize filter form data.
    var intializeFilterData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedSportData,
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
                    domElements.ddlSport.next().children('input').val(item.SportName);
                }
            });
        }
    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data) {
        // Clear old values.
        domElements.ddlLeague.html('');

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlLeague.append($('<option>').attr('value', item.LeagueID).text(item.LeagueName));

                // Select first sport by default.
                if (index === 0) {
                    domElements.ddlLeague.val(item.LeagueID);
                    domElements.ddlLeague.next().children('input').val(item.LeagueName);
                }
            });
        }
    };

    // Hide grids.
    var hideGrids = function () {
        domElements.divMatchTurnoverContainer.addClass('display-none');
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

    $this.InitPage = function (divContent) {
        domElements.divMatchContent = divContent;
        cacheDOMElements();
        bindEventHandlers();
        hideGrids();
        intializeFilterData();
    };
};