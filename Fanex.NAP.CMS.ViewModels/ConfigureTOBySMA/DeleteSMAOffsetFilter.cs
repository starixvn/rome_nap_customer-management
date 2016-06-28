// --------------------------------------------------------------------------
// <copyright file="DeleteSMAOffsetFilter.cs">
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
    /// View modal contains indicates the ID of SMA needs to be removed.
    /// </summary>
    [Serializable]
    public class DeleteSMAOffsetFilter
    {
        public int CustID { get; set; }

        public string SearchedName { get; set; }
    }
}