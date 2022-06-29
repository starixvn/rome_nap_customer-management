namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.Services.NotificationList;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.NotificationList;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class NotificationListController : BaseController
    {
        private readonly INotificationListService notificationListService;

        public NotificationListController() : this(null)
        {
        }

        internal NotificationListController(INotificationListService servive)
        {
            this.notificationListService = servive ?? new NotificationListService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NotificationList, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var functions = new SelectList(notificationListService.GetNotifyFunctions(), "FunctionId", "FunctionName");
            var viewModel = new NotificationListViewModel();
            viewModel.NotificationType = functions;
            return View("~/Areas/MangeChatbots/Views/NotificationList/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NotificationList, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchNotificationListViewModel viewModel)
        {
            return JsonData(notificationListService.GetNotificationList(viewModel, CurrentSession.UserId));
        }
    }
}
