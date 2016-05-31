namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    using System.Collections.Generic;

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
