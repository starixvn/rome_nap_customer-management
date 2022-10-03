"use strict";
(function ($) {
    var txtUsername = $("#txt-username"),
        ddlSite = $("#ddl-site"),
        ddlCurrency = $("#ddl-currency"),
        ddlDummyAgent = $("#ddl-dummy-agent"),
        ckbActive = $("#ckb-active"),
        formSearch = $("#formSearch"),
        freegrid = $("#freegrid"),
        divAddNew = $("#addNew"),
        gridOptions = null,
        submitedDummyAgentId = null;

    function bindEvents() {
        ddlCurrency.combobox();
        ddlSite.combobox();
        ddlDummyAgent.combobox();

        formSearch.submit(function (e) {
            e.preventDefault();
            getFollowBetAccountList();
        });

        if (hasHostPermission) {
            $("#btnAddNew").off().click(function () {
                var addNew = new AddNewFollowBetAccount(
                    divAddNew.clone(),
                    CMS.resolveRootUrl("ManageFollowBetAccount/AddNew"),
                    function () { formSearch.submit(); });
                addNew.init(resources.addNewTitlePopup);
            });
        }
    }

    function getFollowBetAccountList() {
        var siteName = ddlSite.val() === "0" ? "" : ddlSite.find("option:selected").text();
        var url = formSearch.attr("action")
            + "?username=" + txtUsername.val().trim().replace(/[\n\t]+/g, '')
            + "&siteName=" + siteName
            + "&currencyId=" + ddlCurrency.val()
            + "&dummyAgentId=" + ddlDummyAgent.val()
            + "&isActive=" + ckbActive.is(":checked");

        CMS.getJson(url, function (data) {
            var jsonData = $.parseJSON(data);

            gridOptions = gridOptions || initGridOptions();
            gridOptions.bodyRows = jsonData.length > 0 ? renderFollowBetAccountList(jsonData) : []; 

            freegrid.removeClass("display-none").fanexGrid(gridOptions);

            if (jsonData.length > 0) {
                bindEventsAfterRenderFollowBetAccountList();
                submitedDummyAgentId = ddlDummyAgent.val();
            }
        });
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: "#", Width: 40, Name: "colNo", HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0" },
            { HeaderText: resources.username, Width: 180, Name: "colUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.dummyUsername, Width: 140, Name: "colDummyUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.dummyCustId, Width: 140, Name: "colDummyCustId", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.agent, Width: 180, Name: "colAgent", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.currency, Width: 100, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.site, Width: 130, Name: "colSite", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.addedBy, Width: 150, Name: "colAddedBy", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.addedDate, Width: 120, Name: "colAddedDate", HeaderAlign: "center", CellAlign: "center" }
        ];

        hasHostPermission
            && columns.push(
                { HeaderText: resources.active, Width: 80, Name: "colActive", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: resources.action, Width: 80, Name: "colAction", HeaderAlign: "center", CellAlign: "center" }
            );

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 200,
            columns: columns
        };
    }

    function renderFollowBetAccountList(data) {
        var columns;
        var bodyRows = [];

        $.each(data, function (index, value) {
            columns = [
                { Name: "colNo", Value: index + 1 },
                { Name: "colUsername", Value: value.Username },
                { Name: "colDummyUsername", Value: value.DummyUsername },
                { Name: "colDummyCustId", Value: value.DummyCustId },
                { Name: "colAgent", Value: value.Agent },
                { Name: "colCurrency", Value: value.Currency },
                { Name: "colSite", Value: value.Site },
                { Name: "colAddedBy", Value: value.AddedBy },
                { Name: "colAddedDate", Value: value.AddedDate }
            ];
            hasHostPermission
                && columns.push(
                    { Name: "colActive", Value: renderActiveCell(value) },
                    { Name: "colAction", Value: renderActionCell(value) }
                );

            bodyRows.push({ Columns: columns });
        });

        return bodyRows;
    }

    function renderActiveCell(data) {
        var isChecked = data.IsActive ? 'checked' : '';
        return [
            '<input class="ckb-active" type="checkbox" '
            + isChecked + ' '
            + 'data-username=' + data.Username + ' '
            + '/>'
        ].join("");
    }

    function renderActionCell(data) {
        return [
            '<span class="ico-delete remove-account" title="Delete"'
            + 'data-username=' + data.Username + ' '
            + '"></span>'
        ].join("");
    }

    function bindEventsAfterRenderFollowBetAccountList() {
        $(".remove-account").off().click(handleRemoveClick);
        $(".ckb-active").off().change(handleCkbActiveChange);
    }

    function handleRemoveClick() {
        var username = $(this).attr("data-username");

        CMS.showConfirmMessage(resources.removeAccountConfirmationMessage, function () {
            CMS.callAjax(this, 'Remove', { username, dummyAgentId: submitedDummyAgentId }, handleResult);
        });
    }

    function handleCkbActiveChange() {
        var isChecked = $(this).is(":checked");
        $(this).prop("checked", !isChecked);

        var username = $(this).attr("data-username");

        CMS.showConfirmMessage(
            isChecked ? resources.activeAccountConfirmationMessage : resources.inactiveAccountConfirmationMessage,
            function () {
                CMS.callAjax(this, 'Update', { username, dummyAgentId: submitedDummyAgentId, isActive: isChecked }, handleResult);
            });
    }

    function handleResult(result) {
        var jsonData = $.parseJSON(result);
        if (jsonData.Code === 0) {
            CMS.showSuccessMessage(jsonData.Message);
            formSearch.submit();
        } else {
            CMS.showErrorMessage(jsonData.Message);
        }
    }

    $(function () {
        bindEvents();
        $("#divPopupContent").remove();
        txtUsername.focus();
    });
})(jQuery, resources);