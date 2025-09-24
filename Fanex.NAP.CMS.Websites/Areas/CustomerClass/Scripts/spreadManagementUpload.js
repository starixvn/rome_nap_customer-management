"use strict";

const spreadManagementUpload = (function ($) {
    const MaxFileSize = 1024 * 1024 * 5;
    let uploadForm = null,
        inlineMessage = null,
        txtExcelFile = null,
        btnUpload = null,
        tblErrorTitleBox = null,
        tblErrorTitle = null,
        btnExportExcel = null,
        tblError = null,
        tblErrorBox = null,
        tblValidationResult = null,
        tblValidationResultText = null,
        btnProceedUploadCancel = null,
        btnProceedUploadSubmit = null,
        dataUploadProceed = null,
        dataExportProceed = null,
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
        tblErrorTitleBox = $("#tblErrorTitleBox");
        tblErrorTitle = $("#tblErrorTitle");
        btnExportExcel = $("#btnExportExcel");
        tblError = $("#tblError");
        tblErrorBox = $("#tblErrorBox");
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

        btnExportExcel.click(function () {
            handleExportUploadInvalidData(dataExportProceed);
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
        tblErrorTitleBox.addClass("display-none");
        tblErrorTitle.html("");
        tblErrorBox.addClass("display-none");
        tblValidationResult.addClass("display-none");
    }

    function handleUploadValidateFile(file) {
        CMS.disableInput();

        let isUploading = false;
        dataUploadProceed = null;
        dataExportProceed = null;
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

                    dataExportProceed = resultJson;
                } else {
                    if (resultJson.SpreadInvalids && resultJson.SpreadInvalids.length > 0) {
                        btnUpload.prop("disabled", true);
                        renderErrorGrid(resultJson.SpreadInvalids);
                        renderUploadProceedResult(resultJson);

                        dataUploadProceed = resultJson.SpreadValids;
                        dataExportProceed = resultJson.SpreadInvalids;
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
            CMS.disableInput();

            const jqxhr = $.ajax({
                url: CMS.resolveRootUrl(`Spread/UploadSubmit`),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                processData: false,
                data: JSON.stringify(spreadValids),
                success: function (result) {
                    const resultJson = JSON.parse(result);
                    customerClassHelper.showInlineMessageAndClosePopup(resultJson, inlineMessage, reloadSpreadGridFunc, 1000);
                    jqxhr.always(CMS.enableInput);
                },
                error: function () {
                    CMS.handleAjaxError();
                    jqxhr.always(CMS.enableInput);
                }
            });
        }
    }

    function handleExportUploadInvalidData(spreadInvalids)
    {
        if (spreadInvalids)
        {
            CMS.downloadFileWithPost(CMS.resolveRootUrl("Spread/ExportUploadInvalid"), spreadInvalids);
        }
    }

    function renderErrorGrid(errors) {
        tblErrorTitleBox.removeClass("display-none");
        tblErrorTitle.html(`${resources.totalErrors}: ${errors.length}`);
        tblErrorBox.removeClass("display-none");
        tblError.css({ visibility: 'visible', position: 'relative' });

        if ($.fn.DataTable.isDataTable(tblError)) {
            var table = tblError.DataTable();
            table.clear();
            table.rows.add(errors);
            table.draw(false);
        } else {
            $.fn.dataTable.ext.errMode = 'none';
            tblError.DataTable({
                data: errors,
                columns: [
                    { data: 'ID', className: "align-right", width: "60px" },
                    { data: 'PolicyTitle', className: "align-left", width: "100px" },
                    { data: 'CustomerClass', className: "align-right", width: "100px" },
                    { data: 'SportType', className: "align-left", width: "100px" },
                    { data: 'Site', className: "align-left", width: "100px" },
                    { data: 'Currency', className: "align-left", width: "100px" },
                    { data: 'Platform', className: "align-left", width: "100px" },
                    { data: 'Spread', className: "align-right", width: "100px" },
                    { data: 'Reason', className: "align-left", width: "160px" }
                ],
                scrollY: 400,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                searching: false,
                info: false,
                ordering: false,
                autoWidth: false,
                destroy: true
            });
        }
    }

    function renderUploadProceedResult(result) {
        tblValidationResultText.html(`Validation result: ${result.SpreadInvalids.length} invalid rows, ${result.SpreadValids.length} valid rows. Proceed with submission of valid rows?`)

        tblValidationResult.removeClass("display-none");
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);
