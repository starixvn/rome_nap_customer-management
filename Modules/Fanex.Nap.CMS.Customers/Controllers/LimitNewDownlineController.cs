using System.Net;
using System.Web.Mvc;
using Fanex.Nap.CMS.Customers.Services.LimitNewDownline;
using Fanex.Nap.CMS.Customers.ViewModels.LimitNewDownline;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Nap.CMS.Customers.Controllers
{
    public class LimitNewDownlineController: BaseController
    {
        private readonly ILimitNewDownlineService service;

        public LimitNewDownlineController()
            : this(null)
        {
        }

        internal LimitNewDownlineController(ILimitNewDownlineService service)
        {
            this.service = service ?? new LimitNewDownlineService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LimitNewDownlinePerDay, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new LimitNewDownlineViewModel(
                service.GetSites(),
                HasRight(CMSFunctionKeys.LimitNewDownlinePerDay, Constant.Policy.Host));

            return View("~/Areas/Customers/Views/LimitNewDownline/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LimitNewDownlinePerDay, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchLimitDownlineViewModel viewModel)
            => JsonData(service.GetLimitDownlineReport(
                viewModel.Username,
                viewModel.Level,
                viewModel.Site,
                viewModel.PageIndex,
                viewModel.PageSize));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LimitNewDownlinePerDay, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(string usernames)
        {
            var result = string.IsNullOrWhiteSpace(usernames)
                ? new Result((int)HttpStatusCode.BadRequest, Labels.BadRequest)
                : service.AddNewLimit(usernames, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LimitNewDownlinePerDay, Policy = Constant.Policy.Host)]
        public ActionResult Remove(string usernames)
        {
            var result = string.IsNullOrWhiteSpace(usernames)
                ? new Result((int)HttpStatusCode.BadRequest, Labels.BadRequest)
                : service.RemoveLimit(usernames, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
