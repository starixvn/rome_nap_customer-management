namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public sealed class VirtualRateDetailsExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly VirtualRateDetailsReport _report;
        private readonly bool _isGlobalSetting;

        private IFont _font { get; set; }
        private ICellStyle HeaderTextStyle { get; set; }
        private ICellStyle HeaderDisabledTextStyle { get; set; }
        private ICellStyle SportTextStyle { get; set; }
        private ICellStyle SportDisabledTextStyle { get; set; }
        private ICellStyle GameTextStyle { get; set; }
        private ICellStyle GameDisabledTextStyle { get; set; }
        private ICellStyle BetTypeGroupTextStyle { get; set; }
        private ICellStyle BetTypeGroupDisabledTextStyle { get; set; }
        private ICellStyle BetTypeTextStyle { get; set; }
        private ICellStyle BetTypeDisabledTextStyle { get; set; }

        private ICellStyle BodyTextStyle { get; set; }

        public VirtualRateDetailsExporter(VirtualRateDetailsReport report)
        {
            _report = report;
            _isGlobalSetting = report.ReportLevel == 0;

            InitStyles();
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                return ViewLang.VirtualRateDetails;
            }
        }

        public override string SheetName
        {
            get
            {
                return ViewLang.VRSetting;
            }
        }

        public override string Title
        {
            get
            {
                return _isGlobalSetting ? ViewLang.VirtualRateSettingDetails + " - " + ViewLang.GlobalSetting : ViewLang.VirtualRateSettingDetails;
            }
        }

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                return new[]
                {
                    new ExcelColumn(ViewLang.Level, 400),
                    new ExcelColumn(ViewLang.NonLive, 150),
                    new ExcelColumn(ViewLang.Live, 150)
                };
            }
        }

        protected override void ExportHeader()
        {
            ExportExcel.SetColumnWidthByFontWidth(new[] { 100, 400, 200, 200 });
            ExportExcel.MaxColumnIndex = 3;

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderRow(Title, BodyTextStyle);
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.Product + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.ESports, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            if (!_isGlobalSetting)
            {
                ExportExcel.CreateCurrentNPOIRow();
                ExportExcel.RenderCurrentCell(ViewLang.Account + ":", BodyTextStyle);
                ExportExcel.RenderCurrentCell(_report.Username, BodyTextStyle);
                ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
                ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
                ExportExcel.CurrentRow++;
                ExportExcel.ResetCurrentColumn();
            }

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(ViewLang.DateOfReport + ":", BodyTextStyle);
            ExportExcel.RenderCurrentCell(DateTime.Today.ToString("MM/dd/yyyy"), BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();

            ExportExcel.RenderEmptyRow(BodyTextStyle);
            ExportExcel.ResetCurrentColumn();

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Level, HeaderTextStyle);

            ExportExcel.RenderCurrentCell(ViewLang.NonLive, _report.EnableNonLive ? HeaderTextStyle : HeaderDisabledTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Live, _report.EnableLive ? HeaderTextStyle : HeaderDisabledTextStyle);
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

            foreach (var sport in _report.Sports)
            {
                RenderRate(sport, SportTextStyle, SportDisabledTextStyle, ViewLang.FollowUpline, layerPadding[0]);

                foreach (var game in sport.Childs)
                {
                    RenderRate(game, GameTextStyle, GameDisabledTextStyle, ViewLang.FollowParent, layerPadding[1]);

                    foreach (var betTypeGroup in game.Childs)
                    {
                        RenderRate(betTypeGroup, BetTypeGroupTextStyle, BetTypeGroupDisabledTextStyle, ViewLang.FollowParent, layerPadding[2]);

                        foreach (var betType in betTypeGroup.Childs)
                        {
                            RenderRate(betType, BetTypeTextStyle, BetTypeDisabledTextStyle, ViewLang.FollowParent, layerPadding[3]);
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
            HeaderDisabledTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HorizontalAlignment.Center);
            SportTextStyle = CreateStyle(HSSFColor.Pink.Index, HorizontalAlignment.Left);
            SportDisabledTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HorizontalAlignment.Left);
            GameTextStyle = CreateStyle(HSSFColor.Blue.Index, HorizontalAlignment.Left);
            GameDisabledTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HorizontalAlignment.Left);
            BetTypeGroupTextStyle = CreateStyle(HSSFColor.LightGreen.Index, HorizontalAlignment.Left);
            BetTypeGroupDisabledTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HorizontalAlignment.Left);
            BetTypeTextStyle = CreateStyle(HSSFColor.White.Index, HorizontalAlignment.Left);
            BetTypeDisabledTextStyle = CreateStyle(HSSFColor.Grey50Percent.Index, HorizontalAlignment.Left);
            BodyTextStyle = CreateStyle(HSSFColor.White.Index, HorizontalAlignment.Left);
        }

        private void SetFontColors()
        {
            var excelFile = ExportExcel.ExcelFile;
            var palette = excelFile.GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.Grey25Percent.Index, 212, 212, 212);
            palette.SetColorAtIndex(HSSFColor.Grey50Percent.Index, 207, 206, 206);

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

        private string GetRateText(decimal rate)
        {
            return ((rate + 100) * _report.ActualRate / 100).ToString("#,##0.0000");
        }

        private void RenderRate(
            VirtualRateDetailsByGroup rate,
            ICellStyle enabledStyle,
            ICellStyle disabledStyle,
            string followParentText,
            string paddingText)
        {
            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.RenderCurrentCell(string.Empty, BodyTextStyle);
            ExportExcel.RenderCurrentCell(paddingText + rate.GroupName, enabledStyle);
            var nonliveText = rate.NonliveId == 0 ?
                followParentText :
                _isGlobalSetting ? $"{rate.NonLiveValue.ToString("#,##0")}" : $"{rate.NonLiveValue.ToString("#,##0")} ({GetRateText(rate.NonLiveValue)})";
            var liveText = rate.LiveId == 0 ?
                followParentText :
                _isGlobalSetting ? $"{rate.LiveValue.ToString("#,##0")}" : $"{rate.LiveValue.ToString("#,##0")} ({GetRateText(rate.LiveValue)})";
            ExportExcel.RenderCurrentCell(paddingText + nonliveText, _report.EnableNonLive ? enabledStyle : disabledStyle);
            ExportExcel.RenderCurrentCell(paddingText + liveText, _report.EnableLive ? enabledStyle : disabledStyle);
            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();
        }
    }
}
