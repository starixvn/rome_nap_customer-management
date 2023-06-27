namespace Fanex.Nap.CMS.Customers.Controllers
{
    using Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Net;
    using System.Web.Mvc;

    public class LimitDailyPTUpdateController : BaseController
    {
        private readonly ILimitDailyPTUpdateService service;

        public LimitDailyPTUpdateController() : this(null)
        {
        }

        internal LimitDailyPTUpdateController(ILimitDailyPTUpdateService service)
        {
            this.service = service ?? new LimitDailyPTUpdateService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LimitDailyPTUpdate, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new LimitDailyPTUpdateViewModel(
                service.GetSites(), 
                service.GetProducts(), 
                service.GetCurrencies(), 
                HasRight(CMSFunctionKeys.LimitDailyPTUpdate, Constant.Policy.Host)
                );
            return View("~/Areas/Customers/Views/LimitDailyPTUpdate/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LimitDailyPTUpdate, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchDailyPTUpdateViewModel viewModel)
            => JsonData(service.GetLimitReport(viewModel));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LimitDailyPTUpdate, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(AddNewLimitDailyPTUpdateViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.AddNewLimit(viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LimitDailyPTUpdate, Policy = Constant.Policy.Host)]
        public ActionResult Remove(RemoveLimitDailyPTUpdateViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.RemoveLimit(viewModel), JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LimitDailyPTUpdate, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateLimitPTUpdateNewAccountViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.UpdateLimit(viewModel), JsonRequestBehavior.DenyGet);
        }
    }
}
