namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;
    public class CurrencyListCriteria: CriteriaBase
    {
        public CurrencyListCriteria(int exchangeId = 0)
        {
            ExchangeId = exchangeId;
        }

        public int ExchangeId { get; }
    //ToDo: change GetSettingKey
        public override string GetSettingKey()
            => "NAP_TOControl_CurrencySelect";
    }
}