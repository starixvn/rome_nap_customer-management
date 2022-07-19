

namespace Fanex.Nap.CMS.ManageChatbots.Repositories.NotificationList
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;

    public class NotificationListRepository : INotificationListRepository
    {
        public IEnumerable<NotificationListItem> GetNotificationList(NotificationListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = db.Query<NotificationListItem>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                criteria.TotalRows = Convert.ToInt32(outputParams["totalrows"]);

                return result;
            }
        }
    }
}
