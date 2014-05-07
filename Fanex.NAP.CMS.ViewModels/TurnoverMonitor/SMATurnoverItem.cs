// --------------------------------------------------------------------------
// <copyright file="SMATurnoverItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.TurnoverMonitor
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Models.TurnoverMonitor;

    /// <summary>
    /// SMA turnover view model.
    /// </summary>
    [Serializable]
    public class SMATurnoverItem
    {
        public SMATurnoverItem(SMATurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.Username = rawData.Username;
                this.Level = GetLevel(rawData.RoleID);
                this.Currency = rawData.Currency;
                this.GivenCredit = Converter.ToDecimal(rawData.Credit);
                this.MaxTORate = Converter.ToInt(rawData.Value);
                this.MaxTOAmount = Converter.ToDecimal(rawData.MaxTurnover);
                this.MemberTO = Converter.ToDecimal(rawData.TotalTurnover);
                this.PercentageUsed = Converter.ToInt(rawData.PercentageUsed);
            }
        }

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