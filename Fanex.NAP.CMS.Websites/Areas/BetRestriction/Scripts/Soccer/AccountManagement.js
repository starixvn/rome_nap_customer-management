$(document).ready(function () {
    accountManagement = new Fanex.CMS.BetRestrictionSoccer.AccountManagement();
    accountManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionSoccer.AccountManagement');

Fanex.CMS.BetRestrictionSoccer.AccountManagement = function () {
    var $this = $(this);

    var oldData = {};

    var MainDOMElements = {
        formAccountManager: null
    };

    var CachedDOMElements = {
        formAccountManager: null,
        hidCustId: null,
        txtPopupAccUsername: null,
        selPopupAccLiveBetAccept: null,
        selPopupAccSeizeBetTicket: null,
        multiSelPopupLeagueGroup: null,
        txtPopupAccMaxBet: null,
        txtPopupAccMaxPerMatch: null,
        txtPopupAcc8RMAccount: null,
        noteInfoMessage: null,
        btnSave: null,
        btnCancel: null
    };

    var cacheDOMElements = function () {
        CachedDOMElements.formAccountManager = $('#formSoccerAccountManager');
        CachedDOMElements.hidCustId = $('#hidCustId');
        CachedDOMElements.txtPopupAccUsername = $('#txtPopupAccUsername');
        CachedDOMElements.selPopupAccLiveBetAccept = $('#selPopupAccLiveBetAccept');
        CachedDOMElements.selPopupAccSeizeBetTicket = $('#selPopupAccSeizeBetTicket');
        CachedDOMElements.txtPopupAccMaxBet = $('#txtPopupAccMaxBet');
        CachedDOMElements.txtPopupAccMaxPerMatch = $('#txtPopupAccMaxPerMatch');
        CachedDOMElements.txtPopupAcc8RMAccount = $('#txtPopupAcc8RMAccount');
        CachedDOMElements.noteInfoMessage = $('#noteInfoMessage');
        CachedDOMElements.btnSave = $('#btnAccSave');
        CachedDOMElements.btnCancel = $('#btnAccCancel');
        
        $('#selPopupLeagueGroup').multiSelect({
            minWidth: 280,
            maxHeight: 120,
            checkAllText: 'All',
            listData: getLeagueGroupItems(false, []),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
            disabled: true
        });
        CachedDOMElements.multiSelPopupLeagueGroup = $('#selPopupLeagueGroup');
    };

    var bindingEvents = function () {
        CachedDOMElements.txtPopupAccMaxBet.keydown(function (e) { return CMS.forceNumericInput(e, true); });
        CachedDOMElements.txtPopupAccMaxBet.on("cut copy paste", function (e) {
            e.preventDefault();
        });
        CachedDOMElements.txtPopupAccMaxPerMatch.keydown(function (e) { return CMS.forceNumericInput(e, true); });
        CachedDOMElements.txtPopupAccMaxPerMatch.on("cut copy paste", function (e) {
            e.preventDefault();
        });
        CachedDOMElements.btnCancel.click(function () { CMS.closePopup(); });
        CachedDOMElements.noteInfoMessage.click(function () { CMS.showInfoMessage(messages.NoteInfoMaxBet); });
        CachedDOMElements.selPopupAccSeizeBetTicket.change(function () {
            if ($(this).val() == '0' || $(this).val() == '') {
                CachedDOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
                CachedDOMElements.multiSelPopupLeagueGroup.multiSelect("disable");
                CMS.clearTipsyError(CachedDOMElements.txtPopupAcc8RMAccount);
                CMS.clearTipsyError(CachedDOMElements.multiSelPopupLeagueGroup);
            }
            else {
                CachedDOMElements.txtPopupAcc8RMAccount.removeAttr('disabled');
                CachedDOMElements.multiSelPopupLeagueGroup.multiSelect("enable");
            }
        });

    };

    var validateForm = function () {
        if (!Validator.validateForm(CachedDOMElements.formAccountManager, 'ne'))
            return false;

        if (CachedDOMElements.selPopupAccLiveBetAccept.val() == 'Please select') {
            CMS.showTipsyError(CachedDOMElements.selPopupAccLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (!HasSeizeBetTicket()) {
            CachedDOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
            CachedDOMElements.multiSelPopupLeagueGroup.multiSelect('disable');
        }
        else if (CachedDOMElements.multiSelPopupLeagueGroup.multiSelect('getCheckedItems').length === 0) {
            CMS.showTipsyError(CachedDOMElements.multiSelPopupLeagueGroup, messages.RequiredFieldError);
            return false;
        }
        else if (CachedDOMElements.txtPopupAcc8RMAccount.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.txtPopupAcc8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if ((CachedDOMElements.txtPopupAccMaxBet.val().length > 0 || CachedDOMElements.txtPopupAccMaxPerMatch.val().length > 0)
            && parseInt(CachedDOMElements.txtPopupAccMaxBet.val()) > parseInt(CachedDOMElements.txtPopupAccMaxPerMatch.val())) {
            CMS.showTipsyError(CachedDOMElements.txtPopupAccMaxBet, messages.ValidateMaxBetMaxPerMatchError);
            return false;
        }


        if (oldData) {
            var newData = {
                CustId: CachedDOMElements.hidCustId.val(),
                UserName: CachedDOMElements.txtPopupAccUsername.val(),
                LiveBetAccept: CachedDOMElements.selPopupAccLiveBetAccept.val(),
                SeizeBetTicket: CachedDOMElements.selPopupAccSeizeBetTicket.val(),
                LeagueGroupId: HasSeizeBetTicket() ? GetLeagueGroupCheckedItems(CachedDOMElements.multiSelPopupLeagueGroup) : '',
                MaxBet: CachedDOMElements.txtPopupAccMaxBet.val(),
                MaxPerMatch: CachedDOMElements.txtPopupAccMaxPerMatch.val(),
                FollowAccount: HasSeizeBetTicket() ? CachedDOMElements.txtPopupAcc8RMAccount.val() : ''
            };

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }

        return true;
    };

    var saveAddEditAccount = function (isEdit) {
        CMS.disableInput();
        if (!validateForm()) {
            CMS.enableInput();
            return;
        }

        var params = {
            CustId: CachedDOMElements.hidCustId.val(),
            UserName: CachedDOMElements.txtPopupAccUsername.val(),
            LiveBetAccept: CachedDOMElements.selPopupAccLiveBetAccept.val(),
            SeizeBetTicket: CachedDOMElements.selPopupAccSeizeBetTicket.val(),
            LeagueGroupId: HasSeizeBetTicket() ? GetLeagueGroupCheckedItems(CachedDOMElements.multiSelPopupLeagueGroup) : '',
            MaxBet: CachedDOMElements.txtPopupAccMaxBet.val(),
            MaxPerMatch: CachedDOMElements.txtPopupAccMaxPerMatch.val(),
            FollowAccount: HasSeizeBetTicket() ? CachedDOMElements.txtPopupAcc8RMAccount.val() : ''
        };

        if (isEdit) {
            saveAccount(params, isEdit);
        } else {
            CMS.callAjax(
                this,
                'CheckAddAccount',
                params,
                function (result) {
                    if (result.Code > 0) {
                        CMS.showConfirmMessage(
                            '<span style="white-space: pre-line">' + result.Message + '</span>',
                            function () {
                                return saveAccount(params, isEdit);
                            }
                        );
                    } else {
                        saveAccount(params, isEdit);
                    }
                }
            );
        }
    };

    var saveAccount = function (params, isEdit) {
        CMS.callAjax(
            this,
            isEdit ? 'EditAccount' : 'AddAccount',
            params,
            function (result) {
                if (result.Code == 0) {
                    CMS.showSuccessMessage(isEdit ? messages.UpdateAccountSuccessfully : messages.AddAccountSuccessfully);
                    setTimeout(function () {
                        CMS.closePopup();
                        betRestriction.Search();
                    }, 3000);
                }
                else {
                    CMS.showErrorMessage(result.Message);
                }
            }
        );
    };

    var loadAccountDetailForEdit = function (custId) {
        CMS.callAjax(
            this,
            'GetAccountDetail',
            { CustId: custId },
            function (result) {
                oldData = {
                    CustId: result.CustId + '',
                    UserName: result.Account,
                    LiveBetAccept: result.LiveBetAccept == null ? '' : result.LiveBetAccept + '',
                    SeizeBetTicket: result.SeizeBetTicket == null ? '' : result.SeizeBetTicket + '',
                    LeagueGroupId: result.LeagueGroupId == null ? '' : result.LeagueGroupId + '',
                    MaxBet: result.MaxBet == null ? '' : result.MaxBet + '',
                    MaxPerMatch: result.MaxPerMatch == null ? '' : result.MaxPerMatch + '',
                    FollowAccount: result.FollowAccount == null ? '' : result.FollowAccount,
                };

                updateLeagueGroupItems(result.LeagueGroupId);
                CachedDOMElements.txtPopupAccUsername.val(result.Account);
                CachedDOMElements.hidCustId.val(result.CustId);
                CachedDOMElements.selPopupAccLiveBetAccept.val(result.LiveBetAccept);
                CachedDOMElements.selPopupAccSeizeBetTicket.val(result.SeizeBetTicket).change();
                CachedDOMElements.txtPopupAccMaxBet.val(result.MaxBet);
                CachedDOMElements.txtPopupAccMaxPerMatch.val(result.MaxPerMatch);
                CachedDOMElements.txtPopupAcc8RMAccount.val(result.FollowAccount);
            }
        );
    };

    this.Init = function () {
        MainDOMElements.formAccountManager = $('#formSoccerAccountManager').clone();
        $('#containerFormAccountManager').empty();
    };

    this.ShowAddAccountPopup = function () {
        oldData = null;
        var form = MainDOMElements.formAccountManager.clone();
        CMS.modal('Add Bet Restricted - Soccer Account', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.btnSave.click(function () { return saveAddEditAccount(false); });
    };

    this.ShowEditAccountPopup = function (custId) {
        oldData = null;
        CMS.disableInput();
        var form = MainDOMElements.formAccountManager.clone();
        CMS.modal('Edit Bet Restricted - Soccer Account', form);
        cacheDOMElements();
        bindingEvents();
        loadAccountDetailForEdit(custId);
        CachedDOMElements.txtPopupAccUsername.attr('disabled', 'disabled');
        CachedDOMElements.btnSave.click(function () { return saveAddEditAccount(true); });
    };

    this.RemoveAccount = function (custId, userName) {
        CMS.showConfirmMessage(messages.ConfirmRemoveAccount.replace('{0}', userName), function () {
            CMS.callAjax(
                this,
                'RemoveAccount',
                { CustId: custId },
                function (result) {
                    if (result.Code == 0) {
                        CMS.showSuccessMessage(messages.RemoveAccountSuccessfully);
                        setTimeout(function () {
                            betRestriction.Search();
                        }, 3000);
                    }
                    else {
                        CMS.showErrorMessage(result.Message);
                    }
                }
            );
        });
    };

    function HasSeizeBetTicket() {
        return CachedDOMElements.selPopupAccSeizeBetTicket.val() !== '' && CachedDOMElements.selPopupAccSeizeBetTicket.val() !== '0';
    }

    function updateLeagueGroupItems(leagueGroupIds) {
        var data = getLeagueGroupItems(leagueGroupIds === "All", leagueGroupIds ? leagueGroupIds.split(",") : []);

        CachedDOMElements.multiSelPopupLeagueGroup.multiSelect("updateItems", getMultiSelectOptions(leagueGroupIds === "All", data));

        CachedDOMElements.multiSelPopupLeagueGroup = $('#selPopupLeagueGroup');
    }


    function getLeagueGroupItems(isCheckAll, checkedLeagueGroupIds) {
        var result = [];

        leagueGroups.forEach(function (league) {
            result.push({
                Id: league.Value,
                Value: league.Value,
                Text: league.Text,
                Checked: isCheckAll || checkedLeagueGroupIds.includes(league.Value),
                Enable: true
            });
        });

        return result;
    }

    function getMultiSelectOptions(isCheckAll, listData) {
        return {
            minWidth: 280,
            showTooltip: false,
            shadow: true,
            maxHeight: 120,
            isCheckAll: isCheckAll,
            showFooter: false,
            showFilter: true,
            checkAllText: "All",
            tooltipNoItem: "Please select at least one option",
            listData: listData
        };
    }

    function GetLeagueGroupCheckedItems(selLeagueGroup) {
        return selLeagueGroup.multiSelect('isCheckedAllItems') ? "All" : selLeagueGroup.multiSelect('getCheckedItems').join(',');
    }
};
