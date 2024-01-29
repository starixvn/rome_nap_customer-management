namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class CustomerClassSettingController : BaseController
    {
        private readonly IFilterService filterService;
        private readonly IRuleService ruleService;

        public CustomerClassSettingController()
            : this(new FilterService(), new RuleService())
        {
        }

        internal CustomerClassSettingController(
            IFilterService filterService,
            IRuleService ruleService)
        {
            this.filterService = filterService;
            this.ruleService = ruleService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new CustomerClassSettingViewModel(
                filterService.GetAllSites(),
                filterService.GetCustomerClasses(),
                filterService.GetSports(),
                filterService.GetLeagueGroups());

            return View("~/Areas/CustomerClass/Views/CustomerClassSetting/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchCustomerClassSettingRequest request)
            => JsonData(ruleService.GetRules(request));
    }
}
