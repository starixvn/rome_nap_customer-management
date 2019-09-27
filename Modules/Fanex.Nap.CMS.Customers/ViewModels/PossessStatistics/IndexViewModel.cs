namespace Fanex.Nap.CMS.Customers.ViewModels.PossessStatistics
{
    public class IndexViewModel
    {
        public IndexViewModel(bool hasHostPermission)
        {
            HasHostPermission = hasHostPermission;
            FilterViewModel = new FilterViewModel();
        }

        public bool HasHostPermission { get; }

        public FilterViewModel FilterViewModel { get; }
    }
}
