namespace Fanex.Nap.CMS.BetRestriction.ViewModels
{
    using Fanex.Nap.CMS.BetRestriction.Models;
    using System.Collections.Generic;

    public class BetRestrictionSearchViewModel
    {
        public BetRestrictionSearchViewModel()
        { }

        public BetRestrictionDataGridViewModel<AccountModel> AccountViewModel { get; set; }

        public BetRestrictionDataGridViewModel<SiteModel> SiteViewModel { get; set; }

        public BetRestrictionDataGridViewModel<BetRestrictionCurrencyModel> CurrencyViewModel { get; set; }
    }

    public class BetRestrictionDataGridViewModel<T>
    {
        public BetRestrictionDataGridViewModel() { }

        public IEnumerable<T> DataList { get; set; }

        public int TotalItems { get; set; }
    }
}
