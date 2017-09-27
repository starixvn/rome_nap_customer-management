namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitorAdvance
{
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitorAdvance;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using NPOI.HSSF.UserModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VirtualSettingMonitorAdvanceService : ServiceBase, IVirtualSettingMonitorAdvanceService
    {
        private const string VirtualSettingsMonitorSessionKey = "Fanex.NAP.CMS.Reports.VirtualSettingsMonitorAdvance";
        private ExcelStyleTemplateByNPOI _excelStyleTemplate;

        public List<CustomerVirtualSettingAdvanceModel> GetVirtualSettingAdvanceMonitor(GetVirtualSettingMonitorAdvanceCriteria criteria, int monthsToCheck)
        {
            var marginList = GenModels(); ////this.GetData<VirtualSettingAdvanceModel>(criteria);
            var result = new List<CustomerVirtualSettingAdvanceModel>();

            var groupItem = marginList.GroupBy(m => m.Username);

            var lastMonth = new DateTime(criteria.ToDate.Year, criteria.ToDate.Month, 1);

            foreach (var item in groupItem)
            {
                var model = new CustomerVirtualSettingAdvanceModel
                {
                    Username = item.Key,
                    Items = item.ToList()
                };

                model.CustId = model.Items[0].CustId;

                for (var i = 0; i < monthsToCheck; i++)
                {
                    var month = lastMonth.AddMonths(-i);

                    if (!model.Items.Exists(t => t.ReportDate == month))
                    {
                        model.Items.Add(new VirtualSettingAdvanceModel { ReportMonth = month.Month, ReportYear = month.Year });
                    }
                }

                model.Items = model.Items.OrderByDescending(t => t.ReportDate).Take(monthsToCheck).OrderBy(t => t.ReportDate).ToList();

                result.Add(model);
            }

            SessionManager.Set<List<CustomerVirtualSettingAdvanceModel>>(VirtualSettingsMonitorSessionKey, result);

            return result;
        }

        public void ExportVirtualSettingAdvanceMonitor(GetVirtualSettingMonitorAdvanceCriteria criteria, int monthsToCheck)
        {
            var data = SessionManager.Get<List<CustomerVirtualSettingAdvanceModel>>(VirtualSettingsMonitorSessionKey);
            var fileName = string.Format("Virtual_Setting_Monitor_Advance_{0}", DateTime.Now.ToShortFormatedDateWithSlash());

            if (data == null)
            {
                data = GetVirtualSettingAdvanceMonitor(criteria, monthsToCheck);
            }

            var excelFile = new HSSFWorkbook();
            var exportExcelInfo = new ExportExcelInfoByNPOI
            {
                ExcelFile = excelFile,
                Worksheet = excelFile.CreateSheet("Virtual Setting Monitor"),
                MaxColumnIndex = 3 + monthsToCheck
            };

            _excelStyleTemplate = new ExcelStyleTemplateByNPOI(excelFile);
            _excelStyleTemplate.InitAllGeneralStyles();
            ExportHeader(exportExcelInfo, criteria, monthsToCheck);
            ExportBody(exportExcelInfo, data, monthsToCheck);

            ExcelHelper.ResponseStream(excelFile, ExcelHelper.FormatFileName(fileName));
        }

        #region excel
        private void ExportHeader(ExportExcelInfoByNPOI exportExcelInfo, GetVirtualSettingMonitorAdvanceCriteria criteria, int monthsToCheck)
        {
            var widthHeader = new List<int> { 60, 160, 160, 150 };
            for (var i = 0; i < monthsToCheck; i++)
            {
                widthHeader.Add(150);
            }

            exportExcelInfo.SetColumnWidthByFontWidth(widthHeader.ToArray());
            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.SetCurrentRowHeight(400);

            exportExcelInfo.RenderRow("Virtual Setting Monitor - Advance", _excelStyleTemplate.HeaderTextStyle);

            exportExcelInfo.RenderRow(
                string.Format(
                    "Sport: {0}, Rate: {1}, From: {2} - To: {3}, Depth: {4}, Account Name: {5}",
                    GetSportString(criteria.SportId),
                    criteria.Rate,
                    criteria.FromDate.ToShortDateString(),
                    criteria.ToDate.ToShortDateString(),
                    GetDepthString(criteria.Depth),
                    criteria.Username),
                _excelStyleTemplate.TextStyle);

            exportExcelInfo.CreateCurrentNPOIRow();
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.RenderCurrentCell("#", _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell("Account Name", _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell(Labels.Figure, _excelStyleTemplate.HeaderTextStyle);
            exportExcelInfo.RenderCurrentCell("Avg. Performance", _excelStyleTemplate.HeaderTextStyle);

            var months = Helper.GetDate(criteria.ToDate, monthsToCheck, "MMM - yyyy");
            foreach (var month in months)
            {
                exportExcelInfo.RenderCurrentCell(month, _excelStyleTemplate.HeaderTextStyle);
            }

            exportExcelInfo.CurrentRow++;
        }

        private void ExportBody(ExportExcelInfoByNPOI exportExcelInfo, List<CustomerVirtualSettingAdvanceModel> reportData, int monthsToCheck)
        {
            var centerTextStyle = NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, _excelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (reportData != null && reportData.Any())
            {
                var rowIndex = 0;
                var counter = 0;
                var figureTexts = new[] { "Turnover", "Winloss", "Margin (%)", "Bet Count", "Variance Turnover", "Variance Winloss", "Variance Margin (%)" };
                foreach (var record in reportData)
                {
                    exportExcelInfo.ResetCurrentColumn();

                    exportExcelInfo.RenderCurrentCell(rowIndex + 1, centerTextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[0], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.Turnover, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).Turnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).Turnover, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[1], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.Winloss, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).Turnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).Winloss, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[2], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.Margin, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).Turnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).Margin, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[3], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.BetCount, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).Turnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).BetCount, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[4], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.Turnover, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).VarianceTurnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).VarianceTurnover, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[5], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.VarianceWinloss, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).VarianceTurnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).VarianceWinloss, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
                    }

                    exportExcelInfo.CurrentRow++;
                    exportExcelInfo.CurrentColumn = 1;
                    exportExcelInfo.RenderCurrentCell(record.Username, _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(figureTexts[6], _excelStyleTemplate.TextStyle);
                    exportExcelInfo.RenderCurrentCell(record.Total.VarianceMargin, _excelStyleTemplate.DecimalColorStyle);
                    for (counter = 0; counter < monthsToCheck; counter++)
                    {
                        if (record.Items.ElementAt(counter).VarianceTurnover > 0)
                        {
                            exportExcelInfo.RenderCurrentCell(record.Items.ElementAt(counter).VarianceMargin, _excelStyleTemplate.DecimalColorStyle);
                        }
                        else
                        {
                            exportExcelInfo.RenderCurrentCell(string.Empty, _excelStyleTemplate.DecimalColorStyle);
                        }
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

        private string GetSportString(int sportId)
        {
            var result = string.Empty;

            switch (sportId)
            {
                case 0:
                    result = "All";
                    break;
                case 1:
                    result = "Soccer";
                    break;
                case 2:
                    result = "Other Sports";
                    break;
                default:
                    result = "All";
                    break;
            }

            return result;
        }

        private string GetDepthString(int depth)
        {
            var result = string.Empty;

            switch (depth)
            {
                case 0:
                    result = "Super";
                    break;
                case 1:
                    result = "Master";
                    break;
                case 2:
                    result = "Agent";
                    break;
                case 3:
                    result = "Member";
                    break;
                default:
                    result = "Member";
                    break;
            }

            return result;
        }
        #endregion

        #region Test Data
        private List<CustomerVirtualSettingAdvanceModel> Gendata()
        {
            var result = new List<CustomerVirtualSettingAdvanceModel>();

            for (var i = 0; i < 10; i++)
            {
                var item = new CustomerVirtualSettingAdvanceModel
                {
                    CustId = 100,
                    Items = GenModels(),
                    Username = "Cuong" + i
                };

                result.Add(item);
            }

            return result;
        }

        private List<VirtualSettingAdvanceModel> GenModels()
        {
            var result = new List<VirtualSettingAdvanceModel>();
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var item = new VirtualSettingAdvanceModel
                {
                    BetCount = 1000 + random.Next(1, 1000),
                    CustId = 100,
                    ReportMonth = random.Next(1,12),
                    ReportYear = 2017,
                    Turnover = 10000 + random.Next(-1000, 1000),
                    Username = "Cuong" + random.Next(1, 30),
                    VarianceTurnover = 1000 + random.Next(1, 1000),
                    VarianceWinloss = 100 + random.Next(-1000, 1000),
                    Winloss = 1000 + random.Next(-1000, 1000),

                };

                result.Add(item);
            }

            return result;
        }
        #endregion
    }
}