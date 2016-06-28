// --------------------------------------------------------------------------
// <copyright file="SMALossGroup.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.LossMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Models.LossMonitor;

    /// <summary>
    /// SMA loss group view model.
    /// </summary>
    [Serializable]
    public class SMALossGroup
    {
        public SMALossGroup(SMALossInfo rawData, string levelRange)
        {
            if (rawData != null)
            {
                this.LossLevel = rawData.LevelID;
                this.LossLevelRange = levelRange;
                this.SMALossItems = new List<SMALossItem>();
            }
        }

        public byte LossLevel { get; set; }

        public string LossLevelRange { get; set; }

        public List<SMALossItem> SMALossItems { get; set; }
    }
}