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

    public class RuleController : BaseController
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

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new RuleViewModel(
                HasRight(CMSFunctionKeys.CustomerClassManagement, Constant.Policy.Host),
                filterService.GetCustomerClasses(),
                filterService.GetLeagueGroups());
            return View("~/Areas/CustomerClass/Views/Rule/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchRuleRequest request)
            => JsonData(ruleService.GetRules(request));

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetRules(short sportType)
            => JsonData(ruleService.GetRules(sportType));

        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Read)]
        public ActionResult Get(int ruleId)
            => JsonData(ruleService.GetRule(ruleId));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddRuleRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.AddRule(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int ruleId, AddRuleRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.EditRule(ruleId, request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CustomerClassManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(EnableRuleRequest request)
        {
            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            return request.IsValid()
                ? JsonData(ruleService.EnableRule(request), JsonRequestBehavior.DenyGet)
                : BadRequest();
        }
    }
}
