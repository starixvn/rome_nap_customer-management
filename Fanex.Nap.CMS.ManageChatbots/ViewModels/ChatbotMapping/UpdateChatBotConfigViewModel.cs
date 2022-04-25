namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
    using System;
    public class UpdateChatBotConfigViewModel
    {
        public int UserID { get; set; }
        public ChatBotFunctionConfig[] ChatBotFunctionConfig { get; set; }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}