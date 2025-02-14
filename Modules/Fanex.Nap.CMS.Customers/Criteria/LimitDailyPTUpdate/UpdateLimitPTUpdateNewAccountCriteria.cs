namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;

    public class UpdateLimitPTUpdateNewAccountCriteria : CriteriaBase
    {
        public UpdateLimitPTUpdateNewAccountCriteria(byte productID, int custID, int siteID, byte currency, byte type, int userId, string ip, bool limitNewAccUpdPT)
        {
            ProductID = productID;
            CustID = custID;
            SiteID = siteID;
            Currency = currency;
            Type = type;
            UserId = userId;
            IP = ip;
            LimitNewAccUpdPT = !limitNewAccUpdPT;
        }

        public byte ProductID { get; }

        public int CustID { get; }

        public int SiteID { get; }

        public byte Currency { get; }

        public byte Type { get; }

        public int UserId { get; }

        public string IP { get; }
        public bool LimitNewAccUpdPT { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
           => "NAP_LimitPT_Upd";

        public override bool IsValid()
            => true;
    }
}
