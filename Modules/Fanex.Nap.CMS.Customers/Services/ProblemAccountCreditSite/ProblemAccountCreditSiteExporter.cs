namespace Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Constants.ProblemAccount;
    using Constants.ProblemAccountCreditSite;
    using Infrastructure.Constants;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Services.Interfaces;
    using Models.ProblemAccount;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using ProblemAccount;
    using Resources;
    using ViewModels.ProblemAccountCreditSite;

    public sealed class ProblemAccountCreditSiteExporter : ProblemAccountBaseExporter, IExcelExporter
    {
        private readonly SearchCreditSiteViewModel searchFilter;
        private readonly ProblemAccountModel data;
        private readonly IEnumerable<IdNameModel> currencies;
        private readonly IEnumerable<IdNameModel> preventiveActions;

        public const int VoidWinningActionId = 2;

        public ProblemAccountCreditSiteExporter(
            SearchCreditSiteViewModel searchFilter,
            ProblemAccountModel data,
            IEnumerable<IdNameModel> reasons,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> preventiveActions) : base(reasons)
        {
            this.searchFilter = searchFilter;
            this.data = data;
            this.currencies = currencies;
            this.preventiveActions = preventiveActions;
        }

        public override string FileNameWithoutExtension
            => string.Format(ProblemAccountCreditDepositResource.ExcelFileNameCreditSite, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns => GetExcelColumns();

        private ExcelColumn[] GetExcelColumns()
        {
            var columns = new[]
            {
                new ExcelColumn(ProblemAccountResource.Account, Width300),
                new ExcelColumn(ProblemAccountResource.AccountStatus, Width140),
                new ExcelColumn(ProblemAccountResource.Currency, Width100),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxPerMatch, Width140),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxBet, Width140),
                new ExcelColumn(ProblemAccountCreditDepositResource.PreventiveAction, Width300),
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

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.Level, RoleIdConstants.GetRoleName(searchFilter.Level), textStyle);
            ExportCurrencySearchFilter(currencies, searchFilter.Currency, textStyle);
            ExportSearchFilter(ProblemAccountResource.AccountStatus, AccountStatusCreditSiteConst.AccountStatusListForFilter[searchFilter.Status], textStyle);
            ExportIsReducedSearchFilter(searchFilter.IsReduced, textStyle);
            ExportReasonsSearchFilter(searchFilter.Reason, textStyle);
            ExportDirectlyMarkedSearchFilter(searchFilter.IsDirectlyMarked, textStyle);
            ExportPreventiveActionSearchFilter(preventiveActions, searchFilter.PreventiveActionId, textStyle);
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

            IList<decimal?> maxPerMatchs;
            IList<decimal?> maxBets;
            string preventiveAction;

            foreach (var item in data.ProblemAccounts)
            {
                maxPerMatchs = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMPM,
                    item.MasterMPM,
                    item.AgentMPM,
                    item.MPM,
                    searchFilter.Level).ToList();
                maxBets = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMaxBet,
                    item.MasterMaxBet,
                    item.AgentMaxBet,
                    item.MaxBet,
                    searchFilter.Level).ToList();

                preventiveAction = BuildPreventiveAction(item);

                ExportExcel.ResetCurrentColumn();
                ExportAccountCell(item, searchFilter.Level, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.StatusName, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerMatchs, blueFont, wrapTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxBets, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(preventiveAction, centerTextStyle);
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

        protected override void ExportAccountCell(
            ProblemAccountItem problemAccount,
            byte currentLevel,
            IFont blueFont,
            ICellStyle wrapTextStyle)
        {
            int startIndexSuper = -1;
            int endIndexSuper = -1;
            int startIndexMaster = -1;
            int endIndexMaster = -1;
            int startIndexAgent = -1;
            int endIndexAgent = -1;
            int startIndex = -1;
            int endIndex = -1;

            var cellBuilder = new StringBuilder();

            ExportAccountCell(
                cellBuilder,
                problemAccount.IsProblemAccSuper,
                $"{problemAccount.Super}",
                ref startIndexSuper,
                ref endIndexSuper);

            if (searchFilter.Level <= RoleIdConstants.Master)
            {
                cellBuilder.Append(NewLine);
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccMaster,
                    $"    {problemAccount.Master}",
                    ref startIndexMaster,
                    ref endIndexMaster);
            }

            if (searchFilter.Level <= RoleIdConstants.Agent)
            {
                cellBuilder.Append(NewLine);
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccAgent,
                    $"        {problemAccount.Agent}",
                    ref startIndexAgent,
                    ref endIndexAgent);
            }

            if (searchFilter.Level <= RoleIdConstants.Member)
            {
                cellBuilder.Append(NewLine);
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAcc,
                    $"            {problemAccount.Username}",
                    ref startIndex,
                    ref endIndex);
            }

            IRichTextString richText = new HSSFRichTextString(cellBuilder.ToString());
            richText.ApplyFont(wrapTextStyle.FontIndex);

            ApplyBlueFont(richText, startIndexSuper, endIndexSuper, blueFont);
            ApplyBlueFont(richText, startIndexMaster, endIndexMaster, blueFont);
            ApplyBlueFont(richText, startIndexAgent, endIndexAgent, blueFont);
            ApplyBlueFont(richText, startIndex, endIndex, blueFont);

            ExportExcel.RenderCurrentCellText(wrapTextStyle, richText);
        }

        protected override void ExportAccountCell(
            StringBuilder cellBuilder,
            bool isProblemAcc,
            string username,
            ref int startIndex,
            ref int endIndex)
        {
            if (isProblemAcc)
            {
                startIndex = cellBuilder.Length == 0 ? 0 : cellBuilder.Length - 1;
            }

            cellBuilder.Append(username);

            if (isProblemAcc)
            {
                endIndex = cellBuilder.Length;
            }
        }

        protected override void ExportMaxPerEventOrMaxPerBet(IList<decimal?> values, int lastIndex, IFont blueFont, ICellStyle wrapTextStyle)
        {
            string value;
            int startIndex = -1;
            int endIndex = -1;
            var cellBuilder = new StringBuilder(string.Empty);

            for (int i = 0, max = values.Count; i < max; i++)
            {
                if (values[i] == null)
                {
                    value = "N/A";
                }
                else if (values[i] == 0)
                {
                    value = "0";
                }
                else
                {
                    value = values[i].Value.ToString("N0");
                }

                if (i > 0)
                {
                    cellBuilder.Append(NewLine);
                }

                if (i == lastIndex)
                {
                    startIndex = cellBuilder.Length == 0 ? 0 : cellBuilder.Length - 1;
                }

                cellBuilder.Append($"{RoleShortNameMapper[i]}{value}");

                if (i == lastIndex)
                {
                    endIndex = cellBuilder.Length;
                }
            }

            IRichTextString richText = new HSSFRichTextString(cellBuilder.ToString());
            richText.ApplyFont(wrapTextStyle.FontIndex);
            ApplyBlueFont(richText, startIndex, endIndex, blueFont);

            ExportExcel.RenderCurrentCellText(wrapTextStyle, richText);
        }

        private bool IsMemberLevel => searchFilter.Level == RoleIdConstants.Member;

        private static string BuildPreventiveAction(ProblemAccountItem problemAccount)
        {
            if(problemAccount.PreventiveActionId == VoidWinningActionId)
            {
                return $"{problemAccount.PreventiveAction} - {problemAccount.VoidTime} - {problemAccount.LimitAmount}";
            }

            return problemAccount.PreventiveAction;
        }
    }
}
