/* File Created: Oct 03, 2014 */
/* global Namespace, 
Fanex, 
viewResources,
companyTurnover,
CMSConstants,
numeral */
/* jshint strict: false,
camelcase: false,
unused: vars  */
var companyTurnoverManager;
$(document).ready(function () {
    companyTurnoverManager.initPage();
});

Namespace.Register("Fanex.Nap.CMS.TurnoverControl.TurnoverMonitor.CompanyTurnoverManager");
Fanex.Nap.CMS.TurnoverControl.TurnoverMonitor.CompanyTurnoverManager = function () {
    var self = this;
    self.configurations = {
        css: {
            displayNone: 'display-none'
        }
    };
    self.domElements = {
        turnoverBalance: null
    };
    self.initPage = function () {
        self.cacheDOMElements();
        self.renderTurnoverBalance(companyTurnover);
    };
    self.cacheDOMElements = function () {
        self.domElements.turnoverBalance = $("#turnover-balance");
    };
    self.renderTurnoverBalance = function (companyTurnover) {
        var bodyData = [];
        var balanceLength = companyTurnover ? companyTurnover.length : 0;
        for (var i = 0; i < balanceLength; i++) {
            var turnoverBalance = companyTurnover[i];

            var balanceArray = [];
            for (var j = 0; j < turnoverBalance.TurnoverAmount.length; j++) {
                balanceArray.push(numeral(turnoverBalance.TurnoverAmount[j]).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            }

            bodyData.push({
                Columns: [{ Name: "ColBetType", Value: turnoverBalance.Name },
                {
                    Name: "ColBalance",
                    Value: '<div class="div-balance"' +
                           (balanceArray.length ? ' balance-format="' + turnoverBalance.BalanceFormat + '"' : '') +
                           '>' +
                           (balanceArray.length ? balanceArray.join(" - ") : '&nbsp;') +
                        '</div>',
                    TdClass: 'column-balance'
                }]
            });
        }

        var gridOptions = {
            columns: [
            { HeaderText: viewResources.Bet_Type, Width: 160, Name: "ColBetType", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: viewResources.Balance, Width: 400, Name: "ColBalance", HeaderAlign: "center", CellAlign: "center"}],
            showTitle: true,
            titleInfo: {
                text: viewResources.Balance_Info
            },
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: false,
            fullHeight: false,
            gridWidth: 900,
            gridHeight: 400,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25
        };

        self.domElements.turnoverBalance.removeClass(self.configurations.css.displayNone).fanexGrid(gridOptions);
        self.domElements.turnoverBalance.find(".column-balance").each(function () {

        });

        var balanceColumns = self.domElements.turnoverBalance.find(".column-balance");
        balanceColumns.hover(function () {
            var divBalance = $(this).find("div[balance-format]");
            divBalance.attr("title", divBalance.attr("balance-format"));
            divBalance.tipsy({
                trigger: 'manual',
                gravity: 'w',
                classStyle: 'error',
                inDocument: false,
                opacity: 0.97
            });

            divBalance.tipsy('show').triggerHandler('raiseError');
            var tipContainer = $(".error-tipsy.error-tipsy-w");
            tipContainer.css("left", "443px");
        },
        function () {
            var divBalance = $(this).find("div[balance-format]");
            divBalance.tipsy('hide');
            divBalance.removeAttr('original-title').triggerHandler('clearError');
        });
    };
};

companyTurnoverManager = new Fanex.Nap.CMS.TurnoverControl.TurnoverMonitor.CompanyTurnoverManager();