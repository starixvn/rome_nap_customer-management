// --------------------------------------------------------------------------
// <copyright file="SMALossItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.LossMonitor
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.LossMonitor;

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
                this.CustId = rawData.CustId;
                this.Username = rawData.Username;
                this.Level = CacheRef.GetLevel(rawData.RoleID);
                this.Currency = rawData.Currency;
                this.YesterdayBalance = Converter.ToDecimal(rawData.YesterdayBalance);
                this.TotalBalance = Converter.ToDecimal(rawData.TotalBalance);
                this.MaxLoss = Converter.ToDecimal(rawData.MaxLoss);
                this.LossPercentage = Converter.ToInt(rawData.PercentageLoss);
                this.ChangedLevel = Converter.ToInt(rawData.Movement);
            }
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal MaxLoss { get; set; }

        public int LossPercentage { get; set; }

        public int PercentageUsed { get; set; }

        public int ChangedLevel { get; set; }
    }
}