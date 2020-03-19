namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class GlobalAutoSuspendThreshold
    {
        public GlobalAutoSuspendThreshold(
            IEnumerable<RateInfo> virtualRateValueRange,
            IEnumerable<VirtualRateThresholdByGroup> sports)
        {
            VirtualRateValueRange = virtualRateValueRange;
            Sports = sports;
        }

        public IEnumerable<RateInfo> VirtualRateValueRange { get; }

        public IEnumerable<VirtualRateThresholdByGroup> Sports { get; }
    }
}
