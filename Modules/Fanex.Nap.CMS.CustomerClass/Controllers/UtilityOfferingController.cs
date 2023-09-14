namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class UtilityOfferingController : BaseController
    {
        private readonly IFilterService filterService;
        private readonly IUtilityOfferingSerivce offeringSerivce;

        public UtilityOfferingController() : this(new FilterService(), new UtilityOfferingSerivce())
        {
        }

        internal UtilityOfferingController(IFilterService filterService, IUtilityOfferingSerivce offeringSerivce)
        {
            this.filterService = filterService;
            this.offeringSerivce = offeringSerivce;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CCUtilityOffering, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new UtilityOfferingViewModel(
                   HasRight(CMSFunctionKeys.CCUtilityOffering, Constant.Policy.Host),
                   filterService.GetCustomerClasses(),
                   filterService.GetAllSites(),
                   filterService.GetAllCurrencies());

            return View("~/Areas/CustomerClass/Views/UtilityOffering/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CCUtilityOffering, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchUtilityOfferingRequest request)
            => JsonData(offeringSerivce.GetOfferings(request), JsonRequestBehavior.AllowGet);
    }
}
