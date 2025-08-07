namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Requests.Nonsports;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Web.Mvc;

    public class NonsportsController : BaseController
    {
        private readonly IFilterService filterService;
        private readonly INonsportsService service;

        public NonsportsController() : this(new FilterService(), new NonsportsService())
        {
        }

        internal NonsportsController(IFilterService filterService, INonsportsService service)
        {
            this.filterService = filterService;
            this.service = service;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new NonsportsViewModel(
                   HasRight(CMSFunctionKeys.NonsportsCCManagement, Constant.Policy.Host),
                   filterService.GetCustomerClasses(),
                   filterService.GetAllSites(),
                   filterService.GetNonsportsProducts());

            return View("~/Areas/CustomerClass/Views/Nonsports/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchNonsportsRequest request)
            => JsonData(service.GetOfferings(request, filterService.GetNonsportsProducts()), JsonRequestBehavior.AllowGet);

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddEditNonsportsRequest request)
            => JsonData(service.AddOffer(request, CurrentSession.UserId, CurrentContext.IP));

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(AddEditNonsportsRequest request)
            => JsonData(service.UpdateOffer(request, filterService.GetNonsportsProducts(), CurrentSession.UserId, CurrentContext.IP));

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(int policyId)
            => JsonData(service.DeleteOffer(policyId, filterService.GetNonsportsProducts(), CurrentSession.UserId, CurrentContext.IP));

        [NapAuth(FunctionKey = CMSFunctionKeys.NonsportsCCManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(EnableOfferingRequest request)
            => JsonData(service.EnableOffer(request, CurrentSession.UserId, CurrentContext.IP));

    }
}
