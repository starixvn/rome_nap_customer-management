namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
    
    public class UpdateChatBotConfigViewModel
    {
        public int UserID { get; set; }
        
        public int BotId { get; set; }
        
        public ChatBotFunctionConfig[] ChatBotFunctionConfig { get; set; }

        public string Language { get; set; }
        
    }
}