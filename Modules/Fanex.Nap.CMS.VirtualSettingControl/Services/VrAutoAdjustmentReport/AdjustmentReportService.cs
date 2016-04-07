using System;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using Data.Repository;
    using Models.VrAutoAdjustmentReport;

    public class AdjustmentReportService : ServiceBase, IAdjustmentReportService
    {
        public IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria)
        {
            //return this.GetData<VrAutoAdjustmentLogEntry>(criteria);
            return new[]
            {
                new VrAutoAdjustmentLogEntry()
                {
                    CustId = 5,
                    NewRate = 8,
                    OldRate = 10,
                    UpdatedDate = new DateTime(),
                    Username = "dafadf"
                }
            };
        }

        public IEnumerable<CustomerMarginHistory> GetCustomerMarginHistory(ICriteria criteria)
        {
            return new[]
            {
                new CustomerMarginHistory(),
                new CustomerMarginHistory(),
            };
        }
    }
}