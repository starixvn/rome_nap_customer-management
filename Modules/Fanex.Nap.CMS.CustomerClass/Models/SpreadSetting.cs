namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SpreadSetting
    {
        public SpreadSetting(SpreadSettingExcelValidModel model)
        {
            PolicyTitle = model.PolicyTitle;
            CustomerClass = model.CustomerClass;
            SportType = model.SportType;
            SiteID = model.Site;
            Currency = model.Currency;
            Platform = model.Platform;
            Spread = model.Spread;
        }

        public string PolicyTitle { get; set; }
        public short CustomerClass { get; set; }
        public int SportType { get; set; }
        public int SiteID { get; set; }
        public int Currency { get; set; }
        public int Platform { get; set; }
        public decimal Spread { get; set; }
        //public string LeagueGroups { get; set; }
        //public byte IsLive { get; set; }
    }
}
