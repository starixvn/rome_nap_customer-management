namespace Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.ManageProductSetting.Resources;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;
    
    public class CompanyBetLimitSettingExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly IEnumerable<CompanyBetLimitSettingGroups> data;

        public CompanyBetLimitSettingExporter(IEnumerable<CompanyBetLimitSettingGroups> data)
        {
            this.data = data;
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                string today = DateTime.Today.ToShortFormatedDateWithSlash();

                return string.Format(Labels.NonSportCompanyBetLimitSetting, today);
            }
        }

        public override string SheetName => Labels.NonSportCompanyBetLimitSetting_sheetName;

        public override string Title => Labels.NonSportCompanyBetLimitSetting_title;

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                var result = new ExcelColumn[5 + 3];
                result[0] = new ExcelColumn("#", 80);
                result[1] = new ExcelColumn("Sport Type ID", 100);
                result[2] = new ExcelColumn("Sport Type Name", 160);
                result[3] = new ExcelColumn("Currency", 160);
                result[4] = new ExcelColumn("Game Group", 120);

                for (var i = 0; i < 3; i++)
                {
                    result[5 + i] = new ExcelColumn("Bet Limit", 340);
                }

                return result;
            }
        }

        protected override void ExportBody()
        {
            // var centerIntNumberStyle =
            //     NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            // centerIntNumberStyle.Alignment = HorizontalAlignment.Center;
            // var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile,
            //     ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            // ExcelStyleTemplate.HeaderTextStyle.Alignment = HorizontalAlignment.Center;
            //
            // #region HeaderRow
            //
            // ExportExcel.CurrentColumn = 0;
            // ExportExcel.CurrentRow = 2;
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "#",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Sport Type ID",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Sport Type Name",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Currency",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Game Group",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // // + 3 - 1 => calculate index of "remark colum" , +3: group 3 col MinBet,MaxBet, Remark
            // ExportExcel.RenderCellBySpanning(2, ExportExcel.CurrentColumn + 3 - 1, "Bet Limit",
            //     ExcelStyleTemplate.HeaderTextStyle);
            //
            // ExportExcel.CurrentRow = 3;
            // ExportExcel.CurrentColumn = 5;
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Min Bet",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Max Bet",
            //     ExcelStyleTemplate.HeaderTextStyle);
            // ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, "Remark",
            //     ExcelStyleTemplate.HeaderTextStyle);
            //
            // #endregion HeaderRow
            //
            // ExportExcel.CurrentRow = 4;
            //
            // if (data.Any())
            // {
            //     var currentRow = ExportExcel.CurrentRow;
            //     var currentSportGroupRow = currentRow;
            //     var row = 1;
            //     foreach (var companyBetLimitSettingGroups in data)
            //     {
            //         var sportGroupChildren = 0;
            //
            //         foreach (var currencyGroup in companyBetLimitSettingGroups.CurrencyGroups)
            //         {
            //             sportGroupChildren += currencyGroup.GameGroups.Count();
            //             ExportExcel.CurrentColumn = 3;
            //             ExportExcel.RenderCell(currentSportGroupRow,
            //                 currentSportGroupRow + currencyGroup.GameGroups.Count() - 1, ExportExcel.CurrentColumn,
            //                 currencyGroup.Currency, ExcelStyleTemplate.HeaderTextStyle);
            //
            //             var currencyGroupChildren = currentSportGroupRow;
            //             foreach (var gameGroup in currencyGroup.GameGroups)
            //             {
            //                 ExportExcel.CurrentColumn = 0;
            //                 ExportExcel.RenderCell(currencyGroupChildren, currencyGroupChildren, 0, row,
            //                     ExcelStyleTemplate.HeaderTextStyle);
            //                 ExportExcel.CurrentColumn = 4;
            //                 ExportExcel.RenderCell(currencyGroupChildren, currencyGroupChildren,
            //                     ExportExcel.CurrentColumn, gameGroup.GameGroupName, ExcelStyleTemplate.HeaderTextStyle);
            //                 ExportExcel.RenderCell(currencyGroupChildren, currencyGroupChildren,
            //                     ExportExcel.CurrentColumn, gameGroup.Entries.First().MinBet,
            //                     ExcelStyleTemplate.HeaderTextStyle);
            //                 ExportExcel.RenderCell(currencyGroupChildren, currencyGroupChildren,
            //                     ExportExcel.CurrentColumn, gameGroup.Entries.First().MaxBet,
            //                     ExcelStyleTemplate.HeaderTextStyle);
            //                 ExportExcel.RenderCell(currencyGroupChildren, currencyGroupChildren,
            //                     ExportExcel.CurrentColumn, gameGroup.Entries.First().Remark,
            //                     ExcelStyleTemplate.HeaderTextStyle);
            //                 currencyGroupChildren++;
            //                 row++;
            //             }
            //
            //             currentSportGroupRow += currencyGroup.GameGroups.Count();
            //         }
            //
            //         ExportExcel.CurrentColumn = 1;
            //         ExportExcel.RenderCell(currentRow, currentRow + sportGroupChildren - 1, ExportExcel.CurrentColumn,
            //             companyBetLimitSettingGroups.SportType, ExcelStyleTemplate.HeaderTextStyle);
            //         ExportExcel.RenderCell(currentRow, currentRow + sportGroupChildren - 1, ExportExcel.CurrentColumn,
            //             companyBetLimitSettingGroups.SportName, ExcelStyleTemplate.HeaderTextStyle);
            //         currentRow += sportGroupChildren;
            //     }
            // }
            // else
            // {
            //     ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile,
            //         ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            // }
        }
    }
}