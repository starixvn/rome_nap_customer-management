// --------------------------------------------------------------------------
// <copyright file="MatchInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Match Information.
    /// </summary>
    [Serializable]
    public class MatchInfo : IdNameModel
    {
        public IdNameModel HomeTeam { get; set; }

        public IdNameModel AwayTeam { get; set; }

        public DateTime EventDate { get; set; }

        public string EventDateString { get; set; }

        public string EventStatus { get; set; }
    }
}
