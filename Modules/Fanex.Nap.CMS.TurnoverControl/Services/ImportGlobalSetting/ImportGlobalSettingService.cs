using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            var (globalSetting, errorExcel) = DynamicRepository.FetchMultiple<GlobalSettingItem, ErrorGlobalSettingExcel>(new ValidateGlobalSettingCriteria(sportType, globalSettingExcels));

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

        public Result ImportGlobalSettings(short sportType, IEnumerable<GlobalSettingCustomer> globalSettings, int userId, string ip)
        {
            var result = DoAction(new AddMultipleGlobalSettingCriteria(globalSettings, sportType, userId, ip));

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.ImportGlobalSettingSuccessfully;
            }

            return result;
        }

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
