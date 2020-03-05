namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Infrastructure.Models;
    using ProblemAccount;
    using Resources;

    public class IndexLicenseeSiteViewModel : IndexBaseViewModel
    {
        // The release date of this feature.
        private static readonly DateTime DefaultMinDate = new DateTime(2020, 2, 1);

        public IndexLicenseeSiteViewModel(
            bool hasHostPermission,
            bool autoReload,
            SearchBaseViewModel searchViewModel,
            IEnumerable<IdNameModel> filterReasons,
            IEnumerable<IdNameModel> markNewReasons,
            string updater,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> preventiveActions)
            : base(hasHostPermission, autoReload, searchViewModel, filterReasons, markNewReasons, DefaultMinDate, currencies, updater)
        {
            FilterPreventiveActions = BuildSelectListItem(new IdNameModel { Id = 0, Name = ProblemAccountResource.All }, preventiveActions);
            MarkNewPreventiveActions = BuildSelectListItem(new IdNameModel { Id = 0, Name = ProblemAccountResource.PleaseSelect }, preventiveActions);
        }

        public IEnumerable<SelectListItem> AcknowledgementTypes
            => AcknowledgementTypeConst.AcknowledgementTypeList().Select(a => BuildSelectListItem(a));

        public IEnumerable<SelectListItem> VIPs
            => VIPConst.VIPList().Select(v => BuildSelectListItem(v));

        public IEnumerable<SelectListItem> FilterPreventiveActions { get; }

        public IEnumerable<SelectListItem> MarkNewPreventiveActions { get; }
    }
}
