namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountCreditSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants.ProblemAccountCreditSite;
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
            IEnumerable<IdNameModel> markNewReasons,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> preventiveActions)
            : base(hasHostPermission, autoReload, searchViewModel, filterReasons, markNewReasons, DefaultMinDate, currencies, string.Empty)
        {
            FilterPreventiveActions = BuildSelectListItem(new IdNameModel { Id = 0, Name = ProblemAccountResource.All }, preventiveActions);
            MarkNewPreventiveActions = BuildSelectListItem(new IdNameModel { Id = 0, Name = ProblemAccountResource.PleaseSelect }, preventiveActions);
        }

        public IEnumerable<SelectListItem> AccountStatus
            => AccountStatusCreditSiteConst.AccountStatusListForFilter.Select(kvp => BuildSelectListItem(kvp.Key, kvp.Value));

        public IEnumerable<SelectListItem> FilterPreventiveActions { get; }

        public IEnumerable<SelectListItem> MarkNewPreventiveActions { get; }
    }
}
