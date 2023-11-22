"use strict";

const siteManagementAssignSiteSetting = (function ($) {
    let assignSiteSettingForm = null,
        inlineMessage = null,
        sites = null,
        currencies = null,
        platforms = null,
        ddlSportType = null,
        ddlPolicyTitleBox = null,
        ddlPolicyTitle = null,
        dynamicSiteCurrencyControl = null,
        index = null,
        reloadSiteGridFunc = null;

    function init(siteList, currencyList, platformList, reloadSiteGrid) {
        sites = siteList;
        currencies = currencyList;
        platforms = platformList;
        reloadSiteGridFunc = reloadSiteGrid;
        assignSiteSettingForm = $("#assignSiteSettingForm");
    }

    function openModal() {
        CMS.modal(
            resources.assignSiteSettingPopupTitle,
            assignSiteSettingForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

        cacheElements();
        bindEvents();

        index = 0;
        renderDynamicSiteCurrencyControl(++index);
    }

    function cacheElements() {
        inlineMessage = $("#inlineMessageAssignSiteSetting");
        ddlSportType = $("#ddlSportType").combobox();
        ddlPolicyTitleBox = $("#ddlPopupPolicyTitleBox");
        ddlPolicyTitle = $("#ddlPopupPolicyTitle").combobox();
        dynamicSiteCurrencyControl = $("#dynamicSiteCurrencyControl");
    }

    function bindEvents() {
        ddlSportType
            .off("change")
            .change(function () {
                renderPolicyTitles(this.value);
            });
        $("#btnAddMore")
            .off("click")
            .click(function () {
                clearAllTipsyErrors();
                renderDynamicSiteCurrencyControl(++index);
            });
        $("#btnSave")
            .off("click")
            .click(function () { handleOnSave(); });
        $("#btnCancel").off("click").click(CMS.closePopup);
    }

    function renderPolicyTitles(sportType) {
        if (sportType === "0") {
            renderPolicyTitleDropdownList([]);
        } else {
            CMS.getJson(
                CMS.resolveRootUrl(`Policy/GetPolicyTitle?sportType=${sportType}`),
                function (data) {
                    renderPolicyTitleDropdownList($.parseJSON(data));
                });
        }
    }

    function renderPolicyTitleDropdownList(policyTitles) {
        let html = ['<select class="width-390" id="ddlPopupPolicyTitle"><option value="0">Please select</option>'];
        policyTitles.forEach(function (item) {
            html.push('<option value="', item.Id, '">', item.Name, '</option>');
        });

        html.push('</select>');

        ddlPolicyTitleBox.html(html.join(""));
        ddlPolicyTitle = $("#ddlPopupPolicyTitle").combobox();
    }

    function renderDynamicSiteCurrencyControl(id) {
        dynamicSiteCurrencyControl.append(renderSiteCurrencySetting(id));
        bindEventsAfterRenderSiteCurrencySetting(id);
    }

    function renderSiteCurrencySetting(id) {
        var html = [
            '<div class="form-row row-site-currency-setting" data-randomid="', id, '">',
            '<div class="lbl-field"><label>', resources.site, ' <span class="text-red">*</span></label></div>',
            `<div class="edit-field"><div id="ddlPopupSite-${id}"></div></div>`,
            '<div class="lbl-field"><label>', resources.currency, ' <span class="text-red">*</span></label></div>',
            `<div class="edit-field"><div id="ddlCurrency-${id}"></div></div>`,
            '<div class="lbl-field"><label>', resources.platform, ' <span class="text-red">*</span></label></div>',
            `<div class="edit-field"><div id="ddlPopupPlatform-${id}"></div></div>`
        ];

        if (id !== 1) {
            html.push('<div class="lbl-field width-30"><span class="ico-delete" id="icoDelete-', id, '" title="Delete"></span></div>');
        }

        html.push('</div>');
        return html.join("");
    }

    function bindEventsAfterRenderSiteCurrencySetting(id) {
        customerClassHelper.renderMultiSelect(
            `#ddlPopupSite-${id}`,
            {
                minWidth: 158,
                maxHeight: 180,
                showFilter: true,
                showTooltip: false,
                isCheckAll: false,
                listData: sites,
                onCheck: function () {
                    this.checked && clearAllTipsyErrors();
                }
            });
        customerClassHelper.renderMultiSelect(
            `#ddlCurrency-${id}`,
            {
                minWidth: 158,
                maxHeight: 180,
                showFilter: true,
                showTooltip: false,
                isCheckAll: false,
                listData: currencies,
                onCheck: function () {
                    this.checked && clearAllTipsyErrors();
                }
            });
        customerClassHelper.renderMultiSelect(
            `#ddlPopupPlatform-${id}`,
            {
                minWidth: 158,
                maxHeight: 180,
                showTooltip: false,
                isCheckAll: false,
                listData: platforms,
                onCheck: function () {
                    this.checked && clearAllTipsyErrors();
                }
            });
        $(`#icoDelete-${id}`).click(function () {
            clearAllTipsyErrors();
            $(this).closest(".row-site-currency-setting").remove();
        });
    }

    function handleOnSave(formData, isOverwriteAssign = false) {
        formData = formData || getFormData();
        if (formData) {
            CMS.disableInput();
            CMS.callAjax(
                this,
                CMS.resolveRootUrl(`Site/Assign?isOverwrite=${isOverwriteAssign}`),
                formData,
                function (result) {
                    const resultJson = JSON.parse(result);
                    if (Array.isArray(resultJson)) {
                        CMS.showConfirmMessage(buildConfirmAssignSiteSettingMessage(resultJson), function () {
                            formData.isCheck = false;
                            handleOnSave(formData, isOverwriteAssign);
                        });

                        $("#ckbAssignOverwrite").change(function () {
                            isOverwriteAssign = $(this).prop("checked");
                        });
                    } else {
                        customerClassHelper.showInlineMessageAndClosePopup(resultJson, inlineMessage, reloadSiteGridFunc);
                    }
                });
        }
    }

    function buildConfirmAssignSiteSettingMessage(siteSettings) {
        let html = ['<p>Setting info already exists</p>'];

        siteSettings.forEach(function (item) {
            html.push('<p class="existed-site-setting">- ', resources.policyTitle, ": <b>", item.PolicyTitle, "</b>; ");
            html.push(resources.site, ": <b>", item.Site, "</b>; ");
            html.push(resources.currency, ": <b>", item.Currencies.split(",").join(", "), "</b>; ");
            html.push(resources.platform, ": <b>", item.PlatformNames, "</b></p>");
        });

        html.push(`<div><input type="checkbox" id="ckbAssignOverwrite" /><label for="ckbAssignOverwrite">${resources.warningOverwrite}</label></div>`);

        return html.join("");
    }

    function getFormData() {
        const policyId = validatePolicyTitle();
        const siteCurrencies = validateDynamicSiteCurrencies();

        if (policyId && siteCurrencies.length) {
            return { policyId, siteCurrencies };
        }
    }

    function validatePolicyTitle() {
        const policyTitle = ddlPolicyTitle.val();
        if (policyTitle === "0") {
            CMS.showTipsyError(ddlPolicyTitle.next("#ddlPopupPolicyTitle"), resources.requiredField, "w");
            return;
        }

        CMS.clearTipsyError(ddlPolicyTitle);
        return policyTitle;
    }

    function validateDynamicSiteCurrencies() {
        let isValid = true;
        let siteCurrencies = [];
        $(".row-site-currency-setting").each(function () {
            const $this = $(this);
            const id = $this.data("randomid");

            const ddlSite = $this.find(`#ddlPopupSite-${id}`);
            const sites = validateSiteMultiSelect(ddlSite);

            const ddlCurrency = $this.find(`#ddlCurrency-${id}`);
            const currencies = validateMultiSelect(ddlCurrency);

            const ddlPlatform = $this.find(`#ddlPopupPlatform-${id}`);
            const platforms = validateMultiSelect(ddlPlatform);

            if (sites && currencies && platforms) {
                siteCurrencies.push({ sites, currencies, platforms });
            } else {
                isValid = false;
            }
        });

        return isValid ? siteCurrencies : [];
    }

    function validateSiteMultiSelect(element) {
        const selectedValues = element.fanexMultiSelect("getSelectedTexts", true);
        var allSitesLength = sites[0].items.length + sites[1].items.length;
        if (selectedValues.length === allSitesLength) {
            return "All";
        }
        if (selectedValues.length) {
            var selectedCreditSite = sites[0].items.filter(s => selectedValues.includes(s.text)).map(s => s.text);
            var selectedLicenseeSite = sites[1].items.filter(s => selectedValues.includes(s.text)).map(s => s.text);
            var result = [];
            if (selectedCreditSite.length === sites[0].items.length) {
                result.push("AllCredit");
                result = result.concat(selectedLicenseeSite);
            } else if (selectedLicenseeSite.length === sites[1].items.length) {
                result = selectedCreditSite;
                result.push("AllDeposit");
            } else {
                result = selectedValues;
            }

            return result.join();
        }

        CMS.showTipsyError(element, resources.requiredField, "ne");
    }

    function validateMultiSelect(element) {
        const selectedValues = element.fanexMultiSelect("getItems", true);
        if (selectedValues.length === currencyList.length) {
            return "0";
        }
        if (selectedValues.length) {
            return selectedValues.join();
        }

        CMS.showTipsyError(element, resources.requiredField, "ne");
    }

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);
