namespace Fanex.Nap.CMS.ManageChatbots.Services.NotificationList
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.Constants;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

    public class NotificationListService : ServiceBase, INotificationListService
    {
        private readonly IChatBotMappingService chatbotMappingservice;

        public NotificationListService() : this(null) { }

        public NotificationListService(IChatBotMappingService chatbotMappingservice = null) : base()
        {
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
            var notificationList = DynamicRepository.Fetch<NotificationListItem>(criteia);

            return new NotificationListModel(notificationList, criteia.TotalRows);
        }

        public IEnumerable<FunctionModel> GetNotifyFunctions()
        {
            var functions = DynamicRepository.Fetch<FunctionModel>(new GetAllFunctionsCriteria())
                            .Where(function => function.FunctionType == 2 
                            && !HiddenNotifyFunction.HiddenNotifyFunctions.Contains(function.FunctionId));

            return functions;
        }
    }
}
