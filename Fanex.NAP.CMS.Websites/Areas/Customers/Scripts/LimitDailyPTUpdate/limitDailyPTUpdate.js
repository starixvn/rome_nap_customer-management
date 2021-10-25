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
        ddlSite.fanexMultiSelect({
            minWidth: 180,
            checkAllText: "All",
            listData: sites.map(function (item) {
                return { id: item, value: item, text: item, checked: true, enable: true };
            })
        });

        ddlCurrency.fanexMultiSelect({
            minWidth: 180,
            checkAllText: "All",
            listData: currencies.map(function (item) {
                return { id: item, value: item, text: item, checked: true, enable: true };
            })
        });

        ddlProduct.fanexMultiSelect({
            minWidth: 180,
            checkAllText: "All",
            listData: products.map(function (item) {
                return { id: item, value: item, text: item, checked: true, enable: true };
            })
        });
    }

    function bindEvents() {
        $("#note-info").click(function () {
            CMS.popup("Information", noteInfoContentHtml, { resizedable: false, width: "600px" });
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
        var url = formSearch.attr("action")
            + "?username=" + txtUsername.val()
            + "&site=" + ddlSite.fanexMultiSelect("getItems", true).join(",")
            + "&currency=" + ddlCurrency.fanexMultiSelect("getItems", true).join(",")
            + "&product=" + ddlProduct.fanexMultiSelect("getItems", true).join(",")
            + "&type=" + ddlType.val()
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
                itemsPerPageArray: [50, 100, 150, 200],
                pagerSelectCallback: function (pageIndex, pageSize) {
                    cachedPageSize = pageSize;
                    getLimitDailyPTUpdateList(pageIndex, pageSize);
                }
            }
        }
    }

    function renderLimitDailyPTUpdateList(data, startIndex) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colNo", Value: startIndex + index + 1 },
                { Name: "colUsername", Value: value.Username },
                { Name: "colSite", Value: value.Site },
                { Name: "colCurrency", Value: value.Currency },
                { Name: "colProduct", Value: value.Product },
                { Name: "colType", Value: value.Type },
            ];
            hasHostPermission && columns.push({ Name: "colDel", Value: renderDeleteCell(value.Username) });

            bodyRows.push({ Columns: columns });
        });

        return bodyRows;
    }

    function renderDeleteCell(username) {
        return [
            '<span class="ico-delete delete-limit" title="Delete"' + 'data-user-name="' + username + '"></span>'
        ].join("");
    }

    function bindEventsAfterRenderLimitDailyPTUpdateList() {
        $(".delete-limit").off().click(function () {
            var username = $(this).attr("data-user-name");
            var manageLimit = new ManageLimit(
                null,
                CMS.resolveRootUrl("LimitDailyPTUpdate/Remove"),
                function () { formSearch.submit(); });
            manageLimit.RemoveLimit(username);
        });
    }

    $(function () {
        bindEvents();

        renderMultiSelect();

        $("#divPopupContent").remove();
        txtUsername.focus();
    });
})(jQuery, resources);