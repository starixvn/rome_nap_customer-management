using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory
{
    public class ChatbotHistoryModel
    {
        public ChatbotHistoryModel(
            string sendBy,
            string messageContent,
            string sendStatus,
            DateTime date) 
        {
            SendBy = sendBy;
            MessageContent = messageContent;
            SendStatus = sendStatus;
            Date = date; 
        }

        public string SendBy { get; }

        public string MessageContent { get; }

        public string SendStatus { get; }

        public DateTime Date { get; }
    }
}
