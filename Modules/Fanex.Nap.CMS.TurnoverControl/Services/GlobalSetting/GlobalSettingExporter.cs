using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using NPOI.SS.UserModel;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public sealed class GlobalSettingExporter : BaseExcelExporter, IExcelExporter
    {
        private const int Width60 = 60;
        private const int Width80 = 80;
        private const int Width120 = 120;
        private const int Width160 = 160;

        private readonly short sportType;
        private readonly IEnumerable<GlobalSettingCustomer> globalSettings;
        private readonly IEnumerable<IdNameModel> profiles;

        public GlobalSettingExporter(
            short sportType,
            IEnumerable<GlobalSettingCustomer> globalSettings,
            IEnumerable<IdNameModel> profiles)
        {
            this.sportType = sportType;
            this.globalSettings = globalSettings;
            this.profiles = profiles;
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                string today = DateTime.Today.ToShortFormatedDateWithSlash();

                return sportType == SportTypeConst.Others
                    ? string.Format(Labels.GlobalSetting_ExcelFileName_OtherSports, today)
                    : string.Format(Labels.GlobalSetting_ExcelFileName, today);
            }
        }

        public override string SheetName
            => sportType == SportTypeConst.Others
                ? Labels.GlobalSetting_SheetName_OtherSports
                : Labels.GlobalSetting_SheetName;

        public override string Title
            => sportType == SportTypeConst.Others
                ? Labels.GlobalSetting_ExcelTitle_OtherSports
                : Labels.GlobalSetting_ExcelTitle;

        public override ExcelColumn[] ExcelColumns
            => new[]
            {
                new ExcelColumn(Labels.Symbol_No, Width60),
                new ExcelColumn(Labels.UserName, Width160),
                new ExcelColumn(Labels.Level, Width80),
                new ExcelColumn(Labels.Currency, Width80)
            }.Concat(profiles.Select(p => new ExcelColumn(p.Name, Width120))).ToArray();

        protected override void ExportBody()
        {
            var centerIntNumberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            centerIntNumberStyle.Alignment = HorizontalAlignment.Center;

            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (globalSettings.Any())
            {
                int number = 0;

                foreach (var item in globalSettings)
                {
                    ExportExcel.ResetCurrentColumn();

                    ExportExcel.RenderCurrentCell(++number, centerIntNumberStyle);
                    ExportExcel.RenderCurrentCell(item.Username, ExcelStyleTemplate.TextStyle);
                    ExportExcel.RenderCurrentCell(item.Level, centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);

                    foreach (var mto in item.MaxTurnovers)
                    {
                        ExportExcel.RenderCurrentCell(mto.MaxTurnover, ExcelStyleTemplate.DecimalStyle);
                    }

                    ExportExcel.CurrentRow++;
                }
            }
            else
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            }
        }
    }
}
