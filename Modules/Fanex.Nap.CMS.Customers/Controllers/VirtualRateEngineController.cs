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
            HasRight(CMSFunctionKeys.VirtualRateEngine, Constant.Policy.Host);
            HasRight(CMSFunctionKeys.FullPermission, Constant.Policy.Host);
            var matrix = vrEngineService.GetVREngineMatrix();

            var viewModel = new VREngineIndexViewModel(
                HasRight(CMSFunctionKeys.VirtualRateEngine, Constant.Policy.Host),
                HasRight(CMSFunctionKeys.FullPermission, Constant.Policy.Host),
                matrix.CreditSites,
                matrix.LicenseeSites,
                matrix.CustomerClasses,
                matrix.Sports,
                matrix.BetTypes,
                matrix.LeagueGroups,
                matrix.BetfromList);

            return View("~/Areas/Customers/Views/VirtualRateEngine/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetLeagues(string leagueGroupIds, string leagues)
            => JsonData(vrEngineService.GetLeagues(leagueGroupIds, leagues));

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult GetRules()
            => JsonData(vrEngineService.GetRules());

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult CustomerClassHint()
        {
            var matrix = vrEngineService.GetVREngineMatrix();

            return View(
                "~/Areas/Customers/Views/VirtualRateEngine/CustomerClassHint.cshtml",
                matrix.CustomerClasses);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.FullPermission, Policy = Constant.Policy.Host)]
        public ActionResult AddNewRule(AddNewRuleViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(vrEngineService.AddNewRule(viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Host)]
        public ActionResult UpdateRule(int ruleId, AddNewRuleViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(vrEngineService.UpdateRule(ruleId, viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.FullPermission, Policy = Constant.Policy.Host)]
        public ActionResult DeleteRule(int ruleId)
            => JsonData(
                vrEngineService.DeleteRule(ruleId, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
    }
}
