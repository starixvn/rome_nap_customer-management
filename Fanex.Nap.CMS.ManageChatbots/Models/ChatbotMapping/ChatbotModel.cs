namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    public class ChatBotModel
    {
        public ChatBotModel(string chatBotName, string messengerUserName,string userName, string subAccount, string linkStatus, string language, string level, string currency, string status)
        {
            ChatBotName = chatBotName;
            MessengerUserName = messengerUserName;
            UserName = userName;
            SubAccount = subAccount;
            LinkStatus = linkStatus;
            Language = language;
            Level = level;
            Currency = currency;
            Status = status;
    
        }

        public string ChatBotName { get; set; }
        public string MessengerUserName { get; set; }
        public string UserName { get; set; }   
        public string SubAccount { get; set; }   
        public string LinkStatus { get; set; }   
        public string Language { get; set; }   
        public string Level { get; set; }   
        public string Currency { get; set; }   
        public string Status { get; set; }   
        
    }
}