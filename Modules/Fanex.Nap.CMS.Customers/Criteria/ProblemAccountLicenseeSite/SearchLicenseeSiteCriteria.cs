namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using System;
    using Fanex.Data.Repository;

    public class SearchLicenseeSiteCriteria : CriteriaBase
    {
        public SearchLicenseeSiteCriteria(
            DateTime from,
            DateTime to,
            byte acknowledge,
            byte vip,
            int currency,
            byte isReduced,
            byte preventiveActionId,
            string member,
            string licensee,
            string reasonIds,
            int pageIndex,
            int pageSize)
        {
            FromDate = from;
            ToDate = to;
            Type = acknowledge;
            VIP = vip;
            Currency = currency;
            IsReduced = isReduced;
            ActionId = preventiveActionId;
            Username = member;
            Licensee = licensee;
            ReasonList = reasonIds;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte Type { get; }

        public byte VIP { get; }

        public int Currency { get; }

        public byte IsReduced { get; }

        public byte ActionId { get; }

        public string Username { get; }

        public string Licensee { get; }

        public string ReasonList { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeMonitorListSel";

        public override bool IsValid()
         => true;
    }
}
