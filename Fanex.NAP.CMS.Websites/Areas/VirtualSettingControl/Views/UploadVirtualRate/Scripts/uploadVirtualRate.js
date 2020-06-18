"use strict";
(function ($) {
    var MaxFileSize = 1024 * 1024 * 5;
    var textboxFile = $("#txt-excelfile");

    var buttonVerify = $("#btn-verify");
    var gridOptions = null;
    var freegrid = $("#freegrid");
    var spanValidSetting = $("#valid-setting");

    var progressbar = $("#progressbar");
    var progressLabel = $("#progressLabel");
    var progressbarTimeId = null;
    var buttonUpdate = $("#btn-update");
    var sportTypeId = $("#txt-sporttype").val();

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

    function verifyByWeb(file) {
        CMS.disableInput();

        var formData = new FormData();
        formData.append("File", file);
        formData.append("sportType", sportTypeId);

        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/VerifyByWeb"),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                verifyByWebSuccessHandler(data, formData);
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    function verifyByWebSuccessHandler(data, formData) {
        var JSONData = JSON.parse(data);

        if (JSONData.Message) {
            CMS.showErrorMessage(JSONData.Message);
            return;
        }

        if (JSONData.ExcelErrors.length) {
            renderGrid(JSONData.ExcelErrors, JSONData.Title);
            spanValidSetting.text("N/A");
        } else {
            verifyByDB(formData, JSONData.TotalRows);
        }
    }

    function verifyByDB(formData, totalRows) {
        buttonVerify.prop("disabled", true);
        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/VerifyByDB"),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: verifyByDBSuccessHandler,
            error: CMS.handleAjaxError
        });

        jqxhr.always(function () {
            hideProgressBar();
            buttonVerify.prop("disabled", false);
        });

        showProgressBar(totalRows);
    }

    function verifyByDBSuccessHandler(data) {
        hideProgressBar();
        var JSONData = JSON.parse(data);

        if (JSONData.Message) {
            CMS.showErrorMessage(JSONData.Message);
            return;
        }

        renderGrid(JSONData.ExcelErrors, JSONData.Title);
        spanValidSetting.text(JSONData.ValidSetting + "/" + JSONData.TotalRows);
        buttonUpdate.prop("disabled", !JSONData.ValidSetting);
    }

    function renderGrid(data, title) {
        var bodyData = [];

        $.each(data, function (index, error) {
            bodyData.push({
                Columns: [
                    { Name: "colNo", Value: numeral(index + 1).format(CMSConstants.NumberFormat.IntNumber) },
                    { Name: "colExcelCell", Value: error.CellId },
                    { Name: "colAccount", Value: error.Username },
                    { Name: "colValue", Value: error.Value },
                    { Name: "colReason", Value: error.Reason }]
            });
        });

        gridOptions = gridOptions || getGridOptions();
        gridOptions.titleInfo.text = title;
        gridOptions.bodyRows = bodyData;
        freegrid.removeClass("display-none").fanexGrid(gridOptions);
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
        resetGrid();
    }

    function resetGrid() {
        spanValidSetting.text("N/A");
        buttonUpdate.prop("disabled", true);
        freegrid.addClass("display-none");
        $("#freegrid-grid-title").addClass("display-none");
    }

    function uploadFile() {
        CMS.disableInput();
        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("VirtualSettingControl/UploadVirtualRate/Update"),
            type: "POST",
            data: { sportType: sportTypeId },
            success: function (data) {
                var JSONData = JSON.parse(data);
                if (JSONData.Code === 0) {
                    reset();
                    CMS.showSuccessMessage(JSONData.Message);
                } else {
                    CMS.showErrorMessage(JSONData.Message);
                }
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    function showProgressBar(totalRows) {
        var batchSize = 150;
        var totalBatches = Math.ceil(totalRows / batchSize);
        var percentIncreasePerBatch = Math.ceil(100 / totalBatches);
        var processPerBatchInSeconds = 6;

        progressbar.progressbar({
            value: 0,
            change: function () {
                progressLabel.text(progressbar.progressbar("value") + "%");
            }
        });

        function progress() {
            var val = progressbar.progressbar("value") || 0;
            val += percentIncreasePerBatch;

            if (val < 100) {
                progressbar.progressbar("value", val);
                progressbarTimeId = setTimeout(progress, 1000 * processPerBatchInSeconds);
            } else {
                progressbar.progressbar("value", 99);
            }
        }

        progress();
        progressbar.removeClass("display-none");
    }

    function hideProgressBar() {
        clearTimeout(progressbarTimeId);
        progressbar.addClass("display-none");
        progressLabel.text("0%");
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
            resetGrid();
            fileControl && verifyByWeb(fileControl.files[0]);
        });

        buttonUpdate.click(uploadFile);
    });
})(jQuery);