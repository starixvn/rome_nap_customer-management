namespace Fanex.Nap.CMS.Customers.ViewModels.PossessStatistics
{
    public class IndexViewModel
    {
        public IndexViewModel(bool hasUpdateMonitorStatusPermission, bool hasUpdateDangerLevelPermission)
        {
            HasUpdateMonitorStatusPermission = hasUpdateMonitorStatusPermission;
            HasUpdateDangerLevelPermission = hasUpdateDangerLevelPermission;
            FilterViewModel = new FilterViewModel();
        }

        public bool HasUpdateMonitorStatusPermission { get; }

        public bool HasUpdateDangerLevelPermission { get; }

        public FilterViewModel FilterViewModel { get; }
    }
}