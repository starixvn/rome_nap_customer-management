namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory
{
    using System;
    using Fanex.Nap.CMS.ManageChatbots.Constants;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

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

        public string CreatedDateStr => DateTimeExtensions.ToLongDate24hWithSlash(CreatedDate) ?? string.Empty;

        public byte Status { get; set; }

        public string StatusStr => SendStatusConst.SendStatusList[Status];
    }
}
