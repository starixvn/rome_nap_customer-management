
(function ($) {
    let cachedGridOptions = null,
        cachedData = null,
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
            decimalPlaces: 1
        });
        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClass: ddlCustomerClasses.fanexMultiSelect("getItems", true).join(),
                site: ddlSites.fanexMultiSelect("getItems", true).join(),
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
                checked: isChecked || selectedValueList.includes(item.Id.toString()),
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
                listData: optionList
            }
        );
    }


    function renderSites() {
        var listData = buildGroupSiteList(true, true, "All");
        ddlSites = customerClassHelper.renderMultiSelect(
            "#ddlSites",
            {
                minWidth: 180,
                listData: listData,
                isCheckAll: true
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
                listData: optionList
            }
        );
    }

    function search() {
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Search`),
            cachedFilter,
            function (result) {
                renderGrid(result);
                bindEventsAfterRenderGridSuccessfully();
            });
    }

    function renderGrid(data) {
        cachedGridOptions = cachedGridOptions || defaultGridOptions();

        const dataJson = typeof data === "string" ? JSON.parse(data) : data;
        cachedGridOptions.bodyRows = buildBodyRows(dataJson);
        freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
        if (cachedAddNewRule !== null) {
            enableRowForAdding();
        }
    }
    
    function defaultGridOptions() {
        const columns = [
            { HeaderText: resources.title, Width: 150, Name: "colTitle", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.customerClass, Width: 250, Name: "colCustomerClass", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.site, Width: 180, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.currency, Width: 150, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.platform, Width: 250, Name: "colPlatform", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.spread, Width: 60, Name: "colSpread", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.enable, Width: 60, Name: "colEnable", HeaderAlign: "center", CellAlign: "center" }
        ];
        hasHostPermisson && columns.push({ HeaderText: resources.action, Width: 60, Name: "colAction", HeaderAlign: "center", CellAlign: "center" });

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 55,
            bodyRowHeight: 26,
            gridExpandHeight: 160,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(data) {
        cachedData = data;
        return data.map(function (item, index) {
           return renderRuleCell(item, index, data.length - index);
        });
    }

    function renderRuleCell(rule, index, zindex) {
        const columns = [
            { Name: "colTitle", Value: `<div class="view-item">${rule.PolicyTitle}</div><input type="text" class="inputTitle edit-item hidden" id="editTitle-${rule.PolicyID}" />` },
            { Name: "colCustomerClass", Value: `<div class="view-item cell-customer-class">${renderCustomerClassCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCustomerClass-${rule.PolicyID}" class="select-customer-class"></div></div>` },
            { Name: "colSite", Value: `<div class="view-item">${renderSiteCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditSite-${rule.PolicyID}" class="select-customer-class"></div></div>` },
            { Name: "colCurrency", Value: `<div class="view-item">${renderCurrencyCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCurrency-${rule.PolicyID}" class="select-customer-class"></div></div>` },
            { Name: "colPlatform", Value: `<div class="view-item">${renderPlatformCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditPlatform-${rule.PolicyID}" class="select-customer-class"></div></div>` },
            { Name: "colSpread", Value: `<div class="view-item">${rule.Spread}</div><input type="text" class="inputSpread edit-item hidden" id="editSpread-${rule.PolicyID}" />` },
            { Name: "colEnable", Value: renderEnableCell(rule.PolicyID, rule.PolicyTitle, rule.Enable) }
        ];

        hasHostPermisson && columns.push({ Name: "colAction", Value: renderActionCell(rule.PolicyID, index, rule.edittingValue && rule.edittingValue != null) });

        return { Columns: columns, CustomAttr: `style="z-index: ${zindex}"`, CssClass: "position-relative" };
    }

    function renderEnableCell(policyId, policyTitle, enable) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-policyTitle="${policyTitle}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
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
        if (rule.Currencies === "All" ) {
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
        if (!cachedGridOptions || $("#addNewTitle").length > 0) {
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
                Enable: false
            };
        }

        cachedGridOptions.bodyRows.unshift(renderAddingRow(cachedGridOptions.bodyRows.length + 1));
        freeGrid.fanexGrid(cachedGridOptions);

        $(`#addNewTitle`).val(cachedAddNewRule.PolicyTitle).change(function () { cachedAddNewRule.PolicyTitle = $(`#addNewTitle`).val(); });
        renderAddEditCustomerClass("ddlAddNewCustomerClass", cachedAddNewRule.CustomerClass);
        renderAddEditSite("ddlAddNewSite", cachedAddNewRule.Sites);
        renderAddEditCurrency("ddlAddNewCurrency", cachedAddNewRule.Currencies);
        renderAddEditPlatform("ddlAddNewPlatform", cachedAddNewRule.Platforms);
        $(`#addNewSpread`).change(function () { cachedAddNewRule.Spread = $(`#addNewSpread`).val(); });
        $(`#addNewEnable`).change(function () { cachedAddNewRule.Enable = $(`#addNewEnable`).is(":checked"); });
        

        $("#btn-cancel-add").click(cancelAddNew);
        $("#btn-submit-add").click(submitAddNew);
        bindEventsAfterRenderGridSuccessfully();
    }

    function renderAddingRow(zindex){
        return {
            Columns: [
                { Name: "colTitle", Value: '<input type="text" class="inputTitle" id="addNewTitle" />' },
                { Name: "colCustomerClass", Value: '<div class="dropdown-cell"><div id="ddlAddNewCustomerClass" class="select-customer-class"></div></div>' },
                { Name: "colSite", Value: '<div class="dropdown-cell"><div id="ddlAddNewSite" class="select-customer-class"></div></div>' },
                { Name: "colCurrency", Value: '<div class="dropdown-cell"><div id="ddlAddNewCurrency" class="select-customer-class"></div></div>' },
                { Name: "colPlatform", Value: '<div class="dropdown-cell"><div id="ddlAddNewPlatform" class="select-customer-class"></div></div>' },
                { Name: "colSpread", Value: '<input type="text" class="inputSpread" id="addNewSpread" />' },
                { Name: "colEnable", Value: '<input type="checkbox" class="ckbEnable" id="addNewEnable" />' },
                { Name: "colAction", Value: renderAddActionCell() }
            ],
            CssClass: "position-relative",
            CustomAttr: `style="z-index: ${zindex}"`
        };
    }

    function renderAddEditCustomerClass(divId, selectedValue, index) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: selectedValueList.includes(item.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 248,
                listData: optionList,
                isCheckAll: false,
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
        var listData = buildGroupSiteList(selectedValue.includes("AllCredit"), selectedValue.includes("AllDeposit"), selectedValue);
        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 178,
                listData: listData,
                isCheckAll: false,
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
        let selectedValueList = selectedValue.split(",");
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 148,
                listData: optionList,
                isCheckAll: false,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.Currencies = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
                    } else {
                        cachedAddNewRule.Currencies = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
                    }
                }
            }
        );
    }

    function renderAddEditPlatform(divId, selectedValue, index) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        platforms.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 248,
                listData: optionList,
                isCheckAll: false,
                onCheck: function () {
                    if (index) {
                        cachedData[index].edittingValue.Platforms = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
                    } else {
                        cachedAddNewRule.Platforms = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
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
        if (!cachedGridOptions || cachedGridOptions.bodyRows.length <= 0) {
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

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Add`),
            cachedAddNewRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    cachedAddNewRule.PolicyID = result.PolicyID;
                    CMS.showSuccessMessage(resultJson.Message);
                    showAddedItem();
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

    function showAddedItem() {
        cachedData.unshift(cachedAddNewRule);
        cachedAddNewRule = null;
        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
    }

    function cancelEditItem() {
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
        renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass, index);
        renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites, index);
        renderAddEditCurrency(`ddlEditCurrency-${policyId}`, editPolicy.Currencies, index);
        renderAddEditPlatform(`ddlEditPlatform-${policyId}`, editPolicy.Platforms, index);
        new AutoNumeric(`#editSpread-${policyId}`, {
            minimumValue: 0,
            maximumValue: 20,
            decimalPlaces: 1
        }).set(editPolicy.Spread);
        
        $(`#editSpread-${policyId}`).change(function () { cachedData[index].edittingValue.Spread = $(`#editSpread-${policyId}`).val(); });
        $(`#ckbEdit-${policyId}`).change(function () { cachedData[index].edittingValue.Enable = $(`#ckbEdit-${policyId}`).is(":checked"); });


        rowEdit.find("span[class*=ico-save-16]")
            .off("click")
            .click(saveEditItem);
        rowEdit.find("span[class*=ico-cancel]")
            .off("click")
            .click(cancelEditItem);
    }

    function saveEditItem() {
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        var updateRule = {
            PolicyID: policyId,
            PolicyTitle: $(`#editTitle-${policyId}`).val(),
            Spread: $(`#editSpread-${policyId}`).val(),
            CustomerClass: $(`#ddlEditCustomerClass-${policyId}`).fanexMultiSelect("getItems", true).join(),
            Currencies: $(`#ddlEditCurrency-${policyId}`).fanexMultiSelect("getItems", true).join(),
            Sites: getSelectedSites(`ddlEditSite-${policyId}`),
            Platforms: $(`#ddlEditPlatform-${policyId}`).fanexMultiSelect("getItems", true).join(),
            Enable: $(`#ckbEdit-${policyId}`).is(":checked")
        };

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
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this title "${cachedData[index].PolicyTitle}"?`, function () { callDelete(policyId, index); });
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
        var checkbox = $(this);
        var policyId = checkbox.data("policyid");
        var isEnable = checkbox.is(":checked");
        var action = isEnable ? "ENABLE" : "DISABLE";
        var policyTitle = checkbox.data("policytitle");
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">${action}</span> this title "${policyTitle}"?`, function () { callEnableDisable(policyId, isEnable, checkbox); });
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

    $(function () {
        init();
    });
})(jQuery);