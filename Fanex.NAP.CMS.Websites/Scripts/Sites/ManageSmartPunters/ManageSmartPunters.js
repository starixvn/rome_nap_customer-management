/* File Created: June 4, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages, hasEditRight */

'use strict';

Namespace.Register("Fanex.CMS.ManageSmartPunters.ManageSmartPunters");
Fanex.CMS.ManageSmartPunters.ManageSmartPunters = function () {
    var $this = this;
    var urls = {
        GetSmartPunterList: CMS.resolveRootUrl('ManageSmartPunters/GetSmartPunterList'),
        AddSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/AddSmartPunter'),
        UpdateSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/UpdateSmartPunter'),
        RemoveSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/RemoveSmartPunter')
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
        btnAddSmartPunter: null,
        divSmartPunterList: null,
        divSmartPunterForm: null,
        divPopup: null
    };
    var popupElements = {
        divErrorSuccessMsg: null,
        divSmartPunterForm: null,
        hdnCustID: null,
        txtNewUsername: null,
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
        domElements.btnAddSmartPunter = $('#btnAddSmartPunter');
        domElements.divSmartPunterList = $('#divSmartPunterList');
        domElements.divSmartPunterForm = $('#divSmartPunterForm').clone();
        domElements.divPopup = $('#dialog-popup');

        $('#divSmartPunterForm').empty();
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', domElements.divPopup);
        popupElements.divSmartPunterForm = $('#divSmartPunterForm', domElements.divPopup);
        popupElements.hdnCustID = $('#hdnCustID', domElements.divPopup);
        popupElements.txtNewUsername = $('#txtNewUsername', domElements.divPopup);
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

                if (errors === 0) {
                    parameters.Username = $.trim(domElements.txtUsername.val());
                    parameters.Status = domElements.ddlStatus.val();
                    parameters.PageIndex = 1;

                    domElements.txtUsername.val(parameters.Username);

                    getSmartPunterList();
                }
            }, 70);
        });

        domElements.btnAddSmartPunter.on('click', function () {
            displayPopupForm();
        });
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventHandlers = function (isEdit) {
        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divSmartPunterForm, 'w')) {
                var custID = popupElements.hdnCustID.val();
                var username = $.trim(popupElements.txtNewUsername.val());
                var remark = popupElements.txtRemark.val();
                var params = {
                    Remark: remark,
                    SearchedUsername: parameters.Username,
                    Status: parameters.Status,
                    PageIndex: parameters.PageIndex,
                    PageSize: parameters.PageSize
                };

                if (isEdit) {

                    if (editBackupData.Remark !== remark) {
                        params.CustID = custID;
                        updateSmartPunter(params);
                    } else {
                        Validator.showError(popupElements.txtRemark, messages.ThereIsNoChange, 'w');
                    }
                } else {
                    popupElements.txtNewUsername.val(username);
                    params.Username = username;
                    addNewSmartPunter(params);
                }
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
    };

    // Display popup form.
    var displayPopupForm = function (punter) {
        var form = domElements.divSmartPunterForm.clone();
        var isEdit = punter ? true : false;
        var title = isEdit ? labels.EditSmartPunter : labels.AddSmartPunter;

        CMS.closeMsgBox();
        CMS.modal(title, form);
        cachePopupElements();

        if (isEdit) {
            editBackupData = punter;
            popupElements.hdnCustID.val(punter.CustID);
            popupElements.txtNewUsername.val(punter.Username);
            popupElements.txtRemark.val(punter.Remark).focus();
            popupElements.txtNewUsername.prop('disabled', true);
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

    // Render smart punter list.
    var renderSmartPunterList = function (data) {
        var statusFormat = '<span class="{0}">{1}</span>';
        var remarkFormat = '<span class="ico-remark" title="{0}"></span>';
        var editFormat = '<span class="ico-edit" data-cust-id="{0}" title="{1}"></span>';
        var removeFormat = '<span class="ico-delete" data-cust-id="{0}" title="{1}"></span>';

        var gridOptions = {
            columns: [{ HeaderText: labels.Order, Width: 50, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Username, Width: 135, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Nickname, Width: 135, Name: 'ColNickname', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Status, Width: 65, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 60, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Site, Width: 100, Name: 'ColSite', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Super, Width: 120, Name: 'ColSuper', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Master, Width: 120, Name: 'ColMaster', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.Agent, Width: 120, Name: 'ColAgent', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.AddedBy, Width: 75, Name: 'ColAddedBy', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                      { HeaderText: labels.AddedDate, Width: 70, Name: 'ColAddedDate', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Remark, Width: 45, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'center'}],
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
            gridOptions.columns.push({ HeaderText: labels.Remove, Width: 50, Name: 'ColRemove', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data.SmartPunters && data.SmartPunters.length > 0) {
            $.each(data.SmartPunters, function (index, item) {
                var statusClass = item.Status.toLowerCase();


                row = {
                    Columns: [{ Name: 'ColOrder', Value: index },
                              { Name: 'ColUsername', Value: item.Username, CssClass: 'username-column' },
                              { Name: 'ColNickname', Value: item.Nickname },
                              { Name: 'ColStatus', Value: statusFormat.format(statusClass, item.Status) },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColSite', Value: item.Site },
                              { Name: 'ColSuper', Value: item.Super },
                              { Name: 'ColMaster', Value: item.Master },
                              { Name: 'ColAgent', Value: item.Agent },
                              { Name: 'ColAddedBy', Value: item.AddedBy },
                              { Name: 'ColAddedDate', Value: item.AddedDate },
                              { Name: 'ColRemark', Value: remarkFormat.format(item.Remark || ' ')}]
                };

                if (hasEditRight) {
                    row.Columns.push({ Name: 'ColEdit', Value: editFormat.format(item.CustID, labels.Edit) });
                    row.Columns.push({ Name: 'ColRemove', Value: removeFormat.format(item.CustID, labels.Remove) });
                }

                bodyData.push(row);
            });
        }

        gridOptions.pagerOption.totalItem = data.TotalItems;
        gridOptions.bodyRows = bodyData;
        domElements.divSmartPunterList.fanexGrid(gridOptions);

        if (bodyData.length > 0) {
            handleGridEvents();
        }
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        parameters.PageSize = itemsPerPage;
        parameters.PageIndex = pageIndex + 1;
        getSmartPunterList();
    };

    // Handle grid's events.
    var handleGridEvents = function () {
        domElements.divSmartPunterList.find('span.ico-remark').each(function () {
            $(this).tipsy({ opacity: 0.9, gravity: 'ne', decodeTitle: false });
        });

        domElements.divSmartPunterList.find('span.ico-edit').click(function () {
            var cell = $(this);
            var row = cell.closest('tr');
            var usernameCell = row.find('div.username-column');
            var custID = cell.attr('data-cust-id');
            var remark = row.find('span.ico-remark').attr('original-title');
            var username = usernameCell.attr('title');

            username = username || usernameCell.text();

            displayPopupForm({
                CustID: custID,
                Username: username,
                Remark: remark
            });
        });

        domElements.divSmartPunterList.find('span.ico-delete').click(function () {
            var cell = $(this);
            var custID = cell.attr('data-cust-id');

            CMS.showConfirmMessage(messages.RemoveSmartPunterConfirmationMessage, function () {
                removeSmartPunter(custID);
            });
        });
    };

    // Get smart punter list.
    var getSmartPunterList = function () {
        CMS.closeMsgBox();
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetSmartPunterList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    Validator.showError(domElements.txtUsername, result.ErrorMsg, 'n');
                } else {
                    renderSmartPunterList(result);
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

    // Add new smart punter.
    var addNewSmartPunter = function (addNewSmartPunterParameters) {
        $.ajax({
            type: 'POST',
            url: urls.AddSmartPunter,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(addNewSmartPunterParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineSuccessMsg(messages.AddSmartPunterSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disablePopupForm();
                    renderSmartPunterList(result);
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

    // Update smart punter information.
    var updateSmartPunter = function (updateSmartPunterParameters) {
        $.ajax({
            type: 'POST',
            url: urls.UpdateSmartPunter,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updateSmartPunterParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showInlineSuccessMsg(messages.UpdateSmartPunterSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disablePopupForm();
                    renderSmartPunterList(result);
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

    // Delete smart punter information.
    var removeSmartPunter = function (custID) {
        var removeParameters = {
            CustID: custID,
            SearchedUsername: parameters.Username,
            Status: parameters.Status,
            PageIndex: parameters.PageIndex,
            PageSize: parameters.PageSize
        };

        $.ajax({
            type: 'POST',
            url: urls.RemoveSmartPunter,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(removeParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    CMS.showSuccessMessage(messages.RemoveSmartPunterSuccessfully);
                    renderSmartPunterList(result);
                } else {
                    CMS.showErrorMessage(result.ErrorMsg);
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

    $this.ResetForm = function () {
        domElements.txtUsername.val('');
        domElements.ddlStatus.val(0).combobox('refresh');
    };

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};