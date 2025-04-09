namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class RemoveLimitDailyPTUpdateCriteria : CommandBaseCriteria
    {
        public RemoveLimitDailyPTUpdateCriteria(byte productID, int custID, int siteID, byte currency, byte type, int userId, string ip)
        {
            ProductID = productID;
            CustID = custID;
            SiteID = siteID;
            Currency = currency;
            Type = type;
            UserId = userId;
            IP = ip;
        }

        public byte ProductID { get; }

        public int CustID { get; }

        public int SiteID { get; }

        public byte Currency { get; }

        public byte Type { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
           => "NAP_LimitPTDaily_Del";

        public override bool IsValid()
            => true;
    }
}
