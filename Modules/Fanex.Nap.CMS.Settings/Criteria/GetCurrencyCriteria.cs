namespace Fanex.Nap.CMS.Settings.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;

    public class GetCurrencyCriteria : CriteriaBase
    {
        public bool ExcludeTestData
        {
            get
            {
                return NapSettings.Settings.Environment.Equals("LIVE", StringComparison.OrdinalIgnoreCase);
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySel";
        }
    }
}
