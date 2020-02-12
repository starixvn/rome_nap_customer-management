namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using System;

    public class SearchCreditSitesCriteria : SearchCriteria
    {
        public SearchCreditSitesCriteria(
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
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_MonitorList";
    }
}
