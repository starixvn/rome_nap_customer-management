/* global Namespace, Fanex, CMS, Validator, labels, messages */
// 'use strict';

$(document).ready(function () {
    creditCollectionSummary.init();
});

var creditCollectionSummary = {
    $fromtoDatePicker: null,
    $ddlCode: null,
    $ddlStatus: null,
    $username: null,
    $divReport: null,
    $chkExcludeTestData: null,

    pageSize: 100,
    pageIndex: 1,
    parameters: {
        Code: "",
        AccountType: "",
        Status: "",
        LastBalanceDate: "",
        CurrentBalanceDate: "",
        ExcludeTestData: false,
        PageSize: 100,
        PageIndex: 1
    },

    StatusList: [
        { Id: 1, Name: "Alert" },
        { Id: 3, Name: "Normal" },
        { Id: 2, Name: "Unused" }
    ],
    getMultiselectSource: function (sources) {
        var dataList = [];

        var data;
        for (var i = 0, max = sources.length; i < max; i++) {
            data = sources[i];
            dataList.push({
                id: data.Id,
                value: data.Id,
                text: data.Name,
                checked: data.Id == 2 ? false : true,
                enable: true
            });
        }

        return dataList;
    },

    init: function () {
        creditCollectionSummary.cacheElements();
        creditCollectionSummary.bindEvents();
    },
    cacheElements: function () {
        creditCollectionSummary.$fromtoDatePicker = $("#fromtodate-picker");

        creditCollectionSummary.$ddlCode = $("#ddl-code");
        creditCollectionSummary.$ddlStatus = $("#ddl-status");
        creditCollectionSummary.$username = $("#txt-username");
        creditCollectionSummary.$divReport = $("#div-report");
        creditCollectionSummary.$chkExcludeTestData = $("#chk-exclude-test-data");
    },
    bindEvents: function () {
        $(".ui-combobox-input").attr("readonly", "readonly");
        creditCollectionSummary.$fromtoDatePicker.nexDatePicker({
            nexDatePickerType: 'daterange',
            datePickerFormat: {
                dateFormat: "M dd, yy",
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: null,
                maxDate: yesterday
            },
            firstDatePicker: {
                Id: 'dpk-fromdate',
                Label: viewResources.LastBalanceDate,
                LabelCss: 'col_5 lbl-field',
                TextboxCss: 'edit-field col_7'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: viewResources.CurrentBalanceDate,
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: 'col_6 lbl-field',
                TextboxCss: 'edit-field col_6'
            }
        });
        creditCollectionSummary.$ddlStatus.fanexMultiSelect({
            minWidth: 180,
            checkAllText: viewResources.All,
            isCheckAll: false,
            listData: creditCollectionSummary.getMultiselectSource(creditCollectionSummary.StatusList)
        });

        creditCollectionSummary.$ddlStatus = $("#ddl-status");

        $('#dpk-fromdate').datepicker('setDate', initFromDate);

        $("#btn-generate").off("click").on('click', () => {
            creditCollectionSummary.pageSize = 100;
            creditCollectionSummary.pageIndex = 1;
            creditCollectionSummary.generate();
        });
        $("#btn-export").off("click").on('click', function () {
            var dateRange = creditCollectionSummary.$fromtoDatePicker.nexDatePicker("getDateRange");

            var parameters = {
                Code: creditCollectionSummary.$username.val(),
                AccountType: creditCollectionSummary.$ddlCode.val(),
                Status: creditCollectionSummary.getSelectedStatus(),
                LastBalanceDate: dateRange.FromDate,
                CurrentBalanceDate: dateRange.ToDate,
                ExcludeTestData: creditCollectionSummary.$chkExcludeTestData.is(":checked"),
                PageSize: -1,
                PageIndex: -1
            };

            CMS.downloadFile(CMS.resolveRootUrl('CreditCollectionSummary/Export'), parameters);
        });
        $("#note-info").tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });

    },
    generate: function () {
        // Delay 70ms for jQuery-UI combo-box to verify its state.
        setTimeout(function () {
            CMS.closeMsgBox();
            var dateRange = creditCollectionSummary.$fromtoDatePicker.nexDatePicker("getDateRange");
            creditCollectionSummary.parameters = {
                Code: creditCollectionSummary.$username.val(),
                AccountType: creditCollectionSummary.$ddlCode.val(),
                Status: creditCollectionSummary.getSelectedStatus(),
                LastBalanceDate: dateRange.FromDate,
                CurrentBalanceDate: dateRange.ToDate,
                ExcludeTestData: creditCollectionSummary.$chkExcludeTestData.is(":checked"),
                PageSize: creditCollectionSummary.pageSize,
                PageIndex: creditCollectionSummary.pageIndex
            };

            if (creditCollectionSummary.parameters.status == "") {
                Validator.showError(creditCollectionSummary.$ddlStatus, viewResources.RequiredFieldError, 'n');
                return;
            }

            Validator.clearError(creditCollectionSummary.$ddlStatus);

            CMS.disableInput();

            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('CreditCollectionSummary/Generate'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(creditCollectionSummary.parameters),
                success: function (result) {
                    creditCollectionSummary.render(result);
                },
                error: function (xhr) {
                    CMS.handleAjaxError(xhr);
                },
                complete: function () {
                    CMS.enableInput();
                }
            });
        }, 70);
    },
    render: function (result) {
        var negativeFormat = '$(#,###.00)';

        var headerRows = [
            { HeaderText: viewResources.No, Width: 50, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center', MergeRowNumber: 2 },
            { HeaderText: viewResources.Code, Width: 200, Name: 'ColCode', HeaderAlign: 'center', CellAlign: 'left', MergeRowNumber: 2 },
            { HeaderText: viewResources.Headline + " / " + viewResources.Direct, Width: 80, Name: 'ColHeadline', HeaderAlign: 'center', CellAlign: 'center', MergeRowNumber: 2 },
            { HeaderText: viewResources.Status, Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center', MergeRowNumber: 2 }
        ];

        var columns = [
            { HeaderText: viewResources.No, Width: 50, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: viewResources.Code, Width: 200, Name: 'ColCode', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: viewResources.Headline + " / " + viewResources.Direct, Width: 80, Name: 'ColHeadline', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: viewResources.Status, Width: 80, Name: 'ColStatus', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        if (hasEditAccessRight) {
            columns.push({ HeaderText: viewResources.Edit, Width: 80, Name: 'ColEdit', HeaderAlign: 'center', CellAlign: 'center' });
            headerRows.push({ HeaderText: viewResources.Edit, Width: 80, Name: 'ColEdit', HeaderAlign: 'center', CellAlign: 'center', MergeRowNumber: 2 });
        }

        columns.push({ HeaderText: result.LastBalanceDateStr, Width: 120, Name: 'ColLastBalance', HeaderAlign: 'center', CellAlign: 'right' });
        columns.push({ HeaderText: result.CurrentBalanceDateStr, Width: 120, Name: 'ColCurrentBalance', HeaderAlign: 'center', CellAlign: 'right' });
        columns.push({ HeaderText: viewResources.DebtMovement, Width: 120, Name: 'ColDebtMovement', HeaderAlign: 'center', CellAlign: 'right' });
        columns.push({ HeaderText: "(" + viewResources.Collection + ")/" + viewResources.Payment, Width: 120, Name: 'ColPayment', HeaderAlign: 'center', CellAlign: 'right' });
        columns.push({ HeaderText: viewResources.NetMovement, Width: 120, Name: 'ColNetMovement', HeaderAlign: 'center', CellAlign: 'right' });

        headerRows.push({ HeaderText: viewResources.LastBalance, Width: 120, Name: 'ColLastBalance', HeaderAlign: 'center', CellAlign: 'right' });
        headerRows.push({ HeaderText: viewResources.CurrentBalance, Width: 120, Name: 'ColCurrentBalance', HeaderAlign: 'center', CellAlign: 'right' });
        headerRows.push({ HeaderText: viewResources.DebtMovement, Width: 120, Name: 'ColDebtMovement', HeaderAlign: 'center', CellAlign: 'right', MergeRowNumber: 2 });
        headerRows.push({ HeaderText: "(" + viewResources.Collection + ") / " + viewResources.Payment, Width: 120, Name: 'ColPayment', HeaderAlign: 'center', CellAlign: 'right', MergeRowNumber: 2 });
        headerRows.push({ HeaderText: viewResources.NetMovement, Width: 120, Name: 'ColNetMovement', HeaderAlign: 'center', CellAlign: 'right', MergeRowNumber: 2 });
        headerRows.push({ HeaderText: viewResources.CollectionBreakdown, Width: 120, Name: 'ColCollectionBreakdown-0', HeaderAlign: 'center', CellAlign: 'right', MergeColNumber: result.BreakDowns.length });

        for (var i = 0; i < result.BreakDowns.length; i++) {
            var breakDown = result.BreakDowns[i];
            var text = breakDown.FromDateStr + " " + viewResources.To + " " + breakDown.ToDateStr;
            columns.push({ HeaderText: text, Width: 120, Name: 'ColCollectionBreakdown-' + i, HeaderAlign: 'center', CellAlign: 'right' });
        }

        var bodyRows = [];
        if (result.Groups.length) {
            var startIndex = creditCollectionSummary.parameters.PageSize * (creditCollectionSummary.parameters.PageIndex - 1);
            for (var i = 0; i < result.Groups.length; i++) {
                var group = result.Groups[i];

                var bodyColumns = [];
                bodyColumns.push({ Name: 'ColNo', Value: startIndex + i + 1, TdClass: "bold" });
                bodyColumns.push({ Name: 'ColCode', Value: group.Code });
                bodyColumns.push({ Name: 'ColHeadline', Value: creditCollectionSummary.getDirectHeadline(group.AccountType) });
                bodyColumns.push({ Name: 'ColStatus', Value: creditCollectionSummary.getStatusText(group.Status) });

                if (hasEditAccessRight) {
                    var text = group.AccountType == 3 || group.AccountType == 4 ?
                            "<span class='ico-edit' title='" + viewResources.Edit + "'" + " cust-id='" + group.CustId + "'></span>\
                             <span class='ico-save display-none' title='" + viewResources.Save + "'cust-id='" + group.CustId + "'></span>\
                             <span class='ico-undo display-none' title='" + viewResources.Revert + "'cust-id='" + group.CustId + "'></span>" : "";

                    bodyColumns.push({ Name: 'ColEdit', Value: text });
                }

                bodyColumns.push({ Name: 'ColLastBalance', Value: creditCollectionSummary.formatValue(group.LastBalance) });
                bodyColumns.push({ Name: 'ColCurrentBalance', Value: creditCollectionSummary.formatValue(group.CurrentBalance) });

                bodyColumns.push({ Name: 'ColDebtMovement', Value: creditCollectionSummary.formatValue(group.DebtMovement) });
                bodyColumns.push({ Name: 'ColPayment', Value: creditCollectionSummary.formatValue(group.Payment) });
                bodyColumns.push({ Name: 'ColNetMovement', Value: creditCollectionSummary.formatValue(group.NetMovement) });

                for (var j = 0; j < result.BreakDowns.length; j++) {
                    var breakDown = group.BreakDowns[j];
                    bodyColumns.push({ Name: 'ColCollectionBreakdown-' + j, Value: creditCollectionSummary.formatValue(breakDown) });
                }

                bodyRows.push({
                    CustomAttr: "headline cust-id='" + group.CustId + "' status='" + group.Status + "'",
                    Columns: bodyColumns
                });

                for (var k = 0; k < group.Downlines.length; k++) {
                    var downline = group.Downlines[k];
                    var downlineColumns = [];
                    downlineColumns.push({ Name: 'ColNo', Value: "", TdClass: "bold" });
                    downlineColumns.push({ Name: 'ColCode', Value: "&nbsp;&nbsp;" + downline.Code });
                    downlineColumns.push({ Name: 'ColHeadline', Value: creditCollectionSummary.getDirectHeadline(downline.AccountType) });
                    downlineColumns.push({ Name: 'ColStatus', Value: '' });

                    if (hasEditAccessRight) {
                        downlineColumns.push({ Name: 'ColEdit', Value: "" });
                    }

                    downlineColumns.push({ Name: 'ColLastBalance', Value: creditCollectionSummary.formatValue(downline.LastBalance) });
                    downlineColumns.push({ Name: 'ColCurrentBalance', Value: creditCollectionSummary.formatValue(downline.CurrentBalance) });

                    downlineColumns.push({ Name: 'ColDebtMovement', Value: creditCollectionSummary.formatValue(downline.DebtMovement) });
                    downlineColumns.push({ Name: 'ColPayment', Value: creditCollectionSummary.formatValue(downline.Payment) });
                    downlineColumns.push({ Name: 'ColNetMovement', Value: creditCollectionSummary.formatValue(downline.NetMovement) });

                    for (var j = 0; j < result.BreakDowns.length; j++) {
                        var breakDown = downline.BreakDowns[j];
                        downlineColumns.push({ Name: 'ColCollectionBreakdown-' + j, Value: creditCollectionSummary.formatValue(breakDown) });
                    }

                    bodyRows.push({
                        CustomAttr: "cust-id='" + group.CustId + "' status='" + group.Status + "'",
                        Columns: downlineColumns
                    });
                }
            }
        }

        var gridOptions = {
            showTitle: true,
            titleInfo: {
                text: viewResources.CreditCollectionSummary,
                showExcelButton: true,
                exportExcelFunction: function () {
                    var parameters = {
                        Code: creditCollectionSummary.parameters.Code,
                        AccountType: creditCollectionSummary.parameters.AccountType,
                        Status: creditCollectionSummary.parameters.Status,
                        LastBalanceDate: creditCollectionSummary.parameters.LastBalanceDate,
                        CurrentBalanceDate: creditCollectionSummary.parameters.CurrentBalanceDate,
                        ExcludeTestData: creditCollectionSummary.parameters.ExcludeTestData,
                        PageSize: -1,
                        PageIndex: -1
                    };

                    CMS.downloadFile(CMS.resolveRootUrl('CreditCollectionSummary/Export'), parameters);
                }
            },
            customizeHeader: true,
            groupHeaderRows: [{
                Columns: headerRows
            }],
            columns: columns,
            numberOfFrozenColumn: hasEditAccessRight ? 5 : 4,
            showDistinctRow: false,
            showNumberColumn: false,
            showPager: true,
            customPager: true,
            headerColumnHeight: 30,
            pagerOption: {
                itemsPerPage: creditCollectionSummary.parameters.PageSize,
                currentPage: creditCollectionSummary.parameters.PageIndex - 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: creditCollectionSummary.handleChangingPage,
                totalItem: result.TotalRows
            },
            gridExpandHeight: 240,
            bodyRows: bodyRows
        };

        creditCollectionSummary.$divReport.removeClass("display-none");
        creditCollectionSummary.$divReport.fanexGrid(gridOptions);
        $(".ico-edit").off("click").on("click", function () {
            var $this = $(this);
            var custId = $this.attr("cust-id");
            var row = $("tr[headline][cust-id='" + custId + "']");
            $this.addClass("display-none");

            row.find(".col-status").addClass("display-none");

            row.find(".ddl-status-group").removeClass("display-none");
            row.find(".ico-save").removeClass("display-none");
            row.find(".ico-undo").removeClass("display-none");
        });

        $(".ico-save").off("click").on("click", function () {
            var $this = $(this);
            var custId = $this.attr("cust-id");
            var row = $("tr[headline][cust-id='" + custId + "']");

            creditCollectionSummary.save(custId, row.find(".ddl-status-group").val());
        });

        $(".ico-undo").off("click").on("click", function () {
            CMS.closeMsgBox();
            var $this = $(this);
            var custId = $this.attr("cust-id");
            var row = $("tr[headline][cust-id='" + custId + "']");

            row.find(".ddl-status-group").addClass("display-none");
            row.find(".ddl-status-group").val(row.attr("status"));

            row.find(".ico-save").addClass("display-none");
            row.find(".ico-undo").addClass("display-none");
            row.find(".col-status").removeClass("display-none");
            row.find(".ico-edit").removeClass("display-none");
        });
    },
    handleChangingPage: function (pageIndex, itemsPerPage) {
        creditCollectionSummary.pageSize = itemsPerPage;
        creditCollectionSummary.pageIndex = pageIndex + 1;
        creditCollectionSummary.generate();
    },
    getStatusText: function (status) {
        var result = "<select class='ddl-status-group display-none'>\
                            <option value='1' " + (status == 1 ? "selected='selected'" : '') + ">" + viewResources.Alert + "</option>\
                            <option value='0' " + (status == 0 ? "selected='selected'" : '') + ">" + viewResources.Normal + "</option>\
                            <option value='2' " + (status == 2 ? "selected='selected'" : '') + ">" + viewResources.Unused + "</option>\
                      <select>";

        switch (status) {
            case 1:
                result += "<div class='col-status alert'>" + viewResources.Alert + "</div>";
                break;
            case 2:
                result += "<div class='col-status'>" + viewResources.Unused + "</div>";
                break;
            default:
                result += "<div class='col-status'>" + viewResources.Normal + "</div>";
                break;
        }

        return result;
    },
    formatValue: function (value) {
        if (value >= 0) {
            return numeral(value).format(CMS.formats.IntegerFormat);
        }

        return "<span class='format-negative'>(" + numeral(-value).format(CMS.formats.IntegerFormat) + ")</span>";
    },
    save: function (custId, status) {
        CMS.disableInput();
        CMS.closeMsgBox();

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('CreditCollectionSummary/Save'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({
                CustId: custId,
                Status: status
            }),
            success: function (result) {
                if (result.Code == 0) {
                    var row = $("tr[headline][cust-id='" + custId + "']");

                    row.find(".ddl-status-group").addClass("display-none");
                    row.find(".ico-save").addClass("display-none");
                    row.find(".ico-undo").addClass("display-none");
                    row.find(".ico-edit").removeClass("display-none");
                    row.attr("status", status);

                    var colStatus = row.find(".col-status");
                    switch (status) {
                        case "1":
                            colStatus.addClass("alert");
                            colStatus.html(viewResources.Alert);
                            break;
                        case "0":
                            colStatus.removeClass("alert");
                            colStatus.html(viewResources.Normal);
                            break;
                        case "2":
                            colStatus.removeClass("alert");
                            colStatus.html(viewResources.Unused);
                            break;
                    }

                    colStatus.removeClass("display-none");

                    var downlines = $("tr[cust-id='" + custId + "']");
                    downlines.attr("status", status);
                    downlines.find(".col-status").html(creditCollectionSummary.getStatusText(parseInt(status, 0)));

                } else {
                    CMS.showErrorMessage(result.Message);
                }
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    },
    getDirectHeadline: function (accountType) {
        return accountType == 4 ? viewResources.Headline : accountType == 3 ? viewResources.Direct : "";
    },
    getSelectedStatus: function () {
        var selectedStatus = creditCollectionSummary.$ddlStatus.fanexMultiSelect('getItems', true);
        var statusParam = [];
        for (var i = 0; i < selectedStatus.length; i++) {
            statusParam.push(selectedStatus[i] == 3 ? 0 : selectedStatus[i]);
        }

        return statusParam.join(',');
    }
};