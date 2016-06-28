// --------------------------------------------------------------------------
// <copyright file="AddSmartPunterFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.12">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;

    /// <summary>
    /// Add smart punter filter.
    /// </summary>
    [Serializable]
    public class AddSmartPunterFilter
    {
        public string Username { get; set; }

        public string Remark { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string SearchedUsername { get; set; }

        public byte Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}