namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;

    public class UpdLanguageChatBotCriteria : NonQueryCommand
    {
        public int UserID { get; set; }

        public int BotID { get; set; }
        
        public string Language { get; set; }
        
        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
        
        public override string GetSettingKey()
            => "Age_Chatbot_CustBotAccountLanguageUpd";

        public override bool IsValid()
            => true;
    }
}