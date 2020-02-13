namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public interface IProblemAccountRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);

        IEnumerable<IdNameModel> GetReasons(ICriteria criteria);

        IEnumerable<IdNameModel> GetPreventiveActions(ICriteria criteria);

        IEnumerable<PreventiveActionReason> GetPreventiveActionReasons(ICriteria criteria);

        Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria);

        Result MarkNew(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
