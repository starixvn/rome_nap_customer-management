(function ($) {
    "use strict";
    var domElements = {
        resultMessageDiv: $("#div-result-message"),
        userNameTextbox: $("#txt-username"),
        currencyDropdown: $("#ddl-currency"),
        table: $("#globalsetting-list"),
        addGlobalSettingForm: $("#formAddGlobalSetting")
    };

    var gridOptions = null;
    var filterParams = {};
    var urls = {
        getList: "",
        getCurrencies: rootUrl + "TurnoverControl/GlobalSetting/GetCurrencies",
        exportExcel: rootUrl + "TurnoverControl/GlobalSetting/ExportList"
    };

    function resetFilterParams() {
        filterParams.UserName = $.trim(domElements.userNameTextbox.val());
        filterParams.ExchangeId = domElements.currencyDropdown.val(),
        filterParams.SortField = "UserName";
        filterParams.SortDirection = "asc";
        filterParams.Page = 0;
        filterParams.Size = 100;
        filterParams.Cache = false;
        filterParams.SportType = sportType;
    }

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: viewResources.Symbol_No,
                Width: 60,
                Name: "ColNo",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.UserName,
                Width: 160,
                Name: "UserName",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                SortDirection: "asc"
            },
            {
                HeaderText: viewResources.Level,
                Width: 120,
                Name: "Level",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            },
            {
                HeaderText: viewResources.Currency,
                Width: 160,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            },
            {
                HeaderText: viewResources.Max_Turnover,
                Width: 160,
                Name: "MaxTurnover",
                HeaderAlign: "center",
                CellAlign: "right",
                Sort: true
            }];

        if (hasHostPermission) {
            headerColumns.push({
                HeaderText: viewResources.Delete,
                Width: 60,
                Name: "ColDelete",
                HeaderAlign: "center",
                CellAlign: "center"
            });
        }

        return {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(urls.exportExcel, filterParams);
                }
            },
            bodyRows: [],
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    filterParams.SortField = sortColumnName;
                    filterParams.SortDirection = sortDirection;
                    filterParams.Page = 0;
                    filterParams.Size = 100;
                    filterParams.Cache = true;
                    getGlobalSettings();
                }
            },
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

    function renderGlobalSettings(data) {
        var bodyData = [];
        var customer, columns;
        var startIndex = filterParams.Page * filterParams.Size + 1;

        for (var i = 0, max = data.length; i < max; i++) {
            customer = data[i];
            columns = [
                { Name: "ColNo", Value: numeral(i + startIndex).format(CMSConstants.NumberFormat.IntNumber) },
                { Name: "UserName", Value: customer.UserName },
                { Name: "Level", Value: customer.Level },
                { Name: "Currency", Value: customer.Currency }];

            if (hasHostPermission) {
                columns.push({
                    Name: "MaxTurnover",
                    Value: [
                        '<a class="c hand edit-maxturnover" href="javascript:;" data-custid="',
                        customer.CustId,
                        '" data-username="',
                        customer.UserName,
                        '" data-isgroupheadline="',
                        customer.Level.toLowerCase() === "headline",
                        '">',
                        numeral(customer.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber),
                        "</a>"
                    ].join(""),
                    CssClass: "edit-maxturnover-container"
                });
                columns.push({
                    Name: "ColDelete",
                    Value: [
                        '<a class="c hand ico-delete" href="javascript:;" data-custid="',
                        customer.CustId,
                        '" data-username="',
                        customer.UserName,
                        '" data-isgroupheadline="',
                        customer.Level.toLowerCase() === "headline",
                        '"></a>'
                    ].join("")
                });
            }
            else {
                columns.push({ Name: "MaxTurnover", Value: numeral(customer.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) });
            }

            bodyData.push({ Columns: columns, CssClass: "customer-row" });
        }

        return bodyData;
    }

    function getGlobalSettings() {
        CMS.hideInlineMsg(domElements.resultMessageDiv);

        CMS.getJson(
            urls.getList + $.param(filterParams),
            function (data) {
                var result = $.parseJSON(data);

                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = renderGlobalSettings(result.GlobalSettings);

                gridOptions.pagerOption.currentPage = filterParams.Page;
                gridOptions.pagerOption.itemsPerPage = filterParams.Size;
                gridOptions.pagerOption.totalItem = result.TotalItems;

                if (!filterParams.Cache) {
                    $.each(gridOptions.columns, function (index, value) {
                        value.SortDirection = value.Name !== filterParams.SortField ?
                            "" :
                            filterParams.SortDirection;
                    });
                }

                domElements.table.removeClass("display-none").fanexGrid(gridOptions);
            });
    }

    function getCurrencies(resetFilterParamsAndSearch) {
        CMS.getJson(
            urls.getCurrencies,
            function (data) {
                var currency;
                var html = [];
                var result = $.parseJSON(data);

                for (var i = 0, max = result.length; i < max; i++) {
                    currency = result[i];

                    html.push('<option value="');
                    html.push(currency.Id);
                    html.push('">');
                    html.push(currency.Name);
                    html.push("</option>");
                }

                domElements.currencyDropdown.empty().append(html.join("")).val(result[0].Id);
                $("#ddl-currency.ui-combobox input").val(result[0].Name);

                resetFilterParamsAndSearch();
            });
    }

    function initialize() {
        var resetFilterParamsAndSearch = function () {
            resetFilterParams();
            getGlobalSettings();
        };

        getCurrencies(resetFilterParamsAndSearch);

        var globalSettingForm = $("#formGlobalSetting");
        urls.getList = globalSettingForm.attr("action") + "?";
        globalSettingForm
            .submit(function (e) {
                e.preventDefault();

                resetFilterParamsAndSearch();
            });

        if (hasHostPermission) {
            $("#btn-add").click(function () {
                CMS.modal(viewResources.GlobalSetting_AddNewSMA_Title, domElements.addGlobalSettingForm.clone(), { resizable: false });

                var addHandler = new AddGlobalSetting(resetFilterParamsAndSearch, sportType);
                addHandler.initialize();
            });

            var deleteHandler = new DeleteGlobalSetting(
                domElements.table,
                domElements.resultMessageDiv,
                resetFilterParamsAndSearch,
                sportType);
            deleteHandler.initialize();

            var editHandler = new EditGlobalSetting(
                domElements.table,
                domElements.resultMessageDiv,
                resetFilterParamsAndSearch,
                sportType);
            editHandler.initialize();

            // Remove the #divPopupContent element since it was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").empty();
            domElements.userNameTextbox.focus();
        }
    }

    initialize();
})(jQuery);
