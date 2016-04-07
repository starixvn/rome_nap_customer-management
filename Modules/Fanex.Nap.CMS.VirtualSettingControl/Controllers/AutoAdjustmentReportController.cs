using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;

namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Services.VirtualSettingMonitor;
    using Services.VrAutoAdjustmentReport;

    public class AutoAdjustmentReportController : BaseController
    {
        private const int MonthsToCheck = 6;

        private readonly IAdjustmentReportService _adjustmentReportService;

        private readonly IVirtualSettingMonitorService _virtualSettingMonitorService;

        public AutoAdjustmentReportController()
            : this(null, null)
        {
        }

        public AutoAdjustmentReportController(IAdjustmentReportService adjustmentReportService, IVirtualSettingMonitorService virtualSettingMonitorService)
        {
            _adjustmentReportService = adjustmentReportService ?? new AdjustmentReportService();
            _virtualSettingMonitorService = virtualSettingMonitorService ?? new VirtualSettingMonitorService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Index.cshtml");
        }

        public ActionResult ViewVrAdjustmentReport(GetVrAdjustmentLogCriteria criteria)
        {
            return Json(_adjustmentReportService.GetReport(criteria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewHistory(int custId)
        {
            var today = DateTime.Now;
            var monthList = Helper.GenerateMonthList(today, MonthsToCheck);
            var criteria = new GetCustomerMarginLatestMonthsCriteria()
            {
                CustId = custId,
                MonthToCheck = MonthsToCheck,
                ReportToDate = today,
                SportId = -1
            };
            var custMarginHistory = _virtualSettingMonitorService.GetCustomerVirtualSettingLatestMonths(criteria, monthList);
            var result = new
            {
                MarginHistory = custMarginHistory,
                MonthList = Helper.GetMonthListName(monthList)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}