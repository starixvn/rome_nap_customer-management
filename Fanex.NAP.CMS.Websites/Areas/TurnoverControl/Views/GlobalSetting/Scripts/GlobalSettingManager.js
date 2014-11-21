/* File Created: Oct 03, 2014 */
/* global Namespace, 
Fanex, 
CMS, 
viewResources,
numeral,
CMSConstants,
keyboardManager,
popUpManager,
maxTurnoverValue */
/* jshint strict: false,
camelcase: false,
unused: vars */
// 'use strict';

var globalSettingManager;
$(document).ready(function () {
    globalSettingManager.initPage();
});

Namespace.Register("Fanex.Nap.CMS.TurnoverControl.GlobalSettingManager");
Fanex.Nap.CMS.TurnoverControl.GlobalSettingManager = function () {
    var self = this;
    self.domElements = {
        btnSearch: null,
        btnAdd: null,
        textboxSMAName: null,
        tableGlobalSettingList: null,
        divResult: null
    };
    self.configurations = null;
    self.params = {
        getGlobalSettingList: {
            SortField: "Name",
            SortDirection: "asc",
            SMAName: ''
        },
        updateGlobalSetting: null,
        deleteGlobalSetting: null
    };
    self.cachedData = {
        globalSettingList: null,
        editingRowIndex: 0,
        shouldReloadGrid: false
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.configurations = {
            getGlobalSettingLink: window.rootUrl + 'GlobalSetting/GlobalSettingList',
            sortGlobalSettingLink: window.rootUrl + 'GlobalSetting/SortGlobalSettingList',
            exportGlobalSettingLink: window.rootUrl + 'GlobalSetting/ExportGlobalSettingList',
            updateGlobalSettingLink: window.rootUrl + 'GlobalSetting/EditGlobalSetting',
            deleteGlobalSettingLink: window.rootUrl + 'GlobalSetting/DeleteGlobalSetting',
            addGlobalSettingLink: window.rootUrl + 'GlobalSetting/OpenAddGlobalSettingView',
            css: {
                displayNone: 'display-none'
            }
        };
        self.initElements();
        self.bindEvents();
        self.btnSearchClick();
    };
    self.cacheDOMElements = function () {
        self.domElements.btnSearch = $('#btn-search');
        self.domElements.btnAdd = $('#btn-add');
        self.domElements.textboxSMAName = $('#txt-username');
        self.domElements.tableGlobalSettingList = $("#globalsetting-list");
        self.domElements.divResult = $("#div-result-message");
    };
    self.initElements = function () {
        if (!viewResources.hasHostPermission) {
            return;
        }

        self.domElements.btnAdd.removeClass(self.configurations.css.displayNone);
    };
    self.bindEvents = function () {
        self.domElements.btnSearch.unbind("click");
        self.domElements.btnSearch.click(self.btnSearchClick);

        self.domElements.textboxSMAName.keyup(function (event) {
            if (event.keyCode === keyboardManager.Enums.KeyCode.Enter) {
                self.btnSearchClick();
            }
        });

        if (!viewResources.hasHostPermission) {
            return;
        }

        self.domElements.btnAdd.unbind("click");
        self.domElements.btnAdd.click(self.btnAddClick);
    };
    self.btnSearchClick = function () {
        CMS.disableInput();
        CMS.hideInlineMsg(self.domElements.divResult);
        self.params.getGlobalSettingList.SMAName = self.domElements.textboxSMAName.val();
        CMS.callAjax(self,
             self.configurations.getGlobalSettingLink,
             self.params.getGlobalSettingList,
             self.renderGlobalSettingList);
    };
    self.renderGlobalSettingList = function (result) {
        self.cachedData.globalSettingList = result;
        if (viewResources.hasHostPermission) {
            self.renderGlobalSettingList_Host(result);
        } else {
            self.renderGlobalSettingList_Read(result);
        }
    };

    self.renderGlobalSettingList_Host = function (result) {
        var bodyData = [];
        var customerLength = result ? result.length : 0;
        var i;
        for (i = 0; i < customerLength; i++) {
            var customerInfo = result[i];
            bodyData.push({
                Columns: [
                    { Name: "ColNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "ColUsername", Value: customerInfo.Name },
                    { Name: "ColRoleName", Value: customerInfo.RoleInfo.Name },
                    { Name: "ColCurrency", Value: customerInfo.Currency.Name },
                    {
                        Name: "ColMaxTurnover",
                        Value: '<a class="c hand edit-setting" href="JavaScript:void(0);" data-row-index="' + i + '">' +
                                numeral(customerInfo.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) +
                            '</a>',
                        CssClass: 'edit-setting-column row-index-' + i
                    },
                    {
                        Name: "ColDelete",
                        Value: '<a class="c hand ico-delete" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                    }],
                CssClass: 'customer-row row-index-' + i
            });
        }

        var sortColumnIndex = self.getSortColumnIndex();
        var headerColumns = [
            { HeaderText: viewResources.Symbol_No, Width: 60, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
            {
                HeaderText: viewResources.SMA_Name,
                Width: 160,
                Name: "ColUsername",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                SortDirection: 'desc'
            },
            {
                HeaderText: viewResources.Level,
                Width: 120,
                Name: "ColRoleName",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true,
                SortDirection: 'desc'
            },
            {
                HeaderText: viewResources.Currency,
                Width: 160,
                Name: "ColCurrency",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true,
                SortDirection: 'desc'
            },
            {
                HeaderText: viewResources.Max_Turnover,
                Width: 160,
                Name: "ColMaxTurnover",
                HeaderAlign: "center",
                CellAlign: "right",
                Sort: true,
                SortDirection: 'desc'
            },
            { HeaderText: viewResources.Delete, Width: 60, Name: "ColDelete", HeaderAlign: "center", CellAlign: "center"}];

        for (i = 0; i < 6; i++) {
            if (i === sortColumnIndex) {
                headerColumns[i].SortDirection = self.params.getGlobalSettingList.SortDirection;
            } else {
                headerColumns[i].SortDirection = "";
            }
        }

        var gridOptions = {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                showExcelButton: true,
                exportExcelFunction: function () {
                    self.exportGlobalSettingList();
                }
            },
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    self.params.getGlobalSettingList.SortField = self.getGlobalSettingListField(sortColumnName);
                    self.params.getGlobalSettingList.SortDirection = sortDirection;
                    self.sortGlobalSettingList();
                }
            }
        };

        self.domElements.tableGlobalSettingList.removeClass(self.configurations.css.displayNone).fanexGrid(gridOptions);

        var editIcons = self.domElements.tableGlobalSettingList.find(".edit-setting");
        editIcons.unbind("click");
        editIcons.click(function () {
            var rowIndex = parseInt($(this).attr("data-row-index"));
            self.editMaxTurnover(rowIndex);
        });

        var deleteIcons = self.domElements.tableGlobalSettingList.find(".ico-delete");
        deleteIcons.unbind("click");
        deleteIcons.click(function () {
            var rowIndex = parseInt($(this).attr("data-row-index"));
            self.deleteGlobalSetting(rowIndex);
        });
    };
    self.renderGlobalSettingList_Read = function (result) {
        var bodyData = [];
        var customerLength = result ? result.length : 0;
        for (var i = 0; i < customerLength; i++) {
            var customerInfo = result[i];
            bodyData.push({
                Columns: [
                    { Name: "ColNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "ColUsername", Value: customerInfo.Name },
                    { Name: "ColRoleName", Value: customerInfo.RoleInfo.Name },
                    { Name: "ColCurrency", Value: customerInfo.Currency.Name },
                    { Name: "ColMaxTurnover", Value: numeral(customerInfo.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber)}],
                CssClass: 'customer-row row-index-' + i
            });
        }

        var sortColumnIndex = self.getSortColumnIndex();
        var headerColumns = [
            { HeaderText: viewResources.Symbol_No, Width: 60, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
            {
                HeaderText: viewResources.SMA_Name,
                Width: 160,
                Name: "ColUsername",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                SortDirection: 'desc'
            },
            { HeaderText: viewResources.Level, Width: 120, Name: "ColRoleName", HeaderAlign: "center", CellAlign: "center", Sort: true, SortDirection: 'desc' },
            {
                HeaderText: viewResources.Currency,
                Width: 160,
                Name: "ColCurrency",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true,
                SortDirection: 'desc'
            },
            {
                HeaderText: viewResources.Max_Turnover,
                Width: 160,
                Name: "ColMaxTurnover",
                HeaderAlign: "center",
                CellAlign: "right",
                Sort: true,
                SortDirection: 'desc'
            }];

        for (i = 0; i < 5; i++) {
            if (i === sortColumnIndex) {
                headerColumns[i].SortDirection = self.params.getGlobalSettingList.SortDirection;
            } else {
                headerColumns[i].SortDirection = "";
            }
        }

        var gridOptions = {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                showExcelButton: true,
                exportExcelFunction: function () {
                    self.exportGlobalSettingList();
                }
            },
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            showHighlightRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    self.params.getGlobalSettingList.SortField = self.getGlobalSettingListField(sortColumnName);
                    self.params.getGlobalSettingList.SortDirection = sortDirection;
                    self.sortGlobalSettingList();
                }
            }
        };

        self.domElements.tableGlobalSettingList.removeClass(self.configurations.css.displayNone).fanexGrid(gridOptions);
    };
    self.sortGlobalSettingList = function () {
        CMS.disableInput();
        CMS.hideInlineMsg(self.domElements.divResult);
        CMS.callAjax(self,
            self.configurations.sortGlobalSettingLink,
            self.params.getGlobalSettingList,
            self.renderGlobalSettingList);
    };
    self.exportGlobalSettingList = function () {
        CMS.downloadFile(self.configurations.exportGlobalSettingLink, self.params.getGlobalSettingList);
    };
    self.getGlobalSettingListField = function (sortColumnName) {
        switch (sortColumnName) {
            case "ColUsername":
                return "Name";
            case "ColRoleName":
                return "RoleName";
            case "ColCurrency":
                return "CurrencyName";
            case "ColMaxTurnover":
                return "MaxTurnover";
            default:
                return "";
        }
    };
    self.getSortColumnIndex = function () {
        var sortField = self.params.getGlobalSettingList.SortField;
        switch (sortField) {
            case "Name":
                return 1;
            case "RoleName":
                return 2;
            case "CurrencyName":
                return 3;
            case "MaxTurnover":
                return 4;
            default:
                return -1;
        }
    };
    self.editMaxTurnover = function (rowIndex) {
        var customerRow = self.domElements.tableGlobalSettingList.find('.customer-row.row-index-' + rowIndex);
        customerRow.addClass("current-editing-row");

        var customerInfo = self.cachedData.globalSettingList[rowIndex];
        var columnEdit = self.domElements.tableGlobalSettingList.find('.edit-setting-column.row-index-' + rowIndex);
        columnEdit.attr("align", "center");

        var editLink = columnEdit.find('a.edit-setting');
        editLink.addClass(self.configurations.css.displayNone);
        columnEdit.append('<input class="width-120 clearable edit-setting textbox-maxturnover" type="text" maxlength="13" value="' +
                numeral(customerInfo.MaxTurnover).format(CMSConstants.NumberFormat.IntNumber) +
            '" />');

        var textboxEdit = columnEdit.find("input.edit-setting");
        textboxEdit.focus();
        textboxEdit.blur(function () {
            self.completeEditMaxTurnover(rowIndex);
        });

        textboxEdit.clearfield();
        textboxEdit.addClass('x-icon');

        textboxEdit.unbind("keypress");
        textboxEdit.keypress(function (e) {
            var allowInput = CMS.forceNumberInput(e);
            if (!allowInput) {
                return false;
            }

            textboxEdit.addClass('x-icon');
        });

        textboxEdit.unbind("keyup");
        textboxEdit.keyup(function (e) {
            var textboxValue = $(this).val();
            var maxTurnover = parseInt(textboxValue.replace(/,/g, ""));
            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.Escape) {
                self.completeEditMaxTurnover(rowIndex);
                return;
            }

            if (code === keyboardManager.Enums.KeyCode.Enter) {
                CMS.showConfirmMessage(
                    viewResources.GlobalSetting_EditMaxTurnover_Confirm.format(customerInfo.Name),
                    function () {
                        self.updateMaxTurnover(rowIndex, maxTurnover);
                    });
                return;
            }

            self.formatMaxTurnover(textboxEdit);
        });

        textboxEdit.on("paste", function (e) {
            setTimeout(function () {
                self.formatMaxTurnover(textboxEdit);
            }, 100);
        });
    };
    self.formatMaxTurnover = function (textboxEdit) {
        var textboxValue = textboxEdit.val();
        var maxTurnover = parseInt(textboxValue.replace(/,/g, ""));
        if (maxTurnover.toString() === textboxValue.replace(/,/g, "")) {
            textboxEdit.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    };
    self.completeEditMaxTurnover = function (rowIndex) {
        var customerRow = self.domElements.tableGlobalSettingList.find('.customer-row.row-index-' + rowIndex);
        customerRow.removeClass("current-editing-row");

        var columnEdit = self.domElements.tableGlobalSettingList.find('.edit-setting-column.row-index-' + rowIndex);
        var editLink = columnEdit.find('a.edit-setting');
        var textboxEdit = columnEdit.find("input.edit-setting");
        columnEdit.attr("align", "right");
        editLink.removeClass(self.configurations.css.displayNone);
        textboxEdit.remove();
    };

    self.checkUpdateMaxTurnoverParam = function (updateMaxTurnoverParam) {
        if (updateMaxTurnoverParam.MaxTurnover > maxTurnoverValue) {
            CMS.showInlineErrorMsg(viewResources.GlobalSetting_MaxTurnover_ExceedMaxTurnover, self.domElements.divResult);
            return false;
        }

        CMS.hideInlineMsg(self.domElements.divResult);
        return true;
    };
    self.updateMaxTurnover = function (rowIndex, maxTurnover) {
        CMS.disableInput();
        self.cachedData.editingRowIndex = rowIndex;
        self.params.updateGlobalSetting = {};
        $.extend(self.params.updateGlobalSetting, self.cachedData.globalSettingList[rowIndex]);
        self.params.updateGlobalSetting.MaxTurnover = maxTurnover;
        if (!self.checkUpdateMaxTurnoverParam(self.params.updateGlobalSetting)) {
            CMS.enableInput();
            return;
        }

        CMS.callAjax(self,
             self.configurations.updateGlobalSettingLink,
             self.params.updateGlobalSetting,
             self.completeUpdateMaxTurnover);
    };
    self.completeUpdateMaxTurnover = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.domElements.divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.domElements.divResult, true);
        var rowIndex = self.cachedData.editingRowIndex;
        var maxTurnover = self.params.updateGlobalSetting.MaxTurnover;
        var customerRow = self.domElements.tableGlobalSettingList.find('.customer-row.row-index-' + rowIndex);
        customerRow.removeClass("current-editing-row");

        self.cachedData.globalSettingList[rowIndex].MaxTurnover = maxTurnover;
        var columnEdit = self.domElements.tableGlobalSettingList.find('.edit-setting-column.row-index-' + rowIndex);
        var editLink = columnEdit.find('a.edit-setting');
        editLink.html(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        columnEdit.attr("align", "right");
        editLink.removeClass(self.configurations.css.displayNone);
    };
    self.deleteGlobalSetting = function (rowIndex) {
        self.cachedData.editingRowIndex = rowIndex;
        CMS.showConfirmMessage(
            viewResources.GlobalSetting_DeleteMaxTurnover_Confirm,
            function () {
                CMS.disableInput();
                self.params.deleteGlobalSetting = {};
                $.extend(self.params.deleteGlobalSetting, self.cachedData.globalSettingList[rowIndex]);
                CMS.callAjax(self,
                     self.configurations.deleteGlobalSettingLink,
                     self.params.deleteGlobalSetting,
                     self.completeDeleteCustomerGlobalSetting);
            });
    };
    self.completeDeleteCustomerGlobalSetting = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.domElements.divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.domElements.divResult, true);
        self.reloadGlobalSettingList();
    };
    self.reloadGlobalSettingList = function () {
        CMS.disableInput();
        CMS.callAjax(self,
            self.configurations.getGlobalSettingLink,
            self.params.getGlobalSettingList,
            self.renderGlobalSettingList);
    };
    self.btnAddClick = function () {
        self.cachedData.shouldReloadGrid = false;
        popUpManager.openPopup(
            self.configurations.addGlobalSettingLink,
            viewResources.GlobalSetting_AddNewSMA_Title,
            560,
            230,
            true,
            false,
            parent.window,
            3000,
            null,
            self.closeAddView,
            ['center', 160 - CMS.popupTopMargin]);
    };
    self.closeAddView = function () {
        if (globalSettingManager.cachedData.shouldReloadGrid) {
            self.reloadGlobalSettingList();
            globalSettingManager.cachedData.shouldReloadGrid = false;
        }
    };
};

globalSettingManager = new Fanex.Nap.CMS.TurnoverControl.GlobalSettingManager();