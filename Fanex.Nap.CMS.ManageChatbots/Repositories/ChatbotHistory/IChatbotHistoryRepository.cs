using Fanex.Data.Repository;
using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory
{
    public interface IChatbotHistoryRepository
    {
        IEnumerable<ChatbotHistoryItem> GetChatbotHistories(ChatbotHistoryCriteria criteria);

        IEnumerable<SelectListItem> GetAllFunctions(ICriteria criteria);
    }
}
