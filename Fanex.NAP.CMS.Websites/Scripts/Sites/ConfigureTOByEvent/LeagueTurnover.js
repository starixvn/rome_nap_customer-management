/* File Created: April 17, 2014 */
/* global Namespace, Fanex, CMS, labels, messages, hasEditRight */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover');
Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetLeagueTurnover'),
        UpdateLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateLeagueTurnover')
    };
    var initialParameters = {};
    var instructionOptions = {
        AllowShow: true,
        IsShown: false,
        IsFocused: false
    };
    var matchReport = null;
    var domElements = {};

    // Initialize some common behaviors.
    var handleEvents = function () {
        $('body').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('percentage')) {
                var $input = domElements.divLeagueTurnover.find('input:focus');
                var $link = $input.prev();

                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
                instructionOptions.IsShown = false;
            }
        });

        instructionOptions.AllowShow = initialParameters.AllowShowInstruction;
    };

    // Render grid.
    var renderGrid = function (data) {
        var sportFormat = '<span class="link league-link" data-league-id="{0}" data-league-value="{1}">{2}</span>';
        var percentageFormat = '<span class="percentage-link link" data-value="{0}">{1}</span><input type="text" class="percentage display-none" />';
        var exceptionFormat = '<span class="ico-delete" data-match-code="{0}" title="{1}"></span>';
        var showExceptionChk = '<input type="checkbox" id="chkShowException" /><label for="chkShowException">{0}</label>';
        var percentage = '';
        var gridOptions = {
            columns: [{ HeaderText: labels.League, Width: 600, Name: 'ColLeague', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 115,
            showTitle: true,
            titleInfo: {
                text: generateBreadCrumb(),
                customButtons: showExceptionChk.format(labels.OnlyShowExceptionSetting)
            }
        };
        var bodyData = [];
        var row;

        if (hasEditRight) {
            gridOptions.columns.push({ HeaderText: labels.Exception, Width: 100, Name: 'ColException', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                percentage = item.Percentage === 0 ? labels.Unlimited : item.Percentage;
                row = {
                    Columns: [{ Name: 'ColLeague', Value: sportFormat.format(item.LeagueID, item.Percentage, item.LeagueName) },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageFormat.format(item.Percentage, percentage) : percentage}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColException', Value: exceptionFormat.format(item.LeagueID, labels.RemoveException) });
                }

                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divLeagueTurnover.fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var percentageLinks = domElements.divLeagueTurnover.find('span.percentage-link');
        var percentageInputs = domElements.divLeagueTurnover.find('input.percentage');

        percentageLinks.on('click', function () {
            var $link = $(this);
            var $input = $link.next();
            var percentage = $link.attr('data-value');

            $.each(percentageLinks, function (index, link) {
                $(link).removeClass('display-none').next().addClass('display-none').tipsy('hide');
                instructionOptions.IsShown = false;
            });

            percentage = percentage === '0' ? 'U' : percentage;
            $link.addClass('display-none');
            $input.val(percentage).removeClass('display-none').focus().select();

            if (instructionOptions.AllowShow && !instructionOptions.IsShown) {
                var instruction = '<div id="divInstruction">' +
                                      '{0}<br/>' +
                                      '<input type="checkbox" id="chkInstruction" class="instruction"/>' +
                                      '<label for="chkInstruction">{1}</label>' +
                                  '</div>';

                instruction = instruction.format(messages.TurnoverInputInstruction, messages.NotShowAgain);

                $input.attr('title', instruction).tipsy({
                    trigger: 'manual',
                    html: true,
                    opacity: 0.97,
                    gravity: 's'
                }).tipsy('show');
                instructionOptions.IsShown = true;

                $('#chkInstruction').on('change', function () {
                    if ($(this).is(':checked')) {
                        instructionOptions.AllowShow = false;
                        instructionOptions.IsShown = false;
                        $input.tipsy('hide').focus();
                    }
                });

                setTimeout(function () {
                    $input.tipsy('hide');
                    instructionOptions.IsShown = false;
                }, 5000);
            }
        });

        percentageInputs.mask('Z00,000', { translation: { 'Z': { pattern: /[uU0-9]/, reverse: true, optional: true}} });
        percentageInputs.on('keyup', function (event) {
            var $input = $(this);
            var $link = $input.prev();
            var percentage = $input.val();

            if (event.keyCode === 13) {
                percentage = percentage.toLowerCase() === 'u' ? '0' : percentage;
                $link.attr('data-value', percentage);
                percentage = percentage === '0' ? labels.Unlimited : percentage;
                $link.text(percentage);
                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
                instructionOptions.IsShown = false;
                // @@TODO: Update data.
            } else if (event.keyCode === 27) {
                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
                instructionOptions.IsShown = false;
            } else if (percentage.length > 0 && percentage[0].toLowerCase() === 'u') {
                percentage = percentage[0];
                $input.val(percentage);
            }
        });

        domElements.divLeagueTurnover.find('span.league-link').on('click', function () {
            var $obj = $(this);
            var leagueID = $obj.attr('data-league-id');
            var leagueValue = $obj.attr('data-league-value');
            var leagueName = $obj.text();

            leagueValue = leagueValue === '0' ? labels.Unlimited : leagueValue;
            matchReport = matchReport || new Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover();
            matchReport.Initialize(domElements, {
                PreviousReportObjects: initialParameters.PreviousReportObjects.push($this),
                LastItem: '{0} ({1})'.format(leagueName, leagueValue),
                SportID: initialParameters.SportID,
                LeagueID: leagueID,
                AllowShowInstruction: instructionOptions.AllowShow
            });
            domElements.divLeagueTurnoverContainer.addClass('display-none');
        });

        domElements.divLeagueTurnoverContainer.find('span.breadCrumb').on('click', function () {
            var index = $(this).attr('data-index');

            domElements.divLeagueTurnoverContainer.addClass('display-none');
            initialParameters.PreviousReportObjects[index].ShowReport();
        });
    };

    // Generate breadcrumb.
    var generateBreadCrumb = function () {
        var breadCumbTemplate = '<span class="breadCrumb" data-index="{0}">{1}</span> >> ';
        var breadCrumb = '<div id="divBreadCrumb">';

        if (initialParameters) {
            $.each(initialParameters.PreviousReportObjects, function (index, prevReport) {
                breadCrumb += breadCumbTemplate.format(index, prevReport.ReportName);
            });

            breadCrumb += initialParameters.LastItem;
        }

        return breadCrumb + '</div>';
    };

    // Report name.
    $this.ReportName = '';

    // Show hidden report.
    $this.ShowReport = function () {
        domElements.divLeagueTurnoverContainer.removeClass('display-none');
    };

    // Get report data.
    $this.GetReportData = function () {
        var parameters = {
            SportID: initialParameters.SportID
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetLeagueTurnover,
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
    $this.Initialize = function (cachedDomElements, initialParamters) {
        domElements = cachedDomElements;
        initialParameters = initialParamters;
        handleEvents();
    };
};