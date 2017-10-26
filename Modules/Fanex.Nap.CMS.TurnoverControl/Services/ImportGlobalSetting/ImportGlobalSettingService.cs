using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Helpers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Criteria;
using Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using Fanex.NAP.Framework.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public class ImportGlobalSettingService : IImportGlobalSettingService
    {
        private const int BadRequest = (int)HttpStatusCode.BadRequest;

        private readonly IImportGlobalSettingRepository _importRepository;

        public ImportGlobalSettingService(IImportGlobalSettingRepository importRepository = null)
        {
            _importRepository = importRepository ?? new ImportGlobalSettingRepository();
        }

        public ImportGlobalSettingModel ReadExcelStream(SportType sportType, Stream stream, string fileExtension)
        {
            ISheet sheet = null;
            var result = ValidateTemplate(stream, fileExtension, ref sheet);

            if (result.Code != ErrorCodeConstants.NoneError)
            {
                return new ImportGlobalSettingModel(result.Code, result.Message);
            }

            var globalSettingExcels = ReadExcelStream(sheet);
            ICriteria criteria = new ValidateGlobalSettingCriteria(sportType, globalSettingExcels);

            return _importRepository.ValidateGlobalSettingExcels(criteria);
        }

        public Result ImportGlobalSettings(NonQueryCriteria criteria)
        {
            var result = _importRepository.ImportGlobalSettings(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.ImportGlobalSettingSuccessfully;
            }

            return result;
        }

        #region Validate excel template

        private static Result ValidateTemplate(Stream stream, string fileExtension, ref ISheet sheet)
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

            IRow headerRow = sheet.GetRow(0);

            if (InvalidHeaderRow(headerRow))
            {
                return new Result(BadRequest, Labels.Error_IncorrectLayout);
            }

            return new Result();
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
                Logs.Error(ex);
            }

            return null;
        }

        private static bool InvalidHeaderRow(IRow headerRow)
        {
            if (headerRow == null || headerRow.LastCellNum < 3)
            {
                return true;
            }

            string userName = headerRow.Cells[0].ToString();

            if (string.IsNullOrEmpty(userName) ||
                !userName.Trim().Equals(Labels.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string headline = headerRow.Cells[1].ToString();

            if (string.IsNullOrEmpty(headline) ||
                !headline.Trim().Equals(Labels.Headline, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string maxTurnover = headerRow.Cells[2].ToString();

            if (string.IsNullOrEmpty(maxTurnover) ||
                !maxTurnover.Trim().Equals(Labels.Max_Turnover, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static IEnumerable<GlobalSettingExcel> ReadExcelStream(ISheet sheet)
        {
            IRow row;
            int totalRows = sheet.LastRowNum + 1;
            GlobalSettingExcel setting;

            for (int i = 1; i < totalRows; i++)
            {
                row = sheet.GetRow(i);

                if (row != null)
                {
                    setting = GetGlobalSettingExcel(row);

                    if (!string.IsNullOrEmpty(setting.UserName) ||
                        !string.IsNullOrEmpty(setting.Headline) ||
                        !string.IsNullOrEmpty(setting.MaxTurnover))
                    {
                        yield return setting;
                    }
                }
            }
        }

        private static GlobalSettingExcel GetGlobalSettingExcel(IRow row)
        {
            GlobalSettingExcel globalSettingExcel = new GlobalSettingExcel();

            foreach (ICell cell in row.Cells)
            {
                switch (cell.ColumnIndex)
                {
                    case 0:
                        globalSettingExcel.UserName = cell.ToString().Trim();
                        break;
                    case 1:
                        globalSettingExcel.Headline = cell.ToString().Trim();
                        break;
                    case 2:
                        globalSettingExcel.MaxTurnover = cell.ToString().Trim();
                        break;
                    default:
                        continue;
                }
            }

            return globalSettingExcel;
        }

        #endregion
    }
}
