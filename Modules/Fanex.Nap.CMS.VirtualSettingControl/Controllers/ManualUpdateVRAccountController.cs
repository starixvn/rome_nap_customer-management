namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Services.ManualUpdateVRAccount;

    public class ManualUpdateVRAccountController : BaseController
    {
        private readonly IManualUpdateVRAccountService manualUpdateService;

        public ManualUpdateVRAccountController()
            : this(null)
        {
        }

        public ManualUpdateVRAccountController(IManualUpdateVRAccountService manualUpdateService)
        {
            this.manualUpdateService = manualUpdateService ?? new ManualUpdateVRAccountService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_ManuallyUpdatedVirtualRateAccounts, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.SportType = SportTypeConstants.ESports;
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ESports_ManuallyUpdatedVirtualRateAccounts, Constant.Policy.Host);

            return View("~/Areas/VirtualSettingControl/Views/ManualUpdateVRAccount/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_ManuallyUpdatedVirtualRateAccounts, Policy = Constant.Policy.Read)]
        public ActionResult Search(int sportType, string account)
            => JsonData(manualUpdateService.GetManualUpdateAccounts(sportType, account));

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_ManuallyUpdatedVirtualRateAccounts, Policy = Constant.Policy.Host)]
        public ActionResult Unmark(int sportType, string custIds)
            => JsonData(
                manualUpdateService.UnmarkManualUpdateAccount(sportType, custIds, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
    }
}
