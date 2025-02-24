(function ($) {
    let cachedFilter = null,
        cachedGridOptions = null,
        cachedData = null,
        hasHostPermission = false,
        freeGrid = $("#freegrid");

    function init() {
        const txtFilterUsername = $("#txtFilterUsername");
        const searchForm = $("#formSearch").submit(function (e) {
            e.preventDefault();

            const site = $("#ddl-site").val();
            const currency = $("#ddl-currency").val();
            const usernameFilter = txtFilterUsername.val();

            cachedFilter = {
                SiteId: site,
                Username: usernameFilter,
                Currency: currency
            };
            search();
        });

        const btnAddNew = $("#btnAddNew");
        hasHostPermission = btnAddNew.length > 0;

        if (hasHostPermission) {
            betLimitIncreaseAddOrEdit.init(search);

            btnAddNew.click(function () {
                betLimitIncreaseAddOrEdit.openModal();
            });
        }
    }

    function search() {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`BetLimitIncrease/Search`),
            cachedFilter,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message) {
                    CMS.showErrorMessage(result.Message);
                    assignCachedGridOptions([]);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                } else {
                    assignCachedGridOptions(result);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                    bindEventsAfterRenderGridSuccessfully();
                }
            },
            function (error) {
                assignCachedGridOptions([]);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            },
            function () {
                CMS.enableInput();
            });
    }

    function assignCachedGridOptions(data) {
        cachedData = data;
        cachedGridOptions = cachedGridOptions || defaultGridOptions();
        cachedGridOptions.bodyRows = buildBodyRows(data);
    }

    function defaultGridOptions() {
        var columns = [
            { HeaderText: resources.username, Width: 200, Name: "colUsername", HeaderAlign: "center", CellAlign: "left"},
            { HeaderText: resources.site, Width: 80, Name: "colSite", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.currency, Width: 80, Name: "colCurrency", HeaderAlign: "center", CellAlign: "left"},
            { HeaderText: resources.multiplier, Width: 60, Name: "colMultiplier", HeaderAlign: "center", CellAlign: "right"},
            { HeaderText: resources.addBy, Width: 120, Name: "colAddBy", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.addDate, Width: 120, Name: "colAddDate", HeaderAlign: "center", CellAlign: "right" }
        ];

        if (hasHostPermission) {
            columns.push({ HeaderText: 'Action', Width: 80, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' });
        }

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 180,
            columns: columns
        };
    }

    function buildBodyRows(data) {
        return data.map(function (betLimit, index) {
            var columns = [
                { Name: "colUsername", Value: betLimit.Username },
                { Name: "colSite", Value: betLimit.Site },
                { Name: "colCurrency", Value: betLimit.Currency },
                { Name: "colMultiplier", Value: betLimit.Multiplier },
                { Name: "colAddBy", Value: betLimit.LastModifiedUser },
                { Name: "colAddDate", Value: betLimit.LastModifiedTime }
            ];

            if (hasHostPermission) {
                columns.push(
                    {
                        Name: 'Action',
                        Value: "<span title='Update' class='ico-edit' data-username='"
                            + betLimit.Username
                            + "' data-index='"
                            + index
                            + "'  ></span><span title='Delete' class='ico-delete mid-margin-left' data-username='"
                            + betLimit.Username
                            + "' data-index='"
                            + index
                            + "'></span>"
                    });
            }
            return {
                Columns: columns
            };
        });
    }

    function bindEventsAfterRenderGridSuccessfully() {
        $(".ico-edit")
            .off("click")
            .click(function (e) {
                e.stopPropagation();
                var pos = $(this).data("index");
                if (pos !== undefined && cachedData) {
                    betLimitIncreaseAddOrEdit.openModal(cachedData[pos]);
                }
            });

        $(".ico-delete")
            .off("click")
            .click(function (e) {
                e.stopPropagation();
                var pos = $(this).data("index");
                var username = $(this).data("username");
                if (pos !== undefined && cachedData) {
                    showConfirmDeleteBetLimit(pos, username);
                }
            });
    }

    function showConfirmDeleteBetLimit(index, username){
        CMS.showConfirmMessage(`Do you want to remove the selected Bet Limit Increase for ${username}?`, function () {
            CMS.disableInput();
            CMS.callAjax(
                this,
                CMS.resolveRootUrl("BetLimitIncrease/Delete"),
                cachedData[index],
                function (result) {
                    const resultJson = JSON.parse(result);
                    if (resultJson.IsSuccess) {
                        CMS.showSuccessMessage("Delete Bet Limit Increase Successfully!");
                        search();
                    } else {
                        CMS.showErrorMessage(resultJson.Message);
                    }
                });
        });
    }

    $(function () {
        init();
        search();
    });
})(jQuery);