namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class VirtualSettingMonitorController : BaseController
    {
        private const int MonthsToCheck = 13;

        private static readonly int[] Months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        private readonly ISettingService _settingService;

        private readonly IVirtualSettingMonitorService _virtualSettingMonitorService;

        private readonly ICustomerDashboardService _customerDashboardService;

        public VirtualSettingMonitorController()
            : this(null, null, null)
        {
        }

        public VirtualSettingMonitorController(
            ISettingService settingService,
            IVirtualSettingMonitorService virtualSettingMonitorService,
            ICustomerDashboardService customerDashboardService)
        {
            _settingService = settingService ?? new SettingService();
            _virtualSettingMonitorService = virtualSettingMonitorService ?? new VirtualSettingMonitorService();
            _customerDashboardService = customerDashboardService ?? new CustomerDashboardService();
        }

        #region Virtual Rate Monitoring
        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.VirtualSettingMonitor, Constant.Policy.Read);
            return View("~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetVirtualSettingMonitorCriteria criteria)
        {
            criteria.ReportToDate = DateTime.Now;
            var monthList = GenerateMonthList(criteria.ReportToDate.Month, MonthsToCheck);
            var reversedMonthList = GenerateMonthList(criteria.ReportToDate.Month, MonthsToCheck);
            Array.Reverse(reversedMonthList);
            var virtualSettings = _virtualSettingMonitorService.GetVirtualSettingMonitor(criteria, monthList);

            return Json(
                new
                {
                    TotalRow = virtualSettings == null ? 0 : virtualSettings.CustMargin.Count,
                    MonthList = Helper.GetDate(DateTime.Today, MonthsToCheck, "MMM - yyyy"),
                    VirtualSettings = virtualSettings
                }, 
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Utilities
        private int[] GenerateMonthList(int currentMonth, int monthRange)
        {
            for (var i = 23; i >= 0; i--)
            {
                if (Months[i] == currentMonth)
                {
                    int[] result = new int[monthRange];
                    Array.Copy(Months, i - monthRange + 1, result, 0, monthRange);
                    return result;
                }
            }

            return null;
        }
        #endregion
    }
}
