
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
        cachedDomAddNewRule = null,
        errorDiv = $("#errorDiv"),
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
        cachedDomAddNewRule = null;
        cachedGridOptions = cachedGridOptions || defaultGridOptions();

        const dataJson = typeof data === "string" ? JSON.parse(data) : data;
        cachedGridOptions.bodyRows = buildBodyRows(dataJson);
        freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
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

        hasHostPermisson && columns.push({ Name: "colAction", Value: renderActionCell(rule.PolicyID, index) });

        return { Columns: columns, CustomAttr: `style="z-index: ${zindex}"`, CssClass: "position-relative" };
    }

    function renderEnableCell(policyId, policyTitle, enable) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-policyTitle="${policyTitle}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
    }

    function renderActionCell(policyId, index) {
        return [
            `<span class="ico-edit mid-margin-right view-item" title="Edit" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-trash view-item" title="Delete" data-policyId="${policyId}"></span>`,
            `<span class="ico-save-16 mid-margin-right edit-item hidden" title="Save" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-cancel edit-item hidden" title="Cancel" data-policyId="${policyId}"></span>`
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
    }

    function enableRowForAdding() {
        if (!cachedGridOptions || $("#addNewTitle").length > 0 || cachedDomAddNewRule) {
            return;
        }
        cachedGridOptions.bodyRows.unshift(renderAddingRow(cachedGridOptions.bodyRows.length + 1));
        freeGrid.fanexGrid(cachedGridOptions);
        cachedDomAddNewRule = {
            title: $("#addNewTitle"),
            customerClass: renderAddEditCustomerClass("ddlAddNewCustomerClass", ""),
            site: renderAddEditSite("ddlAddNewSite", ""),
            currency: renderAddEditCurrency("ddlAddNewCurrency", ""),
            platform: renderAddEditPlatform("ddlAddNewPlatform", ""),
            spread: new AutoNumeric("#addNewSpread", {
                minimumValue: 0,
                maximumValue: 20,
                decimalPlaces: 1}),
            enable: $("#addNewEnable")
        };
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

    function renderAddEditCustomerClass(divId, selectedValue) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: selectedValueList.includes(item.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 248,
                listData: optionList,
                isCheckAll: false
            }
        );
    }

    function renderAddEditSite(divId, selectedValue) {
        var listData = buildGroupSiteList(selectedValue.includes("AllCredit"), selectedValue.includes("AllDeposit"), selectedValue);
        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 178,
                listData: listData,
                isCheckAll: false
            }
        );
    }

    function renderAddEditCurrency(divId, selectedValue) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 148,
                listData: optionList,
                isCheckAll: false
            }
        );
    }

    function renderAddEditPlatform(divId, selectedValue) {
        let optionList = [];
        let selectedValueList = selectedValue.split(",");
        platforms.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 248,
                listData: optionList,
                isCheckAll: false
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
        cachedDomAddNewRule = null;
        cachedGridOptions.bodyRows.shift();
        freeGrid.fanexGrid(cachedGridOptions);
        bindEventsAfterRenderGridSuccessfully();
    }

    function submitAddNew() {
        if (!cachedDomAddNewRule) {
            return;
        }

        var newRule = {
            PolicyTitle: cachedDomAddNewRule.title.val(),
            Spread: cachedDomAddNewRule.spread.get(),
            CustomerClass: cachedDomAddNewRule.customerClass.fanexMultiSelect("getItems", true).join(),
            Currencies: cachedDomAddNewRule.currency.fanexMultiSelect("getItems", true).join(),
            Sites: getSelectedSites("ddlAddNewSite"),
            Platforms: cachedDomAddNewRule.platform.fanexMultiSelect("getItems", true).join(),
            Enable: cachedDomAddNewRule.enable.is(":checked")
        };

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Spread/Add`),
            newRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    newRule.PolicyID = result.PolicyID;
                    CMS.showSuccessMessage(resultJson.Message);
                    showAddedItem(newRule);
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

    function showAddedItem(newRule) {
        cachedDomAddNewRule = null;
        cachedGridOptions.bodyRows.shift();
        cachedGridOptions.bodyRows.unshift(renderRuleCell(newRule));
        freeGrid.fanexGrid(cachedGridOptions);
    }

    function cancelEditItem() {
        var rowEdit = $(this).closest('tr');
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
        var editPolicy = cachedData[index];
        $(`#editTitle-${policyId}`).val(editPolicy.PolicyTitle);
        rowEdit.find("div[class*=view-item]").addClass("hidden");
        rowEdit.find("div[class*=edit-item]").removeClass("hidden");
        rowEdit.find("input[class*=edit-item]").removeClass("hidden");
        rowEdit.find("span[class*=view-item]").addClass("hidden");
        rowEdit.find("span[class*=edit-item]").removeClass("hidden");
        renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass);
        renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites);
        renderAddEditCurrency(`ddlEditCurrency-${policyId}`, editPolicy.Currencies);
        renderAddEditPlatform(`ddlEditPlatform-${policyId}`, editPolicy.Platforms);
        new AutoNumeric(`#editSpread-${policyId}`, {
            minimumValue: 0,
            maximumValue: 20,
            decimalPlaces: 1
        }).set(editPolicy.Spread);

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