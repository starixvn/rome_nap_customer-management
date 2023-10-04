"use strict";
(function ($) {
    let elements = {
        //new code
        isCredit: true,
        rdbRestriction: $("[name=Restriction]"),

    };

    let gridOptions = null;

    const getFilterParams = (isCredit) => {
        const filterParams = {};

        filterParams.IsCredit = isCredit;

        if (isCredit) {
            const creditProductsDdl = $("#ddl-credit-products");
            const creditCurrenciesDdl = $("#ddl-deposit-currencies");
            filterParams.ProductIds = creditProductsDdl.multiSelect("getCheckedItems").join(",")
            filterParams.Currencies = creditCurrenciesDdl.multiSelect("getCheckedItems").join(",")
        } else {
            const depositProductsDdl = $("#ddl-deposit-products");
            const depositCurrenciesDdl = $("#ddl-credit-currencies");
            filterParams.ProductIds = depositProductsDdl.multiSelect("getCheckedItems").join(",")
            filterParams.Currencies = depositCurrenciesDdl.multiSelect("getCheckedItems").join(",")
        }
        return filterParams;
    };

    const getGridOptions = (isCredit) => {
        const headerColumns = [
            {HeaderText: "Sport Type ID", Name: "SportType", CellAlign: "center", Width: 80, HeaderAlign: "center",},
            {
                HeaderText: "Sport Type Name",
                Name: "SportName",
                Width: 140,
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            },
            {
                HeaderText: isCredit ? "Credit Currency" : "Deposit Currency",
                Name: "Currency",
                Width: 90,
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: "Game Group",
                Name: "GameGroups",
                Width: 120,
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            },
            {
                HeaderText: "Min Bet",
                Width: 180,
                Name: "MinBet",
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            },
            {
                HeaderText: "Max Bet",
                Width: 180,
                Name: "MaxBet",
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            },
            {
                HeaderText: "Remark",
                Width: 180,
                Name: "Remark",
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            },
            {
                HeaderText: "Edit",
                Name: "Edit",
                Width: 180,
                HeaderAlign: "center",
                CellAlign: "left",
                AllowTruncate: true,
            }, {
                HeaderText: "Remove",
                Name: "Remove",
                Width: 100,
                HeaderAlign: "center",
                CellAlign: "center",
            },
        ];

        return {
            columns: headerColumns,
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        {HeaderText: "Sport Type ID", Name: "SportType", MergeRowNumber: 2},
                        {HeaderText: "Sport Type Name", Name: "SportName", MergeRowNumber: 2},
                        {
                            HeaderText: isCredit ? "Credit Currency" : "Deposit Currency",
                            Name: "Currency",
                            MergeRowNumber: 2
                        },
                        {HeaderText: "Game Group", Name: "GameGroups", MergeRowNumber: 2},
                        {HeaderText: "Group Header 1", Name: "MinBet", MergeColNumber: 3},
                        {HeaderText: "Edit", Name: "Edit", MergeRowNumber: 2},
                        {HeaderText: "Remove", Name: "Remove", MergeRowNumber: 2},
                    ]
                }
            ],
            showTitle: false,
            titleInfo: {
                showExcelButton: false
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: false,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: [100, 200, 500, 1000, 2000],
                totalItem: 0,
                pagerSelectCallBack: function (index, pageSize) {
                    filterParams.PageIndex = index;
                    filterParams.PageSize = pageSize;
                    filterParams.Cache = true;
                    getChatBot();
                }
            },
            showNumberColumn: false,
            gridExpandHeight: 180,
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
        let currentAgentRow;
        let currentMasterRow;
        let masterChildren;
        let downlineList;
        let highlightClass;

        if (data.length > 0) {

            $.each(data, function (masterIndex, sport) {
                masterChildren = 0;
                currentMasterRow = currentRow;
                $.each(sport.CurrencyGroups, function (agentIndex, agent) {
                    masterChildren += agent.GameGroups.length;
                    currentAgentRow = currentRow;
                    $.each(agent.GameGroups, function (index, item) {
                        row = {
                            Columns: [
                                {Name: 'GameGroups', Value: item.GameGroup,},
                                {Name: 'MinBet', Value: item.Entries[0]?.MinBet,},
                                {Name: 'MaxBet', Value: item.Entries[0]?.MaxBet,},
                                {Name: 'Remark', Value: item.Entries[0]?.Remark,},
                                {Name: 'Edit', Value: '',},
                                {Name: 'Remove', Value: '',},
                            ]
                        };

                        bodyData.push(row);
                        currentRow++;
                    });
                    row = bodyData[currentAgentRow];
                    row.Columns.splice(0, 0, {
                        Name: 'Currency',
                        Value: agent.Currency,
                        MergeRowNumber: agent.GameGroups.length,
                        // CssClass: 'chk-column'
                    });
                });
                row = bodyData[currentMasterRow];
                row.Columns.splice(0, 0, ...[
                    {
                        Name: 'SportType',
                        Value: sport.SportType,
                        MergeRowNumber: masterChildren,
                        // CssClass: 'chk-column'
                    },
                    {
                        Name: 'SportName',
                        Value: sport.SportName,
                        MergeRowNumber: masterChildren,
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
            gridOptions = getGridOptions(params.IsCredit);
            gridOptions.bodyRows = renderGridBody(jsonData);
           
            $("#grid-table").removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    const renderMultiChoiceDDL = () => {
        // Get the HTML elements for the two drop-down lists.
        const creditProducts = $("#ddl-credit-products");
        const creditCurrencies = $("#ddl-credit-currencies");

        const depositProducts = $("#ddl-deposit-products");
        const depositCurrencies = $("#ddl-deposit-currencies");

        // Generate the list data for the drop-down lists.
        const listDataGenerator = (ddl) => {
            return ddl.find("option").map(function () {
                return {
                    Id: this.text + "-" + this.value,
                    Value: this.value,
                    Text: this.text,
                    Checked: true,
                    Enable: true
                };
            });
        };
        
        creditProducts.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: listDataGenerator(creditProducts),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });

        creditCurrencies.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: listDataGenerator(creditCurrencies),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });

        depositProducts.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: listDataGenerator(depositProducts),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });

        depositCurrencies.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: listDataGenerator(depositCurrencies),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });
    };

    $(function () {

        $("#formCompanyBetLimitSetting").submit(function (event) {
            event.preventDefault();
            const params = getFilterParams(elements.isCredit);
            getData(params);
        });

        elements.rdbRestriction.change(function () {
            const selectedValue = $(this).val();
            const creditProducts = $("#credit-products");
            const creditCurrencies = $("#credit-currencies");
            const depositProducts = $("#deposit-products");
            const depositCurrencies = $("#deposit-currencies");
            if (selectedValue === 'Credit') {
                creditProducts.removeClass("display-none");
                creditCurrencies.removeClass("display-none");

                depositProducts.addClass("display-none");
                depositCurrencies.addClass("display-none");
                elements.isCredit = true;
            } else {
                depositProducts.removeClass("display-none");
                depositCurrencies.removeClass("display-none");

                creditProducts.addClass("display-none");
                creditCurrencies.addClass("display-none");
                elements.isCredit = false;
            }
        });

        renderMultiChoiceDDL();
    });
})(jQuery);