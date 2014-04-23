/* File Created: April 17, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover');
Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetLeagueTurnover'),
        UpdateLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateLeagueTurnover'),
        RemoveLeagueException: CMS.resolveRootUrl('ConfigureTOByEvent/RemoveLeagueException')
    };
    var initialParameters = {};
    var instructionOptions = {
        AllowShow: true,
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

                if ($input.hasClass('percentage')) {
                    $input.addClass('display-none');
                    $link.removeClass('display-none');
                    $input.tipsy('hide');
                }
            }
        });

        instructionOptions.AllowShow = initialParameters.AllowShowInstruction;
    };

    // Render grid.
    var renderGrid = function (data) {
        var leagueFormat = '<span class="link league-link" data-league-id="{0}">{1}</span>';
        var percentageFormat = '<span class="percentage-link link" data-league-id="{0}" data-value="{1}">{2}</span>' +
                               '<input type="text" maxlength="7" class="percentage display-none" />';
        var exceptionFormat = '<span class="remove-exception ico-delete" data-league-id="{0}" title="{1}"></span>';
        var showExceptionChk = '<input type="checkbox" id="chkShowException" /><label for="chkShowException">{0}</label>';
        var percentage = '';
        var leagueCol = '';
        var percentageCol = '';
        var exceptionCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.League, Width: 600, Name: 'ColLeague', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 200,
            showTitle: true,
            titleInfo: {
                showButtonsWhenNoData: true,
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
                percentage = numeral(item.Percentage).format(CMS.formats.IntegerFormat);
                leagueCol = leagueFormat.format(item.LeagueID, item.LeagueName);
                percentageCol = percentageFormat.format(item.LeagueID, percentage, item.Percentage === 0 ? labels.Unlimited : percentage);
                exceptionCol = item.IsException ? exceptionFormat.format(item.LeagueID, labels.RemoveException) : '';

                row = {
                    Columns: [{ Name: 'ColLeague', Value: leagueCol },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageCol : percentage}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColException', Value: exceptionCol });
                }

                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divLeagueTurnover.fanexGrid(gridOptions);
        domElements.divLeagueTurnoverContainer.attr('data-current-sport', initialParameters.SportID);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var percentageLinks = domElements.divLeagueTurnover.find('span.percentage-link');
        var percentageInputs = domElements.divLeagueTurnover.find('input.percentage');

        // Format the parcentage value which is input into the text box.
        percentageInputs.numberField(0, false, 'u');

        // When user clicks on Percentage link.
        percentageLinks.on('click', function () {
            var $link = $(this);
            var $input = $link.next();
            var percentage = $link.attr('data-value');

            // Disable other active fields.
            $.each(percentageLinks, function (index, link) {
                $(link).removeClass('display-none').next().addClass('display-none').tipsy('hide');
            });

            // Fill current value into text box.
            percentage = percentage === '0' ? 'U' : percentage;
            $link.addClass('display-none');
            $input.val(percentage).removeClass('display-none').clearfield().focus().select();

            // Show instruction if it is allowed.
            if (instructionOptions.AllowShow) {
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

                $('#chkInstruction').on('change', function () {
                    if ($(this).is(':checked')) {
                        instructionOptions.AllowShow = false;
                        $input.tipsy('hide').focus();
                    }
                });

                // Auto-hide after 5 seconds.
                instructionOptions.TimeoutID = setTimeout(function () {
                    $input.tipsy('hide');
                }, 5000);
            }
        });

        // Handle key up event.
        percentageInputs.on('keyup', function (event) {
            var $input = $(this);
            var $link = $input.prev();
            var percentage = $input.val();
            var formatedPercentage = percentage.toLowerCase() === 'u' ? labels.Unlimited : percentage;

            if (event.keyCode === 13 && validatePercentage($input)) {
                CMS.showConfirmMessage(messages.UpdatePercentageConfirmMessage.format('<b>' + formatedPercentage + '</b>'), function () {
                    percentage = percentage.toLowerCase() === 'u' ? '0' : percentage;

                    if (updatePercentage($link.attr('data-league-id'), percentage.replace(/,/g, ''))) {
                        $link.attr('data-value', percentage);
                        $link.text(formatedPercentage);
                        $input.addClass('display-none');
                        $link.removeClass('display-none');
                        $input.tipsy('hide');
                    }
                });
            } else if (event.keyCode === 27) {
                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
            }
        });

        domElements.divLeagueTurnover.find('span.league-link').on('click', function () {
            var $obj = $(this);
            var leagueID = $obj.attr('data-league-id');
            var previousReportObjects = initialParameters.PreviousReportObjects.slice(0);

            previousReportObjects.push($this);
            matchReport = matchReport || new Fanex.NAP.CMS.ConfigureTOByEvent.MatchTurnover();
            matchReport.Initialize(domElements, {
                PreviousReportObjects: previousReportObjects,
                SportID: initialParameters.SportID,
                LeagueID: leagueID,
                AllowShowInstruction: instructionOptions.AllowShow
            });
            matchReport.GetReportData();
            domElements.divLeagueTurnoverContainer.addClass('display-none');
        });

        domElements.divLeagueTurnover.find('span.remove-exception').on('click', function () {
            var $obj = $(this);

            CMS.showConfirmMessage(messages.RemoveTOExceptionConfirmMessage, function () {
                removeException($obj.attr('data-league-id'));
            });
        });

        domElements.divLeagueTurnover.find('div.grid-container').on('scroll', function () {
            var $input = domElements.divLeagueTurnover.find('input:focus');

            if ($input.hasClass('percentage')) {
                $input.tipsy('hide');
            }
        });

        domElements.divLeagueTurnoverContainer.find('span.breadCrumb').on('click', function () {
            var index = $(this).attr('data-index');

            domElements.divLeagueTurnoverContainer.addClass('display-none');
            initialParameters.PreviousReportObjects[index].ShowReport(instructionOptions.AllowShow);
        });

        domElements.divLeagueTurnoverContainer.find('#chkShowException').on('click', function () {
            initialParameters.ShowExceptionOnly = $(this).is(':checked');
            $this.GetReportData();
        }).prop('checked', initialParameters.ShowExceptionOnly);
    };

    // Generate breadcrumb.
    var generateBreadCrumb = function () {
        var breadCrumbTemplate = '<span class="{0}" data-index="{1}">{2}</span> >> ';
        var breadCrumbClass = '';
        var breadCrumb = '<div id="divBreadCrumb">';

        if (initialParameters) {
            $.each(initialParameters.PreviousReportObjects, function (index, prevReport) {
                breadCrumbClass = prevReport.ShowReport ? 'breadCrumb' : '';
                breadCrumb += breadCrumbTemplate.format(breadCrumbClass, index, prevReport.ReportName);
            });

            breadCrumb += $this.ReportName;
        }

        return breadCrumb + '</div>';
    };

    // Validate input percentage.
    var validatePercentage = function ($input) {
        var value = $input.val().replace(/,/g, '');
        var isValid = ($.isNumeric(value) && parseInt(value) > 0) || (value.length === 1 && value[0].toLowerCase() === 'u');

        // Remove previous instruction tooltip if there is any.
        Validator.clearError($input);
        $.data($input.get(0), 'tipsy', null);
        clearTimeout(instructionOptions.TimeoutID);

        if (!isValid) {
            Validator.showError($input, messages.InvalidValueError, 'e');
        }

        return isValid;
    };

    var generateReportName = function (sportName, sportValue) {
        var value = sportValue === 0 ? labels.Unlimited : sportValue;

        return '{0} ({1})'.format(sportName, value);
    };

    // Update percentage value of s sport.
    var updatePercentage = function (leagueID, value) {
        var parameters = {
            SportID: initialParameters.SportID,
            LeagueID: leagueID,
            Value: value,
            OnlyShowException: initialParameters.ShowExceptionOnly
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateLeagueTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    renderGrid(result.Turnovers.LeagueTurnovers);

                    return true;
                } else {
                    CMS.showErrorMessage(result.ErrorMessage);

                    return false;
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

    var removeException = function (leagueID) {
        var parameters = {
            SportID: initialParameters.SportID,
            LeagueID: leagueID,
            OnlyShowException: initialParameters.ShowExceptionOnly
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.RemoveLeagueException,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    renderGrid(result.Turnovers.LeagueTurnovers);
                } else {
                    CMS.showErrorMessage(result.ErrorMessage);
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

    // Report name.
    $this.ReportName = '';

    // Show hidden report.
    $this.ShowReport = function (allowShowInstruction) {
        instructionOptions.AllowShow = allowShowInstruction;

        if (domElements.divLeagueTurnoverContainer.attr('data-current-sport') === initialParameters.SportID &&
            domElements.divLeagueTurnover.html().length > 0) {
            domElements.divLeagueTurnoverContainer.removeClass('display-none');
        } else {
            $this.GetReportData();
        }
    };

    // Get report data.
    $this.GetReportData = function () {
        var parameters = {
            SportID: initialParameters.SportID,
            OnlyShowException: initialParameters.ShowExceptionOnly
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetLeagueTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result) {
                    $this.ReportName = generateReportName(result.SportName, result.SportValue);
                    renderGrid(result.LeagueTurnovers);
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

    // Initialize report.
    $this.Initialize = function (cachedDomElements, initialParams) {
        domElements = cachedDomElements;
        initialParameters = initialParams;
        initialParameters.ShowExceptionOnly = false;
        handleEvents();
    };
};