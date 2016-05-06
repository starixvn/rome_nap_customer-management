namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.CustomerDashboard
{
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;

    public interface ICustomerDashboardRepository
    {
        CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria);
    }
}
