namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using Data.Repository;
    using Models.VrAutoAdjustmentReport;

    public class AdjustmentReportService : IAdjustmentReportService
    {
        public IEnumerable<VrAutoAdjustmentLogEntry> GetHistory(ICriteria criteria)
        {
            return new[]
            {
                new VrAutoAdjustmentLogEntry(),
                new VrAutoAdjustmentLogEntry(),
            };
        }

        public IEnumerable<CustomerMarginHistory> GetEntryDetails(ICriteria criteria)
        {
            return new[]
            {
                new CustomerMarginHistory(),
                new CustomerMarginHistory(),
            };
        }
    }
}