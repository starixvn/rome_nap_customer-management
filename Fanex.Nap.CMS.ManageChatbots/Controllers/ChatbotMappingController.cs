namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotMapping;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping;
    using Fanex.NAP.Core.Business;

    public class ChatbotMappingController : BaseController
    {
        private readonly IChatbotMappingService chatbotMappingService;

        public ChatbotMappingController()
            : this(null)
        {
        }

        public ChatbotMappingController(IChatbotMappingService chatbotMappingService)
        {
            this.chatbotMappingService = chatbotMappingService ?? new ChatbotMappingService();
        }

        // GET
        public ActionResult Index()
        {
            var viewModel = new ChatbotMappingViewModel(
                chatbotMappingService.GetCurrencies()
            );
            return View("~/Areas/MangeChatbots/Views/ChatbotMapping/Index.cshtml", viewModel);
        }

        public JsonResult GetChatBot(GetListChatbotMappingViewModel viewModel)
        {
            /*var model = !viewModel.IsValid() || !HasReadGlobalSetting(viewModel.SportType)
                ? new GlobalSettingModel()*/
            var model = chatbotMappingService.GetList(
                viewModel.Username,
                viewModel.SportType,
                viewModel.ExchangeId,
                viewModel.SortField,
                viewModel.SortDirection,
                viewModel.Page,
                viewModel.Size,
                viewModel.Cache);

            return JsonData(model);
        }


        public JsonResult GetCurrencies()
            => JsonData(chatbotMappingService.GetCurrencies());

        private bool HasReadChatbotMapping(short sporttype)
            => HasPermission(sporttype, Constant.Policy.Read);

        private bool HasHostChatbotMapping(short sporttype)
            => HasPermission(sporttype, Constant.Policy.Host);

        private bool HasPermission(short sporttype, Constant.Policy policy)
        {
            //Todo config permission
            var permissionKey = "ChatbotMapping";


            return HasRight(permissionKey, policy);
        }
    }
}