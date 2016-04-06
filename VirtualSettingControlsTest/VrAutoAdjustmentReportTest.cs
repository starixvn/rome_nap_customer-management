using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Nap.CMS.VirtualSettingControl.Controllers;
using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
using Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VirtualSettingControlsTest
{
    [TestClass]
    public class VrAutoAdjustmentReportTest
    {
        AutoAdjustmentReportController controller = new AutoAdjustmentReportController();


        [TestMethod]
        public void SendGetReportRequest()
        {
            var request =
                controller.GetAutoAdjustmentResult(new GetAutoAdjustmentReportCriteria()
                {
                    FromDate = new DateTime(2016, 4, 3),
                    ToDate = new DateTime(2016, 4, 3)
                });

            var result = Utility.Helper.CastJsonResult<IEnumerable<VrAutoAdjustmentLogEntry>>(request as JsonResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void SendGetCustomerMarginHistoryRequest()
        {
            var request = controller.GetCustomerMarginHistory(new GetCustomerMarginCriteria() { CustId = 8 });

            var result = Utility.Helper.CastJsonResult<Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VrAutoAdjustmentReport.CustomerMarginHistory>(request as JsonResult);

            Assert.IsNotNull(result);
            foreach (var month in result.Months)
            {
                Console.WriteLine(month);
            }
        }
    }
}
