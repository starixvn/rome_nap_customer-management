namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;

    public class SiteGroup
    {
        public string SiteGroupName { get; set; }

        public IEnumerable<GameGroup> GameGroups { get; set; }
        
    }
}