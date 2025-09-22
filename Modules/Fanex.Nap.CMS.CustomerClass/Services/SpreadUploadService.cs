namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal interface ISpreadUploadService
    {
        IReadOnlyCollection<ExcelError> ExcelErrors { get; }

        bool HasExcelErrors { get; }

        SpreadSettingExcel ValidateExcelFile(
            Stream fileStream,
            string extension,
            SpreadSettingBodyExcelPreparer settingValidateExcelModel);
    }
    internal class SpreadUploadService : ISpreadUploadService
    {
        private static Dictionary<string, string> SpreadColumnNameMapping => new Dictionary<string, string>
        {
            { Labels.Title        , nameof(SpreadSettingExcelValidModel.PolicyTitle)  },
            { Labels.CustomerClass, nameof(SpreadSettingExcelValidModel.CustomerClass)},
            { Labels.SportType    , nameof(SpreadSettingExcelValidModel.SportType)    },
            { Labels.Site         , nameof(SpreadSettingExcelValidModel.Site)         },
            { Labels.Currency     , nameof(SpreadSettingExcelValidModel.Currency)     },
            { Labels.Platform     , nameof(SpreadSettingExcelValidModel.Platform)     },
            { Labels.Spread       , nameof(SpreadSettingExcelValidModel.Spread)       },
        };

        private readonly List<ExcelError> excelErrors = new List<ExcelError>();

        private SpreadSettingBodyExcelBuilder Setting;

        public IReadOnlyCollection<ExcelError> ExcelErrors => excelErrors;

        public bool HasExcelErrors => excelErrors.Count > 0;

        public SpreadSettingExcel ValidateExcelFile(Stream fileStream, string extension, SpreadSettingBodyExcelPreparer settingValidateExcelModel)
        {
            excelErrors.Clear();
            var sheet = ValidateExcelTemplate(fileStream, extension);

            return HasExcelErrors
                ? new SpreadSettingExcel()
                : ValidateBodyRows(sheet, settingValidateExcelModel);
        }

        private ISheet ValidateExcelTemplate(Stream fileStream, string extension)
        {
            var workbook = ValidateWorkbook(fileStream, extension);
            if (workbook == null)
            {
                return null;
            }

            var sheet = ValidateSheet(workbook);
            if (sheet != null)
            {
                ValidateHeaderRow(sheet.GetRow(0));
            }

            return sheet;
        }

        private IWorkbook ValidateWorkbook(Stream fileStream, string fileExtension)
        {
            try
            {
                if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    return new HSSFWorkbook(fileStream);
                }

                if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new XSSFWorkbook(fileStream);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            excelErrors.Add(new ExcelError(Labels.NotExcelFileError));
            return null;
        }

        private ISheet ValidateSheet(IWorkbook workbook)
        {
            if (workbook.NumberOfSheets == 0)
            {
                excelErrors.Add(new ExcelError(Labels.IncorrectLayoutError));

                return null;
            }

            return workbook.GetSheetAt(0);
        }

#pragma warning disable S1541,S3776,S134,S1227,S107,S1067
        private void ValidateHeaderRow(IRow headerRow)
        {
            if (headerRow == null)
            {
                excelErrors.Add(new ExcelError(Labels.InvalidHeader));
            }
            else
            {
                foreach (var headerVal in SpreadColumnNameMapping.Select((value, index) => new { value, index }))
                {
                    var headerName = headerRow.Cells[headerVal.index]?.ToString()?.Trim();
                    if (headerName != headerVal.value.Key)
                    {
                        var columnIndex = (char)('A' + headerVal.index);
                        var columnRef = $"{columnIndex}:1";
                        excelErrors.Add(new ExcelError(columnRef, headerVal.value.Key, headerName, Labels.InvalidHeader));
                    }
                }
            }
        }

        private SpreadSettingExcel ValidateBodyRows(
            ISheet sheet,
            SpreadSettingBodyExcelPreparer settingValidateExcel)
        {
            var spreadSettingExcel = new SpreadSettingExcel();

            const int uploadLimitRows = SpreadViewModel.UploadLimitRows;
            if (sheet.LastRowNum > uploadLimitRows)
            {
                excelErrors.Add(new ExcelError(string.Format(Labels.UploadLimitRowError, uploadLimitRows.ToString("N0"))));
                return spreadSettingExcel;
            }

            Setting = new SpreadSettingBodyExcelBuilder(settingValidateExcel);

            IRow row;
            var totalSheetRows = sheet.LastRowNum + 1;

            for (var i = 1; i < totalSheetRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    var spreadRowData = new Dictionary<string, string>();
                    foreach (var val in SpreadColumnNameMapping.Select((value, index) => new { value, index }))
                    {
                        var cellRef = new CellReference((char)('A' + val.index) + string.Empty);
                        var cellValue = row.GetCell(val.index)?.ToString()?.Trim();

                        spreadRowData.Add(val.value.Key, cellValue);
                    }

                    ValidateBodyRow(spreadSettingExcel, spreadRowData, i + 1);
                }
            }

            return spreadSettingExcel;
        }

        private void ValidateBodyRow(
            SpreadSettingExcel spreadSettingExcel,
            IDictionary<string, string> spreadRowData,
            int rowNumber)
        {
            var reasons = new List<string>();

            var title = GetValueOrDefault(spreadRowData, Labels.Title);
            var isTitleValid =
                    ValidateWithRequired(title) &&
                    ValidateWithMaxLength(title, Setting.PolicyTitleMaxLength);

            var customerClass = GetValueOrDefault(spreadRowData, Labels.CustomerClass);
            short customerClassConverted = default;
            var isCustomerClassValid =
                    ValidateWithRequired(customerClass) &&
                    ValidateWithHashSet(customerClass, Setting.CustomerClassesHashSet, out customerClassConverted);

            var sportType = GetValueOrDefault(spreadRowData, Labels.SportType);
            int sportTypeId = default;
            var isSportTypeValid =
                    ValidateWithRequired(sportType) &&
                    ValidateWithDictionary(sportType, Setting.SportTypesDictionary, out sportTypeId);

            var site = GetValueOrDefault(spreadRowData, Labels.Site);
            int siteId = default;
            var isSiteValid =
                    ValidateWithRequired(site) &&
                    ValidateWithDictionary(site, Setting.SitesDictionary, out siteId);

            var currency = GetValueOrDefault(spreadRowData, Labels.Currency);
            int currencyId = default;
            var isCurrencyValid =
                    ValidateWithRequired(currency) &&
                    ValidateWithDictionary(currency, Setting.CurrenciesDictionary, out currencyId);

            var platform = GetValueOrDefault(spreadRowData, Labels.Platform);
            byte platformId = default;
            var isPlatformValid =
                    ValidateWithRequired(platform) &&
                    ValidateWithDictionary(platform, Setting.PlatformsDictionary, out platformId);

            var spread = GetValueOrDefault(spreadRowData, Labels.Spread);
            decimal spreadConverted = default;
            var isSpreadValid =
                    ValidateWithRequired(spread) &&
                    ValidateWithRange(spread, Setting.SpreadRange, out spreadConverted);

            // Collect validation reasons
            if (!isTitleValid) reasons.Add(Labels.Title);
            if (!isCustomerClassValid) reasons.Add(Labels.CustomerClass);
            if (!isSportTypeValid) reasons.Add(Labels.SportType);
            if (!isSiteValid) reasons.Add(Labels.Site);
            if (!isCurrencyValid) reasons.Add(Labels.Currency);
            if (!isPlatformValid) reasons.Add(Labels.Platform);
            if (!isSpreadValid) reasons.Add(Labels.Spread);

            // Handle validation results
            if (reasons.Any())
            {
                var invalid = CreateInvalidModel(spreadRowData, reasons, rowNumber);
                spreadSettingExcel.SpreadInvalids.Add(invalid);
            }
            else
            {
                var valid = CreateValidModel(title, customerClassConverted, sportTypeId,
                               siteId, currencyId, platformId, spreadConverted);

                spreadSettingExcel.SpreadValids.Add(valid);
            }
        }

        private SpreadSettingExcelInvalidModel CreateInvalidModel(IDictionary<string, string> spreadRowData,
                                      List<string> reasons, int rowNumber)
        {
            return new SpreadSettingExcelInvalidModel
            {
                PolicyTitle = GetValueOrDefault(spreadRowData, Labels.Title),
                CustomerClass = GetValueOrDefault(spreadRowData, Labels.CustomerClass),
                SportType = GetValueOrDefault(spreadRowData, Labels.SportType),
                Site = GetValueOrDefault(spreadRowData, Labels.Site),
                Currency = GetValueOrDefault(spreadRowData, Labels.Currency),
                Platform = GetValueOrDefault(spreadRowData, Labels.Platform),
                Spread = GetValueOrDefault(spreadRowData, Labels.Spread),
                ID = rowNumber,
                Reason = "Invalid " + string.Join(", ", reasons)
            };
        }

        private SpreadSettingExcelValidModel CreateValidModel(string title, short customerClass,
                                    int sportTypeId, int siteId, int currencyId, byte platformId, decimal spread)
        {
            return new SpreadSettingExcelValidModel
            {
                PolicyTitle = title,
                CustomerClass = customerClass,
                SportType = sportTypeId,
                Site = siteId,
                Currency = currencyId,
                Platform = platformId,
                Spread = spread
            };
        }
#pragma warning restore S1541,S3776,S134,S1227,S107,S1067

#pragma warning disable S126
        private bool ValidateWithRequired(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        private bool ValidateWithMaxLength(string value, int maxLength)
        {
            return value.Length <= maxLength;
        }

        private bool ValidateWithRange<T>(string value, (T min, T max) range, out T converted) where T : IComparable<T>
        {
            try
            {
                converted = (T)Convert.ChangeType(value, typeof(T));

                return converted.CompareTo(range.min) >= 0 && converted.CompareTo(range.max) <= 0;
            }
            catch { }

            converted = default;
            return false;
        }

        private bool ValidateWithDictionary<T>(string key, IDictionary<string, T> dictionary, out T value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private bool ValidateWithHashSet<T>(string value, HashSet<T> hashSet, out T converted)
        {
            try
            {
                converted = (T)Convert.ChangeType(value, typeof(T));

                if (hashSet.Contains(converted))
                {
                    return true;
                }
            }
            catch { }

            converted = default;
            return false;
        }

        private string GetValueOrDefault(IDictionary<string, string> data, string key)
        {
            return data.TryGetValue(key, out var value) ? value : string.Empty;
        }
#pragma warning restore S126
    }
}
