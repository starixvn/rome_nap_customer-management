namespace Fanex.NAP.CMS.BetLimitControl
{
    using Fanex.Nap.CMS.BetLimitControl.Request;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class BetLimitControlController : BaseController
    {
        private readonly IService service;

        public BetLimitControlController() : this(null)
        { }

        public BetLimitControlController(IService service)
        {
            this.service = service ?? new Service(CurrentSession, CurrentContext);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsHost = HasRight(CMSFunctionKeys.BetLimitTypeControl, Constant.Policy.Host);
            var viewModel = service.GetIndexViewModel();
            return View(viewModel);
        }
        
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest searchRequest) => Json(service.Search(searchRequest));
    }
}