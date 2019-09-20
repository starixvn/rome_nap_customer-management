$(document).ready(function () {
    currencyManagement = new Fanex.CMS.BetRestriction.CurrencyManagement();
    currencyManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestriction.CurrencyManagement');

Fanex.CMS.BetRestriction.CurrencyManagement = function () {
    var $this = $(this);

    var oldData = {};

    var DOMElements = {
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
        btnCurrencyCancel: null,
        htmlformCurrencyManagement: null
    };

    var cacheDOMElements = function () {
        DOMElements.formCurrencyManagement = $('#formCurrencyManagement');
        DOMElements.hidExchangeId = $('#hidExchangeId');
        DOMElements.txtPopupCurrency = $('#txtPopupCurrency');
        DOMElements.ddlPopupCurrency = $('#ddlPopupCurrency');
        DOMElements.ddlPopupCurrencyType = $('#ddlPopupCurrencyType');
        DOMElements.hidCurrentType = $('#hidCurrentType');
        DOMElements.selPopupCurrencyLiveBetAccept = $('#selPopupCurrencyLiveBetAccept');
        DOMElements.selPopupCurrencySeizeBetTicket = $('#selPopupCurrencySeizeBetTicket');
        DOMElements.txtPopupCurrency8RMAccount = $('#txtPopupCurrency8RMAccount');
        DOMElements.btnCurrencySave = $('#btnCurrencySave');
        DOMElements.btnCurrencyCancel = $('#btnCurrencyCancel');
    }

    var bindingEvents = function () {
        DOMElements.btnCurrencyCancel.click(function () { CMS.closePopup(); });
        DOMElements.selPopupCurrencySeizeBetTicket.change(function () {
            if ($(this).val() == '0' || $(this).val() == '') {
                DOMElements.txtPopupCurrency8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(DOMElements.txtPopupCurrency8RMAccount);
            }
            else
                DOMElements.txtPopupCurrency8RMAccount.removeAttr('disabled');
        });

    }

    var validateForm = function (isEdit) {
        if (!isEdit && DOMElements.ddlPopupCurrency.val().length <= 0) {
            CMS.showTipsyError(DOMElements.ddlPopupCurrency, messages.RequiredFieldError, 'ne');
            return false;
        }

        if (DOMElements.ddlPopupCurrencyType.val().length <= 0) {
            CMS.showTipsyError(DOMElements.ddlPopupCurrencyType, messages.RequiredFieldError);
            return false;
        }

        if (DOMElements.selPopupCurrencyLiveBetAccept.val() == 'Please select') {
            CMS.showTipsyError(DOMElements.selPopupCurrencyLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (DOMElements.selPopupCurrencySeizeBetTicket.val() == '' || DOMElements.selPopupCurrencySeizeBetTicket.val() == '0') {
            DOMElements.txtPopupCurrency8RMAccount.attr('disabled', 'disabled');
        }
        else if (DOMElements.txtPopupCurrency8RMAccount.val().length <= 0) {
            CMS.showTipsyError(DOMElements.txtPopupCurrency8RMAccount, messages.RequiredFieldError);
            return false;
        }

        if (oldData) {
            var newData = {
                ExchangeId: isEdit ? DOMElements.hidExchangeId.val() : DOMElements.ddlPopupCurrency.val(),
                Type: isEdit ? DOMElements.hidCurrentType.val() : DOMElements.ddlPopupCurrencyType.val(),
                NewType: DOMElements.ddlPopupCurrencyType.val(),
                LiveBetAccept: DOMElements.selPopupCurrencyLiveBetAccept.val(),
                SeizeBetTicket: DOMElements.selPopupCurrencySeizeBetTicket.val(),
                FollowAccount: DOMElements.txtPopupCurrency8RMAccount.attr('disabled') ? '' : DOMElements.txtPopupCurrency8RMAccount.val()
            }

            if (CMS.isValueEqual(oldData, newData)) {
                CMS.showErrorMessage(messages.NothingChanged);
                return false;
            }
        }

        return true;
    }

    var saveAddEditCurrency = function (isEdit) {
        CMS.disableInput();
        if (!validateForm(isEdit)) {
            CMS.enableInput();
            return;
        }

        var params = {
            ExchangeId: isEdit ? DOMElements.hidExchangeId.val() : DOMElements.ddlPopupCurrency.val(),
            Type: isEdit ? DOMElements.hidCurrentType.val() : DOMElements.ddlPopupCurrencyType.val(),
            NewType: DOMElements.ddlPopupCurrencyType.val(),
            LiveBetAccept: DOMElements.selPopupCurrencyLiveBetAccept.val(),
            SeizeBetTicket: DOMElements.selPopupCurrencySeizeBetTicket.val(),
            FollowAccount: DOMElements.txtPopupCurrency8RMAccount.attr('disabled') ? '' : DOMElements.txtPopupCurrency8RMAccount.val()
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
    }

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

                DOMElements.hidExchangeId.val(result.ExchangeId);
                DOMElements.txtPopupCurrency.val(result.Currency);
                DOMElements.ddlPopupCurrency.val(result.ExchangeId);
                DOMElements.ddlPopupCurrencyType.val(result.Type);
                DOMElements.hidCurrentType.val(result.Type);
                DOMElements.selPopupCurrencyLiveBetAccept.val(result.LiveBetAccept);
                DOMElements.selPopupCurrencySeizeBetTicket.val(result.SeizeBetTicket).change();
                DOMElements.txtPopupCurrency8RMAccount.val(result.FollowAccount);
            }
        );
    }

    this.Init = function () {
        DOMElements.htmlformCurrencyManagement = $('#containerFormCurrencyManager').html();//.replace('display-none', '');
        $('#containerFormCurrencyManager').remove();
    }

    this.ShowAddCurrencyPopup = function () {
        CMS.modal('Add Bet Restricted Currency', DOMElements.htmlformCurrencyManagement);
        cacheDOMElements();
        bindingEvents();
        DOMElements.hidExchangeId.remove();
        DOMElements.txtPopupCurrency.remove();
        DOMElements.formCurrencyManagement.removeClass('display-none');
        DOMElements.btnCurrencySave.click(function () { return saveAddEditCurrency(false); });
    }

    this.ShowEditCurrencyPopup = function (exchangeId, type) {
        CMS.disableInput();
        CMS.modal('Edit Bet Restricted Currency', DOMElements.htmlformCurrencyManagement);
        cacheDOMElements();
        bindingEvents();
        DOMElements.ddlPopupCurrency.remove();
        loadCurrencyDetailForEdit(exchangeId, type)
        DOMElements.formCurrencyManagement.removeClass('display-none');
        DOMElements.btnCurrencySave.click(function () { return saveAddEditCurrency(true); });
    }

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
    }
}