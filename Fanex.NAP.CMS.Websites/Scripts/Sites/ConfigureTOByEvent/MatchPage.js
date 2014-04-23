/* File Created: April 17, 2014 */
/* global Namespace, Fanex */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage');
Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage = function () {
    'use strict';

    var $this = this;
    var domElements = {
        divMatchContent: null,
        ddlSport: null,
        ddlLeague: null,
        btnSearch: null,
        divMatchTurnoverContainer: null,
        divMatchTurnover: null
    };

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        domElements.divMatchContent = domElements.divMatchContent || $('#divMatchContent');
        domElements.ddlSport = domElements.divMatchContent.find('#ddlSport');
        domElements.ddlLeague = domElements.divMatchContent.find('#ddlLeague');
        domElements.btnSearch = domElements.divMatchContent.find('#btnSearch');
        domElements.divMatchTurnoverContainer = domElements.divMatchContent.find('#divMatchTurnoverContainer');
        domElements.divMatchTurnover = domElements.divMatchContent.find('#divMatchTurnover');
    };

    // Bind event handlers.
    var bindEventHandlers = function () {

    };

    // Initialize filter form data.
    var intializeFilterData = function () {
    };

    // Update Sport drop-down list.
    var updateSportDropdown = function (data) {

    };

    // Update League drop-down list.
    var updateLeagueDropdown = function (data) {
    };

    $this.InitPage = function (divContent) {
        domElements.divMatchContent = divContent;
        cacheDOMElements();
        domElements.divMatchTurnoverContainer.addClass('display-none');
        //        $this.CurrentReport = $this.CurrentReport || new Fanex.NAP.CMS.ConfigureTOByEvent.SportTurnover();
        //        $this.CurrentReport.Initialize(domElements);
        //        $this.CurrentReport.GetReportData();
    };
};