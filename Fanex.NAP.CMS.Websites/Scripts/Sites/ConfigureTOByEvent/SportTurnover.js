/* File Created: April 17, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages, hasEditRight */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.SportTurnover');
Fanex.NAP.CMS.ConfigureTOByEvent.SportTurnover = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetSportTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetSportTurnover'),
        UpdateSportTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateSportTurnover')
    };
    var instructionOptions = {
        AllowShow: true,
        IsShown: false,
        IsFocused: false
    };
    var leagueReport = null;
    var domElements = {};

    // Initialize some common behaviors.
    var handleEvents = function () {
        $('body').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('percentage')) {
                var $input = domElements.divSportTurnover.find('input:focus');
                var $link = $input.prev();

                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
                instructionOptions.IsShown = false;
            }
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var sportFormat = '<span class="link sport-link" data-sport-id="{0}" data-sport-value="{1}">{2}</span>';
        var percentageFormat = '<span class="percentage-link link" data-value="{0}">{1}</span><input type="text" class="percentage display-none" />';
        var percentage = '';
        var gridOptions = {
            columns: [{ HeaderText: labels.Sport, Width: 350, Name: 'ColSport', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 115
        };
        var bodyData = [];
        var row;

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                percentage = item.Percentage === 0 ? labels.Unlimited : item.Percentage;
                row = {
                    Columns: [{ Name: 'ColSport', Value: sportFormat.format(item.SportID, item.Percentage, item.SportName) },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageFormat.format(item.Percentage, percentage) : percentage}]
                };

                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSportTurnover.fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var percentageLinks = domElements.divSportTurnover.find('span.percentage-link');
        var percentageInputs = domElements.divSportTurnover.find('input.percentage');

        // When user clicks on Percentage link.
        percentageLinks.on('click', function () {
            var $link = $(this);
            var $input = $link.next();
            var percentage = $link.attr('data-value');

            // Disable other active fields.
            $.each(percentageLinks, function (index, link) {
                $(link).removeClass('display-none').next().addClass('display-none').tipsy('hide');
                instructionOptions.IsShown = false;
            });

            // Fill current value into text box.
            percentage = percentage === '0' ? 'U' : percentage;
            $link.addClass('display-none');
            $input.val(percentage).removeClass('display-none').focus().select();

            // Show instruction if it is allowed.
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

                // Auto-hide after 5 seconds.
                setTimeout(function () {
                    $input.tipsy('hide');
                    instructionOptions.IsShown = false;
                }, 5000);
            }
        });

        // Format the parcentage value which is input into the text box.
        //percentageInputs.mask('Z00,000', { translation: { 'Z': { pattern: /[uU0-9]/, reverse: true, optional: true}} });
        percentageInputs.on('keyup', function (event) {
            var $input = $(this);
            var $link = $input.prev();
            var percentage = $input.val();

            if (event.keyCode === 13 && validatePercentage($input)) {
                percentage = percentage.toLowerCase() === 'u' ? '0' : percentage;
                $link.attr('data-value', percentage);
                percentage = percentage === '0' ? labels.Unlimited : percentage;
                $link.text(percentage);
                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
                instructionOptions.IsShown = false;

                updatePercentage($input.attr('data-sport-id'), percentage);
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

        domElements.divSportTurnover.find('span.sport-link').on('click', function () {
            var $obj = $(this);
            var sportID = $obj.attr('data-sport-id');
            var sportValue = $obj.attr('data-sport-value');
            var sportName = $obj.text();

            sportValue = sportValue === '0' ? labels.Unlimited : sportValue;
            leagueReport = leagueReport || new Fanex.NAP.CMS.ConfigureTOByEvent.LeagueTurnover();
            leagueReport.Initialize(domElements, {
                PreviousReportObjects: [$this],
                LastItem: '{0} ({1})'.format(sportName, sportValue),
                SportID: sportID,
                AllowShowInstruction: instructionOptions.AllowShow
            });
            leagueReport.GetReportData();
            domElements.divSportTurnoverContainer.addClass('display-none');
        });
    };

    // Validate input percentage.
    var validatePercentage = function ($input) {
        var value = $input.val().replace(/,/g, '');
        var isValid = $.isNumeric(value) || (value.length === 0 && value[0].toLowerCase() === 'u');

        Validator.clearError($input);

        if (!isValid) {
            Validator.showError($input, messages.InvalidValueError, 'e');
        }

        return isValid;
    };

    // Update percentage value of s sport.
    var updatePercentage = function (sportID, value) {
        var parameters = {
            SportID: sportID,
            Value: value
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateSportTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    renderGrid(result.SportTurnover);
                } else {
                    CMS.showErrorMessage(result.Message);
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
    $this.ShowReport = function () {
        domElements.divSportTurnoverContainer.removeClass('display-none');
    };

    // Get report data.
    $this.GetReportData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSportTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
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
    $this.Initialize = function (cachedDomElements) {
        domElements = cachedDomElements;
        $this.ReportName = labels.Sport;
        handleEvents();
    };
};