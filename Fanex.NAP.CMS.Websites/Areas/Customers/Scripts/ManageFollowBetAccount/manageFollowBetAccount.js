"use strict";
(function ($) {
    var txtUsername = $("#txt-username"),
        ddlSite = $("#ddl-site"),
        ddlCurrency = $("#ddl-currency"),
        ddlDummyAgent = $("#ddl-dummy-agent"),
        ckbActive = $("#ckb-active"),
        ckbFollow = $("#ckb-follow"),
        formSearch = $("#formSearch"),
        freegrid = $("#freegrid"),
        divAddNew = $("#addNew"),
        gridOptions = null,
        submitedDummyAgentId = null,
        filterParams = null;

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
        filterParams = {
            username: txtUsername.val().trim().replace(/[\n\t]+/g, ''),
            siteName: siteName,
            currencyId: ddlCurrency.val(),
            dummyAgentId: ddlDummyAgent.val(),
            isActive: ckbActive.is(":checked"),
            isFollow: ckbFollow.is(":checked"),

            currencyName: ddlCurrency.find('option:selected').text(),
            dummyAgentName: ddlDummyAgent.find('option:selected').text(),
        };

        var url = formSearch.attr("action") + "?" + $.param(filterParams);

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
                { HeaderText: resources.follow, Width: 80, Name: "colFollow", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: resources.remove + '<br /><div class="act-delete"><input id="ckb-all" type="checkbox" /><div class="ico-delete-all" title="' + resources.remove + '"></div></div><div></div>', Width: 80, Name: "colRemove", HeaderAlign: "center", CellAlign: "center" }
            );

        return {
            showTitle: true,
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl("ManageFollowBetAccount/Export"), filterParams);
                }
            },
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
                    { Name: "colFollow", Value: renderFollowCell(value) },
                    { Name: "colRemove", Value: renderActionCell(value) }
                );

            bodyRows.push({ Columns: columns, CssClass: value.IsFollow ? "" : "unfollow" });
        });

        return bodyRows;
    }

    function renderActiveCell(data) {
        var isChecked = data.IsActive ? 'checked' : '';
        return [
            '<input class="ckb-active" type="checkbox" ',
            isChecked,
            ' data-username="',
            data.Username,
            '" />'
        ].join("");
    }

    function renderFollowCell(data) {
        var isChecked = data.IsFollow ? 'checked' : '';
        return [
            '<input class="ckb-follow" type="checkbox" ',
            isChecked,
            ' data-username="',
            data.Username,
            '" />'
        ].join("");
    }

    function renderActionCell(data) {
        return '<input type="checkbox" class="ckb-delete remove-account" data-username="' + data.Username + '" />';
    }

    function bindEventsAfterRenderFollowBetAccountList() {
        $("#ckb-all").off().click(handleCheckAllClick);
        $(".ckb-delete").off().click(handleRemoveClick);
        $(".ico-delete-all").off().click(handleRemoveAllClick);
        $(".ckb-active").off().change(handleCkbActiveChange);
        $(".ckb-follow").off().change(handleCkbFollowChange);
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

    function handleCkbFollowChange() {
        var $this = $(this);
        var isChecked = $this.is(":checked");
        $this.prop("checked", !isChecked);
        var username = $this.data("username");

        CMS.callAjax(
            this,
            'UpdateFollowStatus',
            { username, dummyAgentId: submitedDummyAgentId, isActive: isChecked },
            handleResult);
    }

    function handleResult(result) {
        var jsonData = $.parseJSON(result);
        if (jsonData.Code === 0) {
            jsonData.Message && CMS.showSuccessMessage(jsonData.Message);
            formSearch.submit();
        } else {
            CMS.showErrorMessage(jsonData.Message);
        }
    }

    function handleCheckAllClick() {
        if ($(this).attr('checked')) {
            $(".ckb-delete").attr("checked", true);
        } else {
            $(".ckb-delete").attr("checked", null);
        }
    }

    function handleRemoveAllClick() {
        var selectedBoxes = $(".ckb-delete:checked");
        if (selectedBoxes.length == 0) {
            return;
        }

        var selectedUsernames = [];
        selectedBoxes.each(function (i, e) {
            var username = $(e).attr("data-username");
            selectedUsernames.push(username);
        });

        CMS.showConfirmMessage(resources.removeAccountsConfirmationMessage, function () {
            CMS.callAjax(this, 'RemoveAccounts', { usernames: selectedUsernames.join(","), dummyAgentId: submitedDummyAgentId }, handleResult);
        });
    }

    function handleRemoveClick() {
        var chks = $(".ckb-delete").length;
        var selectedChks = $(".ckb-delete:checked").length;
        if (chks === selectedChks) {
            $("#ckb-all").attr('checked', true);
        } else {
            $("#ckb-all").attr('checked', null);
        }
    }
    $(function () {
        bindEvents();
        $("#divPopupContent").remove();
        txtUsername.focus();
    });
})(jQuery, resources);