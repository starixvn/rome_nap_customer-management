namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport
{
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using System;
    using System.Collections.Generic;

    public class VrAdjustmentViewModel
    {
        public List<CustomerMarginModel> vrAdjustment { get; set; }

        public VrAdjustmentViewModel()
        {
            vrAdjustment = new List<CustomerMarginModel>();
        }
    }
}