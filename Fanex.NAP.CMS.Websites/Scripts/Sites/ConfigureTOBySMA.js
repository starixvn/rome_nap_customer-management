/* File Created: April 16, 2014 */
/* global Namespace, numeral, Fanex, CMS, Validator, labels, messages, hasEditRight */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.ConfigureTOBySMA();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.ConfigureTOBySMA");
Fanex.CMS.ConfigureTOBySMA = function () {
    var $this = this;
    var urls = {
        GetSMAOffset: CMS.resolveRootUrl('ConfigureTOBySMA/GetSMAOffset'),
        InsertSMAOffset: CMS.resolveRootUrl('ConfigureTOBySMA/InsertSMAOffset'),
        UpdateSMAOffset: CMS.resolveRootUrl('ConfigureTOBySMA/UpdateSMAOffset'),
        DeleteSMAOffset: CMS.resolveRootUrl('ConfigureTOBySMA/DeleteSMAOffset')
    };
    var domElements = {
        txtSMAName: null,
        btnSearch: null,
        btnAddSMA: null,
        divSMAOffset: null,
        divNewSMAOffsetForm: null,
        divPopup: null
    };
    var popupElements = {
        divErrorSuccessMsg: null,
        divNewSMAOffsetForm: null,
        txtNewSMAName: null,
        txtOffset: null,
        btnSave: null,
        btnCancel: null
    };
    var instructionOptions = {
        AllowShow: true,
        TimeoutID: false
    };
    var searchedName = '';

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.txtSMAName = $('#txtSMAName');
        domElements.ddlStatus = $('#ddlStatus');
        domElements.btnSearch = $('#btnSearch');
        domElements.btnAddSMA = $('#btnAddSMA');
        domElements.divSMAOffset = $('#divSMAOffset');
        domElements.divNewSMAOffsetForm = $('#divNewSMAOffsetForm').clone();
        domElements.divPopup = $('#dialog-popup');

        $('#divNewSMAOffsetForm').empty();
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', domElements.divPopup);
        popupElements.divNewSMAOffsetForm = $('#divNewSMAOffsetForm', domElements.divPopup);
        popupElements.txtNewSMAName = $('#txtNewSMAName', domElements.divPopup);
        popupElements.txtOffset = $('#txtOffset', domElements.divPopup);
        popupElements.btnSave = $('#btnSave', domElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', domElements.divPopup);
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        var allowShowInstruction = localStorage.smaTOShowInstruction === undefined ? 'true' : localStorage.smaTOShowInstruction;

        instructionOptions.AllowShow = allowShowInstruction !== 'false';

        domElements.txtSMAName.on('keyup', function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });
        domElements.txtSMAName.focus();

        domElements.btnSearch.on('click', function () {
            searchedName = $.trim(domElements.txtSMAName.val());
            domElements.txtSMAName.val(searchedName);
            getSMAOffset();
        });

        domElements.btnAddSMA.on('click', function () {
            displayAddNewForm();
        });

        // Hide instruction tooltip.
        $('html').on('mousedown', function (event) {
            if ($(event.target).closest('div.tipsy').length === 0 && !$(event.target).hasClass('offset')) {
                var $input = domElements.divSMAOffset.find('input:focus');
                var $link = $input.prev();

                if ($input.hasClass('offset')) {
                    $input.addClass('display-none');
                    $link.removeClass('display-none');
                    $input.tipsy('hide');
                }
            }
        });
    };

    // Display request form.
    var displayAddNewForm = function () {
        var form = domElements.divNewSMAOffsetForm.clone();

        CMS.closeMsgBox();
        CMS.modal(labels.NewSMATOControl, form);
        cachePopupElements();

        bindPopupEventhandlers();
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventhandlers = function () {
        popupElements.txtOffset.numberField(0, true, 'u');

        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divNewSMAOffsetForm, 'w')) {
                var username = $.trim(popupElements.txtNewSMAName.val());
                var offset = popupElements.txtOffset.val().replace(/,/g, '');
                var isValid = ($.isNumeric(offset) && parseInt(offset) !== 0) || (offset.length === 1 && offset[0].toLowerCase() === 'u');

                popupElements.txtNewSMAName.val(username);

                if (!isValid) {
                    Validator.showError(popupElements.txtOffset, messages.InvalidValueError, 'w');
                } else {
                    insertOffset(username, offset);
                }
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
    };

    // Disable request form.
    var disableAddNewSMAForm = function () {
        popupElements.txtNewSMAName.prop('disabled', true);
        popupElements.txtOffset.prop('disabled', true);
        popupElements.btnSave.prop('disabled', true);
        popupElements.btnCancel.prop('disabled', true);
    };

    // Render grid.
    var renderGrid = function (data) {
        var offsetFormat = '<span class="offset-link link" data-cust-id="{0}" data-value="{1}">{2}</span>' +
                           '<input type="text" maxlength="7" class="offset display-none" />';
        var offset = '';
        var formattedOffset = '';
        var offsetCol = '';

        var gridOptions = {
            columns: [{ HeaderText: '#', Width: 70, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 100, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.GivenCredit, Width: 150, Name: 'ColGivenCredit', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.Offset, Width: 120, Name: 'ColOffset', HeaderAlign: 'center', CellAlign: 'right'}],
            gridExpandHeight: 150,
            showPager: true,
            showNumberColumn: true,
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
                offset = numeral(item.Offset).format(CMS.formats.IntegerFormat);
                formattedOffset = item.Offset === 0 ? labels.Unlimited : offset;
                offsetCol = offsetFormat.format(item.CustID, offset, formattedOffset);

                row = {
                    Columns: [{ Name: 'ColOrder', Value: index },
                              { Name: 'ColSMAName', Value: item.Username },
                              { Name: 'ColLevel', Value: item.Level },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColGivenCredit', Value: item.GivenCredit },
                              { Name: 'ColOffset', Value: hasEditRight ? offsetCol : formattedOffset}]
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSMAOffset.fanexGrid(gridOptions);
        handleGridEvents();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var offsetLinks = domElements.divSMAOffset.find('span.offset-link');
        var offsetInputs = domElements.divSMAOffset.find('input.offset');

        // Format the offset value which is input into the text box.
        offsetInputs.numberField(0, true, 'u');

        // When user clicks on Offset link.
        offsetLinks.on('click', function () {
            var $link = $(this);
            var $input = $link.next();
            var offset = $link.attr('data-value');

            // Disable other active fields.
            $.each(offsetLinks, function (index, link) {
                $(link).removeClass('display-none').next().addClass('display-none').tipsy('hide');
            });

            // Fill current value into text box.
            offset = offset === '0' ? 'U' : offset;
            $link.addClass('display-none');
            $input.val(offset).removeClass('display-none').clearfield().focus().select();

            // Show instruction if it is allowed.
            if (instructionOptions.AllowShow) {
                var instruction = '<div id="divInstruction">' +
                                      '{0}<br/>' +
                                      '{1}<br/>' +
                                      '<input type="checkbox" id="chkInstruction" class="instruction"/>' +
                                      '<label for="chkInstruction">{2}</label>' +
                                  '</div>';

                instruction = instruction.format(messages.TurnoverInputInstruction, messages.RemoveOffsetInstruction, messages.NotShowAgain);

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
                        localStorage.setItem('smaTOShowInstruction', 'false');
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
        offsetInputs.on('keyup', function (event) {
            var $input = $(this);
            var $link = $input.prev();
            var offset = $input.val();
            var formatedOffset = offset.toLowerCase() === 'u' ? labels.Unlimited : offset;

            if (event.keyCode === 13 && validateOffset($input)) {
                if (offset !== '0') {
                    CMS.showConfirmMessage(messages.ProceedConfirmMessage, function () {
                        offset = offset.toLowerCase() === 'u' ? '0' : offset;

                        if (updateOffset($link.attr('data-cust-id'), offset.replace(/,/g, ''))) {
                            $link.attr('data-value', offset);
                            $link.text(formatedOffset);
                            $input.addClass('display-none');
                            $link.removeClass('display-none');
                            $input.tipsy('hide');
                        }
                    });
                } else {
                    CMS.showConfirmMessage(messages.ProceedConfirmMessage, function () {
                        offset = offset.toLowerCase() === 'u' ? '0' : offset;
                        removeException($link.attr('data-cust-id'));
                    });
                }
            } else if (event.keyCode === 27) {
                $input.addClass('display-none');
                $link.removeClass('display-none');
                $input.tipsy('hide');
            }
        });

        domElements.divSMAOffset.find('div.grid-container').on('scroll', function () {
            var $input = domElements.divSMAOffset.find('input:focus');

            if ($input.hasClass('offset')) {
                $input.tipsy('hide');
            }
        });
    };

    // Validate input offset.
    var validateOffset = function ($input) {
        var value = $input.val().replace(/,/g, '');
        var isValid = $.isNumeric(value) || (value.length === 1 && value[0].toLowerCase() === 'u');

        // Remove previous instruction tooltip if there is any.
        Validator.clearError($input);
        $.data($input.get(0), 'tipsy', null);
        clearTimeout(instructionOptions.TimeoutID);

        if (!isValid) {
            Validator.showError($input, messages.InvalidValueError, 'e');
        }

        return isValid;
    };

    // Get member list.
    var getSMAOffset = function () {
        var parameters = {
            SMAName: searchedName
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSMAOffset,
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

    // Insert offset value of SMA.
    var insertOffset = function (username, value) {
        var parameters = {
            SMAName: username,
            Value: value,
            SearchedName: searchedName
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.InsertSMAOffset,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineInfoMsg(messages.AddSMATOSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disableAddNewSMAForm();
                    renderGrid(result.OffsetItems);
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

    // Update offset value of SMA.
    var updateOffset = function (custID, value) {
        var parameters = {
            CustID: custID,
            Value: value,
            SearchedName: searchedName
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateSMAOffset,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.UpdateOffsetSuccessfully);
                    renderGrid(result.OffsetItems);

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

    // Remove exception.
    var removeException = function (custID) {
        var parameters = {
            CustID: custID,
            SearchedName: searchedName
        };

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.DeleteSMAOffset,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInfoMessage(messages.RemoveOffsetSuccessfully);
                    renderGrid(result.OffsetItems);

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

    $this.InitPage = function () {
        cacheDOM();
        initializeFields();
    };
};