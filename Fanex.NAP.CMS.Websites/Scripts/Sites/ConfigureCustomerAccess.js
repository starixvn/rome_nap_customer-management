/* File Created: March 6, 2014 */
/* global Namespace, Fanex, Validator, CMS, Layout, labels, messages, hasEditAccessRight, minDate */
/* jshint strict: false */
// 'use strict';

$(document).ready(function () {
    var manager = new Fanex.NAP.CMS.ConfigureCustomerAccess();

    manager.InitPage();
});

Namespace.Register('Fanex.NAP.CMS.ConfigureCustomerAccess');
Fanex.NAP.CMS.ConfigureCustomerAccess = function () {
    var $this = this;
    var urls = {
        GetRequestList: CMS.resolveRootUrl('ConfigureCustomerAccess/GetRequestList'),
        InsertRequest: CMS.resolveRootUrl('ConfigureCustomerAccess/InsertRequest'),
        UpdateRequest: CMS.resolveRootUrl('ConfigureCustomerAccess/UpdateRequest'),
        DeleteRequest: CMS.resolveRootUrl('ConfigureCustomerAccess/DeleteRequest')
    };
    var cachedData = null;
    var searchParameters = {
        Username: null,
        PageIndex: null,
        PageSize: null
    };
    var mainDOMElements = {
        txtSearchUserName: null,
        btnSearch: null,
        btnAddRequest: null,
        divMemberList: null,
        divRequestForm: null,
        divPopup: null
    };
    var popupElements = {
        divErrorSuccessMsg: null,
        divForm: null,
        hdnRequestId: null,
        hdnCustId: null,
        txtUsername: null,
        rdbRestriction: null,
        ddlCountry: null,
        txtIP: null,
        dpkExpiredDate: null,
        chkUnlimited: null,
        txtRemark: null,
        btnSave: null,
        btnCancel: null
    };

    // Cache DOM elements
    var cacheMainDOMElements = function () {
        mainDOMElements.txtSearchUserName = $('#txtSearchUserName');
        mainDOMElements.btnSearch = $('#btnSearch');
        mainDOMElements.btnAddRequest = $('#btnAddRequest');
        mainDOMElements.divMemberList = $('#divMemberList');
        mainDOMElements.divRequestForm = $('#divRequestForm').clone();
        mainDOMElements.divPopup = $('#dialog-popup');

        // Remove the #divRequestForm element since it was cached. It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $('#divPopupContent').empty();
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', mainDOMElements.divPopup);
        popupElements.divForm = $('#divRequestForm', mainDOMElements.divPopup);
        popupElements.hdnRequestId = $('#hdnRequestId', mainDOMElements.divPopup);
        popupElements.hdnCustId = $('#hdnCustId', mainDOMElements.divPopup);
        popupElements.hdnExpiredDate = $('#hdnExpiredDate', mainDOMElements.divPopup);
        popupElements.txtUsername = $('#txtUsername', mainDOMElements.divPopup);
        popupElements.rdbRestriction = $('[name=Restriction]', mainDOMElements.divPopup);
        popupElements.ddlCountry = $('#ddlCountry', mainDOMElements.divPopup);
        popupElements.txtIP = $('#txtIP', mainDOMElements.divPopup);
        popupElements.dpkExpiredDate = $('#dpkExpiredDate', mainDOMElements.divPopup);
        popupElements.chkUnlimited = $('#chkUnlimited', mainDOMElements.divPopup);
        popupElements.txtRemark = $('#txtRemark', mainDOMElements.divPopup);
        popupElements.btnSave = $('#btnSave', mainDOMElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', mainDOMElements.divPopup);
    };

    // Initialize fields and handle events.
    var initFields = function () {
        searchParameters.PageSize = CMS.pageSize;
        searchParameters.PageIndex = 0;

        mainDOMElements.txtSearchUserName.keyup(function (event) {
            if (event.keyCode === 13) {
                mainDOMElements.btnSearch.click();
            }
        });
        mainDOMElements.txtSearchUserName.focus();

        mainDOMElements.btnSearch.click(function () {
            searchParameters.PageIndex = 0;
            searchParameters.Username = mainDOMElements.txtSearchUserName.val();
            cachedData = null;
            getRequestList();
        });

        mainDOMElements.btnAddRequest.click(function () {
            displayRequestForm();
        });
    };

    // Bind event handlers to elements on the popup.
    var bindPopupEventhandlers = function (isNewRequest) {
        popupElements.rdbRestriction.change(function () {
            var selectedValue = $(this).val();

            if (selectedValue === 'Country') {
                popupElements.ddlCountry.combobox('enable');
                popupElements.txtIP.prop('disabled', true);
                Validator.clearError(popupElements.txtIP); // Clear error when the field is disabled.
            } else {
                popupElements.ddlCountry.combobox('disable');
                popupElements.txtIP.prop('disabled', false);
            }
        });

        popupElements.chkUnlimited.change(function () {
            if (popupElements.chkUnlimited.is(':checked')) {
                popupElements.divForm.find('#txtExpiredDate').prop('disabled', true);
            } else {
                popupElements.divForm.find('#txtExpiredDate').prop('disabled', false);
            }
        });

        popupElements.btnSave.click(function () {
            if (Validator.validateForm(popupElements.divForm, 'w')) {
                var parameters = {
                    RequestId: popupElements.hdnRequestId.val(),
                    CustId: popupElements.hdnCustId.val(),
                    Username: popupElements.txtUsername.val(),
                    CountryCode: '',
                    IPRequest: '',
                    Remark: popupElements.txtRemark.val().encode()
                };

                if ($(popupElements.rdbRestriction[0]).is(':checked')) {
                    parameters.CountryCode = popupElements.ddlCountry.val();
                } else {
                    parameters.IPRequest = popupElements.txtIP.val();
                }

                if (!popupElements.chkUnlimited.is(':checked')) {
                    parameters.ExpiryDate = popupElements.dpkExpiredDate.nexDatePicker('getSingleDate');
                }

                if (isNewRequest) {
                    addNewRequest(parameters);
                } else {
                    updateRequest(parameters);
                }
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });

        // Initialize fields.
        popupElements.divForm.find('select').each(function () {
            $(this).addClass('combobox-autocomplete').combobox();
        });
        popupElements.divForm.find('input[type=text]').each(function () {
            $(this).clearfield();
        });

        var expiredDate = Date.parse(popupElements.hdnExpiredDate.val());
        Layout.createDatepicker(
            popupElements.dpkExpiredDate,
            'txtExpiredDate',
            labels.expiredDate,
            expiredDate,
            new Date(minDate) > new Date(expiredDate) ? expiredDate : minDate,
            null);

        popupElements.divForm.find('#txtExpiredDate').addClass('width-120');
        popupElements.txtIP.ipAddress();

        if (!isNewRequest) {
            popupElements.txtUsername.prop('disabled', true);
            popupElements.chkUnlimited.change();

            if ($(popupElements.rdbRestriction[0]).is(':checked')) {
                $(popupElements.rdbRestriction[0]).change();
            } else {
                $(popupElements.rdbRestriction[1]).change();
            }
        } else {
            $(popupElements.rdbRestriction[0]).prop('checked', true).change();
        }
    };

    // Get data
    var getRequestList = function () {
        $.ajax({
            type: 'POST',
            url: urls.GetRequestList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(searchParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                renderTable(result.RequestList, result.TotalItems);
            },
            error: function (xhr, status, err) {
                CMS.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Render data table for agent lisy.
    var renderTable = function (data, totalItems) {
        var bodyData = [];
        var columns = [];
        var gridOptions = {
            showDistinctRow: false,
            showFooter: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: searchParameters.PageSize,
                currentPage: searchParameters.PageIndex,
                totalItem: totalItems,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 150
        };

        mainDOMElements.divMemberList.addClass('display-none');

        columns = [{ HeaderText: labels.id, Width: 70, Name: 'ColID', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.username, Width: 130, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.expiredDate, Width: 80, Name: 'ColExpiredDate', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.country, Width: 230, Name: 'ColCountry', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.ip, Width: 100, Name: 'ColIP', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.remark, Width: 45, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.createdBy, Width: 130, Name: 'ColCreatedBy', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.createdDate, Width: 80, Name: 'ColCreatedDate', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.lastModifiedBy, Width: 130, Name: 'ColLastModifiedBy', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.lastModifiedDate, Width: 110, Name: 'ColLastModifiedDate', HeaderAlign: 'center', CellAlign: 'center'}];

        if (hasEditAccessRight) {
            columns.push({ HeaderText: labels.edit, Width: 30, Name: 'ColEdit', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: labels.del, Width: 40, Name: 'ColDelete', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            var row = null;
            var expiredDateVal = null;
            var remarkCol = '<span class="ico-remark" title="{0}"></span>';
            var editCol = '<span class="ico-edit" data-item-index="{0}" title="{1}"></span>';
            var delCol = '<span class="ico-delete" data-request-id="{0}" title="{1}"></span>';

            cachedData = data;
            $.each(data, function (index, item) {
                expiredDateVal = item.ExpiredDate || labels.unlimited;
                row = {
                    Columns: [{ Name: 'ColID', Value: item.RequestId },
                              { Name: 'ColUsername', Value: item.Username },
                              { Name: 'ColExpiredDate', Value: expiredDateVal },
                              { Name: 'ColCountry', Value: item.Country },
                              { Name: 'ColIP', Value: item.IP },
                              { Name: 'ColRemark', Value: remarkCol.format(item.Remark) },
                              { Name: 'ColCreatedBy', Value: item.CreatedBy },
                              { Name: 'ColCreatedDate', Value: item.CreatedDate },
                              { Name: 'ColLastModifiedBy', Value: item.LastModifiedBy },
                              { Name: 'ColLastModifiedDate', Value: item.LastModifiedDate}]
                };

                if (hasEditAccessRight) {
                    row.Columns.push({ Name: 'ColEdit', Value: editCol.format(index, labels.edit) });
                    row.Columns.push({ Name: 'ColDelete', Value: delCol.format(item.RequestId, labels.del) });
                }

                if (item.IsExpired) {
                    row.CssClass = 'highlight';
                }

                bodyData.push(row);
            });
        }

        gridOptions.columns = columns;
        gridOptions.bodyRows = bodyData;
        mainDOMElements.divMemberList.removeClass('display-none').fanexGrid(gridOptions);
        bindGridEventhandlers();
    };

    // Bind grid event handlers
    var bindGridEventhandlers = function () {
        mainDOMElements.divMemberList.find('span.ico-remark').each(function () {
            $(this).tipsy({ opacity: 0.9 });
        });

        mainDOMElements.divMemberList.find('span.ico-edit').click(function () {
            var itemIndex = parseInt($(this).attr('data-item-index'));
            var request = cachedData[itemIndex];

            displayRequestForm(request);

        });

        mainDOMElements.divMemberList.find('span.ico-delete').click(function () {
            var requestId = parseInt($(this).attr('data-request-id'));

            deleteRequest(requestId);
        });
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        searchParameters.PageSize = itemsPerPage;
        searchParameters.PageIndex = pageIndex;
        getRequestList();
    };

    // Display request form.
    var displayRequestForm = function (request) {
        var title = labels.addNewRequest;
        var isNewRequest = true;
        var form = mainDOMElements.divRequestForm.clone();

        if (request) {
            title = labels.editRequest;
            isNewRequest = false;
        }

        CMS.modal(title, form);
        cachePopupElements();

        if (!isNewRequest) {
            fillRequestInfo(request);
        }

        bindPopupEventhandlers(isNewRequest);
    };

    // Disable request form.
    var disableRequestForm = function () {
        popupElements.txtUsername.prop('disabled', true);
        popupElements.rdbRestriction.prop('disabled', true);
        popupElements.ddlCountry.combobox('disable');
        popupElements.txtIP.prop('disabled', true);
        popupElements.divForm.find('#txtExpiredDate').prop('disabled', true);
        popupElements.chkUnlimited.prop('disabled', true);
        popupElements.txtRemark.prop('disabled', true);
        popupElements.btnSave.prop('disabled', true);
        popupElements.btnCancel.prop('disabled', true);
    };

    // Fill request data into edit form.
    var fillRequestInfo = function (request) {
        if (request) {
            popupElements.txtUsername.val(request.Username);
            popupElements.txtRemark.val(request.Remark.decode());
            popupElements.hdnCustId.val(request.CustId);
            popupElements.hdnExpiredDate.val(request.ExpiredDate);
            popupElements.hdnRequestId.val(request.RequestId);

            if (request.IP) {
                popupElements.txtIP.val(request.IP);
                $(popupElements.rdbRestriction[1]).prop('checked', true);
            }

            if (request.CountryCode) {
                popupElements.ddlCountry.val(request.CountryCode);
                $(popupElements.rdbRestriction[0]).prop('checked', true);
            }

            if (request.ExpiredDate) {
                popupElements.dpkExpiredDate.datepicker('setDate', request.ExpiredDate);
            } else {
                popupElements.chkUnlimited.prop('checked', true);
            }
        }
    };

    // Add new request.
    var addNewRequest = function (addNewReqParameters) {
        $.ajax({
            type: 'POST',
            url: urls.InsertRequest,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(addNewReqParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.IsSuccessful) {
                    CMS.showInlineSuccessMsg(messages.addRequestSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disableRequestForm();
                    getRequestList();
                } else {
                    CMS.showInlineErrorMsg(result.Message, popupElements.divErrorSuccessMsg);
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

    // Update request.
    var updateRequest = function (updateReqParameters) {
        $.ajax({
            type: 'POST',
            url: urls.UpdateRequest,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updateReqParameters),
            beforeSend: function () {
                CMS.disableInput();
            },
            success: function (result) {
                if (result.IsSuccessful) {
                    CMS.showInlineSuccessMsg(messages.updateRequestSuccessfully, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disableRequestForm();
                    getRequestList();
                } else {
                    CMS.showInlineErrorMsg(result.Message, popupElements.divErrorSuccessMsg);
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

    // Delete request.
    var deleteRequest = function (requestId) {
        CMS.showConfirmMessage(messages.deleteRequestConfirm.format(requestId), function () {
            var parameters = {
                RequestId: requestId
            };

            $.ajax({
                type: 'POST',
                url: urls.DeleteRequest,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(parameters),
                beforeSend: function () {
                    CMS.disableInput();
                },
                success: function () {
                    CMS.showSuccessMessage(messages.deleteRequestSuccessfully);
                    getRequestList();
                },
                error: function (xhr) {
                    CMS.handleAjaxError(xhr);
                },
                complete: function () {
                    CMS.enableInput();
                }
            });
        });
    };

    $this.InitPage = function () {
        cacheMainDOMElements();
        initFields();
    };
};