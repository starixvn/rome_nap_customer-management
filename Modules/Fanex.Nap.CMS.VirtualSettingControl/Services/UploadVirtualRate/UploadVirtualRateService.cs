namespace Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Configurations;
    using Criteria.UploadVirtualRate;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails;
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Microsoft.Practices.ObjectBuilder2;
    using Models.UploadVirtualRate;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using Resources;

    public class UploadVirtualRateService : ServiceBase, IUploadVirtualRateService
    {
        private const int DisabledAccountErrCode = 600;
        private const int OneHundred = 100;
        public const int UpdateBatchSize = 150;
        private const string XMLFormat = "<R UN=\"{0}\" Cell=\"{1}\" Mul=\"{2}\" LId=\"{3}\" BTGId=\"{4}\" BTId=\"{5}\" Live=\"{6}\"/>";
        private const string SessionIdSessionKey = "Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate.UploadVirtualRateService.SessionId_{0}";

        private readonly IVirtualRateDetailsService virtualRateDetailsService;
        private readonly int sportType;
        private readonly int limitAccount;
        private readonly VirtualRateBetTypeCollection betTypes;

        public UploadVirtualRateService(int sportType) : base()
        {
            this.sportType = sportType;
            virtualRateDetailsService = new VirtualRateDetailsService();

            limitAccount = LimitAccountConstants.GetLimitAccount(sportType);
            betTypes = sportType == SportTypeConstants.ESports
                ? ESportsVirtualRateSection.Instance.BetTypes
                : BasketballVirtualRateSection.Instance.BetTypes;
        }

        #region Web Validation

        public ExcelErrorModel VerifyByWeb(Stream stream, string fileExtension)
        {
            var actualTotalRows = 0;
            var excelErrors = new List<ExcelErrorDTO>();
            var workbook = GetWorkbook(stream, fileExtension);

            if (workbook == null)
            {
                excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_NotExcelFile));
            }
            else
            {
                var sheet = GetSheet(workbook);

                if (sheet == null)
                {
                    excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_IncorrectLayout));
                }
                else
                {
                    excelErrors.AddRange(ValidateHeaderRow(sheet.GetRow(0)));

                    if (excelErrors.Count == 0)
                    {
                        excelErrors.AddRange(ValidateBodyRows(sheet, ref actualTotalRows));
                    }
                }
            }

            return new ExcelErrorModel(excelErrors, 0, actualTotalRows, $"{ESportsResource.FormatError}: {excelErrors.Count}");
        }

        private static IWorkbook GetWorkbook(Stream stream, string fileExtension)
        {
            try
            {
                if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    return new HSSFWorkbook(stream);
                }

                if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new XSSFWorkbook(stream);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return null;
        }

        private static ISheet GetSheet(IWorkbook workbook)
            => workbook.NumberOfSheets == 0
            ? null
            : workbook.GetSheetAt(0);

        private IEnumerable<ExcelErrorDTO> ValidateHeaderRow(IRow headerRow)
        {
            var errors = new List<ExcelErrorDTO>();

            if (headerRow == null)
            {
                errors.Add(new ExcelErrorDTO(ESportsResource.InvalidHeader));
            }
            else
            {
                ICell cell = headerRow.Cells[0];

                if (cell.ToString() != ESportsResource.Account)
                {
                    errors.Add(new ExcelErrorDTO("A1", null, cell?.ToString(), ESportsResource.InvalidHeader));
                }
                else
                {
                    CellReference cellRef;

                    foreach (VirtualRateBetTypeElement betType in betTypes)
                    {
                        cellRef = new CellReference(betType.Cell);
                        cell = headerRow.GetCell(cellRef.Col);

                        if (cell?.ToString() != betType.Key)
                        {
                            errors.Add(new ExcelErrorDTO(betType.Cell + "1", null, cell?.ToString(), ESportsResource.InvalidHeader));
                        }
                    }
                }
            }

            return errors;
        }

        private IEnumerable<ExcelErrorDTO> ValidateBodyRows(ISheet sheet, ref int actualTotalRows)
        {
            IRow row;
            var excelErrors = new List<ExcelErrorDTO>();
            var rateRanges = virtualRateDetailsService.GetVirtualRateRanges().ToDictionary(kvp => kvp.Value, kvp => true);
            var accountsHash = new Dictionary<string, bool>();

            var totalSheetRows = sheet.LastRowNum + 1;

            for (int i = 1; i < totalSheetRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    actualTotalRows++;

                    if (actualTotalRows > limitAccount)
                    {
                        return new List<ExcelErrorDTO>
                        {
                            new ExcelErrorDTO(string.Format(ESportsResource.LimitAccountError, limitAccount.ToString("N0")))
                        };
                    }

                    ValidateBodyRows(row, excelErrors, rateRanges, accountsHash, i + 1);
                }
            }

            return excelErrors;
        }

        private void ValidateBodyRows(
            IRow row,
            IList<ExcelErrorDTO> excelErrors,
            IDictionary<int, bool> rateRanges,
            IDictionary<string, bool> accountsHash,
            int rowNumber)
        {
            string multiplier;
            var cellRef = new CellReference("A");
            var account = row.GetCell(cellRef.Col)?.ToString();
            ValidateAccount(account, excelErrors, accountsHash, cellRef, rowNumber);

            foreach (VirtualRateBetTypeElement betType in betTypes)
            {
                cellRef = new CellReference(betType.Cell);
                multiplier = row.GetCell(cellRef.Col)?.ToString();

                if (!string.IsNullOrWhiteSpace(multiplier)
                    && (!int.TryParse(multiplier, out var multiplierValue) || !rateRanges.ContainsKey(multiplierValue)))
                {
                    excelErrors.Add(
                        new ExcelErrorDTO(GetExcelCell(cellRef, rowNumber), account, multiplier, ESportsResource.InvalidValue));
                }
            }
        }

        private static void ValidateAccount(
            string account,
            IList<ExcelErrorDTO> excelErrors,
            IDictionary<string, bool> accountsHash,
            CellReference cellRef,
            int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                excelErrors.Add(
                    new ExcelErrorDTO(GetExcelCell(cellRef, rowNumber), account, account, ESportsResource.RequiredUsernameError));
            }
            else
            {
                var accountTrim = account.Trim();
                var accountLowerCase = accountTrim.ToLowerInvariant();

                if (accountsHash.ContainsKey(accountLowerCase))
                {
                    excelErrors.Add(
                        new ExcelErrorDTO(GetExcelCell(cellRef, rowNumber), account, account, ESportsResource.DuplicatedUsernameError));
                }
                else
                {
                    accountsHash.Add(accountLowerCase, true);
                }
            }
        }

        #endregion

        #region DB Validation

        public ExcelErrorModel VerifyByDB(Stream stream, string fileExtension, int userId, int startRow)
        {
            var workbook = GetWorkbook(stream, fileExtension);
            var sheet = GetSheet(workbook);

            var totalRows = sheet.LastRowNum + 1;
            var endRow = startRow + UpdateBatchSize;

            if (endRow > totalRows)
            {
                endRow = totalRows;
            }

            Guid sessionId;

            if (startRow == 1)
            {
                sessionId = Guid.NewGuid();
                SetSessionId(sessionId);
            }
            else
            {
                sessionId = GetSessionId();
            }

            return VerifyExcelTemplate(sheet, startRow, endRow, sessionId, userId);
        }

        private ExcelErrorModel VerifyExcelTemplate(ISheet sheet, int startIndex, int endIndex, Guid sessionId, int userId)
        {
            IRow row;
            var xmlBuilder = new StringBuilder();

            for (int i = startIndex; i < endIndex; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    xmlBuilder.Append(GetVirtualRateXML(row, i + 1));
                }
            }
            if (xmlBuilder.Length == 0) return null;
            var criteria = new VerifyVirtualRateExcelTemplateCriteria(sportType, sessionId.ToString(), userId, $"<Rt>{xmlBuilder}</Rt>");
            var errors = DynamicRepository.Fetch<ExcelErrorDTO>(criteria);
            errors.ForEach(e => e.Value = e.Username);

            return new ExcelErrorModel(errors, criteria.ValidUser_Count, 0, null);
        }

        private string GetVirtualRateXML(IRow row, int rowNumber)
        {
            string multiplier;
            CellReference cellRef;
            var account = GetUsername(row);
            var xmlBuilder = new StringBuilder();

            foreach (VirtualRateBetTypeElement betType in betTypes)
            {
                cellRef = new CellReference(betType.Cell);
                multiplier = row.GetCell(cellRef.Col)?.ToString();
                xmlBuilder.Append(GetVirtualRateXML(account, GetExcelCell(cellRef, rowNumber), ParseMultiplier(multiplier), betType));
            }

            return xmlBuilder.ToString();
        }

        private static string GetUsername(IRow row)
        {
            var cellRef = new CellReference("A");

            return row.GetCell(cellRef.Col)?.ToString();
        }

        private static string GetExcelCell(CellReference cellRef, int rowNumber)
            => $"{cellRef.FormatAsString()}:{rowNumber}";

        private static string GetVirtualRateXML(string account, string cell, decimal? multiplier, VirtualRateBetTypeElement betType)
            => string.Format("<R UN=\"{0}\" Cell=\"{1}\" Mul=\"{2}\" LId=\"{3}\" BTGId=\"{4}\" BTId=\"{5}\" Live=\"{6}\"/>",
                account,
                cell,
                multiplier,
                betType.LeagueId,
                betType.BetTypeGroupId,
                betType.BetTypeId,
                betType.Live ? 1 : 0);

        private static decimal? ParseMultiplier(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !decimal.TryParse(value, out var number))
            {
                return null;
            }

            return (number / OneHundred) + 1;
        }

        #endregion

        public Result Update(int userId, string ip)
        {
            var result = DoAction(new UpdateVirtualRateExcelTemplateCriteria(sportType, GetSessionId().ToString(), userId, ip));

            // Ignore disabled account error.
            if (result.IsSuccess || result.Code == DisabledAccountErrCode)
            {
                RemoveSessionId();
                result.Message = ESportsResource.UploadVirtualRateSuccessfully;

                // Reset ErrCode in case of DisabledAccountErrCode.
                result.Code = 0;
            }

            return result;
        }

        private Guid GetSessionId()
            => SessionManager.Get<Guid>(GetSessionIdSessionKey());

        private void SetSessionId(Guid value)
            => SessionManager.Set(GetSessionIdSessionKey(), value);

        private void RemoveSessionId()
            => SessionManager.Set(GetSessionIdSessionKey(), null);

        private string GetSessionIdSessionKey()
            => string.Format(SessionIdSessionKey, sportType);
    }
}
