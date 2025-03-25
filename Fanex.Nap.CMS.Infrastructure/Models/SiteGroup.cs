namespace Fanex.Nap.CMS.Infrastructure.Models
{
    public class SiteGroup : IdNameModel
    {
        public SiteGroup()
        {
        }

        public SiteGroup(int id, string name, bool isLicensee)
        {
            Id = id;
            Name = name;
            IsLicensee = isLicensee;
        }

        public SiteGroup(int id, string name, string siteLabel, string marketCode)
        {
            Id = id;
            Name = name;
            SiteLabel = siteLabel;
            MarketCode = marketCode;
        }

        public bool IsLicensee { get; set; }

        public string SiteLabel { get; set; }

        public string MarketCode { get; set; }
    }
}
