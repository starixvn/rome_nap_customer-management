"use strict";
$(document).ready(function () {
    var nonsportsExcelBLC = new Fanex.CMS.NonSportCompanyBLCExcel();
    nonsportsExcelBLC.InitPage();
});

Namespace.Register('Fanex.CMS.NonSportCompanyBLCExcel');
Fanex.CMS.NonSportCompanyBLCExcel = function () {
    let errExcelDiv = $("#errExcel");
    let cachedBetLimits = [];

    const MaxFileSize = 1024 * 1024 * 1;

    function registerImportEvent() {
        $("#iconDownloadTemplate").off("click").on("click", downloadTemplate);
        var fileControl;
        $("#file")
            .change(function () {
                fileControl = this;
                var isValidFile = validateUploadedFile(this);
                $("#verifyBtn").prop("disabled", !isValidFile);
            })
            .click(function () {
                this.value = null;
                $("#file").val("");
                $("#verifyBtn").prop("disabled", true);
            });

        $("#verifyBtn").click(function () {
            fileControl && verifyByWeb(fileControl.files[0]);
        });

        $("#importBtn").click(uploadFile);
    }

    function validateUploadedFile(fileControl) {
        var textboxFile = $("#file");
        CMS.clearTipsyError(textboxFile);
        CMS.hideInlineMsg(errExcelDiv);
        var fileName = getFileName(fileControl.files[0].name);

        if (!fileName || !fileName.length) {
            CMS.showTipsyError(textboxFile, "Required Field!", "sw");
            return false;
        }
        var fileExtension = getFileExtension(fileName);

        if (fileExtension !== "xls" && fileExtension !== "xlsx") {
            CMS.showInlineErrorMsg("Sorry, you choose an invalid file extension. Please choose an Excel file (.xls or .xlsx).", errExcelDiv);
            return false;
        }

        if (fileControl.files[0].size > MaxFileSize) {
            CMS.showInlineErrorMsg("Your uploaded file's size can't be larger than 1 MB.", errExcelDiv);
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

    function downloadTemplate() {
        const apiUrl = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/DownloadTemplate");
        window.location.href = apiUrl;
    }

    function verifyByWeb(file) {
        CMS.disableInput();
        var formData = new FormData();
        formData.append("File", file);
        $("#importBtn").attr("disabled", true);

        const apiUrl = CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/ValidateExcel");
        var jqxhr = $.ajax({
            url: apiUrl,
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                verifyByWebSuccessHandler(result, formData);
            },
            error: CMS.handleAjaxError
        });

        jqxhr.always(CMS.enableInput);
    }

    function verifyByWebSuccessHandler(data, formData) {
        if (data.Error) {
            CMS.showInlineErrorMsg(data.Error.Message, errExcelDiv);
        } else {
            CMS.hideInlineMsg(errExcelDiv);
            RenderUploadData(data.BetLimits);
            cachedBetLimits = data.BetLimits;
        }
    }

    function RenderUploadData(betLimits) {
        if (betLimits && betLimits.length > 0) {
            $("#importBtn").attr("disabled", false);
        } else {
            $("#importBtn").attr("disabled", true);
        }
        var options = GetGridOptions();
        options.bodyRows = RenderGridBodyData(betLimits);

        $("#grid-excel-table").removeClass("display-none").fanexGrid(options);
    }

    function GetGridOptions() {
        const headerColumns = [
            {
                HeaderText: viewResources.lblSportTypeID,
                Name: "SportType",
                CellAlign: "center",
                Width: 80,
                HeaderAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblSportTypeName,
                Name: "SportTypeName",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                CanResize: true
            },
            {
                HeaderText: viewResources.lblCreditCurrency,
                Name: "CreditCurrency",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblDepositCurrency,
                Name: "DepositCurrency",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.site,
                Name: "Site",
                Width: 160,
                HeaderAlign: "center",
                CellAlign: "center",
                CanResize: true
            },
            {
                HeaderText: viewResources.lblGameGroup,
                Name: "GameGroups",
                Width: 120,
                HeaderAlign: "center",
                CellAlign: "center",
                AllowTruncate: true,
                CanResize: true
            },
            {
                HeaderText: viewResources.lblMinBet,
                Width: 90,
                Name: "CurMinBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.DecimalFormat
            },
            {
                HeaderText: viewResources.lblMaxBet,
                Width: 90,
                Name: "CurMaxBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.IntegerFormat
            },
            {
                HeaderText: viewResources.lblRemark,
                Width: 160,
                Name: "CurRemark",
                HeaderAlign: "center",
                CellAlign: "center"
            },
            {
                HeaderText: viewResources.lblMinBet,
                Width: 90,
                Name: "NewMinBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.DecimalFormat
            },
            {
                HeaderText: viewResources.lblMaxBet,
                Width: 90,
                Name: "NewMaxBet",
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: CMS.formats.IntegerFormat
            },
            {
                HeaderText: viewResources.lblRemark,
                Width: 160,
                Name: "NewRemark",
                HeaderAlign: "center",
                CellAlign: "center"
            }
        ];

        return {
            columns: headerColumns,
            customizeHeader: true,
            groupHeaderRows: [
                {
                    Columns: [
                        { HeaderText: viewResources.lblSportTypeID, Name: "SportType", MergeRowNumber: 2 },
                        { HeaderText: viewResources.lblSportTypeName, Name: "SportTypeName", MergeRowNumber: 2 },
                        { HeaderText: viewResources.lblCreditCurrency, Name: "CreditCurrency", MergeRowNumber: 2 },
                        { HeaderText: viewResources.lblDepositCurrency, Name: "DepositCurrency", MergeRowNumber: 2 },
                        { HeaderText: viewResources.site, Name: "Site", MergeRowNumber: 2 },
                        { HeaderText: viewResources.lblGameGroup, Name: "GameGroups", MergeRowNumber: 2 },
                        { HeaderText: viewResources.lblCurrentValue, Name: "CurMinBet", MergeColNumber: 3 },
                        { HeaderText: viewResources.lblNewValue, Name: "NewMinBet", MergeColNumber: 3 }
                    ]
                }
            ],
            bodyRows: [],
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            headerColumnHeight: 27,
            bodyRowHeight: 27,
            gridExpandHeight: 250,
            showHighlightRow: true,
            showPager: false
        };
    }

    function RenderGridBodyData(betLimits) {
        const bodyData = [];
        const highlightClass = 'hover-column';
        betLimits.forEach((betLimit) => {
            bodyData.push({
                Columns: [
                    { Name: 'SportType', Value: betLimit.SportTypeId, TdClass: highlightClass },
                    { Name: 'SportTypeName', Value: betLimit.SportType, TdClass: highlightClass },
                    { Name: 'CreditCurrency', Value: betLimit.CreditCurrencyGroup, TdClass: highlightClass },
                    { Name: 'DepositCurrency', Value: betLimit.DepositCurrencyGroup, TdClass: highlightClass },
                    { Name: 'Site', Value: betLimit.Site, TdClass: highlightClass },
                    { Name: 'GameGroups', Value: betLimit.GameGroup, TdClass: highlightClass },
                    { Name: 'CurMinBet', Value: betLimit.CurMinBet === undefined ? "" : betLimit.CurMinBet, TdClass: highlightClass },
                    { Name: 'CurMaxBet', Value: betLimit.CurMaxBet === undefined ? "" : betLimit.CurMaxBet, TdClass: highlightClass },
                    { Name: 'CurRemark', Value: betLimit.CurRemark === undefined ? "" : betLimit.CurRemark, TdClass: highlightClass },
                    { Name: 'NewMinBet', Value: betLimit.MinBet === undefined ? "" : betLimit.MinBet, TdClass: highlightClass },
                    { Name: 'NewMaxBet', Value: betLimit.MaxBet === undefined ? "" : betLimit.MaxBet, TdClass: highlightClass },
                    { Name: 'NewRemark', Value: betLimit.Remark === undefined ? "" : betLimit.Remark, TdClass: highlightClass }
                ]
            });
        });

        return bodyData;
    }

    //function ClearGridImport() {
    //    $("#file").reset();
    //    RenderUploadData([]);
    //}

    function uploadFile() {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("NonSportsCompanyBetLimitSetting/ImportExcel"),
            cachedBetLimits,
            function (response) {
                var result = typeof response === "string" ? JSON.parse(response) : response;
                if (result.IsSuccess) {
                    CMS.showSuccessMessage("Insert successfully!");
                    //ClearGridImport();
                } else {
                    CMS.showErrorMessage(result.Message);
                }
            },
            function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            function () {
                CMS.enableInput();
            });
    }

    this.InitPage = function () {
        registerImportEvent();
    };
};

