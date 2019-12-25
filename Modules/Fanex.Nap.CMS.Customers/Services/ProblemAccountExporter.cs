using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Fanex.Nap.CMS.Customers.Constants;
using Fanex.Nap.CMS.Customers.Models.ProblemAccount;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace Fanex.Nap.CMS.Customers.Services
{
    public sealed class ProblemAccountExporter : BaseExcelExporter, IExcelExporter
    {
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string[] RoleShortNameMapper = new[] { "S:", "M:", "A:", "mb:" };
        private static readonly IDictionary<int, string> RoleNameMapper = new Dictionary<int, string>
        {
            [RoleIdConstants.Super] = nameof(RoleIdConstants.Super),
            [RoleIdConstants.Master] = nameof(RoleIdConstants.Master),
            [RoleIdConstants.Agent] = nameof(RoleIdConstants.Agent),
            [RoleIdConstants.Member] = nameof(RoleIdConstants.Member)
        };

        private static readonly IDictionary<int, string> DirectlyMarkedMapper = new Dictionary<int, string>
        {
            [MarkTypeConst.All] = nameof(MarkTypeConst.All),
            [MarkTypeConst.Yes] = nameof(MarkTypeConst.Yes),
            [MarkTypeConst.No] = nameof(MarkTypeConst.No)
        };

        private readonly SearchViewModel searchFilter;
        private readonly ProblemAccountModel data;
        private readonly IEnumerable<IdNameModel> reasons;

        public ProblemAccountExporter(
            SearchViewModel searchFilter,
            ProblemAccountModel data,
            IEnumerable<IdNameModel> reasons)
        {
            this.searchFilter = searchFilter;
            this.data = data;
            this.reasons = reasons;
        }

        public override string FileNameWithoutExtension
            => string.Format(ProblemAccountResource.ExcelFileName, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns => GetExcelColumns();

        private ExcelColumn[] GetExcelColumns()
        {
            const int Width140 = 140;
            const int Width300 = 300;
            const int Width400 = 400;

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

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.Level, RoleNameMapper[searchFilter.Level], textStyle);
            ExportSearchFilter(ProblemAccountResource.Currency, CurrencyConst.CurrencyList[searchFilter.Currency], textStyle);
            ExportSearchFilter(ProblemAccountResource.AccountStatus, AccountStatusConst.AccountStatusList[searchFilter.Status], textStyle);
            ExportSearchFilter(ProblemAccountResource.IsReduced, IsReducedConst.IsReducedList[searchFilter.IsReduced], textStyle);
            ExportSearchFilter(ProblemAccountResource.Reason, reasonNames, textStyle);
            ExportSearchFilter(ProblemAccountResource.DirectlyMarked, DirectlyMarkedMapper[searchFilter.DirectlyMarked], textStyle);
            ExportSearchFilter(ProblemAccountResource.Username, searchFilter.Username, textStyle);
            ExportSearchFilter(ProblemAccountResource.SearchDownlineExcel, searchFilter.IsSearchDownlines ? nameof(IsReducedConst.Yes) : nameof(IsReducedConst.No), textStyle);
            ExportSearchFilter(ProblemAccountResource.OrderBy, OrderByConst.OrderByList[searchFilter.OrderBy], textStyle);
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

            IList<decimal?> maxPerEvents;
            IList<decimal?> maxPerBets;

            foreach (var item in data.ProblemAccounts)
            {
                maxPerEvents = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMPM,
                    item.MasterMPM,
                    item.AgentMPM,
                    item.MPM).ToList();
                maxPerBets = BuildMaxPerEventOrMaxPerBet(
                    item.SuperMaxBet,
                    item.MasterMaxBet,
                    item.AgentMaxBet,
                    item.MaxBet).ToList();

                ExportExcel.ResetCurrentColumn();
                ExportAccountCell(item, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.StatusName, centerTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerEvents, blueFont, wrapTextStyle);
                ExportMaxPerEventOrMaxPerBet(maxPerBets, blueFont, wrapTextStyle);
                ExportExcel.RenderCurrentCell(item.MarkDateString, centerTextStyle);
                ExportExcel.RenderCurrentCell(Join(item.Reasons, NewLine), wrapTextStyle);
                ExportExcel.RenderCurrentCell(HtmlDecodeText(item.Comment), wrapTextStyle);
                ExportExcel.RenderCurrentCell(item._1stWeekInformed ?? string.Empty, centerTextStyle);
                ExportExcel.RenderCurrentCell(item._2ndWeekInformed ?? string.Empty, centerTextStyle);

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

            var cellBuilder = new StringBuilder(problemAccount.Headline);

            ExportAccountCell(
                cellBuilder,
                problemAccount.IsProblemAccSuper,
                $"    {problemAccount.Super}",
                ref startIndexSuper,
                ref endIndexSuper);

            if (searchFilter.Level <= RoleIdConstants.Master)
            {
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccMaster,
                    $"        {problemAccount.Master}",
                    ref startIndexMaster,
                    ref endIndexMaster);
            }

            if (searchFilter.Level <= RoleIdConstants.Agent)
            {
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccAgent,
                    $"            {problemAccount.Agent}",
                    ref startIndexAgent,
                    ref endIndexAgent);
            }

            if (searchFilter.Level <= RoleIdConstants.Member)
            {
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAcc,
                    $"                {problemAccount.Username}",
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
            cellBuilder.Append(NewLine);

            if (isProblemAcc)
            {
                startIndex = cellBuilder.Length - 1;
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

                cellBuilder.Append(NewLine);

                if (i == lastIndex)
                {
                    startIndex = cellBuilder.Length - 1;
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
