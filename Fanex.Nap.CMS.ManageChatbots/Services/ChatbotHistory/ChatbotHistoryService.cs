using System;
using System.Collections.Generic;
using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;
using System.Linq;
using Fanex.Nap.CMS.ManageChatbots.Constants;
using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory
{
    public class ChatbotHistoryService : IChatbotHistoryService
    {
        private readonly IChatBotMappingService chatbotMappingservice;
        private readonly IDynamicRepository dynamicRepository;

        public ChatbotHistoryService(): this(null) { }

        public ChatbotHistoryService(IDynamicRepository dynamicRepository, IChatBotMappingService chatbotMappingservice = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.chatbotMappingservice = chatbotMappingservice ?? new ChatBotMappingService();
        }
        public ChatbotHistoryModel GetChatbotHistories(SearchChatbotHistoryViewModel viewModel, int userId)
        {
            var criteria = new ChatbotHistoryCriteria(
                viewModel.Username,
                viewModel.ChatID,
                viewModel.FunctionID,
                viewModel.BotIDs,
                new DateTime(viewModel.FromDate.Year, viewModel.FromDate.Month, viewModel.FromDate.Day, 00, 00, 00),
                new DateTime(viewModel.ToDate.Year, viewModel.ToDate.Month, viewModel.ToDate.Day, 23, 59, 59),
                viewModel.Status,
                chatbotMappingservice.HasSpecialSitePermission(userId),
                viewModel.PageIndex,
                viewModel.PageSize);
            var chatbotHistories = dynamicRepository.Fetch<ChatbotHistoryItem>(criteria);

            return new ChatbotHistoryModel(chatbotHistories, criteria.TotalRows);
        }

        public IEnumerable<FunctionModel> GetChatbotHistoryFunctions()
        {
            var functions = new List<FunctionModel>
            {
                new FunctionModel { FunctionId = 0, FunctionName = "All" }
            };

            const int CommonFunctions = 3;
            functions.AddRange(dynamicRepository.Fetch<FunctionModel>(new GetAllFunctionsCriteria())
                .Where(function => function.FunctionType == 1 || function.FunctionType == CommonFunctions || HiddenNotifyFunction.HiddenNotifyFunctions.Contains(function.FunctionId)));

            return functions;
        }
    }  
}
