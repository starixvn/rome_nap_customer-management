/* global CMS, resources */
(function ($, CMS, resources, helper) {
    "use strict";
    var elements = {
        formMarkNew: $("#formMarkNew"),
        formUnmarkMember: $("#formUnmarkMember"),
        formUnmarkSMA: $("#formUnmarkSMA"),
        formReduceBL: $("#formReduceBL"),
        formInactivateAccount: $("#formInactivateAccount"),
        formRemoveReduceBLMemmber: $("#formRemoveReduceBLMemmber"),
        formRemoveReduceBLSMA: $("#formRemoveReduceBLSMA")
    };

    function bindEvents() {
        $("#btn-markNew").click(function () {
            problemAccountMarkNew.init(this.textContent, elements.formMarkNew.clone());
        });

        $("#btn-unmark").click(unmarkHandler);

        $("#btn-reduceBetLimit").click(reduceBLHandler);

        $("#btn-inactivate").click(inactivateHandler);

        $("#btn-removeReducedBetLimit").click(removeReduceBLHandler);
    }

    function unmarkHandler() {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();

        if (isSelectedAccount(problemAccount.usernames)) {
            var isMember = isMemberLevel(problemAccount.level);
            var form = isMember ? elements.formUnmarkMember.clone() : elements.formUnmarkSMA.clone();

            openModal(resources.unmarkProblemAccount, form, problemAccount);

            if (!isMember) {
                form.find("#divUnmarkNote").tipsy({
                    className: "unmark-note",
                    html: true,
                    opacity: 1,
                    gravity: "w"
                });
            }
        }
    }

    function reduceBLHandler() {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();

        if (isSelectedAccount(problemAccount.usernames)) {
            var form = elements.formReduceBL.clone();

            CMS.modal(this.textContent, form, { resizable: false });
            var maxPerEvent = form.find("#txt-maxPerEvent"),
                maxPerBet = form.find("#txt-maxPerBet");
            helper.showSelectedAccounts(form, problemAccount);
            helper.restrictInputMaxPerEventMaxPerBet(maxPerEvent, maxPerBet);

            helper.bindEvents(form, function () {
                return helper.validateMaxPerEventMaxPerBet(maxPerEvent, maxPerBet);
            });
        }
    }

    function inactivateHandler() {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();

        if (isSelectedAccount(problemAccount.usernames)) {
            openModal(resources.inactivateAccount, elements.formInactivateAccount.clone(), problemAccount);
        }
    }

    function removeReduceBLHandler(event) {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();

        if (isSelectedAccount(problemAccount.usernames)) {
            var form = isMemberLevel(problemAccount.level)
                ? elements.formRemoveReduceBLMemmber
                : elements.formRemoveReduceBLSMA;

            openModal(this.textContent, form.clone(), problemAccount);
        }
    }

    function isSelectedAccount(usernames) {
        return usernames.length > 0;
    }

    function isMemberLevel(level) {
        return level === "1";
    }

    function openModal(modalTitle, form, problemAccount) {
        CMS.modal(modalTitle, form, { resizable: false });

        helper.showSelectedAccounts(form, problemAccount);
        helper.bindEvents(form);
    }

    function initialize() {
        problemAccountSearch.init();
        bindEvents();
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();

        autoReload && problemAccountSearch.reloadData();
    }

    initialize();
})(jQuery, CMS, resources, problemAccountHelper);