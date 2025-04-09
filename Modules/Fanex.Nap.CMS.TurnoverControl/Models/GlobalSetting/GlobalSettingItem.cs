namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    public class GlobalSettingItem
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public bool IsLock { get; set; }

        public string Currency { get; set; }

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public decimal MaxTurnover { get; set; }
    }
}
