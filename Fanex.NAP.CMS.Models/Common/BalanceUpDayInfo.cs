// --------------------------------------------------------------------------
// <copyright file="BalanceUpDayInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.Common
{
    using System;

    /// <summary>
    /// Balance up day model.
    /// </summary>
    public class BalanceUpDayInfo
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}