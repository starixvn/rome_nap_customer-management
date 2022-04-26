namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory
{
    using System.Collections.Generic;

    public class ChatbotHistoryModel
    {
        public ChatbotHistoryModel(IEnumerable<ChatbotHistoryItem> chatbotHistories, int totalRows)
        {
            ChatbotHistories = chatbotHistories;
            TotalRows = totalRows;
        }

        public IEnumerable<ChatbotHistoryItem> ChatbotHistories { get; }

        public int TotalRows { get; set; }
    }
}
