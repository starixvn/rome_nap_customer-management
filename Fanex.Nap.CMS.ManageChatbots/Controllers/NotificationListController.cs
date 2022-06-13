namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Services.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;

    public class NotificationListController : BaseController
    {
        private readonly INotificationListService notificationListService;
        private readonly IChatbotHistoryService chatbotHistoryService;

        public NotificationListController() : this(null)
        {
        }

        internal NotificationListController(INotificationListService servive)
        {
            this.notificationListService = notificationListService ?? new NotificationListService();
            this.chatbotHistoryService = chatbotHistoryService ?? new ChatbotHistoryService();
        }
        
        public ActionResult Index()
        {
            var functions = new SelectList(chatbotHistoryService.GetAllFunctions(), "Id", "Name");
            var viewModel = new NotificationListViewModel();
            viewModel.NotificationType = functions;
            return View("~/Areas/MangeChatbots/Views/NotificationList/Index.cshtml", viewModel);
        }

        public ActionResult Search(SearchNotificationListViewModel viewModel)
        {
            return JsonData(notificationListService.GetNotificationList(viewModel, CurrentSession.UserId));
        }
    }
}
