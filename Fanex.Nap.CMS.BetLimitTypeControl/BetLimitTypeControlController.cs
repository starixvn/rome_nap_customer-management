namespace Fanex.NAP.CMS.BetLimitTypeControl
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.CMS.BetLimitTypeControl.Requests;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class BetLimitTypeControlController : BaseController
    {
        private readonly IBetLimitTypeControlService service;

        public BetLimitTypeControlController() : this(null)
        {
        }

        internal BetLimitTypeControlController(IBetLimitTypeControlService service)
        {
            this.service = service ?? new BetLimitTypeControlService(CurrentSession, CurrentContext);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsHost = HasRight(CMSFunctionKeys.BetLimitTypeControl, Constant.Policy.Host);
            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Read)]
        public JsonResult QueryFilter() => Json(service.QueryFilter());

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchRequest request) => JsonNet(service.Search(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Host)]
        public JsonResult Insert(InsertRequest request) => Json(service.Insert(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Host)]
        public JsonResult Update(UpdateRequest request) => Json(service.Update(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitTypeControl, Policy = Constant.Policy.Host)]
        public JsonResult Delete(DeleteRequest request) => Json(service.Delete(request));
    }
}
