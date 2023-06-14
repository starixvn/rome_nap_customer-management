$(document).ready(function () {
    accountManagement = new Fanex.CMS.BetRestrictionTennis.AccountManagement();
    accountManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionTennis.AccountManagement');

Fanex.CMS.BetRestrictionTennis.AccountManagement = function () {
    var oldData = {};

    var DOMElements = {
        formAccountManager: null,
        hidCustId: null,
        txtPopupAccUsername: null,
        selPopupAccLiveBetAccept: null,
        selPopupAccSeizeBetTicket: null,
        txtPopupAcc8RMAccount: null,
        noteInfoMessage: null,
        btnSave: null,
        btnCancel: null,
        htmlformAccountManager: null,
        optsPopupAccLiveBetAccept: null,
        inpPopupAccLiveBetAccept: null,
        spanPopupAccLiveBetAccept: null,
        optsPopupAccSeizeBetTicket: null,
        inpPopupAccSeizeBetTicket: null,
        spanPopupAccSeizeBetTicket: null,
    };

    var cacheDOMElements = function () {
        DOMElements.formAccountManager = $('#formAccountManager');
        DOMElements.hidCustId = $('#hidCustId');
        DOMElements.txtPopupAccUsername = $('#txtPopupAccUsername');
        DOMElements.selPopupAccLiveBetAccept = $('#selPopupAccLiveBetAccept');
        DOMElements.selPopupAccSeizeBetTicket = $('#selPopupAccSeizeBetTicket');
        DOMElements.txtPopupAcc8RMAccount = $('#txtPopupAcc8RMAccount');
        DOMElements.btnSave = $('#btnAccSave');
        DOMElements.btnCancel = $('#btnAccCancel');
    };

    var bindingEvents = function () {
        DOMElements.btnCancel.click(function () {
            CMS.closePopup();
        });
        DOMElements.selPopupAccSeizeBetTicket.change(function () {
            if ($(this).val() == '0' || $(this).val() == 'FollowUpperLevel') {
                DOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(DOMElements.txtPopupAcc8RMAccount);
            } else DOMElements.txtPopupAcc8RMAccount.removeAttr('disabled');
        });
    };

    var validateForm = function () {
        if (!Validator.validateForm(DOMElements.formAccountManager, 'ne')) return false;

        cacheComboboxDOMElements();

        if (
            !validateCombobox(
                DOMElements.inpPopupAccLiveBetAccept,
                DOMElements.optsPopupAccLiveBetAccept,
                DOMElements.selPopupAccLiveBetAccept
            )
        ) {
            CMS.showTipsyError(
                DOMElements.spanPopupAccLiveBetAccept,
                getNotMatchMessage(DOMElements.inpPopupAccLiveBetAccept.val())
            );
            return false;
        }

        if (DOMElements.selPopupAccLiveBetAccept.val() == 'Please select') {
            CMS.showTipsyError(DOMElements.spanPopupAccLiveBetAccept, messages.RequiredFieldError, 'ne');
            return false;
        }

        if (
            !validateCombobox(
                DOMElements.inpPopupAccSeizeBetTicket,
                DOMElements.optsPopupAccSeizeBetTicket,
                DOMElements.selPopupAccSeizeBetTicket
            )
        ) {
            CMS.showTipsyError(
                DOMElements.spanPopupAccSeizeBetTicket,
                getNotMatchMessage(DOMElements.inpPopupAccSeizeBetTicket.val())
            );
            return false;
        }

        if (
            DOMElements.selPopupAccSeizeBetTicket.val() == '' ||
            DOMElements.selPopupAccSeizeBetTicket.val() == 'FollowUpperLevel' ||
            DOMElements.selPopupAccSeizeBetTicket.val() == '0'
        ) {
            DOMElements.txtPopupAcc8RMAccount.attr('disabled', 'disabled');
        } else if (DOMElements.txtPopupAcc8RMAccount.val().length <= 0) {
            CMS.showTipsyError(DOMElements.txtPopupAcc8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if (oldData) {
            var newData = {
                CustId: DOMElements.hidCustId.val(),
                UserName: DOMElements.txtPopupAccUsername.val(),
                LiveBetAccept: DOMElements.selPopupAccLiveBetAccept.val(),
                SeizeBetTicket: DOMElements.selPopupAccSeizeBetTicket.val(),
                FollowAccount: DOMElements.txtPopupAcc8RMAccount.attr('disabled')
                    ? ''
                    : DOMElements.txtPopupAcc8RMAccount.val()
            };

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }
        return true;
    };

    var cacheComboboxDOMElements = function () {
        DOMElements.optsPopupAccLiveBetAccept = $('#selPopupAccLiveBetAccept > option');
        DOMElements.inpPopupAccLiveBetAccept = $('span[id="selPopupAccLiveBetAccept"] > input');
        DOMElements.spanPopupAccLiveBetAccept = $('span[id="selPopupAccLiveBetAccept"]');
        DOMElements.optsPopupAccSeizeBetTicket = $('#selPopupAccSeizeBetTicket > option');
        DOMElements.inpPopupAccSeizeBetTicket = $('span[id="selPopupAccSeizeBetTicket"] > input');
        DOMElements.spanPopupAccSeizeBetTicket = $('span[id="selPopupAccSeizeBetTicket"]');
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
            CustId: DOMElements.hidCustId.val(),
            UserName: DOMElements.txtPopupAccUsername.val(),
            LiveBetAccept: DOMElements.selPopupAccLiveBetAccept.val(),
            SeizeBetTicket: DOMElements.selPopupAccSeizeBetTicket.val(),
            FollowAccount: DOMElements.txtPopupAcc8RMAccount.attr('disabled')
                ? ''
                : DOMElements.txtPopupAcc8RMAccount.val()
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
                FollowAccount: result.FollowAccount == null ? '' : result.FollowAccount,
            };

            DOMElements.txtPopupAccUsername.val(result.Account);
            DOMElements.hidCustId.val(result.CustId);
            DOMElements.selPopupAccLiveBetAccept.val(oldData.LiveBetAccept).combobox('refresh');
            DOMElements.selPopupAccSeizeBetTicket.val(oldData.SeizeBetTicket).combobox('refresh');
            DOMElements.txtPopupAcc8RMAccount.val(result.FollowAccount);

            DOMElements.selPopupAccSeizeBetTicket.trigger('change');
        });
    };

    this.Init = function () {
        DOMElements.htmlformAccountManager = $('#containerFormAccountManager').html(); //.replace('display-none', '');
        $('#containerFormAccountManager').remove();
    };

    this.ShowAddAccountPopup = function () {
        oldData = null;
        CMS.modal('Add Bet Restricted - Tennis Account', DOMElements.htmlformAccountManager);
        cacheDOMElements();
        bindingEvents();
        DOMElements.selPopupAccLiveBetAccept.combobox();
        DOMElements.selPopupAccSeizeBetTicket.combobox();
        DOMElements.btnSave.click(function () {
            return saveAddEditAccount(false);
        });
        DOMElements.formAccountManager.removeClass('display-none');
    };

    this.ShowEditAccountPopup = function (custId) {
        oldData = null;
        CMS.disableInput();
        CMS.modal('Edit Bet Restricted - Tennis Account', DOMElements.htmlformAccountManager);
        $('#popupAccUsernameRequired').remove();
        cacheDOMElements();
        bindingEvents();
        DOMElements.selPopupAccLiveBetAccept.combobox();
        DOMElements.selPopupAccSeizeBetTicket.combobox();
        loadAccountDetailForEdit(custId);
        DOMElements.txtPopupAccUsername.attr('disabled', 'disabled');
        DOMElements.btnSave.click(function () {
            return saveAddEditAccount(true);
        });
        DOMElements.formAccountManager.removeClass('display-none');
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
};
