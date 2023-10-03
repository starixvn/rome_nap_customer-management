"use strict";
(function ($) {
    let elements = {

        //old code
        resultMessageDiv: $("#div-result-message"),
        table: $("#chatBots-list"),
        formChatBotConfig: $("#formChatBotConfig"),
        chatIDTextbox: $("#txt-chatID"),
        userNameSubAccountTextbox: $("#txt-userNameSubAccount"),
        statusDropdown: $("#ddl-status"),
        levelDropdown: $("#ddl-level"),
        linkStatusDropdown: $("#ddl-linkingStatus"),
        subAccountOnly: $("#currencies-deposit"),
        //new code

        rdbRestriction: $("[name=Restriction]"),
        
    };
    let filterParams = {};
    let gridOptions = null;
    // var cachePopupElements = function () {
    //     // popupElements.divErrorSuccessMsg = $('#divErrorSuccessMsg', mainDOMElements.divPopup);
    //     // popupElements.divForm = $('#divRequestForm', mainDOMElements.divPopup);
    //     // popupElements.hdnRequestId = $('#hdnRequestId', mainDOMElements.divPopup);
    //     // popupElements.hdnCustId = $('#hdnCustId', mainDOMElements.divPopup);
    //     // popupElements.hdnExpiredDate = $('#hdnExpiredDate', mainDOMElements.divPopup);
    //     // popupElements.txtUsername = $('#txtUsername', mainDOMElements.divPopup);
    //     // popupElements.rdbRestriction = $('[name=Restriction]', mainDOMElements.divPopup);
    //     // popupElements.ddlCountry = $('#ddlCountry', mainDOMElements.divPopup);
    //     // popupElements.txtIP = $('#txtIP', mainDOMElements.divPopup);
    //     // popupElements.dpkExpiredDate = $('#dpkExpiredDate', mainDOMElements.divPopup);
    //     // popupElements.chkUnlimited = $('#chkUnlimited', mainDOMElements.divPopup);
    //     // popupElements.txtRemark = $('#txtRemark', mainDOMElements.divPopup);
    //     // popupElements.btnSave = $('#btnSave', mainDOMElements.divPopup);
    //     // popupElements.btnCancel = $('#btnCancel', mainDOMElements.divPopup);
    // };

    const getFilterParams = () => {
        filterParams.Username = $.trim(elements.userNameSubAccountTextbox.val());
        filterParams.ChatID = $.trim(elements.chatIDTextbox.val());
        filterParams.IsSubAccOnly = elements.subAccountOnly.prop('checked');
        filterParams.Level = elements.levelDropdown.val();
        filterParams.AccStatus = elements.statusDropdown.val();
        filterParams.LinkStatus = elements.linkStatusDropdown.val();
        filterParams.Currency = currencyDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Site = siteDdl.multiSelect("getCheckedItemsByText").join(",");
        filterParams.BotIDs = chatBotNamesDdl.multiSelect("getCheckedItems").join(",");
        filterParams.PageIndex = 0;
        filterParams.PageSize = 1000;
        filterParams.Cache = false;
        return filterParams;
    };

    const getGridOptions = () => {
        const headerColumns = [
            {
                HeaderText: viewResources.Symbol_No,
                Width: 50,
                Name: "ColNo",
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.ChatBotName,
                Width: 140,
                Name: "ChatBotName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                AllowTruncate: true,
            },
            {
                HeaderText: viewResources.ChatID,
                Width: 90,
                Name: "ChatID",
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.Site,
                Width: 120,
                Name: "SiteName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                AllowTruncate: true,
            },
            {
                HeaderText: viewResources.UserName,
                Width: 180,
                Name: "UserName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                AllowTruncate: true,
            },
            {
                HeaderText: viewResources.SubAccount,
                Width: 180,
                Name: "SubAccount",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                AllowTruncate: true,
            }, {
                HeaderText: viewResources.LinkingStatus,
                Width: 100,
                Name: "LinkingStatus",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true,
            }, {
                HeaderText: viewResources.Language,
                Width: 120,
                Name: "Language",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true,
            }, {
                HeaderText: viewResources.Level,
                Width: 80,
                Name: "Level",
                HeaderAlign: "center",
                CellAlign: "center",
            }, {
                HeaderText: viewResources.Currency,
                Width: 70,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center",
            }, {
                HeaderText: viewResources.AccStatus,
                Width: 80,
                Name: "Status",
                HeaderAlign: "center",
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.DateGMT4,
                Width: 150,
                Name: "DateGMT4",
                HeaderAlign: "center",
                Sort: true,
                CellAlign: "center",
            },
            {
                HeaderText: viewResources.Config,
                Width: 60,
                Name: "Config",
                HeaderAlign: "center",
                CellAlign: "center",
            }
        ];

        return {
            columns: headerColumns,
            showTitle: false,
            titleInfo: {
                showExcelButton: false
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: true,
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
            showNumberColumn: true,
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

    const renderGridChatBots = (data) => {
        const bodyData = [];
        let columns;
        for (var i = 0, max = data.length; i < max; i++) {
            const {
                ChatBotName,
                ChatID,
                LinkStatus,
                UserName,
                SubAccount,
                Language,
                Level,
                Currency,
                AccStatus,
                CusID,
                BotID,
                SubAccID,
                UserID,
                UpdatedTimeStr,
                SiteName
            } = data[i];
            const linkStatusFormatted = `<span class="high-light ${LinkStatus}" >${LinkStatus}</span>`
            columns = [
                {Name: "ColNo", Value: ""},
                {Name: "ChatBotName", Value: ChatBotName},
                {Name: "ChatID", Value: ChatID},
                {Name: "SiteName", Value: SiteName},
                {Name: "UserName", Value: UserName},
                {Name: "SubAccount", Value: SubAccount},
                {Name: "LinkingStatus", Value: linkStatusFormatted},
                {Name: "Language", Value: Language},
                {Name: "Level", Value: Level},
                {Name: "Currency", Value: Currency},
                {Name: "Status", Value: AccStatus},
                {Name: "DateGMT4", Value: UpdatedTimeStr},
                {Name: "Config", Value: renderConfigCell(LinkStatus, CusID, BotID, SubAccID, UserID)}
            ];
            bodyData.push({Columns: columns});
        }

        return bodyData;
    }

    const getData = (params) => {
        // const resetSortDirection = () => {
        //     $.each(gridOptions.columns, function (index, value) {
        //         value.SortDirection = value.Name !== filterParams.SortField
        //             ? ""
        //             : filterParams.SortDirection;
        //     });
        // }
        //
        // CMS.getJson(CMS.resolveRootUrl("ChatbotMapping/SearchChatBots?" + $.param(filterParams)), function (data) {
        //     let jsonData = $.parseJSON(data);
        //     gridOptions = gridOptions || getGridOptions();
        //     gridOptions.bodyRows = renderGridChatBots(jsonData.ChatBots);
        //
        //     gridOptions.pagerOption.currentPage = filterParams.PageIndex;
        //     gridOptions.pagerOption.itemsPerPage = filterParams.PageSize;
        //     gridOptions.pagerOption.totalItem = jsonData.TotalItems;
        //
        //     if (!filterParams.Cache) {
        //         resetSortDirection();
        //     }
        //
        //     elements.table.removeClass("display-none").fanexGrid(gridOptions);
        // });
    }

    const renderMultiChoiceDDL = () => {
        // Get the HTML elements for the two drop-down lists.
        const creditProducts = $("#ddl-credit-products");
        const creditCurrencies = $("#ddl-deposit-currencies");

        const depositProducts = $("#ddl-deposit-products");
        const depositCurrencies = $("#ddl-credit-currencies");

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

        $("#formChatbotMapping").submit(function (event) {
            event.preventDefault();
            const params = getFilterParams();
            getData(params);
        });

        elements.rdbRestriction.change(function () {
            const selectedValue = $(this).val();
            const creditProducts = $("#credit-products");
            const creditCurrencies = $("#deposit-currencies");
            const depositProducts = $("#deposit-products");
            const depositCurrencies = $("#credit-currencies");
            if (selectedValue === 'Credit') {
                creditCurrencies.removeClass("display-none");
                creditProducts.removeClass("display-none");

                depositCurrencies.addClass("display-none");
                depositProducts.addClass("display-none");
            } else {
                depositCurrencies.removeClass("display-none");
                depositProducts.removeClass("display-none");

                creditCurrencies.addClass("display-none");
                creditProducts.addClass("display-none");
            }
        });

        renderMultiChoiceDDL();
    });
})(jQuery);