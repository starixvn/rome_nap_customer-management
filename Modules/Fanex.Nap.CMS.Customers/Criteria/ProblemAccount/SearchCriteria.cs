namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using System;
    using Data.Repository;

    public class SearchCriteria : CriteriaBase
    {
        public SearchCriteria(
            DateTime from,
            DateTime to,
            byte roleId,
            int currencyId,
            byte accountStatus,
            byte isReduced,
            string reasonIds,
            string username,
            bool isSearchDownlines,
            byte orderBy,
            byte isDirectlyMarked,
            int pageIndex,
            int pageSize)
        {
            FromDate = from;
            ToDate = to;
            RoleId = roleId;
            CurrencyId = currencyId;
            Status = accountStatus;
            IsReduce = isReduced;
            ReasonId = reasonIds;
            UserSearch = username;
            IsSearchDownline = isSearchDownlines;
            OrderType = orderBy;
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsDirectlyMarked = isDirectlyMarked;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte RoleId { get; }

        public int CurrencyId { get; }

        public byte Status { get; }

        public byte IsReduce { get; }

        public string ReasonId { get; }

        public string UserSearch { get; }

        public bool IsSearchDownline { get; }

        public byte OrderType { get; }

        public byte IsDirectlyMarked { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_MonitorList";

        public override bool IsValid()
            => true;
    }
}
