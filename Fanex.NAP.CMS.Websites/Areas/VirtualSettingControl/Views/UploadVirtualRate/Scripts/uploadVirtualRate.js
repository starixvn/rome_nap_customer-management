"use strict";

function UploadedFile(fileControl, $textboxFile) {
    var MaxFileSize = 1024 * 1024 * 5;
    this.fileControl = fileControl;
    this.$textboxFile = $textboxFile;

    this.validate = function () {
        CMS.clearTipsyError(this.$textboxFile);
        var fileName = getFileName(this.fileControl.value);

        if (!fileName || !fileName.length) {
            CMS.showTipsyError(this.$textboxFile, resources.Error_RequiredField, "sw");
            return false;
        }

        this.$textboxFile.val(fileName);
        var fileExtension = getFileExtension(fileName);

        if (fileExtension !== "xls" && fileExtension !== "xlsx") {
            CMS.showTipsyError(this.$textboxFile, resources.Error_NotExcelFile, "sw");
            return false;
        }

        if (this.fileControl.files[0].size > MaxFileSize) {
            CMS.showTipsyError(this.$textboxFile, resources.Error_ExcessMaxFileSize, "sw");
            return false;
        }

        return true;
    };

    this.getUploadedFile = function () {
        return this.fileControl.files[0];
    };

    function getFileName(path) {
        return path.replace(/^.*[\\\/]/, "");
    }

    function getFileExtension(fileName) {
        return /[.]/.exec(fileName) ? /[^.]+$/.exec(fileName)[0] : "";
    }
}

function VerifyExcelTemplate() {
    var gridOptions = null;
    var divExcelError = $("#excelError");
    var spanValidSetting = $("#valid-setting");

    function getGridOptions() {
        var headerColumns = [
            {
                HeaderText: "#",
                Width: 60,
                Name: "colNo",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: resources.ExcelZone,
                Width: 100,
                Name: "colExcelZone",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: resources.Rule,
                Width: 200,
                Name: "colRule",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: resources.Reason,
                Width: 250,
                Name: "colReason",
                HeaderAlign: "center",
                CellAlign: "left"
            }];

        return {
            columns: headerColumns,
            showTitle: false,
            bodyRows: [],
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: false,
            gridExpandHeight: 230
        };
    }

    function renderGrid(data) {
        var bodyData = [];

        var JSONData = JSON.parse(data);
        var dataLength = JSONData.length;

        for (var i = 0; i < dataLength; i++) {
            var error = JSONData[i];

            bodyData.push({
                Columns: [
                    { Name: "colNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "colExcelZone", Value: error.CellId },
                    { Name: "colRule", Value: error.Rule },
                    { Name: "colReason", Value: error.Reason }]
            });
        }

        gridOptions = gridOptions || getGridOptions();
        gridOptions.bodyRows = bodyData;

        divExcelError.removeClass("display-none").fanexGrid(gridOptions);
        spanValidSetting.text("N/A");
    }

    this.verify = function (file) {
        CMS.disableInput();

        var formData = new FormData();
        formData.append("File", file);

        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/Verify"),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: renderGrid,
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    };
}

(function ($) {
    $(function () {
        var buttonVerify = $("#btn-verify");
        var buttonUpdate = $("#btn-update");

        var uploadedFileInstance = null;
        var verifyExcel = new VerifyExcelTemplate();

        $("#file")
            .change(function () {
                uploadedFileInstance = uploadedFileInstance || new UploadedFile(this, $("#txt-excelfile"));
                var isValidFile = uploadedFileInstance.validate();
                buttonVerify.prop("disabled", !isValidFile);
            })
            .click(function () {
                this.value = null;
            });

        buttonVerify.click(function () {
            uploadedFileInstance && verifyExcel.verify(uploadedFileInstance.getUploadedFile());
        });

        buttonUpdate.click(function () {

        });
    });
})(jQuery);