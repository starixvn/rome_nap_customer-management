"use strict";
(function ($) {
    let elements = {
        isCredit: true,
        rdbRestriction: $("[name=Restriction]"),
    };
    const getCurrencyLabel = (isCredit) =>  isCredit ? viewResources.lblCreditCurrency : viewResources.lblDepositCurrency;

    const getFilterParams = (isCredit) => {
        const filterParams = {
            IsCredit: isCredit,
            ProductIds: "",
            Currencies: ""
        };
        const productsDdl = isCredit ? $("#ddl-credit-products") : $("#ddl-deposit-products");
        const currenciesDdl = isCredit ? $("#ddl-credit-currencies") : $("#ddl-deposit-currencies");
        filterParams.ProductIds = productsDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Currencies = currenciesDdl.multiSelect("getCheckedItems").join(",");

        return filterParams;
    };

    const getGridOptions = (isCredit) => {
        const currencyLabel = getCurrencyLabel(isCredit);
        const headerColumns = [
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
            },
            {
                HeaderText: viewResources.lblMaxBet,
                Width: 90,
                Name: "MaxBet",
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
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
        const titleInfo = {
            text: "",
            showExcelButton: true,
            exportExcelFunction: function () {
                const url = CMS.resolveRootUrl("CompanyBetLimitSetting/Export");
                const isCredit = getFilterParams(elements.isCredit);
                CMS.downloadFile(url, isCredit);
            }
        };
        return {
            columns: headerColumns,
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        {HeaderText: viewResources.lblSportTypeID, Name: "SportType", MergeRowNumber: 2},
                        {HeaderText: "Sport Type Name", Name: "SportName", MergeRowNumber: 2},
                        {HeaderText: currencyLabel, Name: "Currency", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblGameGroup, Name: "GameGroups", MergeRowNumber: 2},
                        {HeaderText: viewResources.lblBetLimit, Name: "MinBet", MergeColNumber: 3},
                        // {HeaderText: viewResources.lblEdit, Name: "IconEdit", MergeRowNumber: 2},
                        // {HeaderText: viewResources.lblRemove, Name: "IconRemove", MergeRowNumber: 2},
                    ]
                }
            ],
            showTitle: false,
            titleInfo,
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: false,
            showNumberColumn: false,
            gridExpandHeight: 180,
            showHighlightRow: false
        };
    }

    const renderConfigCell = (linkStatus, custId, botId, subAccId, userId) => {
        if (linkStatus === "Unlinked") return "";
        return [
            '<span class="ui-icon ico-config" href="javascript:;" data-custId="',
            custId,
            '" data-botId="',
            botId,
            '" data-userId="',
            userId,
            '" data-subAccId="',
            subAccId,
            '"></span>'
        ].join("");
    }

    const renderGridBody = (data) => {
        const bodyData = [];
        let row;
        let currentRow = 0;
        let currentCurrencyGroupRow;
        let currentSportGroupRow;
        let sportGroupChildren;
        let downlineList;
        let highlightClass;
        const editCol = `<span class="ico-edit" data-item-index={0} title={1}></span>`;
        const delCol = `<span class="ico-delete" data-request-id={0} title={1}></span>`;
        if (data.length > 0) {

            $.each(data, function (sportGroupIndex, sportGroup) {
                sportGroupChildren = 0;
                currentSportGroupRow = currentRow;
                $.each(sportGroup.CurrencyGroups, function (currencyGroupIndex, currencyGroup) {
                    sportGroupChildren += currencyGroup.GameGroups.length;
                    currentCurrencyGroupRow = currentRow;
                    $.each(currencyGroup.GameGroups, function (index, gameGroup) {
                        row = {
                            Columns: [
                                {Name: 'GameGroups', Value: gameGroup.GameGroupName},
                                {Name: 'MinBet', Value: gameGroup.Entries[0]?.MinBet},
                                {Name: 'MaxBet', Value: gameGroup.Entries[0]?.MaxBet},
                                {Name: 'Remark', Value: gameGroup.Entries[0]?.Remark},
                            ]
                        };

                        bodyData.push(row);
                        currentRow++;
                    });
                    row = bodyData[currentCurrencyGroupRow];
                    row.Columns.splice(0, 0, {
                        Name: 'Currency',
                        Value: currencyGroup.Currency,
                        MergeRowNumber: currencyGroup.GameGroups.length,
                    });
                    // row.Columns.push(
                    //     {
                    //         Name: 'IconEdit',
                    //         Value: editCol.format(currencyGroupIndex, "Edit"),
                    //         MergeRowNumber: currencyGroup.GameGroups.length
                    //     },
                    //     {
                    //         Name: 'IconRemove',
                    //         Value: editCol.format(currencyGroupIndex, "Delete"),
                    //         MergeRowNumber: currencyGroup.GameGroups.length
                    //     }
                    // )
                });
                row = bodyData[currentSportGroupRow];
                row.Columns.splice(0, 0, ...[
                    {
                        Name: 'SportType',
                        Value: sportGroup.SportType,
                        MergeRowNumber: sportGroupChildren,
                        // CssClass: 'chk-column'
                    },
                    {
                        Name: 'SportName',
                        Value: sportGroup.SportName,
                        MergeRowNumber: sportGroupChildren,
                        // CssClass: 'chk-column'
                    }
                ]);
            });
        }
        return bodyData;
    }

    const getData = (params) => {
        CMS.getJson(CMS.resolveRootUrl("CompanyBetLimitSetting/Search?" + $.param(params)), function (data) {
            let jsonData = $.parseJSON(data);
            console.log(jsonData)
            const gridOptions = getGridOptions(params.IsCredit);
            gridOptions.bodyRows = renderGridBody(jsonData);

            $("#grid-table").removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    const renderMultiChoiceDDL = () => {
        const creditProducts = $("#ddl-credit-products");
        const creditCurrencies = $("#ddl-credit-currencies");

        const depositProducts = $("#ddl-deposit-products");
        const depositCurrencies = $("#ddl-deposit-currencies");

        const multiSelectOptions = {
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        };
        const generateListData = (ddl) => {
            return ddl.find("option").map(function () {
                const {text, value} = this;
                return {
                    Id: `${text}-${value}`,
                    Value: value,
                    Text: text,
                    Checked: true,
                    Enable: true
                };
            });
        };

        creditProducts.multiSelect({
            ...multiSelectOptions,
            listData: generateListData(creditProducts),
        });

        creditCurrencies.multiSelect({
            ...multiSelectOptions,
            listData: generateListData(creditCurrencies),
        });

        depositProducts.multiSelect({
            ...multiSelectOptions,
            listData: generateListData(depositProducts),
        });

        depositCurrencies.multiSelect({
            ...multiSelectOptions,
            listData: generateListData(depositCurrencies),
        });
    };

    

    const registerRadiobuttonHandler = () => {
        elements.rdbRestriction.change(function () {
            const selectedValue = $(this).val();
            const creditProducts = $("#credit-products");
            const creditCurrencies = $("#credit-currencies");
            const depositProducts = $("#deposit-products");
            const depositCurrencies = $("#deposit-currencies");
            const lblCurrency = $("#lbl-currency");
            const isCredit = selectedValue === 'Credit';
            lblCurrency.text(getCurrencyLabel(isCredit));
            creditProducts.toggleClass("display-none", !isCredit);
            creditCurrencies.toggleClass("display-none", !isCredit);
            depositProducts.toggleClass("display-none", isCredit);
            depositCurrencies.toggleClass("display-none", isCredit);
            elements.isCredit = isCredit;
        });
    }

    const registerSubmitHandler = () => {
        $("#formCompanyBetLimitSetting").submit(function (event) {
            event.preventDefault();
            const params = getFilterParams(elements.isCredit);
            getData(params);
        });
    };

    $(function () {
        registerSubmitHandler();
        registerRadiobuttonHandler();
        renderMultiChoiceDDL();
    });
})(jQuery);