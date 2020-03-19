"use strict";
(function ($, resources, CMS) {
    var gridOptions = null;
    var divContentGrid = $("#content-grid");
    var divFreeGrid = $("#freegrid");
    var sportType = $("#sportType").val();
    var formSearch = $("#formSearch");
    
    function renderGrid(data) {
        var bodyData = [];
        var JSONData = JSON.parse(data);

        $.each(JSONData, function (index, datum) {
            bodyData.push({
                Columns: [
                    { Name: "colUsername", Value: datum.Username },
                    { Name: "colLevel", Value: datum.Level },
                    { Name: "colSelect", Value: '<input type="checkbox" class="chk-select-item" data-custid="' + datum.CustId + '" />' }]
            });
        });

        gridOptions = gridOptions || getGridOptions();
        gridOptions.bodyRows = bodyData;

        divFreeGrid.fanexGrid(gridOptions);
        bindEventsAfterRenderGridSuccessfully();

        divContentGrid.removeClass("display-none");
    }

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: resources.username,
                Width: 300,
                Name: "colUsername",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: resources.level,
                Width: 120,
                Name: "colLevel",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: '<input type="checkbox" id="chk-select-all">',
                Width: 180,
                Name: "colSelect",
                HeaderAlign: "center",
                CellAlign: "center"
            }];

        return {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                text: resources.reportTitle,
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl("VirtualSettingControl/ManualUpdateVRAccount/Export"), formSearch.serializeArray());
                }
            },
            bodyRows: [],
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: false,
            gridExpandHeight: 250
        };
    }

    function bindEventsAfterRenderGridSuccessfully() {
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

    function unmarkManualUpdateAccounts(checkboxes) {
        CMS.disableInput();

        var custIds = [];
        $.each(checkboxes, function () {
            custIds.push($(this).data("custid"));
        });

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("VirtualSettingControl/ManualUpdateVRAccount/Unmark"),
            { sportType: sportType, custIds: custIds.join(",") },
            function (data) {
                var JSONData = JSON.parse(data);

                if (JSONData.Code === 0) {
                    CMS.showSuccessMessage(JSONData.Message);
                    formSearch.trigger("submit");
                } else {
                    CMS.showErrorMessage(JSONData.Message);
                }
            })
    }

    $(function () {
        formSearch.submit(function (event) {
            event.preventDefault();

            CMS.getJson(
                formSearch.attr("action") + "?" + formSearch.serialize(),
                renderGrid);
        });

        $(".btn-unmark").click(function () {
            var selectedAccounts = $(".chk-select-item:checked");

            selectedAccounts.length
                && CMS.showConfirmMessage(resources.unmarkConfirmMessage, function () {
                    unmarkManualUpdateAccounts(selectedAccounts);
                });
        });
    });
})(jQuery, resources, CMS);