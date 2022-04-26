using Fanex.Nap.CMS.ManageChatbots.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory
{
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
