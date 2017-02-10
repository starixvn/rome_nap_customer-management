namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Services.VrAutoAdjustmentReport;
    using System;
    using System.Web.Mvc;

    public class NewAutoAdjustmentReportController : BaseController
    {
        private readonly IAdjustmentReportService _adjustmentReportService;

        public NewAutoAdjustmentReportController()
        {
            _adjustmentReportService = new AdjustmentReportService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var reportDateList = _adjustmentReportService.GetReportDate(new GetCustomerMarginHistoryDateCriteria());
            ViewBag.ReportDateList = reportDateList;
            return View("~/Areas/VirtualSettingControl/Views/NewVrAutoAdjustmentReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult ViewVrAdjustmentReport(GetCustomerMarginHistoryCriteria criteria)
        {
            var result = new
            {
                Months = Helper.GetDate(criteria.AdjustDate, 13, "MMM - yyyy"),
                Reports = _adjustmentReportService.GetCustomerMarginHistory(criteria, 13, criteria.AdjustDate)
            };

            return Json(result);
        }
    }
}