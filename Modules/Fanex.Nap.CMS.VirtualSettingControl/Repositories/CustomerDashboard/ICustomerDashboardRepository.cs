namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.CustomerDashboard
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;

    public interface ICustomerDashboardRepository
    {
        CustomerDashboardModel GetDashboardInfo(ICriteria criteria);
    }
}
