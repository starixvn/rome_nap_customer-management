/* File Created: April 16, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.ConfigureSMATOByEvent();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.ConfigureSMATOByEvent");
Fanex.CMS.ConfigureSMATOByEvent = function () {
    var $this = this;
    var urls = {
        GetInitializedData: CMS.resolveRootUrl('ConfigureSMATOByEvent/GetInitializedData'),
        GetLeaguesOfSport: CMS.resolveRootUrl('ConfigureSMATOByEvent/GetLeaguesOfSport'),
        GetSMATurnoverByEvent: CMS.resolveRootUrl('ConfigureSMATOByEvent/GetSMATurnoverByEvent'),
        InsertSMATurnoverByEvent: CMS.resolveRootUrl('ConfigureSMATOByEvent/InsertSMATurnoverByEvent'),
        UpdateSMATurnoverByEvent: CMS.resolveRootUrl('ConfigureSMATOByEvent/UpdateSMATurnoverByEvent'),
        RemoveSMATurnoverByEvent: CMS.resolveRootUrl('ConfigureSMATOByEvent/RemoveSMATurnoverByEvent')
    };
    var domElements = {
        divSMAByEventForm: null,
        ddlSport: null,
        ddlLeague: null,
        btnSearch: null,
        divSMAMaxTORate: null,
        divPopup: null,
        divNewSMATOForm: null
    };
    var popupElements = {
        divErrorSuccessMsg: null,
        divNewSMATOForm: null,
        txtEvent: null,
        hdnEvent: null,
        txtNewSMAName: null,
        txtMaxTORate: null,
        btnSave: null,
        btnCancel: null
    };
    var instructionOptions = {
        AllowShow: true,
        TimeoutID: false
    };
    var parameters = {
        SportID: null,
        LeagueID: null
    };
    var itemCounter;

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divSMAByEventForm = $('#divSMAByEventForm');
        domElements.ddlSport = $('#ddlSport');
        domElements.ddlLeague = $('#ddlLeague');
        domElements.btnSearch = $('#btnSearch');
        domElements.divSMAMaxTORate = $('#divSMAMaxTORate');
        domElements.divNewSMATOForm = $('#divNewSMATOForm').clone();
        domElements.divPopup = $('#dialog-popup');

        $('#divNewSMATOForm').empty();
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', domElements.divPopup);
        popupElements.divNewSMATOForm = $('#divNewSMATOForm', domElements.divPopup);
        popupElements.txtEvent = $('#txtEvent', domElements.divPopup);
        popupElements.hdnEvent = $('#hdnEvent', domElements.divPopup);
        popupElements.txtNewSMAName = $('#txtNewSMAName', domElements.divPopup);
        popupElements.txtMaxTORate = $('#txtMaxTORate', domElements.divPopup);
        popupElements.btnSave = $('#btnSave', domElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', domElements.divPopup);
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        var allowShowInstruction = localStorage.smaTOByEventShowInstruction === undefined ? 'true' : localStorage.smaTOByEventShowInstruction;

        instructionOptions.AllowShow = allowShowInstruction !== 'false';

        domElements.ddlSport.on('change', function () {
            var sportID = $(this).val();

            getLeaguesOfSport(sportID);
        });

        domElements.btnSearch.on('click', function () {
            // Delay 70ms for jQuery-UI combo-box to verify its state.
            setTimeout(function () {
                if (Validator.countErrors(domElements.divSMAByEventForm) === 0) {
                    parameters.SportID = domElements.ddlSport.val();
                    parameters.LeagueID = domElements.ddlLeague.val();

                    getSMATurnoverByEvent();
                }
            }, 70);
        });

        // Hide instruction tooltip.
        $('html').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('rate')) {
                var $input = domElements.divSMAMaxTORate.find('input:focus');
                var $link = $input.prev();

                if ($input.hasClass('rate')) {
                    $input.addClass('display-none');
                    $link.removeClass('display-none');
                    $input.tipsy('hide');
                }
            }
        });

        intializeFilterData();
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventhandlers = function () {
        popupElements.txtMaxTORate.numberField(0, false, 'u');

        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divNewSMATOForm, 'w')) {
                var matchID = popupElements.hdnEvent.val();
                var username = popupElements.txtNewSMAName.val();
                var rate = popupElements.txtMaxTORate.val().replace(/,/g, '');
                var isValid = ($.isNumeric(rate) && parseInt(rate) > 0) || (rate.length === 1 && rate[0].toLowerCase() === 'u');

                if (!isValid) {
                    Validator.showError(popupElements.txtMaxTORate, messages.InvalidValueError, 'w');
                } else {
                    insertMaxTuronverByEvent(matchID, username, rate);
                }
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
    };

    // Display request form.
    var displayAddNewForm = function (matchID, matchName) {
        var form = domElements.divNewSMATOForm.clone();

        CMS.closeMsgBox();
        CMS.modal(labels.ControlTOBySMAEvent, form);
        cachePopupElements();
        bindPopupEventhandlers();

        popupElements.txtEvent.val(matchName);
        popupElements.hdnEvent.val(matchID);
    };

    // Disable request form.
    var disableAddNewSMAForm = function () {
        popupElements.txtNewSMAName.prop('disabled', true);
        popupElements.txtMaxTORate.prop('disabled', true);
        popupElements.btnSave.prop('disabled', true);
        popupElements.btnCancel.prop('disabled', true);
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

    // Render grid.
    var renderGrid = function (data) {
        var rateFormat = '<span class="rate-link link" data-match-id="{0}" data-sma-id="{1}" data-value="{2}">{3}</span>' +
                           '<input type="text" maxlength="7" class="rate display-none" />';
        var actionFormat = '<span class="remove ico-delete" data-match-id="{0}" data-sma-id="{1}" title="{2}"></span>';
        var groupFormat = '<span class="group-title">' +
                              '<span class="event-date">{0}: {1}</span> -- ' +
                              '<span>{2}: {3}</span> -- ' +
                              '<span class="match-name" title="{4}">{5}</span>' +
                          '</span>';
        var addNewLink = '<span class="add-new-link" data-match-name="{0}" data-match-id="{1}"><span class="ico-add"></span>{2}</span>';
        var rate = '';
        var formattedRate = '';
        var rateCol = '';
        var actionCol = '';
        var groupRow = '';
        var matchName = '';

        var gridOptions = {
            columns: [{ HeaderText: '#', Width: 70, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 100, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.GivenCredit, Width: 150, Name: 'ColGivenCredit', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MaxTORate, Width: 120, Name: 'ColMaxTORate', HeaderAlign: 'center', CellAlign: 'right' },
                      { HeaderText: labels.MaxTOAmount, Width: 150, Name: 'ColMaxTOAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      }],
            gridExpandHeight: 150,
            showTree: true,
            showPager: true,
            showDistinctRow: false,
            //showNumberColumn: true,
            pagerOption: {
                currentPage: 0,
                itemsPerPage: CMS.pageSize,
                itemsPerPageArray: CMS.pagerRanges
            }
        };
        var bodyData = [];
        var row;
        var subRows;

        if (hasEditRight) {
            gridOptions.columns.push({ HeaderText: labels.Action, Width: 50, Name: 'ColAction', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            itemCounter = 0;

            $.each(data, function (groupIndex, group) {
                matchName = group.MatchName.length > 85 ? group.MatchName.substring(0, 80) + '...' : group.MatchName;
                groupRow = groupFormat.format(labels.Date, group.EventDate, labels.MatchCode, group.MatchCode, group.MatchName, matchName);
                subRows = [];

                $.each(group.SMAItems, function (index, item) {
                    rate = numeral(item.MaxTORate).format(CMS.formats.IntegerFormat);
                    formattedRate = item.MaxTORate === 0 ? labels.Unlimited : rate;
                    rateCol = rateFormat.format(item.MatchID, item.CustID, rate, formattedRate);
                    actionCol = actionFormat.format(item.MatchID, item.CustID, labels.Remove);
                    itemCounter++;

                    row = {
                        Columns: [{ Name: 'ColOrder', Value: itemCounter },
                                  { Name: 'ColSMAName', Value: item.Username },
                                  { Name: 'ColLevel', Value: item.Level },
                                  { Name: 'ColCurrency', Value: item.Currency },
                                  { Name: 'ColGivenCredit', Value: item.GivenCredit },
                                  { Name: 'ColMaxTORate', Value: hasEditRight ? rateCol : formattedRate },
                                  { Name: 'ColMaxTOAmount', Value: item.MaxTOAmount === 0 ? labels.Unlimited : item.MaxTOAmount}]
                    };

                    if (hasEditRight) {
                        row.Columns.push({ Name: 'ColAction', Value: actionCol });
                    }

                    subRows.push(row);
                });

                if (hasEditRight) {
                    groupRow += addNewLink.format(group.MatchName, group.MatchID, labels.AddSMA);
                }

                row = {
                    Columns: [{ Name: 'ColOrder', Value: groupRow, CellAlign: 'left', MergeColNumber: 8}],
                    SubRows: subRows
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSMAMaxTORate.fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var rateLinks = domElements.divSMAMaxTORate.find('span.rate-link');
        var rateInputs = domElements.divSMAMaxTORate.find('input.rate');

        // Format the rate value which is input into the text box.
        rateInputs.numberField(0, false, 'u');

        // When user clicks on rate link.
        rateLinks.on('click', function () {
            var $link = $(this);
            var $input = $link.next();
            var rate = $link.attr('data-value');

            // Disable other active fields.
            $.each(rateLinks, function (index, link) {
                $(link).removeClass('display-none').next().addClass('display-none').tipsy('hide');
            });

            // Fill current value into text box.
            rate = rate === '0' ? 'U' : rate;
            $link.addClass('display-none');
            $input.val(rate).removeClass('display-none').clearfield().focus().select();

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
                        localStorage.setItem('smaTOByEventShowInstruction', 'false');
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
        rateInputs.on('keyup', function (event) {
            var $input = $(this);
            var $link = $input.prev();
            var rate = $input.val();
            var formatedRate = rate.toLowerCase() === 'u' ? labels.Unlimited : rate;

            if (event.keyCode === 13 && validateRate($input)) {
                CMS.showConfirmMessage(messages.ProceedConfirmMessage, function () {
                    var matchID = $link.attr('data-match-id');
                    var custID = $link.attr('data-sma-id');

                    rate = rate.toLowerCase() === 'u' ? '0' : rate;

                    if (updateMaxTuronverRate(matchID, custID, rate.replace(/,/g, ''))) {
                        $link.attr('data-value', rate);
                        $link.text(formatedRate);
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
        domElements.divSMAMaxTORate.find('div.grid-container').on('scroll', function () {
            var $input = domElements.divSMAMaxTORate.find('input:focus');

            if ($input.hasClass('rate')) {
                $input.tipsy('hide');
            }
        });

        // Hande clicking event on delete icon.
        domElements.divSMAMaxTORate.find('span.remove').on('click', function () {
            var $obj = $(this);
            var matchID = $obj.attr('data-match-id');
            var custID = $obj.attr('data-sma-id');

            CMS.showConfirmMessage(messages.RemoveSettingConfirmMessage, function () {
                removeSetting(matchID, custID);
            });
        });

        // Hande clicking event on delete icon.
        domElements.divSMAMaxTORate.find('span.add-new-link').on('click', function () {
            var $obj = $(this);
            var matchID = $obj.attr('data-match-id');
            var matchName = $obj.attr('data-match-name');

            displayAddNewForm(matchID, matchName);
        });
    };

    // Validate input rate.
    var validateRate = function ($input) {
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

    // Initialize filter form data.
    var intializeFilterData = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetInitializedData,
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

    // Get SMA turnover by event.
    var getSMATurnoverByEvent = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSMATurnoverByEvent,
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

    // Remove exception.
    var removeSetting = function (matchID, custID) {
        var removingParameters = {
            MatchID: matchID,
            CustID: custID,
            SportID: parameters.SportID,
            LeagueID: parameters.LeagueID
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.RemoveSMATurnoverByEvent,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(removingParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.RemoveSettingSuccessfully);
                    renderGrid(result.SMATurnovers);
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

    // Update max turnover rate value of SMA.
    var updateMaxTuronverRate = function (matchID, custID, value) {
        var updatingParameters = {
            MatchID: matchID,
            CustID: custID,
            Value: value,
            SportID: parameters.SportID,
            LeagueID: parameters.LeagueID
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateSMATurnoverByEvent,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updatingParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.UpdateSettingSuccessfully);
                    renderGrid(result.SMATurnovers);

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

    // Insert turnover value on a matc of SMA.
    var insertMaxTuronverByEvent = function (matchID, username, value) {
        var isertingParameters = {
            MatchID: matchID,
            SMAName: username,
            Value: value,
            SportID: parameters.SportID,
            LeagueID: parameters.LeagueID
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.InsertSMATurnoverByEvent,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(isertingParameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineInfoMsg(messages.AddSMATOSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disableAddNewSMAForm();
                    renderGrid(result.SMATurnovers);
                } else {
                    CMS.showInlineErrorMsg(result.ErrorMessage, popupElements.divErrorSuccessMsg);

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

    $this.InitPage = function () {
        cacheDOM();
        initializeFields();
    };
};