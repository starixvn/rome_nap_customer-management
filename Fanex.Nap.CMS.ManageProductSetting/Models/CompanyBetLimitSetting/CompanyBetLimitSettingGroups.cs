namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    public class CompanyBetLimitSettingGroups
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public IEnumerable<CurrencyGroup> CurrencyGroups { get; set; }
        
    }
}