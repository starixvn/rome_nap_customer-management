namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
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

        Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria);

        Result MarkNew(ICriteria criteria);

        Tuple<Result, string> Unmark(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
