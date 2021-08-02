namespace Fanex.Nap.CMS.BetRestriction.Services
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels.Basketball;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBetRestrictionBasketballService
    {
        IEnumerable<SelectListItem> GetSites(GetAllSiteCriteria criteria);

        IEnumerable<SelectListItem> GetCurrency(GetAllCurrencyCriteria criteria);

        BetRestrictionBasketballSearchViewModel Search(SearchAccountCriteria criteria);

        BetRestrictionBasketballDataGridViewModel<T> SearchDataGrid<T>(PagingCriteria criteria);

        Result CheckAddAccount(AccountRequest request);

        Result CheckEditAccount(AccountRequest request);

        EditAccountViewModel GetAccountDetailForEdit(int custId);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
