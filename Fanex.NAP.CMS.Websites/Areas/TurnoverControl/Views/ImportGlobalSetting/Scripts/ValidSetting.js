function ValidSetting() {
    "use strict";
    var self = this;
    var domElements = {
        table: $("#validsetting-list"),
        submitButton: $("#btn-submit"),
        validSettingMessageSpan: $("#validsetting-result-message")
    };
    var importUrl = rootUrl + "TurnoverControl/ImportGlobalSetting/Import";
    var gridOptions = null;
    var batchSize = 150;

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
                Name: "UserName",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: viewResources.Level,
                Width: 120,
                Name: "Level",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.Currency,
                Width: 160,
                Name: "Currency",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.Max_Turnover,
                Width: 250,
                Name: "MaxTurnover",
                HeaderAlign: "center",
                CellAlign: "right"
            }];

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
        var customer;

        for (var i = 0, max = data.length; i < max; i++) {
            customer = data[i];

            bodyData.push({
                Columns: [
                    { Name: "ColNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "UserName", Value: customer.UserName },
                    { Name: "Level", Value: customer.Level },
                    { Name: "Currency", Value: customer.Currency },
                    { Name: "MaxTurnover", Value: numeral(customer.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) }]
            });
        }

        return bodyData;
    }

    function importGlobalSettings(data) {
        CMS.disableInput();

        var totalItems = data.length;
        var totalBatch = Math.ceil(totalItems / batchSize);

        for (var i = 0; i < totalBatch; i++) {
            var globalSettings = paginate(data, batchSize, i);

            var jqxhr = $.ajax({
                type: "POST",
                url: importUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ SportType: sportType, GlobalSettingItems: globalSettings })
            }).fail(CMS.handleAjaxError);

            if (i === totalBatch - 1) {
                jqxhr.done(function (result) {
                    var resultJSON = $.parseJSON(result);

                    domElements.validSettingMessageSpan
                        .text(resultJSON.Message)
                        .removeClass();

                    if (resultJSON.Code !== 0) {
                        domElements.validSettingMessageSpan.addClass("span-error");
                    }
                    else {
                        domElements.submitButton.addClass("display-none");
                        domElements.validSettingMessageSpan.addClass("span-success");
                    }
                });

                jqxhr.always(CMS.enableInput);
            }
        }
    }

    this.initialize = function (data) {
        gridOptions = gridOptions || getGridOptions();
        gridOptions.bodyRows = renderSettings(data);

        domElements.table.removeClass("display-none").fanexGrid(gridOptions);
        domElements.validSettingMessageSpan.text("");

        if (data && data.length > 0) {
            domElements.submitButton
                .removeClass("display-none")
                .off("click")
                .on("click", function () {
                    importGlobalSettings(data);
                });
        }
        else {
            domElements.submitButton.addClass("display-none").off("click");
            domElements.validSettingMessageSpan.addClass("display-none");
        }
    };
}