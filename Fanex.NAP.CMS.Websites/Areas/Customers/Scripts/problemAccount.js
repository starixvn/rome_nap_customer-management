(function ($, resources) {
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
            var markNew = new ProblemAccountMarkNew(elements.formMarkNew.clone(), problemAccountSearch.reloadDataForMarkNew);
            markNew.init(this.textContent);
        });

        $("#btn-unmark").click(function () {
            handleActions(function (usernames, level) {
                var unmark = new ProblemAccountUnmark(getUnmarkForm(level).clone(), problemAccountSearch.reloadData);
                unmark.init(resources.unmarkProblemAccount, usernames, level);
            });
        });

        $("#btn-reduceBetLimit").click(function () {
            var modalTitle = this.textContent;

            handleActions(function (usernames, level) {
                var reduceBL = new ProblemAccountReduceBL(elements.formReduceBL.clone(), problemAccountSearch.reloadData);
                reduceBL.init(modalTitle, usernames, level);
            });
        });

        $("#btn-inactivate").click(function () {
            handleActions(function (usernames, level) {
                var inactivate = new ProblemAccountConfirmAction(elements.formInactivateAccount.clone(), problemAccountSearch.reloadData);
                inactivate.init(resources.inactivateAccount, usernames, level);
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
        problemAccount.usernames.length > 0 && handleActionCallback(problemAccount.usernames, problemAccount.level);
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
})(jQuery, resources);