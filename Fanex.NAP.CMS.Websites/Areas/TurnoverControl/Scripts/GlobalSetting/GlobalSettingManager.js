"use strict";
(function ($) {
    var elements = {
        resultMessageDiv: $("#div-result-message"),
        usernameTextbox: $("#txt-username"),
        currencyDropdown: $("#ddl-currency"),
        table: $("#globalsetting-list"),
        addGlobalSettingForm: $("#formAddGlobalSetting")
    };

    var gridOptions = null;
    var filterParams = {};
    var profileLength = profiles.length;

    function resetFilterParams() {
        filterParams.Username = $.trim(elements.usernameTextbox.val());
        filterParams.ExchangeId = elements.currencyDropdown.val();
        filterParams.SortField = "Username";
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
                Name: "Username",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                SortDirection: "asc"
            },
            {
                HeaderText: viewResources.Level,
                Width: 80,
                Name: "Level",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            },
            {
                HeaderText: viewResources.Currency,
                Width: 80,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            }
        ];

        for (var i = 0; i < profileLength; i++) {
            var profile = profiles[i];

            headerColumns.push({
                HeaderText: profile.Name,
                Width: 120,
                Name: "Profile" + profile.Id,
                HeaderAlign: "center",
                CellAlign: "right",
                Sort: true
            });
        }

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
                    CMS.downloadFile(CMS.resolveRootUrl("TurnoverControl/GlobalSetting/ExportList"), filterParams);
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

    function getGlobalSettings() {
        CMS.hideInlineMsg(elements.resultMessageDiv);

        CMS.getJson(
            CMS.resolveRootUrl("TurnoverControl/GlobalSetting/GetList?" + $.param(filterParams)),
            function (data) {
                var jsonData = $.parseJSON(data);

                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = renderGlobalSettings(jsonData.GlobalSettings);

                gridOptions.pagerOption.currentPage = filterParams.Page;
                gridOptions.pagerOption.itemsPerPage = filterParams.Size;
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

    function renderGlobalSettings(data) {
        var bodyData = [];
        var customer, columns;
        var startIndex = filterParams.Page * filterParams.Size + 1;

        for (var i = 0, max = data.length; i < max; i++) {
            customer = data[i];
            columns = [
                { Name: "ColNo", Value: numeral(i + startIndex).format(CMSConstants.NumberFormat.IntNumber) },
                { Name: "Username", Value: customer.Username },
                { Name: "Level", Value: customer.Level },
                { Name: "Currency", Value: customer.Currency }];

            bodyData.push({
                Columns: renderMaxTurnovers(hasHostPermission, columns, customer),
                CssClass: "customer-row"
            });
        }

        return bodyData;
    }

    function renderMaxTurnovers(hasHostPermission, columns, customer) {
        var custId = customer.CustId;
        var username = customer.Username;
        var level = customer.Level;
        var isGroupHeadline = customer.IsGroupHeadline;
        var maxTurnovers = customer.MaxTurnovers;
        var i = 0, item;

        if (hasHostPermission) {
            for (; i < profileLength; i++) {
                item = maxTurnovers[i];
                columns.push({
                    Name: "Profile" + item.ProfileId,
                    Value: renderMaxTurnoverCell(custId, username, level, item.ProfileId, item.MaxTurnover, isGroupHeadline),
                    CssClass: "edit-maxturnover-container"
                });
            }
            columns.push({
                Name: "ColDelete",
                Value: renderDelete(custId, username, level, isGroupHeadline)
            });
        }
        else {
            for (; i < profileLength; i++) {
                item = maxTurnovers[i];
                columns.push({
                    Name: "Profile" + item.ProfileId,
                    Value: numeral(item.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber)
                });
            }
        }

        return columns;
    }

    function renderMaxTurnoverCell(custId, username, level, profileId, maxTurnover, isGroupHeadline) {
        return [
            '<a class="c hand edit-maxturnover" href="javascript:;" data-custid="',
            custId,
            '" data-username="',
            username,
            '" data-isgroupheadline="',
            isGroupHeadline,
            '" data-profileid="',
            profileId,
            '">',
            numeral(maxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber),
            "</a>"
        ].join("");
    }

    function renderDelete(custId, username, level, isGroupHeadline) {
        return [
            '<a class="c hand ico-delete" href="javascript:;" data-custid="',
            custId,
            '" data-username="',
            username,
            '" data-isgroupheadline="',
            isGroupHeadline,
            '"></a>'
        ].join("");
    }

    function renderCurrencyDropdown() {
        var html = [], currency;

        for (var i = 0, max = currencies.length; i < max; i++) {
            currency = currencies[i];
            html.push('<option value="');
            html.push(currency.Id);
            html.push('">');
            html.push(currency.Name);
            html.push("</option>");
        }

        if (html.length) {
            currency = currencies[0];
            elements.currencyDropdown.empty().append(html.join("")).val(currency.Id);
            $("#ddl-currency.ui-combobox input").val(currency.Name);
        }
    }

    $(function () {
        var resetFilterParamsAndSearch = function () {
            resetFilterParams();
            getGlobalSettings();
        };

        renderCurrencyDropdown();

        var globalSettingForm = $("#formGlobalSetting");
        globalSettingForm
            .submit(function (event) {
                event.preventDefault();
                resetFilterParamsAndSearch();
            });

        if (hasHostPermission) {
            $("#btn-add").click(function () {
                var addHandler = new AddGlobalSetting(
                    viewResources,
                    elements.addGlobalSettingForm.clone(),
                    resetFilterParamsAndSearch,
                    sportType);
                addHandler.initialize();
            });

            var deleteHandler = new DeleteGlobalSetting(
                viewResources,
                elements.table,
                elements.resultMessageDiv,
                resetFilterParamsAndSearch,
                sportType);
            deleteHandler.initialize();

            var editHandler = new EditGlobalSetting(
                viewResources,
                elements.table,
                elements.resultMessageDiv,
                resetFilterParamsAndSearch,
                sportType);
            editHandler.initialize();

            // Remove the #divPopupContent element since it was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").empty();
            elements.usernameTextbox.focus();

            resetFilterParamsAndSearch();
        }
    });
})(jQuery);
