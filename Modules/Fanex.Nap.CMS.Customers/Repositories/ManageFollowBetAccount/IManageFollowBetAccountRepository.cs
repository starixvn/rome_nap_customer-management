namespace Fanex.Nap.CMS.Customers.Repositories.ManageFollowBetAccount
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IManageFollowBetAccountRepository
    {
        IEnumerable<SelectListItem> GetSites(ICriteria criteria);
        IEnumerable<SelectListItem> GetCurrencies(ICriteria criteria);
        IEnumerable<SelectListItem> GetDummyAgents(ICriteria criteria);
        IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(ICriteria criteria);
        Result ExecuteNonQuery(ICriteria criteria);
    }
}
