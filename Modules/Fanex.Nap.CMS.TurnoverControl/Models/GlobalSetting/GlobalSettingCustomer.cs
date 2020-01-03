using System;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    [Serializable]
    public class GlobalSettingCustomer
    {
        public GlobalSettingCustomer(
            string username,
            bool isGroupHeadline,
            IEnumerable<MaxTurnoverItem> maxTurnovers)
            : this(0, username, isGroupHeadline ? "headline" : string.Empty, string.Empty, maxTurnovers)
        {
        }

        public GlobalSettingCustomer(
            int custId,
            string username,
            string level,
            string currency,
            IEnumerable<MaxTurnoverItem> maxTurnovers)
        {
            CustId = custId;
            Username = username;
            Level = level;
            Currency = currency;
            MaxTurnovers = maxTurnovers;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Level { get; }

        public string Currency { get; }

        public IEnumerable<MaxTurnoverItem> MaxTurnovers { get; }

        public bool IsGroupHeadline
            => Level.Equals("headline", StringComparison.OrdinalIgnoreCase);
    }
}
