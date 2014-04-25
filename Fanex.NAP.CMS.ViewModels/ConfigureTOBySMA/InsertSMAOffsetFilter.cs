// --------------------------------------------------------------------------
// <copyright file="InsertSMAOffsetFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA
{
    using System;

    /// <summary>
    /// View modal contains indicates the new offset of a SMA.
    /// </summary>
    [Serializable]
    public class InsertSMAOffsetFilter
    {
        public string SMAName { get; set; }

        public int Value { get; set; }

        public string SearchedName { get; set; }
    }
}
