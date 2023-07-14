$(document).ready(function () {
    siteManagement = new Fanex.CMS.BetRestrictionSoccer.SiteManagement();
    siteManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionSoccer.SiteManagement');

Fanex.CMS.BetRestrictionSoccer.SiteManagement = function () {
    var $this = $(this);

    var MainDOMElements = {
        formSiteManagement: null
    };

    var CachedDOMElements = {
        formSiteManagement: null,
        hidSiteId: null,
        txtPopupSite: null,
        ddlPopupSite: null,
        selPopupSiteLiveBetAccept: null,
        selPopupSiteSeizeBetTicket: null,
        txtPopupSite8RMAccount: null,
        btnSiteSave: null,
        btnSiteCancel: null
    };

    var cacheDOMElements = function () {
        CachedDOMElements.formSiteManagement = $('#formSiteManagement');
        CachedDOMElements.hidSiteId = $('#hidSiteId');
        CachedDOMElements.txtPopupSite = $('#txtPopupSite');
        CachedDOMElements.ddlPopupSite = $('#ddlPopupSite');
        CachedDOMElements.selPopupSiteLiveBetAccept = $('#selPopupSiteLiveBetAccept');
        CachedDOMElements.selPopupSiteSeizeBetTicket = $('#selPopupSiteSeizeBetTicket');
        CachedDOMElements.txtPopupSite8RMAccount = $('#txtPopupSite8RMAccount');
        CachedDOMElements.ckbSiteAccumulateBalance = $('#ckbSiteAccumulateBalance');
        CachedDOMElements.btnSiteSave = $('#btnSiteSave');
        CachedDOMElements.btnSiteCancel = $('#btnSiteCancel');
    };

    var bindingEvents = function () {
        CachedDOMElements.btnSiteCancel.click(function () { CMS.closePopup(); });
        CachedDOMElements.ckbSiteAccumulateBalance.change(update8RMAccountStatus);
        CachedDOMElements.selPopupSiteSeizeBetTicket.change(function () {
            if ($(this).val() === '0' || $(this).val() === '') {
                CachedDOMElements.txtPopupSite8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(CachedDOMElements.txtPopupSite8RMAccount);
                CachedDOMElements.ckbSiteAccumulateBalance.attr('disabled', 'disabled');
            }
            else {
                CachedDOMElements.ckbSiteAccumulateBalance.removeAttr('disabled');
                update8RMAccountStatus();
            }
        });

    };

    var update8RMAccountStatus = function () {
        if (CachedDOMElements.selPopupSiteSeizeBetTicket.val() === '0' || CachedDOMElements.selPopupSiteSeizeBetTicket.val() === '' || CachedDOMElements.ckbSiteAccumulateBalance.is(":checked")) {
            CachedDOMElements.txtPopupSite8RMAccount.attr('disabled', 'disabled');
            CMS.clearTipsyError(CachedDOMElements.txtPopupSite8RMAccount);
        } else {
            CachedDOMElements.txtPopupSite8RMAccount.removeAttr('disabled');
        }
    };

    var validateForm = function (isEdit) {
        if (!isEdit && CachedDOMElements.ddlPopupSite.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.ddlPopupSite, messages.RequiredFieldError, 'ne');
            return false;
        }

        if (CachedDOMElements.selPopupSiteLiveBetAccept.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.selPopupSiteLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (CachedDOMElements.selPopupSiteSeizeBetTicket.val() == '0' || CachedDOMElements.ckbSiteAccumulateBalance.is(":checked")) {
            CachedDOMElements.txtPopupSite8RMAccount.attr('disabled', 'disabled');
        }
        else if (CachedDOMElements.txtPopupSite8RMAccount.val().length <= 0) {
            CMS.showTipsyError(CachedDOMElements.txtPopupSite8RMAccount, messages.RequiredFieldError);
            return false;
        }

        return true;
    };

    var saveAddEditSite = function (isEdit) {
        CMS.disableInput();
        if (!validateForm(isEdit)) {
            CMS.enableInput();
            return;
        }

        var params = {
            SiteId: isEdit ? CachedDOMElements.hidSiteId.val() : CachedDOMElements.ddlPopupSite.val(),
            LiveBetAccept: CachedDOMElements.selPopupSiteLiveBetAccept.val(),
            SeizeBetTicket: CachedDOMElements.selPopupSiteSeizeBetTicket.val(),
            FollowAccount: CachedDOMElements.txtPopupSite8RMAccount.attr('disabled') ? '' : CachedDOMElements.txtPopupSite8RMAccount.val(),
            AccumulateBalance: CachedDOMElements.ckbSiteAccumulateBalance.attr('disabled') || !CachedDOMElements.ckbSiteAccumulateBalance.is(":checked") ? 0 : 1
        };

        CMS.callAjax(
            this,
            isEdit ? 'EditSite' : 'AddSite',
            params,
            function (result) {
                if (result.Code == 0) {
                    CMS.showSuccessMessage(isEdit ? messages.UpdateSiteSuccessfully : messages.AddSiteSuccessfully);
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

    var loadSiteDetailForEdit = function (siteId) {
        CMS.callAjax(
            this,
            'GetSiteDetail',
            { SiteId: siteId },
            function (result) {
                CachedDOMElements.hidSiteId.val(result.SiteId);
                CachedDOMElements.txtPopupSite.val(result.Site);
                CachedDOMElements.selPopupSiteLiveBetAccept.val(result.LiveBetAccept);
                CachedDOMElements.txtPopupSite8RMAccount.val(result.FollowAccount);
                CachedDOMElements.ckbSiteAccumulateBalance.prop('checked', result.AccumulateBalance);
                CachedDOMElements.selPopupSiteSeizeBetTicket.val(result.SeizeBetTicket).change();
            }
        );
    };

    this.Init = function () {
        MainDOMElements.formSiteManagement = $('#formSiteManager').clone();
        $('#containerFormSiteManager').empty();
    };

    this.ShowAddSitePopup = function () {
        var form = MainDOMElements.formSiteManagement.clone();
        CMS.modal('Add Bet Restricted Site', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.hidSiteId.remove();
        CachedDOMElements.txtPopupSite.remove();
        CachedDOMElements.btnSiteSave.click(function () { return saveAddEditSite(false); });
    };

    this.ShowEditSitePopup = function (siteId) {
        CMS.disableInput();
        var form = MainDOMElements.formSiteManagement.clone();
        CMS.modal('Edit Bet Restricted Site', form);
        cacheDOMElements();
        bindingEvents();
        CachedDOMElements.ddlPopupSite.remove();
        loadSiteDetailForEdit(siteId);
        CachedDOMElements.btnSiteSave.click(function () { return saveAddEditSite(true); });
    };

    this.RemoveSite = function (siteId, siteName) {
        CMS.showConfirmMessage(messages.ConfirmRemoveSite.replace('{0}', siteName), function () {
            CMS.callAjax(
                this,
                'RemoveSite',
                { SiteId: siteId },
                function (result) {
                    if (result.Code == 0) {
                        CMS.showSuccessMessage(messages.RemoveSiteSuccessfully);
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