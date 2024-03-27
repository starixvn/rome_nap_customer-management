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

    public class PolicyController : SportbookBaseController
    {
        private readonly IFilterService filterService;
        private readonly IPolicyService policyService;
        private readonly IRuleService ruleService;

        public PolicyController() : this(new FilterService(), new PolicyService(), new RuleService())
        {
        }

        internal PolicyController(IFilterService filterService, IPolicyService policyService, IRuleService ruleService)
        {
            this.filterService = filterService;
            this.policyService = policyService;
            this.ruleService = ruleService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            return View(
                "~/Areas/CustomerClass/Views/Policy/Index.cshtml",
                new PolicyViewModel(havePermissionPortTypes, HasRight(CMSFunctionKeys.PolicyManagement, Constant.Policy.Host)));
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPolicies(string policyTitle, short sportType)
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read).Where(sp => sportType == 0 || sp.Id == sportType);
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(policyService.GetPolicies(havePermissionPortTypes, policyTitle), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult Get(int policyId)
        {

            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(policyService.GetPolicy(havePermissionPortTypes, policyId), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetRules(short sportType)
        {

            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }
            
            return JsonData(ruleService.GetRules(sportType), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddPolicyRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.AddPolicy(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int policyId, AddPolicyRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return policyId > 0 && request.IsValid()
                ? JsonData(policyService.EditPolicy(policyId, request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult CheckPolicies(DeletePolicyRequest request)
        {
            if (!HavePermissionOnSportType(request.SportTypes, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.CheckPolicies(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult DeletePolicies(DeletePolicyRequest request)
        {
            if (!HavePermissionOnSportType(request.SportTypes, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.DeletePolicies(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }
    }
}
