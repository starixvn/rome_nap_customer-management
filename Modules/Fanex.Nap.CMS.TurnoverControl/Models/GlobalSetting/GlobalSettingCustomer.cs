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
            : this(0,
                   username,
                   isGroupHeadline ? "headline" : string.Empty,
                   false,
                   string.Empty,
                   maxTurnovers)
        {
        }

        public GlobalSettingCustomer(
            int custId,
            string username,
            string level,
            bool isLock,
            string currency,
            IEnumerable<MaxTurnoverItem> maxTurnovers)
        {
            CustId = custId;
            Username = username;
            Level = level;
            IsLock = isLock;
            Currency = currency;
            MaxTurnovers = maxTurnovers;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Level { get; }

        public bool IsLock { get; }

        public string Currency { get; }

        public IEnumerable<MaxTurnoverItem> MaxTurnovers { get; }

        public bool IsGroupHeadline
            => !string.IsNullOrEmpty(Level) && Level.Equals("headline", StringComparison.OrdinalIgnoreCase);
    }
}
