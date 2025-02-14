namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite;
    using Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public interface IProblemAccountRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetReasons(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetPreventiveActions(CriteriaBase criteria);

        Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(CriteriaBase criteria);

        Result MarkNew(MarkNewCriteria criteria);

        Tuple<Result, string> Unmark(UnmarkLicenseeSiteCriteria criteria);

        Tuple<Result, IEnumerable<CTSCustomerReason>> Update(NonQueryWithoutRoleIdCriteriaBase criteria, byte newReasonId);

        Result ExecuteNonQuery(CommandBaseCriteria criteria);
    }
}
