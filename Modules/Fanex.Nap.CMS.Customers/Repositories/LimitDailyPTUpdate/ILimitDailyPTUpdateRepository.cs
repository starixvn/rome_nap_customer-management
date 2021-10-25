namespace Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using System.Collections.Generic;

    public interface ILimitDailyPTUpdateRepository
    {
        IEnumerable<string> GetSites();

        IEnumerable<string> GetProducts();

        IEnumerable<string> GetCurrencies();

        IEnumerable<LimitDailyPTUpdateItem> GetLimitDailyPTUpdateList(GetLimitDailyPTUpdateListCriteria criteria);
    }
}
