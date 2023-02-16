namespace Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public class ManageFollowBetAccountExporter : BaseExcelExporter, IExcelExporter
    {
        protected const int Width40 = 40;
        protected const int Width140 = 140;
        protected const int Width200 = 200;
        protected const string tick = "√";

        private readonly SearchFollowBetAccountViewModel searchFilter;
        private readonly IEnumerable<FollowBetAccountItem> data;

        public ManageFollowBetAccountExporter(
            SearchFollowBetAccountViewModel searchFilter,
            IEnumerable<FollowBetAccountItem> data)
        {
            this.searchFilter = searchFilter;
            this.data = data;
        }

        public override string FileNameWithoutExtension
            => string.Format(Labels.ManageFollowBetAccountExcel, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns => new[]
        {
            new ExcelColumn("#", Width40),
            new ExcelColumn(Labels.Username, Width200),
            new ExcelColumn(Labels.DummyUsername, Width200),
            new ExcelColumn(Labels.DummyCustId, Width140),
            new ExcelColumn(Labels.Agent, Width200),
            new ExcelColumn(Labels.Currency, Width140),
            new ExcelColumn(Labels.Site, Width140),
            new ExcelColumn(Labels.AddedBy, Width200),
            new ExcelColumn(Labels.AddedDate, Width140),
            new ExcelColumn(Labels.Active, Width140),
            new ExcelColumn(Labels.Follow, Width140),
        };

        protected override void ExportBody()
        {
            if (!data.Any())
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
                return;
            }

            var textStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Left);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            const string yes = "Yes";
            var index = 1;

            foreach (var item in data)
            {
                ExportExcel.ResetCurrentColumn();
                ExportExcel.RenderCurrentCell(index, textStyle);
                ExportExcel.RenderCurrentCell(item.Username, textStyle);
                ExportExcel.RenderCurrentCell(item.DummyUsername, textStyle);
                ExportExcel.RenderCurrentCell(item.DummyCustId, textStyle);
                ExportExcel.RenderCurrentCell(item.Agent, textStyle);
                ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.Site, textStyle);
                ExportExcel.RenderCurrentCell(item.AddedBy, textStyle);
                ExportExcel.RenderCurrentCell(item.AddedDate, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.IsActive ? yes : string.Empty, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.IsFollow ? yes : string.Empty, centerTextStyle);
                ExportExcel.CurrentRow++;
                index++;
            }
        }

        protected void CreateNPOIRowAndResetColumnIndex()
        {
            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();
        }

        protected void ExportSearchFilter()
        {
            var textStyle = NpoiExcelExtensions.TextStyleWithoutBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);

            ExportSearchFilter(Labels.Username, searchFilter.Username, textStyle);
            ExportSearchFilter(Labels.Site, searchFilter.SiteName, textStyle);
            ExportSearchFilter(Labels.Currency, searchFilter.CurrencyName, textStyle);
            ExportSearchFilter(Labels.DummyAgent, searchFilter.DummyAgentName, textStyle);
            ExportSearchFilter(Labels.Active, searchFilter.IsActive ? tick : String.Empty, textStyle);
            ExportSearchFilter(Labels.Follow, searchFilter.IsFollow ? tick : String.Empty, textStyle);
        }

        protected void ExportSearchFilter(string filterText, string filterValue, ICellStyle celltyle)
        {
            CreateNPOIRowAndResetColumnIndex();
            ExportExcel.RenderCurrentCell(filterText, celltyle);
            ExportExcel.RenderCellBySpanning(ExportExcel.CurrentRow, ExportExcel.MaxColumnIndex, filterValue, celltyle);
            ExportExcel.CurrentRow++;
        }
    }
}
