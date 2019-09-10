$(document).ready(function () {
    accountManagement = new Fanex.CMS.BetRestriction.AccountManagement();
    accountManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestriction.AccountManagement');

Fanex.CMS.BetRestriction.AccountManagement = function () {
    var $this = $(this);

    var oldData = {};

    var DOMElements = {
        formAccountManager: null,
        hidCustId: null,
        txtPopupAccUsername: null,
        chkPopupAccLiveBetAccept: null,
        chkPopupAccSeizeBetTicket: null,
        txtPopupAccMaxBet: null,
        txtPopupAccMaxPerMatch: null,
        txtPopupAcc8RMAccount: null,
        noteInfoMessage: null,
        btnSave: null,
        btnCancel: null,
        htmlformAccountManager: null
    };

    var cacheDOMElements = function () {
        DOMElements.formAccountManager = $('#formAccountManager');
        DOMElements.hidCustId = $('#hidCustId');
        DOMElements.txtPopupAccUsername = $('#txtPopupAccUsername');
        DOMElements.chkPopupAccLiveBetAccept = $('#chkPopupAccLiveBetAccept');
        DOMElements.chkPopupAccSeizeBetTicket = $('#chkPopupAccSeizeBetTicket');
        DOMElements.txtPopupAccMaxBet = $('#txtPopupAccMaxBet');
        DOMElements.txtPopupAccMaxPerMatch = $('#txtPopupAccMaxPerMatch');
        DOMElements.txtPopupAcc8RMAccount = $('#txtPopupAcc8RMAccount');
        DOMElements.noteInfoMessage = $('#noteInfoMessage');
        DOMElements.btnSave = $('#btnAccSave');
        DOMElements.btnCancel = $('#btnAccCancel');
    }

    var bindingEvents = function () {
        DOMElements.txtPopupAccMaxBet.keydown(function (e) { return CMS.forceNumericInput(e, true); });
        DOMElements.txtPopupAccMaxBet.on("cut copy paste", function (e) {
            e.preventDefault();
        });
        DOMElements.txtPopupAccMaxPerMatch.keydown(function (e) { return CMS.forceNumericInput(e, true); });
        DOMElements.txtPopupAccMaxPerMatch.on("cut copy paste", function (e) {
            e.preventDefault();
        });
        DOMElements.btnCancel.click(function () { CMS.closePopup(); });
        DOMElements.noteInfoMessage.click(function () { CMS.showInfoMessage(messages.NoteInfoMaxBet); });
        DOMElements.chkPopupAccSeizeBetTicket.change(function () {
            if ($(this).val() == '') {
                DOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(DOMElements.txtPopupAcc8RMAccount);
            }
            else
                DOMElements.txtPopupAcc8RMAccount.removeAttr('disabled');
        });

    }

    var validateForm = function () {

        if (!Validator.validateForm(DOMElements.formAccountManager, 'ne'))
            return false;

        if (DOMElements.chkPopupAccLiveBetAccept.val().length <= 0) {
            CMS.showTipsyError(DOMElements.chkPopupAccLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (DOMElements.chkPopupAccSeizeBetTicket.val() == '') {
            DOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
        }
        else if (DOMElements.txtPopupAcc8RMAccount.val().length <= 0) {
            CMS.showTipsyError(DOMElements.txtPopupAcc8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if ((DOMElements.txtPopupAccMaxBet.val().length > 0 || DOMElements.txtPopupAccMaxPerMatch.val().length > 0)
            && DOMElements.txtPopupAccMaxBet.val() > DOMElements.txtPopupAccMaxPerMatch.val()) {
            CMS.showTipsyError(DOMElements.txtPopupAccMaxBet, messages.ValidateMaxBetMaxPerMatchError);
            return false;
        }

        if (oldData) {
            var newData = {
                CustId: DOMElements.hidCustId.val(),
                UserName: DOMElements.txtPopupAccUsername.val(),
                LiveBetAccept: DOMElements.chkPopupAccLiveBetAccept.val(),
                SeizeBetTicket: DOMElements.chkPopupAccSeizeBetTicket.val(),
                MaxBet: DOMElements.txtPopupAccMaxBet.val(),
                MaxPerMatch: DOMElements.txtPopupAccMaxPerMatch.val(),
                FollowAccount: DOMElements.txtPopupAcc8RMAccount.attr('disabled') ? '' : DOMElements.txtPopupAcc8RMAccount.val()
            }

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }

        return true;
    }

    var saveAddEditAccount = function (isEdit) {
        CMS.disableInput();
        if (!validateForm()) {
            CMS.enableInput();
            return;
        }

        var params = {
            CustId: DOMElements.hidCustId.val(),
            UserName: DOMElements.txtPopupAccUsername.val(),
            LiveBetAccept: DOMElements.chkPopupAccLiveBetAccept.val(),
            SeizeBetTicket: DOMElements.chkPopupAccSeizeBetTicket.val(),
            MaxBet: DOMElements.txtPopupAccMaxBet.val(),
            MaxPerMatch: DOMElements.txtPopupAccMaxPerMatch.val(),
            FollowAccount: DOMElements.txtPopupAcc8RMAccount.attr('disabled') ? '' : DOMElements.txtPopupAcc8RMAccount.val()
        }

        CMS.callAjax(
            this,
            isEdit ? 'CheckEditAccount' : 'CheckAddAccount',
            params,
            function (result) {
                if (result.Code > 0) {
                    CMS.showConfirmMessage('<span style="white-space: pre-line">' + result.Message + '</span>', function () { return saveAccount(params, isEdit); });
                }
                else {
                    saveAccount(params, isEdit);
                }
            }
        );
    }

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
    }

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
                    MaxBet: result.MaxBet == null ? '' : result.MaxBet + '',
                    MaxPerMatch: result.MaxPerMatch == null ? '' : result.MaxPerMatch + '',
                    FollowAccount: result.FollowAccount == null ? '' : result.FollowAccount,
                };

                DOMElements.txtPopupAccUsername.val(result.Account);
                DOMElements.hidCustId.val(result.CustId);
                DOMElements.chkPopupAccLiveBetAccept.val(result.LiveBetAccept);
                DOMElements.chkPopupAccSeizeBetTicket.val(result.SeizeBetTicket).change();
                DOMElements.txtPopupAccMaxBet.val(result.MaxBet);
                DOMElements.txtPopupAccMaxPerMatch.val(result.MaxPerMatch);
                DOMElements.txtPopupAcc8RMAccount.val(result.FollowAccount);
            }
        );
    }

    this.Init = function () {
        DOMElements.htmlformAccountManager = $('#containerFormAccountManager').html();//.replace('display-none', '');
        $('#containerFormAccountManager').remove();
    }

    this.ShowAddAccountPopup = function () {
        oldData = null;
        CMS.modal('Add Bet Restricted Account', DOMElements.htmlformAccountManager);
        cacheDOMElements();
        bindingEvents();
        DOMElements.btnSave.click(function () { return saveAddEditAccount(false); });
        DOMElements.formAccountManager.removeClass('display-none');
    }

    this.ShowEditAccountPopup = function (custId) {
        oldData = null;
        CMS.disableInput();
        CMS.modal('Edit Bet Restricted Account', DOMElements.htmlformAccountManager);
        cacheDOMElements();
        bindingEvents();
        loadAccountDetailForEdit(custId);
        DOMElements.txtPopupAccUsername.attr('disabled', 'disabled');
        DOMElements.btnSave.click(function () { return saveAddEditAccount(true); });
        DOMElements.formAccountManager.removeClass('display-none');
    }

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
    }
}