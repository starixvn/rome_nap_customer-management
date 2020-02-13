namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Infrastructure.Models;
    using Resources;

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
            string updater,
            IEnumerable<IdNameModel> markNewReasons)
            : base(hasHostPermission, autoReload, searchViewModel, filterReasons, DefaultMinDate, CurrencyConst.CurrencyList(), updater)
        {
            MarkNewReasons = BuildMarkNewReasons(markNewReasons);
        }

        public IEnumerable<SelectListItem> AccountStatus
            => AccountStatusConst.AccountStatusList.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> MarkNewReasons { get; }

        private static IEnumerable<SelectListItem> BuildMarkNewReasons(IEnumerable<IdNameModel> reasons)
            => new[] { BuildSelectListItem(0, ProblemAccountResource.PleaseSelect) }
            .Concat(reasons.Select(r => BuildSelectListItem(r)))
            .ToList();
    }
}
