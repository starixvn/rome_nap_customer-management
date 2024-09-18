namespace Fanex.NAP.CMS.BetLimitControl
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.CMS.BetLimitControl.Request;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class BetLimitControlController : BaseController
    {
        private readonly IService service;
        private readonly string permissions;

        public BetLimitControlController() : this(null)
        { }

        public BetLimitControlController(IService service)
        {
            this.service = service ?? new Service(CurrentSession, CurrentContext);
            var betLimitPermissions = new Dictionary<int, bool>
            {
                {1, HasRight(CMSFunctionKeys.BLC_Credit, Constant.Policy.Read)},
                {2, HasRight(CMSFunctionKeys.BLC_LicenseeSite, Constant.Policy.Read)}
            };
            permissions = string.Join(",", betLimitPermissions.Where(x => x.Value).Select(x => x.Key).ToArray());
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsHost = HasRight(CMSFunctionKeys.BetLimitControl, Constant.Policy.Host);
            var viewModel = service.GetIndexViewModel(permissions);
            return View(viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest searchRequest)
        {
            searchRequest.Permission = permissions;
            return Json(service.Search(searchRequest));
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Add(AddRequest addRequest)
        {
            addRequest.Permission = permissions;
            return Json(service.Add(addRequest));
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Update(UpdateRequest updateRequest)
        {
            updateRequest.Permission = permissions;
            return Json(service.Update(updateRequest));
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitControl, Policy = Constant.Policy.Host)]
        public JsonResult Delete(DeleteRequest deleteRequest)
        {
            deleteRequest.Permissisons = permissions;
            return Json(service.Delete(deleteRequest));
        }
    }
}