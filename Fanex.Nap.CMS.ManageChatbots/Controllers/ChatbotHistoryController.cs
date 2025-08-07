namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory;
    using Fanex.NAP.Client.Attributes;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;

    public class ChatbotHistoryController : BaseController
    {
        private readonly IChatbotHistoryService chatbotHistoryservice;
        private readonly IChatBotMappingService chatBotMappingService;

        public ChatbotHistoryController() : this(null)
        {
        }

        internal ChatbotHistoryController(IChatbotHistoryService servive, IChatBotMappingService botMappingService = null)
        {
            this.chatbotHistoryservice = servive ?? new ChatbotHistoryService();
            this.chatBotMappingService = botMappingService ?? new ChatBotMappingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotHistory, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var functions = new SelectList(chatbotHistoryservice.GetChatbotHistoryFunctions(), "FunctionId", "FunctionName");
            var bots = chatBotMappingService.GetChatBotNames();
            var viewModel = new ChatbotHistoryViewModel(functions)
            {
                Bots = new SelectList(bots, "Id", "Name")
            };
            return View("~/Areas/MangeChatbots/Views/ChatbotHistory/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotHistory, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchChatbotHistoryViewModel viewModel)
        {
            return JsonData(chatbotHistoryservice.GetChatbotHistories(viewModel, CurrentSession.UserId));
        }
    }
}
