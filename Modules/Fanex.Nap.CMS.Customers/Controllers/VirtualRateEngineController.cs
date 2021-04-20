namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class VirtualRateEngineController : BaseController
    {
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Customers/Views/VirtualRateEngine/Index.cshtml");
        }
    }
}
