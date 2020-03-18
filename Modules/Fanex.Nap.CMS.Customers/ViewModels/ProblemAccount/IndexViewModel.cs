namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants.ProblemAccount;
    using Infrastructure.Models;

    public class IndexViewModel : IndexBaseViewModel
    {
        public const int MaxPerEvent = 5000;
        public const int MaxPerBet = 1000;

        // The release date of this feature.
        private static readonly DateTime DefaultMinDate = new DateTime(2019, 9, 12);

        public IndexViewModel(
            bool hasHostPermission,
            bool autoReload,
            SearchBaseViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons,
            string updater)
            : base(hasHostPermission, autoReload, searchViewModel, filterReasons, markNewReasons, DefaultMinDate, CurrencyConst.CurrencyList(), updater)
        {
        }

        public IEnumerable<SelectListItem> AccountStatus
            => AccountStatusConst.AccountStatusList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));
    }
}
