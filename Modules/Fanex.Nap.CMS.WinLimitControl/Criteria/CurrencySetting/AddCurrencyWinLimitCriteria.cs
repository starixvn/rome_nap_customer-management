namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using System;
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class AddCurrencyWinLimitCriteria : CriteriaBase
    {
        public WinLimitEntity WinLimit { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingIns";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "ExchangeID", WinLimit.Currency.Id },
                { "ProductID", WinLimit.Game.Id },
                { "MaxWinLimit", Math.Round(WinLimit.MaxWinLimit) },
                { "UserId", UserId },
                { "IP", IP },
                { "ErrCode", ErrorCode },
                { "ErrMsg", ErrorMessage }
            };
        }
    }
}
