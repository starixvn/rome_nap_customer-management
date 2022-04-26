using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Nap.CMS.ManageChatbots.Constants;
using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;

namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    public class ChatbotHistoryController : BaseController
    {
        private readonly IChatbotHistoryService chatbotHistoryservice;
        private readonly IChatBotMappingService chatbotMappingservice;

        public ChatbotHistoryController() : this(null)
        {

        }

        internal ChatbotHistoryController(IChatbotHistoryService servive)
        {
            this.chatbotHistoryservice = chatbotHistoryservice ?? new ChatbotHistoryService();
            this.chatbotMappingservice = chatbotMappingservice ?? new ChatBotMappingService();
        }

        public ActionResult Index()
        {
            var viewModel = new ChatbotHistoryViewModel(chatbotHistoryservice.GetAllFunctions());
            return View("~/Areas/MangeChatbots/Views/ChatbotHistory/Index.cshtml", viewModel);
        }

        public ActionResult Search(SearchChatbotHistoryViewModel viewModel)
        {
            viewModel.IsSpecial = chatbotMappingservice.HasSpecialSitePermission(CurrentSession.UserId);
            return JsonData(chatbotHistoryservice.GetChatbotHistories(viewModel));
        }
    }
}
