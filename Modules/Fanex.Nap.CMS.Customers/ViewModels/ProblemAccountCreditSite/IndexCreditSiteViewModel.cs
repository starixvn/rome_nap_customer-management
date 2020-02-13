namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountCreditSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Infrastructure.Models;
    using ProblemAccount;
    using Resources;

    public class IndexCreditSiteViewModel : IndexBaseViewModel
    {
        // The release date of this feature.
        private static readonly DateTime DefaultMinDate = new DateTime(2020, 2, 1);

        public IndexCreditSiteViewModel(
            bool hasHostPermission,
            bool autoReload,
            SearchBaseViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> preventiveActions,
            IDictionary<int, IList<IdNameModel>> preventiveActionReasons)
            : base(hasHostPermission, autoReload, searchViewModel, filterReasons, DefaultMinDate, currencies, string.Empty)
        {
            FilterPreventiveActions = new[] { new IdNameModel { Id = 0, Name = ProblemAccountResource.All } }
                .Concat(preventiveActions)
                .Select(c => BuildSelectListItem(c));
            MarkNewPreventiveActions = new[] { new IdNameModel { Id = 0, Name = ProblemAccountResource.PleaseSelect } }
                .Concat(preventiveActions)
                .Select(c => BuildSelectListItem(c));
            PreventiveActionReasons = preventiveActionReasons;
        }

        public IEnumerable<SelectListItem> AccountStatus
            => AccountStatusCreditSiteConst.AccountStatusListForFilter.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> FilterPreventiveActions { get; }

        public IEnumerable<SelectListItem> MarkNewPreventiveActions { get; }

        public IDictionary<int, IList<IdNameModel>> PreventiveActionReasons { get; }
    }
}
