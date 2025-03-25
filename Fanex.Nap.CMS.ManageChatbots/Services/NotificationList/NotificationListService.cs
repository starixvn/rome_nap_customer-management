namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Repositories.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Constants;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;
    using Fanex.Data.Repository;

    public class NotificationListService : INotificationListService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly INotificationListRepository notificationListRepository;
        private readonly IChatBotMappingService chatbotMappingservice;

        public NotificationListService() : this(null) { }

        public NotificationListService(IDynamicRepository dynamicRepository, INotificationListRepository notificationListRepository = null, IChatBotMappingService chatbotMappingservice = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.notificationListRepository = notificationListRepository ?? new NotificationListRepository();
            this.chatbotMappingservice = chatbotMappingservice ?? new ChatBotMappingService();
        }
        public NotificationListModel GetNotificationList(SearchNotificationListViewModel viewModel, int userId)
        {
            var criteia = new NotificationListCriteria
            {
                FunctionIds = viewModel.NotificationType,
                Username = viewModel.Recipient,
                FromDate = new DateTime(viewModel.FromDate.Year, viewModel.FromDate.Month, viewModel.FromDate.Day, 00, 00, 00),
                ToDate = new DateTime(viewModel.ToDate.Year, viewModel.ToDate.Month, viewModel.ToDate.Day, 23, 59, 59),
                NotificationId = viewModel.NotificationId,
                Status = viewModel.Status == "All" ? null : viewModel.Status,
                BotIDs = viewModel.BotIDs,
                IsSpecial = chatbotMappingservice.HasSpecialSitePermission(userId),
                PageIndex = viewModel.PageIndex,
                PageSize = viewModel.PageSize
            };
            return new NotificationListModel(notificationListRepository.GetNotificationList(criteia), criteia.TotalRows);
        }

        public IEnumerable<FunctionModel> GetNotifyFunctions()
        {
            var functions = dynamicRepository.Fetch<FunctionModel>(new GetAllFunctionsCriteria())
                            .Where(function => function.FunctionType == 2 
                            && !HiddenNotifyFunction.HiddenNotifyFunctions.Contains(function.FunctionId));
            return functions;
        }
    }
}
