namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IChatbotHistoryRepository
    {
        IEnumerable<ChatbotHistoryItem> GetChatbotHistories(ChatbotHistoryCriteria criteria);

        IEnumerable<FunctionModel> GetAllFunctions(ICriteria criteria);
    }
}
