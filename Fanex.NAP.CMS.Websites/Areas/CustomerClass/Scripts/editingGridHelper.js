"use strict";

const editingGridHelper = (function ($) {


    function buildRowNoData(colspan, textNoData) {
        return `<tr><td colspan="${colspan}" class="text-red align-center">${textNoData}</td></tr>`;
    }

    function renderAddEditCustomerClass(divId, selectedValue, fnOnCheck) {
        let optionList = [];
        let isCheckAll = selectedValue === "All" || selectedValue === "0";
        let selectedValueList = selectedValue.split(",");
        customerClassList.forEach(item => optionList.push({ value: item, text: item.toString(), checked: isCheckAll || selectedValueList.includes(item.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 246,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: fnOnCheck
            }
        );
    }

    function renderAddEditSite(divId, selectedValue, cachedCreditSites, cachedLicenseeSites, isCheckAll, isAllCredit, isAllDeposit, fnOnCheck) {
        var listData = customerClassHelper.buildGroupSiteList(isAllCredit, isAllDeposit, selectedValue, cachedCreditSites, cachedLicenseeSites);
        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 176,
                maxHeight: 230,
                listData: listData,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: fnOnCheck
            }
        );
    }

    function renderAddEditCurrency(divId, selectedValue, fnOnCheck) {
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
                onCheck: fnOnCheck
            }
        );
    }

    function renderAddEditPlatform(divId, selectedValue, platforms, fnOnCheck) {
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
                onCheck: fnOnCheck
            }
        );
    }

    function renderAddEditUtility(divId, selectedValue, utilityList, fnOnCheck) {
        let selectDiv = $(`#${divId}`);
        selectDiv.empty();
        selectDiv.append($('<option>', {
            value: "1",
            text: 'Parlay Boost'
        }));

        selectDiv.combobox();

        return selectDiv;
    }

    function renderAddEditProduct(divId, selectedValue, fnOnCheck, isSelectAll = false) {
        let optionList = [];
        let isCheckAll = selectedValue === "All" || selectedValue === "0" || isSelectAll;
        let selectedValueList = selectedValue.split(",");
        productList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: isCheckAll || selectedValueList.includes(item.Id.toString()), enable: true })
        );

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 146,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: isCheckAll,
                showFilter: true,
                onCheck: fnOnCheck
            }
        );
    }

    function renderCombobox(id, dataList, selectedValue) {
        var datum, html = [];
        if (typeof selectedValue === "undefined") {
            selectedValue = dataList[0].Value;
        }

        for (var i = 0, max = dataList.length; i < max; i++) {
            datum = dataList[i];
            html.push('<option value="');
            html.push(datum.Value);
            html.push('"');

            if (datum.Value === selectedValue.toString()) {
                html.push(' selected');
            }

            html.push('>');
            html.push(datum.Text);
            html.push('</option>');
        }

        return $("#" + id).html(html.join("")).combobox();
    }


    function renderCurrencyCell(selectedCurrencies, currencyList) {
        if (selectedCurrencies === "All" || selectedCurrencies === "0") {
            return `<a href="javascript:;" class="link-currency" data-currencies="${currencyList.map(cur => cur.Name).join()}">All</a>`;
        }

        const currencyArray = selectedCurrencies.split(",");
        var currencyNames = currencyList.filter(cur => currencyArray.includes(cur.Id.toString())).map(cur => cur.Name);
        return currencyNames.length <= 1
            ? currencyNames.join()
            : `<a href="javascript:;" class="link-currency" data-currencies="${currencyNames.join()}">${currencyNames.length}</a>`;
    }

    function renderCustomerClassCell(selectedCustomerClass) {
        if (selectedCustomerClass === "0" || selectedCustomerClass === "") {
            return "All";
        }

        const customerClassList = selectedCustomerClass.split(",");
        return customerClassList.join(", ");
    }

    function renderSiteCell(selectedSites, siteList, cachedCreditSites, cachedLicenseeSites, allValue, allCreditValue, allDepositValue, isIdSelection) {
        if (selectedSites === allValue) {
            return `<a href="javascript:;" class="link-site" data-sites="${siteList.map(st => st.Name).join()}">All</a>`;
        }
        let detailSelectedSites = selectedSites;

        if (selectedSites.includes(allCreditValue)) {
            detailSelectedSites = detailSelectedSites.replace(allCreditValue, cachedCreditSites.map(st => st.value).join());
        }

        if (selectedSites.includes(allDepositValue)) {
            detailSelectedSites = detailSelectedSites.replace(allDepositValue, cachedLicenseeSites.map(st => st.value).join());
        }

        var sitesNameList = detailSelectedSites.split(",");
        var displayName = sitesNameList.length;
        if (isIdSelection) {
            sitesNameList.forEach(function (siteName, pos) {
                var matchSite = siteList.find(st => st.Id.toString() === siteName);
                if (matchSite) {
                    sitesNameList[pos] = matchSite.Name;
                }
            });
        }
        if (selectedSites === allCreditValue) {
            displayName = 'All Credit';
        }

        if (selectedSites === allDepositValue) {
            displayName = 'All Deposit';
        }

        return sitesNameList.length <= 1
            ? sitesNameList
            : `<a href="javascript:;" class="link-site" data-sites="${sitesNameList.join()}">${displayName}</a>`;
    }
    
    function renderProductCell(selectedProducts, productList) {
        if (selectedProducts === null || selectedProducts === undefined) {
            return "";
        }

        if (selectedProducts === "All" || selectedProducts === "0" || selectedProducts === "256") {
            return `<a href="javascript:;" class="link-product" data-products="${productList.map(pro => pro.Name).join()}">All</a>`;
        }

        const productArray = selectedProducts.split(",");
        var productNames = productList.filter(pro => productArray.includes(pro.Id.toString())).map(pro => pro.Name);
        return productNames.length <= 1
            ? productNames.join()
            : `<a href="javascript:;" class="link-product" data-products="${productNames.join()}">${productNames.length}</a>`;
    }

    function renderEnableCell(policyId, policyTitle, enable, hasHostPermisson) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-policyTitle="${policyTitle.replaceAll('"', '&#34;')}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
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

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    function isEmptyValue(value) {
        return typeof value === "undefined" || value === null || value === "";
    }


    function validateRequiredField(div, value) {
        if (isEmptyValue(value)) {
            CMS.showTipsyError(div, resources.requiredField, "n");
            return;
        }

        return value;
    }

    function validateTitle(inputElement, gravity) {
        const textValue = $.trim(inputElement.val());
        if (!textValue.length) {
            CMS.showTipsyError(inputElement, resources.requiredField, "nw");
            return;
        }

        if (!customerClassHelper.isASCII(textValue)) {
            CMS.showTipsyError(inputElement, `Title cannot contain symbols.`, "nw");
            return;
        }

        return textValue;
    }

    function isValidUtilityOffer(rule) {
        return !isEmptyValue(rule.PolicyTitle)
            && !isEmptyValue(rule.CustomerClass)
            && !isEmptyValue(rule.Currencies)
            && !isEmptyValue(rule.Sites);
    }


    function getSelectedSites(divId, siteList, allValue, cachedCreditSites, allCreditSitesValue, cachedLicenseeSites, allLicenseeSitesValue) {
        var selectedSites = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedSites.length === siteList.length) {
            return allValue;
        }

        var selectedCreditSites = selectedSites.filter(site => cachedCreditSites.some(creditSite => creditSite.value.toString() === site.toString()));
        var selectedDepositSites = selectedSites.filter(site => cachedLicenseeSites.some(licenseeSite => licenseeSite.value.toString() === site.toString()));

        selectedSites = [];

        if (selectedCreditSites.length === cachedCreditSites.length) {
            selectedSites.push(allCreditSitesValue);
        } else {
            selectedSites = selectedCreditSites;
        }

        if (selectedDepositSites.length === cachedLicenseeSites.length) {
            selectedSites.push(allLicenseeSitesValue);
        } else {
            selectedSites.push(...selectedDepositSites);
        }

        return selectedSites.join();
    }

    return {
        buildRowNoData: buildRowNoData,
        renderAddEditCustomerClass: renderAddEditCustomerClass,
        renderAddEditSite: renderAddEditSite,
        renderAddEditCurrency: renderAddEditCurrency,
        renderAddEditPlatform: renderAddEditPlatform,
        renderAddEditUtility: renderAddEditUtility,
        renderAddEditProduct: renderAddEditProduct,
        renderCombobox: renderCombobox,
        renderCurrencyCell: renderCurrencyCell,
        renderProductCell: renderProductCell,
        renderCustomerClassCell: renderCustomerClassCell,
        renderSiteCell: renderSiteCell,
        renderEnableCell: renderEnableCell,
        handleViewItems: handleViewItems,
        clearAllTipsyErrors: clearAllTipsyErrors,
        isEmptyValue: isEmptyValue,
        validateRequiredField: validateRequiredField,
        validateTitle: validateTitle,
        isValidUtilityOffer: isValidUtilityOffer,
        getSelectedSites: getSelectedSites
    };
})(jQuery);