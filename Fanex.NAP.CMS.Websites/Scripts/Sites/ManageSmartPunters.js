/* File Created: June 4, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages, hasEditRight */

'use strict';

$(document).ready(function () {
    var manageSmartPunters = new Fanex.CMS.ManageSmartPunters();

    manageSmartPunters.InitPage();
});

Namespace.Register("Fanex.CMS.ManageSmartPunters");
Fanex.CMS.ManageSmartPunters = function () {
    var $this = this;
    var urls = {
        GetSmartPunterList: CMS.resolveRootUrl('ManageSmartPunters/GetSmartPunterList'),
        AddSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/AddSmartPunter'),
        UpdateSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/UpdateSmartPunter'),
        RemoveSmartPunter: CMS.resolveRootUrl('ManageSmartPunters/RemoveSmartPunter')
    };
    var parameters = {
        Username: null,
        SiteID: null,
        Status: null,
        PageIndex: null,
        PageSize: null
    };
    var domElements = {
        spnNote: null,
        divFilterForm: null,
        txtUsername: null,
        ddlSite: null,
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
        ddlNewSite: null,
        txtRemark: null,
        btnSave: null,
        btnCancel: null
    };
    var editBackupData = null;

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.spnNote = $('#spnNote');
        domElements.divFilterForm = $('#divFilterForm');
        domElements.txtUsername = $('#txtUsername');
        domElements.ddlSite = $('#ddlSite');
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
        popupElements.ddlNewSite = $('#ddlNewSite', domElements.divPopup);
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
                    parameters.Site = domElements.ddlSite.val();
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

        domElements.spnNote.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventHandlers = function (isEdit) {
        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divSmartPunterForm, 'w')) {
                var custID = popupElements.hdnCustID.val();
                var username = $.trim(popupElements.txtNewUsername.val());
                var siteID = parseInt(popupElements.ddlNewSite.val());
                var remark = popupElements.txtRemark.val();
                var params = {
                    SiteID: siteID,
                    Remark: remark,
                    SearchedUsername: parameters.Username,
                    SearchedSiteID: parameters.SiteID,
                    Status: parameters.Status,
                    PageIndex: parameters.PageIndex,
                    PageSize: parameters.PageSize
                };

                if (isEdit) {

                    if (editBackupData.SiteID !== siteID || editBackupData.Remark !== remark) {
                        params.CustID = custID;
                        disablePopupForm();
                        //@@TODO: Update
                    } else {
                        if (editBackupData.SiteID === siteID) {
                            Validator.showError(popupElements.ddlNewSite, messages.ThereIsNoChange, 'w');
                        } else {
                            Validator.showError(popupElements.txtRemark, messages.ThereIsNoChange, 'w');
                        }
                    }
                } else {
                    popupElements.txtNewUsername.val(username);
                    params.Username = username;
                    disablePopupForm();
                    // @@TODO: Add new.
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
            popupElements.ddlNewSite.val(punter.Site);
            popupElements.txtRemark.val(punter.Remark).focus();
            popupElements.txtNewUsername.prop('disabled', true);
        }

        bindPopupEventHandlers(isEdit);
    };

    // Disable popup form.
    var disablePopupForm = function () {
        popupElements.txtNewUsername.prop('disabled', true);
        popupElements.ddlNewSite.combobox('disable');
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
                              { Name: 'ColUsername', Value: item.Username },
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
            var custID = cell.attr('data-cust-id');
            var username = row.find('div.username-column').text();
            var remark = row.find('span.ico-remark').attr('original-title');

            displayPopupForm({
                CustID: custID,
                Username: username,
                Remark: remark
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

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};