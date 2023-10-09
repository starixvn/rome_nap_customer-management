namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    public class GameGroup
    {
        public string GameGroupName { get; set; }

        public IEnumerable<CompanyBetLimitSetting> Entries { get; set; }
        
    }
}