namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public sealed class VirtualRateRealValuesExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly VirtualRateRealValues _report;
        private readonly int _sportType;
        private IFont _font { get; set; }
        private ICellStyle HeaderTextStyle { get; set; }
        private ICellStyle SportTextStyle { get; set; }
        private ICellStyle GameTextStyle { get; set; }
        private ICellStyle BetTypeGroupTextStyle { get; set; }
        private ICellStyle BetTypeTextStyle { get; set; }
        private ICellStyle BodyTextStyle { get; set; }

        public VirtualRateRealValuesExporter(VirtualRateRealValues report, int sportType)
        {
            _report = report;
            _sportType = sportType;
            InitStyles();
        }

        public override string FileNameWithoutExtension
            => ViewLang.VirtualRateDetails;

        public override string SheetName
            => ViewLang.VRSetting;

        public override string Title
            => ViewLang.VirtualRateSettingDetails;

        public override ExcelColumn[] ExcelColumns
            => new[]
                {
                    new ExcelColumn(ViewLang.Level, 400),
                    new ExcelColumn(ViewLang.NonLive, 250),
                    new ExcelColumn(ViewLang.Live, 250)
                };

        protected override void ExportHeader()
        {
            ExportExcel.SetColumnWidthByFontWidth(new[] { 100, 400, 250, 250 });
            ExportExcel.MaxColumnIndex = 3;

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderRow(Title, BodyTextStyle);
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.Product + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(CacheRef.GetSportName(_sportType), BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.Account + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(_report.Username, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.DateOfReport + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(DateTime.Today.ToString("MM/dd/yyyy"), BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.Currency + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(_report.Currency, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.ExchangeRate + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(_report.ExchangeRateText, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.RenderEmptyRow(BodyTextStyle);
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Level, HeaderTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.NonLive, HeaderTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Live, HeaderTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();
        }

        protected override void ExportBody()
        {
            var layerPadding = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                layerPadding.Add(string.Concat(Enumerable.Repeat(" ", i * 8)));
            }

            foreach (var sport in _report.Childs)
            {
                RenderRate(sport, SportTextStyle, ViewLang.FollowUpline, layerPadding[0]);

                foreach (var league in sport.Childs)
                {
                    RenderRate(league, GameTextStyle, ViewLang.FollowParent, layerPadding[1]);

                    foreach (var betTypeGroup in league.Childs)
                    {
                        RenderRate(betTypeGroup, BetTypeGroupTextStyle, ViewLang.FollowParent, layerPadding[2]);

                        foreach (var betType in betTypeGroup.Childs)
                        {
                            RenderRate(betType, BetTypeTextStyle, ViewLang.FollowParent, layerPadding[3]);
                        }
                    }
                }
            }
        }

        private void InitStyles()
        {
            SetFontColors();
            _font = CreateFont();

            HeaderTextStyle = CreateStyle(HSSFColor.Orange.Index, HorizontalAlignment.Center);
            SportTextStyle = CreateStyle(HSSFColor.Pink.Index, HorizontalAlignment.Left);
            GameTextStyle = CreateStyle(HSSFColor.Blue.Index, HorizontalAlignment.Left);
            BetTypeGroupTextStyle = CreateStyle(HSSFColor.LightGreen.Index, HorizontalAlignment.Left);
            BetTypeTextStyle = CreateStyle(HSSFColor.White.Index, HorizontalAlignment.Left);
            BodyTextStyle = CreateStyle(HSSFColor.White.Index, HorizontalAlignment.Left);
        }

        private void SetFontColors()
        {
            var excelFile = ExportExcel.ExcelFile;
            var palette = excelFile.GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.Grey25Percent.Index, 212, 212, 212);

            palette.SetColorAtIndex(HSSFColor.Orange.Index, 254, 242, 203);
            palette.SetColorAtIndex(HSSFColor.Pink.Index, 247, 202, 172);
            palette.SetColorAtIndex(HSSFColor.Blue.Index, 221, 235, 246);
            palette.SetColorAtIndex(HSSFColor.LightGreen.Index, 226, 239, 216);
            palette.SetColorAtIndex(HSSFColor.White.Index, 255, 255, 255);
        }

        private IFont CreateFont()
        {
            var excelFile = ExportExcel.ExcelFile;
            var font = excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.None;
            font.Color = HSSFColor.Black.Index;
            font.FontHeightInPoints = 8;
            font.FontName = "Tahoma";

            return font;
        }

        private ICellStyle CreateStyle(short colorIndex, HorizontalAlignment alignment)
        {
            var excelFile = ExportExcel.ExcelFile;
            var cellStyle = excelFile.CreateCellStyle();

            cellStyle.FillForegroundColor = colorIndex;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.Top;
            cellStyle.SetFont(_font);

            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = HSSFColor.Grey25Percent.Index;
            cellStyle.LeftBorderColor = HSSFColor.Grey25Percent.Index;
            cellStyle.RightBorderColor = HSSFColor.Grey25Percent.Index;
            cellStyle.TopBorderColor = HSSFColor.Grey25Percent.Index;

            return cellStyle;
        }

        private void RenderRate(
           VirtualRateDetailsByGroup rate,
           ICellStyle style,
           string followParentText,
           string paddingText)
        {
            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(paddingText + rate.GroupName, style);
            ExportExcel.RenderCurrentCell(paddingText + rate.NonliveLog, style);
            ExportExcel.RenderCurrentCell(paddingText + rate.LiveLog, style);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();
        }
    }
}
