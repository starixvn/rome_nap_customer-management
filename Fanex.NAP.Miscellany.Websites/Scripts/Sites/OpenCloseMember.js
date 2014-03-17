/* File Created: March 4, 2014 */
/* global Namespace, Fanex, Miscellany, Validator, labels */

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
        divMemberList: null,
        divButtons: null,
        btnOpen: null,
        btnClose: null
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.txtUsername = $('#txtUsername');
        domElements.ddlStatus = $('#ddlStatus');
        domElements.btnSearch = $('#btnSearch');
        domElements.spnNote = $('#spnNote');
        domElements.divMemberList = $('#divMemberList');
        domElements.divButtons = $('#divButtons');
        domElements.btnOpen = $('#btnOpen');
        domElements.btnClose = $('#btnClose');
    };

    // Get parameters.
    var getParameters = function () {
        parameters.Username = domElements.txtUsername.val();
        parameters.Status = domElements.ddlStatus.val();
        parameters.PageIndex = 0;
    };

    // Initialize fields and handle events.
    var initFields = function () {
        parameters.PageSize = Miscellany.pageSize;

        domElements.spnNote.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });

        domElements.txtUsername.keyup(function (event) {
            if (event.keyCode === 13) {
                domElements.btnSearch.click();
            }
        });

        domElements.btnSearch.click(function () {
            var isValid = Validator.validateRequiredField(domElements.txtUsername, 'n');

            if (isValid) {
                getParameters();
                getMemberList();
            }
        });
    };

    // Render grid.
    var renderMemberList = function (data) {
        var chkFormat = '<input type="checkbox" id="{0}" data-chk-group="{1}"/><label for="{0}" class="lbl-for-chk">{2}</label>';
        var statusFormat = '<input type="checkbox" id="{0}"/><label for="{0}" class="lbl-for-chkStatus">{1}</label>';
        var statusCol = chkFormat.format('chkStatus', '', labels.status);

        var gridOptions =
        {
            columns: [{ HeaderText: labels.order, Width: 50, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.member, Width: 220, Name: 'ColMember', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: statusCol, Width: 120, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'left'}],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex,
                itemsPerPageArray: Miscellany.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 180
        };
        var bodyData = [];
        var row;
        var currentRow = 0;
        var currentAgentRow;
        var currentMasterRow;
        var masterChildren;
        var downlineList;

        if (data && data.DownlineList.length > 0) {
            downlineList = data.DownlineList;

            if (downlineList[0].AgentStatusList[0].Username) {
                gridOptions.columns.splice(1, 0, { HeaderText: labels.agent, Width: 220, Name: 'ColAgent', HeaderAlign: 'center', CellAlign: 'center' });
            }

            if (downlineList[0].Username) {
                gridOptions.columns.splice(1, 0, { HeaderText: labels.master, Width: 220, Name: 'ColMaster', HeaderAlign: 'center', CellAlign: 'center' });
            }

            $.each(downlineList, function (masterIndex, master) {
                masterChildren = 0;
                currentMasterRow = currentRow;

                $.each(master.AgentStatusList, function (agentIndex, agent) {
                    masterChildren += agent.MemberStatusList.length;
                    currentAgentRow = currentRow;

                    $.each(agent.MemberStatusList, function (index, item) {
                        row = {
                            Columns: [{ Name: 'ColOrder', Value: currentRow },
                                      { Name: 'ColMember', Value: item.Username },
                                      { Name: 'ColStatus', Value: statusFormat.format(item.CustId, item.Closed ? labels.closed : labels.open)}]
                        };
                        bodyData.push(row);
                        currentRow++;
                    });

                    if (agent.Username) {
                        row = bodyData[currentAgentRow];
                        row.Columns.splice(1, 0, {
                            Name: 'ColAgent',
                            Value: chkFormat.format('age' + agent.Username, 'mas' + master.Username, agent.Username),
                            MergeRowNumber: agent.MemberStatusList.length
                        });
                    }
                });

                if (master.Username) {
                    row = bodyData[currentMasterRow];
                    row.Columns.splice(1, 0, {
                        Name: 'ColMaster',
                        Value: chkFormat.format('mas' + master.Username, 'chkStatus', master.Username),
                        MergeRowNumber: masterChildren
                    });
                }
            });
        }

        gridOptions.pagerOption.totalItem = data.TotalItems;
        gridOptions.bodyRows = bodyData;
        domElements.divMemberList.fanexGrid(gridOptions);

        if (bodyData.length > 0) {
            handleGridEvents();
            showHideButtons();
        }
    };

    // Handle paging at server side.
    var handleChangingPage = function (pageIndex, itemsPerPage) {
        parameters.PageSize = itemsPerPage;
        parameters.PageIndex = pageIndex;
        getMemberList();
    };

    var handleGridEvents = function () {
        domElements.divMemberList.find('input[type=checkbox]').each(function () {
            $(this).checkboxGroup();
        });
    };

    var showHideButtons = function () {
        var status = parseInt(domElements.ddlStatus.val());

        domElements.divButtons.addClass('display-none');
        domElements.btnOpen.hide();
        domElements.btnClose.hide();

        if (status === -1) {
            domElements.btnOpen.show();
            domElements.btnClose.show();
        } else if (status === 0) {
            domElements.btnOpen.show();
        } else {
            domElements.btnClose.show();
        }

        domElements.divButtons.removeClass('display-none');
    };

    var getMemberList = function () {
        Miscellany.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetMemberList,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(parameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    Validator.showError(domElements.txtUsername, result.ErrorMsg, 'n');
                } else {
                    renderMemberList(result);
                }
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