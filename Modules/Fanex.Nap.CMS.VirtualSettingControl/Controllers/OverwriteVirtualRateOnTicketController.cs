using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure.Controllers;

namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    public class OverwriteVirtualRateOnTicketController : BaseController
    {
        //[NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/OverwriteVirtualRateOnTicket/Index.cshtml");
        }
    }
}
