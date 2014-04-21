/* File Created: April 17, 2014 */
/* global Namespace, Fanex */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.SportPage');
Fanex.NAP.CMS.ConfigureTOByEvent.SportPage = function () {
    'use strict';

    var $this = this;
    var domElements = {
        divSportContent: null,
        divSportTurnoverContainer: null,
        divSportTurnover: null,
        divLeagueTurnoverContainer: null,
        divLeagueTurnover: null,
        divMatchTurnoverContainer: null,
        divMatchTurnover: null
    };

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        domElements.divSportContent = domElements.divSportContent || $('#divSportContent');
        domElements.divSportTurnoverContainer = domElements.divSportContent.find('#divSportTurnoverContainer');
        domElements.divSportTurnover = domElements.divSportContent.find('#divSportTurnover');
        domElements.divLeagueTurnoverContainer = domElements.divSportContent.find('#divLeagueTurnoverContainer');
        domElements.divLeagueTurnover = domElements.divSportContent.find('#divLeagueTurnover');
        domElements.divMatchTurnoverContainer = domElements.divSportContent.find('#divMatchTurnoverContainer');
        domElements.divMatchTurnover = domElements.divSportContent.find('#divMatchTurnover');
    };

    // Hide grids.
    var hideGrids = function () {
        domElements.divSportTurnoverContainer.addClass('display-none');
        domElements.divLeagueTurnoverContainer.addClass('display-none');
        domElements.divMatchTurnoverContainer.addClass('display-none');
    };

    $this.CurrentReport = null;

    $this.InitPage = function (divContent) {
        domElements.divSportContent = divContent;
        cacheDOMElements();
        hideGrids();
        $this.CurrentReport = $this.CurrentReport || new Fanex.NAP.CMS.ConfigureTOByEvent.SportTurnover();
        $this.CurrentReport.Initialize(domElements);
        $this.CurrentReport.GetReportData();
    };
};