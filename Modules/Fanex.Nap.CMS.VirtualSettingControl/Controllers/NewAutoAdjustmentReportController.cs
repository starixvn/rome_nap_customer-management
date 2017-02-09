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

        private const int MonthsToCheck = 13;

        public NewAutoAdjustmentReportController()
        {
            _adjustmentReportService = new AdjustmentReportService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/VirtualSettingControl/Views/NewVrAutoAdjustmentReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult ViewVrAdjustmentReport(GetCustomerMarginHistoryCriteria criteria)
        {
            criteria.AdjustDate = new DateTime(2017, 2, 9, 0, 6, 48, 650);
            var data = _adjustmentReportService.GetCustomerMarginHistory(criteria);
            var result = new
            {
                Months = Helper.GetDate(DateTime.Today, MonthsToCheck, "MMM - yyyy"),
                Reports = data
            };

            return Json(result);
        }
    }
}