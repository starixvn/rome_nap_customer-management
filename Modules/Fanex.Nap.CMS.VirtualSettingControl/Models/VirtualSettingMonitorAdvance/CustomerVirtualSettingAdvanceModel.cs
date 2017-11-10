namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitorAdvance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class CustomerVirtualSettingAdvanceModel
    {
        public CustomerVirtualSettingAdvanceModel()
        {
            Items = new List<VirtualSettingAdvanceModel>();
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public List<VirtualSettingAdvanceModel> Items { get; set; }

        public VirtualSettingAdvanceModel Total
        {
            get
            {
                var result = new VirtualSettingAdvanceModel
                {
                    BetCount = Items.Sum(t => t.BetCount),
                    Turnover = Items.Sum(t => t.Turnover),
                    Winloss = Items.Sum(t => t.Winloss),
                    TurnoverVirtual = Items.Sum(t => t.TurnoverVirtual),
                    WinlossVirtual = Items.Sum(t => t.WinlossVirtual)
                };

                return result;
            }
        }
    }
}