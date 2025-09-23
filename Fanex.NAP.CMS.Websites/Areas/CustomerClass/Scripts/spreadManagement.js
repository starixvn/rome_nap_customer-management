
(function ($) {
    let cachedData = null,
        cachedLicenseeSites = [],
        cachedCreditSites = [],
        cachedFilter = null,
        ddlCustomerClasses = null,
        ddlSites = null,
        ddlCurrencies = null,
        filterSpread = null,
        freeGrid = $("#freegrid"),
        cachedAddNewRule = null,
        hasHostPermisson = null,
        btnAdd = null,
        btnUpload = null;

    function init() {
        renderCustomerClass();
        renderSites();
        renderCurrencies();
        filterSpread = new AutoNumeric("#filterSpread", {
            minimumValue: 0,
            maximumValue: 0.5,
            decimalPlaces: 2,
            onInvalidPaste: 'ignore'
        });
        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClass: ddlCustomerClasses.fanexMultiSelect("getItems", true).join(),
                site: getSelectedReplaceAllValue("ddlSites", "0", siteList.length),
                sportTypes: $("#ddlSportType").val(),
                currency: getSelectedReplaceAllValue("ddlCurrencies", "0", currencyList.length),
                spread: filterSpread.get()
            };
            if (formData.sportTypes === "0") {
                formData.sportTypes = sportList.map(sp => sp.Value).join(',');
            }
            cachedAddNewRule = null;
            cachedFilter = formData;
            search();
        });

        btnUpload = $("#btnUpload");
        btnAdd = $("#btnAdd");
        hasHostPermisson = btnAdd.length > 0 && btnUpload.length > 0;

        if (hasHostPermisson) {
            spreadManagementUpload.init(
                function () { $("#formSearch").trigger("submit"); }
            );

            $("#btnAdd").off("click").click(enableRowForAdding);

            btnUpload.click(function () {
                spreadManagementUpload.openModal();
            });

            // Empty the #divPopupContent since it's children was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").remove();
        }
    }

    function buildGroupSiteList(isAllCredit, isAllLicensee, selectedValue) {
        let creditOrLicensee;
        cachedCreditSites = [];
        cachedLicenseeSites = [];
        let selectedValueList = selectedValue.split(",");
        siteList.forEach(function (item) {
            creditOrLicensee = item.IsLicensee ? cachedLicenseeSites : cachedCreditSites;
            let isChecked = selectedValue === "All" || item.IsLicensee && isAllLicensee || !item.IsLicensee && isAllCredit;
            creditOrLicensee.push({
                value: item.Id,
                text: item.Name,
                checked: isChecked || selectedValueList.includes(item.Name.toString()),
                enable: true
            });
        });

        return [
            {
                value: "credit",
                text: "Credit",
                checked: isAllCredit,
                enable: true,
                items: cachedCreditSites
            },
            {
                value: "licensee",
                text: "Licensee",
                checked: isAllLicensee,
                enable: true,
                items: cachedLicenseeSites
            }
        ];
    }


    function renderCustomerClass() {
        let optionList = [];
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: true, enable: true })
        );

        ddlCustomerClasses = customerClassHelper.renderMultiSelect(
            "#ddlCustomerClasses",
            {
                minWidth: 180,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }


    function renderSites() {
        var listData = buildGroupSiteList(true, true, "All");
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


    function renderCurrencies() {
        let optionList = [];
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: true, enable: true })
        );

        ddlCurrencies = customerClassHelper.renderMultiSelect(
            "#ddlCurrencies",
            {
                minWidth: 160,
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
            CMS.resolveRootUrl(`Spread/Search`),
            cachedFilter,
            function (result) {
                cachedData = typeof result === "string" ? JSON.parse(result) : result;
                if (cachedData.Message) {
                    CMS.showErrorMessage(cachedData.Message);
                    cachedAddNewRule = null;
                    renderGrid(new Array());
                } else {
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                }
            },
            null,
            function () {
                CMS.enableInput();
            });
    }

    function renderGrid(data) {

        let html = [
            `<table class="tbl-freegrid" style="height: calc(100vh - 200px);"><thead style="z-index: ${data.length + 2}"><tr>`,
            `<th class="width-180">${resources.title}</th>`,
            `<th class="width-150">${resources.customerClass}</th>`,
            `<th class="width-200">${resources.sportType}</th>`,
            `<th class="width-180">${resources.site}</th>`,
            `<th class="width-150">${resources.currency}</th>`,
            `<th class="width-200">${resources.platform}</th>`,
            `<th class="width-60">${resources.spread}</th>`,
            `<th class="width-60">${resources.enable}</th>`,
            hasHostPermisson ? `<th class="width-60">${resources.action}</th>` : '',
            '</tr></thead > ',
            '<tbody>'
        ];

        if (cachedAddNewRule === null && data.length === 0) {
            let colspan = hasHostPermisson ? 9 : 8;
            html.push(buildRowNoData(colspan, resources.noData));
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

    function buildRowNoData(colspan, textNoData) {
        return `<tr><td colspan="${colspan}" class="text-red align-center">${textNoData}</td></tr>`;
    }

    function buildRowItem(rule, index, zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
        `<td class="rule-cell width-180"><div class="view-item">${CMS.htmlEncode(rule.PolicyTitle)}</div > <div class="dropdown-cell edit-item hidden"><input type="text" class="inputTitle" id="editTitle-${rule.PolicyID}" maxlength="500" /></div></td>`,
        `<td class="rule-cell width-150"><div class="view-item text-center cell-customer-class">${editingGridHelper.renderCustomerClassCellWithLink(rule.CustomerClass, customerClassList)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCustomerClass-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-200"><div class="view-item text-center">${renderSportTypeCell(rule)}</div><div class="dropdown-cell edit-item ddlSportType hidden"><select id="ddlEditSportType-${rule.PolicyID}" class="select-customer-class"></select></div></td>`,
        `<td class="rule-cell width-180"><div class="view-item text-center">${editingGridHelper.renderSiteCell(rule.Sites, siteList, cachedCreditSites, cachedLicenseeSites, "0", "-1", "-2", true)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditSite-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-150"><div class="view-item text-center">${renderCurrencyCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCurrency-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-200"><div class="view-item text-center">${editingGridHelper.renderPlatformCell(rule.Platforms, platforms)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditPlatform-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-60"><div class="view-item text-center">${rule.Spread}</div><input type="text" class="inputSpread edit-item hidden" id="editSpread-${rule.PolicyID}" /></td>`,
        `<td class="rule-cell width-60 text-center">${renderEnableCell(rule.PolicyID, CMS.htmlEncode(rule.PolicyTitle), rule.SportType, rule.Enable)}</td>`,
        hasHostPermisson ? `<td class="width-60 text-center">${renderActionCell(rule.PolicyID, index, rule.edittingValue && rule.edittingValue !== null)}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function buildAddRow(zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
            `<td class="rule-cell width-180 add-cell"><div><input type="text" class="inputTitle" id="addNewTitle" maxlength="500" /></div></td>`,
            `<td class="rule-cell width-150 add-cell"><div><div id="ddlAddNewCustomerClass" class="select-customer-class"></div></div</td>`,
            `<td class="rule-cell width-200 add-cell"><div class="ddlSportType"><select id="ddlAddNewSportType" class="select-customer-class"></select></div></td>`,
            `<td class="rule-cell width-180 add-cell"><div><div id="ddlAddNewSite" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-150 add-cell"><div><div id="ddlAddNewCurrency" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-200 add-cell"><div><div id="ddlAddNewPlatform" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-60 add-cell"><div><input type="text" class="inputSpread" id="addNewSpread" /></div></td>`,
            `<td class="rule-cell width-60 text-center"><input type="checkbox" class="ckbEnable" id="addNewEnable" checked /></td>`,
        hasHostPermisson ? `<td class="width-60 text-center">${renderAddActionCell()}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function renderEnableCell(policyId, policyTitle, sportType, enable) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-sportType="${sportType}" data-policyTitle="${policyTitle.replaceAll('"', '&#34;')}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
    }

    function renderActionCell(policyId, index, isEditting) {
        return [
            `<span class="ico-edit ${isEditting ? "is-editting" : ""} mid-margin-right view-item" title="Edit" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-trash view-item" title="Delete" data-policyId="${policyId}"></span>`,
            `<span class="ico-save-16 mid-margin-right edit-item hidden" title="Save" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-cancel edit-item hidden" title="Cancel" data-policyId="${policyId}" data-index="${index}"></span>`
        ].join("");
    }

    function renderCurrencyCell(rule) {
        if (rule.Currencies === "All" || rule.Currencies === "0") {
            return `<a href="javascript:;" class="link-currency" data-currencies="${currencyList.map(cur => cur.Name).join()}">All</a>`;
        }

        const currencyArray = rule.Currencies.split(",");
        var currencyNames = currencyList.filter(cur => currencyArray.includes(cur.Id.toString())).map(cur => cur.Name);
        return currencyNames.length <= 1
            ? currencyNames.join()
            : `<a href="javascript:;" class="link-currency" data-currencies="${currencyNames.join()}">${currencyNames.length}</a>`;
    }

    function renderSportTypeCell(rule) {
        var sport = sportList.find(st => st.Value === rule.SportType.toString());
        if (sport) {
            return sport.Text;
        }
    }

    function handleViewItems(element, title, dataType) {
        let html = [
            '<div class="form" id="viewCurrenciesForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', title, '</th></tr></thead>',
            '<tbody>'
        ];
        element.data(dataType).split(",").forEach(function (item, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            title,
            html.join(""),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });
    }

    function bindEventsAfterRenderGridSuccessfully() {
        freeGrid.find(".ico-edit")
            .off("click")
            .click(showEditItem);
        freeGrid.find(".ico-trash")
            .off("click")
            .click(showDeleteConfirmPopup);
        freeGrid.find(".link-currency")
            .off("click")
            .click(function () { handleViewItems($(this), resources.currency, "currencies"); });
        freeGrid.find(".link-customerclass")
            .off("click")
            .click(function () { handleViewItems($(this), resources.customerClass, "customerclass"); });
        freeGrid.find(".link-site")
            .off("click")
            .click(function () { handleViewItems($(this), resources.site, "sites"); });
        freeGrid.find(".link-platform")
            .off("click")
            .click(function () { handleViewItems($(this), resources.platform, "platform"); });
        freeGrid.find(".checkbox-enable")
            .off("click")
            .click(showEnableDisablePopup);
        freeGrid.find(".is-editting").trigger('click');
    }

    function enableRowForAdding() {
        clearAllTipsyErrors();
        if ($("#addNewTitle").length > 0) {
            return;
        }

        if (cachedAddNewRule === null) {
            cachedAddNewRule = {
                PolicyTitle: "",
                SportType: "",
                Spread: "",
                CustomerClass: "",
                Currencies: "",
                Sites: "",
                Platforms: "",
                Enable: true
            };
        }

        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
        $(`#addNewTitle`).focus();
    }

    function bindingEventForAddRow() {
        $(`#addNewTitle`).val(cachedAddNewRule.PolicyTitle).change(function () { cachedAddNewRule.PolicyTitle = $(`#addNewTitle`).val(); });
        editingGridHelper.renderAddEditCustomerClass("ddlAddNewCustomerClass", cachedAddNewRule.CustomerClass, function () { onCustomerClassChange("ddlAddNewCustomerClass"); });
        editingGridHelper.renderCombobox("ddlAddNewSportType", sportList, cachedAddNewRule.SportType);
        editingGridHelper.renderAddEditSite("ddlAddNewSite", cachedAddNewRule.Sites, cachedCreditSites, cachedLicenseeSites, cachedAddNewRule.Sites === "0", cachedAddNewRule.Sites.includes("-1"), cachedAddNewRule.Sites.includes("-2"), function () { onSiteChange("ddlAddNewSite"); });
        editingGridHelper.renderAddEditCurrency("ddlAddNewCurrency", cachedAddNewRule.Currencies, function () { onCurrencyChange("ddlAddNewCurrency"); });
        editingGridHelper.renderAddEditPlatform("ddlAddNewPlatform", cachedAddNewRule.Platforms, platforms, function () { onPlatformChange("ddlAddNewPlatform"); });
        var newSpread = new AutoNumeric("#addNewSpread", {
            minimumValue: 0,
            maximumValue: 0.5,
            decimalPlaces: 2,
            onInvalidPaste: "ignore"
        });
        $(`#addNewSpread`).change(function () { cachedAddNewRule.Spread = newSpread.get(); });
        $(`#addNewEnable`).change(function () { cachedAddNewRule.Enable = $(`#addNewEnable`).is(":checked"); });


        $("#btn-cancel-add").click(cancelAddNew);
        $("#btn-submit-add").click(submitAddNew);
    }

    function onCustomerClassChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
        } else {
            cachedAddNewRule.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
        }
    }

    function onSiteChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        } else {
            cachedAddNewRule.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        }
    }

    function onCurrencyChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Currencies = getSelectedReplaceAllValue(divId, "0", currencyList.length);
        } else {
            cachedAddNewRule.Currencies = getSelectedReplaceAllValue(divId, "0", currencyList.length);
        }
    }

    function onPlatformChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Platforms = getSelectedReplaceAllValue(divId, "0", platforms.length);
        } else {
            cachedAddNewRule.Platforms = getSelectedReplaceAllValue(divId, "0", platforms.length);
        }
    }

    function renderAddActionCell(ruleId) {
        return [
            '<span class="ico-save-16 mid-margin-right" title="Save" id="btn-submit-add"></span>',
            '<span class="ico-cancel" title="Cancel" id="btn-cancel-add"></span>'
        ].join("");
    }

    function cancelAddNew() {
        clearAllTipsyErrors();
        if ($("#addNewTitle").length <= 0) {
            return;
        }
        cachedAddNewRule = null;
        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
    }

    function submitAddNew() {
        clearAllTipsyErrors();
        if (!cachedAddNewRule) {
            return;
        }
        cachedAddNewRule.SportType = $(`#ddlAddNewSportType`).val();
        if (cachedAddNewRule.SportType === "Please select") {
            cachedAddNewRule.SportType = "";
        }
        var newRuleTitle = spreadManagementHelper.validateTitle($(`#addNewTitle`));
        spreadManagementHelper.validateRequiredField($(`#addNewSpread`), cachedAddNewRule.Spread);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCustomerClass`), cachedAddNewRule.CustomerClass);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCurrency`), cachedAddNewRule.Currencies);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewSite`), cachedAddNewRule.Sites);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewPlatform`), cachedAddNewRule.Platforms);
        spreadManagementHelper.validateRequiredField($(`span#ddlAddNewSportType`), cachedAddNewRule.SportType);
        if (spreadManagementHelper.isEmptyValue(newRuleTitle) || !spreadManagementHelper.isValidRule(cachedAddNewRule)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Add`),
            cachedAddNewRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    cachedAddNewRule.PolicyID = resultJson.PolicyID;
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

    function getSelectedReplaceAllValue(divId, allvalue, allItemsLength) {
        var selectedItems = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedItems.length === allItemsLength) {
            return allvalue;
        }

        return selectedItems.join();
    }

    function cancelEditItem(div) {
        clearAllTipsyErrors();
        var rowEdit = div.closest('tr');
        var index = div.data("index");
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

    function showEditItem() {
        clearAllTipsyErrors();
        var rowEdit = $(this).closest('tr');
        var policyId = $(this).data("policyid");
        var index = $(this).data("index");
        var editPolicy = cachedData[index].edittingValue || cachedData[index];
        cachedData[index].edittingValue = { ...cachedData[index] };
        $(`#editTitle-${policyId}`).val(editPolicy.PolicyTitle).change(function () { cachedData[index].edittingValue.PolicyTitle = $(`#editTitle-${policyId}`).val(); });
        rowEdit.find("div[class*=view-item]").addClass("hidden");
        rowEdit.find("div[class*=edit-item]").removeClass("hidden");
        rowEdit.find("input[class*=edit-item]").removeClass("hidden");
        rowEdit.find("span[class*=view-item]").addClass("hidden");
        rowEdit.find("span[class*=edit-item]").removeClass("hidden");
        editingGridHelper.renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass, function () { onCustomerClassChange(`ddlEditCustomerClass-${policyId}`, index); });
        editingGridHelper.renderCombobox(`ddlEditSportType-${policyId}`, sportList, editPolicy.SportType);
        editingGridHelper.renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites, cachedCreditSites, cachedLicenseeSites, editPolicy.Sites === "0", editPolicy.Sites.includes("-1"), editPolicy.Sites.includes("-2"), function () { onSiteChange(`ddlEditSite-${policyId}`, index); });
        editingGridHelper.renderAddEditCurrency(`ddlEditCurrency-${policyId}`, editPolicy.Currencies, function () { onCurrencyChange(`ddlEditCurrency-${policyId}`, index); });
        editingGridHelper.renderAddEditPlatform(`ddlEditPlatform-${policyId}`, editPolicy.Platforms, platforms, function () { onPlatformChange(`ddlEditPlatform-${policyId}`, index); });
        var spreadInput = new AutoNumeric(`#editSpread-${policyId}`, {
            minimumValue: 0,
            maximumValue: 0.5,
            decimalPlaces: 2,
            onInvalidPaste: 'ignore'
        }).set(editPolicy.Spread);

        $(`#editSpread-${policyId}`).change(function () { cachedData[index].edittingValue.Spread = spreadInput.get(); });
        $(`#ckbEdit-${policyId}`).change(function () { cachedData[index].edittingValue.Enable = $(`#ckbEdit-${policyId}`).is(":checked"); });


        rowEdit.find("span[class*=ico-save-16]")
            .off("click")
            .click(saveEditItem);
        rowEdit.find("span[class*=ico-cancel]")
            .off("click")
            .click(function () { spreadInput.remove(); cancelEditItem($(this)) });
        $(`#editTitle-${policyId}`).focus();
    }

    function saveEditItem() {
        clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        var sportType = $(`#ddlEditSportType-${policyId}`).val();
        if (sportType === "Please select") {
            sportType = "";
        }
        var updateRule = {
            PolicyID: policyId,
            PolicyTitle: spreadManagementHelper.validateTitle($(`#editTitle-${policyId}`)),
            Spread: spreadManagementHelper.validateRequiredField($(`#editSpread-${policyId}`), $(`#editSpread-${policyId}`).val()),
            SportType: spreadManagementHelper.validateRequiredField($(`span#ddlEditSportType-${policyId}`), sportType),
            CustomerClass: spreadManagementHelper.validateRequiredField($(`#ddlEditCustomerClass-${policyId}`), $(`#ddlEditCustomerClass-${policyId}`).fanexMultiSelect("getItems", true).join()),
            Currencies: spreadManagementHelper.validateRequiredField($(`#ddlEditCurrency-${policyId}`), getSelectedReplaceAllValue(`ddlEditCurrency-${policyId}`, "0", currencyList.length)),
            Sites: spreadManagementHelper.validateRequiredField($(`#ddlEditSite-${policyId}`), editingGridHelper.getSelectedSites(`ddlEditSite-${policyId}`, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2")),
            Platforms: spreadManagementHelper.validateRequiredField($(`#ddlEditPlatform-${policyId}`), getSelectedReplaceAllValue(`ddlEditPlatform-${policyId}`, "0", platforms.length)),
            Enable: $(`#ckbEdit-${policyId}`).is(":checked")
        };

        if (!spreadManagementHelper.isValidRule(updateRule)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Update`),
            updateRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    cachedData[index] = updateRule;
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function showDeleteConfirmPopup() {
        clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this title "${CMS.htmlEncode(cachedData[index].PolicyTitle)}"?`, function () { callDelete(policyId, cachedData[index].SportType, index); });
    }

    function callDelete(policyID, sportType, index) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Delete?policyID=${policyID}&sportType=${sportType}`),
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
        clearAllTipsyErrors();
        var checkbox = $(this);
        var policyId = checkbox.data("policyid");
        var isEnable = checkbox.is(":checked");
        var action = isEnable ? "ENABLE" : "DISABLE";
        var policyTitle = checkbox.data("policytitle");
        var sportType = checkbox.data("sporttype");
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">${action}</span> this title "${CMS.htmlEncode(policyTitle)}"?`, function () { callEnableDisable(policyId, sportType, isEnable, checkbox); });
    }

    function callEnableDisable(policyID, sportType, isEnable, checkbox) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Enable?policyID=${policyID}&sportType=${sportType}&enable=${isEnable}`),
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

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    $(function () {
        init();
    });
})(jQuery);