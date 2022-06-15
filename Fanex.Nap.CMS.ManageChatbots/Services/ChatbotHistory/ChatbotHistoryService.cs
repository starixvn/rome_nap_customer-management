namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;
    using System.Linq;

    public class ChatbotHistoryService : IChatbotHistoryService
    {
        private readonly IChatbotHistoryRepository repository;
        private readonly IChatBotMappingService chatbotMappingservice;

        public ChatbotHistoryService(IChatbotHistoryRepository repository = null, IChatBotMappingService chatbotMappingservice = null)
        {
            this.repository = repository ?? new ChatbotHistoryRepository();
            this.chatbotMappingservice = chatbotMappingservice ?? new ChatBotMappingService();
        }
        public ChatbotHistoryModel GetChatbotHistories(SearchChatbotHistoryViewModel viewModel, int userId)
        {
            var criteria = new ChatbotHistoryCriteria(
                viewModel.Username,
                viewModel.ChatID,
                viewModel.FunctionID,
                new DateTime(viewModel.FromDate.Year, viewModel.FromDate.Month, viewModel.FromDate.Day, 00, 00, 00),
                new DateTime(viewModel.ToDate.Year, viewModel.ToDate.Month, viewModel.ToDate.Day, 23, 59, 59),
                viewModel.Status,
                chatbotMappingservice.HasSpecialSitePermission(userId),
                viewModel.PageIndex,
                viewModel.PageSize);
            var chatbotHistories = this.repository.GetChatbotHistories(criteria);

            return new ChatbotHistoryModel(chatbotHistories, criteria.TotalRows);
        }

        public IEnumerable<FunctionModel> GetChatbotHistoryFunctions()
        {
            var functions = new List<FunctionModel>();
            functions.Add(new FunctionModel { FunctionId = 0, FunctionName = "All" });
            functions.AddRange(repository.GetAllFunctions(new GetAllFunctionsCriteria())
                .Where(function => function.FunctionType == 1 || function.FunctionType == 3));
           return functions;
        }
    }  
}
