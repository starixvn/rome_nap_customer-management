namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Linq;

    public class ChatBotsConfigModel
    {
        public IEnumerable<ChatBotFunctionConfig> FunctionConfigs { get; set; }

        public ChatBotModel ChatBotModel { get; set; }
    }
}