using System.Collections.Generic;

namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    public class CurrencyGroup
    {
        public CurrencyGroup() { }

        public CurrencyGroup(int groupId, IEnumerable<CurrencyGroupItem> currencies)
        {
            Currencies = currencies;
            CurrencyGroupID = groupId;
            var exchangeIds = new List<int>();
            var curNames = new List<string>();
            var curCodes = new List<string>();
            foreach (var cur in currencies)
            {
                exchangeIds.Add(cur.ExchangeID);
                curNames.Add(cur.Currency);
                curCodes.Add(cur.Code);
            }

            ExchangeIDs = string.Join(",", exchangeIds);
            CurrencyNames = string.Join(", ", curNames);
            Codes = string.Join(", ", curCodes);
        }

        public string ExchangeIDs { get; set; }

        public string CurrencyNames { get; set; }

        public string Codes { get; set; }

        IEnumerable<CurrencyGroupItem> Currencies { get; set; }

        public int CurrencyGroupID { get; set; }
    }
}