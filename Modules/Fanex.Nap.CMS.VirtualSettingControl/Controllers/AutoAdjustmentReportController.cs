namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Services.VirtualSettingMonitor;
    using Services.VrAutoAdjustmentReport;

    public class AutoAdjustmentReportController : BaseController
    {
        private const int MonthsToCheck = 6;

        private readonly IAdjustmentReportService _adjustmentReportService;

        public AutoAdjustmentReportController()
            : this(null, null)
        {
        }

        public AutoAdjustmentReportController(IAdjustmentReportService adjustmentReportService, IVirtualSettingMonitorService virtualSettingMonitorService)
        {
            _adjustmentReportService = adjustmentReportService ?? new AdjustmentReportService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult ViewVrAdjustmentReport(GetVrAdjustmentLogCriteria criteria)
        {
            var logResult = _adjustmentReportService.GetReport(criteria);

            var histories =
                _adjustmentReportService.GetVirtualSettingLatestMonths(logResult.Select(c => c.CustId), DateTime.Today, MonthsToCheck);

            return Json(new
            {
                Months = Helper.GetDate(DateTime.Today, MonthsToCheck, "MMM - yyyy"),
                Reports = logResult.Select(entry => new LogEntryViewModel(entry, histories[entry.CustId]))
            });
        }
    }
}