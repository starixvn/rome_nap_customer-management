// --------------------------------------------------------------------------
// <copyright file="BalanceUpDayInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
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