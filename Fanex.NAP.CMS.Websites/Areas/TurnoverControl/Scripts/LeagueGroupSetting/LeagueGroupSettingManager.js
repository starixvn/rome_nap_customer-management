"use strict";
(function ($) {
    var elements = {
        table: $("#leaguegroupsetting-list"),
        addGlobalSettingForm: $("#formAddLeagueGroupSetting")
    };

    var gridOptions = null;
    var filterParams = {};
    var profileLength = profiles.length;

    function resetFilterParams() {
        filterParams.SortField = "LeagueGroup";
        filterParams.SortDirection = "asc";
        filterParams.SportType = sportType;
    }

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: viewResources.Symbol_No,
                Width: 50,
                Name: "ColNo",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.LeagueGroup,
                Width: 160,
                Name: "LeagueGroup",
                HeaderAlign: "center",
                CellAlign: "left",
                Sort: true,
                SortDirection: "asc"
            },
            {
                HeaderText: viewResources.Market,
                Width: 60,
                Name: "Market",
                HeaderAlign: "center",
                CellAlign: "center",
                Sort: true
            }
        ];

        for (var i = 0; i < profileLength; i++) {
            var profile = profiles[i];

            headerColumns.push({
                HeaderText: profile.Name,
                Width: profile.Name.indexOf("Non Live") === -1 ? 125 : 140,
                Name: "Profile" + profile.Id,
                HeaderAlign: "center",
                CellAlign: "right",
                Sort: true
            });
        }

        if (hasHostPermission) {
            headerColumns.push({
                HeaderText: viewResources.Action,
                Width: 60,
                Name: "ColAction",
                HeaderAlign: "center",
                CellAlign: "center"
            });
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
                    CMS.downloadFile(CMS.resolveRootUrl("TurnoverControl/TurnoverLeagueGroup/ExportList"), filterParams);
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
                    getLeagueGroupSettings();
                }
            },
            gridExpandHeight: 180
        };
    }

    function getLeagueGroupSettings() {
        CMS.hideInlineMsg(elements.resultMessageDiv);

        CMS.getJson(
            CMS.resolveRootUrl("TurnoverControl/GlobalSetting/GetList?" + $.param(filterParams)),
            function (data) {
                var jsonData = $.parseJSON(data);

                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = renderGlobalSettings(jsonData.GlobalSettings);

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
        var leagueSetting, columns;
        var startIndex = filterParams.Page * filterParams.Size + 1;

        for (var i = 0, max = data.length; i < max; i++) {
            leagueSetting = data[i];
            columns = [
                { Name: "ColNo", Value: numeral(i + startIndex).format(CMSConstants.NumberFormat.IntNumber) },
                { Name: "LeagueGroup", Value: leagueSetting.LeagueGroupName },
                { Name: "Market", Value: leagueSetting.MarketName }
            ];

            bodyData.push({
                Columns: renderLeagueGroupMaxTurnovers(hasHostPermission, columns, leagueSetting),
                CssClass: "customer-row"
            });
        }

        return bodyData;
    }

    function renderLockCell(hasHostPermission, leagueSetting) {
        var cell = ['<span class="', leagueSetting.IsLock ? "ico-lock" : "ico-unlock"];

        if (hasHostPermission) {
            cell.push(' lock-action" ');
            cell.push('data-custid="');
            cell.push(leagueSetting.CustId);
            cell.push('" data-isgroupheadline="');
            cell.push(leagueSetting.IsGroupHeadline);
        }

        cell.push('"></span>');
        return cell.join("");
    }

    function renderLeagueGroupMaxTurnovers(hasHostPermission, columns, leagueSetting) {
        var maxTurnovers = leagueSetting.MaxTurnovers;
        var i = 0, item;

        if (hasHostPermission) {
            for (; i < profileLength; i++) {
                item = maxTurnovers[i];
                columns.push({
                    Name: "Profile" + item.ProfileId,
                    Value: item.MaxTurnover,
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
                    Value: numeral(item.MaxTurnover).format(CMSConstants.NumberFormat.IntNumber)
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
            numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber),
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
            getLeagueGroupSettings();
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

            var toggleLockHandler = new ToggleLockGlobalSetting(elements.table, sportType);
            toggleLockHandler.initialize();

            // Remove the #divPopupContent element since it was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").empty();
            elements.usernameTextbox.focus();

            resetFilterParamsAndSearch();
        }
    });
})(jQuery);
