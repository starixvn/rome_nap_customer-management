namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;

    public class GetConfigChatBotCriteria : CriteriaBase
    {
        public int CustID { get; set; }
        
        public int BotID { get; set; }
        
        public int SubAccID { get; set; }
        
        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotAccountAllInfoSel";

        public override bool IsValid()
            => true;
    }
}