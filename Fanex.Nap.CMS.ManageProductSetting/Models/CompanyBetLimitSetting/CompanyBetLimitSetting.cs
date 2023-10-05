namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    using System.Collections.Generic;

    public class CompanyBetLimitSetting
    {
        public int SportType { get; set; }
        public string SportName { get; set; }
        public int CurrencyGroupID { get; set; }
        public string Currency { get; set; }
        public string GameGroup { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
        public string Remark { get; set; }
    }


    public class GameGroup
    {
        public string GameGroupName { get; set; }

        public IEnumerable<CompanyBetLimitSetting> Entries { get; set; }
        
    }

    public class CurrencyGroup
    {
        public string Currency { get; set; }
        public int CurrencyGroupID { get; set; }

        public IEnumerable<GameGroup> GameGroups { get; set; }
        
    }

    public class CompanyBetLimitSettingGroups
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public IEnumerable<CurrencyGroup> CurrencyGroups { get; set; }
        
    }
}