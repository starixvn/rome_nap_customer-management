namespace Fanex.Nap.CMS.Customers.Services
{
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using System;

    public sealed class RobotsExporter : BaseExcelExporter, IExcelExporter
    {
        private const int ColumnCount = 15;
        private const int FontHeight = 9;
        private const int Cell50Width = 50;
        private const int Cell100Width = 100;
        private const int Cell200Width = 200;
        private const int Cell400Width = 400;

        private readonly int[] ColumnWidth
            = new[] { Cell50Width, Cell100Width, Cell200Width, Cell100Width,
                Cell100Width, Cell200Width, Cell100Width, Cell100Width,
                Cell100Width, Cell200Width, Cell100Width, Cell100Width,
                Cell100Width, Cell100Width, Cell100Width };

        private readonly RobotReport report;

        private IFont HeaderFont { get; set; }

        private IFont BodyFont { get; set; }

        private ICellStyle TitleTextStyle { get; set; }

        private ICellStyle HeaderTextStyle { get; set; }

        private ICellStyle BodyTextStyle { get; set; }

        public RobotsExporter(RobotReport report)
        {
            this.report = report;
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
                    new ExcelColumn(Labels.CustId, Cell100Width),
                    new ExcelColumn(Labels.Username, Cell200Width),
                    new ExcelColumn(Labels.Level, Cell100Width),
                    new ExcelColumn(Labels.Currency, Cell100Width),
                    new ExcelColumn(Labels.Site, Cell200Width),
                    new ExcelColumn(Labels.IP, Cell100Width),
                    new ExcelColumn(Labels.Country, Cell200Width),
                    new ExcelColumn(Labels.Region, Cell100Width),
                    new ExcelColumn(Labels.LastChangeTime, Cell200Width),
                    new ExcelColumn(Labels.Counter, Cell100Width),
                    new ExcelColumn(Labels.ServerIP, Cell100Width),
                    new ExcelColumn(Labels.AddedBy, Cell100Width),
                    new ExcelColumn(Labels.AddedDate, Cell100Width),
                    new ExcelColumn(Labels.Remark, Cell400Width)
                };
            }
        }

        public override string Title
        {
            get
            {
                if (report.FromDate == null || report.ToDate == null)
                {
                    return Labels.ManageRobots;
                }

                return $"{Labels.ManageRobots} - {report.FromDate?.ToShortDateString()}-{report.ToDate?.ToShortDateString()}";
            }
        }

        //protected override void ExportHeader()
        //{
        //    ExportExcel.SetColumnWidthByFontWidth(ColumnWidth);

        //    ExportExcel.MaxColumnIndex = ColumnCount - 1;

        //    ExportExcel.CreateCurrentNPOIRow();
        //    ExportExcel.RenderRow(Title, TitleTextStyle);
        //    ExportExcel.ResetCurrentColumn();

        //    ExportExcel.CreateCurrentNPOIRow();
        //    ExportExcel.RenderCurrentCell("#", HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.CustId, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Username, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Level, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Currency, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Site, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.IP, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Country, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Region, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.LastChangeTime, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Counter, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.ServerIP, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.AddedBy, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.AddedDate, HeaderTextStyle);
        //    ExportExcel.RenderCurrentCell(Labels.Remark, HeaderTextStyle);
        //    ExportExcel.CurrentRow++;
        //    ExportExcel.ResetCurrentColumn();
        //}

        protected override void ExportBody()
        {
            ExportExcel.ResetCurrentColumn();
            for (int i = 0; i < report.Robots.Count; i++)
            {
                var robot = report.Robots[i];
                ExportExcel.CreateCurrentNPOIRow();
                ExportExcel.RenderCurrentCell(i + 1, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.CustId, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Username, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Level, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Currency, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Site, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.IP, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Country, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Region, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.LastChangeTimeStr, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.CounterStr, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.ServerIP, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.AddedBy, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.AddedDateStr, BodyTextStyle);
                ExportExcel.RenderCurrentCell(robot.Remark, BodyTextStyle);
                ExportExcel.CurrentRow++;
                ExportExcel.ResetCurrentColumn();
            }
        }

        private void InitStyles()
        {
            SetFontColors();
            HeaderFont = CreateHeaderFont();
            BodyFont = CreateBodyFont();

            TitleTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HeaderFont, HorizontalAlignment.Left);
            HeaderTextStyle = CreateStyle(HSSFColor.LightBlue.Index, HeaderFont, HorizontalAlignment.Center);
            BodyTextStyle = CreateStyle(HSSFColor.White.Index, BodyFont, HorizontalAlignment.Left);
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

        private IFont CreateHeaderFont()
        {
            var font = CreateBodyFont();
            font.Boldweight = (short)FontBoldWeight.Bold;

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