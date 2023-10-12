namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using System.Linq;
    public class SiteGroup
    {
        public string SiteGroupName { get; set; }

        public IEnumerable<GameGroup> GameGroups { get; set; }

        public int NumberRow => GameGroups.Select(e=>e.NumberRow).Sum();
    }
}