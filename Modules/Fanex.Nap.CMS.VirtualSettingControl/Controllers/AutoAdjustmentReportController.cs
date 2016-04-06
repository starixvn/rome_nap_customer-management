namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Web.Mvc;
    using Criteria.VrAutoAdjustmentReport;
    using Criteria.VirtualSettingMonitor;
    using ViewModels.VrAutoAdjustmentReport;
    using Services.VrAutoAdjustmentReport;

    public class AutoAdjustmentReportController : Controller
    {
        private readonly IAdjustmentReportService _reportService;

        public AutoAdjustmentReportController() : this(null)
        {
        }

        public AutoAdjustmentReportController(IAdjustmentReportService reportService)
        {
            _reportService = reportService ?? new AdjustmentReportService();
        }

        public ActionResult GetAutoAdjustmentResult(GetAutoAdjustmentReportCriteria criteria)
        {
            return Json(_reportService.GetHistory(criteria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerMarginHistory(GetCustomerMarginHistoryCriteria criteria)
        {
            var model = _reportService.GetEntryDetails(criteria);
            var viewModel = new CustomerMarginHistory(model, new MarginCalculator(), Utility.Helper.GetMonthName);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}