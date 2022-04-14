using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory
{
    public class ChatbotHistoryService : IChatbotHistoryService
    {
        private readonly IChatbotHistoryRepository repository;

        public ChatbotHistoryService(IChatbotHistoryRepository repository = null)
        {
            this.repository = repository ?? new ChatbotHistoryRepository();
        }
        public IEnumerable<ChatbotHistoryModel> GetChatbotHistories(SearchChatbotHistoryViewModel viewModel)
        {
            var criteria = new ChatbotHistoryCriteria(
                viewModel.Username,
                viewModel.MessengerUsername,
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.Function,
                viewModel.SendStatus);
            var result = this.repository.GetChatbotHistories(criteria);

            return result;
        }

        public IEnumerable<SelectListItem> GetAllFunctions()
        {
           return repository.GetAllFunctions(new GetAllFunctionsCriteria());
        }
    }
              
}
