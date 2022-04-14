using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    public class ChatbotHistoryController : BaseController
    {
        private readonly IChatbotHistoryService service;

        public ChatbotHistoryController() : this(null)
        {

        }

        internal ChatbotHistoryController(IChatbotHistoryService servive)
        {
            this.service = service ?? new ChatbotHistoryService();
        }

        public ActionResult Index()
        {
            var viewModel = new ChatbotHistoryViewModel(service.GetAllFunctions());
            return View("~/Areas/MangeChatbots/Views/ChatbotHistory/Index.cshtml", viewModel);
        }

        public ActionResult Search(SearchChatbotHistoryViewModel viewModel)
            => JsonData(service.GetChatbotHistories(viewModel));
    }
}
