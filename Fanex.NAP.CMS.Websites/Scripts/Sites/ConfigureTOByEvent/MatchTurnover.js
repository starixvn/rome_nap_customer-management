/* File Created: April 17, 2014 */
/* global Namespace, Fanex, CMS, labels, hasEditRight */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.MatchTurnover');
Fanex.NAP.CMS.ConfigureTOByEvent.MatchTurnover = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetMatchTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetMatchTurnover'),
        UpdateMatchTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateTurnover')
    };
    var domElements = {
        divMatchContainer: null,
        divMatch: null
    };
    var initialParameters = {
        PreviousReportObjects: [],
        SportID: null,
        LeagueID: null,
        MatchCode: null
    };

    // Render grid.
    var renderGrid = function (data) {
        var percentageFormat = '<span class="percentage-link link" data-value="{0}">{0}</span><input type="text" class="percentage display-none" />';
        var exceptionFormat = '<span class="delete-ico" data-match-code="{0}" title="{1}"></span>';

        var gridOptions = {
            columns: [{ HeaderText: labels.Code, Width: 100, Name: 'ColCode', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Event, Width: 270, Name: 'ColEvent', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.EventDate, Width: 100, Name: 'ColEventDate', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Percentage, Width: 100, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 200
        };
        var bodyData = [];
        var row;

        if (hasEditRight) {
            gridOptions.columns.push({ HeaderText: labels.Exception, Width: 100, Name: 'ColException', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                row = {
                    Columns: [{ Name: 'ColCode', Value: item.Code },
                              { Name: 'ColEvent', Value: item.Event },
                              { Name: 'ColEventDate', Value: item.EventDate },
                              { Name: 'ColPercentage', Value: percentageFormat.format(item.Percentage)}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColException', Value: exceptionFormat.format(item.MatchCode, labels.RemoveException) });
                }

                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divMatch.fanexGrid(gridOptions);
        generateBreadCrumb();
        handleGridEvents();
    };

    // Generate breadcrumb.
    var generateBreadCrumb = function () {
        var breadCumbTemplate = '<span class="breadCrumb {0}" data-index="{1}">{2} {3}</span>';
        var narrow = '';
        var link = '';
        var breadCrumb = '<div id="divBreadCrumb">';

        if (breadCrumb) {
            $.each(initialParameters.PreviousReportObjects, function (index, prevReport) {
                if (index < initialParameters.PreviousReportObjects.length - 1) {
                    narrow = '>>';
                    link = 'link';
                } else {
                    narrow = '';
                    link = '';
                }

                breadCrumb += breadCumbTemplate.format(link, index, prevReport.ReportName, narrow);
            });
        }

        return breadCrumb + '</div>';
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        domElements.divMatch.find('span.breadCrumb').on('click', function () {
            var index = parseInt($(this).attr('data-index'));

            domElements.divMatchContainer.addClass('display-none');
            initialParameters.PreviousReportObjects[index].ShowReport();
        });
    };

    // Report name.
    $this.ReportName = '';
    $this.ReportValue = '';

    // Get report data.
    $this.GetReportData = function () {
        var parameters = {
            SportID: initialParameters.SportID,
            LeagueID: initialParameters.LeagueID,
            MatchCode: initialParameters.MatchCode
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetMatchTurnover,
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

    // Initialize report.
    $this.Initialize = function (divContainer, initialValues) {
        domElements.divMatchContainer = divContainer;
        initialParameters = initialValues;
    };
};