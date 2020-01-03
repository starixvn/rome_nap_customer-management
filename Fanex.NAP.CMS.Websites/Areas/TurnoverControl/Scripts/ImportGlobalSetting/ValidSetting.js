"use strict";
function ValidSetting(profiles, sportType) {
    var elements = {
        table: $("#validsetting-list"),
        submitButton: $("#btn-submit"),
        validSettingMessageSpan: $("#validsetting-result-message")
    };
    var gridOptions = null;
    var batchSize = 150;
    var profileLength = profiles.length;

    // pageNumber start with 0.
    function paginate(array, pageSize, pageNumber) {
        return array.slice(pageNumber * pageSize, (pageNumber + 1) * pageSize);
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
                CellAlign: "left"
            },
            {
                HeaderText: viewResources.Level,
                Width: 80,
                Name: "Level",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.Currency,
                Width: 80,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center"
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

        return {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                text: viewResources.ValidSetting
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
            customPager: false,
            pagerOption: {
                itemsPerPage: 100,
                itemsPerPageArray: CMS.pagerRanges,
                currentPage: 0
            },
            gridExpandHeight: 580
        };
    }

    function renderSettings(data) {
        var bodyData = [];
        var customer, columns, mto;

        for (var i = 0, max = data.length; i < max; i++) {
            customer = data[i];
            columns = [
                { Name: "ColNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                { Name: "Username", Value: customer.Username },
                { Name: "Level", Value: customer.Level },
                { Name: "Currency", Value: customer.Currency }
            ];

            for (var j = 0; j < profileLength; j++) {
                mto = customer.MaxTurnovers[j];
                columns.push({
                    Name: "Profile" + mto.ProfileId,
                    Value: numeral(mto.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber),
                    CssClass: "edit-maxturnover-container"
                });
            }

            bodyData.push({ Columns: columns, CssClass: "customer-row" });
        }

        return bodyData;
    }

    function importGlobalSettings(data) {
        CMS.disableInput();

        var totalItems = data.length;
        var totalBatch = Math.ceil(totalItems / batchSize);

        for (var i = 0; i < totalBatch; i++) {
            var globalSettings = paginate(data, batchSize, i);
            var formData = globalSettings.map(function (item) {
                return {
                    Username: item.Username,
                    IsGroupHeadline: item.Level && item.Level.toLowerCase() === "headline",
                    SportType: sportType,
                    MaxTurnovers: item.MaxTurnovers
                };
            });

            var jqxhr = $.ajax({
                type: "POST",
                url: CMS.resolveRootUrl("TurnoverControl/ImportGlobalSetting/Import"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(formData)
            }).fail(CMS.handleAjaxError);

            if (i === totalBatch - 1) {
                jqxhr.done(function (result) {
                    var resultJSON = $.parseJSON(result);

                    elements.validSettingMessageSpan
                        .text(resultJSON.Message)
                        .removeClass();

                    if (resultJSON.Code !== 0) {
                        elements.validSettingMessageSpan.addClass("span-error");
                    }
                    else {
                        elements.submitButton.addClass("display-none");
                        elements.validSettingMessageSpan.addClass("span-success");
                    }
                });

                jqxhr.always(CMS.enableInput);
            }
        }
    }

    this.initialize = function (data) {
        gridOptions = gridOptions || getGridOptions();
        gridOptions.bodyRows = renderSettings(data);

        elements.table.removeClass("display-none").fanexGrid(gridOptions);
        elements.validSettingMessageSpan.text("");

        if (data && data.length > 0) {
            elements.submitButton
                .removeClass("display-none")
                .off("click")
                .on("click", function () {
                    importGlobalSettings(data);
                });
        }
        else {
            elements.submitButton.addClass("display-none").off("click");
            elements.validSettingMessageSpan.addClass("display-none");
        }
    };
}