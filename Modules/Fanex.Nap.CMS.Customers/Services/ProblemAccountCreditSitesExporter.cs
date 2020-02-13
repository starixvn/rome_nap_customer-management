namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Constants;
    using Infrastructure.Constants;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Services.Implementations;
    using Infrastructure.Services.Interfaces;
    using Models.ProblemAccount;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using Resources;
    using ViewModels.ProblemAccountCreditSite;

    public sealed class ProblemAccountCreditSitesExporter : BaseExcelExporter, IExcelExporter
    {
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string[] RoleShortNameMapper = new[] { "S:", "M:", "A:", "mb:" };
        
        private readonly SearchCreditSiteViewModel searchFilter;
        private readonly ProblemAccountModel data;
        private readonly IEnumerable<IdNameModel> reasons;
        private readonly IEnumerable<IdNameModel> currencies;
        private readonly IEnumerable<IdNameModel> preventiveActions;

        public ProblemAccountCreditSitesExporter(
            SearchCreditSiteViewModel searchFilter,
            ProblemAccountModel data,
            IEnumerable<IdNameModel> reasons,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> preventiveActions)
        {
            this.searchFilter = searchFilter;
            this.data = data;
            this.reasons = reasons;
            this.currencies = currencies;
            this.preventiveActions = preventiveActions;
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
                new ExcelColumn(ProblemAccountCreditDepositResource.PreventiveAction, Width140),
                new ExcelColumn(ProblemAccountResource.MarkDate, Width140),
                new ExcelColumn(ProblemAccountResource.Reason, Width400),
                new ExcelColumn(ProblemAccountResource.Comment, Width400)
            };

            return IsMemberLevel
                ? columns.Concat(new[] { new ExcelColumn(ProblemAccountResource.AbnormalBet, Width140) }).ToArray()
                : columns;
        }

        protected override void ExportHeader()
        {
            var excelColumns = ExcelColumns;

            ExportExcel.MaxColumnIndex = excelColumns.Length - 1;
            ExportExcel.SetColumnWidthByFontWidth(excelColumns.Select(c => c.Width).ToArray());

            ExportSearchFilter();
            ExportExcel.RenderEmptyRow(ExcelStyleTemplate.TextStyle);

            CreateNPOIRowAndResetColumnIndex();

            foreach (ExcelColumn column in excelColumns)
            {
                ExportExcel.RenderCurrentCell(column.Name, ExcelStyleTemplate.HeaderTextStyle);
            }

            ExportExcel.CurrentRow++;
        }

        private void ExportSearchFilter()
        {
            var textStyle = NpoiExcelExtensions.TextStyleWithoutBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);

            var reasonDict = reasons.ToDictionary(r => r.Id.ToString(), r => r.Name);
            var reasonNames = Join(searchFilter.Reason.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(r => reasonDict[r]), ", ");

            var currency = currencies.FirstOrDefault(c => c.Id == searchFilter.Currency);
            var isReduced = IsReducedConst.IsReducedList().FirstOrDefault(r => r.Id == searchFilter.IsReduced);
            var directlyMarked = DirectlyMarkedConst.DirectlyMarkedList().FirstOrDefault(m => m.Id == searchFilter.IsDirectlyMarked);
            var orderBy = OrderByConst.OrderByList().FirstOrDefault(o => o.Id == searchFilter.OrderBy);
            var preventiveAction = preventiveActions.FirstOrDefault(p => p.Id == searchFilter.PreventiveActionId);

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.Level, RoleIdConstants.GetRoleName(searchFilter.Level), textStyle);
            ExportSearchFilter(ProblemAccountResource.Currency, currency == null ? string.Empty : currency.Name, textStyle);
            ExportSearchFilter(ProblemAccountResource.AccountStatus, AccountStatusCreditSiteConst.AccountStatusListForFilter[searchFilter.Status], textStyle);
            ExportSearchFilter(ProblemAccountResource.IsReduced, isReduced?.Name ?? string.Empty, textStyle);
            ExportSearchFilter(ProblemAccountResource.Reason, reasonNames, textStyle);
            ExportSearchFilter(ProblemAccountResource.DirectlyMarked, directlyMarked?.Name ?? string.Empty, textStyle);
            ExportSearchFilter(ProblemAccountCreditDepositResource.PreventiveAction, preventiveAction?.Name ?? ProblemAccountResource.All, textStyle);
            ExportSearchFilter(ProblemAccountResource.Username, searchFilter.Username, textStyle);
            ExportSearchFilter(ProblemAccountResource.SearchDownlineExcel, searchFilter.IsSearchDownlines ? nameof(IsReducedConst.Yes) : nameof(IsReducedConst.No), textStyle);
            ExportSearchFilter(ProblemAccountResource.OrderBy, orderBy?.Name ?? string.Empty, textStyle);
        }

        private void ExportSearchFilter(string filterText, string filterValue, ICellStyle celltyle)
        {
            CreateNPOIRowAndResetColumnIndex();
            ExportExcel.RenderCurrentCell(filterText, celltyle);
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
                ExportExcel.RenderCurrentCell(item.PreventiveAction, centerTextStyle);
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

        private bool IsMemberLevel => searchFilter.Level == RoleIdConstants.Member;

        private IEnumerable<decimal?> BuildMaxPerEventOrMaxPerBet(
            decimal? super,
            decimal? master,
            decimal? agent,
            decimal? member)
        {
            yield return super;

            if (searchFilter.Level <= RoleIdConstants.Master)
            {
                yield return master;
            }

            if (searchFilter.Level <= RoleIdConstants.Agent)
            {
                yield return agent;
            }

            if (searchFilter.Level <= RoleIdConstants.Member)
            {
                yield return member;
            }
        }

        private void ExportAccountCell(ProblemAccountItem problemAccount, IFont blueFont, ICellStyle wrapTextStyle)
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

        private static void ExportAccountCell(
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

        private IFont CreateBlueFont(ICellStyle wrapTextStyle)
        {
            var font = wrapTextStyle.GetFont(ExportExcel.ExcelFile);

            IFont blueFont = ExportExcel.ExcelFile.CreateFont();
            blueFont.FontHeightInPoints = font.FontHeightInPoints;
            blueFont.FontName = font.FontName;
            blueFont.Color = HSSFColor.Blue.Index;

            return blueFont;
        }

        private static void ApplyBlueFont(IRichTextString richText, int startIndex, int endIndex, IFont blueFont)
        {
            if (startIndex != -1 && endIndex != -1)
            {
                richText.ApplyFont(startIndex, endIndex, blueFont);
            }
        }

        private void ExportMaxPerEventOrMaxPerBet(IList<decimal?> values, IFont blueFont, ICellStyle wrapTextStyle)
        {
            var lastIndex = LastIndexOfMinValue(values);

            if (lastIndex == -1)
            {
                ExportExcel.RenderCurrentCell(string.Empty, wrapTextStyle);
            }
            else
            {
                ExportMaxPerEventOrMaxPerBet(values, lastIndex, blueFont, wrapTextStyle);
            }
        }

        private void ExportMaxPerEventOrMaxPerBet(IList<decimal?> values, int lastIndex, IFont blueFont, ICellStyle wrapTextStyle)
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

        private static int LastIndexOfMinValue(IList<decimal?> values)
        {
            var lastIndex = 0;
            var minValue = values[0];

            for (int i = 1, max = values.Count; i < max; i++)
            {
                if (minValue == null || (values[i].HasValue && values[i].Value <= minValue))
                {
                    lastIndex = i;
                    minValue = values[i];
                }
            }

            return minValue == null ? -1 : lastIndex;
        }

        private static string Join(IEnumerable<string> values, string separator)
            => values?.Any() == true ? string.Join(separator, values) : string.Empty;

        private static string HtmlDecodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlDecode(text);
    }
}
