namespace Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using BaseLabels = Fanex.Nap.CMS.Infrastructure.Resources.Labels;
    using Fanex.Nap.CMS.ManageProductSetting.Resources;
    using NPOI.SS.UserModel;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

    public class CompanyBetLimitSettingExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly IEnumerable<CompanyBetLimitSettingGroups> data;
        private readonly bool isCredit;
        private readonly ICellStyle decimalStyle;
        private readonly ICellStyle intNumberStyle;
        private readonly ICellStyle textStyle;

        private const int BetLimitCols = 3;

        public CompanyBetLimitSettingExporter(IEnumerable<CompanyBetLimitSettingGroups> data, bool isCredit)
        {
            this.data = data;
            this.isCredit = isCredit;
            decimalStyle = NpoiExcelExtensions.DecimalStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            decimalStyle.Alignment = HorizontalAlignment.Center;
            intNumberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            textStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                var today = DateTime.Today.ToShortFormatedDateWithSlash();

                return string.Format(Labels.NonSportCompanyBetLimitSetting, today);
            }
        }

        public override string SheetName => Labels.NonSportCompanyBetLimitSetting_sheetName;

        public override string Title => Labels.NonSportCompanyBetLimitSetting_title;

        private string CurrencyText => isCredit ? Labels.lblCreditCurrency : Labels.lblDepositCurrency;
        private string SiteText => isCredit ? Labels.lblCreditSite : Labels.lblDepositSite;

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                var result = new ExcelColumn[6 + BetLimitCols];
                result[0] = new ExcelColumn(BaseLabels.Symbol_No, 50);
                result[1] = new ExcelColumn(Labels.lblSportTypeID, 100);
                result[2] = new ExcelColumn(Labels.lblSportTypeName, 160);
                result[3] = new ExcelColumn(CurrencyText, 160);
                result[4] = new ExcelColumn(SiteText, 160);
                result[5] = new ExcelColumn(Labels.lblGameGroup, 120);

                for (var i = 0; i < BetLimitCols; i++)
                {
                    result[6 + i] = new ExcelColumn("", 120);
                }

                return result;
            }
        }

        private void RenderHeaders()
        {
            ExcelStyleTemplate.HeaderTextStyle.Alignment = HorizontalAlignment.Center;
            ExportExcel.CurrentColumn = 0;
            ExportExcel.CurrentRow = 2;
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, BaseLabels.Symbol_No, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, Labels.lblSportTypeID, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, Labels.lblSportTypeName, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, CurrencyText, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, SiteText, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(2, 3, ExportExcel.CurrentColumn, Labels.lblGameGroup, ExcelStyleTemplate.HeaderTextStyle);
         
            ExportExcel.RenderCell(2, 2, ExportExcel.CurrentColumn + BetLimitCols - 1, Labels.lblBetLimit, ExcelStyleTemplate.HeaderTextStyle);

            ExportExcel.CurrentRow = 3;
            ExportExcel.CurrentColumn = 6;
            ExportExcel.RenderCell(3, 3, ExportExcel.CurrentColumn, Labels.lblMinBet, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(3, 3, ExportExcel.CurrentColumn, Labels.lblMaxBet, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(3, 3, ExportExcel.CurrentColumn, Labels.lblRemark, ExcelStyleTemplate.HeaderTextStyle);
        }

        protected override void ExportBody()
        {
            RenderHeaders();
            if (data.Any())
            {
                RenderRows();
            }
            else
            {
                ExportExcel.RenderEmptyReport(textStyle);
            }
        }

        private void RenderRows()
        {
            ExportExcel.CurrentRow = 4;
            var sportTypeRow = ExportExcel.CurrentRow;
            var currencyRow = ExportExcel.CurrentRow;
            var siteRow = ExportExcel.CurrentRow;
            var gameGroupRow = ExportExcel.CurrentRow;
            // rowCount use for column "#"
            var rowCount = 1;

            RenderSportGroup(ref sportTypeRow, ref currencyRow, ref siteRow, ref gameGroupRow, ref rowCount, data);
        }

        private void RenderSportGroup(ref int sportTypeRow, ref int currencyRow, ref int siteRow, ref int gameGroupRow, ref int rowCount, IEnumerable<CompanyBetLimitSettingGroups> sportGroups)
        {
            foreach (var sportGroup in sportGroups)
            {
                ExportExcel.CurrentColumn = 1;
                var startRow = sportTypeRow;
                var endRow = sportTypeRow + sportGroup.NumberRow - 1;
                ExportExcel.RenderCell(startRow, endRow, ExportExcel.CurrentColumn, sportGroup.SportType, textStyle);
                ExportExcel.RenderCell(startRow, endRow, ExportExcel.CurrentColumn, sportGroup.SportName, textStyle);
                sportTypeRow += sportGroup.NumberRow;

                RenderCurrencyGroup(ref currencyRow, ref siteRow, ref gameGroupRow, ref rowCount, sportGroup);
            }
        }

        private void RenderCurrencyGroup(ref int currencyRow, ref int siteRow, ref int gameGroupRow, ref int rowCount, CompanyBetLimitSettingGroups currencyGroups)
        {
            foreach (var currencyGroup in currencyGroups.CurrencyGroups)
            {
                ExportExcel.CurrentColumn = 3;
                var currencyRowStart = currencyRow;
                var currencyRowEnd = currencyRow + currencyGroup.NumberRow - 1;
                ExportExcel.RenderCell(currencyRowStart, currencyRowEnd, ExportExcel.CurrentColumn, currencyGroup.Currency, textStyle);
                currencyRow += currencyGroup.NumberRow;

                RenderSiteGroup(ref siteRow, ref gameGroupRow, ref rowCount, currencyGroup);
            }
        }

        private void RenderSiteGroup(ref int siteRow, ref int gameGroupRow, ref int rowCount, CurrencyGroup siteGroups)
        {
            foreach (var siteGroup in siteGroups.SiteGroups)
            {
                ExportExcel.CurrentColumn = 4;
                ExportExcel.RenderCell(siteRow, siteRow + siteGroup.NumberRow - 1, ExportExcel.CurrentColumn, siteGroup.SiteGroupName, textStyle);
                siteRow += siteGroup.NumberRow;

                RenderGameGroup(ref gameGroupRow, ref rowCount, siteGroup);
            }
        }

        private void RenderGameGroup(ref int gameGroupRow, ref int rowCount, Models.CompanyBetLimitSetting.SiteGroup siteGroup)
        {
            foreach (var gameGroup in siteGroup.GameGroups)
            {
                ExportExcel.CurrentColumn = 0;
                var gameGroupRowStart = gameGroupRow;
                var gameGroupRowEnd = gameGroupRow + gameGroup.NumberRow - 1;
                ExportExcel.RenderCell(gameGroupRowStart, gameGroupRowEnd, 0, rowCount, intNumberStyle);
                rowCount++;

                ExportExcel.CurrentColumn = 5;
                var item = gameGroup.Entries.First(e => e.GameGroup == gameGroup.GameGroupName);
                ExportExcel.RenderCell(gameGroupRowStart, gameGroupRowEnd, ExportExcel.CurrentColumn, gameGroup.GameGroupName, textStyle);
                ExportExcel.RenderCell(gameGroupRowStart, gameGroupRowEnd, ExportExcel.CurrentColumn, item.MinBet, decimalStyle);
                ExportExcel.RenderCell(gameGroupRowStart, gameGroupRowEnd, ExportExcel.CurrentColumn, item.MaxBet, intNumberStyle);
                ExportExcel.RenderCell(gameGroupRowStart, gameGroupRowEnd, ExportExcel.CurrentColumn, item.Remark, textStyle);
                gameGroupRow += gameGroup.NumberRow;
            }
        }
    }
}