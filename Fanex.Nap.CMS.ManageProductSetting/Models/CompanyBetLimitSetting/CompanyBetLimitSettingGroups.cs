namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using System.Linq;

    public class CompanyBetLimitSettingGroups
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public IEnumerable<BetLimitCurrencyGroup> CurrencyGroups { get; set; }

        public int NumberRow => CurrencyGroups.Select(e => e.NumberRow).Sum();
        
    }
}