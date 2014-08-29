/* File Created: April 17, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */
/* jshint strict: false */
// 'use strict';

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.SportPage');
Fanex.NAP.CMS.ConfigureTOByEvent.SportPage = function () {
    var $this = this;
    var urls = {
        GetSportTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetSportTurnover'),
        UpdateSportTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateSportTurnover')
    };
    var instructionOptions = {
        AllowShow: true,
        TimeoutID: null
    };
    var domElements = {
        divSportContent: null,
        divSportTurnoverContainer: null,
        divSportTurnover: null
    };

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        domElements.divSportContent = domElements.divSportContent || $('#divSportContent');
        domElements.divSportTurnoverContainer = domElements.divSportContent.find('#divSportTurnoverContainer');
        domElements.divSportTurnover = domElements.divSportContent.find('#divSportTurnover');
    };

    // Initialize some common behaviors.
    var handleEvents = function () {
        $('html').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('percentage')) {
                var $input = domElements.divSportTurnover.find('input:focus');
                var $link = $input.prev();

                if ($input.hasClass('percentage')) {
                    $input.addClass('display-none');
                    $link.removeClass('display-none');
                    $input.tipsy('hide');
                }
            }
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var percentageFormat = '<span class="percentage-link link" data-value="{0}" data-sport="{1}">{2}</span>' +
                               '<input type="text" maxlength="3" class="percentage display-none" />';
        var percentage = '';
        var formattedPercentage = '';
        var percentageCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.Sport, Width: 350, Name: 'ColSport', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 115,
            showPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges
            }
        };
        var bodyData = [];
        var row;

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                percentage = numeral(item.Percentage).format(CMS.formats.IntegerFormat);
                formattedPercentage = item.Percentage === 0 ? labels.Unlimited : percentage;
                percentageCol = percentageFormat.format(percentage, item.SportID, formattedPercentage);

                row = {
                    Columns: [{ Name: 'ColSport', Value: item.SportName },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageCol : formattedPercentage}]
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

                // Clear previous error tooltip data if there is any.
                $input.removeAttr('original-title').tipsy('hide');
                $.data($input.get(0), 'tipsy', null);

                $input.attr('title', instruction).tipsy({
                    trigger: 'manual',
                    html: true,
                    opacity: 0.97,
                    gravity: 's'
                }).tipsy('show');

                $('#chkInstruction').on('change', function () {
                    if ($(this).is(':checked')) {
                        localStorage.setItem('sportTOShowInstruction', 'false');
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
                CMS.showConfirmMessage(messages.UpdatePercentageConfirmMessage, function () {
                    percentage = percentage.toLowerCase() === 'u' ? '0' : percentage;

                    if (updatePercentage($link.attr('data-sport'), percentage.replace(/,/g, ''))) {
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

        // Hide instruction tooltip when scrolling.
        domElements.divSportTurnover.find('div.grid-container').on('scroll', function () {
            var $input = domElements.divSportTurnover.find('input:focus');

            if ($input.hasClass('percentage')) {
                $input.tipsy('hide');
            }
        });
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

    // Get report data.
    var getReportData = function () {
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
                    CMS.showSuccessMessage(messages.UpdatePercentageSuccessfully);
                    renderGrid(result.Turnovers);

                    return false;
                } else {
                    CMS.showErrorMessage(result.ErrorMessage);

                    return true;
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

    $this.CurrentReport = null;

    $this.InitPage = function (divContent) {
        var allowShowInstruction = localStorage.sportTOShowInstruction === undefined ? 'true' : localStorage.sportTOShowInstruction;

        domElements.divSportContent = divContent;
        instructionOptions.AllowShow = allowShowInstruction !== 'false';
        cacheDOMElements();
        handleEvents();
        getReportData();
    };
};