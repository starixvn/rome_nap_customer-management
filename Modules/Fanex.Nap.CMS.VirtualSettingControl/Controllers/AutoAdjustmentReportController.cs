namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Services.VrAutoAdjustmentReport;
    using System.Web.Mvc;

    public class AutoAdjustmentReportController : BaseController
    {
        private readonly IAdjustmentReportService _adjustmentReportService;

        public AutoAdjustmentReportController()
            : this(null)
        {
        }

        public AutoAdjustmentReportController(IAdjustmentReportService adjustmentReportService)
        {
            _adjustmentReportService = adjustmentReportService ?? new AdjustmentReportService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Index.cshtml");
        }
    }
}