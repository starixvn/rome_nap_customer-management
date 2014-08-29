/* File Created: April 16, 2014 */
/* global Namespace, Fanex */
/* jshint strict: false */
// 'use strict';

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.PageManager');
Fanex.NAP.CMS.ConfigureTOByEvent.PageManager = function () {
    var $this = this;
    var domElements = {
        lnkSport: null,
        lnkLeague: null,
        lnkMatch: null,
        divSportContent: null,
        divLeagueContent: null,
        divMatchContent: null
    };

    // Cache DOM elements
    var cacheDOMElements = function () {
        domElements.lnkSport = $('#lnkSport');
        domElements.lnkLeague = $('#lnkLeague');
        domElements.lnkMatch = $('#lnkMatch');
        domElements.divSportContent = $('#divSportContent');
        domElements.divLeagueContent = $('#divLeagueContent');
        domElements.divMatchContent = $('#divMatchContent');
    };

    // Initialize fields and handle events.
    var initFields = function () {
        domElements.lnkSport.on('click', function () {
            if (!domElements.lnkSport.hasClass('disabled')) {
                domElements.lnkLeague.removeClass('disabled');
                domElements.lnkMatch.removeClass('disabled');
                domElements.lnkSport.addClass('disabled');
                domElements.divSportContent.removeClass('display-none');
                domElements.divLeagueContent.addClass('display-none');
                domElements.divMatchContent.addClass('display-none');

                $this.CurrentPage = new Fanex.NAP.CMS.ConfigureTOByEvent.SportPage();
                $this.CurrentPage.InitPage(domElements.divSportContent);
            }
        });

        domElements.lnkLeague.on('click', function () {
            if (!domElements.lnkLeague.hasClass('disabled')) {
                domElements.lnkSport.removeClass('disabled');
                domElements.lnkMatch.removeClass('disabled');
                domElements.lnkLeague.addClass('disabled');
                domElements.divSportContent.addClass('display-none');
                domElements.divLeagueContent.removeClass('display-none');
                domElements.divMatchContent.addClass('display-none');

                $this.CurrentPage = new Fanex.NAP.CMS.ConfigureTOByEvent.LeaguePage();
                $this.CurrentPage.InitPage(domElements.divLeagueContent);
            }
        });

        domElements.lnkMatch.on('click', function () {
            if (!domElements.lnkMatch.hasClass('disabled')) {
                domElements.lnkSport.removeClass('disabled');
                domElements.lnkLeague.removeClass('disabled');
                domElements.lnkMatch.addClass('disabled');
                domElements.divSportContent.addClass('display-none');
                domElements.divLeagueContent.addClass('display-none');
                domElements.divMatchContent.removeClass('display-none');

                $this.CurrentPage = new Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage();
                $this.CurrentPage.InitPage(domElements.divMatchContent);
            }
        });
    };

    $this.CurrentPage = null;

    $this.InitPage = function () {
        cacheDOMElements();
        initFields();
    };
};

$(document).ready(function () {
    var tunoverPageManager = new Fanex.NAP.CMS.ConfigureTOByEvent.PageManager();

    tunoverPageManager.InitPage();
});