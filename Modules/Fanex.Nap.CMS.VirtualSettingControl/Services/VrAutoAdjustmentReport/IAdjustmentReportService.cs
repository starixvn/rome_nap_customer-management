namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using Data.Repository;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria);
    }
}