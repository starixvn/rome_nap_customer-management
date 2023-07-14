
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
        hasHostPermisson = null;

    const platforms = [{ Id: 1, Name: "Desktop" }, { Id: 2, Name: "Mobile" }, { Id: 3, Name: "APP" }, { Id: 4, Name: "Oddsfeed" }, { Id: 5, Name: "DirectAPI" }];

    function init() {
        renderCustomerClass();
        renderSites();
        renderCurrencies();
        filterSpread = new AutoNumeric("#filterSpread", {
            minimumValue: 0,
            maximumValue: 20,
            decimalPlaces: 1,
            onInvalidPaste: 'ignore'
        });
        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClass: ddlCustomerClasses.fanexMultiSelect("getItems", true).join(),
                site: getSelectedSites("ddlSites"),
                currency: ddlCurrencies.fanexMultiSelect("getItems", true).join(),
                spread: filterSpread.get()
            };
            cachedAddNewRule = null;
            cachedFilter = formData;
            search();
        }).trigger("submit");

        hasHostPermisson = $("#btnAdd").length > 0;

        hasHostPermisson && $("#btnAdd").off("click").click(enableRowForAdding);
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
                value: item.Name,
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
                minWidth: 140,
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
                minWidth: 140,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }

    function search() {
        cachedData = [];
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Search`),
            cachedFilter,
            function (result) {
                cachedData = typeof result === "string" ? JSON.parse(result) : result;
            },
            null,
            function () {
                renderGrid(cachedData);
                bindEventsAfterRenderGridSuccessfully();
            });
    }

    function renderGrid(data) {

        let html = [
            `<table class="tbl-freegrid" style="height: calc(100vh - 150px);"><thead style="z-index: ${data.length + 2}"><tr>`,
            `<th class="width-150">${resources.title}</th>`,
            `<th class="width-250">${resources.customerClass}</th>`,
            `<th class="width-180">${resources.site}</th>`,
            `<th class="width-150">${resources.currency}</th>`,
            `<th class="width-200">${resources.platform}</th>`,
            `<th class="width-60">${resources.spread}</th>`,
            `<th class="width-60">${resources.enable}</th>`,
            hasHostPermisson ? `<th class="width-60">${resources.action}</th>` : '',
            '</tr></thead > ',
            '<tbody>'
        ];

        if (cachedAddNewRule !== null) {
            html.push(buildAddRow(data.length + 1));
        }

        data.forEach(function (item, index) {
            html.push(buildRowItem(item, index, data.length - index));
        });
        html.push('</tbody></table>');

        freeGrid.removeClass("display-none").html(html.join(""));

        cachedAddNewRule !== null && bindingEventForAddRow();
    }

    function buildRowItem(rule, index, zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
            `<td class="rule-cell width-150"><div class="view-item">${CMS.htmlEncode(rule.PolicyTitle)}</div > <div class="dropdown-cell edit-item hidden"><input type="text" class="inputTitle" id="editTitle-${rule.PolicyID}" maxlength="500" /></div></td>`,
            `<td class="rule-cell width-250"><div class="view-item text-center cell-customer-class">${renderCustomerClassCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCustomerClass-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-180"><div class="view-item text-center">${renderSiteCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditSite-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-150"><div class="view-item text-center">${renderCurrencyCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCurrency-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-200"><div class="view-item text-center">${renderPlatformCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditPlatform-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-60"><div class="view-item text-center">${rule.Spread}</div><input type="text" class="inputSpread edit-item hidden" id="editSpread-${rule.PolicyID}" /></td>`,
            `<td class="rule-cell width-60 text-center">${renderEnableCell(rule.PolicyID, CMS.htmlEncode(rule.PolicyTitle), rule.Enable)}</td>`,
            hasHostPermisson ? `<td class="width-60 text-center">${renderActionCell(rule.PolicyID, index, rule.edittingValue && rule.edittingValue !== null)}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function buildAddRow(zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
        `<td class="rule-cell width-150 add-cell"><div><input type="text" class="inputTitle" id="addNewTitle" maxlength="500" /></div></td>`,
        `<td class="rule-cell width-250 add-cell"><div><div id="ddlAddNewCustomerClass" class="select-customer-class"></div></div</td>`,
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

    function renderEnableCell(policyId, policyTitle, enable) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-policyTitle="${policyTitle.replaceAll('"', '&#34;')}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
    }

    function renderActionCell(policyId, index, isEditting) {
        return [
            `<span class="ico-edit ${ isEditting ? "is-editting" : "" } mid-margin-right view-item" title="Edit" data-policyId="${policyId}" data-index="${index}" ></span>`,
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

    function renderCustomerClassCell(rule) {
        const customerClassList = rule.CustomerClass.split(",");
        return customerClassList.join(", ");
    }

    function renderSiteCell(rule) {
        if (rule.Sites === "All") {
            return `<a href="javascript:;" class="link-site" data-sites="${siteList.map(st=> st.Name).join()}">All</a>`;
        }

        let sites = rule.Sites;
        if (rule.Sites.includes("AllCredit")) {
            sites = sites.replace("AllCredit", cachedCreditSites.map(st => st.value).join());
        }

        if (rule.Sites.includes("AllDeposit")) {
            sites = sites.replace("AllDeposit", cachedLicenseeSites.map(st => st.value).join());
        }

        const sitesNameList = sites.split(",");
        var displayName = rule.Sites !== 'AllCredit' && rule.Sites !== 'AllDeposit' ? sitesNameList.length : rule.Sites;
        return sitesNameList.length <= 1
            ? sites
            : `<a href="javascript:;" class="link-site" data-sites="${sitesNameList.join()}">${displayName}</a>`;
    }

    function renderPlatformCell(rule) {

        if (rule.Platforms === "All" || rule.Platforms === "0") {
            return platforms.map(plf => plf.Name).join(', ');
        }

        const platformList = rule.Platforms.split(",");

        return platforms.filter(plf => platformList.includes(plf.Id.toString())).map(plf => plf.Name).join(', ');
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
            .click(function () { handleViewItems($(this), resources.customerClass, "customerclasses"); });
        freeGrid.find(".link-site")
            .off("click")
            .click(function () { handleViewItems($(this), resources.site, "sites"); });
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
        $(`#addNewTitle`).val(CMS.htmlEncode(cachedAddNewRule.PolicyTitle)).change(function () { cachedAddNewRule.PolicyTitle = $(`#addNewTitle`).val(); });
        renderAddEditCustomerClass("ddlAddNewCustomerClass", cachedAddNewRule.CustomerClass);
        renderAddEditSite("ddlAddNewSite", cachedAddNewRule.Sites);
        renderAddEditCurrency("ddlAddNewCurrency", cachedAddNewRule.Currencies);
        renderAddEditPlatform("ddlAddNewPlatform", cachedAddNewRule.Platforms);
        var newSpread = new AutoNumeric("#addNewSpread", {
            minimumValue: 0,
            maximumValue: 20,
            decimalPlaces: 1,
            onInvalidPaste: "ignore"
        });
        $(`#addNewSpread`).change(function () { cachedAddNewRule.Spread = newSpread.get(); });
        $(`#addNewEnable`).change(function () { cachedAddNewRule.Enable = $(`#addNewEnable`).is(":checked"); });


        $("#btn-cancel-add").click(cancelAddNew);
        $("#btn-submit-add").click(submitAddNew);
    }

    function renderAddEditCustomerClass(divId, selectedValue, index) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: selectedValueList.includes(item.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 246,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: false,
                showFilter: true,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
                    } else {
                        cachedAddNewRule.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
                    }
                }
            }
        );
    }

    function renderAddEditSite(divId, selectedValue, index) {
        var isCheckAll = selectedValue === "All";
        var listData = buildGroupSiteList(isCheckAll || selectedValue.includes("AllCredit"), isCheckAll || selectedValue.includes("AllDeposit"), selectedValue);
        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 176,
                maxHeight: 230,
                listData: listData,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.Sites = getSelectedSites(divId);
                    } else {
                        cachedAddNewRule.Sites = getSelectedSites(divId);
                    }
                }
            }
        );
    }

    function renderAddEditCurrency(divId, selectedValue, index) {
        let optionList = [];
        let isCheckAll = selectedValue === "All" || selectedValue === "0";
        let selectedValueList = selectedValue.split(",");
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: isCheckAll || selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 146,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.Currencies = getSelectedReplaceAllValue(divId, "0");
                    } else {
                        cachedAddNewRule.Currencies = getSelectedReplaceAllValue(divId, "0");
                    }
                }
            }
        );
    }

    function renderAddEditPlatform(divId, selectedValue, index) {
        let optionList = [];
        let isCheckAll = selectedValue === "All" || selectedValue === "0";
        let selectedValueList = selectedValue.split(",");
        platforms.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: isCheckAll || selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 198,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.Platforms = getSelectedReplaceAllValue(divId, "0");
                    } else {
                        cachedAddNewRule.Platforms = getSelectedReplaceAllValue(divId, "0");
                    }
                }
            }
        );
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
        if (!cachedAddNewRule) {
            return;
        }

        spreadManagementHelper.validateRequiredField($(`#addNewTitle`), cachedAddNewRule.PolicyTitle);
        spreadManagementHelper.validateRequiredField($(`#addNewSpread`), cachedAddNewRule.Spread);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCustomerClass`), cachedAddNewRule.CustomerClass);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCurrency`), cachedAddNewRule.Currencies);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewSite`), cachedAddNewRule.Sites);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewPlatform`), cachedAddNewRule.Platforms);
        if (!spreadManagementHelper.isValidRule(cachedAddNewRule)) {
            return;
        }

        clearAllTipsyErrors();
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

    function getSelectedSites(divId) {
        if ($(`#${divId}`).fanexMultiSelect("isCheckAll")) {
            return "All";
        }

        var selectedSites = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if ($(`#${divId}`).parent().find('input[data-is-group="true"][value="licensee"]').is(":checked")) {
            selectedSites = selectedSites.filter(site => cachedLicenseeSites.filter(licenseeSite => licenseeSite.value.toString() === site.toString()).length <= 0);
            selectedSites.unshift("AllDeposit");
        }

        if ($(`#${divId}`).parent().find('input[data-is-group="true"][value="credit"]').is(":checked")) {
            selectedSites = selectedSites.filter(site => cachedCreditSites.filter(creditSite => creditSite.value.toString() === site.toString()).length <= 0);
            selectedSites.unshift("AllCredit");
        }

        return selectedSites.join();
    }

    function getSelectedReplaceAllValue(divId, allvalue) {
        if ($(`#${divId}`).fanexMultiSelect("isCheckAll")) {
            return allvalue;
        }

        return $(`#${divId}`).fanexMultiSelect("getItems", true).join();
    }

    function cancelEditItem() {
        clearAllTipsyErrors();
        var rowEdit = $(this).closest('tr');
        var index = $(this).data("index");
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
        $(`#editTitle-${policyId}`).val(CMS.htmlEncode(editPolicy.PolicyTitle)).change(function () { cachedData[index].edittingValue.PolicyTitle = $(`#editTitle-${policyId}`).val(); });
        rowEdit.find("div[class*=view-item]").addClass("hidden");
        rowEdit.find("div[class*=edit-item]").removeClass("hidden");
        rowEdit.find("input[class*=edit-item]").removeClass("hidden");
        rowEdit.find("span[class*=view-item]").addClass("hidden");
        rowEdit.find("span[class*=edit-item]").removeClass("hidden");
        renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass, index);
        renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites, index);
        renderAddEditCurrency(`ddlEditCurrency-${policyId}`, editPolicy.Currencies, index);
        renderAddEditPlatform(`ddlEditPlatform-${policyId}`, editPolicy.Platforms, index);
        new AutoNumeric(`#editSpread-${policyId}`, {
            minimumValue: 0,
            maximumValue: 20,
            decimalPlaces: 1,
            onInvalidPaste: 'ignore'
        }).set(editPolicy.Spread);
        
        $(`#editSpread-${policyId}`).change(function () { cachedData[index].edittingValue.Spread = $(`#editSpread-${policyId}`).val(); });
        $(`#ckbEdit-${policyId}`).change(function () { cachedData[index].edittingValue.Enable = $(`#ckbEdit-${policyId}`).is(":checked"); });


        rowEdit.find("span[class*=ico-save-16]")
            .off("click")
            .click(saveEditItem);
        rowEdit.find("span[class*=ico-cancel]")
            .off("click")
            .click(cancelEditItem);
        $(`#editTitle-${policyId}`).focus();
    }

    function saveEditItem() {
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        var updateRule = {
            PolicyID: policyId,
            PolicyTitle: spreadManagementHelper.validateRequiredField($(`#editTitle-${policyId}`), $(`#editTitle-${policyId}`).val()),
            Spread: spreadManagementHelper.validateRequiredField($(`#editSpread-${policyId}`),$(`#editSpread-${policyId}`).val()),
            CustomerClass: spreadManagementHelper.validateRequiredField($(`#ddlEditCustomerClass-${policyId}`), $(`#ddlEditCustomerClass-${policyId}`).fanexMultiSelect("getItems", true).join()),
            Currencies: spreadManagementHelper.validateRequiredField($(`#ddlEditCurrency-${policyId}`), getSelectedReplaceAllValue(`ddlEditCurrency-${policyId}`, "0")),
            Sites: spreadManagementHelper.validateRequiredField($(`#ddlEditSite-${policyId}`), getSelectedSites(`ddlEditSite-${policyId}`)),
            Platforms: spreadManagementHelper.validateRequiredField($(`#ddlEditPlatform-${policyId}`), getSelectedReplaceAllValue(`ddlEditPlatform-${policyId}`, "0")),
            Enable: $(`#ckbEdit-${policyId}`).is(":checked")
        };

        if (!spreadManagementHelper.isValidRule(updateRule)) {
            return;
        }

        clearAllTipsyErrors();
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
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this title "${CMS.htmlEncode(cachedData[index].PolicyTitle)}"?`, function () { callDelete(policyId, index); });
    }

    function callDelete(policyID, index) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Delete?policyID=${policyID}`),
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
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">${action}</span> this title "${CMS.htmlEncode(policyTitle)}"?`, function () { callEnableDisable(policyId, isEnable, checkbox); });
    }

    function callEnableDisable(policyID, isEnable, checkbox) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Enable?policyID=${policyID}&enable=${isEnable}`),
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