/* File Created: March 4, 2014 */
/* global Namespace, Fanex, CMS, Validator, labels, messages */
/* jshint strict: false */
// 'use strict';

$(document).ready(function () {
    var memberList = new Fanex.CMS.CloseOpenMember();

    memberList.InitPage();
});

Namespace.Register("Fanex.CMS.CloseOpenMember");
Fanex.CMS.CloseOpenMember = function () {
    var $this = this;
    var urls = {
        GetMemberList: CMS.resolveRootUrl('CloseOpenMember/GetMemberList'),
        UpdateMembers: CMS.resolveRootUrl('CloseOpenMember/UpdateMembers')
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
        btnOpenValue: null,
        btnClose: null,
        btnCloseValue: null
    };
    var updateType = {
        close: 0,
        open: 1
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
        domElements.btnOpenValue = domElements.btnOpen.attr('data-value');
        domElements.btnClose = $('#btnClose');
        domElements.btnCloseValue = domElements.btnClose.attr('data-value');
    };

    // Get parameters.
    var getParameters = function () {
        parameters.Username = domElements.txtUsername.val();
        parameters.Status = domElements.ddlStatus.val();
        parameters.PageIndex = 0;
    };

    // Initialize fields and handle events.
    var initFields = function () {
        parameters.PageSize = CMS.pageSize;

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
        domElements.txtUsername.focus();

        domElements.btnSearch.click(function () {
            var isValid = Validator.validateRequiredField(domElements.txtUsername, 'n');

            clearStatusColError();

            if (isValid) {
                getParameters();
                getMemberList();
            }
        });

        domElements.btnOpen.click(function () {
            var selectedMembers = getCheckedMembers(true);

            if (selectedMembers.length > 0) {
                var updatingParameters = {
                    MemberStatusList: selectedMembers,
                    UpdateType: updateType.open
                };

                CMS.showConfirmMessage(messages.openMemberConfirmationMsg.format(selectedMembers.length), function () {
                    updateMemberStatus(updatingParameters);
                });
            }
        });

        domElements.btnClose.click(function () {
            var selectedMembers = getCheckedMembers(false);

            if (selectedMembers.length > 0) {
                var updatingParameters = {
                    MemberStatusList: selectedMembers,
                    UpdateType: updateType.close
                };

                CMS.showConfirmMessage(messages.closeMemberConfirmationMsg.format(selectedMembers.length), function () {
                    updateMemberStatus(updatingParameters);
                });
            }
        });

        resetButtonValues();
    };

    // Reset counting values of buttons.
    var resetButtonValues = function () {
        domElements.btnOpen.attr('value', domElements.btnOpenValue.format(''));
        domElements.btnClose.attr('value', domElements.btnCloseValue.format(''));
    };

    // Update counters in the Open/Close buttons.
    var updateButtonsCounters = function ($obj, checkboxes, countAll) {
        var updatedCounter = '';
        var counter;

        if (countAll) {
            // Update OPEN button.
            counter = checkboxes.filter('[data-closed="true"]:checked').length;
            updatedCounter = counter > 0 ? counter + ' ' : updatedCounter;
            domElements.btnOpen.attr('value', domElements.btnOpenValue.format(updatedCounter));

            // Update CLOSE button.
            counter = checkboxes.filter('[data-closed="false"]:checked').length;
            updatedCounter = counter > 0 ? counter + ' ' : updatedCounter;
            domElements.btnClose.attr('value', domElements.btnCloseValue.format(updatedCounter));
        } else {
            if ($obj.attr('data-closed') === 'true') {
                counter = checkboxes.filter('[data-closed="true"]:checked').length;

                updatedCounter = counter > 0 ? counter + ' ' : updatedCounter;
                domElements.btnOpen.attr('value', domElements.btnOpenValue.format(updatedCounter));
            } else {
                counter = checkboxes.filter('[data-closed="false"]:checked').length;

                updatedCounter = counter > 0 ? counter + ' ' : updatedCounter;
                domElements.btnClose.attr('value', domElements.btnCloseValue.format(updatedCounter));
            }
        }
    };

    // Render grid.
    var renderMemberList = function (data) {
        var statusChkColId = 'chkStatus';
        var masterChkId;
        var agentChkId;

        var chkFormat = '<div class="agent-column">' +
            '<input type="checkbox" id="{0}" data-chk-group="{1}"/>' +
            '<label for="{0}">{2}</label>' +
            '</div>';
        var statusHeaderFormat = '<div class="checkbox-status status-header"><input type="checkbox" id="{0}"/></div>' +
            '<label for="{0}" class="lbl-for-chkStatus">{1}</label>';
        var statusFormat = '<div class="checkbox-status">' +
            '<input type="checkbox" id="{0}" data-checked="{1}" data-closed="{2}" data-sync-casino="{3}" data-chk-group="{4}"/>' +
            '</div>' +
            '<label for="{0}" class="lbl-for-chkStatus">{5}</label>';
        var statusCol = statusHeaderFormat.format(statusChkColId, labels.status);
        var memberFormat = '<label>{0}</label>';

        var gridOptions = {
            columns: [{ HeaderText: labels.order, Width: 50, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: labels.member, Width: 220, Name: 'ColMember', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: statusCol, Width: 120, Name: 'ColStatus', HeaderAlign: 'left', CellAlign: 'left', CssClass: 'chk-column' }],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: parameters.PageSize,
                currentPage: parameters.PageIndex,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: handleChangingPage
            },
            gridExpandHeight: 200
        };
        var bodyData = [];
        var row;
        var currentRow = 0;
        var currentAgentRow;
        var currentMasterRow;
        var masterChildren;
        var downlineList;
        var highlightClass;

        resetButtonValues();

        if (data && data.DownlineList.length > 0) {
            downlineList = data.DownlineList;

            if (downlineList[0].AgentStatusList[0].Username) {
                gridOptions.columns.splice(1, 0, { HeaderText: labels.agent, Width: 220, Name: 'ColAgent', HeaderAlign: 'center', CellAlign: 'left' });
            }

            if (downlineList[0].Username) {
                gridOptions.columns.splice(1, 0, { HeaderText: labels.master, Width: 220, Name: 'ColMaster', HeaderAlign: 'center', CellAlign: 'left' });
            }

            $.each(downlineList, function (masterIndex, master) {
                masterChildren = 0;
                currentMasterRow = currentRow;
                masterChkId = master.Username ? 'mas' + master.Username : statusChkColId;

                $.each(master.AgentStatusList, function (agentIndex, agent) {
                    masterChildren += agent.MemberStatusList.length;
                    currentAgentRow = currentRow;
                    agentChkId = agent.Username ? 'age' + agent.Username : statusChkColId;

                    $.each(agent.MemberStatusList, function (index, item) {
                        statusCol = statusFormat.format(
                            item.CustId,
                            item.IsChecked,
                            item.Closed,
                            item.SyncCasino,
                            agentChkId, item.Closed ? labels.closed : labels.open);
                        highlightClass = (item.Closed ? 'highlight ' : '') + 'hover-column';
                        row = {
                            Columns: [{ Name: 'ColOrder', Value: currentRow },
                            { Name: 'ColMember', Value: memberFormat.format(item.Username), TdClass: highlightClass, CssClass: 'member-column' },
                            { Name: 'ColStatus', Value: statusCol, TdClass: highlightClass, CssClass: 'chk-column' }]
                        };

                        bodyData.push(row);
                        currentRow++;
                    });

                    // Show Agent column if the searched user is Super/Master.
                    if (agent.Username) {
                        row = bodyData[currentAgentRow];
                        row.Columns.splice(1, 0, {
                            Name: 'ColAgent',
                            Value: chkFormat.format(agentChkId, masterChkId, agent.Username),
                            MergeRowNumber: agent.MemberStatusList.length,
                            CssClass: 'chk-column'
                        });
                    }
                });

                // Show Master column if searched user is Super.
                if (master.Username) {
                    row = bodyData[currentMasterRow];
                    row.Columns.splice(1, 0, {
                        Name: 'ColMaster',
                        Value: chkFormat.format(masterChkId, 'chkStatus', master.Username),
                        MergeRowNumber: masterChildren,
                        CssClass: 'chk-column'
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

    // Handle grid's events.
    var handleGridEvents = function () {
        var chkStatus = domElements.divMemberList.find('#chkStatus');
        var checkboxes = domElements.divMemberList.find('input[type=checkbox]');
        var memberCheckboxes = checkboxes.filter('[data-closed]');
        var bindMemberCounterHandler = function () {
            memberCheckboxes.on('change', function () {
                updateButtonsCounters($(this), checkboxes);
            }).on('update', function () {
                updateButtonsCounters($(this), checkboxes);
            });
        };
        var unbindMemberCounterHandler = function () {
            memberCheckboxes.off('change').off('update');
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

        setTimeout(function () {
            checkboxes.filter('[data-checked="true"]').each(function () {
                var $ele = $(this);

                $ele.prop('checked', true).trigger('update', true);
            });
        }, 50);
    };

    // Show/hide OPEN/CLOSE buttons.
    var showHideButtons = function () {
        var status = parseInt(domElements.ddlStatus.val());

        domElements.divButtons.addClass('display-none');
        domElements.btnOpen.hide();
        domElements.btnClose.hide();

        if (status === -1) {
            domElements.btnOpen.show();
            domElements.btnClose.show();
        } else if (status === 0) {
            domElements.btnClose.show();
        } else {
            domElements.btnOpen.show();
        }

        domElements.divButtons.removeClass('display-none');
    };

    // Validate status column.
    var getCheckedMembers = function (isClosed) {
        var checkedRows = domElements.divMemberList.find('input[type="checkbox"][data-closed="' + isClosed + '"]:checked');
        var chkStatus = domElements.divMemberList.find('#chkStatus');
        var selectedMembers = [];
        var msg = messages.requiredStatusCol.format(isClosed ? labels.closed : labels.open);

        clearStatusColError();

        if (checkedRows.length === 0) {
            chkStatus.attr('title', msg).tipsy({
                trigger: 'manual',
                gravity: 'sw',
                classStyle: 'error',
                opacity: 0.97
            }).tipsy('show');
        } else {
            $.each(checkedRows, function (index, member) {
                var $member = $(member);

                selectedMembers.push({
                    CustId: $member.attr('id'),
                    SyncCasino: $member.attr('data-sync-casino')
                });
            });
        }

        return selectedMembers;
    };

    var clearStatusColError = function () {
        var chkStatus = domElements.divMemberList.find('#chkStatus');

        chkStatus.tipsy('hide').removeAttr('original-title');
    };

    // Get member list.
    var getMemberList = function () {
        CMS.closeMsgBox();
        CMS.disableInput();
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
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    // Update members' status.
    var updateMemberStatus = function (updatingParameters) {
        updatingParameters.SearchedUsername = parameters.Username;
        updatingParameters.Status = parameters.Status;
        updatingParameters.PageSize = parameters.PageSize;
        updatingParameters.PageIndex = parameters.PageIndex;

        CMS.closeMsgBox();
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.UpdateMembers,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(updatingParameters),
            success: function (result) {
                if (result.ErrorCode > 0) {
                    handleUpdateMemberStatusError(result);
                } else if (result.NumberOfUpdatedItems === result.TotalItems) {
                    CMS.showSuccessMessage(messages.updateStatusSuccessfully.format(result.NumberOfUpdatedItems, result.TotalItems));
                } else if (result.NumberOfFailedItems === result.TotalItems) {
                    CMS.showErrorMessage(messages.updateStatusFailed.format(result.NumberOfFailedItems, result.TotalItems));
                } else {
                    CMS.showMixMessage(
                        messages.updateStatusSuccessfully.format(result.NumberOfUpdatedItems, result.TotalItems),
                        messages.updateStatusFailed.format(result.NumberOfFailedItems, result.TotalItems));
                }

                renderMemberList(result.MembersStatusResult);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    var handleUpdateMemberStatusError = function (result) {
        if (result.ErrorCode === 105) {
            if (result.UpdatedCount > 0) {
                CMS.showMixMessage(
                    messages.updateStatusSuccessfully.format(result.UpdatedCount, result.TotalItems),
                    `${result.ListCustRemove}: ${result.ErrorMsg}`);
            } else {
                CMS.showErrorMessage(`${result.ListCustRemove}: ${result.ErrorMsg}`);
            }
        } else {
            CMS.showErrorMessage(result.ErrorMsg);
        }
    };

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};