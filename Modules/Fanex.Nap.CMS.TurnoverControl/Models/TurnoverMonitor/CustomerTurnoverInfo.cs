// --------------------------------------------------------------------------
// <copyright file="CustomerTurnoverInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Models;

    /// <summary>
    /// Customer Turnover Information.
    /// </summary>
    [Serializable]
    public class CustomerTurnoverInfo : CustomerInfo
    {
        public decimal MemberTurnover { get; set; }

        public decimal CompanyTurnover { get; set; }

        public MatchInfo MatchInfo { get; set; }

        public IdNameModel League { get; set; }
    }
}
