$(document).ready(function () {
    accountManagement = new Fanex.CMS.BetRestrictionBasketball.AccountManagement();
    accountManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionBasketball.AccountManagement');

Fanex.CMS.BetRestrictionBasketball.AccountManagement = function () {
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
        txtPopupAcc8RMAccount: null,
        noteInfoMessage: null,
        btnSave: null,
        btnCancel: null,
        optsPopupAccLiveBetAccept: null,
        inpPopupAccLiveBetAccept: null,
        spanPopupAccLiveBetAccept: null,
        optsPopupAccSeizeBetTicket: null,
        inpPopupAccSeizeBetTicket: null,
        spanPopupAccSeizeBetTicket: null
    };

    var cacheDOMElements = function () {
        CachedDOMElements.formAccountManager = $('#formAccountManager');
        CachedDOMElements.hidCustId = $('#hidCustId');
        CachedDOMElements.txtPopupAccUsername = $('#txtPopupAccUsername');
        CachedDOMElements.selPopupAccLiveBetAccept = $('#selPopupAccLiveBetAccept');
        CachedDOMElements.selPopupAccSeizeBetTicket = $('#selPopupAccSeizeBetTicket');
        CachedDOMElements.txtPopupAcc8RMAccount = $('#txtPopupAcc8RMAccount');
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
        CachedDOMElements.btnCancel.click(function () {
            CMS.closePopup();
        });
        CachedDOMElements.selPopupAccSeizeBetTicket.change(function () {
            if ($(this).val() == '0' || $(this).val() == 'FollowUpperLevel') {
                CachedDOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
                CachedDOMElements.multiSelPopupLeagueGroup.multiSelect("disable");
                CMS.clearTipsyError(CachedDOMElements.txtPopupAcc8RMAccount);
                CMS.clearTipsyError(CachedDOMElements.multiSelPopupLeagueGroup);
            } else {
                CachedDOMElements.txtPopupAcc8RMAccount.removeAttr('disabled');
                CachedDOMElements.multiSelPopupLeagueGroup.multiSelect("enable");
            }
        });
    };

    var validateForm = function () {
        if (!Validator.validateForm(CachedDOMElements.formAccountManager, 'ne')) return false;

        cacheComboboxCachedDOMElements();

        if (
            !validateCombobox(
                CachedDOMElements.inpPopupAccLiveBetAccept,
                CachedDOMElements.optsPopupAccLiveBetAccept,
                CachedDOMElements.selPopupAccLiveBetAccept
            )
        ) {
            CMS.showTipsyError(
                CachedDOMElements.spanPopupAccLiveBetAccept,
                getNotMatchMessage(CachedDOMElements.inpPopupAccLiveBetAccept.val())
            );
            return false;
        }

        if (CachedDOMElements.selPopupAccLiveBetAccept.val() == 'Please select') {
            CMS.showTipsyError(CachedDOMElements.spanPopupAccLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (
            !validateCombobox(
                CachedDOMElements.inpPopupAccSeizeBetTicket,
                CachedDOMElements.optsPopupAccSeizeBetTicket,
                CachedDOMElements.selPopupAccSeizeBetTicket
            )
        ) {
            CMS.showTipsyError(
                CachedDOMElements.spanPopupAccSeizeBetTicket,
                getNotMatchMessage(CachedDOMElements.inpPopupAccSeizeBetTicket.val())
            );
            return false;
        }

        if (!HasSeizeBetTicket()) {
            CachedDOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
            CachedDOMElements.multiSelPopupLeagueGroup.multiSelect('disable');
        } else if (CachedDOMElements.multiSelPopupLeagueGroup.multiSelect('getCheckedItems').length === 0) {
            CMS.showTipsyError(CachedDOMElements.multiSelPopupLeagueGroup, messages.RequiredFieldError);
            return false;
        } else if (CachedDOMElements.txtPopupAcc8RMAccount.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.txtPopupAcc8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if (oldData) {
            var newData = {
                CustId: CachedDOMElements.hidCustId.val(),
                UserName: CachedDOMElements.txtPopupAccUsername.val(),
                LiveBetAccept: CachedDOMElements.selPopupAccLiveBetAccept.val(),
                SeizeBetTicket: CachedDOMElements.selPopupAccSeizeBetTicket.val(),
                LeagueGroupId: HasSeizeBetTicket() ? GetLeagueGroupCheckedItems(CachedDOMElements.multiSelPopupLeagueGroup) : '',
                FollowAccount: HasSeizeBetTicket() ? CachedDOMElements.txtPopupAcc8RMAccount.val() : ''
            };

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }
        return true;
    };

    var cacheComboboxCachedDOMElements = function () {
        CachedDOMElements.optsPopupAccLiveBetAccept = $('#selPopupAccLiveBetAccept > option');
        CachedDOMElements.inpPopupAccLiveBetAccept = $('span[id="selPopupAccLiveBetAccept"] > input');
        CachedDOMElements.spanPopupAccLiveBetAccept = $('span[id="selPopupAccLiveBetAccept"]');
        CachedDOMElements.optsPopupAccSeizeBetTicket = $('#selPopupAccSeizeBetTicket > option');
        CachedDOMElements.inpPopupAccSeizeBetTicket = $('span[id="selPopupAccSeizeBetTicket"] > input');
        CachedDOMElements.spanPopupAccSeizeBetTicket = $('span[id="selPopupAccSeizeBetTicket"]');
    };

    var validateCombobox = function (input, options, select) {
        var optValue = getOptionValue(input, options);
        if (!optValue) {
            return false;
        }

        if (input.val() !== select.text) {
            select.val(optValue).combobox('refresh');
        }

        return true;
    };

    var getOptionValue = function (input, options) {
        var value = null;
        options.each(function () {
            if (input.val() === this.text) {
                value = this.value;
                return;
            }
        });

        return value;
    };

    var getNotMatchMessage = function (text) {
        return text + " didn't match any item.";
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
        CMS.callAjax(this, isEdit ? 'EditAccount' : 'AddAccount', params, function (result) {
            if (result.Code == 0) {
                CMS.showSuccessMessage(
                    isEdit ? messages.UpdateAccountSuccessfully : messages.AddAccountSuccessfully
                );
                setTimeout(function () {
                    CMS.closePopup();
                    betRestriction.Search();
                }, 3000);
            } else {
                CMS.showErrorMessage(result.Message);
            }
        });
    };

    var loadAccountDetailForEdit = function (custId) {
        CMS.callAjax(this, 'GetAccountDetail', { CustId: custId }, function (result) {
            oldData = {
                CustId: result.CustId + '',
                UserName: result.Account,
                LiveBetAccept:
                    result.LiveBetAccept == null ? 'FollowUpperLevel' : result.LiveBetAccept + '',
                SeizeBetTicket:
                    result.SeizeBetTicket == null ? 'FollowUpperLevel' : result.SeizeBetTicket + '',
                LeagueGroupId:
                    result.LeagueGroupId == null ? '' : result.LeagueGroupId + '',
                FollowAccount: result.FollowAccount == null ? '' : result.FollowAccount
            };

            updateLeagueGroupItems(result.LeagueGroupId);
            CachedDOMElements.txtPopupAccUsername.val(result.Account);
            CachedDOMElements.hidCustId.val(result.CustId);
            CachedDOMElements.selPopupAccLiveBetAccept.val(oldData.LiveBetAccept).combobox('refresh').trigger('change');
            CachedDOMElements.txtPopupAcc8RMAccount.val(result.FollowAccount);
            CachedDOMElements.selPopupAccSeizeBetTicket.val(oldData.SeizeBetTicket).combobox('refresh').trigger('change');
        });
    };

    this.Init = function () {
        MainDOMElements.formAccountManager = $('#formAccountManager').clone();
        $('#containerFormAccountManager').empty();
    };

    this.ShowAddAccountPopup = function () {
        oldData = null;
        var form = MainDOMElements.formAccountManager.clone();
        CMS.modal('Add Bet Restricted - Basketball Account', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.selPopupAccLiveBetAccept.combobox();
        CachedDOMElements.selPopupAccSeizeBetTicket.combobox();
        CachedDOMElements.btnSave.click(function () {
            return saveAddEditAccount(false);
        });
    };

    this.ShowEditAccountPopup = function (custId) {
        oldData = null;
        CMS.disableInput();
        var form = MainDOMElements.formAccountManager.clone();
        CMS.modal('Edit Bet Restricted - Basketball Account', form);
        $('#popupAccUsernameRequired').remove();
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.selPopupAccLiveBetAccept.combobox();
        CachedDOMElements.selPopupAccSeizeBetTicket.combobox();
        loadAccountDetailForEdit(custId);
        CachedDOMElements.txtPopupAccUsername.attr('disabled', 'disabled');
        CachedDOMElements.btnSave.click(function () {
            return saveAddEditAccount(true);
        });
    };

    this.RemoveAccount = function (custId, userName) {
        CMS.showConfirmMessage(messages.ConfirmRemoveAccount.replace('{0}', userName), function () {
            CMS.callAjax(this, 'RemoveAccount', { CustId: custId }, function (result) {
                if (result.Code == 0) {
                    CMS.showSuccessMessage(messages.RemoveAccountSuccessfully);
                    setTimeout(function () {
                        betRestriction.Search();
                    }, 3000);
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
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
                Id: "basketball_lg_" + league.Value,
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
