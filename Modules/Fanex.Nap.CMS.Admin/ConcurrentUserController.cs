namespace Fanex.Nap.CMS.Admin.Controllers
{
    using Fanex.Nap.CMS.Admin.Services.ConcurrentUser;
    using Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Web.Mvc;

    public class ConcurrentUserController : BaseController
    {
        private readonly IConcurrentUserService service;

        public ConcurrentUserController()
            : this(null)
        {
        }

        public ConcurrentUserController(IConcurrentUserService service)
        {
            this.service = service ?? new ConcurrentUserService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ConcurrentUserReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = service.GetDataLatest();
            return View("~/Areas/Admin/Views/ConcurrentUser/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ConcurrentUserReport, Policy = Constant.Policy.Read)]
        public ActionResult History()
            => View("~/Areas/Admin/Views/ConcurrentUser/History.cshtml", new ConcurrentUserHistoryViewModel(DateTime.UtcNow));

        [NapAuth(FunctionKey = CMSFunctionKeys.ConcurrentUserReport, Policy = Constant.Policy.Read)]
        public ActionResult HistoryByDaily()
            => View("~/Areas/Admin/Views/ConcurrentUser/HistoryByDaily.cshtml", new ConcurrentUserHistoryViewModel(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow));

        [NapAuth(FunctionKey = CMSFunctionKeys.ConcurrentUserReport, Policy = Constant.Policy.Read)]
        public ActionResult GetHistory(DateTime date)
        {
            var viewModel = service.GetDataHistory(date);

            return JsonData(viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ConcurrentUserReport, Policy = Constant.Policy.Read)]
        public ActionResult GetHistoryDaily(DateTime fromDate, DateTime toDate)
        {
            var viewModel = service.GetDataHistory(fromDate, toDate);

            return JsonData(viewModel);
        }
        public ActionResult RequestForm()
            => View("~/Areas/Admin/Views/ConcurrentUser/popup.cshtml");
    }
}
