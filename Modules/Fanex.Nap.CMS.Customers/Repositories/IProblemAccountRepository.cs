namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using Criteria.ProblemAccount;
    using Data.Repository;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public interface IProblemAccountRepository
    {
        IEnumerable<IdNameModel> GetReasons(ICriteria criteria);

        Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>> GetProblemAccounts(SearchCriteria criteria);

        void MarkNew(MarkNewCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
