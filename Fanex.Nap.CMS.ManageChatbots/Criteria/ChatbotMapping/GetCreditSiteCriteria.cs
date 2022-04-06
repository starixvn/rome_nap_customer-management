namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;
    public class GetCreditSiteCriteria : CriteriaBase
    {
        public GetCreditSiteCriteria(bool isSpecial)
        {
            this.isSpecial = isSpecial;
        }

        public bool isSpecial { get; }

        public override string GetSettingKey()
            => "NAP_Chatbot_SiteSel";
    }
}