namespace Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard
{
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;

    public interface ICustomerDashboardService
    {
        CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria);
    }
}
