namespace Fanex.Nap.CMS.ManageChatbots.Repositories.NotificationList
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;

    public interface INotificationListRepository
    {
        IEnumerable<NotificationListItem> GetNotificationList(NotificationListCriteria criteria);
    }
}
