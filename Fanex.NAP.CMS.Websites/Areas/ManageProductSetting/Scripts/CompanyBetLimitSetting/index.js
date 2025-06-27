"use strict";
$(document).ready(function () {
    var nonsportsBLC = new Fanex.CMS.NonSportCompanyBLC();
    nonsportsBLC.InitPage();
});

Namespace.Register('Fanex.CMS.NonSportCompanyBLC');
Fanex.CMS.NonSportCompanyBLC = function () {
    let elements = {
        isCredit: true,
        hasHostPermission: false,
        rdbRestriction: $("[name=Restriction]")
    };

    let cachedData = [], cachedFilterData = null;

    const getCurrencyLabel = (isCredit) => isCredit ? viewResources.lblCreditCurrency : viewResources.lblDepositCurrency;
    const getSiteLabel = (isCredit) => isCredit ? viewResources.lblCreditSite : viewResources.lblDepositSite;
    const doubleFormatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    const intFormatter = new Intl.NumberFormat('en-US', {
        maximumFractionDigits: 0
    });

    function getFilterParams(isCredit) {
        const filterParams = {
            IsCredit: true,
            ProductIds: "",
            Currencies: "",
            Sites: ""
        };
        const productsDdl = isCredit ? $("#ddl-credit-products") : $("#ddl-deposit-products");
        const currenciesDdl = isCredit ? $("#ddl-credit-currencies") : $("#ddl-deposit-currencies");
        const sitesDdl = isCredit ? $("#ddl-credit-sites") : $("#ddl-deposit-sites");
        filterParams.ProductIds = productsDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Currencies = currenciesDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Sites = sitesDdl.multiSelect("getCheckedItems").join(",");
        filterParams.IsCredit = isCredit;

        return filterParams;
    }

    function getGridOptions(isCredit) {
        const currencyLabel = getCurrencyLabel(isCredit);
        const siteLabel = getSiteLabel(isCredit);
        const headerColumns = [
            {
                HeaderText: viewResources.lblSymbol_No,
                Name: "ColNo",
                Width: 50,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblSportTypeID,
                Name: "SportType",
                CellAlign: "center",
                Width: 80,
                HeaderAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblSportTypeName,
                Name: "SportName",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                CanResize: true
            },
            {
                HeaderText: currencyLabel,
                Name: "Currency",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: siteLabel,
                Name: "Site",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblGameGroup,
                Name: "GameGroups",
                Width: 120,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                CanResize: true
            },
            {
                HeaderText: viewResources.lblMinBet,
                Width: 90,
                Name: "MinBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.DecimalFormat
            },
            {
                HeaderText: viewResources.lblMaxBet,
                Width: 90,
                Name: "MaxBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.IntegerFormat,
            },
            {
                HeaderText: viewResources.lblRemark,
                Width: 160,
                Name: "Remark",
                HeaderAlign: "center",
                CellAlign: "center"
            },
        ];
        if (elements.hasHostPermission) {
            headerColumns.push(
                {
                    HeaderText: viewResources.lblEdit,
                    Name: "IconEdit",
                    Width: 60,
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            headerColumns.push(
                    {
                        HeaderText: viewResources.lblRemove,
                        Name: "IconRemove",
                        Width: 60,
                        HeaderAlign: "center",
                        CellAlign: "center"
                    });
        }

        var groupHeaderRows = [
            {
                Columns: [
                    { HeaderText: viewResources.lblSymbol_No, Name: "ColNo", MergeRowNumber: 2 },
                    { HeaderText: viewResources.lblSportTypeID, Name: "SportType", MergeRowNumber: 2 },
                    { HeaderText: viewResources.lblSportTypeName, Name: "SportName", MergeRowNumber: 2 },
                    { HeaderText: currencyLabel, Name: "Currency", MergeRowNumber: 2 },
                    { HeaderText: siteLabel, Name: "Site", MergeRowNumber: 2 },
                    { HeaderText: viewResources.lblGameGroup, Name: "GameGroups", MergeRowNumber: 2 },
                    { HeaderText: viewResources.lblBetLimit, Name: "MinBet", MergeColNumber: 3 }
                    
                ]
            }
        ];
        if (elements.hasHostPermission) {
            groupHeaderRows[0].Columns.push({ HeaderText: viewResources.lblEdit, Name: "IconEdit", MergeRowNumber: 2 });
            groupHeaderRows[0].Columns.push({ HeaderText: viewResources.lblRemove, Name: "IconRemove", MergeRowNumber: 2 });
        }

        return {
            columns: headerColumns,
            customizeHeader: true,
            groupHeaderRows: groupHeaderRows,
            showTitle: true,
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    const url = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Export");
                    const isCredit = getFilterParams(elements.isCredit);
                    CMS.downloadFile(url, isCredit);
                }
            },
            bodyRows: [],
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            headerColumnHeight: 27,
            bodyRowHeight: 27,
            showNumberColumn: true,
            gridExpandHeight: 250,
            showHighlightRow: true,
            showPager: false
        };
    }

    function renderGrid(data) {
        cachedData = data;
        const bodyData = [];
        let row;
        let currentRow = 0;
        let currentCurrencyGroupRow;
        let currentSportGroupRow;
        let currentSiteRow;
        let sportGroupChildren;
        let currencyGroupChildren;
        let highlightClass;
        const editCol = `<span class="ico-edit col-view-sport-{0}-cur-{1}-site-{2}" data-pos-sport="{0}" data-pos-cur="{1}" data-pos-site="{2}" title={3}></span><span class="ico-save-16 col-edit-sport-{0}-cur-{1}-site-{2} hidden" title="Save" data-pos-sport="{0}" data-pos-cur="{1}" data-pos-site="{2}"></span><span class="ico-cancel col-edit-sport-{0}-cur-{1}-site-{2} ml-10 hidden" title="Cancel" data-pos-sport="{0}" data-pos-cur="{1}" data-pos-site="{2}"></span>`;
        const delCol = `<span class="ico-delete" data-pos-sport="{0}" data-pos-cur="{1}" data-pos-site="{2}" title={3}></span>`;
        const minBetCol = `<div class="input-minMaxBet"><span class="col-view-sport-{0}-cur-{1}-site-{2}">{4}</span><input id="minbet-{0}-{1}-{2}-{3}" class="col-edit-sport-{0}-cur-{1}-site-{2} input-bet hidden" value="{4}"/></div>`;
        const maxBetCol = `<div class="input-minMaxBet"><span class="col-view-sport-{0}-cur-{1}-site-{2}">{4}</span><input id="maxbet-{0}-{1}-{2}-{3}" class="col-edit-sport-{0}-cur-{1}-site-{2} input-bet hidden" value="{4}"/></div>`;
        const remarkCol = `<div><span class="col-view-sport-{0}-cur-{1}-site-{2}">{4}</span><textarea id="remark-{0}-{1}-{2}-{3}" class="col-edit-sport-{0}-cur-{1}-site-{2} input-remark hidden" cols="5" rows="1">{4}</textarea></div>`;

        if (data.length > 0) {

            $.each(data, function (sportGroupIndex, sportGroup) {
                sportGroupChildren = 0;
                currentSportGroupRow = currentRow;
                $.each(sportGroup.CurrencyGroups, function (currencyGroupIndex, currencyGroup) {
                    currencyGroupChildren = 0;
                    currentCurrencyGroupRow = currentRow;
                    $.each(currencyGroup.SiteGroups, function (siteGroupIndex, siteGroup) {
                        sportGroupChildren += siteGroup.GameGroups.length;
                        currencyGroupChildren += siteGroup.GameGroups.length;
                        currentSiteRow = currentRow;
                        $.each(siteGroup.GameGroups, function (index, gameGroup) {
                            highlightClass = 'hover-column';
                            const entry = gameGroup.Entries.find(e => e.GameGroup === gameGroup.GameGroupName);
                            row = {
                                Columns: [
                                    { Name: 'ColNo', Value: "" },
                                    { Name: 'GameGroups', Value: gameGroup.GameGroupName, TdClass: highlightClass },
                                    { Name: 'MinBet', Value: minBetCol.format(sportGroupIndex, currencyGroupIndex, siteGroupIndex, index, entry.MinBet === undefined ? "" : formatNumber(entry.MinBet)), TdClass: highlightClass },
                                    { Name: 'MaxBet', Value: maxBetCol.format(sportGroupIndex, currencyGroupIndex, siteGroupIndex, index, entry.MaxBet === undefined ? "" : formatNumber(entry.MaxBet)), TdClass: highlightClass },
                                    { Name: 'Remark', Value: remarkCol.format(sportGroupIndex, currencyGroupIndex, siteGroupIndex, index, entry.Remark === undefined ? "" : entry.Remark), TdClass: highlightClass }
                                ]
                            };

                            bodyData.push(row);
                            currentRow++;
                        });
                        row = bodyData[currentSiteRow];
                        row.Columns.splice(1, 0, {
                            Name: 'Site',
                            Value: siteGroup.SiteGroupName,
                            MergeRowNumber: siteGroup.GameGroups.length
                        });
                        if (elements.hasHostPermission) {
                            row.Columns.push(
                                {
                                    Name: 'IconEdit',
                                    Value: editCol.format(sportGroupIndex, currencyGroupIndex, siteGroupIndex, "Edit"),
                                    MergeRowNumber: siteGroup.GameGroups.length
                                },
                                {
                                    Name: 'IconRemove',
                                    Value: delCol.format(sportGroupIndex, currencyGroupIndex, siteGroupIndex, "Delete"),
                                    MergeRowNumber: siteGroup.GameGroups.length
                                }
                            );
                        }
                    });
                    row = bodyData[currentCurrencyGroupRow];
                    row.Columns.splice(1, 0, {
                        Name: 'Currency',
                        Value: currencyGroup.Currency,
                        MergeRowNumber: currencyGroupChildren
                    });

                });
                row = bodyData[currentSportGroupRow];
                row.Columns.splice(1, 0, ...[
                    {
                        Name: 'SportType',
                        Value: sportGroup.SportType,
                        MergeRowNumber: sportGroupChildren,
                        CssClass: 'chk-column'
                    },
                    {
                        Name: 'SportName',
                        Value: sportGroup.SportName,
                        MergeRowNumber: sportGroupChildren,
                        CssClass: 'chk-column'
                    }
                ]);
            });
        }
        return bodyData;
    }

    function getData() {
        const apiUrl = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Search?" + $.param(cachedFilterData));
        CMS.getJson(apiUrl, (data) => {
            const jsonData = $.parseJSON(data);
            const gridOptions = getGridOptions(cachedFilterData.IsCredit);
            gridOptions.bodyRows = renderGrid(jsonData);
            $("#grid-table").removeClass("display-none").fanexGrid(gridOptions);
            bindingEventAfterRenderGrid();
        });
    }

    function bindingEventAfterRenderGrid() {
        $(".input-bet").keydown(function (e) { return CMS.forceNumericInput(e, true); });
        $(".ico-edit").off("click").click(function () {
            var posSport = +$(this).data("pos-sport");
            var posCur = +$(this).data("pos-cur");
            var posSite = +$(this).data("pos-site");
            $(`.col-edit-sport-${posSport}-cur-${posCur}-site-${posSite}`).removeClass("hidden");
            $(`.col-view-sport-${posSport}-cur-${posCur}-site-${posSite}`).each(function() {
                if (!$(this).hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            });
        });

        $(".ico-cancel").off("click").click(function () {
            var posSport = +$(this).data("pos-sport");
            var posCur = +$(this).data("pos-cur");
            var posSite = +$(this).data("pos-site");
            $(`.col-view-sport-${posSport}-cur-${posCur}-site-${posSite}`).removeClass("hidden");
            $(`.col-edit-sport-${posSport}-cur-${posCur}-site-${posSite}`).each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            });
            cachedData[posSport].CurrencyGroups[posCur].SiteGroups[posSite].GameGroups.forEach((gameGroup, index) => {
                CMS.clearTipsyError($(`#minbet-${posSport}-${posCur}-${posSite}-${index}`));
                CMS.clearTipsyError($(`#maxbet-${posSport}-${posCur}-${posSite}-${index}`));
                CMS.clearTipsyError($(`#remark-${posSport}-${posCur}-${posSite}-${index}`));
            });
        });

        $(".ico-save-16").off("click").click(function () {
            var posSport = +$(this).data("pos-sport");
            var posCur = +$(this).data("pos-cur");
            var posSite = +$(this).data("pos-site");
            var isHaveError = false;
            var requestBody = [];
            cachedData[posSport].CurrencyGroups[posCur].SiteGroups[posSite].GameGroups.forEach((gameGroup, index) => {
                var newMinBetDiv = $(`#minbet-${posSport}-${posCur}-${posSite}-${index}`);
                var newMinBet = newMinBetDiv.val();
                var newMaxBetDiv = $(`#maxbet-${posSport}-${posCur}-${posSite}-${index}`);
                var newMaxBet = newMaxBetDiv.val();
                var newRemarkDiv = $(`#remark-${posSport}-${posCur}-${posSite}-${index}`);
                var newRemark = newRemarkDiv.val();
                if (newMinBet === "") {
                    CMS.showTipsyError(newMinBetDiv, viewResources.requiredField, "w");
                    isHaveError = true;
                }
                if (newMaxBet === "") {
                    CMS.showTipsyError(newMaxBetDiv, viewResources.requiredField, "w");
                    isHaveError = true;
                }
                var posRemarkError = "se";
                if(posSport === 0 && posCur === 0 && ( posSite === 0 && index < 2 || posSite === 1 && index < 1)) {
                    posRemarkError = "ne";
                }
                if (newRemark.length > 1000) {
                    CMS.showTipsyError(newRemarkDiv, viewResources.remarkLengthError, posRemarkError);
                    isHaveError = true;
                }
                if (newRemark.match(/<[^>]*>/)) {
                    CMS.showTipsyError(newRemarkDiv, "Invalid parameter value.", posRemarkError);
                    isHaveError = true;
                }
                const entry = gameGroup.Entries.find(e => e.GameGroup === gameGroup.GameGroupName);
                if (+newMinBet !== entry.MinBet || +newMaxBet !== entry.MaxBet || newRemark !== entry.Remark) {
                    requestBody.push({
                        SportType: entry.SportType,
                        CreditGroupID: elements.isCredit ? entry.CurrencyGroupID : null,
                        LicenseeGroupID: elements.isCredit ? null : entry.CurrencyGroupID,
                        SiteID: entry.SiteID,
                        GameGroupID: entry.GameGroupID,
                        NewMinBet: newMinBet,
                        NewMaxBet: newMaxBet,
                        NewRemark: newRemark,
                        CurMinBet: entry.MinBet === undefined ? null : entry.MinBet,
                        CurMaxBet: entry.MaxBet === undefined ? null : entry.MaxBet,
                        CurRemark: entry.Remark

                    });
                }
            });
            if (isHaveError) {
                return;
            }
            if (requestBody.length <= 0) {
                CMS.showErrorMessage("The New Min Bet, New Max Bet are the same as The Current Min Bet, the Current Max Bet. Please input another value for the New values again.");
                return;
            }

            CMS.disableInput();
            CMS.callAjax(
                this,
                CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Update"),
                requestBody,
                function (response) {
                    var result = typeof response === "string" ? JSON.parse(response) : response;
                    if (result.IsSuccess) {
                        CMS.showSuccessMessage("Update successfully!", getData);
                        $(`.col-view-sport-${posSport}-cur-${posCur}-site-${posSite}`).removeClass("hidden");
                        $(`.col-edit-sport-${posSport}-cur-${posCur}-site-${posSite}`).each(function () {
                            if (!$(this).hasClass("hidden")) {
                                $(this).addClass("hidden");
                            }
                        });
                    } else {
                        CMS.showErrorMessage(result.Message);
                    }
                });
        });

        $(".ico-delete").off("click").click(function () {
            var posSport = +$(this).data("pos-sport");
            var posCur = +$(this).data("pos-cur");
            var posSite = +$(this).data("pos-site");
            var requestBody = [];
            cachedData[posSport].CurrencyGroups[posCur].SiteGroups[posSite].GameGroups.forEach((gameGroup, index) => {
                const entry = gameGroup.Entries.find(e => e.GameGroup === gameGroup.GameGroupName);
                requestBody.push({
                    SportType: entry.SportType,
                    CurrencyGroupID: entry.CurrencyGroupID,
                    SiteID: entry.SiteID,
                    GameGroupID: entry.GameGroupID

                });
                cachedData[posSport].CurrencyGroups[posCur].SiteGroups[posSite].GameGroups.forEach((gameGroup, index) => {
                    CMS.clearTipsyError($(`#minbet-${posSport}-${posCur}-${posSite}-${index}`));
                    CMS.clearTipsyError($(`#maxbet-${posSport}-${posCur}-${posSite}-${index}`));
                    CMS.clearTipsyError($(`#remark-${posSport}-${posCur}-${posSite}-${index}`));
                });
            });

            CMS.showConfirmMessage(
                'Are you sure you want to delete this setting?',
                function () {
                    CMS.disableInput();
                    CMS.callAjax(
                        this,
                        CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Delete?isCredit=" + elements.isCredit),
                        requestBody,
                        function (response) {
                            var result = typeof response === "string" ? JSON.parse(response) : response;
                            if (result.IsSuccess) {
                                CMS.showSuccessMessage("Delete successfully!", getData);
                                $(`.col-view-sport-${posSport}-cur-${posCur}-site-${posSite}`).removeClass("hidden");
                                $(`.col-edit-sport-${posSport}-cur-${posCur}-site-${posSite}`).each(function () {
                                    if (!$(this).hasClass("hidden")) {
                                        $(this).addClass("hidden");
                                    }
                                });
                            } else {
                                CMS.showErrorMessage(result.Message);
                            }
                        });
                }
            );
            
        });
    }

    function renderMultiChoiceDDL() {
        const depositSelectors = [$("#ddl-deposit-products"), $("#ddl-deposit-currencies"), $("#ddl-deposit-sites")];
        const creditSelectors = [$("#ddl-credit-products"), $("#ddl-credit-currencies"), $("#ddl-credit-sites")];
        const multiSelectOptions = {
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true
        };
        const generateListData = (ddl, prefixId) => {
            return ddl.find("option").map(function () {
                const { text, value } = this;
                return {
                    Id: `${prefixId}-${text}-${value}`,
                    Value: value,
                    Text: text,
                    Checked: true,
                    Enable: true
                };
            });
        };
        depositSelectors.forEach(e => e.multiSelect({ ...multiSelectOptions, listData: generateListData(e, "credit") }));
        creditSelectors.forEach(e => e.multiSelect({ ...multiSelectOptions, listData: generateListData(e, 'deposit') }));
    }

    function registerRadiobuttonHandler() {
        elements.rdbRestriction.change(function () {
            const selectedValue = $(this).val();
            const creditSelectors = [$("#credit-products"), $("#credit-currencies"), $("#credit-sites")];
            const depositSelectors = [$("#deposit-products"), $("#deposit-currencies"), $("#deposit-sites")];

            const isCredit = selectedValue === 'Credit';
            elements.isCredit = isCredit;

            creditSelectors.forEach(e => e.toggleClass("display-none", !isCredit));
            depositSelectors.forEach(e => e.toggleClass("display-none", isCredit));
        });
        if ($(elements.rdbRestriction[1]).is(':checked')) {
            $(elements.rdbRestriction[1]).change();
        } else {
            $(elements.rdbRestriction[0]).change();
        }
    }

    function registerSubmitHandler() {
        $("#formCompanyBetLimitSetting").submit(function (event) {
            event.preventDefault();
            cachedFilterData = getFilterParams(elements.isCredit);
            getData();
        });
    }

    function handleNumberCommanTextInput(e) {
        const keyCodeArray = [46, 8, 9, 27, 13, 188, 110, 190];
        if (keyCodeArray.indexOf(e.keyCode) !== -1) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    }

    function formatNumber(value) {
        return Number.isInteger(value) ? intFormatter.format(value) : doubleFormatter.format(value);
    }

    this.InitPage = function () {
        elements.hasHostPermission = $("#addNewCurrencyGroupForm").length > 0;
        registerSubmitHandler();
        registerRadiobuttonHandler();
        renderMultiChoiceDDL();
    };
};

