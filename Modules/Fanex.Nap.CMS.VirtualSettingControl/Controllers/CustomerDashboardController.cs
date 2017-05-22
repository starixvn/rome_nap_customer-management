namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class CustomerDashboardController : BaseController
    {
        private readonly int[] _months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        private readonly IVirtualSettingMonitorService _virtualSettingMonitorService;

        private readonly ICustomerDashboardService _customerDashboardService;

        public CustomerDashboardController()
            : this(null, null)
        {
        }

        public CustomerDashboardController(
            IVirtualSettingMonitorService virtualSettingMonitorService,
            ICustomerDashboardService customerDashboardService)
        {
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
            criteria.ReportToDate = DateTime.Now;
            var dashboadInfo = _customerDashboardService.GetDashboardInfo(criteria);
            dashboadInfo.DashboardMarginModels = GetMarginData(criteria);

            return Json(new { DashboardData = dashboadInfo });
        }

        private List<MarginModel> GetMarginData(GetDashboardInfoCriteria inputParams)
        {
            var monthList = GenerateMonthList(DateTime.Now.Month, inputParams.MonthToCheck);
            var criteria = new GetCustomerMarginHistoryCriteria
            {
                CustIds = inputParams.CustId.ToString(),
                MonthToCheck = inputParams.MonthToCheck,
                ReportToDate = DateTime.Now,
                SportId = inputParams.SportId
            };

            var margins = _virtualSettingMonitorService.GetCustomerVirtualSettingHistories(criteria, monthList).ToList();
            margins.RemoveAt(margins.Count - 1);

            return margins;
        }

        #region Utilities
        private int[] GenerateMonthList(int currentMonth, int monthRange)
        {
            for (var i = 23; i >= 0; i--)
            {
                if (_months[i] == currentMonth)
                {
                    int[] result = new int[monthRange];
                    Array.Copy(_months, i - monthRange + 1, result, 0, monthRange);
                    return result;
                }
            }

            return null;
        }

        #endregion
    }
}
