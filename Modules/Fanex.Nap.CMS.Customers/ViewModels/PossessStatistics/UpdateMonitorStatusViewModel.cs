namespace Fanex.Nap.CMS.Customers.ViewModels.PossessStatistics
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Resources;

    public class UpdateMonitorStatusViewModel : BaseViewModel
    {
        public UpdateMonitorStatusViewModel()
        {
            MonitorStatus = true;
        }

        public string SelectedCustIds { get; set; }

        public bool MonitorStatus { get; set; }

        public IEnumerable<SelectListItem> MonitorStatusOptions { get; } = new[]
        {
            new SelectListItem { Value = true.ToString(), Text = PossessStatisticsResource.Yes },
             new SelectListItem { Value = false.ToString(), Text = PossessStatisticsResource.No },
        };
    }
}