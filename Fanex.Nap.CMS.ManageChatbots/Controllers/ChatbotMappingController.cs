using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    public class ChatbotMappingController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View("~/Areas/MangeChatbots/Views/ChatbotMapping/Index.cshtml");
        }
    }
}