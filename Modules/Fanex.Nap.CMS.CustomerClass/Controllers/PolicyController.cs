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

    public class PolicyController : BaseController
    {
        private readonly IPolicyService policyService;

        public PolicyController() : this(new PolicyService())
        {
        }

        internal PolicyController(IPolicyService policyService)
            => this.policyService = policyService;

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View(
                "~/Areas/CustomerClass/Views/Policy/Index.cshtml",
                new PolicyViewModel(HasRight(CMSFunctionKeys.PolicyManagement, Constant.Policy.Host)));

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPolicies()
            => JsonData(policyService.GetPolicies());

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPolicyTitle(short sportType)
            => JsonData(policyService.GetPolicyTitles(sportType));

        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Read)]
        public ActionResult Get(int policyId)
            => JsonData(policyService.GetPolicy(policyId));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddPolicyRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.AddPolicy(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PolicyManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int policyId, AddPolicyRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return policyId > 0 && request.IsValid()
                ? JsonData(policyService.EditPolicy(policyId, request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult CheckPolicies(DeletePolicyRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.CheckPolicies(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.RuleManagement, Policy = Constant.Policy.Host)]
        public ActionResult DeletePolicies(DeletePolicyRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(policyService.DeletePolicies(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }
    }
}
