namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using Services.VrAutoAdjustmentReport;
    using System;
    using System.Web.Mvc;

    public class NewAutoAdjustmentReportController : BaseController
    {
        private const int MonthsToCheck = 12;

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
                Months = Helper.GetDate(criteria.AdjustDate.AddDays(-1), MonthsToCheck, "MMM - yyyy"),
                Reports = _adjustmentReportService.GetNewCustomerMarginHistory(criteria, MonthsToCheck, criteria.AdjustDate)
            };

            return Json(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public void ExportVRAdjustmentReport(GetCustomerMarginHistoryCriteria criteria)
        {
            _adjustmentReportService.ExportNewVRAdjustmentReport(criteria, MonthsToCheck, criteria.AdjustDate);
        }
    }
}