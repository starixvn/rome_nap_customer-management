namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System.Collections.Generic;
    using Criteria.ProblemAccount;
    using Data.Repository;
    using Infrastructure.Models;

    public interface IProblemAccountRepository
    {
        IEnumerable<IdNameModel> GetReasons(ICriteria criteria);

        void MarkNew(MarkNewCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
