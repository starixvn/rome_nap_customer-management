$(document).ready(function () {
    siteManagement = new Fanex.CMS.BetRestrictionSoccer.SiteManagement();
    siteManagement.Init();
});

Namespace.Register('Fanex.CMS.BetRestrictionSoccer.SiteManagement');

Fanex.CMS.BetRestrictionSoccer.SiteManagement = function () {
    var $this = $(this);

    var DOMElements = {
        formSiteManagement: null,
        hidSiteId: null,
        txtPopupSite: null,
        ddlPopupSite: null,
        selPopupSiteLiveBetAccept: null,
        selPopupSiteSeizeBetTicket: null,
        txtPopupSite8RMAccount: null,
        btnSiteSave: null,
        btnSiteCancel: null,
        htmlformSiteManagement: null
    };

    var cacheDOMElements = function () {
        DOMElements.formSiteManagement = $('#formSiteManagement');
        DOMElements.hidSiteId = $('#hidSiteId');
        DOMElements.txtPopupSite = $('#txtPopupSite');
        DOMElements.ddlPopupSite = $('#ddlPopupSite');
        DOMElements.selPopupSiteLiveBetAccept = $('#selPopupSiteLiveBetAccept');
        DOMElements.selPopupSiteSeizeBetTicket = $('#selPopupSiteSeizeBetTicket');
        DOMElements.txtPopupSite8RMAccount = $('#txtPopupSite8RMAccount');
        DOMElements.btnSiteSave = $('#btnSiteSave');
        DOMElements.btnSiteCancel = $('#btnSiteCancel');
    }

    var bindingEvents = function () {
        DOMElements.btnSiteCancel.click(function () { CMS.closePopup(); });
        DOMElements.selPopupSiteSeizeBetTicket.change(function () {
            if ($(this).val() == '0') {
                DOMElements.txtPopupSite8RMAccount.attr('disabled', 'disabled');
                CMS.clearTipsyError(DOMElements.txtPopupSite8RMAccount);
            }
            else
                DOMElements.txtPopupSite8RMAccount.removeAttr('disabled');
        });

    }

    var validateForm = function (isEdit) {
        if (!isEdit && DOMElements.ddlPopupSite.val().length <= 0) {
            CMS.showTipsyError(DOMElements.ddlPopupSite, messages.RequiredFieldError, 'ne');
            return false;
        }

        if (DOMElements.selPopupSiteLiveBetAccept.val().length <= 0) {
            CMS.showTipsyError(DOMElements.selPopupSiteLiveBetAccept, messages.RequiredFieldError);
            return false;
        }

        if (DOMElements.selPopupSiteSeizeBetTicket.val() == '0') {
            DOMElements.txtPopupSite8RMAccount.attr('disabled', 'disabled');
        }
        else if (DOMElements.txtPopupSite8RMAccount.val().length <= 0) {
            CMS.showTipsyError(DOMElements.txtPopupSite8RMAccount, messages.RequiredFieldError);
            return false;
        }

        return true;
    }

    var saveAddEditSite = function (isEdit) {
        CMS.disableInput();
        if (!validateForm(isEdit)) {
            CMS.enableInput();
            return;
        }

        var params = {
            SiteId: isEdit ? DOMElements.hidSiteId.val() : DOMElements.ddlPopupSite.val(),
            LiveBetAccept: DOMElements.selPopupSiteLiveBetAccept.val(),
            SeizeBetTicket: DOMElements.selPopupSiteSeizeBetTicket.val(),
            FollowAccount: DOMElements.txtPopupSite8RMAccount.attr('disabled') ? '' :   DOMElements.txtPopupSite8RMAccount.val()
        }

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
    }

    var loadSiteDetailForEdit = function (siteId) {
        CMS.callAjax(
            this,
            'GetSiteDetail',
            { SiteId: siteId },
            function (result) {
                DOMElements.hidSiteId.val(result.SiteId);
                DOMElements.txtPopupSite.val(result.Site);
                DOMElements.selPopupSiteLiveBetAccept.val(result.LiveBetAccept);
                DOMElements.selPopupSiteSeizeBetTicket.val(result.SeizeBetTicket).change();
                DOMElements.txtPopupSite8RMAccount.val(result.FollowAccount);
            }
        );
    }

    this.Init = function () {
        DOMElements.htmlformSiteManagement = $('#containerFormSiteManager').clone();//.replace('display-none', '');
    }

    this.ShowAddSitePopup = function () {
        CMS.modal('Add Bet Restricted Site', DOMElements.htmlformSiteManagement);
        cacheDOMElements();
        bindingEvents();
        DOMElements.hidSiteId.remove();
        DOMElements.txtPopupSite.remove();
        DOMElements.formSiteManagement.removeClass('display-none');
        DOMElements.btnSiteSave.click(function () { return saveAddEditSite(false); });
    }

    this.ShowEditSitePopup = function (siteId) {
        CMS.disableInput();
        CMS.modal('Edit Bet Restricted Site', DOMElements.htmlformSiteManagement);
        cacheDOMElements();
        bindingEvents();
        DOMElements.ddlPopupSite.remove();
        loadSiteDetailForEdit(siteId)
        DOMElements.formSiteManagement.removeClass('display-none');
        DOMElements.btnSiteSave.click(function () { return saveAddEditSite(true); });
    }

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
    }
}