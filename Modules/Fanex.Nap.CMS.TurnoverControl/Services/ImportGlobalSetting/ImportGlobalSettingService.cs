using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Fanex.Logging.Extensions.Web;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public class ImportGlobalSettingService : ServiceBase, IImportGlobalSettingService
    {
        private const int BadRequest = (int)HttpStatusCode.BadRequest;
        private const int UsernameColumnIndex = 0;
        private const int HeadlineColumnIndex = 1;
        private const int StartOfMaxTurnoverColumnIndex = 2;

        private readonly IGlobalSettingService globalSettingService;

        public ImportGlobalSettingService(IGlobalSettingService globalSettingService = null) 
        {
            this.globalSettingService = globalSettingService ?? new GlobalSettingService();
        }

        public ImportGlobalSettingModel ReadExcelStream(short sportType, Stream stream, string fileExtension)
        {
            ISheet sheet = null;
            var profiles = globalSettingService.GetProfiles(sportType).ToList();
            var result = ValidateTemplate(stream, fileExtension, profiles, ref sheet);

            if (!result.IsSuccess)
            {
                return new ImportGlobalSettingModel(result.Code, result.Message);
            }

            var globalSettingExcels = ReadExcelStream(sheet, profiles);
            var criteria = new ValidateGlobalSettingCriteria
            {
                GlobalSetting = ToValidateTemplateXml(globalSettingExcels, sportType),
                SportType = sportType
            };

            var (globalSetting, errorExcel) = DynamicRepository.FetchMultiple<GlobalSettingItem, ErrorGlobalSettingExcel>(criteria);

            var validSettings = globalSetting
                    .GroupBy(g => new { g.CustId, g.Username, g.Level, g.Currency })
                    .Select(grp => new GlobalSettingCustomer(
                        grp.Key.CustId,
                        grp.Key.Username,
                        grp.Key.Level,
                        false,
                        grp.Key.Currency,
                        grp.Select(g => new MaxTurnoverItem(g.ProfileId, g.MaxTurnover)).ToList()));

            return new ImportGlobalSettingModel(validSettings, errorExcel);
        }

        private static string ToValidateTemplateXml(IEnumerable<GlobalSettingExcel> globalSettings, int sportType)
        {
            var builder = new StringBuilder();
            builder.Append("<ROOT>");

            foreach (var item in globalSettings)
            {
                foreach (var mto in item.MaxTurnovers)
                {
                    builder.Append(ToValidateTemplateXml(item, mto, sportType));
                }
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private static string ToValidateTemplateXml(GlobalSettingExcel globalSetting, MaxTurnoverExcel mto, int sportType)
        {
            var builder = new StringBuilder("<gs ")
                .Append("us=\"").Append(globalSetting.Username).Append("\" ")
                .Append("isGH=\"").Append(globalSetting.Headline).Append("\" ")
                .Append("maxTO=\"").Append(mto.MaxTurnover).Append("\" ")
                .Append("spt=\"").Append(sportType).Append("\" ")
                .Append("pfID=\"").Append(mto.ProfileId).Append("\" ")
                .Append("rowID=\"").Append(globalSetting.ExcelRow).Append("\"")
                .Append("/>");

            return builder.ToString();
        }

        public Result ImportGlobalSettings(short sportType, IEnumerable<GlobalSettingCustomer> globalSettings, int userId, string ip)
        {
            var criteria = new AddMultipleGlobalSettingCriteria
            {
                GlobalSetting = ToXml(globalSettings, sportType),
                UserId = userId,
                IP = ip
            };

            var result = DoAction(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.ImportGlobalSettingSuccessfully;
            }

            return result;
        }

        private static string ToXml(IEnumerable<GlobalSettingCustomer> globalSettings, int sportType)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in globalSettings)
            {
                foreach (var mto in item.MaxTurnovers)
                {
                    xmlBuilder.Append(ToXml(item, mto, sportType));
                }
            }

            xmlBuilder.Append("</ROOT>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(GlobalSettingCustomer item, MaxTurnoverItem mto, int sportType)
        {
            var builder = new StringBuilder("<gs ")
                .Append("us=\"").Append(item.Username).Append("\" ")
                .Append("isGH=\"").Append(GetIsGroupHeadlineValue(item.Level)).Append("\" ")
                .Append("maxTO=\"").Append(mto.MaxTurnover).Append("\" ")
                .Append("spt=\"").Append(sportType).Append("\" ")
                .Append("pfID=\"").Append(mto.ProfileId).Append("\"")
                .Append("/>");

            return builder.ToString();
        }

        private static int GetIsGroupHeadlineValue(string level)
            => !string.IsNullOrEmpty(level) && level.Equals("headline", StringComparison.OrdinalIgnoreCase)
            ? 1
            : 0;

        #region Validate excel template

        private static Result ValidateTemplate(
            Stream stream,
            string fileExtension,
            IList<IdNameModel> profiles,
            ref ISheet sheet)
        {
            IWorkbook workbook = GetWorkbook(stream, fileExtension);

            if (workbook == null || workbook.NumberOfSheets == 0)
            {
                return new Result(BadRequest, Labels.Error_IncorrectLayout);
            }

            sheet = workbook.GetSheetAt(0);

            if (sheet == null)
            {
                return new Result(BadRequest, Labels.Error_IncorrectLayout);
            }

            return InvalidHeaderRow(sheet.GetRow(0), profiles)
                ? new Result(BadRequest, Labels.Error_IncorrectLayout)
                : new Result();
        }

        private static IWorkbook GetWorkbook(Stream stream, string fileExtension)
        {
            try
            {
                if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    return new HSSFWorkbook(stream);
                }
                else if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new XSSFWorkbook(stream);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return null;
        }

        private static bool InvalidHeaderRow(IRow headerRow, IEnumerable<IdNameModel> profiles)
        {
            const int TotalColumns = 12;
            var columnIndex = StartOfMaxTurnoverColumnIndex;

            return headerRow == null
                || headerRow.LastCellNum < TotalColumns
                || InvalidHeaderRow(headerRow.Cells[UsernameColumnIndex].ToString(), Labels.UserName)
                || InvalidHeaderRow(headerRow.Cells[HeadlineColumnIndex].ToString(), Labels.Headline)
                || profiles.Any(p => InvalidHeaderRow(headerRow.Cells[columnIndex++].ToString(), p.Name));
        }

        private static bool InvalidHeaderRow(string source, string destination)
            => string.IsNullOrEmpty(source) || !source.Trim().Equals(destination, StringComparison.OrdinalIgnoreCase);

        private static IEnumerable<GlobalSettingExcel> ReadExcelStream(ISheet sheet, IList<IdNameModel> profiles)
        {
            IRow row;
            var totalRows = sheet.LastRowNum + 1;

            for (int i = 1; i < totalRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null)
                {
                    yield return GetGlobalSettingExcel(row, profiles, i + 1);
                }
            }
        }

        private static GlobalSettingExcel GetGlobalSettingExcel(IRow row, IList<IdNameModel> profiles, int excelRow)
        {
            var username = string.Empty;
            var headline = string.Empty;
            IList<MaxTurnoverExcel> maxTurnoverExcels = new List<MaxTurnoverExcel>();

            foreach (ICell cell in row.Cells)
            {
                switch (cell.ColumnIndex)
                {
                    case UsernameColumnIndex:
                        username = cell.ToString().Trim();
                        break;
                    case HeadlineColumnIndex:
                        headline = cell.ToString().Trim();
                        break;
                    default:
                        var columnIndex = cell.ColumnIndex - StartOfMaxTurnoverColumnIndex;
                        maxTurnoverExcels.Add(new MaxTurnoverExcel(profiles[columnIndex].Id, cell.ToString().Trim()));
                        break;
                }
            }

            return new GlobalSettingExcel(username, headline, maxTurnoverExcels, excelRow);
        }

        #endregion
    }
}
