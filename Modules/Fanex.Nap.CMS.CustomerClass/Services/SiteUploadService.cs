namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;

    internal interface ISiteUploadService
    {
        IReadOnlyCollection<ExcelError> ExcelErrors { get; }

        bool HasExcelErrors { get; }

        IEnumerable<SiteSettingExcel> ValidateExcelFile(
            Stream fileStream,
            string extension,
            IEnumerable<IdNameModel> sites,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> policyTitles);
    }

    internal class SiteUploadService : ISiteUploadService
    {
        private readonly List<ExcelError> excelErrors = new List<ExcelError>();
        public IReadOnlyCollection<ExcelError> ExcelErrors
        {
            get
            {
                return excelErrors;
            }
        }

        public bool HasExcelErrors => excelErrors.Count > 0;

        public IEnumerable<SiteSettingExcel> ValidateExcelFile(
            Stream fileStream,
            string extension,
            IEnumerable<IdNameModel> sites,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> policyTitles)
        {
            excelErrors.Clear();
            var sheet = ValidateExcelTemplate(fileStream, extension);

            return HasExcelErrors
                ? Enumerable.Empty<SiteSettingExcel>()
                : ValidateBodyRows(sheet, sites, currencies, policyTitles);
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
                var site = headerRow.Cells[0]?.ToString()?.Trim();
                if (site != Labels.Site)
                {
                    excelErrors.Add(new ExcelError("A:1", Labels.Site, site, Labels.InvalidHeader));
                }

                var currency = headerRow.Cells[1]?.ToString()?.Trim();
                if (currency != Labels.CurrencyName)
                {
                    excelErrors.Add(new ExcelError("B:1", Labels.CurrencyName, currency, Labels.InvalidHeader));
                }

                var policy = headerRow.Cells[2]?.ToString()?.Trim();
                if (policy != Labels.Policy)
                {
                    excelErrors.Add(new ExcelError("C:1", Labels.Policy, policy, Labels.InvalidHeader));
                }

                var platform = headerRow.Cells[3]?.ToString()?.Trim();
                if (platform != Labels.Platform)
                {
                    excelErrors.Add(new ExcelError("D:1", Labels.Platform, platform, Labels.InvalidHeader));
                }
            }
        }

        private IEnumerable<SiteSettingExcel> ValidateBodyRows(
            ISheet sheet,
            IEnumerable<IdNameModel> sites,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> policyTitles)
        {
            var siteDict = ToDictionary(sites);
            var currencyDict = ToDictionary(currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency }));
            var policyDict = ToDictionary(policyTitles);
            var platformDict = PlatformConst.Platforms.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            var siteSettingExcelList = new List<SiteSettingExcel>();

            IRow row;
            SiteSettingExcel siteSettingExcel;
            var actualTotalRows = 0;
            const int uploadLimitRows = SiteViewModel.UploadLimitRows;
            var totalSheetRows = sheet.LastRowNum + 1;

            for (var i = 1; i < totalSheetRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    var siteCellRef = new CellReference("A");
                    var site = row.GetCell(siteCellRef.Col)?.ToString()?.Trim();

                    var currencyCellRef = new CellReference("B");
                    var currency = row.GetCell(currencyCellRef.Col)?.ToString()?.Trim();

                    var policyCellRef = new CellReference("C");
                    var policy = row.GetCell(policyCellRef.Col)?.ToString()?.Trim();

                    var platformCellRef = new CellReference("D");
                    var platform = row.GetCell(platformCellRef.Col)?.ToString()?.Trim();

                    if (!string.IsNullOrWhiteSpace(site)
                        || !string.IsNullOrWhiteSpace(currency)
                        || !string.IsNullOrWhiteSpace(policy)
                        || !string.IsNullOrWhiteSpace(platform))
                    {
                        actualTotalRows++;

                        if (actualTotalRows > uploadLimitRows)
                        {
                            excelErrors.Add(new ExcelError(string.Format(Labels.UploadLimitRowError, uploadLimitRows.ToString("N0"))));
                            break;
                        }

                        siteSettingExcel = ValidateBodyRow(
                            siteCellRef,
                            site,
                            siteDict,
                            currencyCellRef,
                            currency,
                            currencyDict,
                            policyCellRef,
                            policy,
                            policyDict,
                            platformCellRef,
                            platform,
                            platformDict,
                            i + 1);

                        if (siteSettingExcel != null)
                        {
                            siteSettingExcelList.Add(siteSettingExcel);
                        }
                    }
                }
            }

            return siteSettingExcelList;
        }

        private SiteSettingExcel ValidateBodyRow(
            CellReference siteCellRef,
            string site,
            IDictionary<string, int> siteDict,
            CellReference currencyCellRef,
            string currency,
            IDictionary<string, int> currencyDict,
            CellReference policyCellRef,
            string policy,
            IDictionary<string, int> policyDict,
            CellReference platformCellRef,
            string platform,
            IDictionary<string, byte> platformDict,
            int rowNumber)
        {
            var sites = ValidateSiteCell(siteCellRef, site, siteDict, rowNumber, out var siteExcelError);
            if (siteExcelError != null)
            {
                excelErrors.Add(siteExcelError);
            }

            var currencies = ValidateCurrencyCell(currencyCellRef, currency, currencyDict, rowNumber, out var currencyExcelError);
            if (currencyExcelError != null)
            {
                excelErrors.Add(currencyExcelError);
            }

            var policyId = ValidatePolicyCell(policyCellRef, policy, policyDict, rowNumber, out var policyExcelError);
            if (policyExcelError != null)
            {
                excelErrors.Add(policyExcelError);
            }

            var platforms = ValidatePlatformCell(platformCellRef, platform, platformDict, rowNumber, out var platformExcelError);
            if (platformExcelError != null)
            {
                excelErrors.Add(platformExcelError);
            }

            return siteExcelError != null || currencyExcelError != null || policyExcelError != null || platformExcelError != null
                ? null
                : new SiteSettingExcel(policyId, sites, currencies, platforms);
        }
