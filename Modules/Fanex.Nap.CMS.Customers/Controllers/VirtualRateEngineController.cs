namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Services.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class VirtualRateEngineController : BaseController
    {
        private readonly IVREngineService vrEngineService;

        public VirtualRateEngineController() : this(new VREngineService())
        {
        }

        internal VirtualRateEngineController(IVREngineService vrEngineService)
        {
            this.vrEngineService = vrEngineService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var adminPermission = HasRight(CMSFunctionKeys.VirtualRateEngineAdminPermission, Constant.Policy.Host);
            var userPermission = HasRight(CMSFunctionKeys.VirtualRateEngineUserPermission, Constant.Policy.Host);

            ViewBag.AdminPermission = adminPermission;
            ViewBag.UserPermission = userPermission;
            ViewBag.CanUpdateRule = adminPermission || userPermission;

            return View("~/Areas/Customers/Views/VirtualRateEngine/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetLeagues(string leagueGroupIds, string leagueGroupListNotIn, string leagues)
            => JsonData(vrEngineService.GetLeagues(leagueGroupIds, leagueGroupListNotIn, leagues));

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetRules()
        {
            var model = vrEngineService.GetRules();

            return JsonData(model.Rules);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetRule(int ruleId)
            => JsonData(vrEngineService.GetRule(ruleId));

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetRulesAndMatrix()
        {
            var model = vrEngineService.GetRules();

            return JsonData(new { model.Rules, Matrix = new VREngineMatrixViewModel(model.Matrix) });
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngineAdminPermission, Policy = Constant.Policy.Host)]
        public ActionResult AddNewRule(AddNewRuleViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(vrEngineService.AddNewRule(viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult UpdateRule(int ruleId, AddNewRuleViewModel viewModel)
        {
            if (HasRight(CMSFunctionKeys.VirtualRateEngineAdminPermission, Constant.Policy.Host)
                || HasRight(CMSFunctionKeys.VirtualRateEngineUserPermission, Constant.Policy.Host))
            {
                viewModel.UserId = CurrentSession.UserId;
                viewModel.IP = CurrentContext.IP;
                return JsonData(vrEngineService.UpdateRule(ruleId, viewModel), JsonRequestBehavior.DenyGet);
            }

            return Forbidden();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngineAdminPermission, Policy = Constant.Policy.Host)]
        public ActionResult DeleteRule(int ruleId)
            => JsonData(
                vrEngineService.DeleteRule(ruleId, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
    }
}
