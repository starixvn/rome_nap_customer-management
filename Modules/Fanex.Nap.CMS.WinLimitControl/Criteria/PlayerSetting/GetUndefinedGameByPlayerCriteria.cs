namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;

    public class GetUndefinedGameByPlayerCriteria : CriteriaBase
    {
        public string CustName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool ExcludeTestData
        {
            get
            {
                return NapSettings.Settings.Environment.Equals("LIVE", StringComparison.OrdinalIgnoreCase);
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustNotConfigProductSel";
        }
    }
}
