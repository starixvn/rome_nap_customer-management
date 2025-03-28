namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    public class GlobalSettingItem
    {
        public GlobalSettingItem(
            string username,
            string level,
            bool isLock,
            string currency,
            int profileId,
            string name,
            decimal maxTurnover)
            : this(0, username, level, isLock, currency, profileId, name, maxTurnover)
        {
        }

        public GlobalSettingItem(
            int custId,
            string username,
            string level,
            bool isLock,
            string currency,
            int profileId,
            string profileName,
            decimal maxTurnover)
        {
            CustId = custId;
            Username = username;
            Level = level;
            IsLock = isLock;
            Currency = currency;
            ProfileId = profileId;
            ProfileName = profileName;
            MaxTurnover = maxTurnover;
        }

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
