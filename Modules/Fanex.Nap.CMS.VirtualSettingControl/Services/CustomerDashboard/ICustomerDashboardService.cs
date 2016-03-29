namespace Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface ICustomerDashboardService
    {
        CustomerDashboardModel GetDashboardInfo(ICriteria criteria);
    }
}
