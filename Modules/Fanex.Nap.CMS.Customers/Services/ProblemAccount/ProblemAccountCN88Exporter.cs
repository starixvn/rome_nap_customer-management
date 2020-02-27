using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Models.ProblemAccount;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using NPOI.HSSF.Util;

namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    public sealed class ProblemAccountCN88Exporter : ProblemAccountBaseExporter, IExcelExporter
    {
        private readonly SearchViewModel searchFilter;
        private readonly ProblemAccountModel data;

        public ProblemAccountCN88Exporter(
            SearchViewModel searchFilter,
            ProblemAccountModel data,
            IEnumerable<IdNameModel> reasons) : base(reasons)
        {
            this.searchFilter = searchFilter;
            this.data = data;
        }

        public override string FileNameWithoutExtension
            => string.Format(ProblemAccountResource.ExcelFileName, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns => GetExcelColumns();

        private ExcelColumn[] GetExcelColumns()
        {
            var columns = new[]
            {
                new ExcelColumn(ProblemAccountResource.Account, Width300),
                new ExcelColumn(ProblemAccountResource.AccountStatus, Width140),
                new ExcelColumn(ProblemAccountResource.MaxPerEvent, Width140),
                new ExcelColumn(ProblemAccountResource.MaxPerBet, Width140),
                new ExcelColumn(ProblemAccountResource.MarkDate, Width140),
                new ExcelColumn(ProblemAccountResource.Reason, Width400),
                new ExcelColumn(ProblemAccountResource.Comment, Width400),
                new ExcelColumn(ProblemAccountResource._1stWeekInformed, Width140),
                new ExcelColumn(ProblemAccountResource._2ndWeekInformed, Width140)
            };

            return IsMemberLevel
                ? columns.Concat(new[] { new ExcelColumn(ProblemAccountResource.AbnormalBet, Width140) }).ToArray()
                : columns;
        }

        protected override void ExportSearchFilter()
        {
            var textStyle = NpoiExcelExtensions.TextStyleWithoutBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.Level, RoleIdConstants.GetRoleName(searchFilter.Level), textStyle);
            ExportCurrencySearchFilter(CurrencyConst.CurrencyList(), searchFilter.Currency, textStyle);
            ExportSearchFilter(ProblemAccountResource.AccountStatus, AccountStatusConst.AccountStatusList[searchFilter.Status], textStyle);
            ExportIsReducedSearchFilter(searchFilter.IsReduced, textStyle);
            ExportReasonsSearchFilter(searchFilter.Reason, textStyle);
            ExportDirectlyMarkedSearchFilter(searchFilter.IsDirectlyMarked, textStyle);
            ExportSearchFilter(ProblemAccountResource.Username, searchFilter.Username, textStyle);
            ExportSearchFilter(ProblemAccountResource.SearchDownlineExcel, searchFilter.IsSearchDownlines ? nameof(IsReducedConst.Yes) : nameof(IsReducedConst.No), textStyle);
            ExportOrderBySearchFilter(searchFilter.OrderBy, textStyle);
        }

        protected override void ExportBody()
        {
            if (data.TotalItems == 0)
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
                return;
            }

            var wrapTextStyle = ExcelStyleTemplate.TextStyle;
            wrapTextStyle.WrapText = true;

            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            var numberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);
            var blueFont = CreateBlueFont(wrapTextStyle);

            IList<decimal?> maxPerEvents;
            IList<decimal?> maxPerBets;

            foreach (var item in data.ProblemAccounts)
            {
                maxPerEvents = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMPM,
                    item.MasterMPM,
                    item.AgentMPM,
                    item.MPM,
                    searchFilter.Level).ToList();
                maxPerBets = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMaxBet,
                    item.MasterMaxBet,
                    item.AgentMaxBet,
                    item.MaxBet,
                    searchFilter.Level).ToList();

                ExportExcel.ResetCurrentColumn();
                ExportAccountCell(item, searchFilter.Level, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.StatusName, centerTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerEvents, blueFont, wrapTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerBets, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.MarkDateString, centerTextStyle);
                ExportExcel.RenderCurrentCell(Join(item.Reasons, NewLine), wrapTextStyle);
                ExportExcel.RenderCurrentCell(HtmlDecodeText(item.Comment), wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.FirstWeekInformed, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.SecondWeekInformed, centerTextStyle);

                if (IsMemberLevel)
                {
                    ExportExcel.RenderCurrentCell(item.AbnormalBetNumber.Value, numberStyle);
                }

                ExportExcel.CurrentRow++;
            }
        }

        private bool IsMemberLevel => searchFilter.Level == RoleIdConstants.Member;
    }
}
