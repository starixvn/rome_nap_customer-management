namespace Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants.ProblemAccountLicenseeSite;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Services.Interfaces;
    using Models.ProblemAccountLicenseeSite;
    using NPOI.HSSF.Util;
    using ProblemAccount;
    using Resources;
    using ViewModels.ProblemAccountLicenseeSite;

    public sealed class ProblemAccountLicenseeSiteExporter : ProblemAccountBaseExporter, IExcelExporter
    {
        private readonly SearchLicenseeSiteViewModel searchFilter;
        private readonly ProblemAccountLicenseeSiteModel data;
        private readonly IEnumerable<IdNameModel> currencies;
        private readonly IEnumerable<IdNameModel> preventiveActions;

        public const string VoidWinningAction = "Void Winning Tickets";

        public ProblemAccountLicenseeSiteExporter(
            SearchLicenseeSiteViewModel searchFilter,
            ProblemAccountLicenseeSiteModel data,
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
           => string.Format(ProblemAccountCreditDepositResource.ExcelFileNameLicenseeSite, DateTime.Today.ToString("yyyyMMdd"));

        public override ExcelColumn[] ExcelColumns =>
            new[]
            {
                new ExcelColumn(ProblemAccountCreditDepositResource.Member, Width300),
                new ExcelColumn(ProblemAccountResource.Currency, Width100),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxPerMatch, Width140),
                new ExcelColumn(ProblemAccountCreditDepositResource.MaxBet, Width140),
                new ExcelColumn(ProblemAccountCreditDepositResource.PreventiveAction, Width300),
                new ExcelColumn(ProblemAccountResource.MarkDate, Width140),
                new ExcelColumn(ProblemAccountResource.Reason, Width400),
                new ExcelColumn(ProblemAccountResource.Comment, Width400),
                new ExcelColumn(ProblemAccountResource._1stWeekInformed, Width140),
                new ExcelColumn(ProblemAccountResource._2ndWeekInformed, Width140),
                new ExcelColumn(ProblemAccountResource.AbnormalBet, Width140)
            };

        protected override void ExportSearchFilter()
        {
            var textStyle = NpoiExcelExtensions.TextStyleWithoutBorder(ExportExcel.ExcelFile, HSSFColor.White.Index, false, Alignment.Left);

            var acknowledgement = AcknowledgementTypeConst.AcknowledgementTypeList().FirstOrDefault(a => a.Id == searchFilter.Acknowledge);
            var vip = VIPConst.VIPList().FirstOrDefault(v => v.Id == searchFilter.VIP);

            ExportSearchFilter(ProblemAccountResource.From, searchFilter.From.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountResource.To, searchFilter.To.ToShortFormatedDateWithSlash(), textStyle);
            ExportSearchFilter(ProblemAccountCreditDepositResource.Type, acknowledgement?.Name, textStyle);
            ExportSearchFilter(ProblemAccountCreditDepositResource.VIP, vip?.Name, textStyle);
            ExportCurrencySearchFilter(currencies, searchFilter.Currency, textStyle);
            ExportIsReducedSearchFilter(searchFilter.IsReduced, textStyle);
            ExportReasonsSearchFilter(searchFilter.Reason, textStyle);
            ExportPreventiveActionSearchFilter(preventiveActions, searchFilter.PreventiveActionId, textStyle);
            ExportSearchFilter(ProblemAccountCreditDepositResource.Member, searchFilter.Member, textStyle);
            ExportSearchFilter(ProblemAccountCreditDepositResource.Licensee, searchFilter.Licensee, textStyle);
        }

        protected override void ExportBody()
        {
            if (data.TotalItems == 0)
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
                return;
            }

            const int VIPColSpan = 3;
            var vipStyle = NpoiExcelExtensions.VIPStyle(ExportExcel.ExcelFile);

            var wrapTextStyle = ExcelStyleTemplate.TextStyle;
            wrapTextStyle.WrapText = true;

            var boldTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Left);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            var numberStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false);

            foreach (var siteGroup in data.ProblemAccounts)
            {
                CreateNPOIRowAndResetColumnIndex();
                ExportExcel.RenderCellBySpanning(
                    ExportExcel.CurrentRow,
                    ExportExcel.MaxColumnIndex,
                    $"{siteGroup.Site} [{siteGroup.Acknowledged}]",
                    boldTextStyle);
                ExportExcel.CurrentRow++;

                foreach (var item in siteGroup.ProblemAccounts)
                {
                    ExportExcel.ResetCurrentColumn();
                    ExportExcel.RenderCurrentCell(
                        string.IsNullOrWhiteSpace(item.Username2) ? $"    {item.Username}" : $"    {item.Username}{NewLine}    {item.Username2}",
                        wrapTextStyle);
                    ExportExcel.RenderCurrentCell(item.Currency, centerTextStyle);

                    if (item.IsVIP)
                    {
                        ExportExcel.RenderCellBySpanning(ExportExcel.CurrentRow, VIPColSpan, ProblemAccountCreditDepositResource.VIP, vipStyle);
                    }
                    else
                    {
                        ExportExcel.RenderCurrentCell(FormatDecimal(item.MaxPerMatch), numberStyle);
                        ExportExcel.RenderCurrentCell(FormatDecimal(item.MaxBet), numberStyle);
                    }

                    ExportExcel.RenderCurrentCell(BuildPreventiveAction(item), centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.MarkDateString, centerTextStyle);
                    ExportExcel.RenderCurrentCell(Join(item.Reasons, NewLine), wrapTextStyle);
                    ExportExcel.RenderCurrentCell(HtmlDecodeText(item.Comment), wrapTextStyle);
                    ExportExcel.RenderCurrentCell(item.Inform1, centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.Inform2, centerTextStyle);
                    ExportExcel.RenderCurrentCell(item.AbnormalBetNumber.Value, numberStyle);

                    ExportExcel.CurrentRow++;
                }
            }
        }

        private static string FormatDecimal(decimal? value)
            => value == null ? "N/A" : value.Value.ToString("N0");


        private static string BuildPreventiveAction(ProblemAccountLicenseeSiteItem problemAccount)
        {
            if (problemAccount.ActionName == VoidWinningAction)
            {
                return $"{problemAccount.ActionName} - {problemAccount.VoidTime} - {problemAccount.LimitAmount}";
            }

            return problemAccount.ActionName;
        }
    }
}
