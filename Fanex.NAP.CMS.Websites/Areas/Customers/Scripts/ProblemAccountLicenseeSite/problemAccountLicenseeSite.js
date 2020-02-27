(function ($, resources, problemAccountSearch) {
    "use strict";
    var elements = {
        formMarkNew: $("#formMarkNew"),
        formUpdateProblemAccount: $("#formUpdateProblemAccount"),
        formUnmarkMember: $("#formUnmarkMember"),
        formReduceBL: $("#formReduceBL"),
        formRemoveReduceBLMemmber: $("#formRemoveReduceBLMemmber"),
        formInformingStatus: $("#formInformingStatus")
    };

    function bindEvents() {
        $("#btn-markNew").click(function () {
            var markNew = new ProblemAccountLicenseeSiteMarkNew(elements.formMarkNew.clone(), problemAccountSearch.reloadDataForMarkNew);
            markNew.init(this.textContent);
        });

        $("#btn-updateProblemAccount").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames) {
                var updateProblemAccount = new ProblemAccountLicenseeSiteUpdate(elements.formUpdateProblemAccount.clone(), problemAccountSearch.reloadData);
                updateProblemAccount.init(modalTitle, usernames);
            });
        });

        $("#btn-unmark").click(function () {
            handleActions(function (usernames) {
                var unmark = new ProblemAccountConfirmAction(elements.formUnmarkMember.clone(), problemAccountSearch.reloadData);
                unmark.init(resources.unmarkProblemAccount, usernames);
            });
        });

        $("#btn-reduceBetLimit").click(function () {
            var problemAccount = problemAccountSearch.getCheckedProblemAccount();

            if (problemAccount.usernames.length) {
                if (problemAccount.currency.length) {
                    if (problemAccount.isAcknowledgement) {
                        var modalTitle = this.textContent;
                        var reduceBL = new ProblemAccountLicenseeSiteReduceBL(elements.formReduceBL.clone(), problemAccountSearch.reloadData);
                        reduceBL.init(modalTitle, problemAccount.usernames, problemAccount.currency);
                    } else {
                        CMS.showErrorBox(resources.reduceBLNotAcknowledgedSelectedError);
                    }
                } else {
                    CMS.showErrorBox(resources.reduceBLMultipleCurrenciesSelectedError);
                }
            }
        });

        $("#btn-removeReducedBetLimit").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames) {
                var removeReduceBL = new ProblemAccountConfirmAction(elements.formRemoveReduceBLMemmber.clone(), problemAccountSearch.reloadData);
                removeReduceBL.init(modalTitle, usernames);
            });
        });

        $("#btn-informing-status").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames) {
                var updateInformStatus = new ProblemAccountInformingStatusLicenseeSite(elements.formInformingStatus.clone(), problemAccountSearch.reloadData);
                updateInformStatus.init(modalTitle, usernames);
            });
        });
    }

    function handleActions(handleActionCallback) {
        var problemAccount = problemAccountSearch.getCheckedProblemAccount();
        problemAccount.usernames.length && handleActionCallback(problemAccount.usernames);
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
})(jQuery, resources, problemAccountLicenseeSiteSearch);