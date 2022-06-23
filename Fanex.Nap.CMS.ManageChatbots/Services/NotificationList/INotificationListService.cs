namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;

    public interface INotificationListService
    {
        NotificationListModel GetNotificationList(SearchNotificationListViewModel viewModel);

        IEnumerable<FunctionModel> GetNotifyFunctions();
    }
}
