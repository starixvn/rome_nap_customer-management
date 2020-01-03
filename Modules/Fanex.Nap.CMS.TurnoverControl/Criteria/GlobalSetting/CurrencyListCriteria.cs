using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class CurrencyListCriteria : CriteriaBase    {        public CurrencyListCriteria(int exchangeId = 0)        {            ExchangeId = exchangeId;        }
        public int ExchangeId { get; }
        public override string GetSettingKey()            => "NAP_TOControl_CurrencySelect";    }
}
