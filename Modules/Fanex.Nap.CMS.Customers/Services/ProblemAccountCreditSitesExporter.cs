namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Services.Interfaces;
    using Models.ProblemAccount;
    using NPOI.HSSF.Util;
    using Resources;
    using ViewModels.ProblemAccount;

    public sealed class ProblemAccountCreditSitesExporter : ProblemAccountExporter, IExcelExporter
    {
        private readonly IEnumerable<IdNameModel> currencies;

        public ProblemAccountCreditSitesExporter(
            SearchViewModel searchFilter,
            ProblemAccountModel data,
            IEnumerable<IdNameModel> reasons,
            IEnumerable<IdNameModel> currencies) : base(searchFilter, data, reasons)
        {
            this.currencies = currencies;
        }

        public override string FileNameWithoutExtension
            => string.Format(ProblemAccountCreditDepositResource.ExcelFileNameCreditSite, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns => GetExcelColumns();

        private ExcelColumn[] GetExcelColumns()
        {
            const int Width100 = 100;
            const int Width140 = 140;
            const int Width300 = 300;
            const int Width400 = 400;

            var columns = new[]
            {
                new ExcelColumn(ProblemAccountResource.Account, Width300),
                new ExcelColumn(ProblemAccountResource.AccountStatus, Width140),
                new ExcelColumn(ProblemAccountResource.Currency, Width100),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxPerMatch, Width140),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxBet, Width140),
                new ExcelColumn(ProblemAccountResource.MarkDate, Width140),
                new ExcelColumn(ProblemAccountResource.Reason, Width400),
                new ExcelColumn(ProblemAccountResource.Comment, Width400)
            };

            return IsMemberLevel
                ? columns.Concat(new[] { new ExcelColumn(ProblemAccountResource.AbnormalBet, Width140) }).ToArray()
                : columns;
        }

        protected override void ExportSearchFilter()
        {
            var textStyle = NpoiExcelExtensions.TextStyleWithoutBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);

            var reasonDict = reasons.ToDictionary(r => r.Id.ToString(), r => r.Name);
            var reasonNames = Join(searchFilter.Reason.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(r => reasonDict[r]), ", ");

            var currency = currencies.FirstOrDefault(c => c.Id == searchFilter.Currency);

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.Level, RoleNameMapper[searchFilter.Level], textStyle);
            ExportSearchFilter(ProblemAccountResource.Currency, currency == null ? string.Empty : currency.Name, textStyle);
            ExportSearchFilter(ProblemAccountResource.AccountStatus, AccountStatusCreditSitesConst.AccountStatusListForFilter[searchFilter.Status], textStyle);
            ExportSearchFilter(ProblemAccountResource.IsReduced, IsReducedConst.IsReducedList[searchFilter.IsReduced], textStyle);
            ExportSearchFilter(ProblemAccountResource.Reason, reasonNames, textStyle);
            ExportSearchFilter(ProblemAccountResource.DirectlyMarked, DirectlyMarkedMapper[searchFilter.IsDirectlyMarked], textStyle);
            ExportSearchFilter(ProblemAccountResource.Username, searchFilter.Username, textStyle);
            ExportSearchFilter(ProblemAccountResource.SearchDownlineExcel, searchFilter.IsSearchDownlines ? nameof(IsReducedConst.Yes) : nameof(IsReducedConst.No), textStyle);
            ExportSearchFilter(ProblemAccountResource.OrderBy, OrderByConst.OrderByList[searchFilter.OrderBy], textStyle);
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

            IList<decimal?> maxPerMatchs;
            IList<decimal?> maxBets;

            foreach (var item in data.ProblemAccounts)
            {
                maxPerMatchs = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMPM,
                    item.MasterMPM,
                    item.AgentMPM,
                    item.MPM).ToList();
                maxBets = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMaxBet,
                    item.MasterMaxBet,
                    item.AgentMaxBet,
                    item.MaxBet).ToList();

                ExportExcel.ResetCurrentColumn();
                ExportAccountCell(item, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.StatusName, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerMatchs, blueFont, wrapTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxBets, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.MarkDateString, centerTextStyle);
                ExportExcel.RenderCurrentCell(Join(item.Reasons, NewLine), wrapTextStyle);
                ExportExcel.RenderCurrentCell(HtmlDecodeText(item.Comment), wrapTextStyle);

                if (IsMemberLevel)
                {
                    ExportExcel.RenderCurrentCell(item.AbnormalBetNumber.Value, numberStyle);
                }

                ExportExcel.CurrentRow++;
            }
        }
    }
}
