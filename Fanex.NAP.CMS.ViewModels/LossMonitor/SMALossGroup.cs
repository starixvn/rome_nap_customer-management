namespace Fanex.NAP.CMS.ViewModels.LossMonitor
{
    using System;
    using System.Collections.Generic;
    using Models.LossMonitor;

    [Serializable]
    public class SMALossGroup
    {
        public SMALossGroup(byte levelId, string levelRange, IEnumerable<SMALossInfo> smaLossItems)
        {
            LossLevel = levelId;
            LossLevelRange = levelRange;
            SMALossItems = smaLossItems;
        }

        public byte LossLevel { get; }

        public string LossLevelRange { get; }

        public IEnumerable<SMALossInfo> SMALossItems { get; }
    }
}