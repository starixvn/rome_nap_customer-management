namespace Fanex.NAP.CMS.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Criteria.CreditCollectionSummary;
    using Data;
    using Interfaces;
    using Models.CreditCollectionSummary;

    public class CreditCollectionSummaryRepository : ICreditCollectionSummaryRepository
    {
        public CreditCollectionSummaryReport Generate(CreditCollectionSummaryCriteria criteria)
        {
            var breakDowns = new List<DateRange>();
            List<CreditCollectionSummaryItem> groups;
            int totalItems;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var dataset = db.ExecuteDataSet(criteria))
                {
                    if (dataset.Tables.Count > 0)
                    {
                        breakDowns = GetBreakDowns(dataset.Tables[0]);
                    }

                    var uplines = new List<CreditCollectionSummaryItem>();
                    var balances = new List<CustomerBalance>();
                    var payments = new List<CustomerPayment>();
                    if (dataset.Tables.Count > 1)
                    {
                        uplines = GetUplines(dataset.Tables[1]);
                    }

                    if (dataset.Tables.Count > 2)
                    {
                        balances = GetBalances(dataset.Tables[2]);
                    }

                    if (dataset.Tables.Count > 3)
                    {
                        payments = GetPayments(dataset.Tables[3]);
                    }

                    groups = GroupData(breakDowns, uplines, balances, payments, criteria);
                }

                totalItems = Converter.ToInt(db.GetOutputParams()["totalitems"]);
            }

            var result = new CreditCollectionSummaryReport
            {
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                LastBalanceDate = criteria.LastBalanceDate,
                CurrentBalanceDate = criteria.CurrentBalanceDate,
                BreakDowns = breakDowns,
                TotalRows = totalItems,
                Groups = groups
            };

            return result;
        }

        private static List<DateRange> GetBreakDowns(DataTable table)
        {
            var breakDowns = new List<DateRange>();

            foreach (DataRow row in table.Rows)
            {
                breakDowns.Add(new DateRange
                {
                    FromDate = Converter.ToDateTime(row["StartDate"]),
                    ToDate = Converter.ToDateTime(row["EndDate"]),
                });
            }

            return breakDowns;
        }

        private static List<CreditCollectionSummaryItem> GetUplines(DataTable table)
        {
            var uplines = new List<CreditCollectionSummaryItem>();

            foreach (DataRow row in table.Rows)
            {
                uplines.Add(new CreditCollectionSummaryItem
                {
                    CustId = Converter.ToInt(row["CustId"]),
                    GroupId = Converter.ToInt(row["GroupId"]),
                    Code = Converter.ToString2(row["Username"]),
                    Status = Converter.ToInt(row["Coll_Status"]),
                    AccountType = Converter.ToBool(row["IsGroupHeadLine"]) ? CodeType.Headline : CodeType.Direct
                });
            }

            return uplines;
        }

        private static List<CustomerBalance> GetBalances(DataTable table)
        {
            var balances = new List<CustomerBalance>();

            foreach (DataRow row in table.Rows)
            {
                balances.Add(new CustomerBalance
                {
                    GroupId = Converter.ToInt(row["GroupId"]),
                    Code = Converter.ToString2(row["Username"]),
                    Balance = Converter.ToDecimal(row["Balance"]),
                    WinLossDate = Converter.ToDateTime(row["WinlostDate"])
                });
            }

            return balances;
        }

        private static List<CustomerPayment> GetPayments(DataTable table)
        {
            var payments = new List<CustomerPayment>();

            foreach (DataRow row in table.Rows)
            {
                payments.Add(new CustomerPayment
                {
                    GroupId = Converter.ToInt(row["GroupId"]),
                    Code = Converter.ToString2(row["Username"]),
                    StartDate = Converter.ToDateTime(row["StartDate"]),
                    EndDate = Converter.ToDateTime(row["EndDate"]),
                    Payment = Converter.ToDecimal(row["Amount"])
                });
            }

            return payments;
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
                    AccountType = upline.AccountType,
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
