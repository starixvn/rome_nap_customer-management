// --------------------------------------------------------------------------
// <copyright file="UpdateSMAOffsetFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA
{
    using System;

    /// <summary>
    /// View modal contains indicates the updating data.
    /// </summary>
    [Serializable]
    public class UpdateSMAOffsetFilter
    {
        public int CustID { get; set; }

        public int Value { get; set; }

        public string SearchedName { get; set; }
    }
}