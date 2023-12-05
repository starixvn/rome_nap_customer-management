"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        cachedGridOptions = null,
        hasHostPermisson = false,
        cachedSites = [],
        creditSites = [],
        depositSites = [],
        freeGrid = $("#freegrid");

    function init() {
        cachedSites = buildGroupSiteList(true);
        creditSites = siteList.filter(s => !s.IsLicensee).map(s => s.Name);
        depositSites = siteList.filter(s => s.IsLicensee).map(s => s.Name);
        const ddlSite = customerClassHelper.renderMultiSelect(
            "#ddlSite",
            {
                minWidth: 180,
                showFilter: true,
                showTooltip: false,
                listData: cachedSites
            });
        const ddlPolicyTitle = $("#ddlPolicyTitle");
        const ddlPlatform = renderPlatform();

        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                sites: getFilterSites(ddlSite),
                policyId: ddlPolicyTitle.val(),
                platforms: ddlPlatform.fanexMultiSelect("getItems", true).join()
            };
            cachedSearchUrlWithQueryString = `${this.action}?${$.param(formData)}`;
            search(0, getPageSize());
        });

        const btnUpload = $("#btnUpload");
        hasHostPermisson = btnUpload.length > 0;
        if (hasHostPermisson) {
            siteManagementUpload.init(function () {
                cachedGridOptions && search(0, getPageSize());
            });

            const mapToMultiSelectItem = function (item) {
                return {
                    value: item.Id,
                    text: item.Name,
                    checked: false,
                    enable: true
                };
            };
            siteManagementAssignSiteSetting.init(
                buildGroupSiteList(false),
                currencyList.map(mapToMultiSelectItem),
                platformList.map(mapToMultiSelectItem),
                function () {
                    cachedGridOptions && search(0, getPageSize());
                });

            btnUpload.click(function () {
                siteManagementUpload.openModal();
            });

            $("#btnAssignSiteSetting").click(function () {
                siteManagementAssignSiteSetting.openModal();
            });

            // Empty the #divPopupContent since it's children was cached.
            // It also helps to prevent the 'for' attributes do not work correctly in the popup.
            $("#divPopupContent").remove();
        }
    }

    function buildGroupSiteList(isChecked) {
        let creditOrLicensee;
        let credits = [];
        let licensees = [];
        siteList.forEach(function (item) {
            creditOrLicensee = item.IsLicensee ? licensees : credits;
            creditOrLicensee.push({
                value: item.Id,
                text: item.Name,
                checked: isChecked,
                enable: true
            });
        });

        return [
            {
                value: "credit",
                text: "Credit",
                checked: isChecked,
                enable: true,
                items: credits
            },
            {
                value: "licensee",
                text: "Licensee",
                checked: isChecked,
                enable: true,
                items: licensees
            }
        ];
    }

    function getFilterSites(element) {
        const selectedValues = element.fanexMultiSelect("getSelectedTexts", true);
        var allSitesLength = cachedSites[0].items.length + cachedSites[1].items.length;
        if (selectedValues.length === allSitesLength) {
            return "All";
        }
        if (selectedValues.length) {
            var selectedCreditSite = cachedSites[0].items.filter(s => selectedValues.includes(s.text)).map(s => s.text);
            var selectedLicenseeSite = cachedSites[1].items.filter(s => selectedValues.includes(s.text)).map(s => s.text);
            var result = [];
            if (selectedCreditSite.length === cachedSites[0].items.length) {
                result.push("AllCredit");
                result = result.concat(selectedLicenseeSite);
            } else if (selectedLicenseeSite.length === cachedSites[1].items.length) {
                result = selectedCreditSite;
                result.push("AllDeposit");
            } else {
                result = selectedValues;
            }

            return result.join();
        }
    }

    function renderPlatform() {
        const platforms = platformList.map(function (item) {
            return { value: item.Id, text: item.Name, checked: true, enable: true };
        });

        return customerClassHelper.renderMultiSelect(
            "#ddlPlatform",
            {
                minWidth: 150,
                listData: platforms
            });
    }

    function search(pageIndex, pageSize) {
        // PageIndex of Fanex.Grid begin at 0 whereas server site begin at 1.
        CMS.getJson(
            `${cachedSearchUrlWithQueryString}&pageIndex=${pageIndex + 1}&pageSize=${pageSize}`,
            function (data) {
                assignCachedGridOptions($.parseJSON(data), pageIndex, pageSize);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                freeGrid.find(".link-currency")
                    .off("click")
                    .click(handleViewCurrencies);
                freeGrid.find(".link-sites")
                    .off("click")
                    .click(handleViewSites);
                freeGrid.find("#chk-select-delete-all")
                    .off("click")
                    .click(handleCheckDelAll);
                freeGrid.find("#btn-delete-selected")
                    .off("click")
                    .click(handleDelSelected);
                freeGrid.find(".chk-select-delete")
                    .off("click")
                    .click(handleSingleCheckDel);
            },
            function (error) {
                assignCachedGridOptions({ TotalItems: 0, SiteSettings: [] }, 0, 100);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            });
    }

    function assignCachedGridOptions(data, pageIndex, pageSize) {
        cachedGridOptions = cachedGridOptions || defaultGridOptions();

        cachedGridOptions.pagerOption.currentPage = pageIndex;
        cachedGridOptions.pagerOption.itemsPerPage = pageSize;
        cachedGridOptions.pagerOption.totalItem = data.TotalItems;

        cachedGridOptions.bodyRows = buildBodyRows(data.SiteSettings);
    }

    function defaultGridOptions() {
        const columns = [
            { HeaderText: resources.policyTitle, Width: 300, Name: "colPolicyTitle", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
            { HeaderText: resources.site, Width: 200, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.currency, Width: 260, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.platform, Width: 260, Name: "colPlatform", HeaderAlign: "center", CellAlign: "center" },
            
        ];
        hasHostPermisson && columns.push({ HeaderText: resources.delete + '<br/><input type="checkbox" id="chk-select-delete-all" class="ver-mid"><span id="btn-delete-selected" class="ico-trash-white ver-mid" title="Delete"></span>', Width: 80, Name: "colDelete", HeaderAlign: "center", CellAlign: "center" });

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 160,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: search
            }
        };
    }

    function buildBodyRows(data) {
        return data.map(function (datum) {
            const columns = [
                { Name: "colPolicyTitle", Value: datum.PolicyTitle },
                { Name: "colSite", Value: renderSitesCell(datum.Site) },
                { Name: "colCurrency", Value: renderCurrencyCell(datum.IsAllCurrencies, datum.Currencies) },
                { Name: "colPlatform", Value: datum.PlatformNames }
            ];
            hasHostPermisson && columns.push({ Name: "colDelete", Value: renderDeleteCell(datum.PolicyId, datum.Site, datum.CurrencyID, datum.Platforms) });

            return {
                Columns: columns
            };
        });
    }

    function renderSitesCell(sites) {
        var arraySites = sites.split(',');
        if (arraySites.includes('All')) {
            return `<a href="javascript:;" class="link-sites" data-sites="${siteList.map(s => s.Name).join()}">All</a>`;
        }

        if (arraySites.includes("AllCredit")) {
            arraySites = arraySites.filter(st => !creditSites.includes(st));
        }

        if (arraySites.includes("AllDeposit")) {
            arraySites = arraySites.filter(st => !depositSites.includes(st));
        }

        var removeDublicateSites = arraySites.join();

        if (removeDublicateSites === "AllCredit") {
            return `<a href="javascript:;" class="link-sites" data-sites="${creditSites.join()}">All Credit</a>`;
        }

        if (removeDublicateSites === "AllDeposit") {
            return `<a href="javascript:;" class="link-sites" data-sites="${depositSites.join()}">All Deposit</a>`;
        }

        if (arraySites.includes("AllCredit")) {
            removeDublicateSites = removeDublicateSites.replace("AllCredit", creditSites.join());
        }

        if (arraySites.includes("AllDeposit")) {
            removeDublicateSites = removeDublicateSites.replace("AllDeposit", depositSites.join());
        }
        arraySites = removeDublicateSites.split(",");
        
        const numOfSites = arraySites.length;

        return numOfSites <= 5
            ? arraySites.join(", ")
            : `<a href="javascript:;" class="link-sites" data-sites="${removeDublicateSites}">${numOfSites}</a>`;
    }

    function renderCurrencyCell(isAllCurrencies, currencies) {
        if (isAllCurrencies) {
            return `<a href="javascript:;" class="link-currency" data-currencies="${currencyList.map(cur => cur.Name).join()}">All</a>`;
        }

        const currencyArray = currencies.split(",");
        const numOfCurrencies = currencyArray.length;
        return numOfCurrencies <= 5
            ? currencyArray.join(", ")
            : `<a href="javascript:;" class="link-currency" data-currencies="${currencies}">${numOfCurrencies}</a>`;
    }

    function handleViewCurrencies() {
        let html = [
            '<div class="form" id="viewCurrenciesForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', resources.currency, '</th></tr></thead>',
            '<tbody>'
        ];
        $(this).data("currencies").split(",").forEach(function (item, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            resources.currency,
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

    function handleViewSites() {
        let html = [
            '<div class="form" id="viewCurrenciesForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', resources.site, '</th></tr></thead>',
            '<tbody>'
        ];
        $(this).data("sites").split(",").forEach(function (item, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            resources.site,
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

    function renderDeleteCell(id, site, currency, platform) {
        return `<input type="checkbox" class="chk-select-delete" data-id="${id}" data-site="${site}" data-currency="${currency}" data-platform="${platform}">`;
    }

    function handleCheckDelAll() {
        let checked = $(this).prop("checked");
        $(".chk-select-delete").each(function () {
            $(this).prop("checked", checked);
        });
    }

    function handleSingleCheckDel() {
        let isCheckAll = true;
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") !== true) {
                isCheckAll = false;

                return false;
            }
        });

        $("#chk-select-delete-all").prop("checked", isCheckAll);
    }

    function handleDelSelected() {
        var settings = [];
        $(".chk-select-delete").each(function () {
            if ($(this).prop("checked") === true) {
                settings.push({
                    PolicyId: $(this).data("id"),
                    Site: $(this).data("site").toString(),
                    Currency: $(this).data("currency").toString(),
                    Platform: $(this).data("platform").toString()
                });
            }
        });
        if (settings.length === 0) {
            return;
        }

        CMS.showConfirmMessage(`Are you sure you want to delete the selected setting row(s)?`, function () { callDelete(settings); });
    }

    function callDelete(siteSettings) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`Site/Delete`),
            {SiteSettings: siteSettings},
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    search(0, getPageSize());
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function getPageSize() {
        return cachedGridOptions ? cachedGridOptions.pagerOption.itemsPerPage : 100;
    }

    $(function () {
        init();
    });
})(jQuery);