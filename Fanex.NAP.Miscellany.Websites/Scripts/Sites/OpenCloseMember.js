/* File Created: March 4, 2014 */
/* global Namespace, Fanex, Miscellany, labels */

'use strict';

$(document).ready(function () {
    var memberList = new Fanex.Miscellany.CloseOpenMember();

    memberList.InitPage();
});

Namespace.Register("Fanex.Miscellany.CloseOpenMember");
Fanex.Miscellany.CloseOpenMember = function () {
    var $this = this;
    var urls = {
        GetMemberList: Miscellany.resolveRootUrl('CloseOpenMember/GetMemberList'),
        UpdateMembers: Miscellany.resolveRootUrl('CloseOpenMember/UpdateMembers')
    };
    var parameters = {
        Username: null,
        Status: null,
        PageIndex: null,
        PageSize: null
    };
    var domElements = {
        txtUsername: null,
        ddlStatus: null,
        btnSearch: null,
        spnNote: null,
        divMemberList: null
    };
    var levels = {
        super: 1,
        master: 2,
        agent: 3
    };
    var pageRanges = [50, 100, 200, 500];

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.txtUsername = $('#txtUsername');
        domElements.ddlStatus = $('#ddlStatus');
        domElements.btnSearch = $('#btnSearch');
        domElements.spnNote = $('#spnNote');
        domElements.divMemberList = $('#divMemberList');
    };

    // Get parameters.
    var getParameters = function () {
        parameters.Username = domElements.txtUsername.val();
        parameters.Status = domElements.ddlStatus.val();
        parameters.PageIndex = 0;
    };

    // Initialize fields and handle events.
    var initFields = function () {
        domElements.spnNote.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });
        domElements.btnSearch.click(function () {
            // @@TODO: Validate.
            getParameters();
            //getMemberList();
            Miscellany.headlineMsgBox.confirm('Are you sure want to OPEN selected accounts', function () { }, { modal: true, closeBtn: false, header: true });
        });
    };

    var renderMemberList = function (data) {
        var gridOptions =
        {
            columns: [{ HeaderText: labels.member, Width: 150, Name: 'ColMember', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.status, Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'left'}],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex,
                itemsPerPageArray: pageRanges,
                pagerSelectCallBack: handleChangingPage
            },
            sortOption: {
                serverSortFunction: handleSorting
            },
            gridExpandHeight: 200
        };
        //var bodyData = [];

        if (data.Level < levels.agent) {
            gridOptions.columns.unshift({ HeaderText: labels.agent, Width: 150, Name: 'ColAgent', HeaderAlign: 'center', CellAlign: 'center' });

            if (data.Level < levels.master) {
                gridOptions.columns.unshift({ HeaderText: labels.master, Width: 150, Name: 'ColMaster', HeaderAlign: 'center', CellAlign: 'center' });
            }
        }

        domElements.divMemberList.fanexGrid(gridOptions);
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        parameters.PageSize = itemsPerPage;
        parameters.PageIndex = pageIndex;
        getMemberList();
    };

    // Handle sorting at server side.
    var handleSorting = function (sortColumnName, sortDirection) {
        parameters.SortDirection = sortDirection;
        parameters.PageIndex = 0;
        getMemberList();
    };

    // @@TODO: Handle given result here.
    var getMemberList = function () {
        Miscellany.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetMemberList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                renderMemberList(result);
            },
            error: function (xhr, status, err) {
                Miscellany.handleAjaxError(xhr, status, err);
            },
            complete: function () {
                Miscellany.enableInput();
            }
        });
    };

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};