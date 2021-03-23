using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Models.LimitNewDownline;

namespace Fanex.Nap.CMS.Customers.Repositories.LimitNewDownline
{
    public interface ILimitNewDownlineRepository
    {
        IEnumerable<string> GetSites(ICriteria criteria);

        IEnumerable<LimitDownlineItem> GetLimitDownlineList(GetLimitDownlineListCriteria criteria);

        AddNewOrRemoveLimitResult ExecuteNonQuery(ICriteria criteria);
    }
}
