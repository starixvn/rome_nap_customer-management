"use strict";
(function ($) {
    let elements = {
        isCredit: true,
        rdbRestriction: $("[name=Restriction]"),
    };

    const getCurrencyLabel = (isCredit) => isCredit ? viewResources.lblCreditCurrency : viewResources.lblDepositCurrency;
    const getSiteLabel = (isCredit) => isCredit ? viewResources.lblCreditSite : viewResources.lblDepositSite;

    const getFilterParams = (isCredit) => {
        const filterParams = {
            IsCredit: true,
            ProductIds: "",
            Currencies: "",
            Sites: "",
        };
        const productsDdl = isCredit ? $("#ddl-credit-products") : $("#ddl-deposit-products");
        const currenciesDdl = isCredit ? $("#ddl-credit-currencies") : $("#ddl-deposit-currencies");
        const sitesDdl = isCredit ? $("#ddl-credit-sites") : $("#ddl-deposit-sites");
        filterParams.ProductIds = productsDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Currencies = currenciesDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Sites = sitesDdl.multiSelect("getCheckedItems").join(",");
        filterParams.IsCredit = isCredit;

        return filterParams;
    };

    const getGridOptions = (isCredit) => {
        const currencyLabel = getCurrencyLabel(isCredit);
        const siteLabel = getSiteLabel(isCredit);
        const headerColumns = [
            {
                HeaderText: viewResources.lblSymbol_No,
                Name: "ColNo",
                Width: 50,
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.lblSportTypeID,
                Name: "SportType",
                CellAlign: "center",
                Width: 80,
                HeaderAlign: "center",
            },
            {
                HeaderText: viewResources.lblSportTypeName,
                Name: "SportName",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
            },
            {
                HeaderText: currencyLabel,
                Name: "Currency",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: siteLabel,
                Name: "Site",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.lblGameGroup,
                Name: "GameGroups",
                Width: 120,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
            },
            {
                HeaderText: viewResources.lblMinBet,
                Width: 90,
                Name: "MinBet",
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                FormatNumber: CMS.formats.DecimalFormat
            },
            {
                HeaderText: viewResources.lblMaxBet,
                Width: 90,
                Name: "MaxBet",
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                FormatNumber: CMS.formats.IntegerFormat
            },
            {
                HeaderText: viewResources.lblRemark,
                Width: 160,
                Name: "Remark",
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
            },
            // {
            //     HeaderText: viewResources.lblEdit,
            //     Name: "IconEdit",
            //     Width: 40,
            //     HeaderAlign: "center",
            //     CellAlign: "center",
            // }, {
            //     HeaderText: viewResources.lblRemove,
            //     Name: "IconRemove",
            //     Width: 40,
            //     HeaderAlign: "center",
            //     CellAlign: "center",
            // },
        ];
        return {
            columns: headerColumns,
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        {HeaderText: viewResources.lblSymbol_No, Name: "ColNo", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblSportTypeID, Name: "SportType", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblSportTypeName, Name: "SportName", MergeRowNumber: 2},
                        {HeaderText: currencyLabel, Name: "Currency", MergeRowNumber: 2},
                        {HeaderText: siteLabel, Name: "Site", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblGameGroup, Name: "GameGroups", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblBetLimit, Name: "MinBet", MergeColNumber: 3},
                        // {HeaderText: viewResources.lblEdit, Name: "IconEdit", MergeRowNumber: 2},
                        // {HeaderText: viewResources.lblRemove, Name: "IconRemove", MergeRowNumber: 2},
                    ]
                }
            ],
            showTitle: true,
            titleInfo:{
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    const url = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Export");
                    const isCredit = getFilterParams(elements.isCredit);
                    CMS.downloadFile(url, isCredit);
                }
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 27,
            showNumberColumn: true,
            gridExpandHeight: 180,
            showHighlightRow: true,
            showPager: false,
            padding: 100,
        };
    }
    
    const renderGrid = (data) => {
        const bodyData = [];
        let row;
        let currentRow = 0;
        let currentCurrencyGroupRow;
        let currentSportGroupRow;
        let currentSiteRow;
        let sportGroupChildren;
        let currencyGroupChildren;
        let highlightClass;
        const editCol = `<span class="ico-edit" data-item-index={0} title={1}></span>`;
        const delCol = `<span class="ico-delete" data-request-id={0} title={1}></span>`;

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
                            row = {
                                Columns: [
                                    {Name: 'ColNo', Value: ""},
                                    {Name: 'GameGroups', Value: gameGroup.GameGroupName, TdClass: highlightClass},
                                    {Name: 'MinBet', Value: gameGroup.Entries[0]?.MinBet, TdClass: highlightClass},
                                    {Name: 'MaxBet', Value: gameGroup.Entries[0]?.MaxBet, TdClass: highlightClass},
                                    {Name: 'Remark', Value: gameGroup.Entries[0]?.Remark, TdClass: highlightClass},
                                ]
                            };

                            bodyData.push(row);
                            currentRow++;
                        });
                        row = bodyData[currentSiteRow];
                        row.Columns.splice(1, 0, {
                            Name: 'Site',
                            Value: siteGroup.SiteGroupName,
                            MergeRowNumber: siteGroup.GameGroups.length,
                        });
                        // row.Columns.push(
                        //     {
                        //         Name: 'IconEdit',
                        //         Value: editCol.format(currencyGroupIndex, "Edit"),
                        //         MergeRowNumber: currencyGroupChildren
                        //     },
                        //     {
                        //         Name: 'IconRemove',
                        //         Value: editCol.format(currencyGroupIndex, "Delete"),
                        //         MergeRowNumber: currencyGroupChildren
                        //     }
                        // )
                    });
                    row = bodyData[currentCurrencyGroupRow];
                    row.Columns.splice(1, 0, {
                        Name: 'Currency',
                        Value: currencyGroup.Currency,
                        MergeRowNumber: currencyGroupChildren,
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

    const getData = (params) => {
        const apiUrl = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/Search?" + $.param(params));
        CMS.getJson(apiUrl, (data) => {
            const jsonData = $.parseJSON(data);
            const gridOptions = getGridOptions(params.IsCredit);
            gridOptions.bodyRows = renderGrid(jsonData);
            $("#grid-table").removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    const renderMultiChoiceDDL = () => {
        const depositSelectors = [$("#ddl-deposit-products"), $("#ddl-deposit-currencies"), $("#ddl-deposit-sites")];
        const creditSelectors = [$("#ddl-credit-products"), $("#ddl-credit-currencies"), $("#ddl-credit-sites")];
        const multiSelectOptions = {
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        };
        const generateListData = (ddl, prefixId) => {
            return ddl.find("option").map(function () {
                const {text, value} = this;
                return {
                    Id: `${prefixId}-${text}-${value}`,
                    Value: value,
                    Text: text,
                    Checked: true,
                    Enable: true
                };
            });
        };
        depositSelectors.forEach(e => e.multiSelect({...multiSelectOptions, listData: generateListData(e,"credit")}));
        creditSelectors.forEach(e => e.multiSelect({...multiSelectOptions, listData: generateListData(e,'deposit')}));
    };
    
    const registerRadiobuttonHandler = () => {
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

    const registerSubmitHandler = () => {
        $("#formCompanyBetLimitSetting").submit(function (event) {
            event.preventDefault();
            const params = getFilterParams(elements.isCredit);
            getData(params);
        });
    };

    $(document).ready(function () {
        registerSubmitHandler();
        registerRadiobuttonHandler();
        renderMultiChoiceDDL();
    });

})(jQuery);

