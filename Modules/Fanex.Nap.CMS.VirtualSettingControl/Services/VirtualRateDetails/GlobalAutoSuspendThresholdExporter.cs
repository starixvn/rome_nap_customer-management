namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.CMS.Core.Resources;
    using Infrastructure.Models;
    using Infrastructure.Services.Implementations;
    using Infrastructure.Services.Interfaces;
    using Models.VirtualRateDetails;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using Resources;

    public class GlobalAutoSuspendThresholdExporter : BaseExcelExporter, IExcelExporter
    {
        private const int Width120 = 120;
        private const int Width200 = 200;
        private const int Width400 = 400;

        private readonly GlobalAutoSuspendThreshold globalAutoSuspendThreshold;
        private readonly string sportTypeName;

        private ICellStyle headerTextStyle;
        private ICellStyle sportTextStyle;
        private ICellStyle gameTextStyle;
        private ICellStyle betTypeGroupTextStyle;
        private ICellStyle betTypeTextStyle;
        private ICellStyle bodyTextStyle;

        public GlobalAutoSuspendThresholdExporter(GlobalAutoSuspendThreshold globalAutoSuspendThreshold, int sportType)
        {
            this.globalAutoSuspendThreshold = globalAutoSuspendThreshold;
            sportTypeName = CacheRef.GetSportName(sportType);
            InitStyles();
        }

        private void InitStyles()
        {
            SetFontColors();

            headerTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.Orange.Index, true, Alignment.Center);
            sportTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.Pink.Index, false, Alignment.Left);
            gameTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.Blue.Index, false, Alignment.Left);
            betTypeGroupTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.LightGreen.Index, false, Alignment.Left);
            betTypeTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);
            bodyTextStyle = NpoiExcelExtensions.TextStyleWithBlurBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);
        }

        private void SetFontColors()
        {
            var excelFile = ExportExcel.ExcelFile;
            var palette = excelFile.GetCustomPalette();

            palette.SetColorAtIndex(HSSFColor.Orange.Index, 254, 242, 203);
            palette.SetColorAtIndex(HSSFColor.Pink.Index, 247, 202, 172);
            palette.SetColorAtIndex(HSSFColor.Blue.Index, 221, 235, 246);
            palette.SetColorAtIndex(HSSFColor.LightGreen.Index, 226, 239, 216);
            palette.SetColorAtIndex(HSSFColor.White.Index, 255, 255, 255);
        }

        public override string FileNameWithoutExtension
            => $"{sportTypeName}_{ESportsResource.GlobalAutoSuspendThreshold}".Replace(" ", "_");

        public override ExcelColumn[] ExcelColumns
            => new[]
            {
                new ExcelColumn(string.Empty, Width120),
                new ExcelColumn(ViewLang.Level, Width400),
                new ExcelColumn(ViewLang.NonLive, Width200),
                new ExcelColumn(ViewLang.Live, Width200)
            };

        protected override void ExportHeader()
        {
            var excelColumns = ExcelColumns;

            ExportExcel.MaxColumnIndex = excelColumns.Length - 1;
            ExportExcel.SetColumnWidthByFontWidth(excelColumns.Select(c => c.Width).ToArray());

            ExportSearchFilter();
            ExportExcel.RenderEmptyRow(bodyTextStyle);

            CreateNPOIRowAndResetColumnIndex();
            ExportExcel.RenderCurrentCell(string.Empty, bodyTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Level, headerTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.NonLive, headerTextStyle);
            ExportExcel.RenderCurrentCell(ViewLang.Live, headerTextStyle);

            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();
        }

        private void ExportSearchFilter()
        {
            ExportSearchFilter(string.Empty, $"{sportTypeName} - {ESportsResource.GlobalAutoSuspendThreshold}", bodyTextStyle);
            ExportSearchFilter(ViewLang.Product, sportTypeName, bodyTextStyle);
            ExportSearchFilter(ViewLang.DateOfReport, DateTime.Today.ToString("MM/dd/yyyy"), bodyTextStyle);
        }

        private void ExportSearchFilter(string filterText, string filterValue, ICellStyle celltyle)
        {
            CreateNPOIRowAndResetColumnIndex();

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                ExportExcel.RenderCurrentCell(filterText, celltyle);
            }

            ExportExcel.RenderCellBySpanning(ExportExcel.CurrentRow, ExportExcel.MaxColumnIndex, filterValue, celltyle);
            ExportExcel.CurrentRow++;
        }

        private void CreateNPOIRowAndResetColumnIndex()
        {
            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();
        }

        protected override void ExportBody()
        {
            const int maxLevel = 4;
            const int paddingLength = 8;

            const int sportLevel = 0;
            const int gameLevel = 1;
            const int betTypeGroupLevel = 2;
            const int betTypeLevel = 3;

            IList<string> layerPadding = new List<string>();

            for (var i = 0; i < maxLevel; i++)
            {
                layerPadding.Add(string.Concat(Enumerable.Repeat(" ", i * paddingLength)));
            }

            foreach (var sport in globalAutoSuspendThreshold.Sports)
            {
                RenderRate(sport, sportTextStyle, layerPadding[sportLevel]);

                foreach (var game in sport.Childs)
                {
                    RenderRate(game, gameTextStyle, layerPadding[gameLevel]);

                    foreach (var betTypeGroup in game.Childs)
                    {
                        RenderRate(betTypeGroup, betTypeGroupTextStyle, layerPadding[betTypeGroupLevel]);

                        foreach (var betType in betTypeGroup.Childs)
                        {
                            RenderRate(betType, betTypeTextStyle, layerPadding[betTypeLevel]);
                        }
                    }
                }
            }
        }

        private void RenderRate(VirtualRateThresholdByGroup rate, ICellStyle cellStyle, string paddingText)
        {
            ExportExcel.CreateCurrentNPOIRow();

            ExportExcel.RenderCurrentCell(string.Empty, bodyTextStyle);
            ExportExcel.RenderCurrentCell(paddingText + rate.GroupName, cellStyle);
            ExportExcel.RenderCurrentCell(paddingText + rate.NonliveAutoSuspendThreshold.ToString("N0"), cellStyle);
            ExportExcel.RenderCurrentCell(paddingText + rate.LiveAutoSuspendThreshold.ToString("N0"), cellStyle);

            ExportExcel.CurrentRow++;
            ExportExcel.ResetCurrentColumn();
        }
    }
}
