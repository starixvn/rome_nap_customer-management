namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Core.Business;

    public class ChatbotHistoryController : BaseController
    {
        private readonly IChatbotHistoryService chatbotHistoryservice;

        public ChatbotHistoryController() : this(null)
        {
        }

        internal ChatbotHistoryController(IChatbotHistoryService servive)
        {
            this.chatbotHistoryservice = servive ?? new ChatbotHistoryService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotHistory, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var functions = new SelectList(chatbotHistoryservice.GetChatbotHistoryFunctions(), "FunctionId", "FunctionName");
            var viewModel = new ChatbotHistoryViewModel(functions);
            return View("~/Areas/MangeChatbots/Views/ChatbotHistory/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotHistory, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchChatbotHistoryViewModel viewModel)
        {
            return JsonData(chatbotHistoryservice.GetChatbotHistories(viewModel, CurrentSession.UserId));
        }
    }
}
