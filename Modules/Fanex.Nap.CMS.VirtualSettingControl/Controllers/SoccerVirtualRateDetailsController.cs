namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class SoccerVirtualRateDetailsController : BaseController
    {
        private readonly IVirtualRateDetailsService _virtualRateDetailsService;

        public SoccerVirtualRateDetailsController()
            : this(null)
        {
        }

        public SoccerVirtualRateDetailsController(IVirtualRateDetailsService virtualRateDetailsService)
        {
            _virtualRateDetailsService = virtualRateDetailsService ?? new VirtualRateDetailsService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult Index(string username)
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Host);
            return View("~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/Index.cshtml", (object)username);
        }

        [HttpPost]
        public JsonResult GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var hasRight =
                 (criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails_GlobalSetting, Constant.Policy.Read)) ||
                 (!criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Read));

            if (!hasRight)
            {
                return Json(new VirtualRateDetailsReport { ResultCode = 500, ResultMessage = Labels.AccessDeniedMessage });
            }

            var result = _virtualRateDetailsService.GetVirtualRateDetails(criteria);

            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
        {
            var hasEditRight =
                 (criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails_GlobalSetting, Constant.Policy.Host)) ||
                 (!criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Host));

            if (!hasEditRight)
            {
                return Json(new Result { Code = 1, Message = Labels.AccessDeniedMessage });
            }

            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _virtualRateDetailsService.SaveVirtualRateDetails(criteria);

            return Json(result);
        }

       

        public void ExportVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var hasRight =
               (criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails_GlobalSetting, Constant.Policy.Read)) ||
               (!criteria.IsGlobalSetting && HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Read));

            if (!hasRight)
            {
                return;
            }

            var report = _virtualRateDetailsService.GetVirtualRateDetails(criteria);
            var exporter = new SoccerVirtualRateDetailsExporter(report);
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
        {
            criteria.Accounts = criteria.Accounts ?? string.Empty;
            var result = _virtualRateDetailsService.ValidateAssociatedAccounts(criteria);

            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria)
        {
            if (!HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Host))
            {
                return Json(new Result { Code = 1, Message = Labels.AccessDeniedMessage });
            }

            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _virtualRateDetailsService.SaveAssociatedAccounts(criteria);

            return Json(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateLog, Policy = Constant.Policy.Read)]
        public ActionResult ViewLog()
            => View("~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/ViewLog.cshtml");

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateLog, Policy = Constant.Policy.Read)]
        public JsonResult GetLogs(GetVirtualRateLogsCriteria criteria)
        {
            var result = _virtualRateDetailsService.GetLogs(criteria);

            return Json(result);
        }

        public ActionResult ViewRealValues(ViewRealValuesCriteria criteria)
        {
            if (!HasRight(CMSFunctionKeys.Soccer_VirtualRateDetails, Constant.Policy.Read))
            {
                return Json(string.Empty);
            }

            var model = _virtualRateDetailsService.ViewRealValues(criteria);
            return View("~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/ViewRealValues.cshtml", model);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public void ExportRealValues(ViewRealValuesCriteria criteria)
        {
            var report = _virtualRateDetailsService.ViewRealValues(criteria);
            var exporter = new VirtualRateRealValuesExporter(report, SportTypeConstants.Soccer);
            exporter.ResponseStream();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Soccer_VirtualRateDetails_GlobalSetting, Policy = Constant.Policy.Read)]
        public ActionResult GlobalSetting()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ESports_VirtualRateDetails_GlobalSetting, Constant.Policy.Host);
            return View("~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/GlobalSetting.cshtml");
        }
    }
}
