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

        public CompanyBetLimitSettingExporter(IEnumerable<CompanyBetLimitSettingGroups> data, bool isCredit)
        {
            this.data = data;
            this.isCredit = isCredit;
            decimalStyle = NpoiExcelExtensions.DecimalStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            decimalStyle.Alignment = HorizontalAlignment.Center;
            intNumberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile,
                ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            textStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex,
                false, Alignment.Center);
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
        
        private string CurrencyText => isCredit ? Labels.lblCreditCurrency : Labels.lblDepositCurrency;
        private string SiteText => isCredit ? Labels.lblCreditSite : Labels.lblDepositSite;

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                var result = new ExcelColumn[6 + 3];
                result[0] = new ExcelColumn("#", 50);
                result[1] = new ExcelColumn(Labels.lblSportTypeID, 100);
                result[2] = new ExcelColumn(Labels.lblSportTypeName, 160);
                result[3] = new ExcelColumn(CurrencyText, 160);
                result[4] = new ExcelColumn(SiteText, 160);
                result[5] = new ExcelColumn("Game Group", 120);

                for (var i = 0; i < 3; i++)
                {
                    result[6 + i] = new ExcelColumn("Bet Limit", 120);
                }

                return result;
            }
        }

        public void RenderHeader()
        {
            ExcelStyleTemplate.HeaderTextStyle.Alignment = HorizontalAlignment.Center;
            ExportExcel.CurrentColumn = 0;
            ExportExcel.CurrentRow = 2;
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, BaseLabels.Symbol_No, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblSportTypeID, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblSportTypeName, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, CurrencyText, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, SiteText, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblGameGroup, ExcelStyleTemplate.HeaderTextStyle);
            // + 3 - 1 => calculate index of "remark colum" , +3: group 3 col MinBet,MaxBet, Remark
            ExportExcel.RenderCellBySpanning(2, ExportExcel.CurrentColumn + 3 - 1, Labels.lblBetLimit, ExcelStyleTemplate.HeaderTextStyle);

            ExportExcel.CurrentRow = 3;
            ExportExcel.CurrentColumn = 6;
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblMinBet, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblMaxBet, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCellBySpanning(3, ExportExcel.CurrentColumn, Labels.lblRemark, ExcelStyleTemplate.HeaderTextStyle);
        }

        protected override void ExportBody()
        {
        RenderHeader();
            if (data.Any())
            {
                RenderRow();
            }
            else
            {
                ExportExcel.RenderEmptyReport(textStyle);
            }
        }

        private void RenderRow()
        {
            ExportExcel.CurrentRow = 4;
            var sportTypeRow = ExportExcel.CurrentRow;
            var currencyRow = ExportExcel.CurrentRow;
            var siteRow = ExportExcel.CurrentRow;
            var gameGroupRow = ExportExcel.CurrentRow;
            var rowCount = 1;
            foreach (var sportGroup in data)
            {
                var sportGroupChildren = 0;
                foreach (var currencyGroup in sportGroup.CurrencyGroups)
                {
                    var currencyGroupChildren = 0;
                    foreach (var siteGroup in currencyGroup.SiteGroups)
                    {
                        sportGroupChildren += siteGroup.GameGroups.Count();
                        currencyGroupChildren += siteGroup.GameGroups.Count();
                        var siteGroupChildren = 0;
                        foreach (var gameGroup in siteGroup.GameGroups)
                        {
                            ExportExcel.CurrentColumn = 0;
                            ExportExcel.RenderCell(gameGroupRow, gameGroupRow, 0, rowCount, intNumberStyle);
                            ExportExcel.CurrentColumn = 5;
                            ExportExcel.RenderCell(gameGroupRow, gameGroupRow, ExportExcel.CurrentColumn, gameGroup.GameGroupName, textStyle);
                            ExportExcel.RenderCell(gameGroupRow, gameGroupRow, ExportExcel.CurrentColumn, gameGroup.Entries.First().MinBet, decimalStyle);
                            ExportExcel.RenderCell(gameGroupRow, gameGroupRow, ExportExcel.CurrentColumn, gameGroup.Entries.First().MaxBet, intNumberStyle);
                            ExportExcel.RenderCell(gameGroupRow, gameGroupRow, ExportExcel.CurrentColumn, gameGroup.Entries.First().Remark, textStyle);
                            gameGroupRow++;
                            siteGroupChildren++;
                            rowCount++;
                        }

                        ExportExcel.CurrentColumn = 4;
                        ExportExcel.RenderCell(siteRow, siteRow + siteGroupChildren - 1, ExportExcel.CurrentColumn,
                            siteGroup.SiteGroupName, textStyle);
                        siteRow += siteGroupChildren;
                    }

                    ExportExcel.CurrentColumn = 3;
                    ExportExcel.RenderCell(currencyRow, currencyRow + currencyGroupChildren - 1, ExportExcel.CurrentColumn, currencyGroup.Currency, textStyle);
                    currencyRow += currencyGroupChildren;
                }

                ExportExcel.CurrentColumn = 1;
                ExportExcel.RenderCell(sportTypeRow, sportTypeRow + sportGroupChildren - 1, ExportExcel.CurrentColumn, sportGroup.SportType, textStyle);
                ExportExcel.RenderCell(sportTypeRow, sportTypeRow + sportGroupChildren - 1, ExportExcel.CurrentColumn, sportGroup.SportName, textStyle);
                sportTypeRow += sportGroupChildren;
            }
        }
    }
}