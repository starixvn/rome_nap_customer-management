namespace Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor
{
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using System.Collections.Generic;

    public class VirtualSettingViewModel
    {
        public VirtualSettingViewModel()
        {
            CustMargin = new List<CustomerVirtualSettingModel>();
        }

        public List<CustomerVirtualSettingModel> CustMargin { get; set; }        
    }
}
