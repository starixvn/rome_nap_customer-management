using System;

namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    [Serializable]
    public class GlobalSettingItem
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal MaxTurnover { get; set; }
    }
}
