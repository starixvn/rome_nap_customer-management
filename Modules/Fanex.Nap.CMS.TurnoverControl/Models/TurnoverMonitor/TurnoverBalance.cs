// --------------------------------------------------------------------------
// <copyright file="TurnoverBalance.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Resources;

    /// <summary>
    /// Turnover Balance.
    /// </summary>
    [Serializable]
    public class TurnoverBalance : IdNameModel
    {
        public string Balance { get; set; }

        public List<decimal> TurnoverAmount
        {
            get
            {
                var result = new List<decimal>();
                var amounts = Balance.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var amount in amounts)
                {
                    result.Add(Converter.ToDecimal(amount));
                }

                return result;
            }
        }

        public string BalanceFormat
        {
            get
            {
                return GetBalanceFormat();
            }
        }

        /// <summary>
        /// Gets the balance format.
        /// </summary>
        /// <returns>The balance format.</returns>
        private string GetBalanceFormat()
        {
            switch (Id)
            {
                case BetTypeConstants.Handicap:
                    return Labels.TurnoverMonitor_BalanceFormat_Handicap;
                case BetTypeConstants.OverUnder:
                    return Labels.TurnoverMonitor_BalanceFormat_OverUnder;
                case BetTypeConstants._1x2:
                    return Labels.TurnoverMonitor_BalanceFormat_1X2;
                case BetTypeConstants._1HHandicap:
                    return Labels.TurnoverMonitor_BalanceFormat_1HHandicap;
                case BetTypeConstants._1HOverUnder:
                    return Labels.TurnoverMonitor_BalanceFormat_1HOverUnder;
                case BetTypeConstants._1H1x2:
                    return Labels.TurnoverMonitor_BalanceFormat_1H1X2;
                default:
                    return Labels.TurnoverMonitor_BalanceFormat_Others;
            }
        }
    }
}
