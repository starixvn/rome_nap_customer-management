$(document).ready(function () {
    var betLimitControl = new Fanex.CMS.BetLimitControl();
    betLimitControl.InitPage();
});

Namespace.Register("Fanex.CMS.BetLimitControl");
Fanex.CMS.BetLimitControl = function () {
    var self = this;

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
        maxPerMatch: null,
        maxBet: null
    }

    this.popupUpdate = {
        btnSave: null,
        btnCancel: null,
        username: null,
        betLimitType: null,
        oldBetLimitType: null,
        maxPerMatch: null,
        maxBet: null,
        custID: null,
        oldBetLimitType: null
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
        this.cachedElements();
        this.InitEventHandler();
    }

    function Search() {
        var params = {
            levelId: self.search.levelId.val(),
            currency: self.search.currency.val(),
            username: self.search.username.val(),
            siteId: self.search.site.val(),
            betLimitTypeID: self.search.betLimitType.val(),
            pageIndex: self.search.pageIndex,
            pageSize: self.search.pageSize
        }
        self.searchWithParams(params)
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
        self.searchWithParams(self.search);
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
                        { HeaderText: 'Upline', Name: 'AgentMaxPerMatch', MergeColNumber: 6 },
                        { HeaderText: 'Action', Name: 'Action', MergeRowNumber: 3 }
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
                { HeaderText: 'Max Bet', Width: 100, Name: 'SuperMaxBet', HeaderAlign: 'center', CellAlign: 'right', AllowTruncate: true },
                { HeaderText: 'Action', Width: 80, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true }
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
            },
        };

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
                        Value: FormatNumber(betLimitType.MaxPerMatch)
                    },
                    {
                        Name: 'MaxBet',
                        Value: FormatNumber(betLimitType.MaxBet)
                    },
                    {
                        Name: 'AgentMaxPerMatch',
                        Value: hasAgent ? FormatNumber(betLimitType.AgentMaxPerMatch) : '',
                        TdClass: hasAgent ? '' : 'cell-disable'
                    },
                    {
                        Name: 'AgentMaxBet',
                        Value: hasAgent ? FormatNumber(betLimitType.AgentMaxBet) : '',
                        TdClass: hasAgent ? '' : 'cell-disable'
                    },
                    {
                        Name: 'MasterMaxPerMatch',
                        Value: hasMaster ? FormatNumber(betLimitType.MasterMaxPerMatch) : '',
                        TdClass: hasMaster ? '' : 'cell-disable'
                    },
                    {
                        Name: 'MasterMaxBet',
                        Value: hasMaster ? FormatNumber(betLimitType.MasterMaxBet) : '',
                        TdClass: hasMaster ? '' : 'cell-disable'
                    },
                    {
                        Name: 'SuperMaxPerMatch',
                        Value: hasSuper ? FormatNumber(betLimitType.SuperMaxPerMatch) : '',
                        TdClass: hasSuper ? '' : 'cell-disable'
                    },
                    {
                        Name: 'SuperMaxBet',
                        Value: hasSuper ? FormatNumber(betLimitType.SuperMaxBet) : '',
                        TdClass: hasSuper ? '' : 'cell-disable'
                    },
                    {
                        Name: 'Action',
                        Value: "<span title='Update' class='ico-edit' data-username='"
                            + betLimitType.Username
                            + "' data-cust-id='"
                            + betLimitType.CustID
                            + "' data-betlimit-type='"
                            + betLimitType.BetLimitTypeID
                            + "' data-maxbet='"
                            + betLimitType.MaxBet
                            + "' data-maxpermatch='"
                            + betLimitType.MaxPerMatch
                            + "'  ></span><span title='Delete' class='ico-delete mid-margin-left' data-cust-id='"
                            + betLimitType.CustID
                            + "' data-betlimit-type='"
                            + betLimitType.BetLimitTypeID
                            + "'></span>"
                    }
                ]);

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
            var maxbet = $this.attr('data-maxbet');
            var maxpermatch = $this.attr('data-maxpermatch');
            self.openUpdateDialog(custId, username, betlimitType, maxbet, maxpermatch);
        });

        this.elements.divBetLimitControl.find(".ico-delete").on("click touchend", function () {
            var custId = $(this).attr("data-cust-id");
            var betLimitTypeId = $(this).attr("data-betlimit-type");
            self.delete(custId, betLimitTypeId);
        });

        this.elements.reportContainer.removeClass("display-none");
    };

    function FormatNumber(number) {
        if (number === null || isNaN(number)) {
            return "N/A";
        } else {
            return numeral(number).format(CMSConstants.NumberFormat.IntNumber);
        }
    }

    function IsPositiveNumber(number) {
        if (number && number >= 0) {
            return true;
        }
        return false;
    }

    function add() {
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
                if (!data.IsSuccess) {
                    CMS.showInlineErrorMsg(data.Message, $('#msgAdd'), false, 15000);
                    return;
                }

                var message = 'New bet limit has been successfully created';
                CMS.showInlineSuccessMsg(message, $('#msgAdd'), true);
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

    function showAddDialog() {
        CMS.modal("New Bet Limit", self.elements.$addDialog.clone(), { resizable: false });

        self.popupAdd.username = $("#txtUsernameAdd");
        self.popupAdd.betLimitType = $("#txtBetLimitTypeAdd");
        self.popupAdd.maxPerMatch = $("#txtMaxPerMatchAdd");
        self.popupAdd.maxBet = $("#txtMaxBetAdd");
        $("#btnSaveAdd").click(add);
        $("#btnCancelAdd").click(CMS.closePopup);
    }

    function update() {
        var betLimitType = self.popupUpdate.betLimitType.val();
        if (!betLimitType || betLimitType === "") {
            CMS.showTipsyError(self.popupUpdate.betLimitType, 'This field is required', "w");
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

                var message = 'Bet limit has been successfully updated.';
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

    this.openUpdateDialog = function (custID, username, betTypeLimit, maxBet, maxPerMatch) {
        CMS.modal("Update Bet Limit", self.elements.$updateDialog.clone(), { resizable: false });

        self.popupUpdate.username = $("#usernameUpdate");
        self.popupUpdate.betLimitType = $("#selBetLimitTypeUpdate");
        self.popupUpdate.maxPerMatch = $("#txtMaxPerMatchUpdate");
        self.popupUpdate.maxBet = $("#txtMaxBetUpdate");

        self.popupUpdate.username.text(username);
        self.popupUpdate.betLimitType.val(betTypeLimit);
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
        CMS.showConfirmMessage("Are you sure to delete?", function () {
            CMS.disableInput();
            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('BetLimitControl/Delete'),
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(request),
                dataType: 'json',
                success: function (data) {
                    if (data.IsSuccess) {
                        CMS.showSuccessMessage("Delete Successfully.")
                        setTimeout(function () {
                            Search();
                        }, 1000);
                    } else {
                        CMS.enableInput();
                        CMS.showErrorMessage(data.Message);
                    }
                },
                error: function (xhr) {
                    CMS.enableInput();
                    CMS.handleAjaxError(xhr);
                }
            });
        })
    }
}