namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.Web.Mvc;
    using Criteria.VirtualSettingMonitor;
    using Criteria.VrAutoAdjustmentReport;
    using Infrastructure;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services.VrAutoAdjustmentReport;
    using ViewModels.VrAutoAdjustmentReport;

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

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult GetAutoAdjustmentResult(GetAutoAdjustmentReportCriteria criteria)
        {
            return Json(_reportService.GetReport(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult GetCustomerMarginHistory(GetCustomerMarginCriteria criteria)
        {
            var model = _reportService.GetCustomerMarginHistory(criteria);
            var viewModel = new CustomerMarginHistory(model, new MarginCalculator(), Utility.Helper.GetMonthName);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}