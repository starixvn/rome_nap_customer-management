"use strict";
(function ($) {
    var MaxFileSize = 1024 * 1024 * 5;
    var textboxFile = $("#txt-excelfile");

    var buttonVerify = $("#btn-verify");
    var gridOptions = null;
    var divExcelError = $("#excelError");
    var spanValidSetting = $("#valid-setting");

    var buttonUpdate = $("#btn-update");

    function validateUploadedFile(fileControl) {
        CMS.clearTipsyError(textboxFile);
        var fileName = getFileName(fileControl.value);

        if (!fileName || !fileName.length) {
            CMS.showTipsyError(textboxFile, resources.Error_RequiredField, "sw");
            return false;
        }

        textboxFile.val(fileName);
        var fileExtension = getFileExtension(fileName);

        if (fileExtension !== "xls" && fileExtension !== "xlsx") {
            CMS.showTipsyError(textboxFile, resources.Error_NotExcelFile, "sw");
            return false;
        }

        if (fileControl.files[0].size > MaxFileSize) {
            CMS.showTipsyError(textboxFile, resources.Error_ExcessMaxFileSize, "sw");
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

    function verifyUploadedFile(file) {
        CMS.disableInput();

        var formData = new FormData();
        formData.append("File", file);

        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/Verify"),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: renderErrorSetting,
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    function renderErrorSetting(data) {
        var bodyData = [];

        var JSONData = JSON.parse(data);
        var excelErrors = JSONData.ExcelErrors;
        var dataLength = excelErrors.length;

        for (var i = 0; i < dataLength; i++) {
            var error = excelErrors[i];

            bodyData.push({
                Columns: [
                    { Name: "colNo", Value: numeral(i + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "colExcelCell", Value: error.CellId },
                    { Name: "colAccount", Value: error.Username },
                    { Name: "colValue", Value: error.Value },
                    { Name: "colReason", Value: error.Reason }]
            });
        }

        gridOptions = gridOptions || getGridOptions();
        gridOptions.titleInfo.text = JSONData.Title;
        gridOptions.bodyRows = bodyData;

        divExcelError.removeClass("display-none").fanexGrid(gridOptions);
        spanValidSetting.text(JSONData.IsFormatError ? "N/A" : JSONData.ValidSetting + "/" + JSONData.TotalRows);
        buttonUpdate.prop("disabled", !JSONData.ValidSetting);
    }

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
                HeaderText: resources.ExcelCell,
                Width: 100,
                Name: "colExcelCell",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: resources.Account,
                Width: 200,
                Name: "colAccount",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: resources.Value,
                Width: 200,
                Name: "colValue",
                HeaderAlign: "center",
                CellAlign: "left"
            },
            {
                HeaderText: resources.Reason,
                Width: 300,
                Name: "colReason",
                HeaderAlign: "center",
                CellAlign: "left"
            }];

        return {
            columns: headerColumns,
            showTitle: true,
            titleInfo: {
                text: ""
            },
            bodyRows: [],
            fullWidth: true,
            fullHeight: true,
            gridHeight: 0,
            showDistinctRow: false,
            headerColumnHeight: 29,
            bodyRowHeight: 25,
            showPager: false,
            gridExpandHeight: 250
        };
    }

    function reset() {
        textboxFile.val("");
        buttonVerify.prop("disabled", true);
        spanValidSetting.text("N/A");
        buttonUpdate.prop("disabled", true);
        divExcelError.addClass("display-none");
        $("#excelError-grid-title").addClass("display-none");
    }

    function uploadFile() {
        CMS.disableInput();
        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/Update"),
            type: "POST",
            success: function (data) {
                var JSONData = JSON.parse(data);
                if (JSONData.Code === 0) {
                    CMS.showSuccessMessage(JSONData.Message, reset);
                } else {
                    CMS.showErrorMessage(JSONData.Message);
                }
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    $(function () {
        var fileControl;

        $("#file")
            .change(function () {
                fileControl = this;
                var isValidFile = validateUploadedFile(this);
                buttonVerify.prop("disabled", !isValidFile);
            })
            .click(function () {
                this.value = null;
                reset();
            });

        buttonVerify.click(function () {
            fileControl && verifyUploadedFile(fileControl.files[0]);
        });

        buttonUpdate.click(uploadFile);
    });
})(jQuery);