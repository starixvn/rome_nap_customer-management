'use strict';
(function ($) {
    let searchUrlAndQueryString = null,
        gridOptions = null,
        freeGrid = $('#freegrid'),
        boxBulkUpdate = $('#box-bulkUpdate'),
        btnBulkUpdate = $('#btn-bulkUpdate'),
        btnBulkRemove = $('#btn-bulkRemove'),
        hasHostPermisson = null,
        siteListData = [],
        countryListData = [];

    function init() {
        const btnAdd = $('#btnAdd');
        hasHostPermisson = btnAdd.length !== 0;

        const ddlSystem = renderSystemOrStatus('ddl-system');
        const ddlStatus = renderSystemOrStatus('ddl-status');
        const txtRemark = $('#txt-remark');
        // Site
        let ddlSite = $('#ddl-site');
        siteListData = buildMultiselectListData(ddlSite);
        ddlSite = renderMultiselect(ddlSite, {
            showFilter: true,
            noItemFoundText: resources.NoItemFound.replace('#', resources.Site),
            listData: siteListData
        });
        // Country
        let ddlCountry = $('#ddl-country');
        countryListData = buildMultiselectListData(ddlCountry);
        ddlCountry = renderMultiselect(ddlCountry, {
            showFilter: true,
            noItemFoundText: resources.NoItemFound.replace('#', resources.RestrictedCountries),
            listData: countryListData
        });

        btnAdd.click(function () {
            domainManagementAddOrEdit.openModal(domainManagementAddOrEdit.ADD_NEW);
        });

        btnBulkUpdate.click(function () {
            const domains = [];
            $('.chk-domain:checked').each(function () {
                const $this = $(this);
                domains.push({ id: $this.data('domainid'), name: $this.data('domainname') });
            });


            domains.length > 1 && domainManagementAddOrEdit.openModal(domainManagementAddOrEdit.BULK_EDIT, domains);
        });

        btnBulkRemove.click(function () {
            const domains = [];
            $('.chk-domain:checked').each(function () {
                const $this = $(this);
                domains.push({ id: $this.data('domainid'), name: $this.data('domainname') });
            });


            domains.length > 1 && handleRemoveDomains(domains, resources.BulkRemoveDomainConfirm.replace('#', domains.length));
        });

        $('#formSearch').submit(function (e) {
            e.preventDefault();

            const searchParams = {
                system: ddlSystem.fanexMultiSelect('getItems', true).join(),
                site: ddlSite.fanexMultiSelect('getItems', true).join(),
                status: ddlStatus.fanexMultiSelect('getItems', true).join(),
                remark: $.trim(txtRemark.val()),
                countries: ddlCountry.fanexMultiSelect('isCheckAll')
                    ? '0'
                    : ddlCountry.fanexMultiSelect('getItems', true).join()
            };
            searchUrlAndQueryString = `${this.action}?${$.param(searchParams)}`;
            search();
        }).trigger('submit');
    }

    function renderSystemOrStatus(elementId) {
        let multiselect = $(`#${elementId}`);

        return renderMultiselect(multiselect, {
            listData: buildMultiselectListData(multiselect),
            minWidth: 130
        });
    }

    function renderMultiselect($element, options) {
        const multiselect = $element.fanexMultiSelect($.extend({
            checkAllText: resources.All,
            noneSelectedText: resources.PleaseSelect,
            tooltipNoItem: '',
            filterPlaceHolder: '',
            isCheckAll: true
        }, options));
        $(`#${$element.attr('id')}`).next().find('#fanex-multiselect-filter-input').attr('autocomplete', 'off');

        return multiselect;
    }

    function buildMultiselectListData($element) {
        const listData = [];
        const optgroup = $element.find('optgroup');

        if (optgroup.length) {
            optgroup.each(function () {
                const group = { text: this.label, value: this.label, enable: true, items: [] };
                $(this).find('option').each(function () {
                    group.items.push({ text: this.innerText, value: this.value, enable: true });
                });
                listData.push(group);
            });
        } else {
            $element.find('option').each(function () {
                listData.push({ text: this.innerText, value: this.value, enable: true });
            });
        }

        return listData;
    }

    function search() {
        CMS.getJson(
            searchUrlAndQueryString,
            function (data) {
                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = buildBodyRows(JSON.parse(data));

                boxBulkUpdate.toggleClass('display-none', gridOptions.bodyRows.length === 0);
                freeGrid.removeClass('display-none').fanexGrid(gridOptions);
                bindEventsAfterRenderGridSuccessfully();
            });
    }

    function getGridOptions() {
        const columns = [
            { HeaderText: '#', Width: 40, Name: 'colNo', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.Domain, Width: 200, Name: 'colDomain', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: resources.System, Width: 100, Name: 'colSystem', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.Site, Width: 100, Name: 'colSite', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.BrandName, Width: 80, Name: 'colBrandName', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.Status + `<span class="light-note-info-message" title="${$('#divNoteStatus').html()}"></span>`, Width: 80, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.RestrictedCountries, Width: 120, Name: 'colRestrictedCountries', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.Remark, Width: 200, Name: 'colRemark', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
            { HeaderText: resources.ExpiredDate, Width: 100, Name: 'colExpiredDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: resources.LastUpdated, Width: 120, Name: 'colLastUpdated', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        if (hasHostPermisson) {
            columns.unshift({ HeaderText: '<input type="checkbox" id="chk-checkAll"/>', Width: 30, Name: 'colCheckAll', HeaderAlign: 'center', CellAlign: 'center' });
            columns.push({ HeaderText: resources.Action, Width: 80, Name: 'colAction', HeaderAlign: 'center', CellAlign: 'center' });
        }

        return {
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 55,
            gridExpandHeight: hasHostPermisson ? 180 : 150,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(data) {
        let number = 0;
        return data.map(function (domain) {
            const columns = [];

            if (hasHostPermisson) {
                columns.push({ Name: 'colCheckAll', Value: renderCheckboxCell(domain.DomainId, domain.DomainName) });
            }

            columns.push({ Name: 'colNo', Value: ++number });
            columns.push({ Name: 'colDomain', Value: renderDomainName(domain.DomainName, domain.OfferType) });
            columns.push({ Name: 'colSystem', Value: domain.System });
            columns.push({ Name: 'colSite', Value: renderTooltipCell(domain.SiteNames, 'site') });
            columns.push({ Name: 'colBrandName', Value: domain.BrandName });
            columns.push({ Name: 'colStatus', Value: domain.StatusName });
            columns.push({ Name: 'colRestrictedCountries', Value: renderTooltipCell(domain.RestrictedCountryNames, 'country', ';') });
            columns.push({ Name: 'colRemark', Value: domain.Remark ? CMS.htmlEncode(domain.Remark) : '' });
            columns.push({ Name: 'colExpiredDate', Value: domain.ExpiredDateString });
            columns.push({ Name: 'colLastUpdated', Value: domain.LastUpdated });

            if (hasHostPermisson) {
                columns.push({ Name: 'colAction', Value: renderActionCell(domain.DomainId, domain.DomainName) });
            }

            return { Columns: columns };
        });
    }

    function renderCheckboxCell(domainId, domainName) {
        return `<input type="checkbox" class="chk-domain" data-domainid="${domainId}" data-domainname="${domainName}"/>`;
    }

    function renderTooltipCell(text, className, splitChar) {
        if (text) {
            const textArray = text.split(splitChar || ',');
            const textArrayLength = textArray.length;

            return textArrayLength === 1
                ? text
                : `<span class="note-info-message ${className}" title="${text}"></span><span>${textArrayLength}</span>`;
        }

        return '';
    }

    function renderActionCell(domainId, domainName) {
        return [
            '<span class="ico-edit mid-margin-right" title="Update" data-domainid="', domainId, '"></span>',
            '<span class="ico-copy mid-margin-right" title="Copy" data-domainid="', domainId, '"></span>',
            '<span class="ico-delete" title="Delete" data-domainid="', domainId, '" data-domainname="', domainName, '"></span>'
        ].join('');
    }

    function renderDomainName(domainName, offerType) {
        return offerType === 1 ? `<div class="domain-name-wrapper"><div class="domain-name">${domainName}</div><div class="ico-offer-cash domain-offer-cash-icon" title="Offer Cash"></div></div>` : `<div class="domain-name">${domainName}</div>`;
    }

    function bindEventsAfterRenderGridSuccessfully() {
        $('.note-info-message.site').tipsy({ opacity: 0.9, gravity: 'sw' });
        $('.note-info-message.country').tipsy({ opacity: 0.9, gravity: 's' });
        $('.light-note-info-message').tipsy({ opacity: 1, gravity: 'w', html: true });

        if (hasHostPermisson) {
            const checkboxAll = $('#chk-checkAll');
            const checkboxDomains = $('.chk-domain');

            checkboxAll.off().change(function () {
                const checked = this.checked;
                checkboxDomains.prop('checked', this.checked);
                if (checked && checkboxDomains.filter(':checked').length > 1) {
                    btnBulkUpdate.removeAttr('disabled');
                    btnBulkRemove.removeAttr('disabled');
                } else {
                    btnBulkUpdate.attr('disabled', 'disabled');
                    btnBulkRemove.attr('disabled', 'disabled');
                }
            });

            checkboxDomains.off().click(function () {
                const checked = this.checked;
                checkboxAll.prop('checked', checked && checkboxDomains.not(':checked').length === 0);
                if (checkboxDomains.filter(':checked').length > 1) {
                    btnBulkUpdate.removeAttr('disabled');
                    btnBulkRemove.removeAttr('disabled');
                } else {
                    btnBulkUpdate.attr('disabled', 'disabled');
                    btnBulkRemove.attr('disabled', 'disabled');
                }
            });

            $('.ico-edit').off().click(function () {
                handleGetDomainById($(this).data('domainid'), domainManagementAddOrEdit.EDIT);
            });

            $('.ico-copy').off().click(function () {
                handleGetDomainById($(this).data('domainid'), domainManagementAddOrEdit.COPY);
            });

            $('.ico-delete').off().click(function () {
                const $this = $(this);
                const id = $this.data('domainid');
                const name = $this.data('domainname');
                handleRemoveDomains([{ id, name }], resources.RemoveDomainConfirm.replace('#', name));
            });
        }
    }

    function handleGetDomainById(domainId, action) {
        CMS.getJson(
            `Get?domainId=${domainId}`,
            function (data) {
                const domain = JSON.parse(data);
                if (action === domainManagementAddOrEdit.COPY) {
                    domain.DomainId = undefined;
                    domain.DomainName = undefined;
                }

                domainManagementAddOrEdit.openModal(action, domain);
            });
    }

    function handleRemoveDomains(domains, confirmMessage) {
        CMS.showConfirmMessage(
            confirmMessage,
            function () {
                CMS.disableInput();
                CMS.callAjax(
                    this,
                    'Remove',
                    domains,
                    function (result) {
                        const resultJson = JSON.parse(result);
                        if (resultJson.IsSuccess) {
                            CMS.showSuccessMessage(resultJson.Message, search);
                        } else {
                            CMS.showErrorMessage(resultJson.Message);
                        }
                    });
            });
    }

    $(function () {
        init();
        domainManagementAddOrEdit.init(search, renderMultiselect, siteListData, countryListData);
    });
})(jQuery);