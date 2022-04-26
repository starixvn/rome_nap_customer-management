using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory
{
    public interface IChatbotHistoryService
    {
        ChatbotHistoryModel GetChatbotHistories(SearchChatbotHistoryViewModel viewModel);

        IEnumerable<SelectListItem> GetAllFunctions();
    }
}
