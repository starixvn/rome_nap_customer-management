"use strict";

const spreadManagementUpload = (function ($) {
    const MaxFileSize = 1024 * 1024 * 5;
    let uploadForm = null,
        inlineMessage = null,
        txtExcelFile = null,
        btnUpload = null,
        tblErrorTitle = null,
        tblError = null,
        tblValidationResult = null,
        tblValidationResultText = null,
        btnProceedUploadCancel = null,
        btnProceedUploadSubmit = null,
        dataUploadProceed = null,
        reloadSpreadGridFunc = null;

    function init(reloadSpreadGrid) {
        uploadForm = $("#uploadForm");
        reloadSpreadGridFunc = reloadSpreadGrid;
    }

    function openModal() {
        CMS.modal(
            resources.uploadSiteSetting,
            uploadForm.clone(),
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
    }

    function cacheElements() {
        inlineMessage = $("#inlineMessageUpload");
        txtExcelFile = $("#txtExcelFile");
        btnUpload = $("#btnPopupUpload");
        tblErrorTitle = $("#tblErrorTitle");
        tblError = $("#tblError");
        tblValidationResult = $("#validationResult");
        tblValidationResultText = $("#validationResultText");
        btnProceedUploadCancel = $("#btnProceedUploadCancel");
        btnProceedUploadSubmit = $("#btnProceedUploadSubmit");
    }

    function bindEvents() {
        let fileControl;

        $("#file")
            .change(function () {
                fileControl = this;
                const isValidFile = validateUploadedFile(this);
                btnUpload.prop("disabled", !isValidFile);
            })
            .click(function () {
                this.value = null;
                resetUIState();
            });

        btnUpload.click(function () {
            resetErrorGrid();
            fileControl && handleUploadValidateFile(fileControl.files[0]);
        });

        btnProceedUploadCancel.click(function () {
            CMS.closePopup();
        });

        btnProceedUploadSubmit.click(function () {
            handleUploadSubmit(dataUploadProceed);
        });
    }

    function validateUploadedFile(fileControl) {
        CMS.clearTipsyError(txtExcelFile);
        const fileName = getFileName(fileControl.value);

        if (!fileName || !fileName.length) {
            CMS.showTipsyError(txtExcelFile, resources.requiredField, "ne");
            return false;
        }

        txtExcelFile.val(fileName);
        const fileExtension = getFileExtension(fileName);

        if (fileExtension !== "xls" && fileExtension !== "xlsx") {
            CMS.showTipsyError(txtExcelFile, resources.notExcelFileError, "ne");
            return false;
        }

        if (fileControl.files[0].size > MaxFileSize) {
            CMS.showTipsyError(txtExcelFile, resources.Error_ExcessMaxFileSize, "ne");
            return false;
        }

        return true;
    }

    function getFileName(path) {
        return path.replace(/^.*[\\\/]/, "");
    }

    function getFileExtension(fileName) {
        return /[.]/.exec(fileName) ? /[^.]+$/.exec(fileName)[0] : "";
    }

    function resetUIState() {
        txtExcelFile.val("");
        btnUpload.prop("disabled", true);
        resetErrorGrid();
    }

    function resetErrorGrid() {
        tblErrorTitle.addClass("display-none").html("");
        tblError.addClass("display-none").find("tbody").html("");
    }

    function handleUploadValidateFile(file) {
        CMS.disableInput();

        let isUploading = false;
        dataUploadProceed = null;
        tblValidationResult.addClass("display-none");

        let formData = new FormData();
        formData.append("File", file);

        const jqxhr = $.ajax({
            url: CMS.resolveRootUrl(`Spread/UploadValidate`),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                const resultJson = JSON.parse(result);
                if (Array.isArray(resultJson)) {
                    btnUpload.prop("disabled", true);
                    renderErrorGrid(resultJson);
                } else {
                    if (resultJson.SpreadInvalids && resultJson.SpreadInvalids.length > 0) {
                        btnUpload.prop("disabled", true);
                        renderErrorGrid(resultJson.SpreadInvalids);
                        renderUploadProceedResult(resultJson);

                        dataUploadProceed = resultJson.SpreadValids;
                    }
                    else {
                        isUploading = true;

                        handleUploadSubmit(resultJson.SpreadValids);
                    }
                }

                if (!isUploading) {
                    jqxhr.always(CMS.enableInput);
                }
            },
            error: function () {
                CMS.handleAjaxError();
                jqxhr.always(CMS.enableInput);
            }
        });
    }

    function handleUploadSubmit(spreadValids) {
        if (spreadValids) {
            const jqxhr = $.ajax({
                url: CMS.resolveRootUrl(`Spread/UploadSubmit`),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                processData: false,
                data: JSON.stringify(spreadValids),
                success: function (result) {
                    const resultJson = JSON.parse(result);
                    customerClassHelper.showInlineMessageAndClosePopup(resultJson, inlineMessage, reloadSpreadGridFunc, 5000);
                    jqxhr.always(CMS.enableInput);
                },
                error: function () {
                    CMS.handleAjaxError();
                    jqxhr.always(CMS.enableInput);
                }
            });
        }
    }

    function renderErrorGrid(errors) {
        const html = errors.map(function (item, index) {
            return [
                '<tr>',
                '<td class="align-right">', item.ID, '</td>',
                '<td class="align-left">', item.PolicyTitle, '</td>',
                '<td class="align-right">', item.CustomerClass, '</td>',
                '<td class="align-left">', item.SportType, '</td>',
                '<td class="align-left">', item.Site, '</td>',
                '<td class="align-left">', item.Currency, '</td>',
                '<td class="align-left">', item.Platform, '</td>',
                '<td class="align-right">', item.Spread, '</td>',
                '<td class="align-left">', item.Reason, '</td>',
                '</tr>'
            ].join("");
        });

        tblErrorTitle.html(`${resources.totalErrors}: ${errors.length}`).removeClass("display-none");
        tblError.find("tbody").html(html.join(""));
        tblError.removeClass("display-none");
    }

    function renderUploadProceedResult(result) {
        tblValidationResultText.html(`Validation result: ${result.SpreadInvalids.length} invalid rows, ${result.SpreadValids.length} valid rows. Proceed with submission off valid rows?`)

        tblValidationResult.removeClass("display-none");
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);
