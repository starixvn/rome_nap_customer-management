namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class VirtualRateDetailsController : BaseController
    {
        private readonly IVirtualRateDetailsService _virtualRateDetailsService;

        public VirtualRateDetailsController()
            : this(null)
        {
        }

        public VirtualRateDetailsController(IVirtualRateDetailsService virtualRateDetailsService)
        {
            _virtualRateDetailsService = virtualRateDetailsService ?? new VirtualRateDetailsService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public ActionResult Index(string username)
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ESports_VirtualRateDetails, Constant.Policy.Host);
            return View("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Index.cshtml", (object)username);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var result = _virtualRateDetailsService.GetVirtualRateDetails(criteria);

            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
        {
            if (!HasRight(CMSFunctionKeys.ESports_VirtualRateDetails, Constant.Policy.Host))
            {
                return Json(new Result { Code = 1, Message = Labels.AccessDeniedMessage });
            }

            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _virtualRateDetailsService.SaveVirtualRateDetails(criteria);

            return Json(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails_GlobalSetting, Policy = Constant.Policy.Read)]
        public ActionResult GlobalSetting()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ESports_VirtualRateDetails_GlobalSetting, Constant.Policy.Host);
            return View("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/GlobalSetting.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public void ExportVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var report = _virtualRateDetailsService.GetVirtualRateDetails(criteria);
            var exporter = new VirtualRateDetailsExporter(report);
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
        {
            criteria.Accounts = criteria.Accounts ?? string.Empty;
            var result = _virtualRateDetailsService.ValidateAssociatedAccounts(criteria);

            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_VirtualRateDetails, Policy = Constant.Policy.Read)]
        public JsonResult SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria)
        {
            if (!HasRight(CMSFunctionKeys.ESports_VirtualRateDetails, Constant.Policy.Host))
            {
                return Json(new Result { Code = 1, Message = Labels.AccessDeniedMessage });
            }

            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _virtualRateDetailsService.SaveAssociatedAccounts(criteria);

            return Json(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateLog, Policy = Constant.Policy.Read)]
        public ActionResult ViewLog()
            => View("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/ViewLog.cshtml");

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateLog, Policy = Constant.Policy.Read)]
        public JsonResult GetLogs(GetVirtualRateLogsCriteria criteria)
        {
            var result = _virtualRateDetailsService.GetLogs(criteria);

            return Json(result);
        }
    }
}
