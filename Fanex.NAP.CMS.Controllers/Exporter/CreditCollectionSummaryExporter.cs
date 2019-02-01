namespace Fanex.NAP.CMS.Controllers.Exporter
{
    using System.Linq;
    using Core.Resources;
    using Models.CreditCollectionSummary;
    using Nap.CMS.Infrastructure.Extensions;
    using Nap.CMS.Infrastructure.Helpers;
    using Nap.CMS.Infrastructure.Models;
    using Nap.CMS.Infrastructure.Web;
    using NPOI.XSSF.UserModel;

    public class CreditCollectionSummaryExporter
    {
        private const int TitleHeight = 400;
        private readonly CreditCollectionSummaryReport report;
        private readonly CreditCollectionSummaryFilter filter;

        private readonly XSSFFileModel _exportExcel;
        private readonly XSSFTemplate _excelStyleTemplate;

        public CreditCollectionSummaryExporter(
            CreditCollectionSummaryReport report,
            CreditCollectionSummaryFilter filter)
        {
            this.report = report;
            this.filter = filter;

            var workbook = new XSSFWorkbook();
            _excelStyleTemplate = new XSSFTemplate(workbook);
            _excelStyleTemplate.InitAllGeneralStyles();
            _exportExcel = new XSSFFileModel
            {
                ExcelFile = workbook
            };
        }

        public static string FileNameWithoutExtension => ViewLang.CreditCollectionSummary;

        public static string SheetName => ViewLang.CreditCollectionSummary;
        public XSSFFileModel ExportExcel => _exportExcel;
        public XSSFTemplate ExcelStyleTemplate => _excelStyleTemplate;

        public string Title
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

        public ExcelColumn[] ExcelColumns
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

        public void ResponseStream()
        {
            Export();

            ExcelHelper.ResponseStreamWithPassword(_exportExcel.ExcelFile, ExcelHelper.FormatFileName(FileNameWithoutExtension));
        }

        public void Export()
        {
            _exportExcel.Worksheet = _exportExcel.ExcelFile.CreateSheet(SheetName);

            ExportHeader();
            ExportBody();
        }

        public void ExportHeader()
        {
            var excelColumns = ExcelColumns;
            _exportExcel.MaxColumnIndex = excelColumns.Length - 1;
            _exportExcel.SetColumnWidthByFontWidth(excelColumns.Select(c => c.Width).ToArray());

            if (!string.IsNullOrEmpty(Title))
            {
                _exportExcel.CreateCurrentRow();
                _exportExcel.SetCurrentRowHeight(TitleHeight);

                _exportExcel.RenderRow(Title, _excelStyleTemplate.HeaderTextStyle);

                _exportExcel.RenderEmptyRow(_excelStyleTemplate.TextStyle);
            }

            _exportExcel.CreateCurrentRow();
            _exportExcel.ResetCurrentColumn();

            foreach (ExcelColumn column in excelColumns)
            {
                _exportExcel.RenderCurrentCell(column.Name, _excelStyleTemplate.HeaderTextStyle);
            }

            _exportExcel.CurrentRow++;
        }

        public void ExportBody()
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

                var headlineColor = new XSSFColor(new byte[] { 228, 241, 252 });
                var unusedColor = new XSSFColor(new byte[] { 237, 236, 234 });
                var notedColor = new XSSFColor(new byte[] { 255, 0, 0 });

                var headlineNoStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, headlineColor, true, Alignment.Center);
                var headlineCodeStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, headlineColor, false, Alignment.Left);
                var headlineNegativeNumberStyle = XSSFExtensions.StyleWithFormula(ExportExcel.ExcelFile, headlineColor, false, "[>=0]##,##0;[RED][<0](##,##0)");
                var headlineStatusStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, headlineColor, false, Alignment.Center);
                var headlineAlertStatusStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, headlineColor, false, Alignment.Center, notedColor);
                var unusedHeadlineStatusStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, unusedColor, false, Alignment.Center);

                var unusedHeadlineNoStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, unusedColor, true, Alignment.Center);
                var unusedHeadlineCodeStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, unusedColor, false, Alignment.Left);
                var unusedHeadlineNegativeNumberStyle = XSSFExtensions.StyleWithFormula(ExportExcel.ExcelFile, unusedColor, false, "[>=0]##,##0;[RED][<0](##,##0)");

                var noStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, true, Alignment.Center);
                var codeStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, false, Alignment.Left);
                var statusStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, false, Alignment.Center);
                var alertStatusStyle = XSSFExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, false, Alignment.Center, notedColor);
                var negativeNumberStyle = XSSFExtensions.StyleWithFormula(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, false, "[>=0]##,##0;[RED][<0](##,##0)");

                foreach (var group in report.Groups)
                {
                    ExportExcel.ResetCurrentColumn();

                    ExportExcel.RenderCurrentCell(++number, group.Status == CreditStatus.Unused ? unusedHeadlineNoStyle : headlineNoStyle);
                    ExportExcel.RenderCurrentCell(group.Code, group.Status == CreditStatus.Unused ? unusedHeadlineCodeStyle : headlineCodeStyle);
                    ExportExcel.RenderCurrentCell(GetDirectHeadline(group.AccountType), group.Status == CreditStatus.Unused ? unusedHeadlineStatusStyle : headlineStatusStyle);

                    var style = group.Status == CreditStatus.Unused
                        ? unusedHeadlineStatusStyle
                        : group.Status == CreditStatus.Alert ? headlineAlertStatusStyle : headlineStatusStyle;
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
                ExportExcel.RenderEmptyReport(XSSFExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColor, true, Alignment.Center));
            }
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
