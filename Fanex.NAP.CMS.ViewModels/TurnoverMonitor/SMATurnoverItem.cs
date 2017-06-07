// --------------------------------------------------------------------------
// <copyright file="SMATurnoverItem.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.TurnoverMonitor
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.TurnoverMonitor;

    /// <summary>
    /// SMA turnover view model.
    /// </summary>
    [Serializable]
    public class SMATurnoverItem
    {
        public SMATurnoverItem()
        {
        }

        public SMATurnoverItem(SMATurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.Username = rawData.Username;
                this.Level = CacheRef.GetLevel(rawData.RoleID);
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
    }
}