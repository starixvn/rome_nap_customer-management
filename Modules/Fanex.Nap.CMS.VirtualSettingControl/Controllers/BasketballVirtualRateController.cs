namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class BasketballVirtualRateController : BaseController
    {
        private const string ViewPath = "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/{0}.cshtml";

        private readonly IVirtualRateDetailsService service;

        public BasketballVirtualRateController()
            : this(null)
        {
        }

        public BasketballVirtualRateController(IVirtualRateDetailsService service)
        {
            this.service = service ?? new VirtualRateDetailsService();
        }

        #region Virtual Rate Detail

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult VirtualRateDetails()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.Basketball_VirtualRateDetails, Constant.Policy.Host);

            return View(string.Format(ViewPath, nameof(VirtualRateDetails)));
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult GetVirtualRateDetails(string username)
        {
            var report = service.GetVirtualRateDetails(new GetVirtualRateDetailsCriteria
            {
                Username = username,
                SportType = SportTypeConstants.Basketball
            });

            if (report.ResultCode == 0)
            {
                var viewName = report.ReportLevel == RoleIdConstants.Member ? "_MemberVirtualRateDetails" : "_SMAVirtualRateDetails";

                return PartialView(string.Format(ViewPath, viewName), report);
            }

            return Content(null);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public void ExportVirtualRateDetails(string username)
        {
            var report = service.GetVirtualRateDetails(new GetVirtualRateDetailsCriteria
            {
                Username = username,
                SportType = SportTypeConstants.Basketball
            });
            var exporter = new VirtualRateDetailsExporter(report, SportTypeConstants.Basketball);
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Host)]
        public ActionResult SaveVirtualRateDetails(
            int custId,
            bool isMember,
            bool isMemberCompactMode,
            bool isManualUpdate,
            VirtualRateDetailsReport rates)
        {
            var result = service.SaveVirtualRateDetails(new SaveVirtualRateDetailsCriteria
            {
                CustId = custId,
                SportType = SportTypeConstants.Basketball,
                IsMember = isMember,
                IsMemberCompactMode = isMemberCompactMode,
                IsManualUpdate = isManualUpdate,
                Rates = rates,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Host)]
        public JsonResult ValidateAssociatedAccounts(string accounts)
        {
            var result = service.ValidateAssociatedAccounts(new ValidateAssociatedAccountsCriteria
            {
                Accounts = accounts ?? string.Empty
            });

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Host)]
        public JsonResult SaveAssociatedAccounts(
            string accounts,
            bool isMember,
            bool isMemberCompactMode,
            bool isManualUpdate,
            VirtualRateDetailsReport rates)
        {
            var result = service.SaveAssociatedAccounts(new SaveAssociatedAccountsCriteria
            {
                Accounts = accounts,
                SportType = SportTypeConstants.Basketball,
                IsMember = isMember,
                IsMemberCompactMode = isMemberCompactMode,
                IsManualUpdate = isManualUpdate,
                Rates = rates,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult ViewRealValues(int custId)
        {
            var virtualRateRealValues = service.ViewRealValues(new ViewRealValuesCriteria
            {
                CustId = custId,
                SportType = SportTypeConstants.Basketball
            });

            return PartialView(string.Format(ViewPath, "_ViewRealValues"), virtualRateRealValues);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public void ExportRealValues(int custId)
        {
            var report = service.ViewRealValues(new ViewRealValuesCriteria
            {
                CustId = custId,
                SportType = SportTypeConstants.Basketball
            });
            var exporter = new VirtualRateRealValuesExporter(report, SportTypeConstants.Basketball);
            exporter.ResponseStream();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult BetTypeVirtualRateMapping()
        {
            ViewBag.ShowMenus = false;
            var betTypeVRMapping = service.GetBetTypeVirtualRateMapping(SportTypeConstants.Basketball);

            return View(string.Format(ViewPath, nameof(BetTypeVirtualRateMapping)), betTypeVRMapping);
        }

        #endregion

        #region Global Settings

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails_GlobalSetting, Policy = Constant.Policy.Read)]
        public ActionResult GlobalSettings()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.Basketball_VirtualRateDetails_GlobalSetting, Constant.Policy.Host);
            var report = service.GetVirtualRateDetails(new GetVirtualRateDetailsCriteria
            {
                IsGlobalSetting = true,
                SportType = SportTypeConstants.Basketball
            });

            return View(string.Format(ViewPath, nameof(GlobalSettings)), report);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails_GlobalSetting, Policy = Constant.Policy.Read)]
        public void ExportGlobalSettings()
        {
            var report = service.GetVirtualRateDetails(new GetVirtualRateDetailsCriteria
            {
                IsGlobalSetting = true,
                SportType = SportTypeConstants.Basketball
            });
            var exporter = new VirtualRateDetailsExporter(report, SportTypeConstants.Basketball);
            exporter.ResponseStream();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateDetails_GlobalSetting, Policy = Constant.Policy.Host)]
        public ActionResult SaveGlobalSettings(VirtualRateDetailsReport rates)
        {
            var result = service.SaveVirtualRateDetails(new SaveVirtualRateDetailsCriteria
            {
                IsGlobalSetting = true,
                SportType = SportTypeConstants.Basketball,
                Rates = rates,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region View Logs

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateLog, Policy = Constant.Policy.Read)]
        public ActionResult VirtualRateLogs()
            => View(string.Format(ViewPath, nameof(VirtualRateLogs)));

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_VirtualRateLog, Policy = Constant.Policy.Read)]
        public ActionResult GetVirtualRateLogs(string username, bool includeUpline, DateTime fromDate, DateTime toDate)
        {
            var report = service.GetLogs(new GetVirtualRateLogsCriteria
            {
                SportType = SportTypeConstants.Basketball,
                Username = username,
                IncludeUpline = includeUpline,
                FromDate = fromDate,
                ToDate = toDate
            });

            if (report.ResultCode == 0)
            {
                return PartialView(string.Format(ViewPath, "_VirtualRateLogs"), report);
            }

            return Content(null);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_AutoSuspendLog, Policy = Constant.Policy.Read)]
        public ActionResult AutoSuspendLogs()
            => View(string.Format(ViewPath, nameof(AutoSuspendLogs)));

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_AutoSuspendLog, Policy = Constant.Policy.Read)]
        public ActionResult GetAutoSuspendLogs(string username, DateTime fromDate, DateTime toDate)
        {
            var report = service.GetAutoSuspendLogs(new GetAutoSuspendLogsCriteria
            {
                SportType = SportTypeConstants.Basketball,
                Username = username,
                FromDate = fromDate,
                ToDate = toDate
            });

            if (report.ResultCode == 0)
            {
                return PartialView(string.Format(ViewPath, "_AutoSuspendLogsReport"), report);
            }

            return Content(null);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_GlobalAutoSuspendThreshold, Policy = Constant.Policy.Read)]
        public ActionResult GlobalAutoSuspendThresholdLogs()
            => View(string.Format(ViewPath, nameof(GlobalAutoSuspendThresholdLogs)));

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_AutoSuspendLog, Policy = Constant.Policy.Read)]
        public ActionResult GetGlobalAutoSuspendThresholdLogs(DateTime fromDate, DateTime toDate)
        {
            var logs = service.GetGlobalAutoSuspendThresholdLogs(SportTypeConstants.Basketball, fromDate, toDate);

            return PartialView(string.Format(ViewPath, "_GlobalAutoSuspendThresholdLogsReport"), logs);
        }

        #endregion

        #region Global Auto Suspend Threshold

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_GlobalAutoSuspendThreshold, Policy = Constant.Policy.Read)]
        public ActionResult GlobalAutoSuspendThreshold()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.Basketball_GlobalAutoSuspendThreshold, Constant.Policy.Host);
            var globalAutoSuspendThreshold = service.GetGlobalAutoSuspendThreshold(SportTypeConstants.Basketball);

            return View(string.Format(ViewPath, nameof(GlobalAutoSuspendThreshold)), globalAutoSuspendThreshold);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_GlobalAutoSuspendThreshold, Policy = Constant.Policy.Read)]
        public void ExportGlobalAutoSuspendThreshold()
        {
            var globalAutoSuspendThreshold = service.GetGlobalAutoSuspendThreshold(SportTypeConstants.Basketball);
            var exporter = new GlobalAutoSuspendThresholdExporter(globalAutoSuspendThreshold, SportTypeConstants.Basketball);
            exporter.ResponseStream();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_GlobalAutoSuspendThreshold, Policy = Constant.Policy.Host)]
        public ActionResult SaveGlobalAutoSuspendThreshold(VirtualRateThresholdByGroup[] sports)
        {
            var result = service.UpdateGlobalAutoSuspendThreshold(
                SportTypeConstants.Basketball,
                sports,
                CurrentSession.UserId,
                CurrentContext.IP);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #endregion
    }
}
