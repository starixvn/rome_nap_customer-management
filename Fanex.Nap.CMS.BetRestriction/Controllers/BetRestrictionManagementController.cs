namespace Fanex.Nap.CMS.BetRestriction.Controllers
{
    using Fanex.Nap.CMS.BetRestriction.Criterias.Management;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.Services;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Web.Mvc;

    public class BetRestrictionManagementController : BaseController
    {
        private readonly string _indexPath = "~/Areas/BetRestriction/Views/BetRestrictionManagement/Index.cshtml";

        private readonly IBetRestrictionManagementService _betRestrictionManagementService;

        public BetRestrictionManagementController() : this(null)
        {
        }

        public BetRestrictionManagementController(IBetRestrictionManagementService betRestrictionManagementService)
        {
            _betRestrictionManagementService = betRestrictionManagementService ?? new BetRestrictionManagementService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.OnOffBetRestriction, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View(_indexPath);

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.OnOffBetRestriction, Policy = Constant.Policy.Read)]
        public JsonResult GetAllSettings() => Json(_betRestrictionManagementService.GetAllSettings(new GetAllSettingCriteria()));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.OnOffBetRestriction, Policy = Constant.Policy.Host)]
        public JsonResult EditSettings(EditSettingsRequest request) => Json(_betRestrictionManagementService.ExecuteNonQuery(new EditSettingsCriteria
        {
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP,
            SettingXML = _betRestrictionManagementService.ParseEditSettingsXML(request)
        }));
    }
}