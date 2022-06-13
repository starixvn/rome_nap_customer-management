
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Models.NotificationList
{
    using System;
    using Fanex.Nap.CMS.ManageChatbots.Constants;

    public class NotificationListItem
    {
        public NotificationListItem(
            long notificationId,
            int notificationType,
            DateTime sendDate,
            string recipient,
            string titleContent,
            byte status
            )
        {
            NotificationId = notificationId;
            NotificationType = notificationType;
            SendDate = sendDate;
            Recipient = recipient;
            TitleContent = titleContent;
            Status = status;
        }

        public long NotificationId { get; set; }

        public int NotificationType { get; set; }

        public DateTime SendDate { get; set; }

        public string Recipient { get; set; }

        public string TitleContent { get; set; }

        public byte Status { get; set; }

        public string StatusStr => NotificationStatus.NotificationStatusList[Status];
    }
}
