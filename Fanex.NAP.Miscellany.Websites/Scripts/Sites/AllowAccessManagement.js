/* File Created: March 6, 2014 */
/* global Namespace, Fanex, Validator, Miscellany, Layout, labels, messages, hasEditAccessRight, minDate */

$(document).ready(function () {
    'use strict';
    var manager = new Fanex.NAP.Miscellany.AllowAccessManagement();

    manager.InitPage();
});

Namespace.Register('Fanex.NAP.Miscellany.AllowAccessManagement');
Fanex.NAP.Miscellany.AllowAccessManagement = function () {
    'use strict';

    var $this = this;
    var urls = {
        GetRequestList: Miscellany.resolveRootUrl('AllowAccessManagement/GetRequestList'),
        InsertRequest: Miscellany.resolveRootUrl('AllowAccessManagement/InsertRequest'),
        UpdateRequest: Miscellany.resolveRootUrl('AllowAccessManagement/UpdateRequest'),
        DeleteRequest: Miscellany.resolveRootUrl('AllowAccessManagement/DeleteRequest')
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
        txtUsername: null,
        rdbRestriction: null,
        ddlCountry: null,
        txtIP: null,
        dpkExpiredDate: null,
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
        mainDOMElements.divRequestForm = $('#divRequestForm');
        mainDOMElements.divPopup = $('#dialog-popup');
    };

    // Cache popup elements
    var cachePopupElements = function () {
        popupElements.txtUsername = $('#txtUsername', mainDOMElements.divPopup);
        popupElements.rdbRestriction = $('[name=Restriction]', mainDOMElements.divPopup);
        popupElements.ddlCountry = $('#ddlCountry', mainDOMElements.divPopup);
        popupElements.txtIP = $('#txtIP', mainDOMElements.divPopup);
        popupElements.dpkExpiredDate = $('#dpkExpiredDate', mainDOMElements.divPopup);
        popupElements.txtRemark = $('#txtRemark', mainDOMElements.divPopup);
        popupElements.btnSave = $('#btnSave', mainDOMElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', mainDOMElements.divPopup);
    };

    // Initialize fields and handle events.
    var initFields = function () {
        searchParameters.PageSize = Miscellany.pageSize;

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
        if (!isNewRequest) {
            popupElements.txtUsername.prop('disabled', true);
        }

        //@@TODO: Combobox, datepicker, radio button.
        mainDOMElements.divPopup.find('select').each(function () {
            $(this).addClass('combobox-autocomplete').combobox();
        });
        mainDOMElements.divPopup.find('input[type=text]').each(function () {
            $(this).clearfield();
        });

        Layout.createDatepicker(popupElements.dpkExpiredDate, 'txtExpiredDate', labels.expiredDate, minDate, minDate, null);
        mainDOMElements.divPopup.find('#txtExpiredDate').addClass('width-200');
        popupElements.txtIP.ipAddress();

        popupElements.btnSave.click(function () {
            if (Validator.validateForm(mainDOMElements.divPopup)) {
                var parameters = {
                    Username: popupElements.txtUsername.val(),
                    Country: null, //??
                    IP: null, //??
                    ExpiredDate: popupElements.dpkExpiredDate.val(),
                    Remark: popupElements.txtRemark.val()
                };

                if (isNewRequest) {
                    addNewRequest(parameters);
                } else {
                    updateRequest(parameters);
                }
            }
        });

        popupElements.btnCancel.click(function () {
            Miscellany.closePopup();
        });
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
                Miscellany.disableInput();
            },
            success: function (result) {
                renderTable(result.RequestList, result.TotalItems);
            },
            error: function (xhr, status, err) {
                Miscellany.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                Miscellany.enableInput();
            }
        });
    };

    // Render data table for agent lisy.
    var renderTable = function (data, totalItems) {
        var bodyData = [];
        var columns = [];
        var gridOptions = {
            showHighlightRow: false,
            showFooter: true,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: searchParameters.PageSize,
                currentPage: searchParameters.PageIndex,
                totalItem: totalItems,
                itemsPerPageArray: Miscellany.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 250
        };

        mainDOMElements.divMemberList.addClass('display-none');

        columns = [{ HeaderText: labels.id, Width: 70, Name: 'ColID', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.username, Width: 130, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.expiredDate, Width: 100, Name: 'ColExpiredDate', HeaderAlign: 'center', CellAlign: 'right' },
                   { HeaderText: labels.country, Width: 120, Name: 'ColCountry', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.ip, Width: 120, Name: 'ColIP', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.remark, Width: 55, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'center' },
                   { HeaderText: labels.creator, Width: 130, Name: 'ColCreator', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.createdDate, Width: 100, Name: 'ColCreatedDate', HeaderAlign: 'center', CellAlign: 'right' },
                   { HeaderText: labels.lastModifiedBy, Width: 130, Name: 'ColLastModifiedBy', HeaderAlign: 'center', CellAlign: 'left' },
                   { HeaderText: labels.lastModifiedDate, Width: 120, Name: 'ColLastModifiedDate', HeaderAlign: 'center', CellAlign: 'right'}];

        if (hasEditAccessRight) {
            columns.push({ HeaderText: labels.edit, Width: 50, Name: 'ColEdit', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: labels.del, Width: 50, Name: 'ColDelete', HeaderAlign: 'center', CellAlign: 'center' });
        }

        if (data && data.length > 0) {
            var row = null;
            var remarkCol = '<span class="ico-remark" title="{0}"></span>';
            var editCol = '<span class="ico-edit" data-item-index="{0}" title="{1}"></span>';
            var delCol = '<span class="ico-delete" data-request-id="{0}" title="{1}"></span>';

            cachedData = data;
            $.each(data, function (index, item) {
                row =
                {
                    Columns: [{ Name: 'ColID', Value: item.RequestId },
                              { Name: 'ColUsername', Value: item.Username },
                              { Name: 'ColExpiredDate', Value: item.ExpiredDate },
                              { Name: 'ColCountry', Value: item.Country },
                              { Name: 'ColIP', Value: item.IP },
                              { Name: 'ColRemark', Value: remarkCol.format(item.Remark) },
                              { Name: 'ColCreator', Value: item.Creator },
                              { Name: 'ColCreatedDate', Value: item.CreadtedDated },
                              { Name: 'ColLastModifiedBy', Value: item.LastModifiedBy },
                              { Name: 'ColLastModifiedDate', Value: item.LastModifiedDate}]
                };

                if (hasEditAccessRight) {
                    row.Columns.push({ Name: 'ColEdit', Value: editCol.format(index, labels.edit) });
                    row.Columns.push({ Name: 'ColDelete', Value: delCol.format(item.RequestId, labels.del) });
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

        Miscellany.modal(title, form);
        cachePopupElements();
        bindPopupEventhandlers(isNewRequest);

        if (!isNewRequest) {
            fillRequestInfo(request);
        }
    };

    // Fill request data into edit form.
    var fillRequestInfo = function (request) {
        console.log(request);
        if (request) {
            popupElements.txtUsername.val(request.Username);
            popupElements.dpkExpiredDate.datepicker('setDate', request.ExpiredDate);
            popupElements.txtRemark.val(request.Remark);
            //popupElements.rdbRestriction ??;

            if (request.IP) {
                popupElements.txtIP.val(request.IP);
            }

            if (request.CountryCode) {
                popupElements.ddlCountry.combobox('autocomplete', request.CountryCode);
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
                Miscellany.disableInput();
            },
            success: function () {
                //@@TODO: Show success msg, close popup and reload grid.
            },
            error: function (xhr, status, err) {
                Miscellany.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                Miscellany.enableInput();
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
                Miscellany.disableInput();
            },
            success: function () {
                //@@TODO: Show success msg, close popup and reload grid.
            },
            error: function (xhr, status, err) {
                Miscellany.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                Miscellany.enableInput();
            }
        });
    };

    // Delete request.
    var deleteRequest = function (requestId) {
        Miscellany.headlineMsgBox.confirm(messages.deleteRequestConfirm, function () {
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
                    Miscellany.disableInput();
                },
                success: function () {
                    //@@TODO: Show success msg.
                },
                error: function (xhr, status, err) {
                    Miscellany.handleAjaxError(xhr, status, err);
                },
                complete: function () {
                    Miscellany.enableInput();
                }
            });
        }, { modal: true, closeBtn: false, header: true });
    };

    $this.InitPage = function () {
        cacheMainDOMElements();
        initFields();
    };
};