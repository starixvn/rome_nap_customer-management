"use strict";

const editingGridHelper = (function ($) {
    function renderAddEditCustomerClass(divId, selectedValue, fnOnCheck) {
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
                onCheck: fnOnCheck
            }
        );
    }

    function renderAddEditSite(divId, selectedValue, cachedCreditSites, cachedLicenseeSites, fnOnCheck) {
        var isCheckAll = selectedValue === "All";
        var listData = customerClassHelper.buildGroupSiteList(isCheckAll || selectedValue.includes("AllCredit"), isCheckAll || selectedValue.includes("AllDeposit"), selectedValue, cachedCreditSites, cachedLicenseeSites)
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
        let optionList = [];

        optionList.push({ value: "Parlay Boost", text: "Parlay Boost", checked: true, enable: true });

        return customerClassHelper.renderMultiSelect(
            "#" + divId,
            {
                minWidth: 198,
                maxHeight: 230,
                listData: optionList,
                isCheckAll: false,
                showFilter: true,
                onCheck: fnOnCheck
            }
        );
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
        const customerClassList = selectedCustomerClass.split(",");
        return customerClassList.join(", ");
    }

    function renderSiteCell(selectedSites, siteList, cachedCreditSites, cachedLicenseeSites) {
        if (selectedSites === "All") {
            return `<a href="javascript:;" class="link-site" data-sites="${siteList.map(st => st.Name).join()}">All</a>`;
        }
        let detailSelectedSites = selectedSites;

        if (selectedSites.includes("AllCredit")) {
            detailSelectedSites = detailSelectedSites.replace("AllCredit", cachedCreditSites.map(st => st.value).join());
        }

        if (selectedSites.includes("AllDeposit")) {
            detailSelectedSites = detailSelectedSites.replace("AllDeposit", cachedLicenseeSites.map(st => st.value).join());
        }

        const sitesNameList = detailSelectedSites.split(",");
        var displayName = selectedSites !== 'AllCredit' && selectedSites !== 'AllDeposit' ? sitesNameList.length : selectedSites;
        return sitesNameList.length <= 1
            ? detailSelectedSites
            : `<a href="javascript:;" class="link-site" data-sites="${sitesNameList.join()}">${displayName}</a>`;
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


    function getSelectedSites(divId, siteList, cachedCreditSites, cachedLicenseeSites) {
        var selectedSites = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedSites.length === siteList.length) {
            return "All";
        }

        var selectedCreditSites = selectedSites.filter(site => cachedCreditSites.some(creditSite => creditSite.value.toString() === site.toString()));
        var selectedDepositSites = selectedSites.filter(site => cachedLicenseeSites.some(licenseeSite => licenseeSite.value.toString() === site.toString()));

        selectedSites = [];

        if (selectedCreditSites.length === cachedCreditSites.length) {
            selectedSites.push("AllCredit");
        } else {
            selectedSites = selectedCreditSites;
        }

        if (selectedDepositSites.length === cachedLicenseeSites.length) {
            selectedSites.push("AllDeposit");
        } else {
            selectedSites.push(...selectedDepositSites);
        }

        return selectedSites.join();
    }

    return {
        renderAddEditCustomerClass: renderAddEditCustomerClass,
        renderAddEditSite: renderAddEditSite,
        renderAddEditCurrency: renderAddEditCurrency,
        renderAddEditPlatform: renderAddEditPlatform,
        renderAddEditUtility: renderAddEditUtility,
        renderCurrencyCell: renderCurrencyCell,
        renderCustomerClassCell: renderCustomerClassCell,
        renderSiteCell: renderSiteCell,
        handleViewItems: handleViewItems,
        clearAllTipsyErrors: clearAllTipsyErrors,
        isEmptyValue: isEmptyValue,
        validateRequiredField: validateRequiredField,
        validateTitle: validateTitle,
        isValidUtilityOffer: isValidUtilityOffer,
        getSelectedSites: getSelectedSites
    };
})(jQuery);