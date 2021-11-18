namespace Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ILimitDailyPTUpdateRepository
    {
        IEnumerable<SelectListItem> GetSites(ICriteria criteria);

        IEnumerable<SelectListItem> GetProducts(ICriteria criteria);

        IEnumerable<SelectListItem> GetCurrencies(ICriteria criteria);

        IEnumerable<LimitDailyPTUpdateItem> GetLimitDailyPTUpdateList(GetLimitDailyPTUpdateListCriteria criteria);

        AddNewOrRemoveLimitResult ExecuteNonQuery(ICriteria criteria);
    }
}
