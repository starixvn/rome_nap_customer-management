namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddNewLimitDailyPTUpdateCriteria : CommandBaseCriteria 
    {
        public AddNewLimitDailyPTUpdateCriteria(string productId, string smaName, byte currency, int siteId, byte type, int userId, string ip, bool limitNewAccUpdPT)
        {
            ProductID = productId;
            SMAName = smaName;
            Currency = currency;
            SiteID = siteId;
            Type = type;
            UserId = userId;
            IP = ip;
            LimitNewAccUpdPT= limitNewAccUpdPT;
        }

        public string ProductID { get; }

        public string SMAName { get; }

        public byte Currency { get; }

        public int SiteID { get; }

        public byte Type { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public int UserId { get; }

        public string IP { get; }

        public bool LimitNewAccUpdPT { get; set; }

        public override string GetSettingKey()
            => "NAP_LimitPTDaily_Ins";

        public override bool IsValid()
            => true;
    }
}
