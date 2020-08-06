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

        public int CustId { get; }

        public string Username { get; }

        public string Level { get; }

        public bool IsLock { get; }

        public string Currency { get; }

        public int ProfileId { get; }

        public string ProfileName { get; }

        public decimal MaxTurnover { get; }
    }
}
