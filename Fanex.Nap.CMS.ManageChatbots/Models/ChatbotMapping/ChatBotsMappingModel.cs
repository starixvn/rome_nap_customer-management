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
            IEnumerable<ChatBotModel> chatBotModels)
        {
            TotalItems = totalItems;
            ChatBots = chatBotModels;
        }

        public int TotalItems { get; }

        public IEnumerable<ChatBotModel> ChatBots { get; }
    }
}