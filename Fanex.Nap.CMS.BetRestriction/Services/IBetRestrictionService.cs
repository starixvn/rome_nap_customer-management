namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Models;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBetRestrictionService
    {
        IEnumerable<SelectListItem> GetSites(GetAllSiteCriteria criteria);

        IEnumerable<SelectListItem> GetCurrency(GetAllCurrencyCriteria criteria);

        BetRestrictionSearchViewModel Search(SearchAccountCriteria criteria);

        BetRestrictionDataGridViewModel<T> SearchDataGrid<T>(PagingCriteria criteria);

        Result CheckAddAccount(AccountRequest request, short sportType);

        ValidateCustomerResultModel ValidateCustomer(AccountRequest request, short sportType);

        EditAccountViewModel GetAccountDetailForEdit(int custId, short sportType);

        EditSiteViewModel GetSiteDetailForEdit(int siteId);

        EditCurrencyViewModel GetCurrencyDetailForEdit(int exchangeId, int type);

        Result ExecuteNonQuery(CommandBaseCriteria criteria);

        IEnumerable<SelectListItem> GetLeagueGroup(GetAllLeagueGroupCriteria criteria);
    }
}
