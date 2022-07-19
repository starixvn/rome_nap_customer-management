namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class GetCurrencyListCriteria : CriteriaBase
    {
        public byte Mode { get; set; }
        
        public override string GetSettingKey()
            => "Acc_ExchangeSel";
    }
}