namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SpreadExporter : BaseExcelExporter, IExcelExporter
    {
        private const int FontHeight = 9;
        private const int Cell50Width = 50;
        private const int Cell150Width = 150;
        private const int Cell200Width = 200;
        private readonly IEnumerable<SpreadSettingExcelInvalidModel> _report;

        private IFont BodyFont { get; set; }

        private ICellStyle BodyTextStyle { get; set; }

        private ICellStyle BodyNumberStyle { get; set; }

        public SpreadExporter(IEnumerable<SpreadSettingExcelInvalidModel> report)
        {
            _report = report;
            InitStyles();
        }

        public override string FileNameWithoutExtension => Title;

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                return new[]
                {
                    new ExcelColumn("#", Cell50Width),
                    new ExcelColumn(Labels.Title, Cell200Width),
                    new ExcelColumn(Labels.CustomerClass, Cell150Width),
                    new ExcelColumn(Labels.SportType, Cell150Width),
                    new ExcelColumn(Labels.Site, Cell150Width),
                    new ExcelColumn(Labels.Currency, Cell150Width),
                    new ExcelColumn(Labels.Platform, Cell150Width),
                    new ExcelColumn(Labels.Spread, Cell150Width),
                    new ExcelColumn(Labels.Reason, Cell200Width),
                };
            }
        }

        public override string Title
        {
            get
            {
                return $"SpreadSettingInvalidData_{DateTime.Now.ToString()}";
            }
        }

        protected override void Export()
        {
            ExportExcel.Worksheet = ExportExcel.ExcelFile.CreateSheet(SheetName);

            ExportHeader();
            ExportBody();
            ExportFooter();
        }

        protected override void ExportHeader()
        {
            ExcelColumn[] excelColumns = ExcelColumns;

            ExportExcel.MaxColumnIndex = excelColumns.Length - 1;
            ExportExcel.SetColumnWidthByFontWidth(excelColumns.Select(c => c.Width).ToArray());

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();

            foreach (ExcelColumn column in excelColumns)
            {
                ExportExcel.RenderCurrentCell(column.Name, ExcelStyleTemplate.HeaderTextStyle);
            }

            ExportExcel.CurrentRow++;
        }

        protected override byte[] ExportCsv()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Join(",", new[]
            {
                BuildCellValue("#"),
                BuildCellValue(Labels.Title),
                BuildCellValue(Labels.CustomerClass),
                BuildCellValue(Labels.SportType),
                BuildCellValue(Labels.Site),
                BuildCellValue(Labels.Currency),
                BuildCellValue(Labels.Platform),
                BuildCellValue(Labels.Spread),
                BuildCellValue(Labels.Reason),
            }));

            foreach(var item in _report)
            {
                stringBuilder.AppendLine(string.Join(",", new[]
                {
                    BuildCellValue(item.ID),
                    BuildCellValue(item.PolicyTitle),
                    BuildCellValue(item.CustomerClass),
                    BuildCellValue(item.SportType),
                    BuildCellValue(item.Site),
                    BuildCellValue(item.Currency),
                    BuildCellValue(item.Platform),
                    BuildCellValue(item.Spread),
                    BuildCellValue(item.Reason)
                }));
            }

            return Encoding.UTF8.GetBytes(stringBuilder.ToString());
        }

        private static string BuildCellValue(object value) => string.Format("\"{0}\"", value);

        protected override void ExportBody()
        {
            ExportExcel.ResetCurrentColumn();

            foreach (var item in _report)
            {
                ExportExcel.CreateCurrentNPOIRow();
                ExportExcel.RenderCurrentCell(item.ID, BodyNumberStyle);
                ExportExcel.RenderCurrentCell(item.PolicyTitle, BodyTextStyle);
                ExportExcel.RenderCurrentCell(item.CustomerClass, BodyNumberStyle);
                ExportExcel.RenderCurrentCell(item.SportType, BodyTextStyle);
                ExportExcel.RenderCurrentCell(item.Site, BodyTextStyle);
                ExportExcel.RenderCurrentCell(item.Currency, BodyTextStyle);
                ExportExcel.RenderCurrentCell(item.Platform, BodyTextStyle);
                ExportExcel.RenderCurrentCell(item.Spread, BodyNumberStyle);
                ExportExcel.RenderCurrentCell(item.Reason, BodyTextStyle);
                ExportExcel.CurrentRow++;
                ExportExcel.ResetCurrentColumn();
            }
        }

        private void InitStyles()
        {
            SetFontColors();
            BodyFont = CreateBodyFont();

            BodyTextStyle = CreateStyle(HSSFColor.White.Index, BodyFont, HorizontalAlignment.Left);
            BodyNumberStyle = CreateStyle(HSSFColor.White.Index, BodyFont, HorizontalAlignment.Right);
        }

        private void SetFontColors()
        {
            var excelFile = ExportExcel.ExcelFile;
            var palette = excelFile.GetCustomPalette();
#pragma warning disable S109 // Magic numbers should not be used
            palette.SetColorAtIndex(HSSFColor.Grey50Percent.Index, 207, 206, 206);
            palette.SetColorAtIndex(HSSFColor.LightBlue.Index, 54, 102, 214);
#pragma warning restore S109 // Magic numbers should not be used
        }

        private IFont CreateBodyFont()
        {
            var excelFile = ExportExcel.ExcelFile;
            var font = excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.None;
            font.Color = HSSFColor.Black.Index;
            font.FontHeightInPoints = FontHeight;
            font.FontName = "Tahoma";

            return font;
        }

        private ICellStyle CreateStyle(short colorIndex, IFont font, HorizontalAlignment alignment)
        {
            var excelFile = ExportExcel.ExcelFile;
            var cellStyle = excelFile.CreateCellStyle();

            cellStyle.FillForegroundColor = colorIndex;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.Top;
            cellStyle.SetFont(font);

            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = HSSFColor.Grey50Percent.Index;
            cellStyle.LeftBorderColor = HSSFColor.Grey50Percent.Index;
            cellStyle.RightBorderColor = HSSFColor.Grey50Percent.Index;
            cellStyle.TopBorderColor = HSSFColor.Grey50Percent.Index;

            return cellStyle;
        }
    }
}
