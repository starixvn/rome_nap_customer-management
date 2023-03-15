$(document).ready(function () {
    currencyManagement = new Fanex.CMS.BetRestrictionSoccer.CurrencyManagement();
    currencyManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionSoccer.CurrencyManagement');

Fanex.CMS.BetRestrictionSoccer.CurrencyManagement = function () {
    var $this = $(this);

    var oldData = {};

    var MainDOMElements = {
        formCurrencyManagement: null
    };

    var CachedDOMElements = {
        formCurrencyManagement: null,
        hidExchangeId: null,
        txtPopupCurrency: null,
        ddlPopupCurrency: null,
        ddlPopupCurrencyType: null,
        hidCurrentType: null,
        selPopupCurrencyLiveBetAccept: null,
        selPopupCurrencySeizeBetTicket: null,
        txtPopupCurrency8RMAccount: null,
        btnCurrencySave: null,
        btnCurrencyCancel: null
    };

    var cacheDOMElements = function () {
        CachedDOMElements.formCurrencyManagement = $('#formCurrencyManagement');
        CachedDOMElements.hidExchangeId = $('#hidExchangeId');
        CachedDOMElements.txtPopupCurrency = $('#txtPopupCurrency');
        CachedDOMElements.ddlPopupCurrency = $('#ddlPopupCurrency');
        CachedDOMElements.ddlPopupCurrencyType = $('#ddlPopupCurrencyType');
        CachedDOMElements.hidCurrentType = $('#hidCurrentType');
        CachedDOMElements.selPopupCurrencyLiveBetAccept = $('#selPopupCurrencyLiveBetAccept');
        CachedDOMElements.selPopupCurrencySeizeBetTicket = $('#selPopupCurrencySeizeBetTicket');
        CachedDOMElements.txtPopupCurrency8RMAccount = $('#txtPopupCurrency8RMAccount');
        CachedDOMElements.btnCurrencySave = $('#btnCurrencySave');
        CachedDOMElements.btnCurrencyCancel = $('#btnCurrencyCancel');
    };

    var bindingEvents = function () {
        CachedDOMElements.btnCurrencyCancel.click(function () { CMS.closePopup(); });
        CachedDOMElements.selPopupCurrencySeizeBetTicket.change(function () {
            if ($(this).val() == '0' || $(this).val() == '') {
                CachedDOMElements.txtPopupCurrency8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(CachedDOMElements.txtPopupCurrency8RMAccount);
            }
            else
                CachedDOMElements.txtPopupCurrency8RMAccount.removeAttr('disabled');
        });

    };

    var validateForm = function (isEdit) {
        if (!isEdit && CachedDOMElements.ddlPopupCurrency.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.ddlPopupCurrency, messages.RequiredFieldError, 'ne');
            return false;
        }

        if (CachedDOMElements.ddlPopupCurrencyType.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.ddlPopupCurrencyType, messages.RequiredFieldError);
            return false;
        }

        if (CachedDOMElements.selPopupCurrencyLiveBetAccept.val() == 'Please select') {
            CMS.showTipsyError(CachedDOMElements.selPopupCurrencyLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (CachedDOMElements.selPopupCurrencySeizeBetTicket.val() == '' || CachedDOMElements.selPopupCurrencySeizeBetTicket.val() == '0') {
            CachedDOMElements.txtPopupCurrency8RMAccount.attr('disabled', 'disabled');
        }
        else if (CachedDOMElements.txtPopupCurrency8RMAccount.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.txtPopupCurrency8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if (oldData) {
            var newData = {
                ExchangeId: isEdit ? CachedDOMElements.hidExchangeId.val() : CachedDOMElements.ddlPopupCurrency.val(),
                Type: isEdit ? CachedDOMElements.hidCurrentType.val() : CachedDOMElements.ddlPopupCurrencyType.val(),
                NewType: CachedDOMElements.ddlPopupCurrencyType.val(),
                LiveBetAccept: CachedDOMElements.selPopupCurrencyLiveBetAccept.val(),
                SeizeBetTicket: CachedDOMElements.selPopupCurrencySeizeBetTicket.val(),
                FollowAccount: CachedDOMElements.txtPopupCurrency8RMAccount.attr('disabled') ? '' : CachedDOMElements.txtPopupCurrency8RMAccount.val()
            };

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }

        return true;
    };

    var saveAddEditCurrency = function (isEdit) {
        CMS.disableInput();
        if (!validateForm(isEdit)) {
            CMS.enableInput();
            return;
        }

        var params = {
            ExchangeId: isEdit ? CachedDOMElements.hidExchangeId.val() : CachedDOMElements.ddlPopupCurrency.val(),
            Type: isEdit ? CachedDOMElements.hidCurrentType.val() : CachedDOMElements.ddlPopupCurrencyType.val(),
            NewType: CachedDOMElements.ddlPopupCurrencyType.val(),
            LiveBetAccept: CachedDOMElements.selPopupCurrencyLiveBetAccept.val(),
            SeizeBetTicket: CachedDOMElements.selPopupCurrencySeizeBetTicket.val(),
            FollowAccount: CachedDOMElements.txtPopupCurrency8RMAccount.attr('disabled') ? '' : CachedDOMElements.txtPopupCurrency8RMAccount.val()
        }

        CMS.callAjax(
            this,
            isEdit ? 'EditCurrency' : 'AddCurrency',
            params,
            function (result) {
                if (result.Code == 0) {
                    CMS.showSuccessMessage(isEdit ? messages.UpdateCurrencySuccessfully : messages.AddCurrencySuccessfully);
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

    var loadCurrencyDetailForEdit = function (exchangeId, type) {
        CMS.callAjax(
            this,
            'GetCurrencyDetail',
            { ExchangeId: exchangeId, Type: type },
            function (result) {
                oldData = {
                    ExchangeId: result.ExchangeId + '',
                    Type: result.Type + '',
                    NewType: result.Type + '',
                    LiveBetAccept: result.LiveBetAccept + '',
                    SeizeBetTicket: result.SeizeBetTicket == null ? '' : result.SeizeBetTicket,
                    FollowAccount: result.FollowAccount == null ? '' : result.FollowAccount
                };

                CachedDOMElements.hidExchangeId.val(result.ExchangeId);
                CachedDOMElements.txtPopupCurrency.val(result.Currency);
                CachedDOMElements.ddlPopupCurrency.val(result.ExchangeId);
                CachedDOMElements.ddlPopupCurrencyType.val(result.Type);
                CachedDOMElements.hidCurrentType.val(result.Type);
                CachedDOMElements.selPopupCurrencyLiveBetAccept.val(result.LiveBetAccept);
                CachedDOMElements.selPopupCurrencySeizeBetTicket.val(result.SeizeBetTicket).change();
                CachedDOMElements.txtPopupCurrency8RMAccount.val(result.FollowAccount);
            }
        );
    };

    this.Init = function () {
        MainDOMElements.formCurrencyManagement = $('#formCurrencyManager').clone();
        $('#containerFormCurrencyManager').empty();
    };

    this.ShowAddCurrencyPopup = function () {
        var form = MainDOMElements.formCurrencyManagement.clone();
        CMS.modal('Add Bet Restricted Currency', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.hidExchangeId.remove();
        CachedDOMElements.txtPopupCurrency.remove();
        CachedDOMElements.btnCurrencySave.click(function () { return saveAddEditCurrency(false); });
    };

    this.ShowEditCurrencyPopup = function (exchangeId, type) {
        CMS.disableInput();
        var form = MainDOMElements.formCurrencyManagement.clone();
        CMS.modal('Edit Bet Restricted Currency', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.ddlPopupCurrency.remove();
        loadCurrencyDetailForEdit(exchangeId, type);
        CachedDOMElements.btnCurrencySave.click(function () { return saveAddEditCurrency(true); });
    };

    this.RemoveCurrency = function (exchangeId, currency, type) {
        CMS.showConfirmMessage(messages.ConfirmRemoveCurrency.replace('{0}', currency), function () {
            CMS.callAjax(
                this,
                'RemoveCurrency',
                { ExchangeId: exchangeId, Type: type },
                function (result) {
                    if (result.Code == 0) {
                        CMS.showSuccessMessage(messages.RemoveCurrencySuccessfully);
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
};