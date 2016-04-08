namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Services.VirtualSettingMonitor;
    using Services.VrAutoAdjustmentReport;

    public class AutoAdjustmentReportController: BaseController
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

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult ViewVrAdjustmentReport(GetVrAdjustmentLogCriteria criteria)
        {
            var result = _adjustmentReportService.GetReport(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
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