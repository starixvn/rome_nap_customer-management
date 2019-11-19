namespace Fanex.NAP.CMS.Controllers.Exporter
{
    using System.Linq;
    using Core.Resources;
    using Models.CreditCollectionSummary;
    using Nap.CMS.Infrastructure.Extensions;
    using Nap.CMS.Infrastructure.Models;
    using Nap.CMS.Infrastructure.Services.Implementations;
    using Nap.CMS.Infrastructure.Services.Interfaces;
    using NPOI.HSSF.Util;

    public class CreditCollectionSummaryExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly CreditCollectionSummaryReport report;
        private readonly CreditCollectionSummaryFilter filter;

        public CreditCollectionSummaryExporter(CreditCollectionSummaryReport report, CreditCollectionSummaryFilter filter)
        {
            this.report = report;
            this.filter = filter;
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                return Core.Resources.ViewLang.CreditCollectionSummary;
            }
        }

        public override string SheetName
        {
            get
            {
                return Core.Resources.ViewLang.CreditCollectionSummary;
            }
        }

        public override string Title
        {
            get
            {
                var result = ViewLang.CreditCollectionSummary;
                if (!string.IsNullOrWhiteSpace(filter.Code))
                {
                    result += $"_{filter.Code}";
                }

                if (filter.AccountType != CodeType.All)
                {
                    result += $"_{GetAccountTypeName(filter.AccountType)}";
                }

                result += $"_{report.LastBalanceDateStr}_{report.CurrentBalanceDateStr}";
                return result;
            }
        }

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                var result = new ExcelColumn[9 + report.BreakDowns.Count()];
                result[0] = new ExcelColumn(ViewLang.No, 80);
                result[1] = new ExcelColumn(ViewLang.Code, 200);
                result[2] = new ExcelColumn($"({ViewLang.Headline})/{ViewLang.Direct}", 120);
                result[3] = new ExcelColumn(ViewLang.Status, 80);

                result[4] = new ExcelColumn(ViewLang.LastBalance, 120);
                result[5] = new ExcelColumn(ViewLang.CurrentBalance, 120);
                result[6] = new ExcelColumn(ViewLang.DebtMovement, 120);
                result[7] = new ExcelColumn($"({ViewLang.Collection})/{ViewLang.Payment}", 160);
                result[8] = new ExcelColumn(ViewLang.NetMovement, 120);

                for (var i = 0; i < report.BreakDowns.Count(); i++)
                {
                    result[9 + i] = new ExcelColumn(ViewLang.CollectionBreakdown, 200);
                }

                return result;
            }
        }

        protected override void ExportBody()
        {
            ExportExcel.CurrentColumn = 0;
            ExportExcel.CurrentRow = 2;
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.No, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.Code, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.Headline + "/" + ViewLang.Direct, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.Status, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.CurrentColumn += 2;
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.DebtMovement, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, $"({ViewLang.Collection})/{ViewLang.Payment}", ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, ViewLang.NetMovement, ExcelStyleTemplate.HeaderTextStyle);

            if (report.BreakDowns.Any())
            {
                ExportExcel.RenderCellBySpanning(2, ExportExcel.CurrentColumn + report.BreakDowns.Count() - 1, ViewLang.CollectionBreakdown, ExcelStyleTemplate.HeaderTextStyle);
            }

            ExportExcel.CurrentRow++;

            ExportExcel.CurrentColumn = 4;
            ExportExcel.RenderCell(3, 3, 4, report.LastBalanceDateStr, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(3, 3, 5, report.CurrentBalanceDateStr, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.CurrentColumn += 3;
            foreach (var breakdownRange in report.BreakDowns)
            {
                var str = $"{breakdownRange.FromDateStr} {ViewLang.To} {breakdownRange.ToDateStr}";
                ExportExcel.RenderCell(3, 3, ExportExcel.CurrentColumn, str, ExcelStyleTemplate.HeaderTextStyle);
            }

            ExportExcel.CurrentRow++;

            if (report.Groups.Any())
            {
                int number = 0;

                NpoiExcelExtensions.SetColorAtIndex(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, 228, 241, 252);
                var headlineNoStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, true, Alignment.Center);
                var headlineCodeStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, false, Alignment.Left);
                var headlineNegativeNumberStyle = NpoiExcelExtensions.StyleWithFormula(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, false, "[>=0]##,##0;[RED][<0](##,##0)");
                var headlineStatusStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, false, Alignment.Center);
                var headlineAlertStatusStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.CornflowerBlue.Index, false, Alignment.Center, HSSFColor.Red.Index);
                var unusedHeadlineStatusStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.DarkTeal.Index, false, Alignment.Center);

                NpoiExcelExtensions.SetColorAtIndex(ExportExcel.ExcelFile, HSSFColor.DarkTeal.Index, 237, 236, 234);
                var unusedHeadlineNoStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.DarkTeal.Index, true, Alignment.Center);
                var unusedHeadlineCodeStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, HSSFColor.DarkTeal.Index, false, Alignment.Left);
                var unusedHeadlineNegativeNumberStyle = NpoiExcelExtensions.StyleWithFormula(ExportExcel.ExcelFile, HSSFColor.DarkTeal.Index, false, "[>=0]##,##0;[RED][<0](##,##0)");

                var noStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center);
                var codeStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Left);
                var statusStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
                var alertStatusStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center, HSSFColor.Red.Index);
                var negativeNumberStyle = NpoiExcelExtensions.StyleWithFormula(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, "[>=0]##,##0;[RED][<0](##,##0)");

                foreach (var group in report.Groups)
                {
                    ExportExcel.ResetCurrentColumn();

                    ExportExcel.RenderCurrentCell(++number, group.Status == CreditStatus.Unused ? unusedHeadlineNoStyle : headlineNoStyle);
                    ExportExcel.RenderCurrentCell(group.Code, group.Status == CreditStatus.Unused ? unusedHeadlineCodeStyle : headlineCodeStyle);
                    ExportExcel.RenderCurrentCell(GetDirectHeadline(group.AccountType), group.Status == CreditStatus.Unused ? unusedHeadlineStatusStyle : headlineStatusStyle);

                    var style = group.Status == CreditStatus.Unused
                        ? unusedHeadlineStatusStyle
                        : GetStatus(headlineStatusStyle, headlineAlertStatusStyle, group);
                    ExportExcel.RenderCurrentCell(GetStatusText(group.Status), style);

                    ExportExcel.RenderCurrentCell(group.LastBalance, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);
                    ExportExcel.RenderCurrentCell(group.CurrentBalance, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);
                    ExportExcel.RenderCurrentCell(group.DebtMovement, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);
                    ExportExcel.RenderCurrentCell(group.Payment, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);
                    ExportExcel.RenderCurrentCell(group.NetMovement, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);

                    foreach (var breakdown in group.BreakDowns)
                    {
                        ExportExcel.RenderCurrentCell(breakdown, group.Status == CreditStatus.Unused ? unusedHeadlineNegativeNumberStyle : headlineNegativeNumberStyle);
                    }

                    ExportExcel.CurrentRow++;

                    foreach (var downline in group.Downlines)
                    {
                        ExportExcel.ResetCurrentColumn();

                        ExportExcel.RenderCurrentCell(string.Empty, noStyle);
                        ExportExcel.RenderCurrentCell("  " + downline.Code, codeStyle);
                        ExportExcel.RenderCurrentCell(GetDirectHeadline(downline.AccountType), statusStyle);
                        ExportExcel.RenderCurrentCell(string.Empty, downline.Status == CreditStatus.Alert ? alertStatusStyle : statusStyle);

                        ExportExcel.RenderCurrentCell(downline.LastBalance, negativeNumberStyle);
                        ExportExcel.RenderCurrentCell(downline.CurrentBalance, negativeNumberStyle);
                        ExportExcel.RenderCurrentCell(downline.DebtMovement, negativeNumberStyle);
                        ExportExcel.RenderCurrentCell(downline.Payment, negativeNumberStyle);
                        ExportExcel.RenderCurrentCell(downline.NetMovement, negativeNumberStyle);

                        foreach (var breakdown in downline.BreakDowns)
                        {
                            ExportExcel.RenderCurrentCell(breakdown, negativeNumberStyle);
                        }

                        ExportExcel.CurrentRow++;
                    }
                }
            }
            else
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            }
        }

        private static NPOI.SS.UserModel.ICellStyle GetStatus(NPOI.SS.UserModel.ICellStyle headlineStatusStyle, NPOI.SS.UserModel.ICellStyle headlineAlertStatusStyle, CreditCollectionSummaryItem group)
        {
            return group.Status == CreditStatus.Alert ? headlineAlertStatusStyle : headlineStatusStyle;
        }

        private static string GetStatusText(int status)
        {
            switch (status)
            {
                case CreditStatus.Alert:
                    return ViewLang.Alert;
                case CreditStatus.Unused:
                    return ViewLang.Unused;
                default:
                    return ViewLang.Normal;
            }
        }

        private static string GetDirectHeadline(int accountType)
        {
            if (accountType == CodeType.Direct)
            {
                return ViewLang.Direct;
            }

            if (accountType == CodeType.Headline)
            {
                return ViewLang.Headline;
            }

            return string.Empty;
        }

        private static string GetAccountTypeName(int accountType)
        {
            switch (accountType)
            {
                case CodeType.OldCodes:
                    return ViewLang.OLDcodes;
                case CodeType.OtherCodes:
                    return ViewLang.Othercodes;
                default:
                    return string.Empty;
            }
        }
    }
}
