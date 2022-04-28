namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public class ChatBotsConfigModel
    {
        public IEnumerable<ChatBotFunctionConfig> FunctionConfigs { get; set; }

        public ChatBotModel ChatBotModel { get; set; }
        
        public List<SelectListItem> OptionsLanguage { get; set; } 
    }
}