namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class UnlinkChatBotCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int BotID { get; set; }

        public bool Status { get; set; }

        public bool IsVerifyOTP { get; set; }
        
        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotAccountInfoUpd";
    }
}