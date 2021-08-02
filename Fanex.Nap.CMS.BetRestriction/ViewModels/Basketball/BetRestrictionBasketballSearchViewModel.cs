namespace Fanex.Nap.CMS.BetRestriction.ViewModels.Basketball
{
    using Fanex.Nap.CMS.BetRestriction.Models.Basketball;
    using System.Collections.Generic;

    public class BetRestrictionBasketballSearchViewModel
    {
        public BetRestrictionBasketballSearchViewModel()
        { }

        public BetRestrictionBasketballDataGridViewModel<AccountModel> AccountViewModel { get; set; }
    }

    public class BetRestrictionBasketballDataGridViewModel<T>
    {
        public BetRestrictionBasketballDataGridViewModel() 
        { }

        public IEnumerable<T> DataList { get; set; }

        public int TotalItems { get; set; }
    }
}
