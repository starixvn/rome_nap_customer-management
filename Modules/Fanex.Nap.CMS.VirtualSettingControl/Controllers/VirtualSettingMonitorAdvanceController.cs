namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitorAdvance;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class VirtualSettingMonitorAdvanceController : BaseController
    {
        private readonly IVirtualSettingMonitorAdvanceService _virtualSettingMonitorAdvanceService;

        public VirtualSettingMonitorAdvanceController()
            : this(null)
        {
        }

        public VirtualSettingMonitorAdvanceController(IVirtualSettingMonitorAdvanceService virtualSettingMonitorAdvanceService)
        {
            _virtualSettingMonitorAdvanceService = virtualSettingMonitorAdvanceService ?? new VirtualSettingMonitorAdvanceService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitorAdvance, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.VirtualSettingMonitor, Constant.Policy.Read);
            return View("~/Areas/VirtualSettingControl/Views/VirtualSettingMonitorAdvance/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitorAdvance, Policy = Constant.Policy.Read)]
        public ActionResult Generate(GetVirtualSettingMonitorAdvanceCriteria criteria)
        {
            var monthsToCheck = (criteria.ToDate.Month + criteria.ToDate.Year * 12) - (criteria.FromDate.Month + criteria.FromDate.Year * 12) + 1;

            var result = new
            {
                Months = Helper.GetDate(criteria.ToDate, monthsToCheck, "MMM - yyyy"),
                Reports = _virtualSettingMonitorAdvanceService.GetVirtualSettingAdvanceMonitor(criteria, monthsToCheck)
            };

            return Json(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitorAdvance, Policy = Constant.Policy.Read)]
        public void Export(GetVirtualSettingMonitorAdvanceCriteria criteria)
        {
            var monthsToCheck = (criteria.ToDate.Month + criteria.ToDate.Year * 12) - (criteria.FromDate.Month + criteria.FromDate.Year * 12) + 1;

            _virtualSettingMonitorAdvanceService.ExportVirtualSettingAdvanceMonitor(criteria, monthsToCheck);
        }
    }
}