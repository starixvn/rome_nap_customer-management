(function ($, resources, problemAccountSearch) {
    "use strict";
    var elements = {
        formMarkNew: $("#formMarkNew"),
        formUpdateProblemAccount: $("#formUpdateProblemAccount"),
        formUnmarkMember: $("#formUnmarkMember"),
        formUnmarkSMA: $("#formUnmarkSMA"),
        formReduceBL: $("#formReduceBL"),
        formCloseAccount: $("#formCloseAccount"),
        formRemoveReduceBLMemmber: $("#formRemoveReduceBLMemmber"),
        formRemoveReduceBLSMA: $("#formRemoveReduceBLSMA")
    };

    function bindEvents() {
        $("#btn-markNew").click(function () {
            var markNew = new ProblemAccountCreditSiteMarkNew(elements.formMarkNew.clone(), problemAccountSearch.reloadDataForMarkNew);
            markNew.init(this.textContent);
        });

        $("#btn-updateProblemAccount").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames, level) {
                var updateProblemAccount = new ProblemAccountCreditSiteUpdate(elements.formUpdateProblemAccount.clone(), problemAccountSearch.reloadData);
                updateProblemAccount.init(modalTitle, usernames, level);
            });
        });

        $("#btn-unmark").click(function () {
            handleActions(function (usernames, level) {
                var unmark = new ProblemAccountUnmark(getUnmarkForm(level).clone(), problemAccountSearch.reloadData);
                unmark.init(resources.unmarkProblemAccount, usernames, level);
            });
        });

        $("#btn-reduceBetLimit").click(function () {
            var problemAccount = problemAccountSearch.getCheckedProblemAccount();

            if (problemAccount.usernames.length) {
                if (problemAccount.currency.length) {
                    var modalTitle = this.textContent;
                    var reduceBL = new ProblemAccountCreditSiteReduceBL(elements.formReduceBL.clone(), problemAccountSearch.reloadData);
                    reduceBL.init(modalTitle, problemAccount.usernames, problemAccount.level, problemAccount.currency);
                } else {
                    CMS.showErrorBox(resources.reduceBLMultipleCurrenciesSelectedError);
                }
            }
        });

        $("#btn-closeAccount").click(function () {
            handleActions(function (usernames, level) {
                var closeAccount = new ProblemAccountConfirmAction(elements.formCloseAccount.clone(), problemAccountSearch.reloadData);
                closeAccount.init(resources.closeAccount, usernames, level);
            });
        });

        $("#btn-removeReducedBetLimit").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames, level) {
                var removeReduceBL = new ProblemAccountConfirmAction(getRemoveReducedBLForm(level).clone(), problemAccountSearch.reloadData);
                removeReduceBL.init(modalTitle, usernames, level);
            });
        });
    }

    function handleActions(handleActionCallback) {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();
        problemAccount.usernames.length && handleActionCallback(problemAccount.usernames, problemAccount.level);
    }

    function getUnmarkForm(level) {
        return isMemberLevel(level) ? elements.formUnmarkMember : elements.formUnmarkSMA;
    }

    function getRemoveReducedBLForm(level) {
        return isMemberLevel(level) ? elements.formRemoveReduceBLMemmber : elements.formRemoveReduceBLSMA;
    }

    function isMemberLevel(level) {
        return level === "1";
    }

    function initialize() {
        problemAccountSearch.init();
        bindEvents();
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();

        if (autoReload) {
            setTimeout(function () {
                problemAccountSearch.autoReloadData(searchFilter.PageIndex, searchFilter.PageSize);
            }, 200);
        }
    }

    initialize();
})(jQuery, resources, problemAccountCreditSiteSearch);