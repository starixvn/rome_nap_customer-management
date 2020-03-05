namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using System;
    using ProblemAccount;

    public class SearchCreditSiteCriteria : SearchCriteria
    {
        public SearchCreditSiteCriteria(
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
            byte preventActionId,
            int pageIndex,
            int pageSize) : base(
                from,
                to,
                roleId,
                currencyId,
                accountStatus,
                isReduced,
                reasonIds,
                username,
                isSearchDownlines,
                orderBy,
                isDirectlyMarked,
                pageIndex,
                pageSize)
        {
            PreventActionId = preventActionId;
        }

        public byte PreventActionId { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_MonitorList";
    }
}
