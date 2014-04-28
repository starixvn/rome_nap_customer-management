// --------------------------------------------------------------------------
// <copyright file="SMATurnover.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.TurnoverMonitor
{
    using System;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Core.Resources;

    /// <summary>
    /// SMA turnover view model.
    /// </summary>
    [Serializable]
    public class SMATurnover
    {
        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal GivenCredit { get; set; }

        public int MaxTORate { get; set; }

        public decimal MaxTOAmount { get; set; }

        public decimal MemberTO { get; set; }

        public int PercentageUsed { get; set; }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <returns>The level name.</returns>
        private string GetLevel(byte? roleID)
        {
            var level = string.Empty;

            if (roleID.HasValue)
            {
                switch (roleID.Value)
                {
                    case CustomerLevels.Super:
                        level = ViewLang.Super;
                        break;

                    case CustomerLevels.Master:
                        level = ViewLang.Master;
                        break;

                    case CustomerLevels.Agent:
                        level = ViewLang.Agent;
                        break;
                }
            }

            return level;
        }
    }
}