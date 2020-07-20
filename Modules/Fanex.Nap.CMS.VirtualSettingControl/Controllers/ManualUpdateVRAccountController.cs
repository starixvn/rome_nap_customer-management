namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.ManualUpdateVRAccount;
    using Fanex.NAP.CMS.Core.Resources;
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
            => IndexView(SportTypeConstants.ESports);

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_ManuallyUpdatedVRAccounts, Policy = Constant.Policy.Read)]
        public ActionResult Basketball()
            => IndexView(SportTypeConstants.Basketball);

        public ActionResult Search(int sportType, string account)
        {
            var manualUpdateAccounts = HasRead(sportType)
                ? manualUpdateService.GetManualUpdateAccounts(sportType, account)
                : Enumerable.Empty<ManualUpdateAccountDTO>();

            return JsonData(manualUpdateAccounts);
        }

        public void Export(int sportType, string account)
        {
            var data = HasRead(sportType)
                ? manualUpdateService.GetManualUpdateAccounts(sportType, account)
                : Enumerable.Empty<ManualUpdateAccountDTO>();

            IExcelExporter exporter = new ManualUpdateVRAccountExporter(data, sportType);
            exporter.ResponseStream();
        }

        public ActionResult Unmark(int sportType, string custIds)
        {
            var result = HasHost(sportType)
                ? manualUpdateService.UnmarkManualUpdateAccount(sportType, custIds, CurrentSession.UserId, CurrentContext.IP)
                : new Result((int)HttpStatusCode.Forbidden, Messages.AccessDenied);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private ActionResult IndexView(int sportType)
        {
            ViewBag.SportType = sportType;
            ViewBag.SportTypeName = CacheRef.GetSportName(sportType);
            ViewBag.HostPermission = HasHost(sportType);

            return View("~/Areas/VirtualSettingControl/Views/ManualUpdateVRAccount/Index.cshtml");
        }

        private bool HasRead(int sportType)
            => HasPermission(sportType, Constant.Policy.Read);

        private bool HasHost(int sportType)
            => HasPermission(sportType, Constant.Policy.Host);

        private bool HasPermission(int sportType, Constant.Policy policy)
            => HasRight(MapSportTypeToFunctionKey(sportType), policy);

        private static string MapSportTypeToFunctionKey(int sportType)
        {
            if (sportType == SportTypeConstants.ESports)
            {
                return CMSFunctionKeys.ESports_ManuallyUpdatedVirtualRateAccounts;
            }

            if (sportType == SportTypeConstants.Basketball)
            {
                return CMSFunctionKeys.Basketball_ManuallyUpdatedVRAccounts;
            }

            return null;
        }
    }
}
