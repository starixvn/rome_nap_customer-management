$(document).ready(function () {
    var betLimitControl = new Fanex.CMS.BetLimitControl();
    betLimitControl.InitPage();
});

Namespace.Register("Fanex.CMS.BetLimitControl");
Fanex.CMS.BetLimitControl = function () {
    var self = this;

    this.isHost = false;

    this.elements = {
        divBetLimitControl: null,
        reportContainer: null,
        $addDialog: null,
        $updateDialog: null
    };

    this.search = {
        levelId: null,
        username: null,
        currency: null,
        site: null,
        betLimitType: null,
        pageIndex: 1,
        pageSize: 50
    }

    this.popupAdd = {
        btnSave: null,
        btnCancel: null,
        username: null,
        betLimitType: null,
        betLimitMode: null,
        maxPerMatch: null,
        maxBet: null,
        msgAddError: null,
        msgAddSuccess: null
    }

    this.popupUpdate = {
        btnSave: null,
        btnCancel: null,
        username: null,
        betLimitType: null,
        oldBetLimitType: null,
        betLimitMode: null,
        oldBetLimitMode: null,
        maxPerMatch: null,
        maxBet: null,
        custID: null
    }

    this.cachedElements = function () {
        this.elements.divBetLimitControl = $("#div-bet-limit-control");
        this.elements.reportContainer = $("#report-container");
        this.elements.$addDialog = $("#frmAdd");
        this.elements.$updateDialog = $("#frmUpd");

        this.search.levelId = $("#selectLevel");
        this.search.currency = $("#selectCurrency");
        this.search.site = $("#selectSite");
        this.search.betLimitType = $("#selectBetLimitType");
        this.search.username = $("#selectUsername");

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#div-popup").remove();
    };

    this.InitEventHandler = function () {
        $('#btnSearch').click(Search);
        $('#btnAdd').click(showAddDialog);
    }

    this.InitPage = function () {
        this.isHost = $('.filter-bar').attr('isHost') === 'True';
        this.cachedElements();
        this.InitEventHandler();
    }

    function Search() {
        self.search.pageIndex = 1;
        var params = {
            levelId: self.search.levelId.val(),
            currency: self.search.currency.val(),
            username: $.trim(self.search.username.val()),
            siteId: self.search.site.val(),
            betLimitTypeID: self.search.betLimitType.val(),
            pageIndex: self.search.pageIndex,
            pageSize: self.search.pageSize
        };
        self.searchWithParams(params);
    }

    this.searchWithParams = function (params) {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitControl/Search'),
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(params),
            dataType: 'json',
            success: function (data) {
                self.renderBetLimit(data);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    this.changePager = function (pageIndex, itemsPerPage) {
        self.search.pageSize = itemsPerPage;
        self.search.pageIndex = pageIndex + 1;
        var params = {
            levelId: self.search.levelId.val(),
            currency: self.search.currency.val(),
            username: $.trim(self.search.username.val()),
            siteId: self.search.site.val(),
            betLimitTypeID: self.search.betLimitType.val(),
            pageIndex: self.search.pageIndex,
            pageSize: self.search.pageSize
        };
        self.searchWithParams(params);
    };

    this.renderBetLimit = function (rs) {
        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        { HeaderText: 'Username', Name: 'Username', MergeRowNumber: 3 },
                        { HeaderText: 'VIP', Name: 'VIP', MergeRowNumber: 3 },
                        { HeaderText: 'Level', Name: 'Level', MergeRowNumber: 3 },
                        { HeaderText: 'Currency', Name: 'Currency', MergeRowNumber: 3 },
                        { HeaderText: 'Site', Name: 'Site', MergeRowNumber: 3 },
                        { HeaderText: 'Bet Limit Type', Name: 'BetLimitType', MergeRowNumber: 3 },
                        { HeaderText: 'Selected Account', Name: 'MaxPerMatch', MergeRowNumber: 2, MergeColNumber: 2 },
                        { HeaderText: 'Upline', Name: 'AgentMaxPerMatch', MergeColNumber: 6 }
                    ]
                },
                {
                    Columns: [
                        { HeaderText: 'Agent', Name: 'AgentMaxPerMatch', MergeColNumber: 2 },
                        { HeaderText: 'Master', Name: 'MasterMaxPerMatch', MergeColNumber: 2 },
                        { HeaderText: 'Super', Name: 'SuperMaxPerMatch', MergeColNumber: 2 }
                    ]
                },
            ],
            columns: [
                { HeaderText: "Username", Width: 140, Name: 'Username', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'VIP', Width: 80, Name: 'VIP', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Level', Width: 80, Name: 'Level', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Currency', Width: 80, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Site', Width: 80, Name: 'Site', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Bet Limit Type', Width: 140, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 100, Name: 'MaxPerMatch', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 100, Name: 'MaxBet', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 100, Name: 'AgentMaxPerMatch', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 100, Name: 'AgentMaxBet', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 100, Name: 'MasterMaxPerMatch', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 100, Name: 'MasterMaxBet', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 100, Name: 'SuperMaxPerMatch', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 100, Name: 'SuperMaxBet', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true }
            ],
            showDistinctRow: false,
            showPager: true,
            customPager: true,
            showTitle: false,
            pagerOption: {
                itemsPerPage: self.search.pageSize,
                currentPage: self.search.pageIndex - 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: self.changePager
            },
            gridExpandHeight: 200,
            titleInfo: {
                text: "",
                showExcelButton: false
            }
        };

        if (self.isHost) {
            gridOptions.groupHeaderRows[0].Columns.push({ HeaderText: 'Action', Name: 'Action', MergeRowNumber: 3 });
            gridOptions.columns.push({ HeaderText: 'Action', Width: 80, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' });
        }

        var bodyData = [];
        $.each(rs.CustomerBetLimitControls, function (_, userBetLimit) {
            var isFirstColumn = true;
            $.each(userBetLimit, function (_, betLimitType) {
                var currentColumms = [];
                var isVIPCustomer = betLimitType.VIP;
                if (isFirstColumn) {
                    var rowMerge = userBetLimit.length;
                    currentColumms = currentColumms.concat([
                        {
                            Name: 'Username',
                            Value: betLimitType.Username,
                            TdClass: 'break-word',
                            MergeRowNumber: rowMerge
                        },
                        {
                            Name: 'VIP',
                            Value: betLimitType.VIP ? 'Yes' : 'No',
                            MergeRowNumber: rowMerge
                        },
                        {
                            Name: 'Level',
                            Value: betLimitType.Level,
                            MergeRowNumber: rowMerge
                        },
                        {
                            Name: 'Currency',
                            Value: betLimitType.Currency,
                            MergeRowNumber: rowMerge
                        },
                        {
                            Name: 'Site',
                            Value: betLimitType.Site,
                            MergeRowNumber: rowMerge
                        }
                    ]);
                }

                var hasAgent = betLimitType.Level === "Member";
                var hasMaster = betLimitType.Level !== "Master" && betLimitType.Level !== "Super";
                var hasSuper = betLimitType.Level !== "Super";
                currentColumms = currentColumms.concat([
                    {
                        Name: 'BetLimitType',
                        Value: betLimitType.BetLimitType
                    },
                    {
                        Name: 'MaxPerMatch',
                        Value: FormatNumber(betLimitType.MaxPerMatch, betLimitType.Mode)
                    },
                    {
                        Name: 'MaxBet',
                        Value: FormatNumber(betLimitType.MaxBet, betLimitType.Mode)
                    },
                    {
                        Name: 'AgentMaxPerMatch',
                        Value: hasAgent ? FormatNumber(betLimitType.AgentMaxPerMatch, betLimitType.Mode) : '',
                        TdClass: hasAgent ? '' : 'cell-disable'
                    },
                    {
                        Name: 'AgentMaxBet',
                        Value: hasAgent ? FormatNumber(betLimitType.AgentMaxBet, betLimitType.Mode) : '',
                        TdClass: hasAgent ? '' : 'cell-disable'
                    },
                    {
                        Name: 'MasterMaxPerMatch',
                        Value: hasMaster ? FormatNumber(betLimitType.MasterMaxPerMatch, betLimitType.Mode) : '',
                        TdClass: hasMaster ? '' : 'cell-disable'
                    },
                    {
                        Name: 'MasterMaxBet',
                        Value: hasMaster ? FormatNumber(betLimitType.MasterMaxBet, betLimitType.Mode) : '',
                        TdClass: hasMaster ? '' : 'cell-disable'
                    },
                    {
                        Name: 'SuperMaxPerMatch',
                        Value: hasSuper ? FormatNumber(betLimitType.SuperMaxPerMatch, betLimitType.Mode) : '',
                        TdClass: hasSuper ? '' : 'cell-disable'
                    },
                    {
                        Name: 'SuperMaxBet',
                        Value: hasSuper ? FormatNumber(betLimitType.SuperMaxBet, betLimitType.Mode) : '',
                        TdClass: hasSuper ? '' : 'cell-disable'
                    }
                ]);

                if (self.isHost) {
                    currentColumms.push(
                        {
                            Name: 'Action',
                            Value: "<span title='Update' class='ico-edit' data-username='"
                                + betLimitType.Username
                                + "' data-cust-id='"
                                + betLimitType.CustID
                                + "' data-betlimit-type='"
                                + betLimitType.BetLimitTypeID
                                + "' data-mode='"
                                + betLimitType.Mode
                                + "' data-maxbet='"
                                + betLimitType.MaxBet
                                + "' data-maxpermatch='"
                                + betLimitType.MaxPerMatch
                                + "'  ></span><span title='Delete' class='ico-delete mid-margin-left' data-cust-id='"
                                + betLimitType.CustID
                                + "' data-betlimit-type='"
                                + betLimitType.BetLimitTypeID
                                + "'></span>"
                        });
                }

                bodyData.push(
                    {
                        Columns: currentColumms,
                        CssClass: isVIPCustomer ? 'row-vip-customer' : ''
                    });
                isFirstColumn = false;
            });
        });

        gridOptions.bodyRows = bodyData;
        gridOptions.pagerOption.totalItem = rs.TotalItems;

        this.elements.divBetLimitControl.fanexGrid(gridOptions);

        this.elements.divBetLimitControl.find(".ico-edit").on("click touchend", function () {
            var $this = $(this);
            var custId = $(this).attr("data-cust-id");
            var username = $this.attr('data-username');
            var betlimitType = $this.attr('data-betlimit-type');
            var mode = $this.attr('data-mode');
            var maxbet = $this.attr('data-maxbet');
            var maxpermatch = $this.attr('data-maxpermatch');
            self.openUpdateDialog(custId, username, betlimitType, mode, maxbet, maxpermatch);
        });

        this.elements.divBetLimitControl.find(".ico-delete").on("click touchend", function () {
            var custId = $(this).attr("data-cust-id");
            var betLimitTypeId = $(this).attr("data-betlimit-type");
            self.delete(custId, betLimitTypeId);
        });

        this.elements.reportContainer.removeClass("display-none");
    };

    function FormatNumber(number, mode) {
        var fixedMode = 1;
        var percentageMode = 2;
        if (number === null || isNaN(number)) {
            return "N/A";
        } else {
            switch (mode) {
                case fixedMode:
                    return numeral(number).format(CMSConstants.NumberFormat.IntNumber);
                case percentageMode:
                    return 'x ' + numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber2);
                default:
                    return '';
            }
        }
    }

    function IsPositiveNumber(number) {
        if (number && number >= 0) {
            return true;
        }
        return false;
    }

    function add() {
        CMS.clearTipsyError(self.popupAdd.username);
        CMS.clearTipsyError(self.popupAdd.betLimitType);
        CMS.clearTipsyError(self.popupAdd.betLimitMode);
        CMS.clearTipsyError(self.popupAdd.maxPerMatch);
        CMS.clearTipsyError(self.popupAdd.maxBet);
        CMS.hideInlineMsg(self.popupAdd.msgAddError);
        CMS.hideInlineMsg(self.popupAdd.msgAddSuccess);

        var username = $.trim(self.popupAdd.username.val());
        if (!username) {
            CMS.showTipsyError(self.popupAdd.username, 'This field is required', "w");
            return;
        }

        var betLimitType = self.popupAdd.betLimitType.val();
        if (!betLimitType || betLimitType === "") {
            CMS.showTipsyError(self.popupAdd.betLimitType, 'This field is required', "w");
            return;
        }

        var betLimitMode = self.popupAdd.betLimitMode.val();
        if (!betLimitMode || betLimitMode === "") {
            CMS.showTipsyError(self.popupAdd.betLimitMode, 'This field is required', "w");
            return;
        }

        var maxPerMatchString = self.popupAdd.maxPerMatch.val();
        if (!maxPerMatchString) {
            CMS.showTipsyError(self.popupAdd.maxPerMatch, 'This field is required', "w");
            return;
        }

        var maxPerMatch = numeral().unformat(maxPerMatchString);
        if (!IsPositiveNumber(maxPerMatch)) {
            CMS.showTipsyError(self.popupAdd.maxPerMatch, 'Please input a positive whole number.', "w");
            return;
        }

        var maxBetString = self.popupAdd.maxBet.val();
        if (!maxBetString) {
            CMS.showTipsyError(self.popupAdd.maxBet, 'This field is required', "w");
            return;
        }

        var maxBet = numeral().unformat(maxBetString);
        if (!IsPositiveNumber(maxBet)) {
            CMS.showTipsyError(self.popupAdd.maxBet, 'Please input a positive whole number.', "w");
            return;
        }

        if (maxBet > maxPerMatch) {
            CMS.showTipsyError(self.popupAdd.maxPerMatch, 'Max Per Match must be greater than or equal to Max Bet.', "w");
            return;
        }

        var request = {
            username: username,
            betLimitTypeID: betLimitType,
            mode: parseInt(betLimitMode),
            maxBet: maxBet,
            maxPerMatch: maxPerMatch
        }
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitControl/Add'),
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            dataType: 'json',
            success: function (data) {
                if (isAddAllResultsSuccessfully(data)) {
                    CMS.showInlineSuccessMsg('New Bet Limit has been successfully created.', self.popupAdd.msgAddSuccess, true);
                    setTimeout(function () {
                        CMS.closePopup();
                        Search();
                    }, 3000);
                } else {
                    var errorMessage = [];
                    var successMessage = '';
                    for (var i = 0, maxLength = data.length; i < maxLength; i++) {
                        if (data[i].IsSuccess) {
                            successMessage = data[i].Message + 'New Bet Limit has been successfully created.';
                        } else {
                            errorMessage.push(data[i].Message);
                        }
                    }

                    if (errorMessage.length) {
                        CMS.showInlineErrorMsg(errorMessage.join('<br/>'), self.popupAdd.msgAddError);
                    }

                    if (successMessage.length) {
                        CMS.showInlineSuccessMsg(successMessage, self.popupAdd.msgAddSuccess, false);
                    }
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    function isAddAllResultsSuccessfully(results) {
        for (var i = 0, max = results.length; i < max; i++) {
            if (!results[i].IsSuccess) {
                return false;
            }
        }

        return true;
    }

    function showAddDialog() {
        CMS.modal("New Bet Limit", self.elements.$addDialog.clone(), { resizable: false });

        self.popupAdd.username = $("#txtUsernameAdd");
        self.popupAdd.username.off("change").on("change", function () {
            var usernamesValue = $.trim(self.popupAdd.username.val());

            if (usernamesValue.length > 0) {
                if (usernamesValue.indexOf("\n") >= 0) {
                    usernamesValue = usernamesValue.replace(/\n/g, " ");
                }

                if (usernamesValue.indexOf("\r") >= 0) {
                    usernamesValue = usernamesValue.replace(/\r/g, " ");
                }

                if (usernamesValue.indexOf("\t") >= 0) {
                    usernamesValue = usernamesValue.replace(/\t/g, " ");
                }

                usernamesValue = usernamesValue.replace(/\s/g, ",");
                var usernamesArray = usernamesValue.split(",").filter(item => item.length > 0);

                self.popupAdd.username.val(usernamesArray.join(","));
            }
        });

        self.popupAdd.betLimitType = $("#txtBetLimitTypeAdd");
        self.popupAdd.betLimitMode = $("#txtBetLimitModeAdd");
        self.popupAdd.maxPerMatch = $("#txtMaxPerMatchAdd");
        self.popupAdd.maxBet = $("#txtMaxBetAdd");
        self.popupAdd.msgAddError = $("#msgAdd");
        self.popupAdd.msgAddSuccess = $("#msgAddSuccess");

        $("#btnSaveAdd").click(add);
        $("#btnCancelAdd").click(CMS.closePopup);
    }

    function update() {
        CMS.clearTipsyError(self.popupUpdate.betLimitType);
        CMS.clearTipsyError(self.popupUpdate.betLimitMode);
        CMS.clearTipsyError(self.popupUpdate.maxPerMatch);
        CMS.clearTipsyError(self.popupUpdate.maxBet);

        var betLimitType = self.popupUpdate.betLimitType.val();
        if (!betLimitType || betLimitType === "") {
            CMS.showTipsyError(self.popupUpdate.betLimitType, 'This field is required', "w");
            return;
        }

        var betLimitMode = self.popupUpdate.betLimitMode.val();
        if (!betLimitMode || betLimitMode === "") {
            CMS.showTipsyError(self.popupUpdate.betLimitMode, 'This field is required', "w");
            return;
        }

        var maxPerMatchString = self.popupUpdate.maxPerMatch.val();
        if (!maxPerMatchString) {
            CMS.showTipsyError(self.popupUpdate.maxPerMatch, 'This field is required', "w");
            return;
        }

        var maxPerMatch = numeral().unformat(maxPerMatchString);
        if (!IsPositiveNumber(maxPerMatch)) {
            CMS.showTipsyError(self.popupUpdate.maxPerMatch, 'Please input a positive whole number.', "w");
            return;
        }

        var maxBetString = self.popupUpdate.maxBet.val();
        if (!maxBetString) {
            CMS.showTipsyError(self.popupUpdate.maxBet, 'This field is required', "w");
            return;
        }

        var maxBet = numeral().unformat(maxBetString);
        if (!IsPositiveNumber(maxBet)) {
            CMS.showTipsyError(self.popupUpdate.maxBet, 'Please input a positive whole number.', "w");
            return;
        }

        if (maxBet > maxPerMatch) {
            CMS.showTipsyError(self.popupUpdate.maxPerMatch, 'Max Per Match must be greater than or equal to Max Bet.', "w");
            return;
        }

        var request = {
            custID: self.popupUpdate.custID,
            mode: parseInt(betLimitMode),
            maxBet: maxBet,
            maxPerMatch: maxPerMatch,
            betLimitTypeID: self.popupUpdate.oldBetLimitType,
            newBetLimitTypeID: betLimitType
        }

        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitControl/Update'),
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(request),
            dataType: 'json',
            success: function (data) {
                if (!data.IsSuccess) {
                    CMS.showInlineErrorMsg(data.Message, $('#msgUpd'), false, 15000);
                    return;
                }

                var message = 'Bet Limit has been successfully updated.';
                CMS.showInlineSuccessMsg(message, $('#msgUpd'), true);
                setTimeout(function () {
                    CMS.closePopup();
                    Search();
                }, 3000);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    this.openUpdateDialog = function (custID, username, betTypeLimit, mode, maxBet, maxPerMatch) {
        CMS.modal("Update Bet Limit", self.elements.$updateDialog.clone(), { resizable: false });

        self.popupUpdate.username = $("#usernameUpdate");
        self.popupUpdate.betLimitType = $("#selBetLimitTypeUpdate");
        self.popupUpdate.betLimitMode = $("#selBetLimitModeUpdate");
        self.popupUpdate.maxPerMatch = $("#txtMaxPerMatchUpdate");
        self.popupUpdate.maxBet = $("#txtMaxBetUpdate");

        self.popupUpdate.username.text(username);
        self.popupUpdate.betLimitType.val(betTypeLimit);
        self.popupUpdate.betLimitMode.val(mode);
        self.popupUpdate.maxBet.val(numeral(maxBet).format(CMSConstants.NumberFormat.IntNumber));
        self.popupUpdate.maxPerMatch.val(numeral(maxPerMatch).format(CMSConstants.NumberFormat.IntNumber));
        self.popupUpdate.oldBetLimitType = betTypeLimit;
        self.popupUpdate.custID = custID;
        $("#btnSaveUpdate").click(update);
        $("#btnCancelUpdate").click(CMS.closePopup);
    }

    this.delete = function (custID, betLimitTypeID) {
        var request = {
            custID: custID,
            betLimitTypeID: betLimitTypeID
        }
        CMS.showConfirmMessage("Do you want to delete the selected Bet Limit?", function () {
            CMS.disableInput();
            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('BetLimitControl/Delete'),
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(request),
                dataType: 'json',
                success: function (data) {
                    if (data.IsSuccess) {
                        CMS.showSuccessMessage("Bet Limit has been successfully deleted.")
                        setTimeout(function () {
                            Search();
                        }, 1000);
                    } else {
                        CMS.showErrorMessage(data.Message);
                    }
                },
                error: function (xhr) {
                    CMS.handleAjaxError(xhr);
                },
                complete: function () {
                    CMS.enableInput();
                }
            });
        })
    }
}