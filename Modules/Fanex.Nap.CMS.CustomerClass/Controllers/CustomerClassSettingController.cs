namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

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
                havePermissionPortTypes,
                filterService.GetLeagueGroups());

            return View("~/Areas/CustomerClass/Views/CustomerClassSetting/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchCustomerClassSettingRequest request)
        {
            if(request.SportType == 0)
            {
                var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
                if (!havePermissionPortTypes.Any())
                {
                    return Forbidden(behavior: JsonRequestBehavior.AllowGet);
                }

                return JsonData(ruleService.GetRules(request, havePermissionPortTypes), JsonRequestBehavior.AllowGet);
            } else
            {
                if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
                {
                    return Forbidden(behavior: JsonRequestBehavior.AllowGet);
                }

                return JsonData(ruleService.GetRules(request), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
