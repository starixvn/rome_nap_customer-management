namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using System;
    using System.Collections;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;

    public class GetPlayerWinLimitCriteria : GetCurrencyWinLimitCriteria
    {
        public string Username { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRow { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingSel";
        }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable
            {
                { "username", Username },
                { "exchangeid", Currency },
                { "productid", Game },
                { "pageindex", PageIndex },
                { "pagesize", PageSize },
                { "sortorder", 1 },
                { "totalrow", TotalRow }
            };

            if (SortColumnName.Equals("colUsername", StringComparison.OrdinalIgnoreCase))
            {
                if (SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    inputParams["sortorder"] = 2;
                }
            }

            return inputParams;
        }
    }
}
