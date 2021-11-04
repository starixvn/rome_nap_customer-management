"use strict";
function ManageLimit(div, postUrl, reloadData) {
    var maxInputSMAs = 500;
    var formAddNew = null,
        ddlPopupType = null,
        txtSMA = null,
        ddlPopupSite = null,
        ddlPopupCurrency = null,
        ddlPopupProduct = null,
        spanPopupSite = null,
        spanPopupCurrency = null,
        fieldPopupType = null,
        fieldPopupSMA = null,
        fieldPopupSite = null,
        fieldPopupCurrency = null,
        fieldPopupProduct = null,
        buttonCancel = null,
        buttonSave = null,
        divInlineMessage = null;

    function cacheElements() {
        formAddNew = div.find("#addNew");
        divInlineMessage = div.find("#inline-message");

        ddlPopupType = div.find("#ddlPopup-type");
        txtSMA = div.find("#txt-sma");
        ddlPopupSite = div.find("#ddlPopup-site");
        ddlPopupCurrency = div.find("#ddlPopup-currency");
        ddlPopupProduct = div.find('#ddlPopup-product');

        fieldPopupType = getGrandParent(ddlPopupType);
        fieldPopupSMA = getGrandParent(txtSMA);
        fieldPopupSite = getGrandParent(ddlPopupSite);
        fieldPopupCurrency = getGrandParent(ddlPopupCurrency);
        fieldPopupProduct = getGrandParent(ddlPopupProduct);

        buttonCancel = div.find("#btn-cancel");
        buttonSave = div.find("#btn-save");
    }

    function getGrandParent(element) {
        return element.parent().parent();
    }

    function bindEvents() {
        renderAndCacheComboboxs();
        renderMultiSelectProduct();

        showInputsWhenSMALineIsSelected();

        ddlPopupType.change(function () {
            clearErrorTips();

            switch ($(this).val()) {
                case '1':
                    showInputsWhenSMALineIsSelected();
                    break;
                case '2':
                    showInputsWhenSiteIsSelected();
                    break;
                case '3':
                    showInputsWhenCurrencyIsSelected();
                    break;
                case '4':
                    showInputsWhenSiteCurrencyIsSelected();
                    break;
                default:
                    break
            }
        });

        txtSMA.off("change").on("change", function () {
            var smasValue = $.trim(txtSMA.val());

            if (smasValue.length > 0) {
                if (smasValue.indexOf("\n") >= 0) {
                    smasValue = smasValue.replace(/\n/g, " ");
                }

                if (smasValue.indexOf("\r") >= 0) {
                    smasValue = smasValue.replace(/\r/g, " ");
                }

                if (smasValue.indexOf("\t") >= 0) {
                    smasValue = smasValue.replace(/\t/g, " ");
                }

                smasValue = smasValue.replace(/\s/g, ",");
                var smasArray = smasValue.split(",").filter(item => item.length > 0);

                txtSMA.val(smasArray.join(","));
            }
        });
        buttonCancel.off("click").on("click", CMS.closePopup);
        buttonSave.off("click").on("click", function () {
            var formData = {};

            formData.type = ddlPopupType.val();

            switch (formData.type) {
                case '1':
                    var inputSMAs = getInputSMAs();
                    if (inputSMAs.length === 0) return;
                    formData.SMAs = inputSMAs;
                    break;
                case '2':
                    var selectedSite = getSelectedSite();
                    if (selectedSite === "") return;
                    formData.site = selectedSite;
                    break;
                case '3':
                    var selectedCurrency = getSelectedCurrency();
                    if (selectedCurrency === "") return;
                    formData.currency = selectedCurrency;
                    break;
                case '4':
                    var selectedSite = getSelectedSite();
                    var selectedCurrency = getSelectedCurrency();
                    if (selectedSite === "" || selectedCurrency === "") return;
                    formData.site = selectedSite;
                    formData.currency = selectedCurrency;
                    break;
                default:
                    break;
            }

            var selectedProducts = getSelectedProducts();
            if (selectedProducts === "") return;
            formData.product = selectedProducts;

            submitForm(formData);
        });
    }

    function renderAndCacheComboboxs() {
        ddlPopupType.combobox();
        ddlPopupSite.combobox();
        ddlPopupCurrency.combobox();

        spanPopupSite = div.find('span[id="ddlPopup-site"]');
        spanPopupCurrency = div.find('span[id="ddlPopup-currency"]');
    }

    function renderMultiSelectProduct() {
        ddlPopupProduct.fanexMultiSelect({
            minWidth: 180,
            checkAllText: "All",
            listData: ddlPopupProduct.find("option").map(function () {
                return {
                    id: this.value,
                    value: this.value,
                    text: this.text,
                    checked: true,
                    enable: true
                };
            }),
        });
        ddlPopupProduct = div.find('#ddlPopup-product');
    }

    function clearErrorTips() {
        CMS.clearTipsyError(txtSMA);
        CMS.clearTipsyError(spanPopupSite);
        CMS.clearTipsyError(spanPopupCurrency);
        CMS.clearTipsyError(ddlPopupProduct);
    }

    function showInputsWhenSMALineIsSelected() {
        fieldPopupSMA.show();
        fieldPopupSite.hide();
        fieldPopupCurrency.hide();
    }

    function showInputsWhenSiteIsSelected() {
        ddlPopupSite.val("0").combobox("refresh");
        fieldPopupSite.show();
        fieldPopupSMA.hide();
        fieldPopupCurrency.hide();
    }

    function showInputsWhenCurrencyIsSelected() {
        ddlPopupCurrency.val("0").combobox("refresh");
        fieldPopupCurrency.show();
        fieldPopupSMA.hide();
        fieldPopupSite.hide();
    }

    function showInputsWhenSiteCurrencyIsSelected() {
        ddlPopupSite.val("0").combobox("refresh");
        ddlPopupCurrency.val("0").combobox("refresh");
        fieldPopupSite.show();
        fieldPopupCurrency.show();
        fieldPopupSMA.hide();
    }

    function getInputSMAs() {
        var inputSMAs = $.trim(txtSMA.val());

        if (inputSMAs.length === 0) {
            CMS.showTipsyError(txtSMA, resources.smaUsernameIsRequired , "se");
            return "";
        }

        // Check usernames contains '\'.
        if (inputSMAs.indexOf("\\") >= 0) {
            CMS.showTipsyError(txtSMA, resources.inputInvalidSMAsError.replace("{0}", '\\'), "se");
            return "";
        }

        // Check max input
        var smasArray = inputSMAs.split(",");
        if (smasArray.length > maxInputSMAs) {
            CMS.showTipsyError(txtSMA, resources.overSMAsLimitationError.replace("{0}", maxInputSMAs), "se");
            return "";
        }

        // Check duplicate
        var smasDuplicateArray = smasArray.filter(function (item, index) {
            return smasArray.indexOf(item) != index;
        });
        if (smasDuplicateArray.length > 0) {
            CMS.showTipsyError(txtSMA, resources.duplicatedAccountsError.replace("{0}", smasDuplicateArray.join(',')), "se");
            return "";
        }
        
        CMS.clearTipsyError(txtSMA);
        return inputSMAs;
    }

    function getSelectedSite() {
        var selectedSite = ddlPopupSite.val();

        if (selectedSite === '0') {
            CMS.showTipsyError(spanPopupSite, resources.siteIsRequired, "w");
            return "";
        }

        CMS.clearTipsyError(spanPopupSite);
        return selectedSite;
    }

    function getSelectedCurrency() {
        var selectedCurrency = ddlPopupCurrency.val();

        if (selectedCurrency === '0') {
            CMS.showTipsyError(spanPopupCurrency, resources.currencyIsRequired, "w");
            return "";
        }

        CMS.clearTipsyError(spanPopupCurrency);
        return selectedCurrency;
    }

    function getSelectedProducts() {
        var selectedProducts = ddlPopupProduct.fanexMultiSelect("getItems", true).join(",");

        if (selectedProducts === '') {
            CMS.showTipsyError(ddlPopupProduct, resources.productIsRequired, "w");
            return "";
        }

        CMS.clearTipsyError(ddlPopupProduct);
        return selectedProducts;
    }

    function submitForm(formData) {
        CMS.disableInput();
        $.post(postUrl, formData, successHandler, "json")
            .fail(CMS.handleAjaxError)
            .always(CMS.enableInput);
    }

    function successHandler(data) {
        var jsonData = $.parseJSON(data);
        if (jsonData.IsSuccess) {
            CMS.showInlineSuccessMsg(
                jsonData.Message,
                divInlineMessage,
                true,
                function () {
                    CMS.closePopup();
                    reloadData();
                });
        } else {
            CMS.showInlineErrorMsg(jsonData.Message, divInlineMessage);
        }
    }

    this.ShowAddLimitPopup = function (modalTitle) {
        CMS.modal(modalTitle, div, { resizeable: false });

        cacheElements();
        bindEvents();
    }

    this.RemoveLimit = function (data) {
        CMS.showConfirmMessage(resources.removeLimitConfirmationMessage, function () {
            CMS.callAjax(this, 'Remove', data, function (result) {
                var jsonData = $.parseJSON(result);
                if (jsonData.Code === 0) {
                    CMS.showSuccessMessage(jsonData.Message);
                    reloadData();
                } else {
                    CMS.showErrorMessage(jsonData.Message);
                }
            });
        });
    }
}