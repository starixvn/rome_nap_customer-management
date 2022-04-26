using Fanex.Nap.CMS.ManageChatbots.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory
{
    public class ChatbotHistoryItem
    {
        public ChatbotHistoryItem(
           string sentBy,
           string message,
           DateTime createdDate,
           byte status)
        {
            SentBy = sentBy;
            Message = message;
            CreatedDate = createdDate;
            Status = status;
        }

        public string SentBy { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateStr => CreatedDate.ToString("MM/dd/yyyy HH:mm:ss") ?? string.Empty;

        public byte Status { get; set; }

        public string StatusStr => SendStatusConst.SendStatusList[Status];

    }
}
