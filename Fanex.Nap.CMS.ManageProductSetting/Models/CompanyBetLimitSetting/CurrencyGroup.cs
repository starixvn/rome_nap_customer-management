namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;

    public class CurrencyGroup
    {
        public string Currency { get; set; }
        
        public int CurrencyGroupID { get; set; }

        public IEnumerable<GameGroup> GameGroups { get; set; }
        
    }
}