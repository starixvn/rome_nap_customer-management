namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;

    public class GetCurrencyWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        [SpParam(SqlParamName = "ExchangeId")]
        public int Currency { get; set; }

        public bool ExcludeTestData
        {
            get 
            {
                return NapSettings.Settings.Environment.Equals("LIVE", StringComparison.OrdinalIgnoreCase);
            }
        }

        [SpParam(Ignored = true)]
        public string SortColumnName { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingSel";
        }
    }
}
