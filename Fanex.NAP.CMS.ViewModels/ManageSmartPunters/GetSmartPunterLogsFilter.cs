// --------------------------------------------------------------------------
// <copyright file="GetSmartPunterLogsFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;

    /// <summary>
    /// Getting smart punter's logs filter.
    /// </summary>
    [Serializable]
    public class GetSmartPunterLogsFilter
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Username { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}