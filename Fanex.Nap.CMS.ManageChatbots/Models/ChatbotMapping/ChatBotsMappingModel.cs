namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class ChatBotsMappingModel
    {
        public ChatBotsMappingModel()
            : this(0, Enumerable.Empty<ChatBotModel>())
        {
        }

        public ChatBotsMappingModel(
            int totalItems,
            IEnumerable<ChatBotModel> globalSettings)
        {
            TotalItems = totalItems;
            ChatBots = globalSettings;
        }

        public int TotalItems { get; }

        public IEnumerable<ChatBotModel> ChatBots { get; }
    }
}