#pragma warning restore S1541,S3776,S134,S1227,S107,S1067

#pragma warning disable S126
        private static IEnumerable<string> ValidateSiteCell(
            CellReference cellRef,
            string site,
            IDictionary<string, int> siteDict,
            int rowNumber,
            out ExcelError excelError)
        {
            excelError = null;
            if (string.IsNullOrWhiteSpace(site))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Site, site, Labels.RequiredSite);
            }
            else if (site.Equals(Labels.All, StringComparison.OrdinalIgnoreCase) || string.Equals(site, Labels.AllCredit, StringComparison.OrdinalIgnoreCase) || string.Equals(site, Labels.AllDeposit, StringComparison.OrdinalIgnoreCase))
            {
                return new[] { site.Replace(" ", string.Empty) };
            }
            else if (!siteDict.ContainsKey(site))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Site, site, Labels.InvalidSite);
            }

            return new[] { site };
        }

        private static IEnumerable<int> ValidateCurrencyCell(
            CellReference cellRef,
            string currency,
            IDictionary<string, int> currencyDict,
            int rowNumber,
            out ExcelError excelError)
        {
            excelError = null;
            var currencyId = 0;
            if (string.IsNullOrWhiteSpace(currency))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Currency, currency, Labels.RequiredCurrency);
            }
            else if (currency.Equals(Labels.All, StringComparison.OrdinalIgnoreCase))
            {
                return new[] { 0 };
            }
            else if (!currencyDict.TryGetValue(currency, out currencyId))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Currency, currency, Labels.InvalidCurrency);
            }

            return currencyId == 0 ? Array.Empty<int>() : new[] { currencyId };
        }

        private static int ValidatePolicyCell(
            CellReference cellRef,
            string policy,
            IDictionary<string, int> policyDict,
            int rowNumber,
            out ExcelError excelError)
        {
            excelError = null;
            var policyId = 0;
            if (string.IsNullOrWhiteSpace(policy))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Policy, policy, Labels.RequiredPolicy);
            }
            else if (!policyDict.TryGetValue(policy, out policyId))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Policy, policy, Labels.InvalidPolicy);
            }

            return policyId;
        }

        private static IEnumerable<byte> ValidatePlatformCell(
            CellReference cellRef,
            string platform,
            IDictionary<string, byte> platformDict,
            int rowNumber,
            out ExcelError excelError)
        {
            excelError = null;
            byte platformId = 0;
            if (string.IsNullOrWhiteSpace(platform))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Platform, platform, Labels.RequiredPlatform);
            }
            else if (platform.Equals(Labels.All, StringComparison.OrdinalIgnoreCase))
            {
                return new[] { (byte)0 };
            }
            else if (!platformDict.TryGetValue(platform, out platformId))
            {
                excelError = new ExcelError(GetExcelCell(cellRef, rowNumber), Labels.Platform, platform, Labels.InvalidPlatform);
            }

            return platformId == 0 ? Array.Empty<byte>() : new[] { platformId };
        }
#pragma warning restore S126

        private static string GetExcelCell(CellReference cellRef, int rowNumber)
            => $"{cellRef.FormatAsString()}:{rowNumber}";

        private static IDictionary<string, int> ToDictionary(IEnumerable<IdNameModel> collections)
            => collections.ToDictionary(kvp => kvp.Name, kvp => kvp.Id);
    }
}
