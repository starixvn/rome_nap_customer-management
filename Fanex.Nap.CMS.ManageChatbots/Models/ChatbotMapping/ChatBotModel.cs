namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    public class ChatBotModel
    {
        public ChatBotModel()
        {
        }

        public ChatBotModel(string chatBotName, long chatID, string userName, string subAccount,
            string linkStatus, string language, string level, string currency, string accStatus, int cusId,
            int subAccId, int botId)
        {
            ChatBotName = chatBotName;
            ChatID = chatID;
            UserName = userName;
            SubAccount = subAccount;
            LinkStatus = linkStatus;
            Language = language;
            Level = level;
            Currency = currency;
            AccStatus = accStatus;
            CusID = cusId;
            SubAccID = subAccId;
            BotID = botId;
        }


        public string ChatBotName { get; set; }
        
        public long ChatID { get; set; }
        public string UserName { get; set; }
        public string SubAccount { get; set; }
        public string LinkStatus { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public string Currency { get; set; }
        public string AccStatus { get; set; }

        public bool Status { get; set; }

        public bool IsVerifyOTP { get; set; }

        public int CusID { get; set; }

        public int SubAccID { get; set; }

        public int BotID { get; set; }
    }
}