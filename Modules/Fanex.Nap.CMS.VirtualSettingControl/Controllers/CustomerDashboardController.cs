namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CustomerDashboardController : BaseController
    {
        private readonly string[] MonthsName = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private readonly int[] Months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        private readonly ISettingService _settingService;

        private readonly IVirtualSettingMonitorService _virtualSettingMonitorService;

        private readonly ICustomerDashboardService _customerDashboardService;

        public CustomerDashboardController()
            : this(null, null, null)
        {
        }

        public CustomerDashboardController(
            ISettingService settingService,
            IVirtualSettingMonitorService virtualSettingMonitorService,
            ICustomerDashboardService customerDashboardService)
        {
            _settingService = settingService ?? new SettingService();
            _virtualSettingMonitorService = virtualSettingMonitorService ?? new VirtualSettingMonitorService();
            _customerDashboardService = customerDashboardService ?? new CustomerDashboardService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index(GetDashboardInfoCriteria criteria)
        {
            return View("~/Areas/VirtualSettingControl/Views/CustomerDashboard/Index.cshtml", criteria);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualSettingMonitor, Policy = Constant.Policy.Read)]
        public ActionResult GetDashboardData(GetDashboardInfoCriteria criteria)
        {
            //criteria.CustId = 111311;
            criteria.ReportToDate = DateTime.Now; //DateTime.Parse("2015-08-01"); // 
            var dashboadInfo = _customerDashboardService.GetDashboardInfo(criteria);
            dashboadInfo.DashboardMarginModels = GetMarginData(criteria);

            return Json(new { DashboardData = dashboadInfo }, JsonRequestBehavior.AllowGet);
        }

        private List<MarginModel> GetMarginData(GetDashboardInfoCriteria inputParams)
        {
            var monthList = GenerateMonthList(DateTime.Now.Month, inputParams.MonthToCheck);
            var criteria = new GetCustomerMarginLatestMonthsCriteria
            {
                CustId = inputParams.CustId,
                MonthToCheck = inputParams.MonthToCheck,
                ReportToDate = DateTime.Now, //DateTime.Parse("2015-08-01"),
                SportId = inputParams.SportId
            };

            var margins = _virtualSettingMonitorService.GetCustomerVirtualSettingLatestMonths(criteria, monthList);
            margins.RemoveAt(margins.Count - 1);

            return margins;
        }

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

        private string[] GetMonthListName(int[] months)
        {
            var monthListName = new string[months.Length];

            for (var i = 0; i < months.Length; i++)
            {
                monthListName[i] = MonthsName[months[i] - 1];
            }

            return monthListName;
        }
        #endregion
    }
}
