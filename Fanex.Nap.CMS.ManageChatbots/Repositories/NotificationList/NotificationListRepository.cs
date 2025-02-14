

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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var result = db.Query<NotificationListItem>(criteria);
                var outputParams = db.GetOutputParams();
                criteria.TotalRows = Convert.ToInt32(outputParams["totalrows"]);

                return result;
            }
        }
    }
}
