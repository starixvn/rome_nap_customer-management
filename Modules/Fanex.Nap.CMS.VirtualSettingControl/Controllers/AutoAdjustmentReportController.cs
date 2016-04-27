namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport;
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
            var queryResult = _adjustmentReportService.GetReport(criteria);

            return Json(new
            {
                queryResult.ItemsCount,
                Months = Helper.GetDate(DateTime.Today, MonthsToCheck, "MMM - yyyy"),
                Reports = GetReport(queryResult.Items)
            });
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.AutoAdjustmentReport, Policy = Constant.Policy.Read)]
        public ActionResult ViewHistory(int custId)
        {
            var monthList = Helper.GenerateMonthList(DateTime.Now, MonthsToCheck);
            var custMarginHistory = _adjustmentReportService.GetVirtualSettingLatestMonths(custId, DateTime.Now, MonthsToCheck);
            var result = new
            {
                MarginHistory = custMarginHistory,
                MonthList = Helper.GetMonthListName(monthList)
            };

            return Json(result);
        }

        private IEnumerable<LogEntryViewModel> GetReport(IEnumerable<VrAutoAdjustmentLogEntry> logs)
        {
            var historyDict = logs.Select(e => e.CustId).Distinct().ToDictionary(id => id, id => (IEnumerable<MarginModel>)null);

            foreach (var entry in logs)
            {
                if (historyDict[entry.CustId] == null)
                {
                    historyDict[entry.CustId] =
                        _adjustmentReportService.GetVirtualSettingLatestMonths(entry.CustId, DateTime.Today, MonthsToCheck);
                }

                yield return new LogEntryViewModel(entry, historyDict[entry.CustId]);
            }
        }
    }
}