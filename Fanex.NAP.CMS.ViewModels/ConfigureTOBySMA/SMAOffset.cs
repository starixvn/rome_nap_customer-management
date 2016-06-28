// --------------------------------------------------------------------------
// <copyright file="SMAOffset.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA
{
    using System;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureTOBySMA;

    /// <summary>
    /// View model contains the SMA turnover.
    /// </summary>
    [Serializable]
    public class SMAOffset
    {
        public SMAOffset(SMAOffsetInfo rawData)
        {
            if (rawData != null)
            {
                this.CustID = rawData.CustID;
                this.Username = rawData.Username;
                this.Level = CacheRef.GetLevel(rawData.RoleID);
                this.Currency = rawData.Currency;
                this.GivenCredit = rawData.Credit ?? 0;
                this.Offset = rawData.Offset;
            }
        }

        public string Username { get; set; }

        public int CustID { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal GivenCredit { get; set; }

        public int Offset { get; set; }
    }
}