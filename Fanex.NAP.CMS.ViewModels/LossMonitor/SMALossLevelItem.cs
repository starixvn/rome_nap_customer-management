// --------------------------------------------------------------------------
// <copyright file="SMALossLevelItem.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.27">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.LossMonitor
{
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Models.LossMonitor;

    /// <summary>
    /// SMA loss item view model.
    /// </summary>
    public class SMALossLevelItem
    {
        public SMALossLevelItem()
        {
            this.Value = string.Empty;
            this.LevelID = 0;
        }

        public SMALossLevelItem(SMALossLevel currentLevel, SMALossLevel higherLevel)
        {
            int maximun = -1;

            if (higherLevel != null)
            {
                maximun = higherLevel.Value - 1;
            }

            if (currentLevel != null)
            {
                int minimum = currentLevel.Value;
                this.LevelID = currentLevel.LevelID;

                this.Value = maximun > 0 ?
                    string.Format("{0}% - {1}%", minimum, maximun) :
                    string.Format("{0}% & {1}", minimum, ViewLang.Above);
            }
        }

        public byte LevelID { get; set; }

        public string Value { get; set; }
    }
}