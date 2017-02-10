using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.NAP.CMS.Criteria.Common;
    using Models.VrAutoAdjustmentReport;
    using NPOI.HSSF.UserModel;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using NPOI.SS.UserModel;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport;

    public class AdjustmentReportService : ServiceBase, IAdjustmentReportService
    {
        private ExcelStyleTemplateByNPOI _excelStyleTemplate;

        public IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria)
        {
            return this.GetData<VrAutoAdjustmentLogEntry>(criteria);
        }

        public ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck)
        {
            var criteria = new GetCustomerMarginByMonthCriteria
            {
                CustIds = string.Join(", ", custIds),
                MonthToCheck = monthsToCheck,
                ReportToDate = reportDate
            };

            var result = GetData<VirtualSettingModel>(criteria);

            return result.ToLookup(m => m.CustId, m => new MarginModel(m.ReportMonth, m.ReportYear, m.Turnover, m.Winloss, m.BetCount));
        }

        public List<CustomerMarginModel> GetCustomerMarginHistory(ICriteria criteria, int monthsToCheck, DateTime AdjustDate)
        {
            var marginList = this.GetData<MarginModel>(criteria);
            var result = new List<CustomerMarginModel>();
            var groupItem = marginList.GroupBy(m => m.Username);

            foreach (var item in groupItem)
            {
                var marginModelList = new List<MarginModel>();


                var model = new CustomerMarginModel
                {
                    Username = item.Key,
                    MarginList = item.ToList()
                };

                if (model.MarginList.Count < monthsToCheck)
                {
                    var lastMonth = new DateTime(AdjustDate.Year, AdjustDate.Month, 1);

                    for (var i = 0; i < monthsToCheck; i++)
                    {
                        var month = lastMonth.AddMonths(-i);

                        if (!model.MarginList.Exists(t => t.ReportDate == month))
                        {
                            model.MarginList.Add(new MarginModel { Month = month.Month, Year = month.Year });
                        }
                    }
                }

                model.MarginList = model.MarginList.OrderBy(t => t.ReportDate).ToList();

                result.Add(model);
            }

            return result;
        }

        public void ExportVRAdjustmentReport(GetVrAdjustmentLogCriteria criteria, int monthsToCheck)
        {
            var reportData = this.GetData<VrAutoAdjustmentLogEntry>(criteria).ToList();
            var reportDetailsCriteria = new GetCustomerMarginByMonthCriteria
            {
                CustIds = string.Join(", ", reportData.Select(l => l.CustId)),
                MonthToCheck = monthsToCheck,
                ReportToDate = DateTime.Today
            };

            var reportDetails = GetData<VirtualSettingModel>(reportDetailsCriteria).ToLookup(m => m.CustId, m => new MarginModel(m.ReportMonth, m.ReportYear, m.Turnover, m.Winloss, m.BetCount));

            HSSFWorkbook excelFile = new HSSFWorkbook();
            ExportExcelInfoByNPOI exportExcelInfo = new ExportExcelInfoByNPOI
            {
                ExcelFile = excelFile,
                Worksheet = excelFile.CreateSheet(Labels.VRAutoAdjustment),
                MaxColumnIndex = 19
            };

            _excelStyleTemplate = new ExcelStyleTemplateByNPOI(excelFile);
            _excelStyleTemplate.InitAllGeneralStyles();
            ExportVRAdjustmentReportHeader(exportExcelInfo, criteria, monthsToCheck);
            ExportVRAdjustmentReportBody(exportExcelInfo, reportData.Select(l => new LogEntryViewModel(l, reportDetails[l.CustId])), monthsToCheck);
            var fileName = string.Format(Labels.VRAutoAdjustment_ExcelFileName, DateTime.Now.ToShortFormatedDateWithSlash());
            ExcelHelper.ResponseStream(excelFile, ExcelHelper.FormatFileName(fileName));
        }

        public IEnumerable<CustomerMarginDateModel> GetReportDate(ICriteria criteria)
        {
            return this.GetData<CustomerMarginDateModel>(criteria);
        }

        #region Utilities

        private void ExportVRAdjustmentReportHeader(
            ExportExcelInfoByNPOI exportExcelInfo,
            GetVrAdjustmentLogCriteria criteria,
            int monthsToCheck)
        {
            exportExcelInfo.SetColumnWidthByFontWidth(new int[] { 60, 160, 120, 80, 80, 80, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 });
            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.SetCurrentRowHeight(400);

            exportExcelInfo.RenderRow(Labels.VRAutoAdjustment, _excelStyleTemplate.HeaderTextStyle);

            exportExcelInfo.RenderRow(string.Format(Labels.VRAutoAdjustment_Legend, criteria.FromDate.ToShortFormatedDateWithSlash(), criteria.ToDate.ToShortFormatedDateWithSlash()), _excelStyleTemplate.TextStyle);

            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.RenderCurrentCell(Labels.Symbol_No, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Member, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.UpdatedDate, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.OldRate, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.NewRate, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Figure, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.AVGPerformance, _excelStyleTemplate.HeaderTextStyle);

            var months = Helper.GetDate(DateTime.Today, monthsToCheck, "MMM - yyyy");
            foreach (var month in months)
            {
                exportExcelInfo.RenderCurrentCell(month, _excelStyleTemplate.HeaderTextStyle);
            }

            exportExcelInfo.CurrentRow++;
        }
        private void ExportVRAdjustmentReportBody(
            ExportExcelInfoByNPOI exportExcelInfo,
            IEnumerable<LogEntryViewModel> reportData,
            int monthsToCheck)
        {
            var centerIntNumberStyle = NpoiExcelExtensions.IntStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false);
            centerIntNumberStyle.Alignment = HorizontalAlignment.Center;
            var centerTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (reportData != null && reportData.Any())
            {
                var rowIndex = 0;
                var counter = 0;
                var figureTexts = new string[4] { "Turnover", "Winloss", "Margin", "Bet Count" };
                foreach (var record in reportData)
                {
                    exportExcelInfo.ResetCurrentColumn();

                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 0, rowIndex + 1, centerIntNumberStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 1, record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 2, record.UpdatedDate, centerTextStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 3, record.OldRate, centerTextStyle);
                    exportExcelInfo.RenderCellBySpanning(exportExcelInfo.CurrentRow + 3, 4, record.NewRate, centerTextStyle);

                    exportExcelInfo.RenderCurrentCell(figureTexts[0], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.AvgPerformance.Turnover, _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.MarginViewModels.ElementAt(counter).Turnover, _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 5;
                    exportExcelInfo.RenderCurrentCell(figureTexts[1], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.AvgPerformance.Winloss, _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.MarginViewModels.ElementAt(counter).Winloss, _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 5;
                    exportExcelInfo.RenderCurrentCell(figureTexts[2], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell((Math.Round(record.AvgPerformance.Margin * 100, 2)).ToString() + "%", _excelStyleTemplate.DecimalStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell((Math.Round(record.MarginViewModels.ElementAt(counter).Margin * 100, 2)).ToString() + "%", _excelStyleTemplate.DecimalStyle);
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 5;
                    exportExcelInfo.RenderCurrentCell(figureTexts[3], centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.AvgPerformance.BetCount, _excelStyleTemplate.IntStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        exportExcelInfo.RenderCurrentCell(record.MarginViewModels.ElementAt(counter).BetCount, _excelStyleTemplate.IntStyle);
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