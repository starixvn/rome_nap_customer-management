namespace Fanex.Nap.CMS.BetRestriction.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BetRestrictionViewModel
    {
        public BetRestrictionViewModel()
        { }

        public IEnumerable<SelectListItem> Sites { get; set; }

        public IEnumerable<SelectListItem> Currency { get; set; }
    }
}
