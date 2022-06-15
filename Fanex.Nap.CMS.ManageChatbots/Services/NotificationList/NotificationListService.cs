namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Repositories.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;

    public class NotificationListService : INotificationListService
    {
        private readonly INotificationListRepository notificationListRepository;
        private readonly IChatbotHistoryRepository chatbotHistoryRepository;

        public NotificationListService(INotificationListRepository notificationListRepository = null,
            IChatbotHistoryRepository historyRepository = null)
        {
            this.notificationListRepository = notificationListRepository ?? new NotificationListRepository();
            this.chatbotHistoryRepository = historyRepository ?? new ChatbotHistoryRepository();
        }
        public NotificationListModel GetNotificationList(SearchNotificationListViewModel viewModel, int userId)
        {
            var criteia = new NotificationListCriteria
            {
                FunctionIds = viewModel.NotificationType,
                Username = viewModel.Recipient,
                FromDate = viewModel.FromDate,
                ToDate = viewModel.ToDate,
                NotificationId = viewModel.NotificationId,
                Status = viewModel.Status == "All" ? null : viewModel.Status,
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };
            return new NotificationListModel(notificationListRepository.GetNotificationList(criteia), criteia.TotalRows);
        }

        public IEnumerable<FunctionModel> GetNotifyFunctions()
        {
            var functions = chatbotHistoryRepository.GetAllFunctions(new GetAllFunctionsCriteria()).Where(function => function.FunctionType == 2);
            return functions;
        }
    }
}
