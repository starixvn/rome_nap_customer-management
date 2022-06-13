using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    public class NotificationListService : INotificationListService
    {
        public NotificationListModel GetNotificationList(SearchNotificationListViewModel viewModel, int userId)
        {
            var items = new[] {
                new NotificationListItem(1, 1, DateTime.Today, "abc", "content", 1),
                new NotificationListItem(1, 1, DateTime.Today, "abc", "content", 1),
                new NotificationListItem(1, 1, DateTime.Today, "abc", "content", 1)
            };
            var result = new NotificationListModel(items, 3);
            return result;
        }
    }
}
