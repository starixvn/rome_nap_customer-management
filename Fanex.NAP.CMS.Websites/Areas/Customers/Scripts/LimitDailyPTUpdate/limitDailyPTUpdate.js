"use strict";
(function ($, resources) {
    var noteInfoContentHtml = $("#note-info-content").html(),
        txtUsername = $("#txt-username"),
        ddlSite = $("#ddl-site"),
        ddlCurrency = $("#ddl-currency"),
        ddlProduct = $("#ddl-product"),
        ddlType = $("#ddl-type"),
        formSearch = $("#formSearch"),
        freegrid = $("#freegrid"),
        divAddNew = $("#addNew"),
        cachedPageSize = 50,
        gridOptions = null,
        hasHostPermission = divAddNew.length > 0;


    function renderMultiSelect() {
        ddlSite.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: ddlSite.find("option").map(function () {
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

        ddlCurrency.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: ddlCurrency.find("option").map(function () {
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

        ddlProduct.multiSelect({
            minWidth: 180,
            checkAllText: "All",
            isCheckAll: true,
            listData: ddlProduct.find("option").map(function () {
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

        ddlSite = $("#ddl-site");
        ddlCurrency = $("#ddl-currency");
        ddlProduct = $("#ddl-product");
    }

    function bindEvents() {
        $("#note-info").click(function () {
            CMS.popup("Information", noteInfoContentHtml, { resizedable: false });
        });

        formSearch.submit(function (e) {
            e.preventDefault();
            getLimitDailyPTUpdateList(0, cachedPageSize);
        });

        if (hasHostPermission) {
            $("#btn-addNew").off().click(function () {
                var manageLimit = new ManageLimit(
                    divAddNew.clone(),
                    CMS.resolveRootUrl("LimitDailyPTUpdate/AddNew"),
                    function () { formSearch.submit(); });
                manageLimit.ShowAddLimitPopup("New Daily PT Update Limitation");
            });
        }
    }

    function getLimitDailyPTUpdateList(pageIndex, pageSize) {
        var siteNames = "0";
        var currencyIds = "0";
        var productIds = "0";

        if (!ddlSite.multiSelect("isCheckedAllItems"))
        {
            siteNames = ddlSite.multiSelect("getCheckedItems").join(",");
        }

        if (!ddlCurrency.multiSelect("isCheckedAllItems")) 
        {
            currencyIds = ddlCurrency.multiSelect("getCheckedItems").join(",");
        }

        if (!ddlProduct.multiSelect("isCheckedAllItems"))
        {
            productIds = ddlProduct.multiSelect("getCheckedItems").join(",");
        }

        var url = formSearch.attr("action")
            + "?username=" + txtUsername.val().trim().replace(/[\n\t]+/g, '')
            + "&siteNames=" + siteNames
            + "&currencyIds=" + currencyIds
            + "&productId=" + productIds
            + "&typeId=" + ddlType.val()
            + "&pageIndex=" + (pageIndex + 1)
            + "&pageSize=" + pageSize;

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            gridOptions = gridOptions || initGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalItems;
            gridOptions.bodyRows = renderLimitDailyPTUpdateList(jsonData.LimitDailyPTUpdateList, pageIndex * pageSize);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);

            if (hasHostPermission && jsonData.TotalItems > 0) {
                bindEventsAfterRenderLimitDailyPTUpdateList();
            }
        })
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: "#", Width: 80, Name: "colNo", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: resources.username, Width: 220, Name: "colUsername", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
            { HeaderText: resources.site, Width: 150, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.currency, Width: 150, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.product, Width: 150, Name: "colProduct", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.type, Width: 150, Name: "colType", HeaderAlign: "center", CellAlign: "center" },
        ];
        hasHostPermission && columns.push(
            { HeaderText: resources.delete, Width: 80, Name: "colDel", HeaderAlign: "center", CellAlign: "center" });

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 200,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: [50, 100, 200, 300, 500],
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    cachedPageSize = pageSize;
                    getLimitDailyPTUpdateList(pageIndex, pageSize);
                }
            }
        };
    }

    function renderLimitDailyPTUpdateList(data, startIndex) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colNo", Value: startIndex + index + 1 },
                { Name: "colUsername", Value: value.Username },
                { Name: "colSite", Value: value.Site },
                { Name: "colCurrency", Value: value.CurrencyName },
                { Name: "colProduct", Value: value.ProductName },
                { Name: "colType", Value: value.TypeName },
            ];
            hasHostPermission && columns.push({ Name: "colDel", Value: renderDeleteCell(value) });

            bodyRows.push({ Columns: columns });
        });

        return bodyRows;
    }

    function renderDeleteCell(data) {
        return [
            '<span class="ico-delete delete-limit" title="Delete"'
            + 'data-cust-id=' + data.CustID + ' '
            + 'data-product-id=' + data.ProductID + ' '
            + 'data-site-id=' + data.SiteID + ' '
            + 'data-currency=' + data.Currency + ' '
            + 'data-type=' + data.Type + ' '
            + '"></span>'
        ].join("");
    }

    function bindEventsAfterRenderLimitDailyPTUpdateList() {
        $(".delete-limit").off().click(function () {
            var custID = $(this).attr("data-cust-id");
            var productID = $(this).attr("data-product-id");
            var siteID = $(this).attr("data-site-id");
            var currency = $(this).attr("data-currency");
            var type = $(this).attr("data-type");

            var manageLimit = new ManageLimit(
                null,
                CMS.resolveRootUrl("LimitDailyPTUpdate/Remove"),
                function () { formSearch.submit(); });
            manageLimit.RemoveLimit({ custID, productID, siteID, type, currency });
        });
    }

    $(function () {
        bindEvents();

        renderMultiSelect();

        $("#divPopupContent").remove();
        txtUsername.focus();
    });
})(jQuery, resources);