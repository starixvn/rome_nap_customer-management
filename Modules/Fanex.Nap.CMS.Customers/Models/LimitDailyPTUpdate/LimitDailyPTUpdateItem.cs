namespace Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Constants.LimitDailyPTUpdate;

    public class LimitDailyPTUpdateItem
    {
        public string Username { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int CustID { get; set; }

        public int SiteID { get; set; }

        public string Site { get; set; }

        public int Currency { get; set; }
        public string CurrencyName { get; set; }

        public string TypeName => TypeConst.TypeList[Type];

        public byte Type { get; set; }

        public bool LimitNewAccUpdPT { get; set; }
    }
}
