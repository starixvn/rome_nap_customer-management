"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        cachedGridOptions = null,
        freeGrid = $("#freegrid");

    function init() {
        const ddlSite = customerClassHelper.renderMultiSelect(
            "#ddlSite",
            {
                minWidth: 180,
                showFilter: true,
                showTooltip: false,
                listData: buildGroupSiteList(true)
            });
        const ddlPolicyTitle = $("#ddlPolicyTitle");
        const ddlPlatform = renderPlatform();

        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                sites: ddlSite.fanexMultiSelect("getSelectedTexts", true).join(),
                policyId: ddlPolicyTitle.val(),
                platforms: ddlPlatform.fanexMultiSelect("getItems", true).join()
            };
            cachedSearchUrlWithQueryString = `${this.action}?${$.param(formData)}`;
            search(0, getPageSize());
        });

        const btnUpload = $("#btnUpload");
        const hasHostPermisson = btnUpload.length > 0;
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
            { HeaderText: resources.platform, Width: 260, Name: "colPlatform", HeaderAlign: "center", CellAlign: "center" }
        ];

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
            return {
                Columns: [
                    { Name: "colPolicyTitle", Value: datum.PolicyTitle },
                    { Name: "colSite", Value: datum.Site },
                    { Name: "colCurrency", Value: renderCurrencyCell(datum.IsAllCurrencies, datum.Currencies) },
                    { Name: "colPlatform", Value: datum.PlatformNames }
                ]
            };
        });
    }

    function renderCurrencyCell(isAllCurrencies, currencies) {
        if (isAllCurrencies) {
            return `<a href="javascript:;" class="link-currency" data-currencies="${currencies}">All</a>`;
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

    function getPageSize() {
        return cachedGridOptions ? cachedGridOptions.pagerOption.itemsPerPage : 100;
    }

    $(function () {
        init();
    });
})(jQuery);