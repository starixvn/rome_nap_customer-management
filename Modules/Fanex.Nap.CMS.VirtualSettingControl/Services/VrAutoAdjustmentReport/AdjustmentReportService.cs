namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Models.VrAutoAdjustmentReport;

    public class AdjustmentReportService : ServiceBase, IAdjustmentReportService
    {
        public IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria)
        {
            return this.GetData<VrAutoAdjustmentLogEntry>(criteria);
        }
    }
}