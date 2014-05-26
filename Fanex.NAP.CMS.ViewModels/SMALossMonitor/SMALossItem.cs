// --------------------------------------------------------------------------
// <copyright file="SMALossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.SMALossMonitor
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.SMALossMonitor;

    /// <summary>
    /// SMA loss item view model.
    /// </summary>
    [Serializable]
    public class SMALossItem
    {
        public SMALossItem(SMALossInfo rawData)
        {
            if (rawData != null)
            {
                this.Username = rawData.Username;
                this.Level = CacheRef.GetLevel(rawData.RoleID);
                this.Currency = rawData.Currency;
                this.TotalBalance = Converter.ToDecimal(rawData.TotalBalance);
                this.MaxLoss = Converter.ToDecimal(rawData.MaxLoss);
                this.LossPercentage = Converter.ToInt(rawData.LossPercentage);
            }
        }

        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal MaxLoss { get; set; }

        public int LossPercentage { get; set; }

        public int PercentageUsed { get; set; }
    }
}