namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;
    public class GetSiteCriteria : CriteriaBase
    {
        public byte Mode { get; set; }
        
        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";

        public override bool IsValid()
            => true;
    }
}