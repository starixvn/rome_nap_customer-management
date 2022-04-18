"use strict";
(function ($) {
    var elements = {
        resultMessageDiv: $("#div-result-message"),
        table: $("#chatBots-list"),
        formChatBotConfig: $("#formChatBotConfig"),
        chatIDTextbox: $("#txt-chatID"),
        userNameSubAccountTextbox: $("#txt-userNameSubAccount"),
        statusDropdown: $("#ddl-status"),
        levelDropdown: $("#ddl-level"),
        linkStatusDropdown: $("#ddl-linkingStatus"),
        subAccountOnly: $("#chk-subAccountsOnly")
    };
    var currencyDdl = $("#ddl-currency"),
        siteDdl = $("#ddl-site");
    var filterParams = {};
    var gridOptions = null;

    const optionsLinkingStatusDDL = [
        {Id: 0, Name: "All"},
        {Id: 1, Name: "Linked"},
        {Id: 2, Name: "Verifying"}
    ];

    const optionsLevelDDL = [
        {Id: 0, Name: "All"},
        {Id: 4, Name: "Super"},
        {Id: 3, Name: "Master"},
        {Id: 2, Name: "Agent"}];

    const optionsAccStatusDDL = [
        {Id: 0, Name: "All"},
        {Id: 1, Name: "Active"},
        {Id: 2, Name: "Closed"},
        {Id: 3, Name: "Suspended"}];


    const resetFilterParams = () => {
        filterParams.Username = $.trim(elements.userNameSubAccountTextbox.val());
        filterParams.ChatID = $.trim(elements.chatIDTextbox.val());
        filterParams.IsSubAccOnly = elements.subAccountOnly.val();
        filterParams.Level = elements.levelDropdown.val();
        filterParams.AccStatus = elements.statusDropdown.val();
        filterParams.LinkStatus = elements.linkStatusDropdown.val();
        filterParams.Currency = currencyDdl.multiSelect("getCheckedItems").join(",");
        filterParams.Site = siteDdl.multiSelect("getCheckedItemsByText").join(",");
        filterParams.PageIndex = 0;
        filterParams.PageSize = 100;
        filterParams.Cache = false;
        //debugger
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
                Width: 150,
                Name: "ChatBotName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true
            },
            {
                HeaderText: viewResources.ChatID,
                Width: 160,
                Name: "ChatID",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
            },
            {
                HeaderText: viewResources.UserName,
                Width: 160,
                Name: "UserName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
            }, {
                HeaderText: viewResources.SubAccount,
                Width: 160,
                Name: "SubAccount",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
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
                Width: 80,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center",
            }, {
                HeaderText: viewResources.AccStatus,
                Width: 80,
                Name: "Status",
                HeaderAlign: "center",
                CellAlign: "center",
            }, {
                HeaderText: viewResources.Config,
                Width: 80,
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
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                totalItem: 0,
                pagerSelectCallBack: function (index, pageSize) {
                    console.log("index, pageSize", index, pageSize)
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

    const renderConfigCell = (custId, botId, subAccId) => {
        return [
            '<span class="ui-icon ui-icon-gear ico-config" href="javascript:;" data-custId="',
            custId,
            '" data-botId="',
            botId,
            '" data-subAccId="',
            subAccId,
            '"></span>'
        ].join("");
    }
    const renderChatIDCell=(chatId)=>{
        return [
            '<span class="ui-icon ui-icon-document tool-tip" href="javascript:;" title="',
            chatId,
            '"></span>'
        ].join("");
        $(".tool-tip").tooltip({
            position: {
                my: "left top",
                at: "right+5 top-5",
                collision: "none"
            },
            tooltipClass: "custom-tooltip-styling",
            create: function( event, ui ) {
                console.log("tooltip opened")
            }
        });
    }

    const renderGridChatBots = (data) => {
        const bodyData = [];
        let columns;
        //ui-icon-gear
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
                SubAccID
            } = data[i];
            const linkStatusCssColor = LinkStatus == "Verifying" ? "yellow" : "green";
            const linkStatusFormatted = `<span class="high-light ${linkStatusCssColor}" >${LinkStatus}</span>`
            columns = [
                {Name: "ColNo", Value: ""},
                {Name: "ChatBotName", Value: ChatBotName},
                {Name: "ChatID", Value: renderChatIDCell(ChatID)},
                {Name: "UserName", Value: UserName},
                {Name: "SubAccount", Value: SubAccount},
                {Name: "LinkingStatus", Value: linkStatusFormatted},
                {Name: "Language", Value: Language},
                {Name: "Level", Value: Level},
                {Name: "Currency", Value: Currency},
                {Name: "Status", Value: AccStatus},
                {Name: "Config", Value: renderConfigCell(CusID, BotID, SubAccID)}
            ];
            const css = (i & 1) == 1 ? 'freegrid-even' : '';
            bodyData.push({Columns: columns, CssClass: css});
        }

        return bodyData;
    }

    const getChatBot = () => {
        //   CMS.hideInlineMsg(elements.resultMessageDiv);
        const resetSortDirection = () => {
            $.each(gridOptions.columns, function (index, value) {
                value.SortDirection = value.Name !== filterParams.SortField
                    ? ""
                    : filterParams.SortDirection;
            });
        }

        CMS.getJson(CMS.resolveRootUrl("ChatbotMapping/SearchChatBots?" + $.param(filterParams)), function (data) {
            var jsonData = $.parseJSON(data);
            console.log(jsonData)
            gridOptions = gridOptions || getGridOptions();
            gridOptions.bodyRows = renderGridChatBots(jsonData.ChatBots);

            gridOptions.pagerOption.currentPage = filterParams.PageIndex;
            gridOptions.pagerOption.itemsPerPage = filterParams.PageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalItems;

            if (!filterParams.Cache) {
                resetSortDirection();
            }

            elements.table.removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    const renderSingleChoiceDDL = (data, elementDdl, valueDDL) => {
        var html = [];
        for (var i = 0, max = data.length; i < max; i++) {
            html.push('<option value="');
            html.push(data[i].Id);
            html.push('">');
            html.push(data[i].Name);
            html.push("</option>");
        }
        if (html.length) {
            elementDdl.empty().append(html.join(""));
            elementDdl.val(data[0].Id);
            valueDDL.val(data[0].Name);
        }
    };

    const renderMultiChoiceDDL = () => {
        currencyDdl.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: currencyDdl.find("option").map(function () {
                return {
                    Id: this.text + "-" + this.value,
                    Value: this.value,
                    Text: this.text,
                    Checked: true,
                    Enable: true
                };
            }),
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });
        currencyDdl = $("#ddl-currency");

        siteDdl.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: siteDdl.find("option").map(function () {
                return {
                    Id: this.text + "-" + this.value,
                    Value: this.value,
                    Text: this.text,
                    Checked: true,
                    Enable: true
                };
            }),
            showFooter: false,
            showTooltip: false,
            shadow: true,
        });
        siteDdl = $("#ddl-site");
    }

    $(function () {

        $("#formChatbotMapping").submit(function (event) {
            event.preventDefault();
            resetFilterParams();
            getChatBot();
        });

        renderSingleChoiceDDL(optionsLinkingStatusDDL, elements.linkStatusDropdown, $("#ddl-linkingStatus.ui-combobox input"));
        renderSingleChoiceDDL(optionsLevelDDL, elements.levelDropdown, $("#ddl-level.ui-combobox input"));
        renderSingleChoiceDDL(optionsAccStatusDDL, elements.statusDropdown, $("#ddl-status.ui-combobox input"));

        renderMultiChoiceDDL();

        var configChatBotModule = new ConfigChatBotModule(
            viewResources,
            elements.table,
            elements.formChatBotConfig.clone(),
            elements.resultMessageDiv);
        configChatBotModule.initialize();

    });
})(jQuery);