// --------------------------------------------------------------------------
// <copyright file="SMALossGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.SMALossMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Models.SMALossMonitor;

    /// <summary>
    /// SMA turnover group view model.
    /// </summary>
    [Serializable]
    public class SMALossGroup
    {
        public SMALossGroup(SMALossInfo rawData)
        {
            if (rawData != null)
            {
                this.LossLevel = rawData.Level;
                this.LossLevelRange = rawData.LevelRange;
                this.SMALossItems = new List<SMALossItem>();
            }
        }

        public byte LossLevel { get; set; }

        public string LossLevelRange { get; set; }

        public List<SMALossItem> SMALossItems { get; set; }
    }
}