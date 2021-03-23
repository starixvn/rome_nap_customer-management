"use strict";
(function ($, resources) {
    var noteInfoContentHtml = $("#note-info-content").html(),
        txtUsername = $("#txt-username"),
        ddlLevel = $("#ddl-level"),
        ddlSite = $("#ddl-site"),
        formSearch = $("#formSearch"),
        freegrid = $("#freegrid"),
        cachedPageSize = 50,
        gridOptions = null,
        divAddNewOrRemoveLimit = $("#addNewOrRemoveLimit"),
        hasHostPermission = divAddNewOrRemoveLimit.length > 0;

    function bindEvents() {
        $("#note-info").click(function () {
            CMS.popup("Limit Rules", noteInfoContentHtml, { resizable: false, width: "600px" });
        });

        formSearch.submit(function (e) {
            e.preventDefault();
            getLimitDownlineList(0, cachedPageSize);
        });

        if (hasHostPermission) {
            $("#btn-addNew").click(function () {
                var addNew = new AddNewOrRemoveLimit(
                    divAddNewOrRemoveLimit.clone(),
                    CMS.resolveRootUrl("LimitNewDownline/AddNew"),
                    function () { formSearch.submit(); });
                addNew.init(this.textContent);
            });

            $("#btn-remove").click(function () {
                var remove = new AddNewOrRemoveLimit(
                    divAddNewOrRemoveLimit.clone(),
                    CMS.resolveRootUrl("LimitNewDownline/Remove"),
                    function () { formSearch.submit(); });
                remove.init(this.textContent, getCheckedUsernames());
            });
        }
    }

    function getLimitDownlineList(pageIndex, pageSize) {
        var url = formSearch.attr("action")
            + "?username=" + txtUsername.val()
            + "&level=" + ddlLevel.val()
            + "&site=" + ddlSite.fanexMultiSelect("getItems", true).join(",")
            + "&pageIndex=" + (pageIndex + 1)
            + "&pageSize=" + pageSize;
        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            gridOptions = gridOptions || initGridOptions();
            gridOptions.pagerOption.currentPage = pageIndex;
            gridOptions.pagerOption.itemsPerPage = pageSize;
            gridOptions.pagerOption.totalItem = jsonData.TotalItems;
            gridOptions.bodyRows = renderLimitDownlineList(jsonData.LimitDownlineList, pageIndex * pageSize);

            freegrid.removeClass("display-none").fanexGrid(gridOptions);

            if (hasHostPermission && jsonData.TotalItems > 0) {
                bindEventsAfterRenderLimitDownlineList();
            }
        });
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: "#", Width: 80, Name: "colNo", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: resources.username, Width: 220, Name: "colUsername", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
            { HeaderText: resources.level, Width: 80, Name: "colLevel", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.currency, Width: 100, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.site, Width: 140, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.addedBy, Width: 160, Name: "colAddedBy", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.addedDate, Width: 140, Name: "colAddedDate", HeaderAlign: "center", CellAlign: "center" }
        ];
        hasHostPermission && columns.push(
            { HeaderText: resources.remove + '<br/><input type="checkbox" id="chk-select-all">', Width: 80, Name: "colSelect", HeaderAlign: "center", CellAlign: "center" });

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
                pagerSelectCallBack: function (pageIndex, pageSize) {
                    cachedPageSize = pageSize;
                    getLimitDownlineList(pageIndex, pageSize);
                }
            }
        };
    }

    function renderLimitDownlineList(data, startIndex) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colNo", Value: startIndex + index + 1 },
                { Name: "colUsername", Value: value.Username },
                { Name: "colLevel", Value: value.Level },
                { Name: "colCurrency", Value: value.Currency },
                { Name: "colSite", Value: value.Site },
                { Name: "colAddedBy", Value: value.AddedBy },
                { Name: "colAddedDate", Value: value.AddedDate }
            ]
            hasHostPermission && columns.push({ Name: "colSelect", Value: renderRemoveCell(value.Username) });

            bodyRows.push({ Columns: columns });
        });

        return bodyRows;
    }

    function renderRemoveCell(username) {
        return [
            '<input type="checkbox" class="chk-select-item" data-username="', username, '" />'
        ].join("");
    }

    function bindEventsAfterRenderLimitDownlineList() {
        var checkall = $("#chk-select-all"),
            checkitems = $(".chk-select-item");

        checkall.off("change").on("change", function () {
            checkitems.prop("checked", this.checked);
        });

        checkitems.off("change").on("change", function () {
            var numberOfCheckedCheckItems = $(".chk-select-item:checked").length;

            if (numberOfCheckedCheckItems === 0) {
                checkall.prop("indeterminate", false).prop("checked", false);
            } else if (numberOfCheckedCheckItems === checkitems.length) {
                checkall.prop("indeterminate", false).prop("checked", true);
            } else {
                checkall.prop("indeterminate", true);
            }
        });
    }

    function getCheckedUsernames() {
        var usernames = [];
        $(".chk-select-item:checked").each(function () {
            var checkbox = $(this);
            usernames.push(checkbox.data("username"));
        });

        return usernames.join(",");
    }

    $(function () {
        ddlSite.fanexMultiSelect({
            minWidth: 180,
            checkAllText: "All",
            listData: sites.map(function (item) {
                return { id: item, value: item, text: item, checked: true, enable: true };
            })
        });

        bindEvents();

        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
        txtUsername.focus();
    });
})(jQuery, resources);