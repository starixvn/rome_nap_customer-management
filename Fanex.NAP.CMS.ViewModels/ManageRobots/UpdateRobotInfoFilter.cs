// --------------------------------------------------------------------------
// <copyright file="UpdateRobotInfoFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageRobots
{
    using System;

    /// <summary>
    /// Update robot information filter.
    /// </summary>
    [Serializable]
    public class UpdateRobotInfoFilter
    {
        public int CustID { get; set; }

        public string Remark { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string SearchedUsername { get; set; }

        public bool? Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}