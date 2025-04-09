namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ChatBotsConfigModel
    {
        public IEnumerable<ChatBotFunctionConfig> FunctionConfigs { get; set; }

        public ChatBotConfigModelItem ChatBotModel { get; set; }

        public List<SelectListItem> OptionsLanguage { get; set; }
    }

    public class ChatBotConfigModelItem : ChatBotModel
    {
        public bool Status { get; set; }
    }
}