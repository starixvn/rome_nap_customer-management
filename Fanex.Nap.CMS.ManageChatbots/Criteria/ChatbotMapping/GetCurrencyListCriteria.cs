namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;

    public class GetCurrencyListCriteria : CriteriaBase
    {
        public byte Mode { get; set; }
        
        public override string GetSettingKey()
            => "Acc_ExchangeSel";

        public override bool IsValid()
            => true;
    }
}