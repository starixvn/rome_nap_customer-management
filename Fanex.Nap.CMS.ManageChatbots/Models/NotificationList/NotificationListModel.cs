using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Models.NotificationList
{
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
