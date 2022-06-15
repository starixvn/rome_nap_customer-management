
namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IChatbotHistoryService
    {
        ChatbotHistoryModel GetChatbotHistories(SearchChatbotHistoryViewModel viewModel, int userId);

        IEnumerable<FunctionModel> GetChatbotHistoryFunctions();
    }
}
