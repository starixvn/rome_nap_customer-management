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
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Models.UploadVirtualRate;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using Repositories.UploadVirtualRate;
    using Repositories.VirtualRateDetails;
    using Resources;

    public class ESportsUploadService : IESportsUploadService
    {
        private const int OneHundred = 100;
        private const int UpdateBatchSize = 150;
        private const int ESportsSportType = 43;
        private const string XMLFormat = "<R UN=\"{0}\" Cell=\"{1}\" Mul=\"{2}\" LId=\"{3}\" BTGId=\"{4}\" BTId=\"{5}\" Live=\"{6}\"/>";
        private const string UploadSessionKey = "Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate.ESportsUploadService";

        private static readonly ESportVirtualRateBetTypeCollection betTypes = ESportsVirtualRateSection.Instance.BetTypes;

        private readonly IUploadVirtualRateRepository uploadRepository;
        private readonly IVirtualRateDetailsRepository virtualRateDetailsRepository;

        public ESportsUploadService(
            IUploadVirtualRateRepository uploadRepository = null,
            IVirtualRateDetailsRepository virtualRateDetailsRepository = null)
        {
            this.uploadRepository = uploadRepository ?? new UploadVirtualRateRepository();
            this.virtualRateDetailsRepository = virtualRateDetailsRepository ?? new VirtualRateDetailsRepository();
        }

        public ExcelErrorModel Verify(Stream stream, string fileExtension, int userId)
        {
            ISheet sheet = null;
            var excelErrorModel = ValidateByWeb(stream, fileExtension, ref sheet);

            return excelErrorModel.ExcelErrors.Any()
                ? excelErrorModel
                : ValidateBySP(sheet, userId);
        }

        #region Web Validation

        private ExcelErrorModel ValidateByWeb(Stream stream, string fileExtension, ref ISheet sheet)
        {
            var excelErrors = new List<ExcelErrorDTO>();
            IWorkbook workbook = GetWorkbook(stream, fileExtension);

            if (workbook == null)
            {
                excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_NotExcelFile));
            }
            else if (workbook.NumberOfSheets == 0)
            {
                excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_IncorrectLayout));
            }
            else
            {
                sheet = workbook.GetSheetAt(0);

                if (sheet == null)
                {
                    excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_IncorrectLayout));
                }
                else
                {
                    excelErrors.AddRange(ValidateHeaderRow(sheet.GetRow(0)));

                    if (excelErrors.Count == 0)
                    {
                        excelErrors.AddRange(ValidateBodyRows(sheet));
                    }
                }
            }

            var totalExcelErrors = excelErrors.Count;

            return new ExcelErrorModel(excelErrors, 0, totalExcelErrors, $"{ESportsResource.FormatError}: {totalExcelErrors}");
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

        private static IEnumerable<ExcelErrorDTO> ValidateHeaderRow(IRow headerRow)
        {
            IList<ExcelErrorDTO> excelErrors = new List<ExcelErrorDTO>();

            if (headerRow == null || headerRow.Cells[0].ToString() != ESportsResource.Account)
            {
                excelErrors.Add(new ExcelErrorDTO(ESportsResource.Error_NotExcelFile));
            }
            else
            {
                ICell cell;
                CellReference cellRef;

                foreach (ESportVirtualRateBetTypeElement betType in betTypes)
                {
                    cellRef = new CellReference(betType.Cell);
                    cell = headerRow.GetCell(cellRef.Col);

                    if (cell?.ToString() != betType.Key)
                    {
                        excelErrors.Add(new ExcelErrorDTO(betType.Cell + "1", null, cell?.ToString(), ESportsResource.InvalidHeader));
                    }
                }
            }

            return excelErrors;
        }

        private IEnumerable<ExcelErrorDTO> ValidateBodyRows(ISheet sheet)
        {
            IRow row;
            var actualTotalRows = 0;
            IList<ExcelErrorDTO> excelErrors = new List<ExcelErrorDTO>();
            var rateRanges = virtualRateDetailsRepository.GetVirtualRateRanges().ToDictionary(kvp => kvp.Value, kvp => true);

            var totalSheetRows = sheet.LastRowNum + 1;

            for (int i = 1; i < totalSheetRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    actualTotalRows++;
                    ValidateBodyRows(row, excelErrors, rateRanges, i + 1);
                }
            }

            return excelErrors;
        }

        private void ValidateBodyRows(IRow row, IList<ExcelErrorDTO> excelErrors, IDictionary<int, bool> rateRanges, int rowNumber)
        {
            string multiplier;
            var cellRef = new CellReference("A");
            var account = row.GetCell(cellRef.Col)?.ToString();

            if (string.IsNullOrWhiteSpace(account))
            {
                excelErrors.Add(
                    new ExcelErrorDTO(GetExcelCell(cellRef, rowNumber), account, account, ESportsResource.RequiredUsernameError));
            }

            foreach (ESportVirtualRateBetTypeElement betType in betTypes)
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

        #endregion

        #region DB Validation

        private ExcelErrorModel ValidateBySP(ISheet sheet, int userId)
        {
            var sessionId = Guid.NewGuid();
            SetSessionId(sessionId);

            var startIndex = 1;
            var endIndex = 1;
            var totalSheetRows = sheet.LastRowNum + 1;
            var validSetting = 0;
            var errors = new List<ExcelErrorDTO>();
            var actualTotalRows = 0;

            do
            {
                endIndex += UpdateBatchSize;

                if (endIndex > totalSheetRows)
                {
                    endIndex = totalSheetRows;
                }

                var errorExcelModel = VerifyExcelTemplate(sheet, startIndex, endIndex, sessionId, userId);
                validSetting += errorExcelModel.ValidSetting;
                actualTotalRows += errorExcelModel.TotalRows;
                errors.AddRange(errorExcelModel.ExcelErrors);

                startIndex = endIndex;
            } while (startIndex < totalSheetRows);

            return new ExcelErrorModel(errors, validSetting, actualTotalRows, $"{ESportsResource.DataError}: {errors.Count}");
        }

        private ExcelErrorModel VerifyExcelTemplate(ISheet sheet, int startIndex, int endIndex, Guid sessionId, int userId)
        {
            IRow row;
            var actualTotalRows = 0;
            var xmlBuilder = new StringBuilder("<Rt>");

            for (int i = startIndex; i < endIndex; i++)
            {
                row = sheet.GetRow(i);

                if (row != null && row.Cells.Count > 0)
                {
                    actualTotalRows++;
                    GetVirtualRateXML(xmlBuilder, row, i + 1);
                }
            }

            xmlBuilder.Append("</Rt>");

            var excelErrorModel = uploadRepository.VerifyExcelTemplate(
                new VerifyVirtualRateExcelTemplateCriteria(ESportsSportType, sessionId, userId, xmlBuilder.ToString()));
            excelErrorModel.TotalRows = actualTotalRows;

            return excelErrorModel;
        }

        private static string GetVirtualRateXML(StringBuilder xmlBuilder, IRow row, int rowNumber)
        {
            string multiplier;
            CellReference cellRef;
            var account = GetUsername(row);

            foreach (ESportVirtualRateBetTypeElement betType in betTypes)
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

        private static string GetVirtualRateXML(string account, string cell, decimal? multiplier, ESportVirtualRateBetTypeElement betType)
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
            var result = uploadRepository.ExecuteNonQuery(
                new UpdateVirtualRateExcelTemplateCriteria(ESportsSportType, GetSessionId(), userId, ip));

            if (result.IsSuccess)
            {
                RemoveSessionId();
                result.Message = ESportsResource.UploadVirtualRateSuccessfully;
            }

            return result;
        }

        private static Guid GetSessionId()
            => SessionManager.Get<Guid>(UploadSessionKey);

        private static void SetSessionId(Guid value)
            => SessionManager.Set(UploadSessionKey, value);

        private static void RemoveSessionId()
            => SessionManager.Set(UploadSessionKey, null);
    }
}
