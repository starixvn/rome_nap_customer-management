/* global CMS, winLimitControlLabels */
var winLimitMonitor;
$(document).ready(function () {
    winLimitMonitor.initPage();
});

var WinLimitMonitor = function () {
    var self = this;
    self.documentElements = {
        formFilter: null,
        freegrid: null,
        divButtons: null,
        btnUnblocked: null
    };
    self.documentData = {
        searchUrl: '',
        gridOptions: null,
        results: null,
        maxPercentageUsed: 100,
        defaultPageSize: 50,
        searchParameters: {
            PageSize: 1,
            PageIndex: 0
        }
    };

    self.initPage = function () {
        self.cacheElements();
        self.bindEvents();
        self.documentData.gridOptions = self.initGridOptions();
        self.documentData.searchUrl = self.documentElements.formFilter.attr('action');
    };
    self.cacheElements = function () {
        self.documentElements.formFilter = $('#form-filterWinLimit');
        self.documentElements.freegrid = $('#freegrid');
        self.documentElements.divButtons = $('#divButtons');
        self.documentElements.btnUnblocked = $('#btnUnblocked');
        self.documentElements.divResult = $('#div-result-message');
    };
    self.initGridOptions = function () {
        var columns = [
            { HeaderText: '#', Width: 60, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Username, Width: 120, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Currency, Width: 120, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Game, Width: 200, Name: 'colGame', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: winLimitControlLabels.WinLimit, Width: 200, Name: 'colWinLimit',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat
            },
            { HeaderText: winLimitControlLabels.WinningAmount, Width: 200, Name: 'colWinningAmount',
                HeaderAlign: 'center', CellAlign: 'right', FormatNumber: CMS.formats.DecimalFormat
            },
            { HeaderText: winLimitControlLabels.BlockedTime, Width: 140, Name: 'colBlockedTime', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.PercentageUsed, Width: 120, Name: 'colPercentageUsed', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: winLimitControlLabels.Blocked, Width: 60, Name: 'colBlocked', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 25,
            gridExpandHeight: 180,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPage: self.documentData.searchParameters.PageSize,
                currentPage: self.documentData.searchParameters.PageIndex,
                totalItem: 0,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: self.handleChangingPage
            }
        };
    }

    self.getPlayerSettings = function () {
        CMS.hideInlineMsg(self.documentElements.divResult);
        var url = self.documentData.searchUrl + '?' +
            self.documentElements.formFilter.serialize() + "&" +
            $.param({
                PageSize: self.documentData.searchParameters.PageSize,
                PageIndex: self.documentData.searchParameters.PageIndex + 1
            });
        var successFunc = function (results) {
            self.documentData.results = results;
            self.documentData.gridOptions.bodyRows = self.buildBodyRows(results.PlayerWinLimitMonitorModels);
            self.documentData.gridOptions.pagerOption.totalItem = results ? results.TotalPlayers : 0;

            self.documentElements.freegrid.removeClass('display-none').fanexGrid(self.documentData.gridOptions);
            if (self.documentData.gridOptions.pagerOption.totalItem) {
                self.documentElements.divButtons.removeClass('display-none');
            } else {
                self.documentElements.divButtons.addClass('display-none');
            }
        };

        CMS.getJson(url, successFunc);
    };
    self.buildBodyRows = function (results) {
        var bodyRows = [];
        var cell, playerWinLimit, winlimit;
        var mergeRowNumber = 0;
        var resultLength = results ? results.length : 0;

        var startIndex = 0;
        var endIndex = startIndex + self.documentData.searchParameters.PageSize - 1;
        for (var i = startIndex; i < resultLength && i <= endIndex; i++) {
            playerWinLimit = results[i];

            mergeRowNumber = playerWinLimit.GameWinLimitMonitors ? playerWinLimit.GameWinLimitMonitors.length : 0;
            if (mergeRowNumber === 0) {
                continue;
            }

            for (var j = 0; j < mergeRowNumber; j++) {
                cell = [];
                winlimit = playerWinLimit.GameWinLimitMonitors[j];

                if (j === 0) {
                    cell.push({ Name: 'colNumber', Value: (i + 1), MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colUsername', Value: playerWinLimit.PlayerInfo.Name, MergeRowNumber: mergeRowNumber });
                    cell.push({ Name: 'colCurrency', Value: playerWinLimit.Currency.Name, MergeRowNumber: mergeRowNumber });
                }

                var checkBoxId = ' player-index="' + i + '"' +
                    ' game-index="' + j + '"';
                cell.push({ Name: 'colGame', Value: winlimit.Game.Name });
                cell.push({ Name: 'colWinLimit', Value: winlimit.MaxWinLimit });
                cell.push({ Name: 'colWinningAmount', Value: winlimit.WinningAmount });
                cell.push({ Name: 'colBlockedTime', Value: winlimit.BlockedTimeDisplay });
                cell.push({ Name: 'colPercentageUsed', Value: self.getPercentageUsedBox(winlimit.PercentageUsed) });
                cell.push({ Name: 'colBlocked',
                    Value: winlimit.IsBlocked ?
                        '<input type="checkbox" class="checkbox-blocked" checked="checked"' + checkBoxId + '/>' :
                        '<input type="checkbox" class="checkbox-blocked" disabled="disabled"' + checkBoxId + ' />'
                });

                bodyRows.push({ Columns: cell });
            }
        }

        return bodyRows;
    };
    self.getPercentageUsedBox = function (percentageUsed) {
        var html = '';
        var displayPercent = parseInt(percentageUsed);
        if (displayPercent >= self.documentData.maxPercentageUsed) {
            html = '<div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + displayPercent + '%</div>';
        } else {
            html = '<div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
                                (100 - displayPercent) + 'px">' + displayPercent +
                   '%</div>';
        }

        return html;
    };
    self.handleChangingPage = function (pageIndex, itemsPerPage) {
        self.documentData.gridOptions.pagerOption.itemsPerPage = itemsPerPage;
        self.documentData.gridOptions.pagerOption.currentPage = pageIndex;
        self.documentData.searchParameters.PageSize = parseInt(itemsPerPage);
        self.documentData.searchParameters.PageIndex = parseInt(pageIndex);
        self.getPlayerSettings();
    };

    self.bindEvents = function () {
        self.documentElements.formFilter.submit(function (e) {
            e.preventDefault();
            self.resetPagingOptions();
            self.getPlayerSettings();
        });

        self.documentElements.btnUnblocked.hover(
        function () {
            self.checkSelectedAccounts();
        },
        function () {
            setTimeout(function () {
                CMS.clearTipsyError(self.documentElements.btnUnblocked);
                self.documentElements.btnUnblocked.removeAttr('original-title');
            }, 100);
        });

        self.documentElements.btnUnblocked.hover(function () {
            self.checkSelectedAccounts();
        });
    };
    self.resetPagingOptions = function () {
        self.documentData.searchParameters.PageSize = self.documentData.defaultPageSize;
        self.documentData.gridOptions.pagerOption.itemsPerPage = self.documentData.defaultPageSize;
        self.documentData.searchParameters.PageIndex = 0;
        self.documentData.gridOptions.pagerOption.currentPage = 0;
    };
    self.checkSelectedAccounts = function () {
        var numberUnblock = 0;
        $(".checkbox-blocked").each(function () {
            var $this = $(this);
            var isChecked = $this.is(':checked');
            if (!isChecked) {
                numberUnblock++;
            }
        });

        if (numberUnblock == 0) {
            CMS.showTipsyError(self.documentElements.btnUnblocked, winLimitControlLabels.WinLimitMonitor_ChooseOneAccountToUnblock, "w");
            self.documentElements.btnUnblocked.unbind("click");
        } else {
            self.documentElements.btnUnblocked.unbind("click");
            self.documentElements.btnUnblocked.click(self.unblockAccounts);
        }
    };
    self.unblockAccounts = function () {
        CMS.showConfirmMessage(
            winLimitControlLabels.WinLimitMonitor_ConfirmUnblockAccounts,
            function () {
                var updateParams = self.getUnblockParams();
                CMS.callAjax(self, window.rootUrl + "WinLimitControl/WinLimitMonitor/UnblockAccounts", updateParams, self.renderUnblockResult);
            });
    };
    self.getUnblockParams = function () {
        var updateParams = {
            WinLimitMonitor: []
        };

        $(".checkbox-blocked").each(function () {
            var $this = $(this);
            var isChecked = $this.is(':checked');
            if (!isChecked) {
                var playerIndex = parseInt($this.attr("player-index"));
                var gameIndex = parseInt($this.attr("game-index"));
                var player = self.documentData.results.PlayerWinLimitMonitorModels[playerIndex];
                updateParams.WinLimitMonitor.push({
                    PlayerInfo: player.PlayerInfo,
                    Currency: player.Currency,
                    Game: player.GameWinLimitMonitors[gameIndex].Game
                });
            }
        });

        return updateParams;
    };
    self.renderUnblockResult = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.documentElements.divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.documentElements.divResult, true, self.getPlayerSettings);
    };
};

winLimitMonitor = new WinLimitMonitor();