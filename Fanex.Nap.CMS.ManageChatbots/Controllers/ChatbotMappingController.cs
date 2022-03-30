using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.ManageChatbots.Services.ChatbotMapping;

namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
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
            return View("~/Areas/MangeChatbots/Views/ChatbotMapping/Index.cshtml");
        }
    }
}