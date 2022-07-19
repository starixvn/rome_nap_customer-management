namespace Fanex.Nap.CMS.ManageChatbots.Models.NotificationList
{
    using System.Collections.Generic;

    public class NotificationListModel
    {
        public NotificationListModel(IEnumerable<NotificationListItem> notificationLists, int totalRows)
        {
            NotificationLists = notificationLists;
            TotalRows = totalRows;
        }
        public IEnumerable<NotificationListItem> NotificationLists { get; }

        public int TotalRows { get; set; }
    }
}
