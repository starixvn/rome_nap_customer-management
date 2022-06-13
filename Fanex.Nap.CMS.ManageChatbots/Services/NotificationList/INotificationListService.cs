using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;

    public interface INotificationListService
    {
        NotificationListModel GetNotificationList(SearchNotificationListViewModel viewModel, int userId);
    }
}
