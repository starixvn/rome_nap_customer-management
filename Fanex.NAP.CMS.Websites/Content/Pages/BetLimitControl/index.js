$(document).ready(function () {
    var betLimitControl = new Fanex.CMS.BetLimitControl();
    betLimitControl.InitPage();

    $('.edit-betlimitcontrol').off();
    $('.edit-betlimitcontrol').click(function () {
        var $this = $(this);
        var username = $this.attr('data-username');
        var betlimitType = $this.attr('data-betlimit-type');
        betLimitControl.openUpdateDialog(username, betlimitType);
    });
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
        maxPerMatch: null,
        maxBet: null
    }

    this.cachedElements = function () {
        this.elements.divBetLimitControl = $("#div-bet-limit-control");
        this.elements.reportContainer = $("#report-container");

        this.popupAdd.btnSave = $("#btnSaveAdd");
        this.popupAdd.btnCancel = $("#btnCancelAdd");
        this.popupAdd.username = $("#txtUsernameAdd");
        this.popupAdd.betLimitType = $("#txtBetLimitTypeAdd");
        this.popupAdd.maxPerMatch = $("#txtMaxPerMatchAdd");
        this.popupAdd.maxBet = $("#txtMaxBetAdd");

        this.popupUpdate.btnSave = $("#btnSaveUpdate");
        this.popupUpdate.btnCancel = $("#btnCancelUpdate");
        this.popupUpdate.username = $("#usernameUpdate");
        this.popupUpdate.betLimitType = $("#selBetLimitTypeUpdate");
        this.popupUpdate.maxPerMatch = $("#txtMaxPerMatchUpdate");
        this.popupUpdate.maxBet = $("#txtMaxBetUpdate");
    };

    this.InitEventHandler = function () {
        $('#btnAdd').click(showAddDialog);
    }

    this.InitPage = function () {
        this.cachedElements();
        this.InitEventHandler();
        this.renderBetLimit();
    }

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
                { HeaderText: "Username", Width: 140, Name: 'Username', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'VIP', Width: 80, Name: 'VIP', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Level', Width: 80, Name: 'Level', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Currency', Width: 80, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Site', Width: 80, Name: 'Site', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Bet Limit Type', Width: 140, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'MaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 140, Name: 'MaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'AgentMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 140, Name: 'AgentMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'MasterMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 140, Name: 'MasterMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Per Match', Width: 140, Name: 'SuperMaxPerMatch', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Max Bet', Width: 140, Name: 'SuperMaxBet', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: 'Action', Width: 100, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true }
            ],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            showTitle: false,
            pagerOption: {
                itemsPerPage: 50,
                currentPage: 1,
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

        row = {
            Columns: [
                {
                    Name: 'Username',
                    Value: 'Member01'
                },
                {
                    Name: 'VIP',
                    Value: 'No'
                },
                {
                    Name: 'Level',
                    Value: 'Member'
                },
                {
                    Name: 'Currency',
                    Value: 'UUS'
                },
                {
                    Name: 'Site',
                    Value: 'BEST-ODDS'
                },
                {
                    Name: 'BetLimitType',
                    Value: 'PA - Credit'
                },
                {
                    Name: 'MaxPerMatch',
                    Value: '6000'
                },
                {
                    Name: 'MaxBet',
                    Value: '3000'
                },
                {
                    Name: 'AgentMaxPerMatch',
                    Value: '6000'
                },
                {
                    Name: 'AgentMaxBet',
                    Value: '3000'
                },
                {
                    Name: 'MasterMaxPerMatch',
                    Value: '6000'
                },
                {
                    Name: 'MasterMaxBet',
                    Value: '3000'
                },
                {
                    Name: 'SuperMaxPerMatch',
                    Value: '6000'
                },
                {
                    Name: 'SuperMaxBet',
                    Value: '3000'
                },
                {
                    Name: 'Action',
                    Value: "<span class='ico-edit edit-betlimitcontrol' data-username='Member01' data-betlimtit-type='1'  ></span><span class='ico-delete mid-margin-left'></span>"
                }
            ]
        };
        bodyData.push(row);

        gridOptions.bodyRows = bodyData;
        gridOptions.pagerOption.totalItem = 0;

        this.elements.divBetLimitControl.fanexGrid(gridOptions);

        this.elements.divBetLimitControl.find(".ico-delete").on("click touchend", function () {
        });

        this.elements.reportContainer.removeClass("display-none");
    };

    function IsPositiveNumber(number) {
        if (number && number >= 0) {
            return true;
        }
        return false;
    }

    function validateNewBetTypeControlValue() {
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

        var maxPerMatch = parseInt(maxPerMatchString);
        if (!IsPositiveNumber(maxPerMatch)) {
            CMS.showTipsyError(self.popupAdd.maxPerMatch, 'Please input a positive whole number.', "w");
            return;
        }

        var maxBetString = self.popupAdd.maxBet.val();
        if (!maxBetString) {
            CMS.showTipsyError(self.popupAdd.maxBet, 'This field is required', "w");
            return;
        }

        var maxBet = parseInt(maxBetString);
        if (!IsPositiveNumber(maxBet)) {
            CMS.showTipsyError(self.popupAdd.maxBet, 'Please input a positive whole number.', "w");
            return;
        }
    }

    function add() {
        validateNewBetTypeControlValue();
    }

    function showAddDialog() {
        CMS.closeMsgBox();
        if (!self.elements.$addDialog) {
            self.popupAdd.btnSave.click(add);
            self.popupAdd.btnCancel.click(CMS.closePopup);
            self.elements.$addDialog = CMS.modal("New Bet Limit", $('#frmAdd'), { resizable: false });
        } else {
            CMS.clearTipsyError(self.popupAdd.username);
            CMS.clearTipsyError(self.popupAdd.betLimitType);
            CMS.clearTipsyError(self.popupAdd.maxPerMatch);
            CMS.clearTipsyError(self.popupAdd.maxBet);
            self.elements.$addDialog.dialog('open');
        }
    }

    function validateUpdateBetTypeControlValue() {
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

        var maxPerMatch = parseInt(maxPerMatchString);
        if (!IsPositiveNumber(maxPerMatch)) {
            CMS.showTipsyError(self.popupUpdate.maxPerMatch, 'Please input a positive whole number.', "w");
            return;
        }

        var maxBetString = self.popupAdd.maxBet.val();
        if (!maxBetString) {
            CMS.showTipsyError(self.popupUpdate.maxBet, 'This field is required', "w");
            return;
        }

        var maxBet = parseInt(maxBetString);
        if (!IsPositiveNumber(maxBet)) {
            CMS.showTipsyError(self.popupUpdate.maxBet, 'Please input a positive whole number.', "w");
            return;
        }
    }

    function update() {
        validateUpdateBetTypeControlValue();
    }

    this.openUpdateDialog = function (username, betTypeLimit) {
        CMS.closeMsgBox();
        if (!self.elements.$updateDialog) {
            this.popupUpdate.username.text(username);
            this.popupUpdate.betLimitType.val(betTypeLimit);
            self.popupUpdate.btnSave.click(update);
            self.popupUpdate.btnCancel.click(CMS.closePopup);
            self.elements.$updateDialog = CMS.modal("New Bet Limit", $('#frmUpd'), { resizable: false });
        } else {
            CMS.clearTipsyError(self.popupUpdate.username);
            CMS.clearTipsyError(self.popupUpdate.betLimitType);
            CMS.clearTipsyError(self.popupUpdate.maxPerMatch);
            CMS.clearTipsyError(self.popupUpdate.maxBet);
            self.elements.$updateDialog.dialog('open');
        }
    }
}