namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class GetConfigChatBotCriteria : CriteriaBase
    {
        public int CustID { get; set; }
        
        public int BotID { get; set; }
        
        public int SubAccID { get; set; }
        
        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotAccountAllInfoSel";
    }
}