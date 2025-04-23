"use strict";
$(document).ready(function () {
    var currencyGroupBLC = new Fanex.CMS.CurrencyGroup();
    currencyGroupBLC.InitPage();
});

Namespace.Register('Fanex.CMS.CurrencyGroup');
Fanex.CMS.CurrencyGroup = function () {
    let elements = {
        isCredit: true,
        $addDialog: null,
        groupedCreditCur: [],
        groupedDepositCur: [],
        cachedGridData: []
    };
    const btnAddNew = $("#btnAddNewCurrencyGroup");
    let hasHostPermisson = btnAddNew.length > 0;
    let cachedGridData = [];
    let creditCurrencies = [], depositCurrencies = [];
    let errDiv = $("#errCurrencyGroup");

    const getCurrencyGroupLabel = (isCredit) => viewResources.lblCurrencyGroup.format(isCredit ? viewResources.Credit : viewResources.Deposit);
    const getCurrencyGroupIdLabel = (isCredit) => viewResources.lblCurrencyGroupId.format(isCredit ? viewResources.Credit : viewResources.Deposit);

    function registerRadiobuttonHandler() {
        $(".rdbCurrencyGroup").off('change').change(function () {
            elements.isCredit = $(this).val() === 'Credit';
            GetCurrencyGroup(elements.isCredit, RenderGridData);
        });
    }

    function GetCurrencyGroup(isCredit, renderFunction) {
        cachedGridData = [];
        const apiUrl = CMS.resolveRootUrl("CurrencyGroupSetting/Search?isCredit=" + isCredit);
        CMS.getJson(apiUrl, (data) => {
            const jsonData = $.parseJSON(data);

            cachedGridData = jsonData.CurrencyGroups;
            if (isCredit) {
                elements.groupedCreditCur = cachedGridData.map(curGroup => curGroup.ExchangeIDs).join().split(',');
                creditCurrencies = jsonData.Currencies;
            } else {
                elements.groupedDepositCur = cachedGridData.map(curGroup => curGroup.ExchangeIDs).join().split(',');
                depositCurrencies = jsonData.Currencies;
            }
            renderFunction && renderFunction();
        });
    }
    

    function RenderGridData() {
        renderGrid(cachedGridData);
        
        registerEventAfterRenderGrid();
    }

    function renderGrid(data) {

        const currencyGroupLabel = getCurrencyGroupLabel(elements.isCredit);
        const currencyIdLabel = getCurrencyGroupIdLabel(elements.isCredit);
        let html = [
            `<table class="tbl-freegrid" style="height: calc(100vh - 200px);"><thead style="z-index: ${data.length + 2}"><tr>`,
            `<th class="width-40">${viewResources.lblSymbol_No}</th>`,
            `<th class="width-160">${currencyGroupLabel}</th>`,
            `<th class="width-120">${currencyIdLabel}</th>`,
            hasHostPermisson ? `<th class="width-60">${viewResources.lblEdit}</th>` : '',
            hasHostPermisson ? `<th class="width-60">${viewResources.lblRemove}</th>` : '',
            '</tr></thead > ',
            '<tbody>'
        ];

        if (data.length === 0) {
            let colspan = hasHostPermisson ? 6 : 4;
            html.push(buildRowNoData(colspan, resources.noData));
        } else {
            data.forEach(function (item, index) {
                html.push(buildRowItem(item, index, data.length - index));
            });
        }
        html.push('</tbody></table>');

        $("#grid-currency-table").removeClass("display-none").html(html.join(""));
    }

    function buildRowItem(group, index, zindex) {
        const curCol = `<div class="col-view-cur-{0} text-center">{1}</div><div class="dropdown-cell col-edit-cur-{0}"><select id="ddlEditCur-{0}" class="select-customer-class hidden"></select></div>`;
        const delCol = `<span class="ico-delete" data-id="{0}" title="{1}"></span>`;
        const editCol = `<span class="ico-edit ico-edit-cur col-view-cur-{0}" data-pos="{0}" title={1}></span><span id="btn-save-cur-{0}" class="ico-save-16 ico-save-cur col-edit-cur-{0} hidden" title="Save" data-pos="{0}"></span><span class="ico-cancel ico-cancel-cur col-edit-cur-{0} ml-10 hidden" data-pos="{0}" title="Cancel"></span>`;

        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
            `<td class="text-center width-40"><div class="view-item"><div>${index + 1}</div></td>`,
            `<td class="rule-cell width-160"><div class="view-item cell-customer-class">${curCol.format(index, group.CurrencyNames)}</div></td>`,
            `<td class="text-center width-120"><div class="view-item text-center">${group.CurrencyGroupID}</div></td>`,
            hasHostPermisson ? `<td class="width-60 text-center">${editCol.format(index, "Edit")}</td>` : '',
            hasHostPermisson ? `<td class="width-60 text-center">${delCol.format(group.CurrencyGroupID, "Delete")}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function registerEventAfterRenderGrid() {
        $(".ico-edit-cur").off("click").click(function () {
            var pos = +$(this).data("pos");

            $(`.col-edit-cur-${pos}`).removeClass("hidden");
            $(`.col-view-cur-${pos}`).each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            });
            var currencyGroup = cachedGridData[pos];
            RenderDDLCurEdit(`ddlEditCur-${pos}`, currencyGroup);

            $(`#btn-save-cur-${pos}`).click(function () {
                SaveEditCur(pos);
            });

        });

        $(".ico-cancel-cur").off("click").click(function () {
            var pos = +$(this).data("pos");
            $(`.col-view-cur-${pos}`).removeClass("hidden");
            $(`.col-edit-cur-${pos}`).each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            });
            $(`#ddlEditCur-${pos}`).html('').addClass("hidden");
            $(`#btn-save-cur-${pos}`).off('click');
        });

        $(".ico-delete").off("click").click(function () {
            var id = +$(this).data("id");
            CMS.showConfirmMessage("Are you sure you want to delete this group?", () => CallDelete(id));
        });
    }

    function SaveEditCur(pos) {
        var selectedCurGroup = cachedGridData[pos];
        var requestBody = {
            ...selectedCurGroup
        };
        var selectedCurs = $(`#ddlEditCur-${pos}`).multiSelect("getCheckedItems").join();
        if (validateRequiredField($(`#ddlEditCur-${pos}`), selectedCurs)) {
            requestBody.ExchangeIDs = selectedCurs;
            CallUpdate(requestBody);
        }
    }

    function CallUpdate(requestBody) {
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("CurrencyGroupSetting/Update?isCredit=" + elements.isCredit),
            requestBody,
            function (response) {
                var result = typeof response === "string" ? JSON.parse(response) : response;
                if (result.IsSuccess) {
                    CMS.showSuccessMessage("Update successfully!", () => GetCurrencyGroup(elements.isCredit, RenderGridData));
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    function CallDelete(groupId) {
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CurrencyGroupSetting/Delete?groupId=${groupId}&isCredit=${elements.isCredit}`),
            null,
            function (response) {
                var result = typeof response === "string" ? JSON.parse(response) : response;
                if (result.IsSuccess) {
                    CMS.showSuccessMessage("Delete successfully!", () => GetCurrencyGroup(elements.isCredit, RenderGridData));
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    function RenderDDLCurEdit(divId, currencyGroup) {
        $(`#${divId}`).html('');
        $(divId).removeClass("hidden");
        var currencies = creditCurrencies;
        var groupedCur = elements.groupedCreditCur;
        if (!elements.isCredit) {
            currencies = depositCurrencies;
            groupedCur = elements.groupedDepositCur;
        }

        var selectedCurs = currencyGroup.ExchangeIDs.split(",");

        currencies.forEach(function (cur, index) {
            cur.Enable = selectedCurs.includes(cur.Value) || !groupedCur.includes(cur.Value);
            cur.Checked = selectedCurs.includes(cur.Value);
            cur.Id = divId + 'opt' + cur.Value;
        });
        var option = {
            minWidth: 150,
            maxHeight: 75,
            checkAllText: 'All',
            listData: currencies,
            showFilter: true,
            showFooter: false,
            shadow: true
        };
        $(`#${divId}`).multiSelect(option);


        $(`#${divId}`).next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').on('keypress', function (e) {
            if (e.which == 13) {
                var filterValue = $(`#${divId}`).next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val().toLowerCase();
                var selectedCurrency = filterValue.split(/(?:,| )+/);
                option.listData.forEach(function (cur) {
                    var curOption = cur.Text.split(" ")[1].toLowerCase();
                    $(`#${cur.Id}`).attr('checked', cur.Enable && selectedCurrency.indexOf(curOption) >= 0);
                });

                $(`#${divId}`).next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val('').keyup();
                $(`#${divId}`).trigger('change');
            }
        });
    }

    function buildRowNoData(colspan, textNoData) {
        return `<tr><td colspan="${colspan}" class="text-red align-center">${textNoData}</td></tr>`;
    }

    function registerEvents() {
        $("#tab-3").click(function () {
            GetCurrencyGroup(elements.isCredit, RenderGridData);
        });

        btnAddNew.off('click').click(function () {
            openAddPopup();
        });
    }

    function openAddPopup() {
        CMS.modal(viewResources.lblAddCurrencyGroup, elements.$addDialog.clone(), { resizable: false });
        $("#btnAddGroup").click(addCurrencyGroup);
        $("#btnCancelAdd").click(CMS.closePopup);
        $("#ddlGroupType").combobox(true).change(RenderCurrencyDDL).trigger('change');
    }

    function addCurrencyGroup() {
        var selectedCurs = $(`#divDdlNewCurrency`).multiSelect("getCheckedItems").join();
        var currencyGroup = {
            ExchangeIDs: selectedCurs
        };
        var isCredit = $("#ddlGroupType").val() === "Credit";
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("CurrencyGroupSetting/Insert?isCredit=" + isCredit),
            currencyGroup,
            function (response) {
                var result = typeof response === "string" ? JSON.parse(response) : response;
                if (result.IsSuccess) {
                    CMS.showSuccessMessage("Insert successfully!", () => {
                        CMS.closePopup(); GetCurrencyGroup(isCredit, RenderGridData); });
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            });
    }

    function RenderCurrencyDDL() {
        var currencies = creditCurrencies;
        var groupedCur = elements.groupedCreditCur;
        var selectedGroup = $("#ddlGroupType").val();
        if (selectedGroup === 'Deposit') {
            currencies = depositCurrencies;
            groupedCur = elements.groupedDepositCur;
        }

        currencies.forEach(function (cur, index) {
            cur.Id = 'optcurrencyNew' + cur.Value;
            cur.Enable = !groupedCur.includes(cur.Value);
        });
        var option = {
            minWidth: 280,
            maxHeight: 75,
            checkAllText: 'All',
            listData: currencies,
            showFilter: true,
            showFooter: false,
            showTooltip: false,
            shadow: true
        };
        $('#divDdlNewCurrency').multiSelect(option);

        $('#divDdlNewCurrency').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').on('keypress', function (e) {
            if (e.which == 13) {
                var filterValue = $('#divDdlNewCurrency').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val().toLowerCase();
                var selectedCurrency = filterValue.split(/(?:,| )+/);
                option.listData.forEach(function (cur) {
                    var curOption = cur.Text.split(" ")[1].toLowerCase();
                    $('#' + cur.Id).prop('checked', selectedCurrency.indexOf(curOption) >= 0);
                });

                $('#divDdlNewCurrency').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val('').keyup();
                $('#divDdlNewCurrency').trigger('change');
            }
        });
    }

    function validateRequiredField(div, value) {
        if (isEmptyValue(value)) {
            CMS.showTipsyError(div, viewResources.requiredField, "n");
            return;
        }

        return value;
    }

    function isEmptyValue(value) {
        return typeof value === "undefined" || value === null || value === "";
    }

    this.InitPage = function () {
        registerRadiobuttonHandler();
        registerEvents();
        GetCurrencyGroup(false);
        elements.$addDialog = $("#addNewCurrencyGroupForm");
        $("#div-popup").remove();
    };
};

