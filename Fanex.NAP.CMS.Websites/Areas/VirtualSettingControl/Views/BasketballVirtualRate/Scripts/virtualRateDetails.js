"use strict";
(function ($, gridEvents) {
    var txtUsername = $("#txt-username");
    var reportContainer = $("#report-container");
    var contentGrid = $("#content-grid");
    var btnAutoSuspendView = $(".btn-autosuspend-view");
    var btnAutoSuspendEdit = $(".btn-autosuspend-edit");
    var oldSports = null;
    // **Associated Accounts**
    var divAssociatedAccounts = $("#div-associated-accounts");
    var chkApplySetting = $("#chk-apply-setting");
    var lblApplyVR = $("#lbl-apply");
    var lblApplyVR_AS = $("#lbl-apply-vr-as");
    var lblApplyVROnly = $("#lbl-apply-vronly");
    var txtAssociatedAccounts = $("#txt-associated-accounts");
    var btnSubmitAssociatedAccounts = $("#btn-submit-associated-accounts");
    var btnSubmitAssociatedAccountsMarkManual = $("#btn-submit-associated-accounts-mark-manual");
    // End of **Associated Accounts**

    function bindEvents() {
        var formSearch = $("#form-search");
        var searchURL = formSearch.attr("action") + "?username=";

        formSearch.submit(function (e) {
            e.preventDefault();
            getVirtualRateDetails(searchURL);
        });

        btnAutoSuspendView.click(function () {
            gridEvents.autoSuspendView();

            btnAutoSuspendView.addClass("display-none")
            btnAutoSuspendEdit.removeClass("display-none");
        });

        btnAutoSuspendEdit.click(function () {
            gridEvents.autoSuspendEdit();

            btnAutoSuspendView.removeClass("display-none")
            btnAutoSuspendEdit.addClass("display-none");
        });

        $(".btn-submit-changes").click(function () {
            showSaveVirtualRateDetailsConfirmation(false);
        });

        $(".btn-submit-changes-and-mark-manual-vr-updated").click(function () {
            showSaveVirtualRateDetailsConfirmation(true);
        });

        // **Associated Accounts**
        chkApplySetting.change(function () {
            disableAssociatedAccountsSection(!this.checked);
        });

        txtAssociatedAccounts.blur(function () {
            var accounts = $.trim(txtAssociatedAccounts.val());

            if (accounts.length) {
                accounts = accounts.replace(/\r\n/g, "\n").replace(/\n/g, ",");
                txtAssociatedAccounts.val(accounts);
            }
        });

        btnSubmitAssociatedAccounts.click(function () {
            validateAssociatedAccounts(false);
        });

        btnSubmitAssociatedAccountsMarkManual.click(function () {
            validateAssociatedAccounts(true);
        });
        // End of **Associated Accounts**

        window.onbeforeunload = function () {
            if (gridEvents.isSportsChanged(oldSports)) {
                return resources.VirtualRateDetails_LeaveSiteWarning;
            }
        };
    }

    function getVirtualRateDetails(url) {
        var username = $.trim(txtUsername.val());

        if (username.length) {
            CMS.getHtml(url + username, function (html) {
                if (html.length) {
                    initVirturalRateDetailsReport(html);
                } else {
                    showTxtUsernameError("This username is not valid.");
                }
            });
        } else {
            showTxtUsernameError(resources.RequiredField);
        }
    }

    function initVirturalRateDetailsReport(html) {
        contentGrid.html(html);

        gridEvents.bindEvents(btnAutoSuspendView, btnAutoSuspendEdit, lblApplyVR_AS, lblApplyVROnly);
        showHideButtonsDependOnReportLevel();
        resetAssociatedAccountsControls();
        oldSports = gridEvents.getSports();

        hideTxtUsernameError();
        $("#spnAutoSuspendNote").tipsy({
            html: true,
            opacity: 0.9,
            gravity: "sw",
            className: "auto-suspend-note"
        });
    }

    function showTxtUsernameError(error) {
        reportContainer.addClass("display-none");
        contentGrid.html("");
        CMS.showTipsyError(txtUsername, error, "n");
    }

    function hideTxtUsernameError() {
        reportContainer.removeClass("display-none");
        CMS.clearTipsyError(txtUsername);
    }

    function showHideButtonsDependOnReportLevel() {
        if (gridEvents.isMemberLevel()) {
            btnAutoSuspendView.addClass("display-none")
            btnAutoSuspendEdit.removeClass("display-none");
        } else {
            btnAutoSuspendView.addClass("display-none")
            btnAutoSuspendEdit.addClass("display-none");
        }
    }

    function showSaveVirtualRateDetailsConfirmation(isManualUpdatedAccount) {
        CMS.showConfirmMessage(
            resources.VirtualRateSubmitChangesConfirm,
            function () {
                saveVirtualRateDetails(isManualUpdatedAccount);
            }
        );
    }

    function saveVirtualRateDetails(isManualUpdatedAccount) {
        CMS.disableInput();
        var newSports = gridEvents.getSports();
        var formData = {
            custId: gridEvents.getCustId(),
            isMember: gridEvents.isMemberLevel(),
            isMemberCompactMode: gridEvents.isMemberLevel() && gridEvents.isCompactMode(),
            isManualUpdate: isManualUpdatedAccount,
            rates: {
                sports: newSports
            }
        };

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/SaveVirtualRateDetails"),
            formData,
            function (result) {
                if (result.IsSuccess) {
                    oldSports = newSports;
                    CMS.showSuccessMessage(result.Message);
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    // Associated Accounts
    function disableAssociatedAccountsSection(disabled) {
        if (disabled) {
            divAssociatedAccounts.attr("disabled", "disabled");
        } else {
            divAssociatedAccounts.removeAttr("disabled");
        }

        txtAssociatedAccounts.prop("disabled", disabled);
        btnSubmitAssociatedAccounts.prop("disabled", disabled);
        btnSubmitAssociatedAccountsMarkManual.prop("disabled", disabled);
    }

    function resetAssociatedAccountsControls() {
        txtAssociatedAccounts.val("");
        chkApplySetting
            .prop("checked", false)
            .trigger("change");

        if (gridEvents.isMemberLevel()) {
            lblApplyVR.addClass("display-none");
            lblApplyVR_AS.removeClass("display-none");
            lblApplyVROnly.addClass("display-none");
        } else {
            lblApplyVR.removeClass("display-none");
            lblApplyVR_AS.addClass("display-none");
            lblApplyVROnly.addClass("display-none");
        }
    }

    function validateAssociatedAccounts(isManualUpdatedAccount) {
        var accounts = $.trim(txtAssociatedAccounts.val());

        if (accounts.length === 0) {
            CMS.showTipsyError(txtAssociatedAccounts, resources.VirtualRate_AssociatedAccounts_Empty, "s");
            return;
        }

        if (accounts.split(",").length > 50) {
            CMS.showTipsyError(txtAssociatedAccounts, resources.VirtualRate_AssociatedAccounts_Error, "s");
            return;
        }

        CMS.clearTipsyError(txtAssociatedAccounts);
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/ValidateAssociatedAccounts"),
            { accounts: accounts },
            function (result) {
                showSaveAssociatedAccountsConfirmation(result, isManualUpdatedAccount);
            });
    }

    function showSaveAssociatedAccountsConfirmation(result, isManualUpdatedAccount) {
        var message = resources.InvalidAccounts + ": " + escapeHtml(result.InvalidAccountsText) + "</br></br>";

        if (result.ValidAccountsText) {
            message += resources.ValidAccounts + ": " + escapeHtml(result.ValidAccountsText) + "</br></br>";
            message += resources.Apply2AssociatedAccounts;

            CMS.showConfirmMessage(
                message,
                function () {
                    saveAssociatedAccounts(result.ValidAccountIdsText, isManualUpdatedAccount);
                });
        } else {
            CMS.showErrorBox(message);
        }
    }

    function saveAssociatedAccounts(accounts, isManualUpdatedAccount) {
        CMS.disableInput();

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/BasketballVirtualRate/SaveAssociatedAccounts"),
            {
                accounts: accounts,
                isMember: gridEvents.isMemberLevel(),
                isMemberCompactMode: gridEvents.isMemberLevel() && gridEvents.isCompactMode(),
                isManualUpdate: isManualUpdatedAccount,
                rates: {
                    sports: gridEvents.getSports()
                }
            },
            function (result) {
                if (result.Code === 0) {
                    CMS.showSuccessMessage(result.Message);
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
    // End of **Associated Accounts**

    $(function () {
        bindEvents();
    });
})(jQuery, virtualRateGridEvents);