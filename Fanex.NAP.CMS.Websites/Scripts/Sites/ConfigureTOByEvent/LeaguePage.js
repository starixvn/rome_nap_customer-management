/* File Created: April 17, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */
/* jshint strict: false */
// 'use strict';

Namespace.Register('Fanex.NAP.CMS.ConfigureTOByEvent.LeaguePage');
Fanex.NAP.CMS.ConfigureTOByEvent.LeaguePage = function () {
    var $this = this;
    var domElements = {
        divLeagueContent: null,
        divLeagueForm: null,
        ddlSport: null,
        ddlSpecialLeague: null,
        txtLeague: null,
        ddlCountry: null,
        ddlMarket: null,
        btnSearch: null,
        divLeagueTurnoverContainer: null,
        divLeagueTurnover: null
    };
    var urls = {
        GetInitializedLeagueData: CMS.resolveRootUrl('ConfigureTOByEvent/GetInitializedLeagueData'),
        GetLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/GetLeagueTurnover'),
        UpdateLeagueTurnover: CMS.resolveRootUrl('ConfigureTOByEvent/UpdateLeagueTurnover'),
        RemoveLeagueException: CMS.resolveRootUrl('ConfigureTOByEvent/RemoveLeagueException')
    };
    var parameters = {
        SportID: null,
        ProgramID: null,
        DisplayMode: null,
        CountryCode: null,
        AllMarket: null,
        LeagueName: null,
        OnlyShowException: null
    };
    var instructionOptions = {
        AllowShow: true,
        IsFocused: false
    };
    var gridTitle = '';

    // Cache DOM Elements.
    var cacheDOMElements = function () {
        // Use class selector for Sport and League to overcome the duplication issue caused by jquery.combobox.
        domElements.divLeagueContent = domElements.divLeagueContent || $('#divLeagueContent');
        domElements.divLeagueForm = domElements.divLeagueContent.find('#divLeagueForm');
        domElements.ddlSport = domElements.divLeagueContent.find('select.sport');
        domElements.ddlSpecialLeague = domElements.divLeagueContent.find('select.special-league');
        domElements.txtLeague = domElements.divLeagueContent.find('#txtLeague');
        domElements.ddlCountry = domElements.divLeagueContent.find('select.country');
        domElements.ddlMarket = domElements.divLeagueContent.find('select.market');
        domElements.btnSearch = domElements.divLeagueContent.find('#btnLeagueSearch');
        domElements.divLeagueTurnoverContainer = domElements.divLeagueContent.find('#divLeagueTurnoverContainer');
        domElements.divLeagueTurnover = domElements.divLeagueContent.find('#divLeagueTurnover');
    };

    // Bind event handlers.
    var bindEventHandlers = function () {
        Validator.clearErrors(domElements.divLeagueForm);

        domElements.txtLeague.off('keyup').on('keyup', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.btnSearch.off('click').on('click', function () {
            var specialLeagueValues = domElements.ddlSpecialLeague.val().split(';');
            var displayMode = 0;
            var programID = '';

            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (specialLeagueValues.length > 1) {
                    displayMode = specialLeagueValues[0];
                    programID = specialLeagueValues[1];
                }

                if (Validator.countErrors(domElements.divLeagueForm) === 0) {
                    parameters.SportID = domElements.ddlSport.val();
                    parameters.CountryCode = domElements.ddlCountry.val();
                    parameters.CountryCode = parameters.CountryCode === 'null' ? '' : parameters.CountryCode;
                    parameters.AllMarket = domElements.ddlMarket.val();
                    parameters.LeagueName = $.trim(domElements.txtLeague.val());
                    parameters.DisplayMode = displayMode;
                    parameters.ProgramID = programID;
                    domElements.txtLeague.val(parameters.LeagueName);

                    getReportData();
                }
            }, 70);
        });

        // Hide instruction tipsy when clicking on document.
        $('html').off('mousedown').on('mousedown', function (event) {
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

        domElements.divLeagueTurnoverContainer.addClass('display-none');
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
    var updateSpecialLeagueDropdown = function (data) {
        // Clear old values.
        domElements.ddlSpecialLeague.html('');

        // Add default item.
        domElements.ddlSpecialLeague.append($('<option>').attr('value', '0;').text(labels.AllLeagues));
        domElements.ddlSpecialLeague.val(null).next().children('input').val(labels.AllLeagues);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlSpecialLeague.append($('<option>').attr('value', '{0};{1}'.format(item.DisplayMode, item.ProgramID)).text(item.DisplayName));
            });
        }
    };

    // Update League drop-down list.
    var updateCountryDropdown = function (data) {
        // Clear old values.
        domElements.ddlCountry.html('');

        // Add default item.
        domElements.ddlCountry.append($('<option>').attr('value', 'null').text(labels.AllCountries));
        domElements.ddlCountry.val(null).next().children('input').val(labels.AllCountries);

        // Add new values.
        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                domElements.ddlCountry.append($('<option>').attr('value', item.CountryCode).text(item.CountryName));
            });
        }
    };

    // Initialize filter form data.
    var intializeFilterData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedLeagueData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                if (result.Sports) {
                    updateSportDropdown(result.Sports);
                }

                if (result.Countries) {
                    updateCountryDropdown(result.Countries);
                }

                if (result.SpecialLeagues) {
                    updateSpecialLeagueDropdown(result.SpecialLeagues);
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

    // Render grid.
    var renderGrid = function (data) {
        var percentageFormat = '<span class="percentage-link link" data-league-id="{0}" data-value="{1}">{2}</span>' +
                               '<input type="text" maxlength="3" class="percentage display-none" />';
        var exceptionFormat = '<span class="remove-exception ico-delete" data-league-id="{0}" title="{1}"></span>';
        var showExceptionChk = '<input type="checkbox" id="chkShowException" class="show-exception" /><label for="chkShowException">{0}</label>';
        var percentage = '';
        var formattedPercentage = '';
        var percentageCol = '';
        var exceptionCol = '';

        var gridOptions = {
            columns: [{ HeaderText: labels.League, Width: 650, Name: 'ColLeague', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Percentage, Width: 120, Name: 'ColPercentage', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 220,
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
                percentageCol = percentageFormat.format(item.LeagueID, percentage, formattedPercentage);
                exceptionCol = item.IsException ? exceptionFormat.format(item.LeagueID, labels.RemoveException) : '';

                row = {
                    Columns: [{ Name: 'ColLeague', Value: item.LeagueName },
                              { Name: 'ColPercentage', Value: hasEditRight ? percentageCol : formattedPercentage}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColException', Value: exceptionCol });
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
                        localStorage.setItem('leagueTOShowInstruction', 'false');
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

        domElements.divLeagueTurnoverContainer.find('#chkShowException').on('click', function () {
            parameters.OnlyShowException = $(this).is(':checked');
            getReportData();
        }).prop('checked', parameters.OnlyShowException);
    };

    // Generate grid title.
    var generateTitle = function (sportName, sportValue) {
        var title = '';

        if (sportName && sportValue) {
            title = '{0} ({1})'.format(sportName, sportValue === 0 ? labels.Unlimited : sportValue + '%');
        }

        return title;
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

    // Update percentage value of s sport.
    var updatePercentage = function (leagueID, value) {
        var gettingParameters = {
            SportID: parameters.SportID,
            LeagueID: leagueID,
            Value: value,
            ProgramID: parameters.ProgramID,
            DisplayMode: parameters.DisplayMode,
            CountryCode: parameters.CountryCode,
            AllMarket: parameters.AllMarket,
            LeagueName: parameters.LeagueName,
            OnlyShowException: parameters.OnlyShowException
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateLeagueTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(gettingParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showSuccessMessage(messages.UpdatePercentageSuccessfully);
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

    // Remove league exception.
    var removeException = function (leagueID) {
        var removeParameters = {
            SportID: parameters.SportID,
            LeagueID: leagueID,
            ProgramID: parameters.ProgramID,
            DisplayMode: parameters.DisplayMode,
            CountryCode: parameters.CountryCode,
            AllMarket: parameters.AllMarket,
            LeagueName: parameters.LeagueName,
            OnlyShowException: parameters.OnlyShowException
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.RemoveLeagueException,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(removeParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showSuccessMessage(messages.RemoveTOExceptionSuccessfully);
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

    // Get report data.
    var getReportData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetLeagueTurnover,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result) {
                    gridTitle = generateTitle(result.SportName, result.SportValue);
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

    $this.InitPage = function (divContent) {
        var allowShowInstruction = localStorage.leagueTOShowInstruction === undefined ? 'true' : localStorage.leagueTOShowInstruction;

        domElements.divLeagueContent = divContent;
        instructionOptions.AllowShow = allowShowInstruction !== 'false';
        cacheDOMElements();
        intializeFilterData();
        bindEventHandlers();
    };
};