// --------------------------------------------------------------------------
// <copyright file="SMALossLevel.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.27">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.LossMonitor
{
    /// <summary>
    /// Loss level model.
    /// </summary>
    public class SMALossLevel
    {
        public byte LevelID { get; set; }

        public byte ActionID { get; set; }

        public int Value { get; set; }
    }
}