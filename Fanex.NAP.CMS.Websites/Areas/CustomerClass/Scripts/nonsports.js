
(function ($) {
    let cachedData = null,
        cachedLicenseeSites = [],
        cachedCreditSites = [],
        cachedFilter = null,
        ddlCustomerClasses = null,
        ddlSites = null,
        ddlProducts = null,
        freeGrid = $("#freegrid"),
        cachedAddNewRule = null,
        hasHostPermisson = null;

    function init() {
        renderCustomerClass();
        renderSites();
        renderProducts();

        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClasses: customerClassHelper.getSelectedReplaceAllValue("ddlCustomerClasses", "0", customerClassList.length),
                sites: customerClassHelper.getSelectedReplaceAllValue("ddlSites", "0", siteList.length),
                products: customerClassHelper.getSelectedReplaceAllValue("ddlProducts", "256", productList.length)
            };

            cachedFilter = formData;
            search();
        }).trigger("submit");

        hasHostPermisson = $("#btnAdd").length > 0;

        hasHostPermisson && $("#btnAdd").off("click").click(enableRowForAdding);
    }


    function renderCustomerClass() {
        let optionList = [];
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: true, enable: true })
        );

        ddlCustomerClasses = customerClassHelper.renderMultiSelect(
            "#ddlCustomerClasses",
            {
                minWidth: 140,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }

    function getCreditAndLicenseeSites() {
        let creditOrLicensee;
        cachedCreditSites = [];
        cachedLicenseeSites = [];
        siteList.forEach(function (item) {
            creditOrLicensee = item.IsLicensee ? cachedLicenseeSites : cachedCreditSites;
            creditOrLicensee.push({
                value: item.Id,
                text: item.Name,
                checked: false,
                enable: true
            });
        });
    }

    function renderSites() {
        getCreditAndLicenseeSites();
        var listData = customerClassHelper.buildGroupSiteList(true, true, "All", cachedCreditSites, cachedLicenseeSites);
        ddlSites = customerClassHelper.renderMultiSelect(
            "#ddlSites",
            {
                minWidth: 180,
                maxHeight: 300,
                listData: listData,
                isCheckAll: true,
                showFilter: true
            }
        );
    }

    function renderProducts() {
        let optionList = [];
        productList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: true, enable: true })
        );

        ddlProducts = customerClassHelper.renderMultiSelect(
            "#ddlProducts",
            {
                minWidth: 140,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }

    function search() {
        cachedData = [];
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Nonsports/Search`),
            cachedFilter,
            function (result) {
                cachedData = typeof result === "string" ? JSON.parse(result) : result;
            },
            null,
            function () {
                CMS.enableInput();
                renderGrid(cachedData);
                bindEventsAfterRenderGridSuccessfully();
            });
    }

    function bindEventsAfterRenderGridSuccessfully() {
        freeGrid.find(".ico-edit")
            .off("click")
            .click(showEditItem);
        freeGrid.find(".ico-trash")
            .off("click")
            .click(showDeleteConfirmPopup);
        freeGrid.find(".link-product")
            .off("click")
            .click(function () { editingGridHelper.handleViewItems($(this), resources.product, "products"); });
        freeGrid.find(".link-customerclass")
            .off("click")
            .click(function () { editingGridHelper.handleViewItems($(this), resources.customerClass, "customerclasses"); });
        freeGrid.find(".link-site")
            .off("click")
            .click(function () { editingGridHelper.handleViewItems($(this), resources.site, "sites"); });
        freeGrid.find(".checkbox-enable")
            .off("click")
            .click(showEnableDisablePopup);
        freeGrid.find(".is-editting").trigger('click');
    }

    function renderGrid(data) {

        let html = [
            `<table class="tbl-freegrid" style="height: calc(100vh - 150px);"><thead style="z-index: ${data.length + 2}"><tr>`,
            `<th class="width-150">${resources.title}</th>`,
            `<th class="width-250">${resources.customerClass}</th>`,
            `<th class="width-180">${resources.site}</th>`,
            `<th class="width-180">${resources.product}</th>`,
            `<th class="width-100">${resources.maxBet}</th>`,
            `<th class="width-60">${resources.enable}</th>`,
            hasHostPermisson ? `<th class="width-60">${resources.action}</th>` : '',
            '</tr></thead > ',
            '<tbody>'
        ];

        if (cachedAddNewRule === null && data.length === 0) {
            let colspan = hasHostPermisson ? 7 : 6;
            html.push(editingGridHelper.buildRowNoData(colspan, resources.noData));
        } else {
            if (cachedAddNewRule !== null) {
                html.push(buildAddRow(data.length + 1));
            }

            data.forEach(function (item, index) {
                html.push(buildRowItem(item, index, data.length - index));
            });
        }
        html.push('</tbody></table>');

        freeGrid.removeClass("display-none").html(html.join(""));

        cachedAddNewRule !== null && bindingEventForAddRow();
    }

    function buildRowItem(policy, index, zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
        `<td class="rule-cell width-150"><div class="view-item">${CMS.htmlEncode(policy.PolicyTitle)}</div > <div class="dropdown-cell edit-item hidden"><input type="text" class="inputTitle" id="editTitle-${policy.PolicyId}" maxlength="500" /></div></td>`,
        `<td class="rule-cell width-250"><div class="view-item text-center cell-customer-class">${editingGridHelper.renderCustomerClassCell(policy.CustomerClass)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCustomerClass-${policy.PolicyId}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-180"><div class="view-item text-center">${editingGridHelper.renderSiteCell(policy.Sites, siteList, cachedCreditSites, cachedLicenseeSites, "0", "-1", "-2", true)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditSite-${policy.PolicyId}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-180"><div class="view-item text-center">${editingGridHelper.renderProductCell(policy.Products, productList)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditProduct-${policy.PolicyId}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-100"><div class="view-item text-center">${numeral(policy.MaxBet).format(CMS.formats.IntegerFormat)}</div><div class="dropdown-cell edit-item hidden"><input type="text" id="inputMaxBet-${policy.PolicyId}" /></div></td>`,
            `<td class="rule-cell width-60 text-center">${editingGridHelper.renderEnableCell(policy.PolicyId, CMS.htmlEncode(policy.PolicyTitle), policy.IsEnable, hasHostPermisson)}</td>`,
        hasHostPermisson ? `<td class="width-60 text-center">${renderActionCell(policy.PolicyId, index, policy.edittingValue && policy.edittingValue !== null)}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function renderActionCell(policyId, index, isEditting) {
        return [
            `<span class="ico-edit ${isEditting ? "is-editting" : ""} mid-margin-right view-item" title="Edit" data-policyid="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-trash view-item" title="Delete" data-policyid="${policyId}"></span>`,
            `<span class="ico-save-16 mid-margin-right edit-item hidden" title="Save" data-policyid="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-cancel edit-item hidden" title="Cancel" data-policyid="${policyId}" data-index="${index}"></span>`
        ].join("");
    }

    function buildAddRow(zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
            `<td class="rule-cell width-150 add-cell"><div><input type="text" class="inputTitle" id="addNewTitle" maxlength="500" /></div></td>`,
            `<td class="rule-cell width-250 add-cell"><div><div id="ddlAddNewCustomerClass" class="select-customer-class"></div></div</td>`,
            `<td class="rule-cell width-180 add-cell"><div><div id="ddlAddNewSite" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-180 add-cell"><div><div id="ddlAddNewProduct" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-100 add-cell"><div><input type="text" class="inputTitle" id="addNewMaxBet" /></div></td>`,
            `<td class="rule-cell width-60 text-center"><input type="checkbox" class="ckbEnable" id="addNewEnable" checked /></td>`,
        hasHostPermisson ? `<td class="width-60 text-center">${renderAddActionCell()}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function renderAddActionCell() {
        return [
            '<span class="ico-save-16 mid-margin-right" title="Save" id="btn-submit-add"></span>',
            '<span class="ico-cancel" title="Cancel" id="btn-cancel-add"></span>'
        ].join("");
    }

    function bindingEventForAddRow() {
        $(`#addNewTitle`).val(cachedAddNewRule.PolicyTitle).change(function () { cachedAddNewRule.PolicyTitle = $(`#addNewTitle`).val(); });
        editingGridHelper.renderAddEditCustomerClass("ddlAddNewCustomerClass", cachedAddNewRule.CustomerClass, function () { onCustomerClassChange("ddlAddNewCustomerClass"); });
        editingGridHelper.renderAddEditSite("ddlAddNewSite", cachedAddNewRule.Sites, cachedCreditSites, cachedLicenseeSites, cachedAddNewRule.Sites === "0", cachedAddNewRule.Sites.includes("-1"), cachedAddNewRule.Sites.includes("-2"), function () { onSiteChange("ddlAddNewSite"); });
        editingGridHelper.renderAddEditProduct("ddlAddNewProduct", cachedAddNewRule.Products, function () { onProductChange("ddlAddNewProduct"); }, cachedAddNewRule.Products === "256");
        var maxbet = new AutoNumeric(`#addNewMaxBet`, {
            minimumValue: 0,
            maximumValue: 2147483647,
            decimalPlaces: 0,
            onInvalidPaste: 'ignore'
        });
        $(`#addNewMaxBet`).change(function () { cachedAddNewRule.MaxBet = maxbet.get(); });
        $(`#addNewEnable`).change(function () { cachedAddNewRule.IsEnable = $(`#addNewEnable`).is(":checked"); });
        $("#btn-cancel-add").click(cancelAddNew);
        $("#btn-submit-add").click(submitAddNew);
    }

    function enableRowForAdding() {
        editingGridHelper.clearAllTipsyErrors();
        if ($("#addNewTitle").length > 0) {
            return;
        }

        if (cachedAddNewRule === null) {
            cachedAddNewRule = {
                PolicyId: 0,
                PolicyTitle: "",
                CustomerClass: "",
                Products: "",
                Sites: "",
                MaxBet: "",
                IsEnable: true
            };
        }

        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
        $(`#addNewTitle`).focus();
    }

    function showEditItem() {
        editingGridHelper.clearAllTipsyErrors();
        var rowEdit = $(this).closest('tr');
        var policyId = $(this).data("policyid");
        var index = $(this).data("index");
        var editPolicy = cachedData[index].edittingValue || cachedData[index];
        cachedData[index].edittingValue = { ...cachedData[index] };
        $(`#editTitle-${policyId}`).val(editPolicy.PolicyTitle).change(function () { cachedData[index].edittingValue.PolicyTitle = $(`#editTitle-${policyId}`).val(); });
       var editMaxBet = new AutoNumeric(`#inputMaxBet-${policyId}`, {
            minimumValue: 0,
            maximumValue: 2147483647,
            decimalPlaces: 0,
            onInvalidPaste: 'ignore'
        }).set(editPolicy.MaxBet);
        $(`#inputMaxBet-${policyId}`).off("change").change(function () { cachedData[index].edittingValue.MaxBet = editMaxBet.get(); });
        
        rowEdit.find("div[class*=view-item]").addClass("hidden");
        rowEdit.find("div[class*=edit-item]").removeClass("hidden");
        rowEdit.find("input[class*=edit-item]").removeClass("hidden");
        rowEdit.find("span[class*=view-item]").addClass("hidden");
        rowEdit.find("span[class*=edit-item]").removeClass("hidden");
        editingGridHelper.renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass, function () { onCustomerClassChange(`ddlEditCustomerClass-${policyId}`, index); });
        editingGridHelper.renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites, cachedCreditSites, cachedLicenseeSites, editPolicy.Sites === "0", editPolicy.Sites.includes("-1"), editPolicy.Sites.includes("-2"), function () { onSiteChange(`ddlEditSite-${policyId}`, index); });
        editingGridHelper.renderAddEditProduct(`ddlEditProduct-${policyId}`, editPolicy.Products, function () { onProductChange(`ddlEditProduct-${policyId}`, index); }, editPolicy.Products === "256");
        $(`#ckbEdit-${policyId}`).change(function () { cachedData[index].edittingValue.IsEnable = $(`#ckbEdit-${policyId}`).is(":checked"); });

        rowEdit.find("span[class*=ico-save-16]")
            .off("click")
            .click(saveEditItem);
        rowEdit.find("span[class*=ico-cancel]")
            .off("click")
            .click(function () { cancelEditItem($(this)); editMaxBet.remove(); });
        $(`#editTitle-${policyId}`).focus();
    }

    function saveEditItem() {
        editingGridHelper.clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyId);
        var updatePolicy = {
            PolicyId: policyId,
            PolicyTitle: editingGridHelper.validateTitle($(`#editTitle-${policyId}`)),
            CustomerClass: editingGridHelper.validateRequiredField($(`#ddlEditCustomerClass-${policyId}`), customerClassHelper.getSelectedReplaceAllValue(`ddlEditCustomerClass-${policyId}`, "0", customerClassList.length)),
            Products: editingGridHelper.validateRequiredField($(`#ddlEditProduct-${policyId}`), customerClassHelper.getSelectedReplaceAllValue(`ddlEditProduct-${policyId}`, "256", productList.length)),
            Sites: editingGridHelper.validateRequiredField($(`#ddlEditSite-${policyId}`), editingGridHelper.getSelectedSites(`ddlEditSite-${policyId}`, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2")),
            MaxBet: editingGridHelper.validateRequiredField($(`#inputMaxBet-${policyId}`), cachedData[index].edittingValue.MaxBet),
            IsEnable: $(`#ckbEdit-${policyId}`).is(":checked")
        };

        if (!isValidPolicy(updatePolicy)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Nonsports/Update`),
            updatePolicy,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    cachedData[index] = updatePolicy;
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function cancelEditItem(cancelBtn) {
        editingGridHelper.clearAllTipsyErrors();
        var rowEdit = cancelBtn.closest('tr');
        var index = cancelBtn.data("index");
        cachedData[index].edittingValue = null;
        rowEdit.find("div[class*=view-item]").removeClass("hidden");
        rowEdit.find("div[class*=edit-item]").addClass("hidden");
        rowEdit.find("input[class*=edit-item]").addClass("hidden");
        rowEdit.find("span[class*=view-item]").removeClass("hidden");
        rowEdit.find("span[class*=edit-item]").addClass("hidden");
        rowEdit.find("span[class*=ico-save-16]")
            .off("click");
        rowEdit.find("span[class*=ico-cancel]")
            .off("click");
    }

    function cancelAddNew() {
        editingGridHelper.clearAllTipsyErrors();
        if ($("#addNewTitle").length <= 0) {
            return;
        }
        cachedAddNewRule = null;
        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
    }

    function submitAddNew() {
        editingGridHelper.clearAllTipsyErrors();
        if (!cachedAddNewRule) {
            return;
        }

        editingGridHelper.validateTitle($(`#addNewTitle`));
        editingGridHelper.validateRequiredField($(`#addNewMaxBet`), $(`#addNewMaxBet`).val());
        editingGridHelper.validateRequiredField($(`#ddlAddNewCustomerClass`), cachedAddNewRule.CustomerClass);
        editingGridHelper.validateRequiredField($(`#ddlAddNewProduct`), cachedAddNewRule.Products);
        editingGridHelper.validateRequiredField($(`#ddlAddNewSite`), cachedAddNewRule.Sites);

        if (!isValidPolicy(cachedAddNewRule)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Nonsports/Add`),
            cachedAddNewRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    cachedAddNewRule.PolicyId = resultJson.PolicyId;
                    cachedData.unshift(cachedAddNewRule);
                    cachedAddNewRule = null;
                    CMS.showSuccessMessage(resultJson.Message);
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function showDeleteConfirmPopup() {
        editingGridHelper.clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyId);
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this title "${CMS.htmlEncode(cachedData[index].PolicyTitle)}"?`, function () { callDelete(policyId, index); });
    }

    function callDelete(policyId, index) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Nonsports/Delete?policyId=${policyId}`),
            null,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    cachedData.splice(index, 1);
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function showEnableDisablePopup() {
        event.preventDefault();
        editingGridHelper.clearAllTipsyErrors();
        var checkbox = $(this);
        var policyId = checkbox.data("policyid");
        var isEnable = checkbox.is(":checked");
        var action = isEnable ? "ENABLE" : "DISABLE";
        var policyTitle = checkbox.data("policytitle");
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">${action}</span> this title "${CMS.htmlEncode(policyTitle)}"?`, function () { callEnableDisable(policyId, isEnable, checkbox); });
    }

    function callEnableDisable(policyID, isEnable, checkbox) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Nonsports/Enable?policyID=${policyID}&enable=${isEnable}`),
            null,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    checkbox.prop('checked', isEnable).trigger("change");
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function onCustomerClassChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.CustomerClass = customerClassHelper.getSelectedReplaceAllValue(divId, "0", customerClassList.length);
        } else {
            cachedAddNewRule.CustomerClass = customerClassHelper.getSelectedReplaceAllValue(divId, "0", customerClassList.length);
        }
    }

    function onSiteChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        } else {
            cachedAddNewRule.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        }
    }

    function onProductChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Products = customerClassHelper.getSelectedReplaceAllValue(divId, "256", productList.length);
        } else {
            cachedAddNewRule.Products = customerClassHelper.getSelectedReplaceAllValue(divId, "256", productList.length);
        }
    }

    function isValidPolicy(policy) {
        return !isEmptyValue(policy.PolicyTitle)
            && !isEmptyValue(policy.CustomerClass)
            && !isEmptyValue(policy.Products)
            && !isEmptyValue(policy.Sites)
            && !isEmptyValue(policy.MaxBet);
    }

    function isEmptyValue(value) {
        return typeof value === "undefined" || value === null || value === "";
    }

    $(function () {
        init();
    });
})(jQuery);