namespace Fanex.Nap.CMS.Customers.Services.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Constants.ProblemAccount;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using Infrastructure.Services.Implementations;
    using Models.ProblemAccount;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using Resources;

    public abstract class ProblemAccountBaseExporter : BaseExcelExporter
    {
        protected const int Width100 = 100;
        protected const int Width140 = 140;
        protected const int Width200 = 200;
        protected const int Width300 = 300;
        protected const int Width400 = 400;
        protected const string Unlimited = "Unlimited";

        protected static readonly string NewLine = Environment.NewLine;
        protected static readonly string[] RoleShortNameMapper = new[] { "S:", "M:", "A:", "mb:" };
        
        protected readonly IEnumerable<IdNameModel> reasons;

        protected ProblemAccountBaseExporter(IEnumerable<IdNameModel> reasons)
        {
            this.reasons = reasons;
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

        protected abstract void ExportSearchFilter();

        protected void ExportCurrencySearchFilter(IEnumerable<IdNameModel> currencyList, int currency, ICellStyle celltyle)
        {
            var selectedCurrency = currencyList.FirstOrDefault(c => c.Id == currency);
            ExportSearchFilter(ProblemAccountResource.Currency, selectedCurrency?.Name, celltyle);
        }

        protected void ExportIsReducedSearchFilter(byte isReduced, ICellStyle celltyle)
        {
            var selectedIsReduced = IsReducedConst.IsReducedList().FirstOrDefault(r => r.Id == isReduced);
            ExportSearchFilter(ProblemAccountResource.IsReduced, selectedIsReduced?.Name, celltyle);
        }

        protected void ExportReasonsSearchFilter(string reason, ICellStyle celltyle)
        {
            var reasonDict = reasons.ToDictionary(r => r.Id.ToString(), r => r.Name);
            var reasonNames = Join(reason.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(r => reasonDict[r]), ", ");

            ExportSearchFilter(ProblemAccountResource.Reason, reasonNames, celltyle);
        }

        protected void ExportDirectlyMarkedSearchFilter(byte directlyMarked, ICellStyle celltyle)
        {
            var selectedDirectlyMarked = DirectlyMarkedConst.DirectlyMarkedList().FirstOrDefault(m => m.Id == directlyMarked);
            ExportSearchFilter(ProblemAccountResource.DirectlyMarked, selectedDirectlyMarked?.Name, celltyle);
        }

        protected void ExportOrderBySearchFilter(byte orderBy, ICellStyle celltyle)
        {
            var selectedOrderBy = OrderByConst.OrderByList().FirstOrDefault(o => o.Id == orderBy);
            ExportSearchFilter(ProblemAccountResource.OrderBy, selectedOrderBy?.Name, celltyle);
        }

        protected void ExportPreventiveActionSearchFilter(IEnumerable<IdNameModel> preventiveActionList, byte preventiveAction, ICellStyle celltyle)
        {
            var selectedPreventiveAction = preventiveActionList.FirstOrDefault(p => p.Id == preventiveAction);
            ExportSearchFilter(ProblemAccountCreditDepositResource.PreventiveAction, selectedPreventiveAction?.Name ?? ProblemAccountResource.All, celltyle);
        }

        protected void ExportSearchFilter(string filterText, string filterValue, ICellStyle celltyle)
        {
            CreateNPOIRowAndResetColumnIndex();
            ExportExcel.RenderCurrentCell(filterText, celltyle);
            ExportExcel.RenderCellBySpanning(ExportExcel.CurrentRow, ExportExcel.MaxColumnIndex, filterValue, celltyle);
            ExportExcel.CurrentRow++;
        }

        protected void CreateNPOIRowAndResetColumnIndex()
        {
            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();
        }

        protected static IEnumerable<decimal?> BuildMaxPerEventOrMaxPerBet(
            decimal? super,
            decimal? master,
            decimal? agent,
            decimal? member,
            byte currentLevel)
        {
            yield return super;

            if (currentLevel <= RoleIdConstants.Master)
            {
                yield return master;
            }

            if (currentLevel <= RoleIdConstants.Agent)
            {
                yield return agent;
            }

            if (currentLevel <= RoleIdConstants.Member)
            {
                yield return member;
            }
        }

        protected virtual void ExportAccountCell(
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

            var cellBuilder = new StringBuilder(problemAccount.Headline);

            ExportAccountCell(
                cellBuilder,
                problemAccount.IsProblemAccSuper,
                $"    {problemAccount.Super}",
                ref startIndexSuper,
                ref endIndexSuper);

            if (currentLevel <= RoleIdConstants.Master)
            {
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccMaster,
                    $"        {problemAccount.Master}",
                    ref startIndexMaster,
                    ref endIndexMaster);
            }

            if (currentLevel <= RoleIdConstants.Agent)
            {
                ExportAccountCell(
                    cellBuilder,
                    problemAccount.IsProblemAccAgent,
                    $"            {problemAccount.Agent}",
                    ref startIndexAgent,
                    ref endIndexAgent);
            }

            if (currentLevel <= RoleIdConstants.Member)
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

        protected virtual void ExportAccountCell(
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

        protected IFont CreateBlueFont(ICellStyle wrapTextStyle)
        {
            var font = wrapTextStyle.GetFont(ExportExcel.ExcelFile);

            IFont blueFont = ExportExcel.ExcelFile.CreateFont();
            blueFont.FontHeightInPoints = font.FontHeightInPoints;
            blueFont.FontName = font.FontName;
            blueFont.Color = HSSFColor.Blue.Index;

            return blueFont;
        }

        protected static void ApplyBlueFont(IRichTextString richText, int startIndex, int endIndex, IFont blueFont)
        {
            if (startIndex != -1 && endIndex != -1)
            {
                richText.ApplyFont(startIndex, endIndex, blueFont);
            }
        }

        protected void ExportMaxPerEventOrMaxPerBet(IList<decimal?> values, IFont blueFont, ICellStyle wrapTextStyle)
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

        protected virtual void ExportMaxPerEventOrMaxPerBet(IList<decimal?> values, int lastIndex, IFont blueFont, ICellStyle wrapTextStyle)
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

        protected static int LastIndexOfMinValue(IList<decimal?> values)
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

        protected static string Join(IEnumerable<string> values, string separator)
            => values?.Any() == true ? string.Join(separator, values) : string.Empty;

        protected static string HtmlDecodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlDecode(text);
    }
}
