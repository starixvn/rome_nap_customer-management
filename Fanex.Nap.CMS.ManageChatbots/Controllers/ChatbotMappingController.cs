namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Controllers;
    using Services.ChatBotMapping;
    using ViewModels.ChatbotMapping;

    public class ChatBotMappingController : BaseController
    {
        private readonly IChatBotMappingService _service;

        public ChatBotMappingController() : this(null)
        {
        }

        public ChatBotMappingController(IChatBotMappingService service)
        {
            this._service = service ?? new ChatBotMappingService();
        }

        // GET
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index()
        {
            var hasSpecialSitePermission = _service.HasSpecialSitePermission(CurrentSession.UserId);

            var siteCurrencies = new ChatBotMappingViewModel(
                _service.GetCurrencies(),
                _service.GetSites(hasSpecialSitePermission)
            );

            return View("~/Areas/MangeChatbots/Views/ChatbotMapping/Index.cshtml", siteCurrencies);
        }

        public JsonResult SearchChatBots(SearchChatBotsViewModel param)
        {
            var chatBots = _service.SearchChatBots(param);

            return JsonData(chatBots);
        }

        public JsonResult ChatBotConfig(GetConfigViewModel param)
        {
            var chatBotConfig = _service.GetConfig(param);
            
            return JsonData(chatBotConfig);
        }
    }
}
