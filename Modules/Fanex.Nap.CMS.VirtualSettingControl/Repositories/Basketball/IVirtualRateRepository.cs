namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.Basketball
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IVirtualRateRepository
    {
        IEnumerable<RateInfo> GetRateInfo(ICriteria criteria);

        Tuple<IEnumerable<VirtualRateDTO>, VirtualRateDetailsReport> GetVirtualRateDetailsReport(ICriteria criteria);
    }
}
