using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure.Controllers;

namespace Fanex.Nap.CMS.BetLimitControl
{
    public class BetLimitControlController : BaseController
    {
        [HttpGet]
        public ActionResult Index() => View();
    }
}