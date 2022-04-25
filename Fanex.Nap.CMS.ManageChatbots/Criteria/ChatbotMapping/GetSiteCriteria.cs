namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;
    public class GetSiteCriteria : CriteriaBase
    {
        public int Mode { get; set; }
        
        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";
    }
}