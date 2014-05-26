// --------------------------------------------------------------------------
// <copyright file="SMATurnoverByEvent.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureSMATOByEvent;

    /// <summary>
    /// SMA turnover by event view model.
    /// </summary>
    [Serializable]
    public class SMATurnoverByEvent
    {
        public SMATurnoverByEvent(SMATurnoverByEventInfo rawData)
        {
            if (rawData != null)
            {
                this.MatchID = rawData.MatchID;
                this.CustID = rawData.CustID.Value;
                this.Username = rawData.Username;
                this.Level = CacheRef.GetLevel(rawData.RoleID);
                this.Currency = rawData.Currency;
                this.GivenCredit = Converter.ToDecimal(rawData.Credit);
                this.MaxTORate = rawData.Value;
                this.MaxTOAmount = Converter.ToDecimal(rawData.MaxTurnover);
            }
        }

        public int MatchID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal GivenCredit { get; set; }

        public int MaxTORate { get; set; }

        public decimal MaxTOAmount { get; set; }
    }
}