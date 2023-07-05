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

    public class SpreadController : BaseController
    {
        private readonly IFilterService filterService;
        private readonly ISpreadService spreadService;

        public SpreadController() : this(new FilterService(), new SpreadService())
        {
        }

        internal SpreadController(IFilterService filterService, ISpreadService spreadService)
        {
            this.filterService = filterService;
            this.spreadService = spreadService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new SpreadViewModel(
                   HasRight(CMSFunctionKeys.CustomerClassManagement, Constant.Policy.Host),
                   filterService.GetCustomerClasses(),
                   filterService.GetAllSites(),
                   filterService.GetAllCurrencies());

            return View("~/Areas/CustomerClass/Views/Spread/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchSpreadRuleRequest request)
            => JsonData(spreadService.GetSpreadRules(request), JsonRequestBehavior.AllowGet);

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddEditSpreadRuleRequest request)
            => JsonData(spreadService.AddRule(request, CurrentSession.UserId, CurrentContext.IP));


        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(AddEditSpreadRuleRequest request)
            => JsonData(spreadService.UpdateRule(request, CurrentSession.UserId, CurrentContext.IP));


        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(int policyID)
            => JsonData(spreadService.DeleteRule(policyID, CurrentSession.UserId, CurrentContext.IP));

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(int policyID, bool enable)
            => JsonData(spreadService.EnableRule(policyID, enable, CurrentSession.UserId, CurrentContext.IP));
    }
}
