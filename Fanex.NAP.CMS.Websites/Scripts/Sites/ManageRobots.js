/* File Created: May 30, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages, hasEditRight */
/* jshint strict: false */
// 'use strict';

$(document).ready(function () {
    var manageRobots = new Fanex.CMS.ManageRobots();

    manageRobots.InitPage();
});

Namespace.Register("Fanex.CMS.ManageRobots");
Fanex.CMS.ManageRobots = function () {
    var $this = this;
    var urls = {
        GetRobotList: CMS.resolveRootUrl('ManageRobots/GetRobotList'),
        UpdateRobotStatus: CMS.resolveRootUrl('ManageRobots/UpdateRobotStatus'),
        AddRobotAccount: CMS.resolveRootUrl('ManageRobots/AddRobotAccount'),
        EditRobotInfo: CMS.resolveRootUrl('ManageRobots/EditRobotInfo')
    };
    var parameters = {
        Username: null,
        Status: null,
        PageIndex: null,
        PageSize: null
    };
    var domElements = {
        divFilterForm: null,
        txtUsername: null,
        ddlStatus: null,
        btnSearch: null,
        btnAddRobot: null,
        divRobotList: null,
        divButtons: null,
        btnActivate: null,
        btnActivateValue: null,
        btnDeactivate: null,
        btnDeactivateValue: null,
        divRobotForm: null,
        divPopup: null
    };
    var popupElements = {
        divErrorSuccessMsg: null,
        divRobotForm: null,
        hdnCustID: null,
        txtNewUsername: null,
        divInstruction: null,
        txtRemark: null,
        btnSave: null,
        btnCancel: null
    };
    var editBackupData = null;

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divFilterForm = $('#divFilterForm');
        domElements.txtUsername = $('#txtUsername');
        domElements.ddlStatus = $('#ddlStatus');
        domElements.btnSearch = $('#btnSearch');
        domElements.btnAddRobot = $('#btnAddRobot');
        domElements.divRobotList = $('#divRobotList');
        domElements.divButtons = $('#divButtons');
        domElements.btnActivate = $('#btnActivate');
        domElements.btnDeactivate = $('#btnDeactivate');
        domElements.divRobotForm = $('#divRobotForm').clone();
        domElements.divPopup = $('#dialog-popup');

        $('#divRobotForm').empty();

        if (hasEditRight) {
            domElements.btnActivateValue = domElements.btnActivate.attr('data-value');
            domElements.btnDeactivateValue = domElements.btnDeactivate.attr('data-value');
        }
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', domElements.divPopup);
        popupElements.divRobotForm = $('#divRobotForm', domElements.divPopup);
        popupElements.hdnCustID = $('#hdnCustID', domElements.divPopup);
        popupElements.txtNewUsername = $('#txtNewUsername', domElements.divPopup);
        popupElements.divInstruction = $('#divInstruction', domElements.divPopup);
        popupElements.txtRemark = $('#txtRemark', domElements.divPopup);
        popupElements.btnSave = $('#btnSave', domElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', domElements.divPopup);
    };

    // Initialize fields and handle events.
    var initFields = function () {
        parameters.PageSize = CMS.pageSize;
        parameters.PageIndex = 1;

        domElements.txtUsername.keyup(function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });
        domElements.txtUsername.focus();

        domElements.btnSearch.click(function () {
            setTimeout(function () {
                var errors = Validator.countErrors(domElements.divFilterForm);

                clearStatusColError();

                if (errors === 0) {
                    parameters.Username = $.trim(domElements.txtUsername.val());
                    parameters.Status = domElements.ddlStatus.val();
                    parameters.PageIndex = 1;

                    domElements.txtUsername.val(parameters.Username);

                    getRobotList();
                }
            }, 70);
        });

        domElements.btnAddRobot.on('click', function () {
            displayPopupForm();
        });

        domElements.btnActivate.click(function () {
            var selectedAccounts = getCheckedAccounts(false);

            if (selectedAccounts.length > 0) {
                var updatingParameters = {
                    CustIDs: selectedAccounts.join(','),
                    IsActive: true,
                    SearchedUsername: parameters.Username,
                    Status: parameters.Status,
                    PageIndex: parameters.PageIndex,
                    PageSize: parameters.PageSize
                };

                CMS.showConfirmMessage(messages.ActivateRobotsConfirmationMessage.format(selectedAccounts.length), function () {
                    updateMemberStatus(updatingParameters);
                });
            }
        });

        domElements.btnDeactivate.click(function () {
            var selectedAccounts = getCheckedAccounts(true);

            if (selectedAccounts.length > 0) {
                var updatingParameters = {
                    CustIDs: selectedAccounts.join(','),
                    IsActive: false,
                    SearchedUsername: parameters.Username,
                    Status: parameters.Status,
                    PageIndex: parameters.PageIndex,
                    PageSize: parameters.PageSize
                };

                CMS.showConfirmMessage(messages.DeactivateRobotsConfirmationMessage.format(selectedAccounts.length), function () {
                    updateMemberStatus(updatingParameters);
                });
            }
        });
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventHandlers = function (isEdit) {
        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divRobotForm, 'w')) {
                var custID = popupElements.hdnCustID.val();
                var usernames = $.trim(popupElements.txtNewUsername.val());
                var remark = popupElements.txtRemark.val();
                var params = {
                    Remark: remark.encode(),
                    SearchedUsername: parameters.Username,
                    Status: parameters.Status,
                    PageIndex: parameters.PageIndex,
                    PageSize: parameters.PageSize
                };

                if (isEdit) {
                    if (editBackupData.Remark !== remark) {
                        params.CustID = custID;
                        updateRobotInfo(params);
                    } else {
                        Validator.showError(popupElements.txtRemark, messages.ThereIsNoChange, 'w');
                    }
                } else {
                    params.Usernames = validateNewUsername(usernames);

                    if (params.Usernames) {
                        popupElements.txtNewUsername.val(params.Usernames);
                        addNewRobot(params);
                    } else {
                        Validator.showError(popupElements.txtNewUsername, messages.InvalidValueError, 'w');
                    }
                }
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
    };

    // Display popup form.
    var displayPopupForm = function (robot) {
        var form = domElements.divRobotForm.clone();
        var isEdit = robot ? true : false;
        var title = isEdit ? labels.EditRobot : labels.AddRobot;

        CMS.closeMsgBox();
        CMS.modal(title, form);
        cachePopupElements();

        if (isEdit) {
            editBackupData = robot;
            popupElements.hdnCustID.val(robot.CustID);
            popupElements.txtNewUsername.val(robot.Username);
            popupElements.txtRemark.val(robot.Remark).focus();
            popupElements.txtNewUsername.prop('disabled', true);
            popupElements.divInstruction.addClass('display-none');
        }

        bindPopupEventHandlers(isEdit);
    };

    // Disable popup form.
    var disablePopupForm = function () {
        popupElements.txtNewUsername.prop('disabled', true);
        popupElements.txtRemark.prop('disabled', true);
        popupElements.btnSave.prop('disabled', true);
        popupElements.btnCancel.prop('disabled', true);
    };

    // Validate username in the adding new robot popup.
    var validateNewUsername = function (usernameStr) {
        var usernames = CMS.htmlDecode(usernameStr).split(/[\r\n\s,]+/);
        var isValid = true;
        var result = '';

        $.each(usernames, function (index, username) {
            if (isValid) {
                username = $.trim(username);

                if (username !== '') {
                    result += username;

                    if (index !== usernames.length - 1) {
                        result += ',';
                    }
                } else if (index !== usernames.length - 1) {
                    isValid = false;
                }
            }
        });

        if (isValid) {
            return result;
        } else {
            return null;
        }
    };

    // Reset counting values of buttons.
    var resetButtonValues = function () {
        domElements.btnActivate.attr('value', domElements.btnActivateValue.format(''));
        domElements.btnDeactivate.attr('value', domElements.btnDeactivateValue.format(''));
    };

    // Update counters in the ACTIVATE/DEACTIVATE buttons.
    var updateButtonsCounters = function ($obj, checkboxes, countAll) {
        var updatedCounter = '';
        var counter;

        if (countAll) {
            // Update ACTIVATE button.
            counter = checkboxes.filter('[data-active="false"]:checked').length;
            updatedCounter = counter > 0 ? counter + ' ' : '';
            domElements.btnActivate.attr('value', domElements.btnActivateValue.format(updatedCounter));

            // Update DEACTIVATE button.
            counter = checkboxes.filter('[data-active="true"]:checked').length;
            updatedCounter = counter > 0 ? counter + ' ' : '';
            domElements.btnDeactivate.attr('value', domElements.btnDeactivateValue.format(updatedCounter));
        } else {
            if ($obj.attr('data-active') === 'false') {
                counter = checkboxes.filter('[data-active="false"]:checked').length;

                updatedCounter = counter > 0 ? counter + ' ' : '';
                domElements.btnActivate.attr('value', domElements.btnActivateValue.format(updatedCounter));
            } else {
                counter = checkboxes.filter('[data-active="true"]:checked').length;

                updatedCounter = counter > 0 ? counter + ' ' : '';
                domElements.btnDeactivate.attr('value', domElements.btnDeactivateValue.format(updatedCounter));
            }
        }
    };

    // Render robot list.
    var renderRobotList = function (data) {
        var statusHeaderFormat = '<div class="checkbox-status status-header"><input type="checkbox" id="chkStatus"/></div>' +
                                 '<label for="chkStatus" class="lbl-for-chkStatus">{0}</label>';
        var statusFormat = '<div class="checkbox-status">' +
                               '<input type="checkbox" id="{0}" data-active="{1}" data-chk-group="chkStatus"/>' +
                           '</div>' +
                           '<label for="{0}" class="lbl-for-chkStatus {2}">{3}</label>';
        var plainStatusFormat = '<span class="{0}">{1}</span>';
        var remarkFormat = '<span class="ico-remark" title="{0}"></span>';
        var editFormat = '<span class="ico-edit" data-cust-id="{0}" title="{1}"></span>';
        var statusCol = hasEditRight ? statusHeaderFormat.format(labels.Status) : labels.Status;
        var gridOptions = {
            columns: [{ HeaderText: labels.Order, Width: 50, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Username, Width: 140, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: statusCol, Width: 80, Name: 'ColStatus', HeaderAlign: hasEditRight ? 'left' : 'center',
                          CellAlign: hasEditRight ? 'left' : 'center', CssClass: hasEditRight ? 'chk-column' : ''
                      },
                      { HeaderText: labels.Currency, Width: 60, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Site, Width: 100, Name: 'ColSite', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Super, Width: 120, Name: 'ColSuper', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Master, Width: 120, Name: 'ColMaster', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Agent, Width: 120, Name: 'ColAgent', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.AddedBy, Width: 75, Name: 'ColAddedBy', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.AddedDate, Width: 70, Name: 'ColAddedDate', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.ModifiedBy, Width: 75, Name: 'ColModifiedBy', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.ModifiedDate, Width: 80, Name: 'ColModifiedDate', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Remark, Width: 50, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'center'}],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex - 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 200
        };
        var bodyData = [];
        var row;

        if (hasEditRight) {
            gridOptions.columns.push({ HeaderText: labels.Edit, Width: 30, Name: 'ColEdit', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data.RobotAccounts && data.RobotAccounts.length > 0) {
            $.each(data.RobotAccounts, function (index, item) {
                var status = item.IsActive ? labels.Active : labels.Inactive;
                var statusClass = item.IsActive ? 'active' : '';

                statusCol = hasEditRight ?
                            statusFormat.format(item.CustID, item.IsActive, statusClass, status) :
                            plainStatusFormat.format(statusClass, status);
                row = {
                    Columns: [{ Name: 'ColOrder', Value: index },
                              { Name: 'ColUsername', Value: item.Username, CssClass: 'username-column' },
                              { Name: 'ColStatus', Value: statusCol, CssClass: 'chk-column' },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColSite', Value: item.Site },
                              { Name: 'ColSuper', Value: item.Super },
                              { Name: 'ColMaster', Value: item.Master },
                              { Name: 'ColAgent', Value: item.Agent },
                              { Name: 'ColAddedBy', Value: item.AddedBy },
                              { Name: 'ColAddedDate', Value: item.AddedDate },
                              { Name: 'ColModifiedBy', Value: item.ModifiedBy },
                              { Name: 'ColModifiedDate', Value: item.ModifiedDate },
                              { Name: 'ColRemark', Value: remarkFormat.format(item.Remark || ' ')}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColEdit', Value: editFormat.format(item.CustID, labels.Edit) });
                }

                bodyData.push(row);
            });
        }

        clearStatusColError();
        gridOptions.pagerOption.totalItem = data.TotalItems;
        gridOptions.bodyRows = bodyData;
        domElements.divRobotList.fanexGrid(gridOptions);

        if (hasEditRight) {
            showHideButtons(bodyData.length > 0);
        }

        if (bodyData.length > 0) {
            handleGridEvents();
        }
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        parameters.PageSize = itemsPerPage;
        parameters.PageIndex = pageIndex + 1;
        getRobotList();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        var chkStatus = domElements.divRobotList.find('#chkStatus');
        var checkboxes = domElements.divRobotList.find('input[type=checkbox]');
        var robotCheckboxes = checkboxes.filter('[data-active]');
        var bindMemberCounterHandler = function () {
            robotCheckboxes.on('change', function () {
                updateButtonsCounters($(this), checkboxes);
            }).on('update', function () {
                updateButtonsCounters($(this), checkboxes);
            });
        };
        var unbindMemberCounterHandler = function () {
            robotCheckboxes.off('change').off('update');
        };

        checkboxes.each(function () {
            $(this).checkboxGroup();
        });

        chkStatus.on('updatingGroup', function () {
            unbindMemberCounterHandler();
        }).on('updatedGroup', function () {
            updateButtonsCounters($(this), checkboxes, true);
            bindMemberCounterHandler();
        });

        // Count number of checked/unchecked checkboxes and update to the Open/Close button.
        bindMemberCounterHandler();

        domElements.divRobotList.find('span.ico-remark').each(function () {
            $(this).tipsy({ opacity: 0.9, gravity: 'ne', decodeTitle: false });
        });

        domElements.divRobotList.find('span.ico-edit').click(function () {
            var cell = $(this);
            var row = cell.closest('tr');
            var custID = cell.attr('data-cust-id');
            var username = row.find('div.username-column').text();
            var remark = row.find('span.ico-remark').attr('original-title');

            displayPopupForm({
                CustID: custID,
                Username: username,
                Remark: remark.decode()
            });

        });
    };

    // Show/hide ACTIVATE/DEACTIVATE buttons.
    var showHideButtons = function (hasData) {
        var status = domElements.ddlStatus.val();

        resetButtonValues();
        domElements.divButtons.addClass('display-none');
        domElements.btnActivate.hide();
        domElements.btnDeactivate.hide();

        if (hasData) {
            if (status === 'true') {
                domElements.btnDeactivate.show();
            } else if (status === 'false') {
                domElements.btnActivate.show();
            } else {
                domElements.btnActivate.show();
                domElements.btnDeactivate.show();
            }
        }

        domElements.divButtons.removeClass('display-none');
    };

    // Get and validate status column.
    var getCheckedAccounts = function (isActive) {
        var checkedRows = domElements.divRobotList.find('input[type="checkbox"][data-active="' + isActive + '"]:checked');
        var chkStatus = domElements.divRobotList.find('#chkStatus');
        var selectedAccounts = [];
        var msg = messages.RequiredStatusCol.format(isActive ? labels.Active : labels.Inactive);

        clearStatusColError();

        if (checkedRows.length === 0) {
            chkStatus.attr('title', msg).tipsy({
                trigger: 'manual',
                gravity: 'sw',
                classStyle: 'error',
                opacity: 0.97
            }).tipsy('show');
        } else {
            $.each(checkedRows, function (index, robot) {
                var $robot = $(robot);

                selectedAccounts.push($robot.attr('id'));
            });
        }

        return selectedAccounts;
    };

    // Hide error tooltip at Status column.
    var clearStatusColError = function () {
        var chkStatus = domElements.divRobotList.find('#chkStatus');

        chkStatus.tipsy('hide').removeAttr('original-title');
    };

    // Get robot list.
    var getRobotList = function () {
        CMS.closeMsgBox();
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetRobotList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    Validator.showError(domElements.txtUsername, result.ErrorMsg, 'n');
                } else {
                    renderRobotList(result);
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

    // Update robots' status.
    var updateMemberStatus = function (updatingParameters) {
        updatingParameters.SearchedUsername = parameters.Username;
        updatingParameters.Status = parameters.Status;
        updatingParameters.PageSize = parameters.PageSize;
        updatingParameters.PageIndex = parameters.PageIndex;

        CMS.closeMsgBox();
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateRobotStatus,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updatingParameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    CMS.showErrorMessage(result.ErrorMsg);
                } else {
                    CMS.showSuccessMessage(messages.UpdateRobotStatusSuccessfully);
                    renderRobotList(result);
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

    // Add new robot.
    var addNewRobot = function (addNewRobotParameters) {
        $.ajax({
            type: 'POST',
            url: urls.AddRobotAccount,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(addNewRobotParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineSuccessMsg(messages.AddRobotsSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disablePopupForm();
                    renderRobotList(result);
                } else if (result.ErrorCode < 0) {
                    CMS.showInlineInfoMsg(result.ErrorMsg.replace(/,$/, ''), popupElements.divErrorSuccessMsg, false, 15000);
                } else {
                    CMS.showInlineErrorMsg(result.ErrorMsg.replace(/,$/, ''), popupElements.divErrorSuccessMsg);
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

    // Update robot information.
    var updateRobotInfo = function (updateRobotParameters) {
        $.ajax({
            type: 'POST',
            url: urls.EditRobotInfo,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updateRobotParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineSuccessMsg(messages.UpdateRobotInfoSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disablePopupForm();
                    renderRobotList(result);
                } else {
                    CMS.showInlineErrorMsg(result.ErrorMsg, popupElements.divErrorSuccessMsg);
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
        initFields();
    };
};