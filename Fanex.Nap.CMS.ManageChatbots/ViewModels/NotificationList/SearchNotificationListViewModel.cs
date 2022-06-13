using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList
{
    public class SearchNotificationListViewModel
    {
        public string Recipient { get; set; }

        public int NotificationType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
