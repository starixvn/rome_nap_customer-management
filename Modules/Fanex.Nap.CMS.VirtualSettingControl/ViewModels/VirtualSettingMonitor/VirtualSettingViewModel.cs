namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor
{
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using System.Collections.Generic;

    public class VirtualSettingViewModel
    {
        public List<CustomerVirtualSettingModel> CustMargin { get; set; }

        public VirtualSettingViewModel()
        {
            CustMargin = new List<CustomerVirtualSettingModel>();
        }
    }
}
