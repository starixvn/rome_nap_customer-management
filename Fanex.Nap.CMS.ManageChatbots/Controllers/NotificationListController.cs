namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping;
    using Fanex.Nap.CMS.ManageChatbots.Services.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;

    public class NotificationListController : BaseController
    {
        private readonly INotificationListService notificationListService;
        private readonly IChatBotMappingService chatbotMappingService;

        public NotificationListController() : this(null)
        {
        }

        internal NotificationListController(INotificationListService servive, IChatBotMappingService botMappingService = null)
        {
            this.notificationListService = servive ?? new NotificationListService();
            this.chatbotMappingService = botMappingService ?? new ChatBotMappingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NotificationList, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var functions = new SelectList(notificationListService.GetNotifyFunctions(), "FunctionId", "FunctionName");
            var bots = chatbotMappingService.GetChatBotNames();
            var viewModel = new NotificationListViewModel();
            viewModel.NotificationType = functions;
            viewModel.Bots = new SelectList(bots, "Id", "Name");
            return View("~/Areas/MangeChatbots/Views/NotificationList/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NotificationList, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchNotificationListViewModel viewModel)
        {
            return JsonData(notificationListService.GetNotificationList(viewModel, CurrentSession.UserId));
        }
    }
}
