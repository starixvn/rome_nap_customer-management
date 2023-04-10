namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Models;

    public class RuleViewModel : BaseViewModel
    {
        public RuleViewModel(
            bool hasHostPermission,
            IEnumerable<short> customerClasses,
            IEnumerable<LeagueGroup> leagueGroups) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses.Select(cc => ToSelectListItem(cc)).ToList();
            LeagueGroups = leagueGroups;
        }

        public IEnumerable<SelectListItem> CustomerClasses { get; }

        public IEnumerable<LeagueGroup> LeagueGroups { get; }

        public IEnumerable<SelectListItem> Indicators { get; }
            = IndicatorConst.Indicators.Select(kvp => ToSelectListItem(kvp)).ToList();
    }
}
