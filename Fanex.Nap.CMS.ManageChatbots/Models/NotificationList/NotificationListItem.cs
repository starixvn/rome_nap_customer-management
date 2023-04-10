namespace Fanex.Nap.CMS.ManageChatbots.Models.NotificationList
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class NotificationListItem
    {
#pragma warning disable S107
        public NotificationListItem(
            long notificationId,
            string functionName,
            string botName,
            string message,
            string recipient,
            DateTime sendDate,
            string status,
            string description,
            byte retry)
#pragma warning restore S107
        {
            NotificationId = notificationId;
            FunctionName = functionName;
            BotName = botName;
            Message = message;
            Recipient = recipient;
            SendDate = sendDate;
            Status = status;
            Description = description;
            Retry = retry;
        }

        public long NotificationId { get; set; }

        public string FunctionName { get; set; }

        public string BotName { get; set; }

        public DateTime SendDate { get; set; }

        public string SendDateStr => DateTimeExtensions.ToLongDate24hWithSlash(SendDate) ?? string.Empty;

        public string Recipient { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public byte Retry { get; set; }
    }
}
