/* File Created: April 17, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage');
Fanex.NAP.CMS.ConfigureTOByEvent.MatchPage = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetMatchTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetMatchTurnover'),
        UpdateMatchTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateMatchTurnover'),
        RemoveMatchException: CMS.resolveRootUrl('ConfigureTOByEvent/RemoveMatchException'),
        GetInitializedMatchData: CMS.resolveRootUrl('ConfigureTOByEvent/GetInitializedMatchData'),
        GetLeaguesOfSport: CMS.resolveRootUrl('ConfigureTOByEvent/GetLeaguesOfSport')
    };
    var instructionOptions = {
        AllowShow: true,
        IsFocused: false
    };
    var domElements = {
        divMatchContent: null,
        divMatchForm: null,
        ddlSport: null,
        ddlLeague: null,
        txtMatchCode: null,
        btnSearch: null,
        divMatchTurnoverContainer: null,
        divMatchTurnover: null
    };
    var parameters = {
        SportID: null,
        LeagueID: null,
        MatchCode: null,
        OnlyShowException: false
    };
    var gridTitle = '';

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        // Use class selector for Sport and League to overcome the duplication issue caused by jquery.combobox.
        domElements.divMatchContent = domElements.divMatchContent || $('#divMatchContent');
        domElements.divMatchForm = domElements.divMatchContent.find('#divMatchForm');
        domElements.ddlSport = domElements.divMatchContent.find('select.sport');
        domElements.ddlLeague = domElements.divMatchContent.find('select.league');
        domElements.txtMatchCode = domElements.divMatchContent.find('#txtMatchCode');
        domElements.btnSearch = domElements.divMatchContent.find('#btnMatchSearch');
        domElements.divMatchTurnoverContainer = domElements.divMatchContent.find('#divMatchTurnoverContainer');
        domElements.divMatchTurnover = domElements.divMatchContent.find('#divMatchTurnover');
    };

    // Bind event handlers.
    var bindEventHandlers = function () {
        Validator.clearErrors(domElements.divMatchForm);

        domElements.ddlSport.off('change').on('change', function () {
            var sportID = $(this).val();

            getLeaguesOfSport(sportID);
        });

        domElements.txtMatchCode.off('keyup').on('keyup', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.btnSearch.off('click').on('click', function () {
            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (Validator.countErrors(domElements.divMatchForm) === 0) {
                    parameters.SportID = domElements.ddlSport.val();
                    parameters.LeagueID = domElements.ddlLeague.val();
                    parameters.MatchCode = $.trim(domElements.txtMatchCode.val());
                    parameters.OnlyShowException = false;
                    domElements.txtMatchCode.val(parameters.MatchCode);

                    getReportData();
                }
            }, 70);
        });

        $('html').off('mousedown').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('percentage')) {
                var $input = domElements.divMatchTurnover.find('input:focus');
                var $link = $input.prev();

                if ($input.hasClass('percentage')) {
                    $input.addClass('display-none');
                    $link.removeClass('display-none');
                    $input.tipsy('hide');
                }
            }
        });

        domElements.divMatchTurnoverContainer.addClass('display-none');
    };

    // Render grid.
    var renderGrid = function (data) {
        var percentageFormat = '<span class="percentage-link link" data-match-id="{0}" data-value="{1}">{2}</span>' +
                               '<input type="text" maxlength="3" class="percentage display-none" />';
        var exceptionFormat = '<span class="remove-exception ico-delete" data-match-id="{0}" title="{1}"></span>';
        var showExceptionChk = '<input type="checkbox" id="chkShowExceptionOnly" class="show-exception" /><label for="chkShowExceptionOnly">{0}</label>';
        var percentage = '';
        var formattedPercentage = '';
        var percentageCol = '';
        var exceptionCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.Code, Width: 100, Name: 'ColCode', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Event, Width: 450, Name: 'ColEvent', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.EventDate, Width: 100, Name: 'ColEventDate', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 200,
            showTitle: true,
            titleInfo: {
                showButtonsWhenNoData: true,
                text: gridTitle,
                customButtons: showExceptionChk.format(labels.OnlyShowException)
            },
            showPager: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges
            }
        };
        var bodyData = [];
        var row;

        if (hasEditRight) {
            gridOptions.columns.push({ HeaderText: labels.Exception, Width: 70, Name: 'ColException', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                percentage = numeral(item.Percentage).format(CMS.formats.IntegerFormat);
                formattedPercentage = item.Percentage === 0 ? labels.Unlimited : percentage;
                percentageCol = percentageFormat.format(item.MatchID, percentage, formattedPercentage);
                exceptionCol = item.IsException ? exceptionFormat.format(item.MatchID, labels.RemoveException) : '';

                row = {
                    Columns: [{ Name: 'ColCode', Value: item.MatchCode },
                              { Name: 'ColEvent', Value: item.MatchName },
                              { Name: 'ColEventDate', Value: item.EventDate },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageCol : formattedPercentage}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColException', Value: exceptionCol });
                }

                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divMatchTurnover.fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var percentageLinks = domElements.divMatchTurnover.find('span.percentage-link');
        var percentageInputs = domElements.divMatchTurnover.find('input.percentage');

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
                        localStorage.setItem('matchTOShowInstruction', 'false');
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
                CMS.showConfirmMessage(messages.UpdateMatchPercentageConfirmMessage, function () {
                    percentage = percentage.toLowerCase() === 'u' ? '0' : percentage;

                    if (updatePercentage($link.attr('data-match-id'), percentage.replace(/,/g, ''))) {
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

        domElements.divMatchTurnover.find('span.remove-exception').on('click', function () {
            var $obj = $(this);

            CMS.showConfirmMessage(messages.RemoveMatchTOConfirmationMessage, function () {
                removeException($obj.attr('data-match-id'));
            });
        });

        domElements.divMatchTurnover.find('div.grid-container').on('scroll', function () {
            var $input = domElements.divMatchTurnover.find('input:focus');

            if ($input.hasClass('percentage')) {
                $input.tipsy('hide');
            }
        });

        domElements.divMatchTurnoverContainer.find('#chkShowExceptionOnly').on('click', function () {
            parameters.OnlyShowException = $(this).is(':checked');
            getReportData();
        }).prop('checked', parameters.OnlyShowException);
    };

    var generateTitle = function (data) {
        var sportValue = data.SportValue === 0 ? labels.Unlimited : data.SportValue + '%';
        var leagueValue = data.LeagueValue === 0 ? labels.Unlimited : data.LeagueValue + '%';
        var leagueName = data.LeagueName;

        if (leagueName.length > 60) {
            leagueName = leagueName.substring(0, 60) + '...';
        }

        return '{0} ({1}) >> <span title="{2}">{3}</span> ({4})'.format(data.SportName, sportValue, data.LeagueName, leagueName, leagueValue);
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

    // Initialize filter form data.
    var intializeFilterData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedMatchData,
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

    // Get report data.
    var getReportData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetMatchTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result) {
                    gridTitle = generateTitle(result);
                    renderGrid(result.MatchTurnovers);
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

    // Update percentage value of s sport.
    var updatePercentage = function (matchID, value) {
        var updateParameters = {
            SportID: parameters.SportID,
            LeagueID: parameters.LeagueID,
            MatchID: matchID,
            MatchCode: parameters.MatchCode,
            Value: value,
            OnlyShowException: parameters.OnlyShowException
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateMatchTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updateParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.UpdatePercentageSuccessfully);
                    renderGrid(result.Turnovers.MatchTurnovers);

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

    var removeException = function (matchID) {
        var removeParameters = {
            SportID: parameters.SportID,
            LeagueID: parameters.LeagueID,
            MatchID: matchID,
            MatchCode: parameters.MatchCode,
            OnlyShowException: parameters.OnlyShowException
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.RemoveMatchException,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(removeParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.RemoveTOExceptionSuccessfully);
                    renderGrid(result.Turnovers.MatchTurnovers);
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
        var allowShowInstruction = localStorage.matchTOShowInstruction === undefined ? 'true' : localStorage.matchTOShowInstruction;

        domElements.divMatchContent = divContent;
        instructionOptions.AllowShow = allowShowInstruction !== 'false';
        cacheDOMElements();
        intializeFilterData();
        bindEventHandlers();
    };
};