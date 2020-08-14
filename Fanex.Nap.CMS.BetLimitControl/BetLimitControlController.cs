namespace Fanex.NAP.CMS.BetLimitControl
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.CMS.BetLimitControl.Request;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

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
            ViewBag.IsHost = HasRight(CMSFunctionKeys.BetLimitControl, Constant.Policy.Host);
            var viewModel = service.GetIndexViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest searchRequest) => Json(service.Search(searchRequest));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Add(AddRequest addRequest) => Json(service.Add(addRequest));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Update(UpdateRequest updateRequest) => Json(service.Update(updateRequest));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Delete(DeleteRequest deleteRequest) => Json(service.Delete(deleteRequest));
    }
}