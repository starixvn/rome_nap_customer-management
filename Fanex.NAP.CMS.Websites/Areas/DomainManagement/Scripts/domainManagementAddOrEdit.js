'use strict';
const domainManagementAddOrEdit = (function ($) {
    const ADD_NEW = 1,
        EDIT = 2,
        COPY = 3,
        BULK_EDIT = 4;
    let addNewOrEditForm = null,
        formMode = null,
        inlineMessage = null,
        txtDomain = null,
        ddlSystem = null,
        ddlOfferType = null,
        ddlSite = null,
        ddlStatus = null,
        txtExpiredDate = null,
        ddlCountry = null,
        txtRemark = null,
        ddlBrandName = null,
        reloadDomainGrid = null,
        renderMultiselect = null,
        siteListData = null,
        countryListData = null;

    function init(reloadGridFunc, renderMultiselectFunc, sites, countries) {
        reloadDomainGrid = reloadGridFunc;
        renderMultiselect = renderMultiselectFunc;
        siteListData = sites;
        countryListData = countries;
        addNewOrEditForm = $('#addNewOrEditForm');
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $('#divPopupContent').remove();
    }

    function openModal(mode, domain) {
        formMode = mode;
        domain = domain || {};
        const form = addNewOrEditForm.clone();
        form.addClass(isEdit() ? 'edit' : (isBulkEdit() ? 'bulk-edit' : 'add-new'));
        CMS.modal(
            getModalTitle(),
            form,
            {
                resizable: false,
                position: {
                    my: 'center top',
                    at: 'center top',
                    of: '#headline'
                }
            });

        cacheElements(domain);
        bindEvents(domain);
    }

    function cacheElements(domain) {
        let domainName = domain.DomainName;
        if (isAddNew() || isCopy()) {
            $('.add-clone').removeClass('display-none');
            $('.edit').remove();
        }

        if (isEdit()) {
            $('.add-clone').remove();
            $('.edit').removeClass('display-none');
        }

        if (isBulkEdit()) {
            $('.edit').remove();
            domainName = domain.map(function (item) {
                return item.name;
            }).join(', ');
        }

        inlineMessage = $('#popup-inlineMessage');
        txtDomain = $('#popup-domain').val(domainName);
        if (isBulkEdit()) {
            txtDomain
                .prop('disabled', true)
                .removeClass('display-none');
        }

        txtRemark = $('#popup-remark').val(domain.Remark);
        const dpkExpiredDate = $('#dpkExpiredDate');
        Layout.createDatepicker(
            dpkExpiredDate,
            'popup-expiredDate',
            resources.ExpiredDate,
            '',
            null,
            null);
        txtExpiredDate = $('#popup-expiredDate').val(domain.ExpiredDateString || resources.PleaseSelect).removeAttr('readonly');
        ddlSystem = renderCombobox('popup-system', domain.System);
        ddlOfferType = renderCombobox('popup-offer-type', domain.OfferType);
        ddlStatus = renderCombobox('popup-status', domain.Status);
        ddlBrandName = renderCombobox('popup-brandName', domain.BrandName);
        ddlSite = $('#popup-site');
        ddlSite = renderMultiselect(ddlSite, {
            showFilter: true,
            noItemFoundText: resources.NoItemFound.replace('#', resources.Site),
            isCheckAll: false,
            listData: buildListDataWithSelected(siteListData, domain.Site),
            maxHeight: 240,
            onCheck: function () {
                this.checked && CMS.clearTipsyError($('#popup-site'));
            }
        });
        ddlCountry = $('#popup-country');
        ddlCountry = renderMultiselect(ddlCountry, {
            showFilter: true,
            noItemFoundText: resources.NoItemFound.replace('#', resources.RestrictedCountries),
            isCheckAll: false,
            listData: buildListDataWithSelected(countryListData, domain.RestrictedCountries),
            maxHeight: 135
        });
        $('#popup-note-status').attr('title', $('#divNoteStatus').html()).tipsy({ opacity: 1, gravity: 'w', html: true });
    }

    function bindEvents(domain) {
        $('#popup-save').off().click(function () {
            const formData = getFormData(domain);
            if (formData) {
                CMS.disableInput();
                CMS.callAjax(
                    this,
                    'AddOrEdit',
                    formData,
                    function (result) {
                        const resultJson = JSON.parse(result);
                        if (resultJson.IsSuccess) {
                            CMS.showInlineSuccessMsg(
                                resultJson.Message,
                                inlineMessage,
                                true,
                                function () {
                                    CMS.closePopup();
                                    reloadDomainGrid();
                                });
                        } else {
                            CMS.showInlineErrorMsg(resultJson.Message, inlineMessage);
                        }
                    });
            }
        });

        $('#popup-cancel').off().click(CMS.closePopup);
    }

    function getFormData(domain) {
        const domainArray = isBulkEdit() ? [] : validateDomain();
        const system = validateCombobox(ddlSystem, ddlSystem.next('#popup-system'));
        const offerType = validateCombobox(ddlOfferType, ddlOfferType.next('#popup-offer-type'));
        const site = validateSite();
        const status = validateCombobox(ddlStatus, ddlStatus.next('#popup-status'));

        const isValid = !isUndefined(domainArray)
            && !isUndefined(system)
            && !isUndefined(offerType)
            && !isUndefined(site)
            && !isUndefined(status);

        if (isValid) {
            let domains, isEdit;
            if (isBulkEdit()) {
                domains = domain;
                isEdit = true;
            } else {
                const domainId = domain.DomainId;
                domains = domainArray.map(function (item) {
                    return { id: domainId, name: item };
                });
                isEdit = !isUndefined(domainId) && domainId > 0;
            }
            
            return {
                domains,
                system,
                offerType: parseInt(offerType),
                site,
                status,
                expiredDate: getExpiredDate(),
                restrictedCountries: ddlCountry.fanexMultiSelect('getItems', true).join(),
                remark: $.trim(txtRemark.val()),
                brandName: getBrandName(),
                isEdit
            };
        }
    }

    function validateDomain() {
        const domainArray = getDomainArray();
        if (!domainArray.length) {
            CMS.showTipsyError(txtDomain, resources.RequiredFieldError, 'ne');
            return;
        }

        if (domainArray.length > 50) {
            CMS.showTipsyError(txtDomain, resources.DomainsOutOfRangeError.replace('#', 50), 'ne');
            return;
        }

        const invalidDomainArray = domainArray.filter(function (domain) {
            return !isValidDomain(domain);
        });

        if (invalidDomainArray.length) {
            CMS.showTipsyError(txtDomain, resources.InvalidDomainFormat.replace('#', invalidDomainArray.join(', ')), 'ne');
            return;
        }

        var duplicatedDomainArray = domainArray.filter(function (item, index) {
            return domainArray.indexOf(item) !== index;
        });

        if (duplicatedDomainArray.length) {
            CMS.showTipsyError(txtDomain, resources.DuplicatedDomain.replace('#', duplicatedDomainArray.join(', ')), 'ne');
            return;
        }

        CMS.clearTipsyError(txtDomain);
        return domainArray;
    }

    function getDomainArray() {
        let domainValue = $.trim(txtDomain.val());
        if (!domainValue.length) {
            return [];
        }

        if (isEdit()) {
            return [domainValue];
        }

        if (domainValue.indexOf('\n') >= 0) {
            domainValue = domainValue.replace(/\n/g, ' ');
        }

        if (domainValue.indexOf('\r') >= 0) {
            domainValue = domainValue.replace(/\r/g, ' ');
        }

        if (domainValue.indexOf('\t') >= 0) {
            domainValue = domainValue.replace(/\t/g, ' ');
        }

        domainValue = domainValue.replace(/\s/g, ',');

        return domainValue.split(",").filter(domain => domain.length > 0);
    }

    function validateCombobox($select, $selectError) {
        const value = $select.val();
        if (value === '-1') {
            CMS.showTipsyError($selectError, resources.RequiredFieldError, 'w');
            return;
        }

        CMS.clearTipsyError($selectError);
        return value;
    }

    function validateSite() {
        const sites = ddlSite.fanexMultiSelect('getItems', true);
        if (!sites.length) {
            CMS.showTipsyError($('#popup-site'), resources.RequiredFieldError, 'w');
            return;
        }

        return sites.join();
    }

    function getExpiredDate() {
        const expiredDate = $.trim(txtExpiredDate.val());

        return isNaN(Date.parse(expiredDate)) ? undefined : expiredDate;
    }

    function getBrandName() {
        const brandName = ddlBrandName.val();

        return brandName === "-1" ? undefined : brandName;
    }

    function buildListDataWithSelected(listData, selectedString) {
        let selectedObject = {};
        if (!isUndefined(selectedString) && selectedString.length) {
            const selectedArray = selectedString.split(",");
            selectedArray.forEach(function (item) {
                selectedObject[item] = true;
            });
        }

        return listData.map(function (item) {
            let isCheckAllChildren = !isUndefined(item.items);
            const children = item.items ? item.items.map(function (child) {
                const isChecked = selectedObject[child.value];
                if (!isChecked) {
                    isCheckAllChildren = false;
                }

                return {
                    text: child.text,
                    value: child.value,
                    enable: true,
                    checked: isChecked
                };
            }) : [];

            return {
                text: item.text,
                value: item.value,
                enable: true,
                checked: isCheckAllChildren || selectedObject[item.value],
                items: children
            };
        });
    }

    function getModalTitle() {
        if (isAddNew() || isCopy()) {
            return resources.AddNewDomain;
        }

        if (isEdit()) {
            return resources.EditDomain;
        }

        if (isBulkEdit()) {
            return `${resources.EditDomain}s`;
        }
    }

    function isAddNew() {
        return formMode === ADD_NEW;
    }

    function isEdit() {
        return formMode === EDIT;
    }

    function isCopy() {
        return formMode === COPY;
    }

    function isBulkEdit() {
        return formMode === BULK_EDIT;
    }

    function renderCombobox(elementId, selectedValue) {
        const $select = $(`#${elementId}`);
        $select.html(`<option value="-1">${resources.PleaseSelect}</option>${$select.html()}`);
        !isUndefined(selectedValue) && $select.val(selectedValue);

        return $select.combobox();
    }

    function isUndefined(value) {
        return typeof value === 'undefined';
    }

    function isValidDomain(url) {
        if (url.startsWith('http')) {
            return false;
        }

        const urlArray = url.split('.');
        const urlArrayLength = urlArray.length;
        return urlArrayLength > 1 && urlArray[urlArrayLength - 1].length > 1;
        //const pattern = new RegExp(
        //    '^(https?:\\/\\/)' + // protocol
        //    '((([a-z\\d]([a-z\\d-]*[a-z\\d])*))\\.)+[a-z]{2,}', // domain name
        //    'i');
        //return pattern.test(url);
    }

    return {
        init: init,
        openModal: openModal,
        ADD_NEW: ADD_NEW,
        EDIT: EDIT,
        COPY: COPY,
        BULK_EDIT: BULK_EDIT
    };
})(jQuery);