using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using NPOI.SS.UserModel;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public sealed class GlobalSettingExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly SportType _sportType;
        private readonly IEnumerable<GlobalSettingItem> _globalSettingItems;

        public GlobalSettingExporter(SportType sportType, IEnumerable<GlobalSettingItem> globalSettingItems)
        {
            _sportType = sportType;
            _globalSettingItems = globalSettingItems;
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                string today = DateTime.Today.ToShortFormatedDateWithSlash();

                return _sportType == SportType.Others ?
                    string.Format(Labels.GlobalSetting_ExcelFileName_OtherSports, today) :
                    string.Format(Labels.GlobalSetting_ExcelFileName, today);
            }
        }

        public override string SheetName
        {
            get
            {
                return _sportType == SportType.Others ?
                    Labels.GlobalSetting_SheetName_OtherSports :
                    Labels.GlobalSetting_SheetName;
            }
        }

        public override string Title
        {
            get
            {
                return _sportType == SportType.Others ?
                    Labels.GlobalSetting_ExcelTitle_OtherSports :
                    Labels.GlobalSetting_ExcelTitle;
            }
        }

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                return new[]
                {
                    new ExcelColumn(Labels.Symbol_No, 60),
                    new ExcelColumn(Labels.UserName, 160),
                    new ExcelColumn(Labels.Level, 120),
                    new ExcelColumn(Labels.Currency, 160),
                    new ExcelColumn(Labels.Max_Turnover, 160)
                };
            }
        }

        protected override void ExportBody()
        {
            var centerIntNumberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            centerIntNumberStyle.Alignment = HorizontalAlignment.Center;

            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            if (_globalSettingItems.Any())
            {
                int number = 0;

                foreach (var item in _globalSettingItems)
                {
                    ExportExcel.ResetCurrentColumn();

                    ExportExcel.RenderCurrentCell(++number, centerIntNumberStyle);
                    ExportExcel.RenderCurrentCell(item.UserName, ExcelStyleTemplate.TextStyle);
                    ExportExcel.RenderCurrentCell(item.Level, centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.MaxTurnover, ExcelStyleTemplate.DecimalStyle);
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
