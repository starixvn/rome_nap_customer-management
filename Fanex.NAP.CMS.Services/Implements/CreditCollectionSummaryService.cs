namespace Fanex.NAP.CMS.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.CreditCollectionSummary;
    using Fanex.Data.Repository;
    using Framework.Command;
    using Interfaces;
    using Models.CreditCollectionSummary;

    public class CreditCollectionSummaryService : CMSBaseService, ICreditCollectionSummaryService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CreditCollectionSummaryService()
            : this(new DynamicRepository())
        {
        }

        public CreditCollectionSummaryService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public CreditCollectionSummaryReport Generate(CreditCollectionSummaryFilter filter)
        {
            var criteria = new CreditCollectionSummaryCriteria
            {
                Code = filter.Code,
                AccountType = filter.AccountType,
                Status = filter.Status,
                LastBalanceDate = filter.LastBalanceDate,
                CurrentBalanceDate = filter.CurrentBalanceDate,
                BalanceUpDay = filter.BalanceUpDay,
                MoveBetsDay = filter.MoveBetsDay,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };

            var (breakDowns, uplines, balances, payments) = dynamicRepository.FetchMultiple<DateRange,CreditCollectionSummaryItem,CustomerBalance,CustomerPayment>(criteria);

            List<CreditCollectionSummaryItem> groups = GroupData(breakDowns.ToList(), uplines.ToList(), balances.ToList(), payments.ToList(), criteria);

            var result = new CreditCollectionSummaryReport
            {
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                LastBalanceDate = criteria.LastBalanceDate,
                CurrentBalanceDate = criteria.CurrentBalanceDate,
                BreakDowns = breakDowns,
                TotalRows = criteria.TotalItems,
                Groups = groups
            };

            return result;
        }

        public ICommandResult Save(SaveCreditStatusParam param)
        {
            var criteria = new SaveCreditStatusCriteria
            {
                CustId = param.CustId,
                Status = param.Status,
                UserID = param.UserId,
                IP = param.IP
            };

            return UpdateData(criteria);
        }
        private static List<CreditCollectionSummaryItem> GroupData(
            List<DateRange> breakdowns,
            List<CreditCollectionSummaryItem> uplines,
            List<CustomerBalance> balances,
            List<CustomerPayment> payments,
            CreditCollectionSummaryCriteria criteria)
        {
            var filteredBalances = balances.Where(b => criteria.AccountType == CodeType.All || criteria.AccountType == GetCodeType(b.Code)).ToList();
            var filteredPayments = payments.Where(p => criteria.AccountType == CodeType.All || criteria.AccountType == GetCodeType(p.Code)).ToList();

            var breakdownDict = new Dictionary<DateTime, int>();
            for (var i = 0; i < breakdowns.Count; i++)
            {
                breakdownDict.Add(breakdowns[i].FromDate, i);
            }

            var groups = new List<CreditCollectionSummaryItem>();

            foreach (var upline in uplines)
            {
                var group = new CreditCollectionSummaryItem
                {
                    CustId = upline.CustId,
                    GroupId = upline.GroupId,
                    Code = upline.Code,
                    Status = upline.Status,
                    IsGroupHeadline = upline.IsGroupHeadline,
                    Downlines = new List<CreditCollectionSummaryItem>(),
                    BreakDowns = Enumerable.Repeat((decimal)0, breakdowns.Count).ToList()
                };

                group.LastBalance = filteredBalances.Where(b => b.GroupId == group.GroupId && b.WinLossDate == criteria.LastBalanceDate)
                    .Select(b => b.Balance).Sum();
                group.CurrentBalance = filteredBalances.Where(b => b.GroupId == group.GroupId && b.WinLossDate == criteria.CurrentBalanceDate.AddDays(1))
                    .Select(b => b.Balance).Sum();
                groups.Add(group);
            }

            var custGroups = filteredBalances.GroupBy(b => new { b.GroupId, b.Code });
            foreach (var custGroup in custGroups)
            {
                var downline = new CreditCollectionSummaryItem
                {
                    GroupId = custGroup.Key.GroupId,
                    Code = custGroup.Key.Code,
                    LastBalance = custGroup.Where(c => c.WinLossDate == criteria.LastBalanceDate).Select(b => b.Balance).Sum(),
                    CurrentBalance = custGroup.Where(c => c.WinLossDate == criteria.CurrentBalanceDate.AddDays(1)).Select(b => b.Balance).Sum(),
                    BreakDowns = Enumerable.Repeat((decimal)0, breakdowns.Count).ToList()
                };

                var custPayments = filteredPayments.Where(p => p.GroupId == custGroup.Key.GroupId && p.Code == custGroup.Key.Code);
                foreach (var payment in custPayments)
                {
                    var ind = breakdownDict[payment.StartDate];
                    downline.BreakDowns[ind] = payment.Payment;
                }

                var upline = groups.FirstOrDefault(g => g.GroupId == custGroup.Key.GroupId && g.Code == custGroup.Key.Code);
                if (upline != null)
                {
                    upline.BreakDowns = downline.BreakDowns;
                    continue;
                }

                upline = groups.FirstOrDefault(g => g.GroupId == custGroup.Key.GroupId && g.AccountType == CodeType.Headline);
                upline.Downlines.Add(downline);
                for (var j = 0; j < downline.BreakDowns.Count; j++)
                {
                    upline.BreakDowns[j] += downline.BreakDowns[j];
                }
            }

            return groups.Where(g => g.AccountType == CodeType.Direct || g.Downlines.Count > 0).ToList();
        }
        private static int GetCodeType(string code) => string.Equals(code, "Other Codes", StringComparison.OrdinalIgnoreCase) ? CodeType.OtherCodes : CodeType.OldCodes;
    }
}
