namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using System.Linq;

    public class CurrencyGroup
    {
        public string Currency { get; set; }

        public int CurrencyGroupID { get; set; }

        public IEnumerable<SiteGroup> SiteGroups { get; set; }

        public int NumberRow => SiteGroups.Select(e=>e.NumberRow).Sum();
    }
}