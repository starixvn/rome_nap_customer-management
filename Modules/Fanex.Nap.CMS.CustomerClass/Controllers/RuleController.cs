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

    public class RuleController : SportbookBaseController
    {
        private readonly IFilterService filterService;
        private readonly IRuleService ruleService;

        public RuleController() : this(new FilterService(), new RuleService())
        {
        }

        internal RuleController(IFilterService filterService, IRuleService ruleService)
        {
            this.filterService = filterService;
            this.ruleService = ruleService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var viewModel = new RuleViewModel(
                HasRight(CMSFunctionKeys.RuleManagement, Constant.Policy.Host),
                filterService.GetCustomerClasses(),
                havePermissionPortTypes,
                filterService.GetLeagueGroups());
            return View("~/Areas/CustomerClass/Views/Rule/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(ruleService.GetRules(request), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult Get(int ruleId)
            => JsonData(ruleService.GetRule(ruleId));
        
        [NapAuth(FunctionKey = CMSFunctionKeys.SportsbookCCManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetLeagueBySport(int sportType)
        {
            return JsonData(filterService.GetLeagueGroups(sportType), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.AddRule(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int ruleId, AddRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.EditRule(ruleId, request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(EnableRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.EnableRule(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult CheckRules(DeleteRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportTypes, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.CheckRuleDel(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult DeleteRules(DeleteRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportTypes, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.DeleteRules(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }
    }
}
