"use strict";

const creditPayoutVerify = (function ($) {
    const VERIFY_SEARCH = 1,
        VERIFY_MEMBER = 2;

    let formMode = 1,
        verifyMemberForm = null,
        txtUsername = null,
        txtVerifyCode = null,
        txtVerifyPromotionType = null,
        txtVerifyMatch = null,
        txtVerifyExpiredDate = null,
        txtVerifySite = null,
        txtVerifyAmount = null,
        inlineMessage = null,
        cachedPayment = null,
        reloadGridFunction = null,
        typePromotion = null;

    function init(reloadGrid) {
        verifyMemberForm = $("#verifyMemberForm");
        $("#divPopupContent").remove();
        reloadGridFunction = reloadGrid;
    }

    function openModal(mode, type, payment) {
        formMode = mode;
        cachedPayment = payment || {};
        CMS.modal(
            resources.verifyMember,
            verifyMemberForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

        cacheElements(cachedPayment, type);
        bindEvents();
    }

    function cacheElements(payment, type) {
        inlineMessage = $("#inlineMessage");
        typePromotion = type;
        if (isVerifySearch()) {
            $("#row-ExpiredDate").show();
            $(".row-verify-member").remove();
            txtVerifyCode = $("#txtVerifyCode");
            txtUsername = $("#txtVerifyUsername");
        } else {
            $("#row-ExpiredDate").hide();
            txtVerifyCode = $("#txtVerifyCodeMember");
            $(".row-verify-search").remove();
            $("#row-username").show();
            $("#txtVerifyUsername").html(`<b>${payment.Username}</b>`);
        }
        
        txtVerifyPromotionType = $("#txtVerifyPromotionType").html("<b>N/A</b>");
        txtVerifyMatch = $("#txtVerifyMatch").html("<b>N/A</b>");
        txtVerifyExpiredDate = $("#txtVerifyExpiredDate").html("<b>N/A</b>");
        txtVerifyExpiredDate.removeClass("expired");
        txtVerifySite = $("#txtVerifySite").html("<b>N/A</b>");
        txtVerifyAmount = $("#txtVerifyAmount").html("<b>N/A</b>");
        if (payment.DrawID) {
            txtVerifyPromotionType.html(`<b>${type}</b>`);
            txtVerifyMatch.html(`<b>${payment.Match}</b>`);
            if (payment.IsExpired) {
                txtVerifyExpiredDate.addClass("expired");
            } else {
                txtVerifyExpiredDate.removeClass("expired");
            }
            txtVerifyExpiredDate.html(`<b>${payment.ExpiredDate}</b>`);
            txtVerifySite.html(`<b>${payment.Site}</b>`);
            txtVerifyAmount.html(`<b class="amount-cell">${payment.Amount} (${payment.Currency})</b>`);
        }
    }

    function bindEvents() {
        if (isVerifySearch()) {
            $("#btnSearchVerify")
                .off("click")
                .click(function () {
                    clearAllTipsyErrors();
                    validateVerificationCode();
                });
            $("#btnSave").prop("disabled", true).off("click");
        }
        if (isVerifyMember()) {
            $("#btnSave").prop("disabled", false)
                .off("click")
                .click(function () {
                    clearAllTipsyErrors();
                    processVerifyCode();
                });
        }
        
        $("#btnCancel").off("click").click(CMS.closePopup);
    }

    function validateVerificationCode() {

        var params = {
            username: txtUsername.val(),
            verificationCode: txtVerifyCode.val()
        };

        if (params.username === "") {
            CMS.showTipsyError(txtUsername, resources.requiredField, "ne");
            return;
        }
        if (params.verificationCode === "") {
            CMS.showTipsyError(txtVerifyCode, resources.requiredField, "ne");
            return;
        }

        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CreditPayout/ValidateVerifyCode`),
            params,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message && result.Message !== "") {
                    CMS.showInlineErrorMsg(result.Message, inlineMessage);
                    $("#btnSave").prop("disabled", false)
                        .off("click");
                } else {
                    CMS.showInlineSuccessMsg("Valid Verification Code!", inlineMessage, true);
                    showVerifyCode(result);
                }
            });
    }

    function showVerifyCode(data) {
        cachedPayment = data;
        txtVerifyPromotionType.html(`<b>${typePromotion}</b>`);
        txtVerifyMatch.html(`<b>${cachedPayment.Match}</b>`);
        $("#row-ExpiredDate").show();
        txtVerifyExpiredDate.html(`<b>${cachedPayment.ExpiredDate}</b>`);
        if (cachedPayment.IsExpired) {
            txtVerifyExpiredDate.addClass("expired");
        } else {
            txtVerifyExpiredDate.removeClass("expired");
        }

        txtVerifySite.html(`<b>${cachedPayment.Site}</b>`);
        txtVerifyAmount.html(`<b class="amount-cell">${cachedPayment.Amount} (${cachedPayment.Currency})</b>`);
        $("#btnSave").prop("disabled", false)
            .off("click")
            .click(function () {
                clearAllTipsyErrors();
                processVerifyCode();
            });
    }

    function processVerifyCode() {
        clearAllTipsyErrors();
        var usernameList = [{ CustId: cachedPayment.CustID, Username: cachedPayment.Username }];
        var params = {
            UsernameList: usernameList,
            CustIDList: BuildCustIdList(),
            VerificationCode: txtVerifyCode.val()
        };

        if (params.VerificationCode === "") {
            CMS.showTipsyError(txtVerifyCode, resources.requiredField, "ne");
            return;
        }

        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CreditPayout/VerifyCode`),
            params,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Code.toString() !== "0") {
                    CMS.showInlineErrorMsg(result.Message, inlineMessage);
                } else {
                    CMS.showInlineSuccessMsg("Verify Successfully!", inlineMessage, true, function () {
                        CMS.closePopup();
                        reloadGridFunction && reloadGridFunction();
                    }, 1000);
                }
            });
    }

    function BuildCustIdList() {
        return `<Root><Promotion DrawID="${cachedPayment.DrawID}" CustID="${cachedPayment.CustID}"/></Root>`;
    }

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    function isVerifySearch() {
        return formMode === VERIFY_SEARCH;
    }

    function isVerifyMember() {
        return formMode === VERIFY_MEMBER;
    }

    function isUndefined(value) {
        return typeof value === "undefined";
    }

    return {
        init: init,
        openModal: openModal,
        VERIFY_SEARCH: VERIFY_SEARCH,
        VERIFY_MEMBER: VERIFY_MEMBER
    };
})(jQuery);