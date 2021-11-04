namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;

    public class RemoveLimitDailyPTUpdateCriteria : CriteriaBase
    {
        public RemoveLimitDailyPTUpdateCriteria(byte productID, int custID, byte siteID, byte currency, byte type, int userId, string ip)
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

        public byte SiteID { get; }

        public byte Currency { get; }

        public byte Type { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
           => "NAP_LimitPTDaily_Del";
    }
}
