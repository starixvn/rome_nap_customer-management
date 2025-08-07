namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;

    public class CustomerClassSettingController : SportbookBaseController
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
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if(!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var viewModel = new CustomerClassSettingViewModel(
                filterService.GetAllSites(),
                filterService.GetCustomerClasses(),
                filterService.GetAllCurrencies(),
                havePermissionPortTypes,
                filterService.GetLeagueGroups());

            return View("~/Areas/CustomerClass/Views/CustomerClassSetting/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchCustomerClassSettingRequest request)
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read).Where(sp => request.SportTypes.Split(',').Contains(sp.Id.ToString()));
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(ruleService.GetRules(request, havePermissionPortTypes), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassSetting, Policy = Constant.Policy.Read)]
        public ActionResult RuleDetail(GetRuleDetailRequest request)
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read).Where(sp => request.SportTypes.Split(',').Contains(sp.Id.ToString()));
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(ruleService.GetRuleDetail(request, havePermissionPortTypes), JsonRequestBehavior.AllowGet);
        }
    }
}
