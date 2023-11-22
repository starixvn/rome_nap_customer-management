"use strict";

const siteManagementUpload = (function ($) {
    const MaxFileSize = 1024 * 1024 * 5;
    let uploadForm = null,
        inlineMessage = null,
        txtExcelFile = null,
        btnUpload = null,
        tblErrorTitle = null,
        tblError = null,
        reloadSiteGridFunc = null;

    function init(reloadSiteGrid) {
        uploadForm = $("#uploadForm");
        reloadSiteGridFunc = reloadSiteGrid;
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
            fileControl && handleUpload(fileControl.files[0], $("#ckbExcelOverwrite").prop("checked"));
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

    function handleUpload(file, isOverwrite) {
        CMS.disableInput();

        let formData = new FormData();
        formData.append("File", file);

        const jqxhr = $.ajax({
            url: CMS.resolveRootUrl(`Site/Upload?isOverwrite=${isOverwrite}`),
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
                    customerClassHelper.showInlineMessageAndClosePopup(resultJson, inlineMessage, reloadSiteGridFunc);
                }
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    function renderErrorGrid(errors) {
        const html = errors.map(function (item, index) {
            return [
                '<tr>',
                '<td class="align-center">', index + 1, '</td>',
                '<td class="align-center">', item.Cell, '</td>',
                '<td>', item.ColumnName, '</td>',
                '<td>', item.Value, '</td>',
                '<td>', item.Reason, '</td>',
                '</tr>'
            ].join("");
        });

        tblErrorTitle.html(`${resources.totalErrors}: ${errors.length}`).removeClass("display-none");
        tblError.find("tbody").html(html.join(""));
        tblError.removeClass("display-none");
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);
