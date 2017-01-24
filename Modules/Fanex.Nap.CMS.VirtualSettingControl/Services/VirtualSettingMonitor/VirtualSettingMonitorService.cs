namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using NPOI.HSSF.UserModel;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using NPOI.SS.UserModel;

    public class VirtualSettingMonitorService : ServiceBase, IVirtualSettingMonitorService
    {
        private const string VirtualSettingsMonitorSessionKey = "Fanex.NAP.CMS.Reports.VirtualSettingsMonitor";
        private ExcelStyleTemplateByNPOI _excelStyleTemplate;

        public VirtualSettingMonitorService()
        {
        }

        public VirtualSettingViewModel GetVirtualSettingMonitor(GetVirtualSettingMonitorCriteria criteria, int[] monthList)
        {
            var custInfo = GetData<VirtualSettingModel>(criteria).ToList();

            var custMarginCriteria = new GetCustomerMarginHistoryCriteria
            {
                CustIds = string.Join(",", custInfo.Select(l => l.CustId)),
                MonthToCheck = 12, //criteria.MonthToCheck,
                SportId = criteria.SportId,
                ReportToDate = criteria.ReportToDate
            };

            var custMargin = GetData<VirtualSettingModel>(custMarginCriteria).ToList();
            var results = BindVirtualSettingData(custInfo, custMargin, monthList);
            SessionManager.Set<VirtualSettingViewModel>(VirtualSettingsMonitorSessionKey, results);

            return results;
        }

        public IEnumerable<MarginModel> GetCustomerVirtualSettingHistories(ICriteria criteria, int[] monthList)
        {
            var rawData = GetData<VirtualSettingModel>(criteria).ToList();
            return BindCustomerVirtualSettingHistories(rawData, monthList);
        }

        public void ExportVirtualSettingsMonitor(GetVirtualSettingMonitorCriteria criteria)
        {
            var reportData = SessionManager.Get<VirtualSettingViewModel>(VirtualSettingsMonitorSessionKey);

            HSSFWorkbook excelFile = new HSSFWorkbook();
            ExportExcelInfoByNPOI exportExcelInfo = new ExportExcelInfoByNPOI
            {
                ExcelFile = excelFile,
                Worksheet = excelFile.CreateSheet(Labels.VirtualSettingsMonitor),
                MaxColumnIndex = 17
            };

            _excelStyleTemplate = new ExcelStyleTemplateByNPOI(excelFile);
            _excelStyleTemplate.InitAllGeneralStyles();
            ExportVirtualSettingsMonitorHeader(exportExcelInfo, criteria);
            ExportVirtualSettingsMonitorBody(exportExcelInfo, reportData, criteria.MonthToCheck);
            var fileName = string.Format(Labels.VirtualSettingsMonitor_ExcelFileName, criteria.SportId == 0 ? "All" : criteria.SportId == 1 ? "Soccer" : "Other Sports", criteria.Rate, criteria.MonthToSum);
            ExcelHelper.ResponseStream(excelFile, ExcelHelper.FormatFileName(fileName));
        }

        #region Utilities

        public VirtualSettingViewModel BindVirtualSettingData(List<VirtualSettingModel> custInfo, List<VirtualSettingModel> custMargin, int[] monthList)
        {
            var virtualSettingViewModel = new VirtualSettingViewModel();
            var custMarginCategorized = custMargin.GroupBy(l => l.CustId);

            foreach (var cust in custInfo)
            {
                var listCustMargin = custMarginCategorized.Any(l => l.Key == cust.CustId)
                    ? custMarginCategorized.Single(l => l.Key == cust.CustId).Select(l => l).ToList()
                    : new List<VirtualSettingModel>();
                var newCust = new CustomerVirtualSettingModel
                {
                    CustId = cust.CustId,
                    Username = cust.Username,
                    VRate = cust.VRate
                };

                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;
                var encounterFirstDecember = false;
                var monthLength = monthList.Count() - 1;
                for (var i = monthLength; i >= 0; i --)
                {
                    if (monthList[i] == 12)
                    {
                        if (currentMonth == 12)
                        {
                            if (!encounterFirstDecember)
                            {
                                encounterFirstDecember = true;
                            }
                            else
                            {
                                currentYear = currentYear - 1;
                            }
                        }
                        else
                        {
                            currentYear = currentYear - 1;
                        }
                    }

                    var custSetting = FindVirtualSetting(listCustMargin, monthList[i], currentYear);
                    newCust.CustMargin.Add(custSetting != null
                            ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss, custSetting.BetCount)
                            : new MarginModel(monthList[i], currentYear));
                }

                newCust.CustMargin = newCust.CustMargin.OrderBy(l => l.Year).ThenBy(l => l.Month).ToList();

                // The Average Performance record
                var sumTurnover = newCust.CustMargin.Sum(l => l.Turnover);
                var sumWinloss = newCust.CustMargin.Sum(l => l.Winloss);
                var sumBet = newCust.CustMargin.Sum(l => l.BetCount);
                newCust.CustMargin.Add(new MarginModel(0, 0, sumTurnover, sumWinloss, sumBet));

                if (sumWinloss > 0)
                {
                    virtualSettingViewModel.CustMargin.Add(newCust);
                }
            }

            return virtualSettingViewModel;
        }

        public IEnumerable<MarginModel> BindCustomerVirtualSettingHistories(List<VirtualSettingModel> rawData, int[] monthList)
        {
            var custVirtualSettingHistory = new List<MarginModel>();
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var encounterFirstDecember = false;
            var monthLength = monthList.Count() - 1;
            for (var i = monthLength; i >= 0; i--)
            {
                if (monthList[i] == 12)
                {
                    if (currentMonth == 12)
                    {
                        if (!encounterFirstDecember)
                        {
                            encounterFirstDecember = true;
                        }
                        else
                        {
                            currentYear = currentYear - 1;
                        }
                    }
                    else
                    {
                        currentYear = currentYear - 1;
                    }
                }

                var custSetting = FindVirtualSetting(rawData, monthList[i], currentYear);
                custVirtualSettingHistory.Add(custSetting != null
                        ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss)
                        : new MarginModel(monthList[i], currentYear));
            }

            custVirtualSettingHistory = custVirtualSettingHistory.OrderBy(l => l.Year).ThenBy(l => l.Month).ToList();

            // The Average Performance record
            var sumTurnover = rawData.Sum(l => l.Turnover);
            var sumWinloss = rawData.Sum(l => l.Winloss);
            custVirtualSettingHistory.Add(new MarginModel(0, 0, sumTurnover, sumWinloss));

            return custVirtualSettingHistory;
        }

        public VirtualSettingModel FindVirtualSetting(List<VirtualSettingModel> rawData, int month, int year)
        {
            return rawData.Any(l => l.ReportMonth == month && l.ReportYear == year)
                ? rawData.First(l => l.ReportMonth == month && l.ReportYear == year)
                : null;
        }
        private void ExportVirtualSettingsMonitorHeader(
            ExportExcelInfoByNPOI exportExcelInfo,
            GetVirtualSettingMonitorCriteria criteria)
        {
            exportExcelInfo.SetColumnWidthByFontWidth(new int[] { 60, 160, 80, 80, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 });
            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.SetCurrentRowHeight(400);

            exportExcelInfo.RenderRow(Labels.VRAutoAdjustment, _excelStyleTemplate.HeaderTextStyle);

            exportExcelInfo.RenderRow(
                string.Format(Labels.VirtualSettingsMonitor_Legend, criteria.SportId == 0 ? "All" : criteria.SportId == 1 ? "Soccer" : "Other Sports", criteria.Rate, criteria.TopRows, criteria.MonthToSum), 
                _excelStyleTemplate.TextStyle);

            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.RenderCurrentCell(Labels.Symbol_No, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Member, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.VirtualRate, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Figure, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.AVGPerformance, _excelStyleTemplate.HeaderTextStyle);

            var months = Helper.GetDate(DateTime.Today, criteria.MonthToCheck, "MMM - yyyy");
            foreach (var month in months)
            {
                exportExcelInfo.RenderCurrentCell(month, _excelStyleTemplate.HeaderTextStyle);
            }

            exportExcelInfo.CurrentRow++;
        }

        private void ExportVirtualSettingsMonitorBody(
            ExportExcelInfoByNPOI exportExcelInfo,
            VirtualSettingViewModel reportData,
            int monthsToCheck)
        {
            var centerIntNumberStyle = NpoiExcelExtensions.IntStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false);
            centerIntNumberStyle.Alignment = HorizontalAlignment.Center;
            var centerTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (reportData != null && reportData.CustMargin.Any())
            {
                var rowIndex = 0;
                var counter = 0;
                
                var figureTexts = new string[4] { "Turnover", "Winloss", "Margin", "Bet Count" };
                foreach (var record in reportData.CustMargin)
                {
                    exportExcelInfo.ResetCurrentColumn();

                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 0, rowIndex + 1, centerIntNumberStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 1, record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 2, record.VRate, centerTextStyle);
                    
                    exportExcelInfo.RenderCurrentCell(figureTexts[0], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.CustMargin[monthsToCheck].Turnover, _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.CustMargin.ElementAt(counter).Turnover, _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 3;
                    exportExcelInfo.RenderCurrentCell(figureTexts[1], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.CustMargin[monthsToCheck].Winloss, _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.CustMargin.ElementAt(counter).Winloss, _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 3;
                    exportExcelInfo.RenderCurrentCell(figureTexts[2], centerTextStyle);

                    exportExcelInfo.RenderCurrentCell(record.CustMargin[monthsToCheck].Turnover == 0 ? 0 : Math.Round(record.CustMargin[monthsToCheck].Winloss / record.CustMargin[monthsToCheck].Turnover * 100, 2), _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.CustMargin.ElementAt(counter).Margin, _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 3;
                    exportExcelInfo.RenderCurrentCell(figureTexts[3], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.CustMargin.Sum(l => l.BetCount), _excelStyleTemplate.IntStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.CustMargin.ElementAt(counter).BetCount, _excelStyleTemplate.IntStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    rowIndex++;
                }
            }
            else
            {
                exportExcelInfo.RenderEmptyReport(NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            }
        }
        #endregion
    }
}