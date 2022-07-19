namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList
{
    using System;

    public class SearchNotificationListViewModel
    {
        public string Recipient { get; set; }

        public string NotificationType { get; set; }

        public long? NotificationId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Status { get; set; }

        public string BotIDs { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
