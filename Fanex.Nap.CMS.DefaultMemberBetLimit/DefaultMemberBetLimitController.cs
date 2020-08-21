namespace Fanex.Nap.CMS.DefaultMemberBetLimit
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.CMS.DefaultMemberBetLimit.Requests;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class DefaultMemberBetLimitController : BaseController
    {
        private readonly IService service;

        public DefaultMemberBetLimitController() : this(null)
        { }

        public DefaultMemberBetLimitController(IService service)
        {
            this.service = service ?? new Service(CurrentSession, CurrentContext);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.DefaultMemberBetLimit, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsHost = HasRight(CMSFunctionKeys.DefaultMemberBetLimit, Constant.Policy.Host);
            var viewModel = service.GetIndexViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DefaultMemberBetLimit, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchRequest request) => JsonNet(service.Search(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DefaultMemberBetLimit, Policy = Constant.Policy.Host)]
        public JsonResult Insert(InsertRequest request) => Json(service.Insert(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DefaultMemberBetLimit, Policy = Constant.Policy.Host)]
        public JsonResult Update(UpdateRequest request) => Json(service.Update(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DefaultMemberBetLimit, Policy = Constant.Policy.Host)]
        public JsonResult Delete(DeleteRequest request) => Json(service.Delete(request));
    }
}
