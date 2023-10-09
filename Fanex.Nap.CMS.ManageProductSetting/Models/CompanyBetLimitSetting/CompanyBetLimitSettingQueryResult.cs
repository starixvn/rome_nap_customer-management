namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{    
    using System.Collections.Generic;
    public class CompanyBetLimitSettingQueryResult
    {
        public IEnumerable<CompanyBetLimitSetting> Entries { get; set; }
        
        public int TotalRows { get; set; }
    }
}