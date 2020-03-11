//namespace Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate
//{
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Text;
//    using Configurations;
//    using Criteria.UploadVirtualRate;
//    using Fanex.Logging.Extensions.Web;
//    using Infrastructure.Models;
//    using Infrastructure.Web;
//    using Models.UploadVirtualRate;
//    using NPOI.HSSF.UserModel;
//    using NPOI.SS.UserModel;
//    using NPOI.SS.Util;
//    using NPOI.XSSF.UserModel;
//    using Repositories.UploadVirtualRate;
//    using Resources;

//    public class ESportsUploadService : IESportsUploadService
//    {
//        private const int OneHundred = 100;
//        private const int UpdateBatchSize = 2;
//        private const int ESportsSportType = 43;
//        private const string XMLFormat = "<R UN=\"{0}\" Cell=\"{1}\" Mul=\"{2}\" LId=\"{3}\" BTGId=\"{4}\" BTId=\"{5}\" Live=\"{6}\"/>";
//        private const string UploadSessionKey = "Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate.ESportsUploadService";

//        private static readonly ESportVirtualRateBetTypeCollection betTypes = ESportsVirtualRateSection.Instance.BetTypes;

//        private readonly IUploadVirtualRateRepository uploadRepository;

//        public ESportsUploadService(IUploadVirtualRateRepository uploadRepository = null)
//        {
//            this.uploadRepository = uploadRepository ?? new UploadVirtualRateRepository();
//        }

//        public IEnumerable<ExcelErrorDTO> Verify(Stream stream, string fileExtension, int userId)
//        {
//            ISheet sheet = null;
//            var result = ValidateTemplate(stream, fileExtension, ref sheet);

//            return result != null
//                ? new[] { result }
//                : VerifyExcelTemplate(sheet, userId);
//        }

//        #region Validate Excel template

//        private static ExcelErrorDTO ValidateTemplate(Stream stream, string fileExtension, ref ISheet sheet)
//        {
//            IWorkbook workbook = GetWorkbook(stream, fileExtension);

//            if (workbook == null)
//            {
//                return new ExcelErrorDTO { Reason = ESportsResource.Error_NotExcelFile };
//            }

//            if (workbook.NumberOfSheets == 0)
//            {
//                return new ExcelErrorDTO { Reason = ESportsResource.Error_NotExcelFile };
//            }

//            sheet = workbook.GetSheetAt(0);

//            if (sheet == null)
//            {
//                return new ExcelErrorDTO { Reason = ESportsResource.Error_NotExcelFile };
//            }

//            return ValidateHeaderRow(sheet.GetRow(0));
//        }

//        private static IWorkbook GetWorkbook(Stream stream, string fileExtension)
//        {
//            try
//            {
//                if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
//                {
//                    return new HSSFWorkbook(stream);
//                }
//                else if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
//                {
//                    return new XSSFWorkbook(stream);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.Handle(ex);
//            }

//            return null;
//        }

//        private static ExcelErrorDTO ValidateHeaderRow(IRow headerRow)
//        {
//            if (headerRow == null || headerRow.Cells[0].ToString() != ESportsResource.Account)
//            {
//                return new ExcelErrorDTO { Reason = ESportsResource.Error_NotExcelFile };
//            }

//            ICell cell;
//            CellReference cellRef;

//            foreach (ESportVirtualRateBetTypeElement betType in betTypes)
//            {
//                cellRef = new CellReference(betType.Cell);
//                cell = headerRow.GetCell(cellRef.Col);

//                if (cell?.ToString() != betType.Key)
//                {
//                    return new ExcelErrorDTO { Reason = $"{ESportsResource.InvalidHeader} at {betType.Cell}1" };
//                }
//            }

//            return null;
//        }

//        #endregion

//        #region DB Validation

//        private IEnumerable<ExcelErrorDTO> VerifyExcelTemplate(ISheet sheet, int userId)
//        {
//            var sessionId = Guid.NewGuid();
//            SetSessionId(sessionId);

//            var startIndex = 1;
//            var endIndex = 1;
//            var totalRows = sheet.LastRowNum + 1;
//            var errors = new List<ExcelErrorDTO>();

//            do
//            {
//                endIndex += UpdateBatchSize;

//                if (endIndex > totalRows)
//                {
//                    endIndex = totalRows;
//                }

//                errors.AddRange(VerifyExcelTemplate(sheet, startIndex, endIndex, sessionId, userId));

//                startIndex = endIndex;
//            } while (startIndex < totalRows);

//            return errors;
//        }

//        private IEnumerable<ExcelErrorDTO> VerifyExcelTemplate(ISheet sheet, int startIndex, int endIndex, Guid sessionId, int userId)
//        {
//            IRow row;
//            string account;
//            string multiplier;
//            CellReference cellRef;
//            var xmlBuilder = new StringBuilder("<Rt>");

//            for (int i = startIndex; i < endIndex; i++)
//            {
//                row = sheet.GetRow(i);
//                account = row.Cells[0]?.ToString();

//                foreach (ESportVirtualRateBetTypeElement betType in betTypes)
//                {
//                    cellRef = new CellReference(betType.Cell);
//                    multiplier = row.GetCell(cellRef.Col)?.ToString();
//                    xmlBuilder.Append(GetVirtualRateXML(account, $"{cellRef.FormatAsString()}:{i + 1}", ParseMultiplier(multiplier), betType));
//                }
//            }

//            xmlBuilder.Append("</Rt>");

//            return uploadRepository.VerifyExcelTemplate(
//                new VerifyVirtualRateExcelTemplateCriteria(ESportsSportType, sessionId, userId, xmlBuilder.ToString()));
//        }

//        private static string GetVirtualRateXML(string account, string cell, decimal? multiplier, ESportVirtualRateBetTypeElement betType)
//            => string.Format("<R UN=\"{0}\" Cell=\"{1}\" Mul=\"{2}\" LId=\"{3}\" BTGId=\"{4}\" BTId=\"{5}\" Live=\"{6}\"/>",
//                account,
//                cell,
//                multiplier,
//                betType.LeagueId,
//                betType.BetTypeGroupId,
//                betType.BetTypeId,
//                betType.Live ? 1 : 0);

//        private static decimal? ParseMultiplier(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value) || !decimal.TryParse(value, out var number))
//            {
//                return null;
//            }

//            return (number / OneHundred) + 1;
//        }

//        #endregion

//        public Result Update(int userId, string ip)
//        {
//            var result = uploadRepository.ExecuteNonQuery(
//                new UpdateVirtualRateExcelTemplateCriteria(ESportsSportType, GetSessionId(), userId, ip));

//            if (result.IsSuccess)
//            {
//                RemoveSessionId();
//                result.Message = ESportsResource.UploadVirtualRateSuccessfully;
//            }

//            return result;
//        }

//        private static Guid GetSessionId()
//            => SessionManager.Get<Guid>(UploadSessionKey);

//        private static void SetSessionId(Guid value)
//            => SessionManager.Set(UploadSessionKey, value);

//        private static void RemoveSessionId()
//            => SessionManager.Set(UploadSessionKey, null);
//    }
//}
