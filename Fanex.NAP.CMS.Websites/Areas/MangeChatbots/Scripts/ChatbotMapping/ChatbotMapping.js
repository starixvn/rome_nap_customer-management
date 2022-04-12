"use strict";
(function ($) {
    var elements = {
        resultMessageDiv: $("#div-result-message"),
        table: $("#chatBots-list"),
        addGlobalSettingForm: $("#formAddGlobalSetting"),
        messengerUserNameTextbox: $("#txt-messengerUserName"),
        userNameSubAccountTextbox: $("#txt-userNameSubAccount"),
        statusDropdown: $("#ddl-status"),
        levelDropdown: $("#ddl-level"),
        linkStatusDropdown: $("#ddl-linkingStatus"),
        subAccountOnly: $("#chk-subAccountsOnly")
    };
    var currencyDdl = $("#ddl-currency"),
        siteDdl = $("#ddl-site");

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

    const optionsStatusDDL = [
        {Id: 0, Name: "All"},
        {Id: 1, Name: "Active"},
        {Id: 2, Name: "Closed"},
        {Id: 3, Name: "Suspended"}];

    const getFilterParams = () => {
        var siteNames = "0";
        var currencyNames = "0";
        siteNames = siteDdl.multiSelect("getCheckedItems").join(",");
        currencyNames = currencyDdl.multiSelect("getCheckedItems").join(",");

        const filterParams = {};
        filterParams.Username = $.trim(elements.userNameSubAccountTextbox.val());
        filterParams.MessengerName = $.trim(elements.messengerUserNameTextbox.val());
        filterParams.IsSubAccOnly = elements.subAccountOnly.val();
        filterParams.Level = elements.levelDropdown.val();
        filterParams.Status = elements.statusDropdown.val();
        filterParams.LinkStatus = elements.linkStatusDropdown.val();
        filterParams.Currency = currencyNames;
        filterParams.Site = siteNames;

        filterParams.SortField = "Username";
        filterParams.SortDirection = "asc";
        filterParams.PageIndex = 0;
        filterParams.PageSize = 100;
        filterParams.Cache = false;
        debugger
        return filterParams;
    };

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: viewResources.Symbol_No,
                Width: 50,
                Name: "ColNo",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            }, {
                HeaderText: viewResources.ChatBotName,
                Width: 150,
                Name: "ChatBotName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true
            },
            {
                HeaderText: viewResources.MessengerUserName,
                Width: 160,
                Name: "MessengerUserName",
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
                HeaderText: viewResources.Status,
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
           /* sortOption: {
                /!* serverSortFunction: function (sortColumnName, sortDirection) {
                     filterParams.SortField = sortColumnName;
                     filterParams.SortDirection = sortDirection;
                     filterParams.Page = 0;
                     filterParams.Size = 100;
                     filterParams.Cache = true;
                     getGlobalSettings();
                 },*!/
                customGridSort: function (rows, sortColumnName, sortDirection) {
                    console.log("rows", rows)
                    console.log("sortColumnName", sortColumnName)
                    console.log("sortDirection", sortDirection)
                    rows.sort((a, b) => {
                        const valueA = a.Columns.find(e=>e.Name==sortColumnName).Value;
                        const valueB = b.Columns.find(e=>e.Name==sortColumnName).Value;
                        if (valueA < valueB) {
                            return -1;
                        }
                        if (valueA > valueB) {
                            return 1;
                        }
                        return 0;
                    })
                    return rows;
                },
            },*/
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                totalItem: 0,
                pagerSelectCallBack: function (index, pageSize) {
                    filterParams.Page = index;
                    filterParams.Size = pageSize;
                    filterParams.Cache = true;
                    getGlobalSettings();
                }
            },
            gridExpandHeight: 180
        };
    }

    function renderGridChatBots(data) {
        var bodyData = [];
        var chatbot, columns;
        /* var startIndex = filterParams.Page * filterParams.Size + 1;*/
        var startIndex = 0;
        //ui-icon-gear
        for (var i = 0, max = data.length; i < max; i++) {
            chatbot = data[i];
            var linkStatusCssColor = chatbot.LinkStatus == "Verifying" ? "yellow" : "green";
            columns = [
                {Name: "ColNo", Value: numeral(i + startIndex).format(CMSConstants.NumberFormat.IntNumber)},
                {Name: "ChatBotName", Value: chatbot.ChatBotName},
                {Name: "MessengerUserName", Value: chatbot.MessengerUserName},
                {Name: "UserName", Value: chatbot.UserName},
                {Name: "SubAccount", Value: chatbot.SubAccount},
                {
                    Name: "LinkingStatus",
                    Value: `<span class="high-light-link-status-item ${linkStatusCssColor}" >${chatbot.LinkStatus}</span>`
                },
                {Name: "Language", Value: chatbot.Language},
                {Name: "Level", Value: chatbot.Level},
                {Name: "Currency", Value: chatbot.Currency},
                {Name: "Status", Value: chatbot.Status},
                {
                    Name: "Config",
                    Value: '<span class="ui-icon ui-icon-gear ico-config" id="btn-config-chatBot" data-cust-id="' + i + '"></span>'
                }
            ];
            var css = (i & 1) == 1 ? 'freegrid-even' : '';
            bodyData.push({
                Columns: columns,
                CssClass: css
            });
        }

        return bodyData;
    }

    function getChatBot() {
        //   CMS.hideInlineMsg(elements.resultMessageDiv);
        const filterParams = getFilterParams();

        CMS.getJson(CMS.resolveRootUrl("ChatbotMapping/SearchChatBots?" + $.param(filterParams)), function (data) {
            var jsonData = $.parseJSON(data);
            console.log(jsonData)
            gridOptions = gridOptions || getGridOptions();
            gridOptions.bodyRows = renderGridChatBots(jsonData.ChatBots);

            gridOptions.pagerOption.currentPage = filterParams.Page || 1;
            gridOptions.pagerOption.itemsPerPage = filterParams.Size || 99;
            gridOptions.pagerOption.totalItem = jsonData.TotalItems;

            if (!filterParams.Cache) {
                $.each(gridOptions.columns, function (index, value) {
                    value.SortDirection = value.Name !== filterParams.SortField
                        ? ""
                        : filterParams.SortDirection;
                });
            }

            elements.table.removeClass("display-none").fanexGrid(gridOptions);
        });
    }

    const renderDropdown = (data, elementDdl, valueDDL) => {
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

    function renderMultiSelect() {
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

        var formChatbotMapping = $("#formChatbotMapping");

        formChatbotMapping
            .submit(function (event) {
                event.preventDefault();
                debugger
                getChatBot();
            });

        renderDropdown(optionsLinkingStatusDDL, elements.linkStatusDropdown, $("#ddl-linkingStatus.ui-combobox input"));
        renderDropdown(optionsLevelDDL, elements.levelDropdown, $("#ddl-level.ui-combobox input"));
        renderDropdown(optionsStatusDDL, elements.statusDropdown, $("#ddl-status.ui-combobox input"));

        renderMultiSelect();


        var configChatBotModule = new ConfigChatBotModule(
            viewResources,
            elements.table,
            elements.addGlobalSettingForm.clone(),
            elements.resultMessageDiv);
        configChatBotModule.initialize();

    });
})(jQuery);