// --------------------------------------------------------------------------
// <copyright file="GlobalSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
    using Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting;
    using Fanex.Nap.CMS.TurnoverControl.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Global Setting Service.
    /// </summary>
    public class GlobalSettingService : IGlobalSettingService
    {
        private static readonly string GlobalSettingListSessionKey = 
            string.Format("Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.ListSessionKey.{0}", AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        private IGlobalSettingRepository _globalSettingRepository;

        private ExcelStyleTemplateByNPOI _excelStyleTemplate;

        public GlobalSettingService()
            : this(null)
        {
        }

        public GlobalSettingService(IGlobalSettingRepository globalSettingRepository)
        {
            _globalSettingRepository = globalSettingRepository ?? new GlobalSettingRepository();
        }

        /// <summary>
        /// Gets the global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The global setting list.</returns>
        public List<CustomerInfo> GlobalSettingList(GlobalSettingListCriteria criteria)
        {
            List<CustomerInfo> result = _globalSettingRepository.GlobalSettingList(criteria);
            SessionManager.AddSession(HttpContext.Current.Session, GlobalSettingListSessionKey, result);

            if (!string.IsNullOrEmpty(criteria.SortField))
            {
                SortGlobalSettingList(criteria);
            }

            return result;
        }

        /// <summary>
        /// Sorts the global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The global setting list.</returns>
        public List<CustomerInfo> SortGlobalSettingList(GlobalSettingListCriteria criteria)
        {
            var result = (List<CustomerInfo>)SessionManager.GetSession(HttpContext.Current.Session, GlobalSettingListSessionKey) ??
                GlobalSettingList(criteria);
            if (string.Equals(criteria.SortDirection, SortDirectionConstants.Asc, StringComparison.OrdinalIgnoreCase))
            {
                return SortGlobalSettingListByAscending(result, criteria);
            }

            if (string.Equals(criteria.SortDirection, SortDirectionConstants.Desc, StringComparison.OrdinalIgnoreCase))
            {
                return SortGlobalSettingListByDescending(result, criteria);
            }

            return result;
        }

        /// <summary>
        /// Exports the global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportGlobalSettingList(GlobalSettingListCriteria criteria)
        {
            var globalSettingList = (List<CustomerInfo>)SessionManager.GetSession(HttpContext.Current.Session, GlobalSettingListSessionKey) ??
                GlobalSettingList(criteria);
            HSSFWorkbook excelFile = new HSSFWorkbook();
            ExportExcelInfoByNPOI exportExcelInfo = new ExportExcelInfoByNPOI
            {
                ExcelFile = excelFile,
                Worksheet = excelFile.CreateSheet(Labels.GlobalSetting_SheetName),
                MaxColumnIndex = 4
            };

            _excelStyleTemplate = new ExcelStyleTemplateByNPOI(excelFile);
            _excelStyleTemplate.InitAllGeneralStyles();
            ExportGlobalSettingListHeader(exportExcelInfo);
            ExportGlobalSettingListBody(exportExcelInfo, globalSettingList);
            var fileName = string.Format(Labels.GlobalSetting_ExcelFileName, DateTime.Now.ToShortFormatedDateWithSlash());
            ExcelHelper.ResponseStream(excelFile, ExcelHelper.FormatFileName(fileName));
        }

        public Result EditGlobalSetting(EditGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.EditGlobalSetting(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.GlobalSetting_EditMaxTurnover_Success;
            }

            return result;
        }

        public Result AddGlobalSetting(AddGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.AddGlobalSetting(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = string.Format(Labels.GlobalSetting_AddMaxTurnover_Success, criteria.Name);
            }

            return result;
        }

        public Result DeleteGlobalSetting(DeleteGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.DeleteGlobalSetting(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.GlobalSetting_DeleteMaxTurnover_Success;
            }

            return result;
        }

        /// <summary>
        /// Sorts the global setting list by ascending.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The global setting list.
        /// </returns>
        private List<CustomerInfo> SortGlobalSettingListByAscending(
            List<CustomerInfo> result,
            GlobalSettingListCriteria criteria)
        {
            switch (criteria.SortField)
            {
                case "Name":
                    return result.OrderBy(customerInfo => customerInfo.Name)
                        .ToList();
                case "RoleName":
                    return result.OrderBy(customerInfo => customerInfo.RoleInfo.Name)
                        .ToList();
                case "CurrencyName":
                    return result.OrderBy(customerInfo => customerInfo.Currency.Name)
                        .ToList();
                case "MaxTurnover":
                    return result.OrderBy(customerInfo => customerInfo.MaxTurnover)
                        .ToList();
                default:
                    return result;
            }
        }

        /// <summary>
        /// Sorts the global setting list by descending.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The global setting list.
        /// </returns>
        private List<CustomerInfo> SortGlobalSettingListByDescending(
            List<CustomerInfo> result,
            GlobalSettingListCriteria criteria)
        {
            switch (criteria.SortField)
            {
                case "Name":
                    return result.OrderByDescending(customerInfo => customerInfo.Name)
                        .ToList();
                case "RoleName":
                    return result.OrderByDescending(customerInfo => customerInfo.RoleInfo.Name)
                        .ToList();
                case "CurrencyName":
                    return result.OrderByDescending(customerInfo => customerInfo.Currency.Name)
                        .ToList();
                case "MaxTurnover":
                    return result.OrderByDescending(customerInfo => customerInfo.MaxTurnover)
                        .ToList();
                default:
                    return result;
            }
        }

        /// <summary>
        /// Exports the global setting list header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportGlobalSettingListHeader(ExportExcelInfoByNPOI exportExcelInfo)
        {
            exportExcelInfo.SetColumnWidthByFontWidth(new int[] { 60, 160, 120, 160, 160 });
            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.SetCurrentRowHeight(400);

            exportExcelInfo.RenderRow(Labels.GlobalSetting_ExcelTitle, _excelStyleTemplate.HeaderTextStyle);

            exportExcelInfo.RenderEmptyRow(_excelStyleTemplate.TextStyle);

            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.RenderCurrentCell(Labels.Symbol_No, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.SMA_Name, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Level, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Currency, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Max_Turnover, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the global setting list body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="globalSettingList">The global setting list.</param>
        private void ExportGlobalSettingListBody(
            ExportExcelInfoByNPOI exportExcelInfo,
            List<CustomerInfo> globalSettingList)
        {
            var centerIntNumberStyle = NpoiExcelExtensions.IntStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false);
            centerIntNumberStyle.Alignment = HorizontalAlignment.Center;
            var centerTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (globalSettingList != null && globalSettingList.Any())
            {
                var globalSettingListLength = globalSettingList.Count;

                for (var i = 0; i < globalSettingListLength; i++)
                {
                    var customerInfo = globalSettingList[i];
                    exportExcelInfo.ResetCurrentColumn();

                    exportExcelInfo.RenderCurrentCell(i + 1, centerIntNumberStyle);
                    exportExcelInfo.RenderCurrentCell(customerInfo.Name, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(customerInfo.RoleInfo.Name, centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(customerInfo.Currency.Name, centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(customerInfo.MaxTurnover, _excelStyleTemplate.IntStyle);
                    exportExcelInfo.CurrentRow++;
                }
            }
            else
            {
                exportExcelInfo.RenderEmptyReport(NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            }
        }
    }
}
