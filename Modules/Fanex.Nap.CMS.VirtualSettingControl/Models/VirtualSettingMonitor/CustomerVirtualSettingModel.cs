namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomerVirtualSettingModel
    {
        public CustomerVirtualSettingModel()
        {
            CustMargin = new List<MarginModel>();
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal VRate { get; set; }

        public List<MarginModel> CustMargin { get; set; }
    }
}
