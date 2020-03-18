"use strict";
(function ($) {
    var elements = {
        file: $("#file"),
        excelFile: $("#txt-excelfile"),
        resultMessageDiv: $("#div-result-message")
    };
    var MaxFileSize = 1024 * 1024 * 5;
    var validSetting = null, errorSetting = null;

    function getFileName(path) {
        return path.replace(/^.*[\\\/]/, "");
    }

    function getFileExtension(fileName) {
        return /[.]/.exec(fileName) ? /[^.]+$/.exec(fileName)[0] : "";
    }

    function validateFileUpload(fileUpload) {
        CMS.clearTipsyError(elements.excelFile);
        var fileName = getFileName(fileUpload.value);

        if (!fileName || fileName.length === 0) {
            CMS.showTipsyError(elements.excelFile, viewResources.Error_RequiredField, "sw");
            return false;
        }

        elements.excelFile.val(fileName);
        var fileExtension = getFileExtension(fileName);

        if (fileExtension !== "xls" && fileExtension !== "xlsx") {
            CMS.showTipsyError(elements.excelFile, viewResources.Error_NotExcelFile, "sw");
            return false;
        }

        if (fileUpload.files[0].size > MaxFileSize) {
            CMS.showTipsyError(elements.excelFile, viewResources.Error_ExcessMaxFileSize, "sw");
            return false;
        }

        return true;
    }

    function uploadFile(file) {
        CMS.disableInput();
        CMS.hideInlineMsg(elements.resultMessageDiv);

        var formData = new FormData();
        formData.append("File", file);
        formData.append("SportType", sportType);

        var jqxhr = $.ajax({
            url: CMS.resolveRootUrl("TurnoverControl/ImportGlobalSetting/Upload"),
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, elements.resultMessageDiv);
                }
                else {
                    validSetting = validSetting || new ValidSetting(profiles, sportType);
                    validSetting.initialize(result.ValidSettings);

                    errorSetting = errorSetting || new ErrorSetting();
                    errorSetting.initialize(result.ErrorSettings);
                }
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }
    
    $(function () {
        elements.file
            .change(function () {
                if (!validateFileUpload(this)) {
                    return;
                }

                uploadFile(this.files[0]);
            })
            .click(function () {
                this.value = null;
            });
    });
})(jQuery);