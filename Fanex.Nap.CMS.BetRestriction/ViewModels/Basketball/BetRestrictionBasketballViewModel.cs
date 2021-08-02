
namespace Fanex.Nap.CMS.BetRestriction.ViewModels.Basketball
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BetRestrictionBasketballViewModel
    {
        public BetRestrictionBasketballViewModel()
        { }

        public IEnumerable<SelectListItem> Sites { get; set; }

        public IEnumerable<SelectListItem> Currency { get; set; }
    }
}
